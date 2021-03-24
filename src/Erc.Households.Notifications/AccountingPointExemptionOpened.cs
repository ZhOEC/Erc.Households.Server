using Erc.Households.Domain.Shared;
using MediatR;
using System;

namespace Erc.Households.NotificationHandlers
{
    public class AccountingPointExemptionOpened : INotification
    {
        public int Id { get; }
        public int AccountingPointId { get; }
        public int CategoryId { get; }
        public DateTime Date { get; }
        public string Certificate { get; }
        public int PersonCount { get; }
        public bool Limit { get; }
        public string Note { get; }
        public Person Person { get; }

        public AccountingPointExemptionOpened(int id, int accountingPointId, int categoryId, DateTime date, string certificate, int personCount, bool limit, string note, Person person)
        {
            Id = id;
            AccountingPointId = accountingPointId;
            CategoryId = categoryId;
            Date = date;
            Certificate = certificate;
            PersonCount = personCount;
            Limit = limit;
            Note = note;
            Person = person;
        }
    }
}