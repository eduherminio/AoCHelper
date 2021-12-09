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

        public override ValueTask<string> Solve_1()
        {
            return new("Solution 1.1");
        }

        public override ValueTask<string> Solve_2()
        {
            return new("Solution 1.2");
        }
    }
}
