using System.IO;
using Xunit;

namespace AoCHelper.Test
{
    public class OverrideInputFileDirPathTests
    {
        private abstract class BaseProblemFixture : BaseProblem
        {
            protected override string InputFileExtension => nameof(OverrideInputFileDirPathTests);

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

        private class Problem33 : BaseProblemFixture { protected override string InputFileDirPath { get; } = "AlternativeInputs"; }
        private class Problem_33 : BaseProblemFixture { protected override string InputFileDirPath { get; } = "AlternativeInputs"; }

        [Fact]
        public void OverrideInputFilePathDir()
        {
            Solver.Solve<Problem33>();
            Solver.Solve<Problem_33>();
        }
    }
}
