using System;
using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.View.Messages
{
    public class MainWindowCreated : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition MainWindowCreatedDefinition { get; } =
            new MessageDefinition(nameof(MainWindowCreated));

        #endregion

        public MainWindowCreated(MainWindow window, params Message[] childMessages)
            : base(Array.Empty<Message>(), MainWindowCreatedDefinition, childMessages)
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
