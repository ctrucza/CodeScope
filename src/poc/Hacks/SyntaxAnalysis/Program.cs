using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;

namespace SyntaxAnalysis
{
    class Program
    {
        private const string root = @"..\..\";

        static void Main(string[] args)
        {
            const string solutionFilePath = root + @"..\Hacks.sln";

            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(solutionFilePath).Result;

            foreach (var project in solution.Projects)
            {
                foreach (var document in project.Documents)
                {
                    DumpDocument(document);
                }
            }
        }

        private static void DumpDocument(Document document)
        {
            var syntaxTree = document.GetSyntaxTreeAsync().Result;
            var roodNode = syntaxTree.GetRoot();
            var classDeclarations = roodNode.DescendantNodes().OfType<ClassDeclarationSyntax>();
            foreach (var classDeclaration in classDeclarations)
            {
                Console.WriteLine(classDeclaration.Identifier);
            }
        }


        //private static void DumpDocument(Document document)
        //{
        //    var syntaxTree = document.GetSyntaxTreeAsync().Result;
        //    DumpTree(syntaxTree.GetRoot(), "");
        //}

        //private static void DumpTree(SyntaxNode node, string prefix)
        //{
        //    Console.WriteLine("{0}({1}) {2}", prefix, node.CSharpKind(), node.GetText());
        //    node.ChildNodes().ToList().ForEach(n => DumpTree(n, prefix + "\t"));
        //}
    }
}
