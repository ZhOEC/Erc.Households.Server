using MediatR;
using System;

namespace Erc.Households.Commands
{
    public class UpdateCompanyCommand : IRequest<Unit>
    {
        public UpdateCompanyCommand(int id, string name, string shortName, string directorName, string address, string email, string www, string taxpayerPhone, string stateRegistryCode,
            string taxpayerNumber, string bookkeeperName, string bookkeeperTaxNumber)
        {
            Id = id;
            Name = name;
            ShortName = shortName;
            DirectorName = directorName;
            Address = address;
            Email = email;
            Www = www;
            TaxpayerPhone = taxpayerPhone;
            StateRegistryCode = stateRegistryCode;
            TaxpayerNumber = taxpayerNumber;
            BookkeeperName = bookkeeperName;
            BookkeeperTaxNumber = bookkeeperTaxNumber;
        }


        public int Id { get; private set; }
        public string Name { get; private set; }
        public string ShortName { get; private set; }
        public string DirectorName { get; private set; }
        public string Address { get; private set; }
        public string Email { get; private set; }
        public string Www { get; private set; }
        public string TaxpayerPhone { get; private set; }
        public string StateRegistryCode { get; private set; }
        public string TaxpayerNumber { get; private set; }
        public string BookkeeperName { get; private set; }
        public string BookkeeperTaxNumber { get; private set; }
    }
}
