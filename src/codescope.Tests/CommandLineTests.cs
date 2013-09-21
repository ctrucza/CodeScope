using System.Linq;
using NUnit.Framework;

namespace codescope.Tests
{
    [TestFixture]
    public class CommandLineTests
    {
        [Test]
        public void CommandLine_WithOneParameter_HasOneParameter()
        {
            CommandLine cmd = new CommandLine(new []{"parameter"});
            CollectionAssert.Contains(cmd.GetParameters(), "parameter");
        }

        [Test]
        public void CommandLine_WithTwoParameters_HasTwoParameters()
        {
            CommandLine cmd = new CommandLine(new []{"firstParameter", "secondParameter"});
            Assert.AreEqual(2, cmd.GetParameters().Count());
            CollectionAssert.Contains(cmd.GetParameters(), "firstParameter");
            CollectionAssert.Contains(cmd.GetParameters(), "secondParameter");
        }

        [Test]
        public void CommandLine_WithOneSwitch_HasNoParameters()
        {
            CommandLine cmd = new CommandLine(new []{"-a"});
            Assert.AreEqual(0, cmd.GetParameters().Count());
        }

        [Test]
        public void CommandLine_WithOneSwitch_HasTheSwitchOn()
        {
            CommandLine cmd = new CommandLine(new[]{"-a"});
            Assert.IsTrue(cmd.GetSwitch("a"));
        }

        [Test]
        public void CommandLine_WithoutASwitch_HasTheSwitchOff()
        {
            CommandLine cmd = new CommandLine(new string[]{});
            Assert.IsFalse(cmd.GetSwitch("a"));
        }

        [Test]
        public void CommandLine_WithTwoSwitches_HasBothSwitchedOn()
        {
            CommandLine cmd = new CommandLine(new []{"-a", "-b"});
            Assert.IsTrue(cmd.GetSwitch("a"));
            Assert.IsTrue(cmd.GetSwitch("b"));
        }


        [Test]
        public void CommandLine_WithOneOption_HasTheOptionSet()
        {
            CommandLine cmd = new CommandLine(new []{"-option", "value"});
            Assert.AreEqual("value", cmd.GetOption("option"));
        }

        [Test]
        public void CommandLine_WithNoOptions_HasTheOptionEmpty()
        {
            CommandLine cmd = new CommandLine(new string[]{});
            Assert.AreEqual("", cmd.GetOption("option"));
        }

        [Test]
        public void CommandLine_WithAnOption_ConsumesTheOption()
        {
            CommandLine cmd = new CommandLine(new []{"-option", "value"});
            CollectionAssert.DoesNotContain(cmd.GetParameters(), "value");
        }
    }
}
