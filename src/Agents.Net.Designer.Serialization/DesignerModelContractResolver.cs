using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agents.Net.Designer.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Agents.Net.Designer.Serialization
{
    internal class DesignerModelContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization)
                       .Where(p => p.PropertyType != typeof(CommunityModel))
                       .ToList();
        }
    }
}
