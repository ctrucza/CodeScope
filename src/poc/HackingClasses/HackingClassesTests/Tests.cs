using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
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
            Assert.AreEqual(14, c.LOC);
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

        [TestCase("class Foo{}", 1)]
        [TestCase(@"class Foo{
            }", 2)]
        [TestCase(@"
            class Foo{
            }", 3)]
        public void test_class_loc_counted_correctly(string source, int lines)
        {
            var syntaxTree = Parse(source);
            var classDeclaration = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
            Class c = new Class(classDeclaration);

            Assert.AreEqual(lines, c.LOC);
        }

        private static SyntaxTree Parse(string source)
        {
            return CSharpSyntaxTree.ParseText(source);
        }
    }

    public class Class
    {
        private string name;
        private IEnumerable<Method> methods;
        private int loc;

        public Class(ClassDeclarationSyntax classDeclaration)
        {
            name = classDeclaration.Identifier.ToString();
            methods = classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>().Select(
                m=>new Method(m)
                );
            loc = classDeclaration.GetText().Lines.Count;
        }

        public string Name => name;
        public int LOC => loc;
        public IEnumerable<Method> Methods => methods;
    }

    public class Method
    {
        public Method(MethodDeclarationSyntax methodDeclarationSyntax)
        {
        }
    }
}
