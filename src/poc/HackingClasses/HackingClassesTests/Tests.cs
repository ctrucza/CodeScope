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

        [TestCase("class Foo{}", "Foo")]
        [TestCase("class Bar{}", "Bar")]
        public void test_class_name_extracted(string source, string name)
        {
            var syntaxTree = Parse(source);
            var classDeclaration = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
            Class c = new Class(classDeclaration);

            Assert.AreEqual(name, c.Name);
        }

        private static SyntaxTree Parse(string source)
        {
            return CSharpSyntaxTree.ParseText(source);
        }
    }

    public class Class
    {
        private string name;
        private List<Method> methods = new List<Method>();

        public Class(ClassDeclarationSyntax classDeclarationSyntax)
        {
            name = classDeclarationSyntax.Identifier.ToString();
            methods.AddRange(classDeclarationSyntax.DescendantNodes().OfType<MethodDeclarationSyntax>().Select(m=>new Method(m)));
        }

        public string Name => name;
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
