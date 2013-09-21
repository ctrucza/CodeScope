using System.Collections.Specialized;
using System.IO;
using System.Linq;

namespace codescope
{
    public class CodeScopeConfiguration
    {
        private readonly string root;
        private readonly string solution;
        private readonly string project;

        private readonly CommandLine commandLine;
        private readonly NameValueCollection configuration;
        private const string SolutionExtension = ".sln";
        private const string ProjectExtension = ".csproj";

        public CodeScopeConfiguration(string[] args, NameValueCollection configuration)
        {
            commandLine = new CommandLine(args);
            this.configuration = configuration;

            root = ReadConfigurationFor("root", Directory.GetCurrentDirectory());
            solution = ReadConfigurationFor("solution", "");
            project = ReadConfigurationFor("project", "");

            if (string.IsNullOrEmpty(solution))
                solution = ReadFileFromParameters(SolutionExtension);

            if (string.IsNullOrEmpty(project))
                project = ReadFileFromParameters(ProjectExtension);
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
            string solutionFileName = Path.Combine(root, relativeFileName);
            return Path.GetFullPath(solutionFileName);
        }

        private string ReadConfigurationFor(string name, string defaultValue)
        {
            string valueFromConfigFile = GetValueFromConfigFile(name);
            string valueFromCommandLine = GetValueFromCommandLine(name);

            string result = valueFromConfigFile;

            if (!string.IsNullOrEmpty(valueFromCommandLine))
                result = valueFromCommandLine;
            
            if (string.IsNullOrEmpty(result))
                result = defaultValue;
            
            return result;
        }

        private string GetValueFromConfigFile(string name)
        {
            return configuration[name];
        }

        private string GetValueFromCommandLine(string name)
        {
            return commandLine.Option(name);
        }

        private string ReadFileFromParameters(string extension)
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