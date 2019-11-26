using System;
using System.Collections.Generic;
using System.Linq;

namespace AoCHelper.Model
{
    /// <summary>
    /// Simple point class, with equals method and equality operators overriden
    /// </summary>
    internal class Point : IEquatable<Point>
    {
        internal int X { get; set; }

        internal int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int ManhattanDistance(Point point)
        {
            return Math.Abs(point.X - X) + Math.Abs(point.Y - Y);
        }

        public Point CalculateClosestManhattanPoint(ICollection<Point> candidatePoints)
        {
            Dictionary<Point, int> pointDistanceDictionary = new Dictionary<Point, int>();

            foreach (Point point in candidatePoints)
            {
                pointDistanceDictionary.Add(point, ManhattanDistance(point));
            }

            return pointDistanceDictionary.OrderBy(pair => pair.Value)
                .First().Key;
        }

        /// <summary>
        /// Returns null of there are multiple points at min Manhattan distance
        /// </summary>
        /// <param name="candidatePoints"></param>
        /// <returns></returns>
        public Point CalculateClosestManhattanPointNotTied(ICollection<Point> candidatePoints)
        {
            Dictionary<Point, int> pointDistanceDictionary = new Dictionary<Point, int>();

            foreach (Point point in candidatePoints)
            {
                pointDistanceDictionary.Add(point, ManhattanDistance(point));
            }

            var orderedDictionary = pointDistanceDictionary.OrderBy(pair => pair.Value);

            return pointDistanceDictionary.Values.Count(distance => distance == orderedDictionary.First().Value) == 1
                ? orderedDictionary.First().Key
                : null;
        }

        static public IEnumerable<Point> GeneratePointRangeIteratingOverYFirst(IEnumerable<int> xRange, IEnumerable<int> yRange)
        {
            foreach (int x in xRange)
            {
                foreach (int y in yRange)
                {
                    yield return new Point(x, y);
                }
            }
        }

        static public IEnumerable<Point> GeneratePointRangeIteratingOverXFirst(IEnumerable<int> xRange, IEnumerable<int> yRange)
        {
            foreach (int y in yRange)
            {
                foreach (int x in xRange)
                {
                    yield return new Point(x, y);
                }
            }
        }

        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }

        #region Equals override
        // https://docs.microsoft.com/en-us/visualstudio/code-quality/ca1815-override-equals-and-operator-equals-on-value-types?view=vs-2017

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Point))
            {
                return false;
            }

            return Equals((Point)obj);
        }

        public bool Equals(Point other)
        {
            if (other == null)
            {
                return false;
            }

            return X == other.X && Y == other.Y;
        }

        public static bool operator ==(Point point1, Point point2)
        {
            if (point1 is null)
            {
                return point2 is null;
            }

            return point1.Equals(point2);
        }

        public static bool operator !=(Point point1, Point point2)
        {
            if (point1 is null)
            {
                return point2 is object;
            }

            return !point1.Equals(point2);
        }
        #endregion
    }
}
