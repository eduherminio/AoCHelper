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

/*
    Hi Mael,

    First of all, thanks for using AoCHelper and for your kind words. And apologies for the delay, it's being a hectic week.

    I can think of two ways of making this work: changing `InputFilePath` or `InputFileDirPath` to accomodate test files/dirs.
    As you probably noticed, those properties don't have a setter, and therefore cannot be modified in an obvious way from a external test method.

    Since the beginning of this library I've wanted to give a proper thought to testing solutions 
    
    I'll note your suggestion down
*/
