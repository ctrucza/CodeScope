﻿using System.IO;
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
            Class c = GetSingleClassFromFile("usage.cs");
            
            Assert.AreEqual("Foo", c.Name);
            Assert.AreEqual(8, c.LOC);
            Assert.AreEqual(1, c.Methods.Count());
        }

        [TestCase("name_Bar.cs", "Bar")]
        [TestCase("name_Baz.cs", "Baz")]
        public void name_is_read_from_the_source(string fileName, string name)
        {
            Class c = GetSingleClassFromFile(fileName);

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

        private static Class GetSingleClassFromFile(string fileName)
        {
            string test_data_path = @"..\..\test_data";
            var path = Path.Combine(test_data_path, fileName);
            var source = File.ReadAllText(path);
            return GetSingleClassFromSource(source);
        }

        private static Class GetSingleClassFromSource(string source)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(source);
            var classDeclaration = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
            return new Class(classDeclaration);
        }
    }
}
