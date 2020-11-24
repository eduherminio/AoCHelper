using System.IO;
using Xunit;

namespace AoCHelper.Test
{
    public class OverrideIndexTests
    {
        private readonly ProblemSolver _solver;

        public OverrideIndexTests()
        {
            _solver = new ProblemSolver();
        }

        private abstract class ProblemFixture : BaseProblem
        {
            public override string Solve_1() => Solve();

            public override string Solve_2() => Solve();

            private string Solve()
            {
                if (!File.Exists(FilePath))
                {
                    throw new FileNotFoundException(FilePath);
                }

                return string.Empty;
            }
        }

        private class CustomProblem : ProblemFixture { public override uint CalculateIndex() => 69; }

        [Fact]
        public void OverrideIndex()
        {
            _solver.Solve<CustomProblem>();
            _solver.SolveWithMetrics<CustomProblem>();
        }
    }
}
