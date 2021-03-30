namespace Erc.Households.PrintBills.Api.Helpers
{
    public static class StringConvertor
    {
        public static string To36(this int number)
        {
            const string digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const int systemBase = 36;

            if (number < 0)
            {
                return string.Empty;
            }

            if (number < 36)
            {
                return digits[number].ToString();
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            while (number != 0)
            {
                sb.Insert(0, (digits[number % systemBase]));
                number /= systemBase;
            }

            return sb.ToString();
        }
    }
}
