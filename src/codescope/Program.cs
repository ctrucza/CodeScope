using System;
using System.Configuration;
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
            CodeScopeConfiguration configuration = new CodeScopeConfiguration(args, ConfigurationManager.AppSettings);

            string solutionFileName = configuration.SolutionFileName();
            string projectFileName = configuration.ProjectFileName();

            IWorkspace workspace;
            if (!string.IsNullOrEmpty(solutionFileName))
            {
                workspace = Workspace.LoadSolution(solutionFileName);
            }
            else if (!string.IsNullOrEmpty(projectFileName))
            {
                workspace = Workspace.LoadStandAloneProject(projectFileName);
            }
            else
            {
                Console.WriteLine("A project or solution was not specified");
                return;
            }

            ISolution solution = workspace.CurrentSolution;

            int projectCount = 0;
            int fileCount = 0;
            int classCount = 0;

            foreach (IProject project in solution.Projects)
            {
                projectCount++;
                foreach (IDocument document in project.Documents)
                {
                    fileCount++;
                    CommonSyntaxTree tree= document.GetSyntaxTree();
                    CommonSyntaxNode node = tree.GetRoot();

                    // BaseTypeDeclarationSyntax: classes, interfaces, enums
                    // TypeDeclarationSyntax: classes, interfaces

                    // ClassDeclarationSyntax
                    // InterfaceDeclarationSyntax
                    // EnumDeclarationSyntax
                        
                    var types = node.DescendantNodes().OfType<ClassDeclarationSyntax>();
                    foreach (var type in types)
                    {
                        classCount++;
                        //Console.WriteLine(classDeclaration.Modifiers);
                        //Console.WriteLine(classDeclaration.Keyword);
                        //Console.WriteLine(type.Identifier);
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("Summary:");
            Console.WriteLine("Projects: {0}", projectCount);
            Console.WriteLine("Files: {0}", fileCount);
            Console.WriteLine("Classes: {0}", classCount);
        }
    }
}
