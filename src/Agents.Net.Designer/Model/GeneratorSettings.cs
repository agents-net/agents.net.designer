using System;
using System.Collections.Generic;
using System.Text;

namespace Agents.Net.Designer.Model
{
    public class GeneratorSettings
    {
        public GeneratorSettings(string packageNamespace = "Root", bool generateAutofacModule = false)
        {
            PackageNamespace = packageNamespace;
            GenerateAutofacModule = generateAutofacModule;
        }

        public string PackageNamespace { get; }

        public bool GenerateAutofacModule { get; }
    }
}
