﻿using System;
using System.Collections.Generic;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;

namespace codescope
{
    class SolutionStatistics
    {
        private readonly List<ProjectStatistics> projectStatistics = new List<ProjectStatistics>();
        private readonly Dictionary<string, Collector> collectors = new Dictionary<string, Collector>();

        public SolutionStatistics(ISolution solution)
        {
            collectors["Classes"] = new Collector(typeof(ClassDeclarationSyntax));
            collectors["Methods"] = new Collector(typeof(MethodDeclarationSyntax));
            collectors["Interfaces"] = new Collector(typeof(InterfaceDeclarationSyntax));
            collectors["Enums"] = new Collector(typeof(EnumDeclarationSyntax));

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
                Console.WriteLine(collector.Value);

            }
        }
    }
}