using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace SettingUp
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = CSharpSyntaxTree.ParseText(@"
                public class Foo
                {
                    public int Bar()
                    {
                        return 42;
                    }
                }
            ");

            foreach (TextLine line in tree.GetText().Lines)
            {
                Console.WriteLine(line);
            }
        }
    }
}
