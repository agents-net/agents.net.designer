using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class ModificationResult : Message
    {
        public ModificationResult(CommunityModel model, Message predecessorMessage)
            : base(predecessorMessage)
        {
            Model = model;
        }

        public ModificationResult(CommunityModel model, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
        {
            Model = model;
        }

        public CommunityModel Model { get; }

        protected override string DataToString()
        {
            return $"{nameof(Model)}: {Model}";
        }
    }
}
