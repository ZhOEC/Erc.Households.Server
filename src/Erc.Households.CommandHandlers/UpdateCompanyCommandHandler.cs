using Erc.Households.Commands;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.CommandHandlers
{
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, Unit>
    {
        private readonly ErcContext _ercContext;

        public UpdateCompanyCommandHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task<Unit> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _ercContext.Company.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (company is null)
                throw new Exception("Company not exist");

            _ercContext.Entry(company).CurrentValues.SetValues(
                new
                {
                    request.Name,
                    request.ShortName,
                    request.DirectorName,
                    request.Address,
                    request.Email,
                    request.Www,
                    request.TaxpayerPhone,
                    request.StateRegistryCode,
                    request.TaxpayerNumber,
                    request.BookkeeperName,
                    request.BookkeeperTaxNumber
                });
            return await _ercContext.SaveChangesAsync() > 0 ? Unit.Value : throw new Exception("Can't update company");
        }
    }
}
