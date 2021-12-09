using AoCHelper;
using AoCHelper.PoC;
using NUnit.Framework;

namespace AoC_2021.Test;

public class SampleTests
{
    [TestCase(typeof(Day01), "Solution 1.1", "Solution 1.2")]
    [TestCase(typeof(Day_02), "Solution 2.1", "Solution 2.2")]
    [TestCase(typeof(Problem03), "Solution 3.1", "Solution 3.2")]
    [TestCase(typeof(Problem04), "Solution 4.1", "Solution 4.2")]
    [TestCase(typeof(RandomProblem), "Solution Random.1", "Solution Random.2")]
    public async Task Test(Type type, string sol1, string sol2)
    {
        // Can't use BaseDay since some of them aren't days, but you probably can
        if (Activator.CreateInstance(type) is BaseProblem instance)
        {
            Assert.AreEqual(sol1, await instance.Solve_1());
            Assert.AreEqual(sol2, await instance.Solve_2());
        }
        else
        {
            Assert.Fail($"{type} is not a BaseProblem");
        }
    }
}
