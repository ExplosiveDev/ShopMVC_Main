namespace ShopMVC.Helper
{
    public static class NumbersFormatting
    {
        public static string FormatNumberWithCommas(decimal number)
        {
            return number.ToString("N");
        }
    }
}
