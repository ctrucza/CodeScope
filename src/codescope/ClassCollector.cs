using Roslyn.Compilers.CSharp;

namespace codescope
{
    class ClassCollector: CommonCollector
    {
        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            Nodes.Add(node);
            //MethodCollector c = new MethodCollector();
            //c.Visit(node);
        }
    }
}