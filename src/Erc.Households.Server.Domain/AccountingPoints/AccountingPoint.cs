using Erc.Households.Domain.Billing;
using Erc.Households.Domain.Extensions;
using Erc.Households.Domain.Payments;
using Erc.Households.Domain.Shared;
using Erc.Households.Domain.Shared.AccountingPoints;
using Erc.Households.Domain.Shared.Addresses;
using Erc.Households.Domain.Shared.Tariffs;
using Erc.Households.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Erc.Households.Domain.AccountingPoints
{
    public class AccountingPoint : IEntity
    {
        readonly List<Contract> _contractsHistory = new List<Contract>();
        private readonly List<AccountingPointTariff> _tariffsHistory = new List<AccountingPointTariff>();

        Person _owner;
        Address _address;
        DistributionSystemOperator _distributionSystemOperator;

        public ICollection<IEntityEvent> Events { get; } = new List<IEntityEvent>();

        private Action<object, string> LazyLoader { get; set; }

        protected AccountingPoint(Action<object, string> lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public AccountingPoint(string eic, string name, Commodity commodity,  DateTime contractStartDate, int tariffId, Address address,
                               Person owner, int branchOfficeId, int dsoId, string currentUser, int buildingTypeId, int usageCategoryId, bool sendPaperBill)
        {
            Eic = eic;
            Name = name;
            Address = address;
            Owner = owner;
            BranchOfficeId = branchOfficeId;
            DistributionSystemOperatorId = dsoId;
            OpenNewContract(contractStartDate, Owner, currentUser, sendPaperBill);
            SetTariff(tariffId, contractStartDate, currentUser);
            BuildingTypeId = buildingTypeId;
            UsageCategoryId = usageCategoryId;
            Commodity = commodity;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Eic { get; private set; }
        public int AddressId { get; private set; }
        public int OwnerId { get; private set; }
        public int DistributionSystemOperatorId { get; private set; }
        public int BranchOfficeId { get; private set; }
        public decimal Debt { get; private set; }
        public ZoneRecord ZoneRecord { get; private set; }
        public Commodity Commodity { get; private set; }
        public bool? IsGasWaterHeaterInstalled { get; init; }
        public bool? IsCentralizedWaterSupply { get; init; }
        public bool? IsCentralizedHotWaterSupply { get; init; }
        public DistributionSystemOperator DistributionSystemOperator
        {
            get => LazyLoader.Load(this, ref _distributionSystemOperator);
            private set { _distributionSystemOperator = value; }
        }
        public Contract CurrentContract => _contractsHistory.OrderByDescending(c => c.StartDate).ThenByDescending(c => c.Id).FirstOrDefault();
        public Tariff CurrentTariff => _tariffsHistory.FirstOrDefault(t => t.StartDate <= DateTime.Today).Tariff;
        public int UsageCategoryId { get; private set; }
        public int BuildingTypeId { get; private set; }
        public AccountingPointExemption Exemption => Exemptions.FirstOrDefault(t => t.EffectiveDate <= DateTime.Today && (t.EndDate ?? DateTime.MaxValue) > DateTime.Today);
        public BuildingType BuildingType { get; private set; }
        public UsageCategory UsageCategory { get; private set; }

        public bool CanBeUsedElectricWaterHeater => UsageCategoryId > 1 || (IsCentralizedWaterSupply ?? false);
        public bool IsElectricHeatig => UsageCategory.Id > 2;

        private List<Invoice> _invoices = new();
        public IReadOnlyCollection<Invoice> Invoices
        {
            get => LazyLoader.Load(this, ref _invoices);
            private set { _invoices = value.ToList(); }
        }
        
        private List<AccountingPointExemption> _exemptions = new();
        public IReadOnlyCollection<AccountingPointExemption> Exemptions
        {
            get => LazyLoader.Load(this, ref _exemptions);
            private set { _exemptions = value.ToList(); }
        }

        private List<Payment> _payments = new();
        public IReadOnlyCollection<Payment> Payments
        {
            get => LazyLoader.Load(this, ref _payments);
            private set { Payments = value.ToList(); }
        }

        public Address Address
        {
            get => LazyLoader.Load(this, ref _address);
            private set { _address = value; }
        }
    
        public Person Owner
        {
            get => LazyLoader.Load(this, ref _owner);
            private set { _owner = value; }
        }

        private BranchOffice _branchOffice;
        public BranchOffice BranchOffice
        {
            get => LazyLoader.Load(this, ref _branchOffice);
            private set { _branchOffice = value; }
        }
        
        public bool ContractIsSigned { get; private set; }

        public IReadOnlyCollection<Contract> ContractsHistory => _contractsHistory.AsReadOnly();

        public IReadOnlyCollection<AccountingPointTariff> TariffsHistory => _tariffsHistory.AsReadOnly();

        public void CloseCurrentContract(DateTime closeDate, string currentUser)
        {
            var currentContract = _contractsHistory.FirstOrDefault(c => c.IsActive);
            if (currentContract != null)
            {
                currentContract.Close(closeDate, currentUser);
                ContractIsSigned = false;
            }
        }

        public void OpenNewContract(DateTime openDate, Person customer, string currentUser, bool sendPaperBill)
        {
            CloseCurrentContract(openDate, currentUser);
            _contractsHistory.Add(new Contract(openDate, customer, currentUser, sendPaperBill));
            ContractIsSigned = true;
        }

        public void SetTariff(int tariffId, DateTime date, string currentUser)
        {
            _tariffsHistory.Add(new AccountingPointTariff(tariffId, date, currentUser));
        }

        public void ProcessPayment(Payment payment, bool updateDebt = true)
        {
            if (payment.Amount > 0)
            {
                foreach (var invoice in Invoices.Where(i => !i.IsPaid).OrderBy(i => i.PeriodId).ThenBy(i => i.Id).ToList())
                {
                    var ipi = invoice.Pay(payment);
                    payment.AddInvoicePaymentItem(ipi);
                    if (payment.IsFullyUsed) break;
                }
            }
            else
            {
                foreach (var invoice in _invoices.Where(i => i.TotalPaid > 0).OrderByDescending(i => i.PeriodId).ToList())
                {
                    var ipi = invoice.TakePaymentBack(payment);
                    payment.AddInvoicePaymentItem(ipi);
                    if (payment.IsFullyUsed) break;
                }
            }
            
            if (updateDebt) Debt -= payment.Amount;
            
            _payments.Add(payment);
        }

        public void AddInvoice(Invoice invoice)
        {
            _invoices.Add(invoice);
            if (invoice.TotalAmountDue > 0 && Debt < 0)
            {
                foreach (var p in Payments.Where(p => !p.IsFullyUsed).ToArray())
                {
                    invoice.Pay(p);
                    if (invoice.IsPaid) break;
                }
            }
            Debt += invoice.TotalAmountDue;
        }

        public void SetNewAddress(Address address)
        {
            if (address.Id == 0) _address = address;
            else AddressId = address.Id;
        }

        public void SetExemption(AccountingPointExemption exemption)
        {
            if (!Exemptions.Any(x => x.EndDate <= exemption.EffectiveDate))
                _exemptions.Add(exemption);
            else throw new ArgumentOutOfRangeException(nameof(exemption.EffectiveDate), "Нова пільга не може бути відкрита у період дії іншої пільги.");
        }
    }
}
