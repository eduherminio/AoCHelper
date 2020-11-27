using Xunit;

namespace AoCHelper.Test
{
    public class OverrideClassPrefixTests
    {
        private abstract class GreatName : BaseProblem
        {
            protected override string ClassPrefix => nameof(GreatName);
        }

        private class GreatName01 : GreatName { public override string Solve_1() => ""; public override string Solve_2() => ""; }
        private class GreatName_01 : GreatName { public override string Solve_1() => ""; public override string Solve_2() => ""; }

        [Fact]
        public void OverrideClassPrefix()
        {
            Solver.Solve<GreatName01>();
            Solver.Solve<GreatName_01>();
        }

        private class Day11 : BaseDay { public override string Solve_1() => ""; public override string Solve_2() => ""; }
        private class Day_11 : BaseDay { public override string Solve_1() => ""; public override string Solve_2() => ""; }

        [Fact]
        public void BaseDay()
        {
            Solver.Solve<Day11>();
            Solver.Solve<Day_11>();
        }
    }
}
