using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace HackingClassesTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void test_usage()
        {
            var syntaxTree = Parse(@"
            class Foo
            {
                void Bar()
                {
            
                }

                void Baz()
                {
            
                }
            }
            ");
            var classDeclarationSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Single();

            Class c = new Class(classDeclarationSyntax);
            
            Assert.AreEqual("Foo", c.Name);
            Assert.AreEqual(1, c.LOC);
            Assert.AreEqual(2, c.Methods.Count());
        }

        private static SyntaxTree Parse(string source)
        {
            return CSharpSyntaxTree.ParseText(source);
        }
    }

    public class Class
    {
        private List<Method> methods = new List<Method>();

        public Class(ClassDeclarationSyntax classDeclarationSyntax)
        {
            methods.AddRange(classDeclarationSyntax.DescendantNodes().OfType<MethodDeclarationSyntax>().Select(m=>new Method(m)));
        }

        public string Name => "Foo";
        public int LOC => 1;
        public IEnumerable<Method> Methods => methods;
    }

    public class Method
    {
        public Method(MethodDeclarationSyntax methodDeclarationSyntax)
        {
        }
    }
}
