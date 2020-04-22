namespace Erc.Households.Server.Domain
{
    public class PaymentChannel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RecordpointFieldName { get; set; }
        public string SumFieldName { get; set; }
        public string DateFieldName { get; set; }
        public string TextDateFormat { get; set; }
        public string PersonFieldName { get; set; }
        public string TotalRecord { get; set; }
        public int Type { get; set; }
    }
}
