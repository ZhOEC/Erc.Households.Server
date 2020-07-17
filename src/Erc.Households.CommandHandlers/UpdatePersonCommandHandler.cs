using Erc.Households.Commands;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.CommandHandlers
{
    class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, Unit>
    {
        private readonly ErcContext _ercContext;

        public UpdatePersonCommandHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task<Unit> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            var person = await _ercContext.People.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (person is null)
                throw new Exception("Person not exist");

            _ercContext.Entry(person).CurrentValues.SetValues(new { request.Id, request.IdCardNumber, request.IdCardIssuanceDate, request.IdCardIssuer,
                                                                    request.IdCardExpDate, request.MobilePhones, request.Email });
            return await _ercContext.SaveChangesAsync() > 0 ? Unit.Value : throw new Exception("Can't update person");
        }
    }
}
