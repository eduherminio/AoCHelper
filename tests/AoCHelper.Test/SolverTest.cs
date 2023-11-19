using System.Reflection;
using Xunit;

namespace AdventOfCode.Days._01
{
    class Day01 : AoCHelper.Test.SolverTest.ProblemFixture { }

}

namespace AdventOfCode.Days._02
{
    class Day02 : AoCHelper.Test.SolverTest.ProblemFixture { }

}

namespace AdventOfCode.Days._10
{
    class Day10 : AoCHelper.Test.SolverTest.ProblemFixture { }

}

namespace AoCHelper.Test
{
    [Collection("Sequential")]
    public class SolverTest
    {
        internal abstract class ProblemFixture : BaseProblem
        {
            public override ValueTask<string> Solve_1() => Solve();

            public override ValueTask<string> Solve_2() => Solve();

            private ValueTask<string> Solve()
            {
                if (!File.Exists(InputFilePath))
                {
                    throw new FileNotFoundException(InputFilePath);
                }

                return new(string.Empty);
            }
        }

        private class Problem66 : ProblemFixture { }

        private class IllCreatedCustomProblem : ProblemFixture
        {
            public IllCreatedCustomProblem()
            {
                throw new Exception();
            }
        }

        [Fact]
        public async Task Solve()
        {
            await Solver.Solve<Problem66>();
            await Solver.Solve<Problem66>(_ => { });
            await Solver.Solve<Problem66>(options: null);
        }

        [Fact]
        public async Task SolveIntParams()
        {
            await Solver.Solve(options: null, 1, 2);
        }

        [Fact]
        public async Task SolveIntEnumerable()
        {
            await Solver.Solve(new List<uint> { 1, 2 });
            await Solver.Solve(new List<uint> { 1, 2 }, _ => { });
        }

        [Fact]
        public async Task SolveTypeParams()
        {
            await Solver.Solve(_ => { }, typeof(Problem66));
            await Solver.Solve(options: null, typeof(Problem66));
        }

        [Fact]
        public async Task SolveTypeEnumerable()
        {
            await Solver.Solve(new List<Type> { typeof(Problem66) });
            await Solver.Solve(new List<Type> { typeof(Problem66) }, _ => { });
        }

        /// <summary>
        /// AoCHelper isn't actually solving anything, since Assembly.GetEntryAssembly() returns xunit assembly.
        /// </summary>
        [Fact]
        public async Task SolveLast()
        {
            await Solver.SolveLast();
            await Solver.SolveLast(_ => { });
        }

        /// <summary>
        /// AoCHelper isn't actually solving anything, since Assembly.GetEntryAssembly() returns xunit assembly.
        /// </summary>
        [Fact]
        public async Task SolveAll()
        {
            await Solver.SolveAll();
            await Solver.SolveAll(_ => { });
        }

        [Fact]
        public async Task ShouldNotThrowExceptionIfCantSolve()
        {
            await Solver.Solve<IllCreatedCustomProblem>();
        }

        [Fact]
        public void LoadAllProblems()
        {
            Assert.Equal(
                Assembly.GetExecutingAssembly()!.GetTypes().Count(type => typeof(BaseProblem).IsAssignableFrom(type) && !type.IsAbstract),
                Solver.LoadAllProblems(Assembly.GetExecutingAssembly()).Count());
        }

        [Fact]
        public void LoadAllProblems_OrderedByFullName()
        {
            var orderedTypes = Solver.LoadAllProblems(Assembly.GetExecutingAssembly()).OrderBy(t => t.FullName);
            var types = Solver.LoadAllProblems(Assembly.GetExecutingAssembly());

            foreach (var (First, Second) in orderedTypes.Zip(types))
            {
                Assert.Equal(First, Second);
            }
        }
    }
}
