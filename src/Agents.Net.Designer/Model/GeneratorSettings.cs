using System;
using System.Collections.Generic;
using System.Text;

namespace Agents.Net.Designer.Model
{
    public class GeneratorSettings
    {
        public string PackageNamespace { get; set; }

        public bool GenerateAutofacModule { get; set; }

        public GeneratorSettings Clone()
        {
            return new GeneratorSettings
            {
                PackageNamespace = PackageNamespace,
                GenerateAutofacModule = GenerateAutofacModule
            };
        }
    }
}
