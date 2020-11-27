using System.IO;
using Xunit;

namespace AoCHelper.Test
{
    public class OverrideInputFileExtensionTests
    {
        private abstract class BaseProblemFixture : BaseProblem
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

        private class Problem01 : BaseProblemFixture { protected override string InputFileExtension => nameof(OverrideInputFileExtensionTests); }
        private class Problem_01 : BaseProblemFixture { protected override string InputFileExtension => nameof(OverrideInputFileExtensionTests); }

        private class Problem02 : BaseProblemFixture { protected override string InputFileExtension => nameof(OverrideInputFileExtensionTests); }
        private class Problem_02 : BaseProblemFixture { protected override string InputFileExtension => nameof(OverrideInputFileExtensionTests); }

        [Fact]
        public void OverrideInputFileExtension()
        {
            Solver.Solve<Problem01>();
            Solver.Solve<Problem_01>();

            Assert.Throws<FileNotFoundException>(() => Solver.Solve<Problem02>());
            Assert.Throws<FileNotFoundException>(() => Solver.Solve<Problem_02>());
        }
    }
}
