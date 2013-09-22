using System;
using Roslyn.Compilers.CSharp;

namespace codescope
{
    class ClassCollector: CommonCollector
    {
        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            Console.WriteLine("Class collector visits node {0}", node);
            Nodes.Add(node);
        }
    }
}