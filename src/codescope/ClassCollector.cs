using System.Collections.Generic;
using Roslyn.Compilers.CSharp;

namespace codescope
{
    class ClassCollector: SyntaxWalker
    {
        public readonly List<ClassDeclarationSyntax> Classes = new List<ClassDeclarationSyntax>(); 

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            Classes.Add(node);
        }
    }
}