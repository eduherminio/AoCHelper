using System.Reflection;

namespace AoCHelper.PoC.Library;

internal abstract class BaseLibraryProblem : BaseProblem
{
    protected override string InputFileDirPath =>
        Path.Combine(
            Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!,
            base.InputFileDirPath);
}
