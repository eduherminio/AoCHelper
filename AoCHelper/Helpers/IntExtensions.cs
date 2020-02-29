namespace AoCHelper.Extensions
{
    public static class IntExtensions
    {
        public static int Factorial(this int n)
        {
            return n > 0
                ? n * Factorial(n - 1)
                : 1;
        }
    }
}
