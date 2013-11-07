using System;
using System.Collections.Generic;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;

namespace codescope
{
    class SolutionStatistics
    {
        private readonly List<ProjectStatistics> projectStatistics = new List<ProjectStatistics>();
        private readonly Dictionary<string, ICollector> collectors = new Dictionary<string, ICollector>();

        public SolutionStatistics(ISolution solution)
        {
            collectors["Classes"] = new Collector<ClassDeclarationSyntax, ClassWrapper>();
            //collectors["Methods"] = new Collector<MethodDeclarationSyntax, MethodWrapper>();
            collectors["Interfaces"] = new Collector<InterfaceDeclarationSyntax, NodeWrapper<InterfaceDeclarationSyntax>>();
            collectors["Enums"] = new Collector<EnumDeclarationSyntax, NodeWrapper<EnumDeclarationSyntax>>();

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

            foreach (var collector in collectors)
            {
                string name = collector.Key;
                Console.WriteLine("{0}", name);
                collector.Value.Report();

            }
        }

        public void Dump()
        {
            foreach (ICollector collector in collectors.Values)
            {
                collector.Dump();
            }
        }
    }
}