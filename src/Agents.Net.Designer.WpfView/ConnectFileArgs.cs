#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

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
