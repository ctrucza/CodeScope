using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;

namespace codescope
{
    class MethodCollector: CommonCollector
    {
        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            Nodes.Add(node);
        }
    }
}