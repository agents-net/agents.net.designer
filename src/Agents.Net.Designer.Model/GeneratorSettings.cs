#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace Agents.Net.Designer.Model
{
    public class GeneratorSettings
    {
        public GeneratorSettings(string packageNamespace = "", bool generateAutofacModule = false)
        {
            PackageNamespace = packageNamespace;
            GenerateAutofacModule = generateAutofacModule;
        }

        public string PackageNamespace { get; }

        public bool GenerateAutofacModule { get; }
    }
}
