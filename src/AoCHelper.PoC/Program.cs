using AoCHelper;
using AoCHelper.PoC.Library;

await Solver.SolveAll(options =>
{
    options.ShowConstructorElapsedTime = true;
    options.ShowTotalElapsedTimePerDay = true;
    options.ShowOverallResults = true;
    options.ClearConsole = false;
    options.ProblemAssemblies = [System.Reflection.Assembly.GetAssembly(typeof(BaseLibraryDay))!, .. options.ProblemAssemblies];
});
