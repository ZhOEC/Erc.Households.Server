using Erc.Households.Commands;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.CommandHandlers
{
    public class UpdateBranchOfficeCommandHandler : IRequestHandler<UpdateBranchOfficeCommand, Unit>
    {
        private readonly ErcContext _ercContext;

        public UpdateBranchOfficeCommandHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task<Unit> Handle(UpdateBranchOfficeCommand request, CancellationToken cancellationToken)
        {
            var branchOffice = await _ercContext.BranchOffices.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (branchOffice is null)
                throw new Exception("Branch Office not exist");

            _ercContext.Entry(branchOffice).CurrentValues.SetValues(
                new
                {
                    request.Name,
                    request.Address,
                    request.Iban,
                    request.BankFullName,
                    request.ChiefName,
                    request.BookkeeperName

                });
            return await _ercContext.SaveChangesAsync() > 0 ? Unit.Value : throw new Exception("Can't update branch office");
        }
    }
}
