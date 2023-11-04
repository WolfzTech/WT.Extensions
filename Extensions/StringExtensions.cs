namespace System
{
    public static class StringExtension
    {
        public static bool Contains(this string initialString, string value)
        {
            return initialString.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
