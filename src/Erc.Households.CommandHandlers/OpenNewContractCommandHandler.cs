using Erc.Households.Commands;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.CommandHandlers
{
    class OpenNewContractCommandHandler : IRequestHandler<OpenNewContractCommand, Unit>
    {
        private readonly ErcContext _ercContext;

        public OpenNewContractCommandHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task<Unit> Handle(OpenNewContractCommand request, CancellationToken cancellationToken)
        {
            var ap = await _ercContext.AccountingPoints.FindAsync(request.AccountingPointId);
            if (ap is null)
                throw new Exception("Accounting point not exist");

            var person = await _ercContext.People.FindAsync(request.PersonId);
            if (person is null)
                throw new Exception("Person not exist");

            person.IdCardNumber = request.IdCardNumber;
            person.IdCardIssuanceDate = request.IdCardIssuanceDate;
            person.IdCardIssuer = request.IdCardIssuer;
            person.IdCardExpDate = request.IdCardExpDate;
            person.TaxCode = request.TaxCode;
            person.FirstName = request.FirstName;
            person.LastName = request.LastName;
            person.Patronymic = request.Patronymic;
            person.MobilePhones = request.MobilePhones;
            person.Email = request.Email;

            _ercContext.Entry(ap.Owner).State = person.Id == 0 ? EntityState.Added : EntityState.Modified;
            ap.OpenNewContract(request.ContractStartDate, person, request.CurrentUser, request.SendPaperBill);

            return Unit.Value;
        }
    }
}
