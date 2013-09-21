using System.Collections.Generic;
using System.Linq;

namespace codescope
{
    public class CommandLine
    {
        private readonly List<string> parameters = new List<string>();
        private readonly List<string> switches = new List<string>();
        private readonly Dictionary<string, string> options = new Dictionary<string, string>();

        private readonly string[] args;
        private int index;

        public CommandLine(string[] args)
        {
            this.args = args;
            Parse();
        }

        public IEnumerable<string> Parameters()
        {
            return parameters;
        }

        public bool IsOn(string switchName)
        {
            return switches.Contains(GetOptionName(switchName));
        }

        public string Option(string optionName)
        {
            string key = GetOptionName(optionName);
            return options.ContainsKey(key) ? options[key] : "";
        }

        private static string GetOptionName(string name)
        {
            return "-" + name;
        }

        private void Parse()
        {
            index = 0;
            while (index < args.Count())
            {
                ParseNext();
            }
        }

        private void ParseNext()
        {
            string name = GetNext();

            if (IsParameter(name))
            {
                parameters.Add(name);
                return;
            }

            if (IsSwitch())
            {
                switches.Add(name);
                return;
            }

            string value = GetNext();
            options[name] = value;
        }

        private bool IsSwitch()
        {
            string next = PeekNext();
            return (NoMoreArguments() || IsOption(next));
        }

        private static bool IsParameter(string name)
        {
            return !IsOption(name);
        }

        private static bool IsOption(string name)
        {
            return name.StartsWith("-");
        }

        private string PeekNext()
        {
            if (NoMoreArguments())
            {
                return "";
            }

            return args[index];
        }

        private bool NoMoreArguments()
        {
            return index >= args.Count();
        }

        private string GetNext()
        {
            string result = args[index];
            index++;
            return result;
        }
    }
}