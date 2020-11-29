using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace AoCHelper.Test
{
    public class SolverTest
    {
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
            Solver.Solve<Problem66>();
        }

        [Fact]
        public void SolveParams()
        {
            Solver.Solve(typeof(Problem66));
        }

        [Fact]
        public void SolveEnumerable()
        {
            Solver.Solve(new[] { typeof(Problem66) });
        }

        /// <summary>
        /// AoCHelper isn't actually solving anything, since Assembly.GetEntryAssembly() returns xunit assembly.
        /// </summary>
        [Fact]
        public void SolveLast()
        {
            Solver.SolveLast();
        }

        [Fact]
        public void ShouldNotThrowExceptionIfCantSolve()
        {
            Solver.Solve<IllCreatedCustomProblem>();
        }

        [Fact]
        public void LoadAllProblems()
        {
            Assert.Equal(
                Assembly.GetExecutingAssembly()!.GetTypes().Count(type => typeof(BaseProblem).IsAssignableFrom(type) && !type.IsAbstract),
                Solver.LoadAllProblems(Assembly.GetExecutingAssembly()).Count());
        }
    }
}
