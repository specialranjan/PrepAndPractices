namespace CSharpQuestions.Lib
{
    using System;
    public class ConstVsStatic
    {
        public const int SIZE = 500;
        public readonly int ReadOnlySize;
        public static int StaticSize;
        public static int StaticGlobalSize = 400;

        public ConstVsStatic(int ReadOnlySize, int staticSize)
        {
            this.ReadOnlySize = ReadOnlySize;
            StaticSize = staticSize;
            StaticGlobalSize++;
        }
    }
}
