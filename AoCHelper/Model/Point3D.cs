using System;
using System.Collections.Generic;

namespace AoCHelper.Model
{
    /// <summary>
    /// Simple 3D Point class, with equality operators overriden
    /// </summary>
    public class Point3D : IEquatable<Point3D>
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        public string Id { get; set; }

        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point3D(string id, int x, int y, int z)
            : this(x, y, z)
        {
            Id = id;
        }

        public double DistanceTo(Point3D otherPoint)
        {
            return Math.Sqrt(
                Math.Pow(otherPoint.X - X, 2)
                + Math.Pow(otherPoint.Y - Y, 2)
                + Math.Pow(otherPoint.Z - Z, 2));
        }

        public override string ToString()
        {
            return $"[{X}, {Y}, {Z}]";
        }

        #region Equals override
        // https://docs.microsoft.com/en-us/visualstudio/code-quality/ca1815-override-equals-and-operator-equals-on-value-types?view=vs-2017

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Point3D))
            {
                return false;
            }

            return Equals((Point3D)obj);
        }

        public bool Equals(Point3D other)
        {
            if (other == null)
            {
                return false;
            }

            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public static bool operator ==(Point3D Point3D1, Point3D Point3D2)
        {
            if (Point3D1 is null)
            {
                return Point3D2 is null;
            }

            return Point3D1.Equals(Point3D2);
        }

        public static bool operator !=(Point3D Point3D1, Point3D Point3D2)
        {
            if (Point3D1 is null)
            {
                return Point3D2 is object;
            }

            return !Point3D1.Equals(Point3D2);
        }

        public override int GetHashCode()
        {
#if NESTANDARD2_1
                    return HashCode.Combine(X, Y, Z, Id);
#else
            var hashCode = -1895077416;

            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);

            return hashCode;
#endif
        }

        #endregion
    }
}
