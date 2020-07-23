using System;
using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.View.Messages
{
    public class MainWindowCreated : Message
    {        public MainWindowCreated(MainWindow window, params Message[] childMessages)
            : base(Array.Empty<Message>(), childMessages:childMessages)
        {
            Window = window;
        }

        public MainWindow Window { get; set; }

        protected override string DataToString()
        {
            return $"{nameof(Window)}: {Window}";
        }
    }
}
