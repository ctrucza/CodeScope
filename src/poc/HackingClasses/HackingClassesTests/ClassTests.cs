using System.Linq;
using HackingClasses;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace HackingClassesTests
{
    [TestFixture]
    public class ClassTests
    {
        [Test]
        public void usage()
        {
            const string source = @"
            class Foo
            {
                void Baz()
                {
            
                }
            }
            ";
            Class c = GetSingleClassFromSource(source);
            
            Assert.AreEqual("Foo", c.Name);
            Assert.AreEqual(9, c.LOC);
            Assert.AreEqual(1, c.Methods.Count());
        }

        [TestCase("class Foo{}", "Foo")]
        [TestCase("class Bar{}", "Bar")]
        public void name_is_read_from_the_source(string source, string name)
        {
            Class c = GetSingleClassFromSource(source);

            Assert.AreEqual(name, c.Name);
        }

        [TestCase("class Foo{}", 1)]
        [TestCase(@"class Foo{
            }", 2)]
        [TestCase(@"
            class Foo{
            }", 3)]
        public void loc_counted_correctly(string source, int lines)
        {
            var c = GetSingleClassFromSource(source);

            Assert.AreEqual(lines, c.LOC);
        }

        private static Class GetSingleClassFromSource(string source)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(source);
            var classDeclaration = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
            return new Class(classDeclaration);
        }
    }
}
