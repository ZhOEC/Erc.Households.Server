using System;

namespace Erc.Households.Domain.Shared.AccountingPoints
{
    public class AccountingPointExemption
    {
        public int Id { get; private set; }
        public int AccountingPointId { get; private set; }
        public int ExemptionCategoryId { get; private set; }
        public int PersonId { get; private set; }
        public DateTime EffectiveDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public string Certificate { get; private set; }
        public int PersonsNumber { get; private set; }
        public bool HasLimit { get; private set; }
        public string Note { get; private set; }
        public Exemptions.ExemptionCategory Category { get; private set; }
        public Person Person { get; private set; }

        public void Open(int accountingPointId, int categoryId, DateTime date, string certificate, int personCount, bool limit, string note, Person person)
        {
            AccountingPointId = accountingPointId;
            ExemptionCategoryId = categoryId;
            EffectiveDate = date;
            Certificate = certificate;
            PersonsNumber = personCount;
            HasLimit = limit;
            Note = note;
            Person = person;
        }

        public void Close(DateTime date, string note)
        {
            EndDate = date >= EffectiveDate ? date : throw new ArgumentOutOfRangeException(nameof(date), "Дата закртиття пільги не може бути меньшою за дату відкртиття");
            if (!string.IsNullOrEmpty(note) && !string.IsNullOrWhiteSpace(note))
                Note += $"\nЗакртиття: {note}";
        }
    }
}
