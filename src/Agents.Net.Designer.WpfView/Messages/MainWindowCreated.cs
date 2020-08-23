﻿using System;

namespace Agents.Net.Designer.WpfView.Messages
{
    public class MainWindowCreated : Message
    {
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