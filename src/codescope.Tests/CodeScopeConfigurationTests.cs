using System.Configuration;
using System.IO;
using NUnit.Framework;

namespace codescope.Tests
{
    [TestFixture]
    public class CodeScopeConfigurationTests
    {
        private readonly string[] emptyArgs = new string[]{};
        private readonly string currentFolder = Directory.GetCurrentDirectory();

        [Test]
        public void Configuration_WithRootInConfigFile_ReadsRootFromConfigFile()
        {
            ConfigurationManager.AppSettings["root"] = @"c:\somePath";
            ConfigurationManager.AppSettings["solution"] = "A.sln";

            CodeScopeConfiguration config = CreateConfig(emptyArgs);
            
            Assert.AreEqual(@"c:\somePath\A.sln", config.SolutionFileName());
        }

        [Test]
        public void Configuration_WithRootInConfigFileAndCommandLine_ReadsRootFromCommandLine()
        {
            ConfigurationManager.AppSettings["root"] = @"c:\somePath";
            ConfigurationManager.AppSettings["solution"] = "A.sln";
            string[] args = new[]
                {
                    "-root", @"c:\someOtherPath",
                };

            CodeScopeConfiguration config = CreateConfig(args);

            Assert.AreEqual(@"c:\someOtherPath\A.sln", config.SolutionFileName());
        }

        [Test]
        public void Configuration_WithRootNotConfigured_SetsRootToCurrentFolder()
        {
            ConfigurationManager.AppSettings["root"] = "";
            ConfigurationManager.AppSettings["solution"] = "A.sln";

            CodeScopeConfiguration config = CreateConfig(emptyArgs);

            string expected = Path.Combine(currentFolder, "A.sln");
            Assert.AreEqual(expected, config.SolutionFileName());
        }

        [Test]
        public void Configuration_WithRootSetToRelativeFolder_ConvertsSolutionFileNameToAbsolute()
        {
            ConfigurationManager.AppSettings["root"] = "solutions";
            ConfigurationManager.AppSettings["solution"] = "A.sln";

            CodeScopeConfiguration config = CreateConfig(emptyArgs);

            string expected = Path.Combine(currentFolder, @"solutions\A.sln");
            Assert.AreEqual(expected, config.SolutionFileName());
        }

        [Test]
        public void Configuration_WithSolutionSetInConfig_SetsSolutionFileCorrectly()
        {
            ConfigurationManager.AppSettings["root"] = @"c:\somePath";
            ConfigurationManager.AppSettings["solution"] = "B.sln";

            CodeScopeConfiguration config = CreateConfig(emptyArgs);

            Assert.AreEqual(@"c:\somePath\B.sln", config.SolutionFileName());
        }

        [Test]
        public void Configuration_WithSolutionPassedInCommandLine_OverridesTheConfiguration()
        {
            ConfigurationManager.AppSettings["root"] = @"c:\somePath";
            ConfigurationManager.AppSettings["solution"] = "A.sln";
            string[] args = new []
                {
                    "-solution", "B.sln"
                };

            CodeScopeConfiguration config = CreateConfig(args);

            Assert.AreEqual(@"c:\somePath\B.sln", config.SolutionFileName());
        }

        private static CodeScopeConfiguration CreateConfig(string[] args)
        {
            return new CodeScopeConfiguration(args, ConfigurationManager.AppSettings);
        }
    }
}
