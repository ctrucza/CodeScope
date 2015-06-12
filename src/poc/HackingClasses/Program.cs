using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace HackingClasses
{
    class Program
    {
        static void Main(string[] args)
        {
            string solutionFileName = args[0];

            var solution = OpenSolution(solutionFileName);

            foreach (var project in solution.Projects)
            {
            }
        }

        private static Solution OpenSolution(string solutionFileName)
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(solutionFileName).Result;
            return solution;
        }
    }
}
