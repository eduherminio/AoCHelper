using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace AoCHelper.Test
{
    public class SolverTest
    {
        private readonly ProblemSolver _solver;

        public SolverTest()
        {
            _solver = new ProblemSolver();
        }

        private abstract class ProblemFixture : BaseProblem
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

        private class Problem01 : ProblemFixture
        {
        }

        private class NonDetectedProblem : ProblemFixture
        {
        }

        [Fact]
        public void Solve()
        {
            _solver.Solve<Problem01>();
            _solver.SolveWithMetrics<Problem01>();

            Assert.Equal(2, _solver.LoadAllProblems(Assembly.GetExecutingAssembly()).Count());
        }

        [Fact]
        public void ShouldNotSolve()
        {
            Assert.Throws<FileNotFoundException>(() => _solver.Solve<NonDetectedProblem>());
            Assert.Throws<FileNotFoundException>(() => _solver.SolveWithMetrics<NonDetectedProblem>());
        }
    }
}
