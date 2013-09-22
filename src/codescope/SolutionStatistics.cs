using System;
using System.Collections.Generic;
using Roslyn.Services;

namespace codescope
{
    class SolutionStatistics
    {
        private readonly List<ProjectStatistics> projectStatistics = new List<ProjectStatistics>();
        private readonly Dictionary<string, CommonCollector> collectors = new Dictionary<string, CommonCollector>(); 
        
        private readonly ClassCollector classCollector = new ClassCollector();
        private readonly InterfaceCollector interfaceCollector = new InterfaceCollector();
        private readonly EnumCollector enumCollector = new EnumCollector();
        
        public SolutionStatistics(ISolution solution)
        {
            collectors["Classes"] = classCollector;
            collectors["Interfaces"] = interfaceCollector;
            collectors["Enums"] = enumCollector;

            // We should be able to create these generic collectors somehow...
            // Just not to create all those pesky oneliner classes

            //collectors["Classes (g)"] = new GenericCollector<ClassDeclarationSyntax>();
            //collectors["Interfaces (g)"] = new GenericCollector<InterfaceDeclarationSyntax>();
            //collectors["Enums (g)"] = new GenericCollector<EnumDeclarationSyntax>();

            foreach (IProject project in solution.Projects)
            {
                ProjectStatistics p = new ProjectStatistics(project);
                projectStatistics.Add(p);

                p.AddCollectors(collectors.Values);
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
                Console.WriteLine("{0}: {1}", collector.Key, collector.Value.Nodes.Count);
            }

            //Console.WriteLine("Classes: {0}", classCollector.Nodes.Count);
            //Console.WriteLine("Interfaces: {0}", interfaceCollector.Nodes.Count);
            //Console.WriteLine("Enums: {0}", enumCollector.Nodes.Count);
        }
    }
}