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

        public override ValueTask<string> Solve_1()
        {
            return new("Solution Random.1");
        }

        public override ValueTask<string> Solve_2()
        {
            Thread.Sleep(10_000);
            return new("Solution Random.2");
        }
    }
}
