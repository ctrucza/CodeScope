using Roslyn.Compilers.CSharp;

namespace codescope
{
    class MethodCollector: CommonCollector
    {
        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            Nodes.Add(node);
        }

        protected override string GetNameFor(SyntaxNode node)
        {
            string result = null;
            if (node is MethodDeclarationSyntax)
                result = (node as MethodDeclarationSyntax).Identifier.ToString();
            return result;
        }
    }
}