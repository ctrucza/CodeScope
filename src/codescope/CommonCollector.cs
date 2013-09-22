using System.Collections.Generic;
using Roslyn.Compilers.CSharp;

namespace codescope
{
    class CommonCollector: SyntaxWalker
    {
        public readonly List<SyntaxNode> Nodes = new List<SyntaxNode>(); 
    }
}