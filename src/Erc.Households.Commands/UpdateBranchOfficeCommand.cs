using MediatR;

namespace Erc.Households.Commands
{
    public class UpdateBranchOfficeCommand : IRequest<Unit>
    {
        public UpdateBranchOfficeCommand(int id, string name, string address, string iban, string bankFullName, string chiefName, string bookkeeperName)
        {
            Id = id;
            Name = name;
            Address = address;
            Iban = iban;
            BankFullName = bankFullName;
            ChiefName = chiefName;
            BookkeeperName = bookkeeperName;
        }


        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Iban { get; private set; }
        public string BankFullName { get; private set; }
        public string ChiefName { get; private set; }
        public string BookkeeperName { get; private set; }
    }
}
