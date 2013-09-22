using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roslyn.Compilers.CSharp;

namespace codescope
{
    class CommonCollector: SyntaxWalker
    {
        public readonly List<SyntaxNode> Nodes = new List<SyntaxNode>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Count: {0}", Nodes.Count);
            sb.AppendLine();

            int totalLoc = Nodes.Sum(node => node.GetText().LineCount);
            int averageLoc = totalLoc / Nodes.Count;
            sb.AppendFormat("Average LOC: {0}", averageLoc);
            sb.AppendLine();

            int maxLoc = Nodes.Max(node => node.GetText().LineCount);
            sb.AppendFormat("Max LOC: {0}", maxLoc);
            sb.AppendLine();

            IEnumerable<SyntaxNode> syntaxNodes = Nodes.Where(node => node.GetText().LineCount == maxLoc);
            foreach (SyntaxNode syntaxNode in syntaxNodes)
            {
                string name = GetNameFor(syntaxNode);
                if (string.IsNullOrEmpty(name))
                    continue;
                sb.AppendLine(name);
            }

            return sb.ToString();
        }

        protected virtual string GetNameFor(SyntaxNode node)
        {
            string result = null;
            if (node is BaseTypeDeclarationSyntax)
                result = (node as BaseTypeDeclarationSyntax).Identifier.ToString();
            return result;
        }
    }
}