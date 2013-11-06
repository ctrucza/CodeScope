using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roslyn.Compilers.CSharp;

namespace codescope
{
    class Collector: SyntaxWalker
    {
        private Type t;
        private readonly List<SyntaxNode> nodes = new List<SyntaxNode>();

        public Collector(Type t)
        {
            this.t = t;
        }

        public override void Visit(SyntaxNode node)
        {
            base.Visit(node);
            if (node.GetType() == t)
                RegisterNode(node);
        }

        private void RegisterNode(SyntaxNode node)
        {
            nodes.Add(node);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("Count: {0}", nodes.Count));

            if (nodes.Count == 0)
                return sb.ToString();

            int totalLoc = nodes.Sum(node => node.GetText().LineCount);
            int averageLoc = totalLoc / nodes.Count;
            sb.AppendLine(string.Format("Average LOC: {0}", averageLoc));

            int maxLoc = nodes.Max(node => node.GetText().LineCount);
            sb.AppendLine(string.Format("Max LOC: {0}", maxLoc));

            IEnumerable<SyntaxNode> syntaxNodes = nodes.Where(node => node.GetText().LineCount == maxLoc);
            foreach (SyntaxNode syntaxNode in syntaxNodes)
            {
                string name = GetNameFor(syntaxNode);
                if (string.IsNullOrEmpty(name))
                    continue;
                sb.AppendLine(name);
            }

            return sb.ToString();
        }

        private string GetNameFor(SyntaxNode node)
        {
            string result = null;

            // TODO: find a nicer view to get the name of a node
            if (node is BaseTypeDeclarationSyntax)
                result = (node as BaseTypeDeclarationSyntax).Identifier.ToString();
            else if (node is MethodDeclarationSyntax)
            {
                result = (node as MethodDeclarationSyntax).Identifier.ToString();
                result = GetNameFor(node.Parent) + "." + result;
            }

            return result;
        }
    }
}