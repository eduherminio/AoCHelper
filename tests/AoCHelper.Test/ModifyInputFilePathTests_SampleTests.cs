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
