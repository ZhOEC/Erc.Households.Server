using MediatR;
using System;

namespace Erc.Households.Commands
{
    public class UpdatePersonCommand : IRequest<Unit>
    {
        public UpdatePersonCommand(int id, string idCardNumber, DateTime idCardIssuanceDate, string idCardIssuer, DateTime? idCardExpDate, string[] mobilePhones, string email)
        {
            Id = id;
            IdCardNumber = idCardNumber;
            IdCardIssuanceDate = idCardIssuanceDate;
            IdCardIssuer = idCardIssuer;
            IdCardExpDate = idCardExpDate;
            MobilePhones = mobilePhones;
            Email = email;
        }

        public int Id { get; private set; }
        public string IdCardNumber { get; private set; }
        public DateTime IdCardIssuanceDate { get; private set; }
        public string IdCardIssuer { get; private set; }
        public DateTime? IdCardExpDate { get; private set; }
        public string[] MobilePhones { get; private set; }
        public string Email { get; private set; }
    }
}
