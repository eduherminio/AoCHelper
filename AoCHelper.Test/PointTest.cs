using AoCHelper.Extensions;
using AoCHelper.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace AoCHelper.Test
{
    public class PointTest
    {
        [Fact]
        public void Equal()
        {
            Point a = new Point(0, 0);
            Point b = new Point(0, 0);
            Point c = new Point(0, 1);

            Assert.Equal(a, b);
            Assert.NotEqual(a, c);

            HashSet<Point> set = new HashSet<Point>() { a };
            Assert.False(set.Add(b));
            Assert.True(set.Add(c));
        }

        [Fact]
        public void DistanceTo()
        {
            Point a = new Point(0, 0);
            Point b = new Point(1, 1);

            var distance = a.DistanceTo(b);

            Assert.True(distance.DoubleEquals(Math.Sqrt(2)));
        }

        [Fact]
        public void ManhattanDistance()
        {
            Point a = new Point(0, 0);
            Point b = new Point(0, 1);
            Point c = new Point(1, 1);

            var distanceAB = a.ManhattanDistance(b);
            Assert.Equal(1, distanceAB);

            var distanceAC = a.ManhattanDistance(c);
            Assert.Equal(2, distanceAC);
        }
    }
}
