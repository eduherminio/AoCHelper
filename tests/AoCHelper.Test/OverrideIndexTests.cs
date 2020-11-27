using System.IO;
using Xunit;

namespace AoCHelper.Test
{
    public class OverrideIndexTests
    {
        private abstract class ProblemFixture : BaseProblem
        {
            public override string Solve_1() => Solve();

            public override string Solve_2() => Solve();

            private string Solve()
            {
                if (!File.Exists(InputFilePath))
                {
                    throw new FileNotFoundException(InputFilePath);
                }

                return string.Empty;
            }
        }

        private class CustomProblem : ProblemFixture { public override uint CalculateIndex() => 69; }

        [Fact]
        public void OverrideIndex()
        {
            Solver.Solve<CustomProblem>();
            Solver.Solve<CustomProblem>();
        }
    }
}
