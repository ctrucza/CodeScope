using System;
using Roslyn.Compilers.CSharp;

namespace codescope
{
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