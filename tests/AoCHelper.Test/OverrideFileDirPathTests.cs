using System.IO;
using Xunit;

namespace AoCHelper.Test
{
    public class OverrideFileDirPathTests
    {
        private readonly ProblemSolver _solver;

        public OverrideFileDirPathTests()
        {
            _solver = new ProblemSolver();
        }

        private abstract class BaseProblemFixture : BaseProblem
        {
            protected override string FileExtension => nameof(OverrideFileDirPathTests);

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

        private class Problem33 : BaseProblemFixture { protected override string FileDirPath { get; } = "AlternativeInputs"; }
        private class Problem_33 : BaseProblemFixture { protected override string FileDirPath { get; } = "AlternativeInputs"; }

        [Fact]
        public void OverrideFilePathDir()
        {
            _solver.Solve<Problem33>();
            _solver.Solve<Problem_33>();
        }
    }
}
