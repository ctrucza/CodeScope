using System;
using Roslyn.Compilers.CSharp;

namespace codescope
{
    class MethodWrapper: NodeWrapper<MethodDeclarationSyntax>
    {
        protected override string DoGetName(MethodDeclarationSyntax aNode)
        {
            string result = aNode.Identifier.ToString();
            NodeWrapper <SyntaxNode> parent = new NodeWrapper<SyntaxNode>();
            parent.SetNode(aNode.Parent);
            result = parent.GetName() + "." + result;
            return result;
        }

        protected override void DoDump(MethodDeclarationSyntax aNode)
        {
            Console.WriteLine("{0} ({1})", GetName(), GetLineCount());
        }
    }
}