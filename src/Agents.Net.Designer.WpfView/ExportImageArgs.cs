using System;

namespace Agents.Net.Designer.WpfView
{
    public class ExportImageArgs : EventArgs
    {
        public ExportImageArgs(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }
}
