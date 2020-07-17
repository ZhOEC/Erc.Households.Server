using MediatR;
using System;

namespace Erc.Households.Commands
{
    public class OpenNewContractCommand : IRequest<Unit>
    {
        public OpenNewContractCommand(int accountingPointId, int personId, DateTime contractStartDate, bool sendPaperBill, string idCardNumber, DateTime idCardIssuanceDate, string idCardIssuer, DateTime? idCardExpDate,
            string taxCode, string firstName, string lastName, string patronymic, string[] mobilePhones, string email, string currentUser)
        {
            AccountingPointId = accountingPointId;
            PersonId = personId;
            ContractStartDate = contractStartDate;
            SendPaperBill = sendPaperBill;
            IdCardNumber = idCardNumber;
            IdCardIssuanceDate = idCardIssuanceDate;
            IdCardIssuer = idCardIssuer;
            IdCardExpDate = idCardExpDate;
            TaxCode = taxCode;
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
            MobilePhones = mobilePhones;
            Email = email;
            CurrentUser = currentUser;
        }

        public int AccountingPointId { get; set; }
        public int PersonId { get; set; }
        public DateTime ContractStartDate { get; private set; } 
        public bool SendPaperBill { get; private set; }
        public string IdCardNumber { get; private set; }
        public DateTime IdCardIssuanceDate { get; private set; }
        public string IdCardIssuer { get; private set; }
        public DateTime? IdCardExpDate { get; private set; }
        public string TaxCode { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Patronymic { get; private set; }
        public string[] MobilePhones { get; private set; }
        public string Email { get; private set; }
        public string CurrentUser { get; private set; }
    }
}
