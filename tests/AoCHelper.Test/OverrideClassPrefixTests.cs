using System.IO;
using Xunit;

namespace AoCHelper.Test
{
    public class OverrideClassPrefixTests
    {
        private readonly ProblemSolver _solver;

        public OverrideClassPrefixTests()
        {
            _solver = new ProblemSolver();
        }

        private abstract class GreatName : BaseProblem
        {
            protected override string ClassPrefix => nameof(GreatName);
        }

        private class GreatName01 : GreatName { public override string Solve_1() => ""; public override string Solve_2() => ""; }
        private class GreatName_01 : GreatName { public override string Solve_1() => ""; public override string Solve_2() => ""; }

        [Fact]
        public void OverrideClassPrefix()
        {
            _solver.Solve<GreatName01>();
            _solver.Solve<GreatName_01>();
        }

        private class Day11 : BaseDay { public override string Solve_1() => ""; public override string Solve_2() => ""; }
        private class Day_11 : BaseDay { public override string Solve_1() => ""; public override string Solve_2() => ""; }

        [Fact]
        public void BaseDay()
        {
            _solver.Solve<Day11>();
            _solver.Solve<Day_11>();
        }
    }
}
