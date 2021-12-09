namespace AoCHelper.PoC
{
    internal class Problem03 : BaseProblem
    {
        public Problem03()
        {
            if (!File.Exists(InputFilePath))
            {
                throw new SolvingException($"Path {InputFilePath} not found for {GetType().Name}");
            }
        }

        public override ValueTask<string> Solve_1()
        {
            Thread.Sleep(101);
            return new("Solution 3.1");
        }

        public override ValueTask<string> Solve_2()
        {
            Thread.Sleep(501);
            return new("Solution 3.2");
        }
    }
}
