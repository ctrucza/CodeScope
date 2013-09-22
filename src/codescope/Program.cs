using System;
using System.Configuration;
using Roslyn.Services;

namespace codescope
{
    class Program
    {
        static void Main(string[] args)
        {
            CodeScopeConfiguration configuration = new CodeScopeConfiguration(args, ConfigurationManager.AppSettings);

            try
            {
                IWorkspace workspace = LoadWorkspace(configuration);
                ISolution solution = workspace.CurrentSolution;
                SolutionStatistics statistics = new SolutionStatistics(solution);
                statistics.CollectStatistics();
                statistics.ReportStatistics();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private static IWorkspace LoadWorkspace(CodeScopeConfiguration configuration)
        {
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
                throw new Exception("A project or solution was not specified");
            }
            return workspace;
        }
    }
}
