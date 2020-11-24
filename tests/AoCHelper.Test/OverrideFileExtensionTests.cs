using System.IO;
using Xunit;

namespace AoCHelper.Test
{
    public class OverrideFileExtensionTests
    {
        private readonly ProblemSolver _solver;

        public OverrideFileExtensionTests()
        {
            _solver = new ProblemSolver();
        }

        private abstract class BaseProblemFixture : BaseProblem
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

        private class Problem01 : BaseProblemFixture { protected override string FileExtension => nameof(OverrideFileExtensionTests); }
        private class Problem_01 : BaseProblemFixture { protected override string FileExtension => nameof(OverrideFileExtensionTests); }

        private class Problem02 : BaseProblemFixture { protected override string FileExtension => nameof(OverrideFileExtensionTests); }
        private class Problem_02 : BaseProblemFixture { protected override string FileExtension => nameof(OverrideFileExtensionTests); }

        [Fact]
        public void OverrideFileExtension()
        {
            _solver.Solve<Problem01>();
            _solver.Solve<Problem_01>();

            Assert.Throws<FileNotFoundException>(() => _solver.Solve<Problem02>());
            Assert.Throws<FileNotFoundException>(() => _solver.Solve<Problem_02>());
        }
    }
}
