using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HackingClassesTests
{
    public class Class
    {
        public string Name { get; }
        public int LOC { get; }
        public IEnumerable<Method> Methods { get; }

        public Class(ClassDeclarationSyntax classDeclaration)
        {
            Name = classDeclaration.Identifier.ToString();
            Methods = classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>().Select(
                m=>new Method(m)
                );
            LOC = classDeclaration.GetText().Lines.Count;
        }
    }
}