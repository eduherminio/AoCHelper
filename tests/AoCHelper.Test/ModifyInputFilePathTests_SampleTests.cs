using AoCHelper;
using NUnit.Framework;

public class ModifyInputFilePathTests
{
    abstract class CustomInputPathBaseDay : BaseDay
    {
        public string TestInputFilePath { private get; set; }
        public override string InputFilePath => TestInputFilePath;

        protected CustomInputPathBaseDay()
        {
            TestInputFilePath = base.InputFilePath;
        }
    }

    class Day_99 : CustomInputPathBaseDay
    {
        public override ValueTask<string> Solve_1() => new($"Custom file path: {InputFilePath}");
        public override ValueTask<string> Solve_2() => new($"No longer useful InputFileDirPath: {InputFileDirPath}");
    }

    [TestCase(typeof(Day_99), "TestInputs/Day_1234.txt", "Custom file path: TestInputs/Day_1234.txt", $"No longer useful InputFileDirPath: Inputs")]
    public async Task ModifyInputFilePath(Type type, string inputFilePath, string sol1, string sol2)
    {
        if (Activator.CreateInstance(type) is CustomInputPathBaseDay instance)
        {
            instance.TestInputFilePath = inputFilePath;

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
