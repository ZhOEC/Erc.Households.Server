using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.Api.Responses
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Domain.Billing.Usage UsageT1 { get; set; }
        public Domain.Billing.Usage UsageT2 { get; set; }
        public Domain.Billing.Usage UsageT3 { get; set; }
        public int TotalUnits { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalAmountDue { get; set; }
        public decimal TotalCharge { get; set; }
        public decimal IncomingBalance { get; set; }
        public string CounterSerialNumber { get; set; }
        public bool IsPaid { get; set; }
        public string Note { get; set; }
        public string PeriodName { get; set; }
        public string Type { get; set; }
    }

    //public class Usage
    //{
    //    public int PresentMeterReading { get; set; }
    //    public int PreviousMeterReading { get; set; }
    //    public int Units { get; set; }
    //    public decimal Charge { get; set; }
    //    public decimal Discount { get; set; }
    //    public decimal DiscountUnits { get; set; }
    //    public decimal Kz { get; set; }
    //    public decimal DiscountWeight { get; set; }
    //    public IEnumerable<Domain.Billing.UsageCalculation> Calculations { get; set; }
    //}

    //public class UsageCalculation
    //{
    //    public decimal PriceValue { get; set; }
    //    public int Units { get; set; }
    //    public decimal Charge { get; set; }
    //}
}
