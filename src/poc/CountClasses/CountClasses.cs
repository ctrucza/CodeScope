using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;

namespace CountClasses
{
    public class CountClasses
    {
        static int  Main(string[] arguments)
        {
            var workspace = MSBuildWorkspace.Create();

            string solutionFileName = arguments[0];
            var solution = workspace.OpenSolutionAsync(solutionFileName).Result;
            foreach (var project in solution.Projects)
            {
                foreach (var document in project.Documents)
                {
                    var root = document.GetSyntaxRootAsync().Result;
                    var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
                    foreach (var classDeclarationSyntax in classes)
                    {
                        Console.WriteLine(classDeclarationSyntax.Identifier);
                    }
                }
            }

            return 0;
        }
    }
}
