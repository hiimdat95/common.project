namespace System
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);

        public static string EnsureEndsWithDot(this string value) => value.EndsWith(".") ? value : $"{value}.";
    }
}