using System;
using System.Collections.Generic;
using System.Text;

namespace Agents.Net.Designer.View
{
    public class ConnectFileArgs : EventArgs
    {
        public ConnectFileArgs(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; }
    }
}
