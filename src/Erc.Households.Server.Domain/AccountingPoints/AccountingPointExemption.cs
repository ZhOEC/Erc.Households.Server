using Erc.Households.Domain.AccountingPoints;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Erc.Households.Domain.AccountingPoints
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

        public void Close(DateTime date, string note)
        {
            EndDate = date >= EffectiveDate ? date : throw new ArgumentOutOfRangeException(nameof(date), "Дата закртиття пільги не може бути меньшою за дату відкртиття");
            if (!string.IsNullOrEmpty(note) && !string.IsNullOrWhiteSpace(note))
                Note += $"\nЗакртиття: {note}";
        }
    }
}
