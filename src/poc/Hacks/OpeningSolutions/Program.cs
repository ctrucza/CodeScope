using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;

namespace OpeningSolutions
{
    public class Program
    {
        private const string root = @"..\..\";

        private static void Main(string[] args)
        {
            //OpenFile();
            //OpenProject();
            OpenSolution();
        }

        private static void OpenSolution()
        {
            const string solutionFilePath = root + @"..\Hacks.sln";

            var workspace = MSBuildWorkspace.Create();
            Solution solution = workspace.OpenSolutionAsync(solutionFilePath).Result;

            foreach (var project in solution.Projects)
            {
                DumpProject(project);
            }
        }

        private static void OpenProject()
        {
            const string projectFilePath = root + @"OpeningSolutions.csproj";
            var workspace = MSBuildWorkspace.Create();
            Project project = workspace.OpenProjectAsync(projectFilePath).Result;
            DumpProject(project);
        }

        private static void OpenFile()
        {
            const string codeFilePath = root + @"Program.cs";
            var code = new StreamReader(codeFilePath).ReadToEnd();
            var tree = CSharpSyntaxTree.ParseText(code);
            DumpTree(tree);
        }

        private static void DumpProject(Project project)
        {
            foreach (var document in project.Documents)
            {
                var tree = document.GetSyntaxTreeAsync().Result;
                DumpTree(tree);
            }
        }

        private static void DumpTree(SyntaxTree tree)
        {
            foreach (TextLine line in tree.GetText().Lines)
            {
                Console.WriteLine(line);
            }
        }
    }
}
