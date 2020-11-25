using System.IO;
using Xunit;

namespace AoCHelper.Test
{
    public class OverrideInputFilePathTests
    {
        private readonly ProblemSolver _solver;

        public OverrideInputFilePathTests()
        {
            _solver = new ProblemSolver();
        }

        private abstract class BaseProblemFixture : BaseProblem
        {
            protected override string InputFileExtension => nameof(OverrideInputFilePathTests);

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

        private class CustomProblem : BaseProblemFixture { public override string InputFilePath => $"AlternativeInputs/43.{nameof(OverrideInputFilePathTests)}"; }

        [Fact]
        public void OverrideInputFilePath()
        {
            _solver.Solve<CustomProblem>();
        }
    }
}
