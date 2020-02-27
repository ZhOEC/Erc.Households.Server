using Erc.Households.Server.Domain.Addresses;
using Erc.Households.Server.Domain.Extensions;
using Erc.Households.Server.Domain.Tariffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Erc.Households.Server.Domain.AccountingPoints
{
    public class AccountingPoint
    {
        readonly List<Contract> _contractsHistory = new List<Contract>();
        private List<AccountingPointTariff> _tariffsHistory = new List<AccountingPointTariff>();

        protected AccountingPoint()
        {
            
        }

        public AccountingPoint(string eic, string name, DateTime contractStartDate, int tariffId, Address address, Person owner, int branchOfficeId, int dsoId, string currentUser)
        {
            Eic = eic;
            Name = name;
            Address = address;
            Owner = owner;
            BranchOfficeId = branchOfficeId;
            DistributionSystemOperatorId = dsoId;
            OpenNewContract(contractStartDate, owner, currentUser);
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
        public DistributionSystemOperator Dso { get; private set; }
        public Tariff CurrentTariff => _tariffsHistory.FirstOrDefault(t => t.StartDate <= DateTime.Today).Tariff;
        public Address Address { get; private set; }
        public Person Owner { get; private set; }
        public BranchOffice BranchOffice { get; private set; }
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
