using System;
using System.Collections.Generic;
using Roslyn.Compilers.CSharp;

namespace codescope
{
    class GenericCollector<T>: CommonCollector where T : SyntaxNode
    {
        public override void DefaultVisit(SyntaxNode node)
        {
            Console.WriteLine("Generic collector of {0} DefaultVisits node {1}", typeof(T), node.GetType());
            if (node is T)
                Nodes.Add(node as T);
        }
        public override void Visit(SyntaxNode node)
        {
            Console.WriteLine("Generic collector of {0} visits node {1}", typeof(T), node.GetType());
            if (node is T)
                Nodes.Add(node as T);
        }
    }
}