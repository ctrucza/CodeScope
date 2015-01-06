using System;

namespace CountClasses
{
    public static class CountClasses
    {
        static void Main(string[] arguments)
        {
            string solutionFileName = arguments[0];
            var s = new Solution(solutionFileName);
            Console.WriteLine(s);
        }
    }
}
