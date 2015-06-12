using System;
using System.Linq;
using Microsoft.CodeAnalysis.MSBuild;

namespace CountClasses
{
    public class Solution
    {
        private readonly Microsoft.CodeAnalysis.Solution solution;
        private readonly string name;
        private int projectCount;
        private Statistics stat;

        public Solution(string solutionFileName)
        {
            var workspace = MSBuildWorkspace.Create();
            Console.WriteLine("opening solution {0}", solutionFileName);
            solution = workspace.OpenSolutionAsync(solutionFileName).Result;
            name = solutionFileName;
            Analyze();

        }

        private void Analyze()
        {
            var projects = solution.Projects.ToList();
            projectCount = projects.Count;
            stat = new Statistics();

            foreach (var p in projects.Select(project => new Project(project)))
            {
                Console.WriteLine(p);
                stat = p.Accumulate(stat);

                p.Inspect(solution);
            }
        }

        public override string ToString()
        {
            return string.Format(
                "solution: {0}\nprojects: {1}\n{2}",
                name,
                projectCount,
                stat
                );
        }
    }
}