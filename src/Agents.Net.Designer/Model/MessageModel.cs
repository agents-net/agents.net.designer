using System;
using System.Collections.Generic;
using System.Text;

namespace Agents.Net.Designer.Model
{
    public class MessageModel
    {
        public MessageModel(string name = "", string ns = ".Messages")
        {
            Name = name;
            Namespace = ns;
        }

        public string Name { get; }

        public string Namespace { get; }
    }
}
