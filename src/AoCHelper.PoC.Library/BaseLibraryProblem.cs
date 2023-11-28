using System.Reflection;

namespace AoCHelper.PoC.Library;
internal abstract class BaseLibraryProblem : BaseProblem
{
    public override string InputFilePath =>
        Path.Combine(
            Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!,
            base.InputFilePath);
}
