using Xunit;

namespace AoCHelper.Test
{
    [Collection("Sequential")]
    public class OverrideInputFilePathTests
    {
        private abstract class BaseProblemFixture : BaseProblem
        {
            protected override string InputFileExtension => nameof(OverrideInputFilePathTests);

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

        private class CustomProblem : BaseProblemFixture { public override string InputFilePath => $"AlternativeInputs/43.{nameof(OverrideInputFilePathTests)}"; }

        [Fact]
        public async Task OverrideInputFilePath()
        {
            await Solver.Solve<CustomProblem>();
        }
    }
}
