using NUnit.Framework;
using Serilog;

namespace Agents.Net.Designer.Tests
{
    [SetUpFixture]
    public class SerilogSetup
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Verbose()
                         .WriteTo.NUnitOutput()
                         .CreateLogger();
        }
    }
}