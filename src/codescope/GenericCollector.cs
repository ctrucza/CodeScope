using Roslyn.Compilers.CSharp;

namespace codescope
{
    // Aborted trial to make collectors generic
    class GenericCollector<T>: CommonCollector where T : SyntaxNode
    {
        public override void DefaultVisit(SyntaxNode node)
        {
            if (node is T)
                Nodes.Add(node as T);
        }
        public override void Visit(SyntaxNode node)
        {
            if (node is T)
                Nodes.Add(node as T);
        }
    }
}