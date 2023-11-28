﻿using System.Reflection;

namespace AoCHelper.PoC.Library;
public abstract class BaseLibraryDay : BaseDay
{
    protected override string ClassPrefix => "LibraryDay";

    public override string InputFilePath =>
        Path.Combine(
            Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!,
            base.InputFilePath);
}
