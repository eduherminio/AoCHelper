using System;

namespace AoCHelper.Extensions
{
    public static class DoubleExtensions
    {
        public const double Delta = 1e-6;

        /// <summary>
        /// Compares two double values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="precision">Default value: <see cref="Delta"/></param>
        /// <returns></returns>
        public static bool DoubleEquals(this double left, double right, double precision = Delta)
        {
            return Math.Abs(left - right) < precision;
        }
    }
}
