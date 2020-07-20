namespace Erc.Households.Api.Responses
{
    public class Address
    {
        public int CityId { get; set; }
        public int StreetId { get; set; }
        public string Building { get; set; }
        public string Apt { get; set; }
        public string Zip { get; set; }
    }
}
