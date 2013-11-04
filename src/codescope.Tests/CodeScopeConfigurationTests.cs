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
        public void Configuration_SolutionSetInCommandLine_SetsSolutionFile()
        {
            string[] args = new []
                {
                    "-solution", "A.sln"
                };

            CodeScopeConfiguration config = CreateConfig(args);

            string expected = Path.Combine(currentFolder, "A.sln");
            Assert.AreEqual(expected, config.SolutionFileName());
        }

        [Test]
        public void Configuration_SolutionSetAsParameter_SetsSolutionFile()
        {
            string[] args = new[]
                {
                    "A.sln"
                };

            CodeScopeConfiguration config = CreateConfig(args);

            string expected = Path.Combine(currentFolder, "A.sln");
            Assert.AreEqual(expected, config.SolutionFileName());
        }

        [Test]
        public void Configuration_SolutionNotSet_ReturnsEmptySolutionFileName()
        {
            CodeScopeConfiguration config = CreateConfig(emptyArgs);
            Assert.AreEqual("", config.SolutionFileName());
        }

        [Test]
        public void Configuration_ProjectSetInCommandLine_SetsProjectFile()
        {
            string[] args = new []
                {
                    "-project", "A.csproj"
                };

            CodeScopeConfiguration config = CreateConfig(args);

            string expected = Path.Combine(currentFolder, "A.csproj");
            Assert.AreEqual(expected, config.ProjectFileName());
        }

        [Test]
        public void Configuration_ProjectSetAsParameter_SetsProjectFile()
        {
            string[] args = new[]
                {
                    "A.csproj"
                };

            CodeScopeConfiguration config = CreateConfig(args);

            string expected = Path.Combine(currentFolder, "A.csproj");
            Assert.AreEqual(expected, config.ProjectFileName());
        }

        [Test]
        public void Configuration_ProjectNotSet_ReturnsEmptyProjectFileName()
        {
            CodeScopeConfiguration config = CreateConfig(emptyArgs);
            Assert.AreEqual("", config.ProjectFileName());
        }


        private static CodeScopeConfiguration CreateConfig(string[] args)
        {
            return new CodeScopeConfiguration(args);
        }
    }
}
