using Erc.Households.Server.Domain.Addresses;
using Erc.Households.Server.Domain.Extensions;
using Erc.Households.Server.Domain.Tariffs;
using Erc.Households.Server.Events;
using Erc.Households.Server.Events.AccountingPoints;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Erc.Households.Server.Domain.AccountingPoints
{
    public class AccountingPoint : IEntity
    {
        readonly List<Contract> _contractsHistory = new List<Contract>();
        private List<AccountingPointTariff> _tariffsHistory = new List<AccountingPointTariff>();
        BranchOffice _branchOffice;
        Person _owner;
        Address _address;
        DistributionSystemOperator _distributionSystemOperator;

        public ICollection<IEvent> Events { get; } = new List<IEvent>();

        private Action<object, string> LazyLoader { get; set; }

        protected AccountingPoint(Action<object, string> lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public AccountingPoint(string eic, string name, DateTime contractStartDate, int tariffId, Address address, Person owner, int branchOfficeId, int dsoId, string currentUser)
        {
            Eic = eic;
            Name = name;
            Address = address;
            Owner = owner;
            BranchOfficeId = branchOfficeId;
            DistributionSystemOperatorId = dsoId;
            OpenNewContract(contractStartDate, Owner, currentUser);
            SetTariff(tariffId, contractStartDate, currentUser);
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Eic { get; private set; }
        public int AddressId { get; private set; }
        public int OwnerId { get; private set; }
        public int DistributionSystemOperatorId { get; private set; }
        public int BranchOfficeId { get; private set; }
        public decimal Debt { get; private set; }
        public DistributionSystemOperator DistributionSystemOperator
        {

            get => LazyLoader.Load(this, ref _distributionSystemOperator);
            private set { _distributionSystemOperator = value; }
        }
        public Contract CurrentContract => _contractsHistory.OrderByDescending(c => c.StartDate).ThenByDescending(c => c.Id).FirstOrDefault();
        public Tariff CurrentTariff => _tariffsHistory.FirstOrDefault(t => t.StartDate <= DateTime.Today).Tariff;
        
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

        public void OpenNewContract(DateTime openDate, Person customer, string currentUser)
        {
            CloseCurrentContract(openDate, currentUser);
            _contractsHistory.Add(new Contract(openDate, customer, currentUser));
            ContractIsSigned = true;
        }

        public void SetTariff(int tariffId, DateTime date, string currentUser)
        {
            _tariffsHistory.Add(new AccountingPointTariff(tariffId, date, currentUser));
        }
    }
}
