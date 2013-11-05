using Roslyn.Compilers.CSharp;

namespace codescope
{
    internal class GenericCollector<T> : CommonCollector
    {
        public override void Visit(SyntaxNode node)
        {
            base.Visit(node);
            if (node is T)
                RegisterNode(node);
        }
    }
}