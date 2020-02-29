using AoCHelper.Extensions;
using System;
using Xunit;

namespace AoCHelper.Test
{
    public class DoubleExtensionsTest
    {
        [Fact]
        public void DoubleEquals()
        {
            const double piApprox1 = 355.0 / 113;   // error of 2.67e-7
            const double piApprox2 = 22.0 / 7;      // error of 1.13e-3

            Assert.True(piApprox1.DoubleEquals(Math.PI));
            Assert.False(piApprox1.DoubleEquals(Math.PI, precision: 2e-7));
            Assert.False(piApprox1.DoubleEquals(Math.PI, precision: 1e-8));

            Assert.False(piApprox2.DoubleEquals(Math.PI));
            Assert.True(piApprox2.DoubleEquals(Math.PI, precision: 1e-2));
        }
    }
}
