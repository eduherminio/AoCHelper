using AoCHelper.Extensions;
using System.Collections.Generic;
using Xunit;

namespace AoCHelper.Test
{
    public class IntExtensionsTest
    {
        [Fact]
        public void Factorial()
        {
            var testCases = new Dictionary<int, int>()
            {
                [0] = 1,
                [1] = 1,
                [2] = 2,
                [3] = 6,
                [4] = 24,
                [5] = 120
            };

            foreach (var pair in testCases)
            {
                Assert.Equal(pair.Value, pair.Key.Factorial());
            }
        }
    }
}
