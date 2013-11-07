using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roslyn.Compilers.CSharp;

namespace codescope
{
    class Collector<T,U>: SyntaxWalker
        where U:NodeWrapper, new()
    {
        private readonly List<U> nodes = new List<U>();

        public override void Visit(SyntaxNode node)
        {
            base.Visit(node);
            if (node is T)
                RegisterNode(node);
        }

        private void RegisterNode(SyntaxNode node)
        {
            U wrapper = new U();
            wrapper.SetNode(node);
            nodes.Add(wrapper);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("Count: {0}", nodes.Count));

            if (nodes.Count == 0)
                return sb.ToString();

            int totalLoc = nodes.Sum(node => node.GetLineCount());
            int averageLoc = totalLoc / nodes.Count;
            sb.AppendLine(string.Format("Average LOC: {0}", averageLoc));

            int maxLoc = nodes.Max(node => node.GetLineCount());
            sb.AppendLine(string.Format("Max LOC: {0}", maxLoc));

            IEnumerable<NodeWrapper> nodeWrappers = nodes.Where(node => node.GetLineCount() == maxLoc);
            foreach (NodeWrapper nodeWrapper in nodeWrappers)
            {
                string name = nodeWrapper.GetName();
                sb.AppendLine(name);
            }

            return sb.ToString();
        }

    }

    class ClassWrapper: NodeWrapper
    {
    }

    class MethodWrapper: NodeWrapper
    {
        protected override string DoGetName(SyntaxNode aNode)
        {
            // Assert aNode is MehodDeclarationSyntax
            string result = (aNode as MethodDeclarationSyntax).Identifier.ToString();
            NodeWrapper parent = new NodeWrapper();
            parent.SetNode(aNode.Parent);
            result = parent.GetName() + "." + result;
            return result;
        }
    }

    internal class NodeWrapper
    {
        private SyntaxNode node;

        public void SetNode(SyntaxNode aNode)
        {
            node = aNode;
        }

        public int GetLineCount()
        {
            return node.GetText().LineCount;
        }

        public string GetName()
        {
            return DoGetName(node);
        }

        protected virtual string DoGetName(SyntaxNode aNode)
        {
            string result = null;

            if (aNode is BaseTypeDeclarationSyntax)
                result = (aNode as BaseTypeDeclarationSyntax).Identifier.ToString();

            return result;
        }
    }
}