using System;
using System.Collections.Generic;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;

namespace codescope
{
    class SolutionStatistics
    {
        private readonly List<ProjectStatistics> projectStatistics = new List<ProjectStatistics>();
        private readonly Dictionary<string, CommonCollector> collectors = new Dictionary<string, CommonCollector>();

        public SolutionStatistics(ISolution solution)
        {
            collectors["Classes"] = new CommonCollector(typeof(ClassDeclarationSyntax));
            collectors["Methods"] = new CommonCollector(typeof(MethodDeclarationSyntax));
            collectors["Interfaces"] = new CommonCollector(typeof(InterfaceDeclarationSyntax));
            collectors["Enums"] = new CommonCollector(typeof(EnumDeclarationSyntax));

            foreach (IProject project in solution.Projects)
            {
                ProjectStatistics p = new ProjectStatistics(project, collectors.Values);
                projectStatistics.Add(p);
            }
        }

        public void CollectStatistics()
        {
            foreach (ProjectStatistics p in projectStatistics)
            {
                p.CollectStatistics();
            }
        }

        public void ReportStatistics()
        {
            Console.WriteLine();
            Console.WriteLine("Summary:");
            Console.WriteLine("Projects: {0}", projectStatistics.Count);

            foreach (KeyValuePair<string, CommonCollector> collector in collectors)
            {
                string name = collector.Key;
                Console.WriteLine("{0}", name);
                Console.WriteLine(collector.Value);

            }
        }
    }
}