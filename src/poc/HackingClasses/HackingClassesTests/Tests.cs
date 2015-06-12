using System.Collections.Generic;
using System.Linq;
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
            const string source = @"
            class Foo
            {
                void Bar()
                {
            
                }

                void Baz()
                {
            
                }
            }
            ";
            Class c = GetClassFromSource(source);
            
            Assert.AreEqual("Foo", c.Name);
            Assert.AreEqual(14, c.LOC);
            Assert.AreEqual(2, c.Methods.Count());
        }

        [TestCase("class Foo{}", "Foo")]
        [TestCase("class Bar{}", "Bar")]
        public void test_class_name_extracted(string source, string name)
        {
            Class c = GetClassFromSource(source);

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
            var c = GetClassFromSource(source);

            Assert.AreEqual(lines, c.LOC);
        }

        private static Class GetClassFromSource(string source)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(source);
            var classDeclaration = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
            return new Class(classDeclaration);
        }
    }

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

    public class Method
    {
        public Method(MethodDeclarationSyntax methodDeclaration)
        {
        }
    }
}
