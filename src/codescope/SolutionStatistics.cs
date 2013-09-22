using System;
using System.Collections.Generic;
using System.Linq;
using Roslyn.Services;

namespace codescope
{
    class SolutionStatistics
    {
        private readonly List<ProjectStatistics> projectStatistics = new List<ProjectStatistics>(); 
        
        public SolutionStatistics(ISolution solution)
        {
            foreach (IProject project in solution.Projects)
            {
                ProjectStatistics p = new ProjectStatistics(project);
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
            Console.WriteLine("Classes: {0}", ClassCount());
        }

        private int ClassCount()
        {
            return projectStatistics.Sum(p => p.ClassCount());
        }
    }
}