using System.IO;

namespace AoCHelper.PoC
{
    internal class Problem04 : BaseProblem
    {
        public Problem04()
        {
            if (!File.Exists(InputFilePath))
            {
                throw new SolvingException($"Path {InputFilePath} not found for {GetType().Name}");
            }
        }

        public override string Solve_1()
        {
            System.Threading.Thread.Sleep(1500);
            return "Solution 1";
        }

        public override string Solve_2()
        {
            System.Threading.Thread.Sleep(2001);
            return "Solution 2";
        }
    }
}
