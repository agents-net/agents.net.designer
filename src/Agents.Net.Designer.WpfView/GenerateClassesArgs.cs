using System;

namespace Agents.Net.Designer.WpfView
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
