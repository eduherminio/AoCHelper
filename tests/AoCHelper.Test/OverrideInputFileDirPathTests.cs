using Xunit;

namespace AoCHelper.Test
{
    [Collection("Sequential")]
    public class OverrideInputFileDirPathTests
    {
        private abstract class BaseProblemFixture : BaseProblem
        {
            protected override string InputFileExtension => nameof(OverrideInputFileDirPathTests);

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

        private class Problem33 : BaseProblemFixture { protected override string InputFileDirPath { get; } = "AlternativeInputs"; }
        private class Problem_33 : BaseProblemFixture { protected override string InputFileDirPath { get; } = "AlternativeInputs"; }

        [Fact]
        public async Task OverrideInputFilePathDir()
        {
            await Solver.Solve<Problem33>();
            await Solver.Solve<Problem_33>();
        }
    }
}
