using AoCHelper.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace AoCHelper.Test
{
    public class NumericExtensionsTest
    {
        [Fact]
        public void ClampInt()
        {
            const int min = 3;
            const int max = 5;

            Dictionary<int, int> numberExpectedClampedValuePair = new Dictionary<int, int>()
            {
                [int.MinValue] = min,
                [0] = min,
                [1] = min,
                [3] = 3,
                [4] = 4,
                [5] = 5,
                [7] = max,
                [int.MaxValue] = max
            };

            foreach (var pair in numberExpectedClampedValuePair)
            {
                Assert.Equal(pair.Value, pair.Key.Clamp(min, max));
            }
        }

        [Fact]
        public void ClampShort()
        {
            const short min = 3;
            const short max = 5;

            Dictionary<short, short> numberExpectedClampedValuePair = new Dictionary<short, short>()
            {
                [short.MinValue] = min,
                [0] = min,
                [1] = min,
                [3] = 3,
                [4] = 4,
                [5] = 5,
                [7] = max,
                [short.MaxValue] = max
            };

            foreach (var pair in numberExpectedClampedValuePair)
            {
                Assert.Equal(pair.Value, pair.Key.Clamp(min, max));
            }
        }

        [Fact]
        public void ClampLong()
        {
            const long min = 3;
            const long max = 5;

            Dictionary<long, long> numberExpectedClampedValuePair = new Dictionary<long, long>()
            {
                [long.MinValue] = min,
                [0] = min,
                [1] = min,
                [3] = 3,
                [4] = 4,
                [5] = 5,
                [7] = max,
                [long.MaxValue] = max
            };

            foreach (var pair in numberExpectedClampedValuePair)
            {
                Assert.Equal(pair.Value, pair.Key.Clamp(min, max));
            }
        }

        [Fact]
        public void ClampFloat()
        {
            const float min = 3;
            const float max = 5;

            Dictionary<float, float> numberExpectedClampedValuePair = new Dictionary<float, float>()
            {
                [float.MinValue] = min,
                [0] = min,
                [1] = min,
                [3] = 3,
                [4] = 4,
                [5] = 5,
                [7] = max,
                [float.MaxValue] = max
            };

            foreach (var pair in numberExpectedClampedValuePair)
            {
                Assert.Equal(pair.Value, pair.Key.Clamp(min, max));
            }
        }

        [Fact]
        public void ClampDouble()
        {
            const double min = 3.01;
            const double max = 4.99;

            Dictionary<double, double> numberExpectedClampedValuePair = new Dictionary<double, double>()
            {
                [double.MinValue] = min,
                [0] = min,
                [1] = min,
                [3.02] = 3.02,
                [4] = 4,
                [4.98] = 4.98,
                [5] = max,
                [double.MaxValue] = max
            };

            foreach (var pair in numberExpectedClampedValuePair)
            {
                Assert.Equal(pair.Value, pair.Key.Clamp(min, max));
            }
        }

        [Fact]
        public void ClampDateTime()
        {
            DateTime min = new DateTime(2019, 10, 25);
            DateTime max = new DateTime(2019, 10, 30);

            Dictionary<DateTime, DateTime> numberExpectedClampedValuePair = new Dictionary<DateTime, DateTime>()
            {
                [DateTime.MinValue] = min,
                [new DateTime(2019, 10, 24, 23, 0, 0)] = min,
                [new DateTime(2019, 10, 25, 1, 0, 0)] = new DateTime(2019, 10, 25, 1, 0, 0),
                [new DateTime(2019, 10, 30, 0, 0, 1)] = max,
                [DateTime.MaxValue] = max
            };

            foreach (var pair in numberExpectedClampedValuePair)
            {
                Assert.Equal(pair.Value, pair.Key.Clamp(min, max));
            }
        }
    }
}
