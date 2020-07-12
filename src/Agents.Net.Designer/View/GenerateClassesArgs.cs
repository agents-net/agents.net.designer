using System;
using System.Collections.Generic;
using System.Text;

namespace Agents.Net.Designer.View
{
    public class GenerateClassesArgs : EventArgs
    {
        public GenerateClassesArgs(string baseDirectory)
        {
            BaseDirectory = baseDirectory;
        }

        public string BaseDirectory { get; }
    }
}
