using System;
using Roslyn.Compilers.CSharp;

namespace codescope
{
    class ClassWrapper: NodeWrapper<ClassDeclarationSyntax>
    {
        protected override void DoDump(ClassDeclarationSyntax aNode)
        {
            Console.WriteLine("{0} ({1})", GetName(), GetLineCount());
            Collector<MethodDeclarationSyntax, MethodWrapper> methods = new Collector<MethodDeclarationSyntax, MethodWrapper>();
            methods.Visit(aNode);
            methods.Dump();
            Console.WriteLine();
        }
    }
}