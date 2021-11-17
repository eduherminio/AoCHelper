using Xunit;

namespace AoCHelper.Test
{
    [Collection("Sequential")]
    public class OverrideClassPrefixTests
    {
        private abstract class GreatName : BaseProblem
        {
            protected override string ClassPrefix => nameof(GreatName);
        }

        private class GreatName01 : GreatName { public override ValueTask<string> Solve_1() => new(""); public override ValueTask<string> Solve_2() => new(""); }
        private class GreatName_01 : GreatName { public override ValueTask<string> Solve_1() => new(""); public override ValueTask<string> Solve_2() => new(""); }

        [Fact]
        public async Task OverrideClassPrefix()
        {
            await Solver.Solve<GreatName01>();
            await Solver.Solve<GreatName_01>();
        }

        private class Day11 : BaseDay { public override ValueTask<string> Solve_1() => new(""); public override ValueTask<string> Solve_2() => new(""); }
        private class Day_11 : BaseDay { public override ValueTask<string> Solve_1() => new(""); public override ValueTask<string> Solve_2() => new(""); }

        [Fact]
        public async Task BaseDay()
        {
            await Solver.Solve<Day11>();
            await Solver.Solve<Day_11>();
        }
    }
}
