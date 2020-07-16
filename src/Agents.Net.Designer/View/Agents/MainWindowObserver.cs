﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using Agents.Net;
using Agents.Net.Designer.Generator.Messages;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.View.Messages;
using Agents.Net.Designer.ViewModel.Messages;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.Designer.View.Agents
{
    public class MainWindowObserver : Agent, IDisposable
    {
        #region Definition

        [AgentDefinition]
        public static AgentDefinition MainWindowObserverDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      MainWindowCreated.MainWindowCreatedDefinition
                                  },
                                  new []
                                  {
                                      SelectedObjectChanged.SelectedObjectChangedDefinition,
                                      AddAgentRequested.AddAgentRequestedDefinition,
                                      AddGeneratorSettingsRequested.AddGeneratorSettingsRequestedDefinition,
                                      AddMessageRequested.AddMessageRequestedDefinition,
                                      ConnectFileRequested.ConnectFileRequestedDefinition,
                                      GenerateFilesRequested.GenerateFilesRequestedDefinition,
                                      ExportImageRequested.ExportImageRequestedDefinition
                                  });

        #endregion

        private List<IViewerObject> subscribedObjects = new List<IViewerObject>();
        private MainWindowCreated mainWindowCreated;

        public MainWindowObserver(IMessageBoard messageBoard, MainWindow mainWindow) : base(MainWindowObserverDefinition, messageBoard)
        {
        }

        private void GraphViewerOnGraphChanged(object? sender, EventArgs e)
        {
            UpdateSelectionChangedEvents();
        }

        private void UpdateSelectionChangedEvents()
        {
            foreach (IViewerObject subscribedObject in subscribedObjects)
            {
                subscribedObject.MarkedForDraggingEvent -= SubscribedObjectOnMarkedForDraggingEvent;
            }
            subscribedObjects = new List<IViewerObject>(mainWindowCreated.Window.GraphViewer.Entities);
            foreach (IViewerObject subscribedObject in subscribedObjects)
            {
                subscribedObject.MarkedForDraggingEvent += SubscribedObjectOnMarkedForDraggingEvent;
            }
        }

        private void SubscribedObjectOnMarkedForDraggingEvent(object? sender, EventArgs e)
        {
            if (mainWindowCreated != null && sender is IViewerObject viewerObject)
            {
                OnMessage(new SelectedObjectChanged(viewerObject.DrawingObject, mainWindowCreated));
            }
        }

        protected override void ExecuteCore(Message messageData)
        {
            mainWindowCreated = messageData.Get<MainWindowCreated>();
            UpdateSelectionChangedEvents();
            mainWindowCreated.Window.GraphViewer.GraphChanged += GraphViewerOnGraphChanged;
            mainWindowCreated.Window.AddAgentClicked += WindowOnAddAgentClicked;
            mainWindowCreated.Window.AddMessageClicked += WindowOnAddMessageClicked;
            mainWindowCreated.Window.ConnectFileClicked += WindowOnConnectFileClicked;
            mainWindowCreated.Window.GenerateClassesClicked += WindowOnGenerateClassesClicked;
            mainWindowCreated.Window.ExportImageClicked += WindowOnExportImageClicked;
            mainWindowCreated.Window.AddGeneratorSettingsClicked += WindowOnAddGeneratorSettingsClicked;
        }

        private void WindowOnAddGeneratorSettingsClicked(object? sender, EventArgs e)
        {
            OnMessage(new AddGeneratorSettingsRequested(mainWindowCreated));
        }

        private void WindowOnExportImageClicked(object? sender, ExportImageArgs e)
        {
            OnMessage(new ExportImageRequested(e.Path, mainWindowCreated));
        }

        private void WindowOnGenerateClassesClicked(object? sender, GenerateClassesArgs e)
        {
            OnMessage(new GenerateFilesRequested(e.BaseDirectory, mainWindowCreated));
        }

        private void WindowOnConnectFileClicked(object? sender, ConnectFileArgs e)
        {
            OnMessage(new ConnectFileRequested(e.FileName, mainWindowCreated));
        }

        private void WindowOnAddMessageClicked(object? sender, EventArgs e)
        {
            OnMessage(new AddMessageRequested(mainWindowCreated));
        }

        private void WindowOnAddAgentClicked(object? sender, EventArgs e)
        {
            OnMessage(new AddAgentRequested(mainWindowCreated));
        }

        public void Dispose()
        {
            mainWindowCreated.Window.GraphViewer.GraphChanged -= GraphViewerOnGraphChanged;
            foreach (IViewerObject subscribedObject in subscribedObjects)
            {
                subscribedObject.MarkedForDraggingEvent -= SubscribedObjectOnMarkedForDraggingEvent;
                mainWindowCreated.Window.AddAgentClicked -= WindowOnAddAgentClicked;
                mainWindowCreated.Window.AddMessageClicked -= WindowOnAddMessageClicked;
                mainWindowCreated.Window.ConnectFileClicked -= WindowOnConnectFileClicked;
                mainWindowCreated.Window.GenerateClassesClicked -= WindowOnGenerateClassesClicked;
                mainWindowCreated.Window.ExportImageClicked -= WindowOnExportImageClicked;
                mainWindowCreated.Window.AddGeneratorSettingsClicked -= WindowOnAddGeneratorSettingsClicked;
            }
        }
    }
}
