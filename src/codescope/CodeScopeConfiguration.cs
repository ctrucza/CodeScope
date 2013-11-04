using System.IO;
using System.Linq;

namespace codescope
{
    public class CodeScopeConfiguration
    {
        private readonly string solution;
        private readonly string project;

        private readonly CommandLine commandLine;
        private const string SolutionExtension = ".sln";
        private const string ProjectExtension = ".csproj";

        public CodeScopeConfiguration(string[] args)
        {
            commandLine = new CommandLine(args);

            solution = ReadConfigurationFor("solution", "");
            project = ReadConfigurationFor("project", "");

            if (string.IsNullOrEmpty(solution))
                solution = ReadFilePassedAsParameter(SolutionExtension);

            if (string.IsNullOrEmpty(project))
                project = ReadFilePassedAsParameter(ProjectExtension);
        }

        public string SolutionFileName()
        {
            return GetFullFileName(solution);
        }

        public string ProjectFileName()
        {
            return GetFullFileName(project);
        }

        private string GetFullFileName(string relativeFileName)
        {
            if (string.IsNullOrEmpty(relativeFileName))
                return "";
            string solutionFileName = Path.Combine(Directory.GetCurrentDirectory(), relativeFileName);
            return Path.GetFullPath(solutionFileName);
        }

        private string ReadConfigurationFor(string name, string defaultValue)
        {
            string result = GetValueFromCommandLine(name);

            if (string.IsNullOrEmpty(result))
                result = defaultValue;
            
            return result;
        }

        private string GetValueFromCommandLine(string name)
        {
            return commandLine.Option(name);
        }

        private string ReadFilePassedAsParameter(string extension)
        {
            string parameter = GetFirstParameter();
            if (!IsFileOfType(parameter, extension))
                return "";
            return parameter;
        }

        private string GetFirstParameter()
        {
            if (commandLine.Parameters().Any())
                return commandLine.Parameters().First();
            
            return "";
        }

        private bool IsFileOfType(string parameter, string extension)
        {
            return Path.GetExtension(parameter) == extension;
        }
    }
}