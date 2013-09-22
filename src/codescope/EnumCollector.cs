using Roslyn.Compilers.CSharp;

namespace codescope
{
    class EnumCollector: CommonCollector
    {
        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            Nodes.Add(node);
        }
    }
}