using System;
using System.Collections.Generic;
using System.Text;

namespace Agents.Net.Designer.Model
{
    public class MessageModel
    {
        public string Name { get; set; }

        public string Namespace { get; set; } = ".Messages";

        public MessageModel Clone()
        {
            return new MessageModel
            {
                Name = Name,
                Namespace = Namespace
            };
        }
    }
}
