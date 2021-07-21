#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

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
