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
        public void Configuration_RootSetInConfigFile_ReadsRootFromConfigFile()
        {
            ConfigurationManager.AppSettings["root"] = @"c:\somePath";
            ConfigurationManager.AppSettings["solution"] = "A.sln";

            CodeScopeConfiguration config = CreateConfig(emptyArgs);
            
            Assert.AreEqual(@"c:\somePath\A.sln", config.SolutionFileName());
        }

        [Test]
        public void Configuration_RootSetInCommandLine_OverridesConfiguration()
        {
            ConfigurationManager.AppSettings["root"] = @"c:\somePath";
            ConfigurationManager.AppSettings["solution"] = "A.sln";
            string[] args = new[]
                {
                    "-root", @"c:\someOtherPath"
                };

            CodeScopeConfiguration config = CreateConfig(args);

            Assert.AreEqual(@"c:\someOtherPath\A.sln", config.SolutionFileName());
        }

        [Test]
        public void Configuration_RootNotSet_SetsRootToCurrentFolder()
        {
            ConfigurationManager.AppSettings["root"] = "";
            ConfigurationManager.AppSettings["solution"] = "A.sln";

            CodeScopeConfiguration config = CreateConfig(emptyArgs);

            string expected = Path.Combine(currentFolder, "A.sln");
            Assert.AreEqual(expected, config.SolutionFileName());
        }

        [Test]
        public void Configuration_RootSetToRelativeFolder_ConvertsSolutionFileNameToAbsolute()
        {
            ConfigurationManager.AppSettings["root"] = "solutions";
            ConfigurationManager.AppSettings["solution"] = "A.sln";

            CodeScopeConfiguration config = CreateConfig(emptyArgs);

            string expected = Path.Combine(currentFolder, @"solutions\A.sln");
            Assert.AreEqual(expected, config.SolutionFileName());
        }

        [Test]
        public void Configuration_SolutionSetInConfigFile_SetsSolutionFileCorrectly()
        {
            ConfigurationManager.AppSettings["root"] = @"c:\somePath";
            ConfigurationManager.AppSettings["solution"] = "B.sln";

            CodeScopeConfiguration config = CreateConfig(emptyArgs);

            Assert.AreEqual(@"c:\somePath\B.sln", config.SolutionFileName());
        }

        [Test]
        public void Configuration_SolutionSetInCommandLine_OverridesConfiguration()
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

        [Test]
        public void Configuration_SolutionNotSet_ReturnsEmptySolutionFileName()
        {
            ConfigurationManager.AppSettings["solution"] = "";
            CodeScopeConfiguration config = CreateConfig(emptyArgs);
            Assert.AreEqual("", config.SolutionFileName());
        }

        [Test]
        public void Configuration_ProjectSetInConfigFile_SetsProjectFileCorrectly()
        {
            ConfigurationManager.AppSettings["root"] = @"c:\somePath";
            ConfigurationManager.AppSettings["project"] = "someProject";

            CodeScopeConfiguration config = CreateConfig(emptyArgs);

            Assert.AreEqual(@"c:\somePath\someProject", config.ProjectFileName());
        }

        [Test]
        public void Configuration_ProjectSetInCommandLine_SetsProjectFileCorrectly()
        {
            ConfigurationManager.AppSettings["root"] = @"c:\somePath";
            string[] args = new []
                {
                    "-project", "aProject"
                };

            CodeScopeConfiguration config = CreateConfig(args);

            Assert.AreEqual(@"c:\somePath\aProject", config.ProjectFileName());
        }

        [Test]
        public void Configuration_ProjectSetInCommandLine_OverridesConfiguration()
        {
            ConfigurationManager.AppSettings["root"] = @"c:\somePath";
            ConfigurationManager.AppSettings["project"] = "someProject";

            string[] args = new[]
                {
                    "-project", "aProject"
                };

            CodeScopeConfiguration config = CreateConfig(args);

            Assert.AreEqual(@"c:\somePath\aProject", config.ProjectFileName());
        }

        [Test]
        public void Configuration_ProjectNotSet_ReturnsEmptyProjectFileName()
        {
            ConfigurationManager.AppSettings["project"] = "";
            CodeScopeConfiguration config = CreateConfig(emptyArgs);
            Assert.AreEqual("", config.ProjectFileName());
        }


        private static CodeScopeConfiguration CreateConfig(string[] args)
        {
            return new CodeScopeConfiguration(args, ConfigurationManager.AppSettings);
        }
    }
}
