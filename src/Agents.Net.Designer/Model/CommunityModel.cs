using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agents.Net.Designer.Model
{
    public class CommunityModel
    {
        public AgentModel[] Agents { get; set; } = new AgentModel[0];

        public MessageModel[] Messages { get; set; } = new MessageModel[0];

        public CommunityModel Clone()
        {
            CommunityModel clone = new CommunityModel
            {
                Agents = Agents.Select(a => a.Clone()).ToArray(),
                Messages = Messages.Select(m => m.Clone()).ToArray()
            };
            return clone;
        }
    }
}
