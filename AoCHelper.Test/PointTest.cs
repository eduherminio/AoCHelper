using AoCHelper.Model;
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
    }
}
