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
            string source = "class Foo {}";
            var syntaxTree = CSharpSyntaxTree.ParseText(source);
            var classDeclarationSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Single();

            Class c = new Class(classDeclarationSyntax);
            
            Assert.AreEqual("Foo", c.Name());
        }
    }

    public class Class
    {
        public Class(ClassDeclarationSyntax classDeclarationSyntax)
        {
        }

        public string Name()
        {
            return "Foo";
        }
    }
}
