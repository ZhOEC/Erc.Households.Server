using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Server.Domain.Billing
{
    public class Period
    {
        public Period(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
            Name = char.ToUpper(startDate.ToString("Y")[0]) + startDate.ToString("Y").Substring(1);
        }

        public int Id { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public string Name { get; private set; }
    }
}
