using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;

namespace codescope
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootFolder = Directory.GetCurrentDirectory();
            string testFolder = ConfigurationManager.AppSettings["sourceFolder"];
            testFolder = Path.Combine(rootFolder, testFolder);
            string solutionFolder = Path.Combine(testFolder, "helloworld");
            string solutionName = "helloworld.sln";
            string solutionFileName = Path.Combine(solutionFolder, solutionName);

            ISolution solution = Solution.Load(solutionFileName);

            foreach (IProject project in solution.Projects)
            {
                foreach (IDocument document in project.Documents)
                {
                    CommonSyntaxTree tree= document.GetSyntaxTree();
                    CommonSyntaxNode node = tree.GetRoot();

                    // BaseTypeDeclarationSyntax: classes, interfaces, enums
                    // TypeDeclarationSyntax: classes, interfaces

                    // ClassDeclarationSyntax
                    // InterfaceDeclarationSyntax
                    // EnumDeclarationSyntax
                        
                    var types = node.DescendantNodes().OfType<BaseTypeDeclarationSyntax>();
                    foreach (var type in types)
                    {
                        //Console.WriteLine(classDeclaration.Modifiers);
                        //Console.WriteLine(classDeclaration.Keyword);
                        Console.WriteLine(type.Identifier);
                    }
                }
            }
        }
    }
}
