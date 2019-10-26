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
            public override void Solve_1() => Solve();

            public override void Solve_2() => Solve();

            private void Solve()
            {
                if (!File.Exists(FilePath))
                {
                    throw new FileNotFoundException(FilePath);
                }
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
