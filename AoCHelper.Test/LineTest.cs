using AoCHelper.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AoCHelper.Test
{
    public class LineTest
    {
        [Fact]
        public void Equal()
        {
            Point a = new Point(0, 0);
            Point b = new Point(1, 1);
            Point c = new Point(2, 2);
            Point d = new Point(-1, 0);

            Line line1 = new Line(a, b);
            Line line2 = new Line(a, c);
            Line line3 = new Line(b, c);
            Line line4 = new Line(a, d);

            Assert.Equal(line1, line2);
            Assert.Equal(line1, line3);
            Assert.Equal(line2, line3);

            Assert.NotEqual(line1, line4);
            Assert.NotEqual(line2, line4);
            Assert.NotEqual(line3, line4);

            HashSet<Line> set = new HashSet<Line>() { line1 };
            Assert.False(set.Add(line2));
            Assert.True(set.Add(line4));
        }

        [Fact]
        public void Equal_InfiniteM()
        {
            Point a = new Point(0, 0);
            Point b = new Point(0, 1);
            Point c = new Point(0, 2);
            Point d = new Point(-1, 0);
            Point e = new Point(-1, -1);

            Line line1 = new Line(a, b);
            Line line2 = new Line(a, c);
            Line line3 = new Line(b, c);
            Line line4 = new Line(a, d);
            Line line5 = new Line(d, e);

            Assert.Equal(line1, line2);
            Assert.Equal(line1, line3);
            Assert.Equal(line2, line3);

            Assert.NotEqual(line1, line4);
            Assert.NotEqual(line2, line4);
            Assert.NotEqual(line3, line4);
            Assert.NotEqual(line1, line5);

            HashSet<Line> set = new HashSet<Line>() { line1 };
            Assert.False(set.Add(line2));
            Assert.True(set.Add(line4));
            Assert.True(set.Add(line5));
        }

        [Fact]
        public void Equal_0M()
        {
            Point a = new Point(3, 2);
            Point b = new Point(-3, 2);
            Point c = new Point(5, 2);
            Point d = new Point(3, 3);
            Point e = new Point(-3, 3);

            Line line1 = new Line(a, b);
            Line line2 = new Line(a, c);
            Line line3 = new Line(b, c);
            Line line4 = new Line(a, d);
            Line line5 = new Line(d, e);

            Assert.Equal(line1, line2);
            Assert.Equal(line1, line3);
            Assert.Equal(line2, line3);

            Assert.NotEqual(line1, line4);
            Assert.NotEqual(line2, line4);
            Assert.NotEqual(line3, line4);
            Assert.NotEqual(line1, line5);

            HashSet<Line> set = new HashSet<Line>() { line1 };
            Assert.False(set.Add(line2));
            Assert.True(set.Add(line4));
            Assert.True(set.Add(line5));
        }
    }
}
