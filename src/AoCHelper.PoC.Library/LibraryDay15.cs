﻿namespace AoCHelper.PoC.Library
{
    public class LibraryDay15 : BaseLibraryDay
    {
        public LibraryDay15()
        {
            throw new SolvingException("Exception in constructor");
        }

        public override ValueTask<string> Solve_1()
        {
            Thread.Sleep(66);
            return new("Solution Library 5.1");
        }

        public override ValueTask<string> Solve_2()
        {
            Thread.Sleep(66);
            return new("Solution Library 5.2");
        }
    }
}
