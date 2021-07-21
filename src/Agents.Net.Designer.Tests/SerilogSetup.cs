#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

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