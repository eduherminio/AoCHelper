using AoCHelper;
using NUnit.Framework;

public class ModifyInputFileDirPathTests
{
    abstract class CustomDirBaseDay : BaseDay
    {
        public string TestInputFileDirPath { private get; set; }
        protected override string InputFileDirPath => TestInputFileDirPath;

        protected CustomDirBaseDay()
        {
            TestInputFileDirPath = base.InputFileDirPath;
        }
    }

    class Day_99 : CustomDirBaseDay
    {
        public override ValueTask<string> Solve_1() => new($"Custom file path: {InputFilePath}");
        public override ValueTask<string> Solve_2() => new($"Custom dir path: {InputFileDirPath}");
    }

    [TestCase(typeof(Day_99), "CustomInputDir/", "Custom file path: CustomInputDir/99.txt", "Custom dir path: CustomInputDir/")]
    public async Task ModifyInputFileDirPath(Type type, string inputFileDirPath, string sol1, string sol2)
    {
        if (Activator.CreateInstance(type) is CustomDirBaseDay instance)
        {
            instance.TestInputFileDirPath = inputFileDirPath;

            Assert.AreEqual(sol1, await instance.Solve_1());
            Assert.AreEqual(sol2, await instance.Solve_2());
        }
        else
        {
            Assert.Fail();
        }
    }
}
