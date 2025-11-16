using System.Reflection;

namespace AoCHelper.PoC.Library;

public abstract class BaseLibraryDay : BaseDay
{
    protected override string ClassPrefix => "LibraryDay";

    protected override string InputFileDirPath =>
        Path.Combine(
            Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!,
            base.InputFileDirPath);
}
