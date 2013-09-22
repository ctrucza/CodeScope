using Roslyn.Compilers.CSharp;

namespace codescope
{
    class InterfaceCollector: CommonCollector
    {
        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            Nodes.Add(node);
        }
    }
}