using System.IO;

namespace AoCHelper.PoC
{
    internal class RandomProblem : BaseProblem
    {
        /// <summary>
        /// Overriding FilePath, due to problem not following convention
        /// </summary>
        public override string FilePath => "Inputs/01.in";

        public RandomProblem()
        {
            if (!File.Exists(FilePath))
            {
                throw new SolvingException($"Path {FilePath} not found for {GetType().Name}");
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
