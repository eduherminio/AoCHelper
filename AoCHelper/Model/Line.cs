using System;
using AoCHelper.Extensions;

namespace AoCHelper.Model
{
    /// <summary>
    /// Simple straight line class, with equality operators overriden
    /// </summary>
    public class Line : IEquatable<Line>
    {
        public double M { get; set; }

        public double Y0 { get; set; }

        public double X0 { get; set; }

        public Line(Point a, Point b)
        {
            M = (double)(b.Y - a.Y) / (b.X - a.X);
            X0 = a.X;
            Y0 = a.Y;
        }

        public double CalculateY(double x)
        {
            return Y0 + M * (x - X0);
        }

        public override string ToString()
        {
            return $"[y = {Y0} + ({M}) * (X - {X0}]";
        }

        #region Equals override
        // https://docs.microsoft.com/en-us/visualstudio/code-quality/ca1815-override-equals-and-operator-equals-on-value-types?view=vs-2017

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = M.GetHashCode();
                hashCode = (hashCode * 397) ^ Y0.GetHashCode();
                hashCode = (hashCode * 397) ^ X0.GetHashCode();
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Line))
            {
                return false;
            }

            return Equals((Line)obj);
        }

        /// <summary>
        /// Check if both have the same m, and then check if other goes through X0 and Y0
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Line other)
        {
            if (other == null)
            {
                return false;
            }

            return double.IsInfinity(M)
                ? double.IsInfinity(other.M)
                    && X0.DoubleEquals(other.X0)
                : M.DoubleEquals(other.M)
                    && Y0.DoubleEquals(other.CalculateY(X0));
        }

        public static bool operator ==(Line line1, Line line2)
        {
            if (line1 is null)
            {
                return line2 is null;
            }

            return line1.Equals(line2);
        }

        public static bool operator !=(Line line1, Line line2)
        {
            if (line1 is null)
            {
                return line2 is object;
            }

            return !line1.Equals(line2);
        }
        #endregion
    }
}
