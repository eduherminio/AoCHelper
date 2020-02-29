using AoCHelper.Extensions;
using AoCHelper.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace AoCHelper.Test
{
    public class Point3DTest
    {
        [Fact]
        public void Equal()
        {
            Point3D a = new Point3D(0, 0, 0);
            Point3D b = new Point3D(0, 0, 0);
            Point3D c = new Point3D(0, 1, 0);

            Assert.Equal(a, b);
            Assert.NotEqual(a, c);

            HashSet<Point3D> set = new HashSet<Point3D>() { a };
            Assert.False(set.Add(b));
            Assert.True(set.Add(c));
        }

        [Fact]
        public void DistanceTo()
        {
            Point3D a = new Point3D(0, 0, 0);
            Point3D b = new Point3D(0, 0, 1);
            Point3D c = new Point3D(0, 1, 1);
            Point3D d = new Point3D(1, 1, 1);

            var distanceAB = a.DistanceTo(b);
            Assert.True(distanceAB.DoubleEquals(1));

            var distanceAC = a.DistanceTo(c);
            Assert.True(distanceAC.DoubleEquals(Math.Sqrt(2)));

            var distanceAD = a.DistanceTo(d);
            Assert.True(distanceAD.DoubleEquals(Math.Sqrt(3)));
        }
    }
}
