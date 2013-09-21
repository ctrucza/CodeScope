using System.Collections.Specialized;
using System.Configuration;
using System.IO;

namespace codescope
{
    public class CodeScopeConfiguration
    {
        private readonly string root;
        private readonly string solution;

        private readonly CommandLine commandLine;
        private readonly NameValueCollection configuration;

        public CodeScopeConfiguration(string[] args, NameValueCollection configuration)
        {
            commandLine = new CommandLine(args);
            this.configuration = configuration;

            root = ReadConfigurationFor("root", Directory.GetCurrentDirectory());
            solution = ReadConfigurationFor("solution", "");
        }

        public string SolutionFileName()
        {
            string solutionFileName = Path.Combine(root, solution);
            return Path.GetFullPath(solutionFileName);
        }

        private string ReadConfigurationFor(string name, string defaultValue)
        {
            string valueFromConfigFile = GetValueFromConfigFile(name);
            string valueFromCommandLine = GetValueFromCommandLine(name);

            string result = valueFromConfigFile;

            if (valueFromCommandLine != "")
                result = valueFromCommandLine;
            
            if (result == "")
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
    }
}