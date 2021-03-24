using MediatR;
using System;
using Erc.Households.Domain;
using Erc.Households.Domain.Shared;

namespace Erc.Households.Notifications
{
    public class OpenAccountingPointExemptionCommand : IRequest
    {
        public OpenAccountingPointExemptionCommand(int accountingPointId, int categoryId, DateTime date, string certificate, int personCount, bool limit, string note, Person person)
        {
            AccountingPointId = accountingPointId;
            CategoryId = categoryId;
            Date = date;
            Certificate = certificate;
            PersonCount = personCount;
            Limit = limit;
            Note = note;
            Person = person;
        }

        public int AccountingPointId { get; private set; }
        public int CategoryId { get; private set; }
        public DateTime Date { get; private set; }
        public string Certificate { get; private set; }
        public int PersonCount { get; private set; }
        public bool Limit { get; private set; }
        public string Note { get; private set; }
        public Person Person { get; private set; }
    }
}
