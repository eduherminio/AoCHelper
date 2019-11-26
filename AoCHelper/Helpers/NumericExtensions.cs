using System;

namespace AoCHelper.Helpers
{
    public static class NumericExtensions
    {
        /// <summary>
        /// Makes T fit within [min, max] range
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static T Clamp<T>(this T value, T min, T max)
            where T : struct,
                      IComparable,
                      IComparable<T>,
                      IConvertible,
                      IEquatable<T>,
                      IFormattable
        {
            return (value.CompareTo(min) <= 0)
                ? min
                : (value.CompareTo(max) >= 0)
                    ? max
                    : value;
        }
    }
}
