using System;

namespace Agents.Net.Designer.WpfView
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
