#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;

namespace Agents.Net.Designer.WpfView.Messages
{
    public class MainWindowCreated : Message
    {
        public MainWindowCreated(MainWindow window)
            : base(Array.Empty<Message>())
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
