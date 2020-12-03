using System.IO;

namespace AoCHelper.PoC
{
    public class Day01 : BaseDay
    {
        public Day01()
        {
            if (!File.Exists(InputFilePath))
            {
                throw new SolvingException($"Path {InputFilePath} not found for {GetType().Name}");
            }
        }

        public override string Solve_1()
        {
            return "Solution 1";
        }

        public override string Solve_2()
        {
            return "Solution 2";
        }
    }
}
