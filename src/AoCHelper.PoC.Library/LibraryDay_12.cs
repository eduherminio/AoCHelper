namespace AoCHelper.PoC.Library
{
    public class LibraryDay_12 : BaseLibraryDay
    {
        public LibraryDay_12()
        {
            if (!File.Exists(InputFilePath))
            {
                throw new SolvingException($"Path {InputFilePath} not found for {GetType().Name}");
            }
        }

        public override ValueTask<string> Solve_1()
        {
            Thread.Sleep(1);
            return new("Solution Library 2.1");
        }

        public override ValueTask<string> Solve_2()
        {
            Thread.Sleep(11);
            return new("Solution Library 2.2");
        }
    }
}
