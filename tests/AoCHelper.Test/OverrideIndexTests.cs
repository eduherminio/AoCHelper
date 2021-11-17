using Xunit;

namespace AoCHelper.Test
{
    [Collection("Sequential")]
    public class OverrideIndexTests
    {
        private abstract class ProblemFixture : BaseProblem
        {
            public override ValueTask<string> Solve_1() => Solve();

            public override ValueTask<string> Solve_2() => Solve();

            private ValueTask<string> Solve()
            {
                if (!File.Exists(InputFilePath))
                {
                    throw new FileNotFoundException(InputFilePath);
                }

                return new(string.Empty);
            }
        }

        private class CustomProblem : ProblemFixture { public override uint CalculateIndex() => 69; }

        [Fact]
        public async Task OverrideIndex()
        {
            await Solver.Solve<CustomProblem>();
            await Solver.Solve<CustomProblem>();
        }
    }
}
