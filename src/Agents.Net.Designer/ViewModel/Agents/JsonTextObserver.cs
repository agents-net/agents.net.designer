﻿using System;
using System.ComponentModel;
using Agents.Net;
using Agents.Net.Designer.Json.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(JsonViewModelCreated))]
    [Produces(typeof(JsonModelSourceChanged))]
    public class JsonTextObserver : Agent, IDisposable
    {        public JsonTextObserver(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        private JsonViewModelCreated viewModelCreated;
        private readonly object syncRoot = new object();

        protected override void ExecuteCore(Message messageData)
        {
            JsonViewModelCreated created = messageData.Get<JsonViewModelCreated>();
            lock (syncRoot)
            {
                if (viewModelCreated != null)
                {
                    viewModelCreated.ViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
                }

                viewModelCreated = created;
                viewModelCreated.ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
            }
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnMessage(new JsonModelSourceChanged(viewModelCreated.ViewModel.Text, viewModelCreated));
        }

        public void Dispose()
        {
            lock (syncRoot)
            {
                if (viewModelCreated != null)
                {
                    viewModelCreated.ViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
                }

                viewModelCreated = null;
            }
        }
    }
}
