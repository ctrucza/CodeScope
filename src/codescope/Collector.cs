using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roslyn.Compilers.CSharp;

namespace codescope
{
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
}