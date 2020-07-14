namespace WinlinkInterop
{
    public static class Extensions
    {
        public static bool IsNumeric(this string text)
        {
            double test;
            return double.TryParse(text, out test);
        }

    }
}
