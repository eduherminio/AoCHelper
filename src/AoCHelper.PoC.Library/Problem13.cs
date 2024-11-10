namespace AoCHelper.PoC.Library;

internal class Problem13 : BaseLibraryProblem
{
    public Problem13()
    {
        if (!File.Exists(InputFilePath))
        {
            throw new SolvingException($"Path {InputFilePath} not found for {GetType().Name}");
        }
    }

    public override ValueTask<string> Solve_1()
    {
        Thread.Sleep(101);
        return new("Solution Library 3.1");
    }

    public override ValueTask<string> Solve_2()
    {
        Thread.Sleep(501);
        return new("Solution Library 3.2");
    }
}
