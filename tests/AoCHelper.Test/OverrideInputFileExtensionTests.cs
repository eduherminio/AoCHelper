using Xunit;

namespace AoCHelper.Test
{
    [Collection("Sequential")]
    public class OverrideInputFileExtensionTests
    {
        private abstract class BaseProblemFixture : BaseProblem
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

        private class Problem01 : BaseProblemFixture { protected override string InputFileExtension => nameof(OverrideInputFileExtensionTests); }
        private class Problem_01 : BaseProblemFixture { protected override string InputFileExtension => nameof(OverrideInputFileExtensionTests); }

        [Fact]
        public async Task OverrideInputFileExtension()
        {
            await Solver.Solve<Problem01>();
            await Solver.Solve<Problem_01>();
        }
    }
}
