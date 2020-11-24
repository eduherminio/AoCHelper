using System.IO;
using Xunit;

namespace AoCHelper.Test
{
    public class OverrideFilePathTests
    {
        private readonly ProblemSolver _solver;

        public OverrideFilePathTests()
        {
            _solver = new ProblemSolver();
        }

        private abstract class BaseProblemFixture : BaseProblem
        {
            protected override string FileExtension => nameof(OverrideFilePathTests);

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

        private class CustomProblem : BaseProblemFixture { public override string FilePath => "AlternativeInputs/43.OverrideFilePathTests"; }

        [Fact]
        public void OverrideFilePath()
        {
            _solver.Solve<CustomProblem>();
        }
    }
}
