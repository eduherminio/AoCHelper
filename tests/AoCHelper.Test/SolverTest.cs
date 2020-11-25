using System;
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
                if (!File.Exists(InputFilePath))
                {
                    throw new FileNotFoundException(InputFilePath);
                }

                return string.Empty;
            }
        }

        private class Problem66 : ProblemFixture { }

        private class IllCreatedCustomProblem : ProblemFixture { }

        [Fact]
        public void Solve()
        {
            _solver.Solve<Problem66>();
        }

        [Fact]
        public void SolveWithMetrics()
        {
            _solver.SolveWithMetrics<Problem66>();
        }

        [Fact]
        public void ShouldNotSolve()
        {
            Assert.Throws<FileNotFoundException>(() => _solver.Solve<IllCreatedCustomProblem>());
        }

        [Fact]
        public void ShouldNotSolveWithMetrics()
        {
            Assert.Throws<FileNotFoundException>(() => _solver.SolveWithMetrics<IllCreatedCustomProblem>());
        }

        [Fact]
        public void LoadAllProblems()
        {
            Assert.Equal(14, ProblemSolver.LoadAllProblems(Assembly.GetExecutingAssembly()).Count());
        }
    }
}
