using System;
using System.Collections.Generic;
using System.Text;

namespace Agents.Net.Designer.View
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
