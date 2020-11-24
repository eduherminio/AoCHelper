using System.IO;

namespace AoCHelper.PoC
{
    internal class RandomProblem : BaseProblem
    {
        /// <summary>
        /// Overriding FilePath, due to problem not following any convention (not even index one).
        /// </summary>
        public override string InputFilePath => "Inputs/RandomInput.random";

        public RandomProblem()
        {
            if (!File.Exists(InputFilePath))
            {
                throw new SolvingException($"Path {InputFilePath} not found for {GetType().Name}");
            }
        }

        public override string Solve_1()
        {
            System.Threading.Thread.Sleep(500);
            return "Solution 1";
        }

        public override string Solve_2()
        {
            System.Threading.Thread.Sleep(1501);
            return "Solution 2";
        }
    }
}
