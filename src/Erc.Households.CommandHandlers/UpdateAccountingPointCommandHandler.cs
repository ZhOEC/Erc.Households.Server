using Erc.Households.Commands;
using Erc.Households.Domain.Shared.Addresses;
using Erc.Households.EF.PostgreSQL;
using Erc.Households.Events.AccountingPoints;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.CommandHandlers
{
    public class UpdateAccountingPointCommandHandler : IRequestHandler<UpdateAccountingPointCommand, Unit>
    {
        private readonly ErcContext _ercContext;

        public UpdateAccountingPointCommandHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task<Unit> Handle(UpdateAccountingPointCommand request, CancellationToken cancellationToken)
        {
            var ap = await _ercContext.AccountingPoints.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (ap is null)
                throw new Exception("Accounting point not exist");

            var address = new Address
            {
                Id =
                    (await _ercContext.Addresses
                    .Where(a => a.StreetId == request.Address.StreetId && a.Building == request.Address.Building && ((a.Apt ?? string.Empty) == (request.Address.Apt ?? string.Empty)))
                    .Select(a => (int?)a.Id)
                    .FirstOrDefaultAsync()) ?? 0,
                StreetId = request.Address.StreetId,
                Building = request.Address.Building,
                Apt = request.Address.Apt,
                Zip = request.Address.Zip
            };

            ap.SetNewAddress(address);
            _ercContext.Entry(ap).CurrentValues.SetValues(request);

            ap.Events.Add(new AccountingPointUpdated
            {
                StreetAddress = ap.Address.StreetLocation,
                CityName = ap.Address.CityName,
                Eic = ap.Eic,
                Name = ap.Name,
                PersonFirstName = ap.Owner.FirstName,
                PersonIdCardNumber = ap.Owner.IdCardNumber,
                PersonLastName = ap.Owner.LastName,
                PersonPatronymic = ap.Owner.Patronymic,
                PersonTaxCode = ap.Owner.TaxCode,
                BranchOfficeStringId = ap.BranchOffice.StringId,
                BranchOfficeName = ap.BranchOffice.Name
            });

            return Unit.Value;
        }
    }
}
