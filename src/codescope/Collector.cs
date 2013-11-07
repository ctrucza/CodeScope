using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roslyn.Compilers.CSharp;

namespace codescope
{
    interface ICollector
    {
        void Report();
        void Dump();
    }

    class Collector<T,U>: SyntaxWalker, ICollector
        where T: SyntaxNode
        where U: NodeWrapper<T>, new()
    {
        private readonly List<U> nodes = new List<U>();

        public override void Visit(SyntaxNode node)
        {
            base.Visit(node);
            if (node is T)
                RegisterNode(node as T);
        }

        private void RegisterNode(T node)
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

            IEnumerable<U> nodeWrappers = nodes.Where(node => node.GetLineCount() == maxLoc);
            foreach (U nodeWrapper in nodeWrappers)
            {
                string name = nodeWrapper.GetName();
                sb.AppendLine(name);
            }

            return sb.ToString();
        }

        public void Report()
        {
            Console.WriteLine(this);
        }

        public void Dump()
        {
            foreach (U node in nodes)
            {
                node.Dump();
            }
        }
    }

    class ClassWrapper: NodeWrapper<ClassDeclarationSyntax>
    {
    }

    class MethodWrapper: NodeWrapper<MethodDeclarationSyntax>
    {
        protected override string DoGetName(MethodDeclarationSyntax aNode)
        {
            // Assert aNode is MehodDeclarationSyntax
            string result = aNode.Identifier.ToString();
            NodeWrapper <SyntaxNode> parent = new NodeWrapper<SyntaxNode>();
            parent.SetNode(aNode.Parent);
            result = parent.GetName() + "." + result;
            return result;
        }
    }

    internal class NodeWrapper<T>
        where T: SyntaxNode
    {
        private T node;

        public void SetNode(T aNode)
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

        protected virtual string DoGetName(T aNode)
        {
            string result = null;

            if (aNode is BaseTypeDeclarationSyntax)
                result = (aNode as BaseTypeDeclarationSyntax).Identifier.ToString();

            return result;
        }

        public void Dump()
        {
            DoDump(node);
        }

        protected virtual void DoDump(T aNode)
        {
            Console.WriteLine(GetName());
        }
    }
}