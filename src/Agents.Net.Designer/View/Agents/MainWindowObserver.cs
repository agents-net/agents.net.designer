using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;
using Agents.Net;
using Agents.Net.Designer.Generator.Messages;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.View.Messages;
using Agents.Net.Designer.ViewModel.Messages;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.Designer.View.Agents
{
    //TODO Enable nullable for whole project
    [Consumes(typeof(MainWindowCreated))]
    [Produces(typeof(SelectedGraphObjectChanged))]
    [Produces(typeof(AddAgentRequested))]
    [Produces(typeof(AddMessageRequested))]
    [Produces(typeof(AddMessageDecoratorRequested))]
    [Produces(typeof(AddInterceptorAgentRequested))]
    [Produces(typeof(ConnectFileRequested))]
    [Produces(typeof(GenerateFilesRequested))]
    [Produces(typeof(ExportImageRequested))]
    [Produces(typeof(SelectedTreeViewItemChanged))]
    [Produces(typeof(DeleteItemRequested))]
    public class MainWindowObserver : Agent, IDisposable
    {
        private List<IViewerObject> subscribedObjects = new List<IViewerObject>();
        private MainWindowCreated mainWindowCreated;

        public MainWindowObserver(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        private void GraphViewerOnGraphChanged(object? sender, EventArgs e)
        {
            UpdateSelectionChangedEvents();
            OnMessage(new GraphViewModelUpdated(mainWindowCreated));
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
                OnMessage(new SelectedGraphObjectChanged(viewerObject.DrawingObject, mainWindowCreated));
            }
        }

        protected override void ExecuteCore(Message messageData)
        {
            mainWindowCreated = messageData.Get<MainWindowCreated>();
            UpdateSelectionChangedEvents();
            mainWindowCreated.Window.GraphViewer.GraphChanged += GraphViewerOnGraphChanged;
            mainWindowCreated.Window.AddAgentClicked += WindowOnAddAgentClicked;
            mainWindowCreated.Window.AddMessageClicked += WindowOnAddMessageClicked;
            mainWindowCreated.Window.AddMessageDecoratorClicked += WindowOnAddMessageDecoratorClicked;
            mainWindowCreated.Window.AddInterceptorAgentClicked += WindowOnAddInterceptorAgentClicked;
            mainWindowCreated.Window.ConnectFileClicked += WindowOnConnectFileClicked;
            mainWindowCreated.Window.GenerateClassesClicked += WindowOnGenerateClassesClicked;
            mainWindowCreated.Window.ExportImageClicked += WindowOnExportImageClicked;
            mainWindowCreated.Window.SelectedTreeViewItemChanged += WindowOnSelectedTreeViewItemChanged;
            mainWindowCreated.Window.TreeView.KeyDown += TreeViewOnKeyDown;
        }

        private void TreeViewOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete &&
                !e.IsRepeat)
            {
                OnMessage(new DeleteItemRequested(mainWindowCreated));
            }
        }

        private void WindowOnSelectedTreeViewItemChanged(object? sender, SelectedTreeViewItemChangedArgs e)
        {
            OnMessage(new SelectedTreeViewItemChanged(e.SelectedItem, mainWindowCreated));
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

        private void WindowOnAddInterceptorAgentClicked(object? sender, EventArgs e)
        {
            OnMessage(new AddInterceptorAgentRequested(mainWindowCreated));
        }

        private void WindowOnAddMessageDecoratorClicked(object? sender, EventArgs e)
        {
            OnMessage(new AddMessageDecoratorRequested(mainWindowCreated));
        }

        public void Dispose()
        {
            mainWindowCreated.Window.GraphViewer.GraphChanged -= GraphViewerOnGraphChanged;
            foreach (IViewerObject subscribedObject in subscribedObjects)
            {
                subscribedObject.MarkedForDraggingEvent -= SubscribedObjectOnMarkedForDraggingEvent;
                mainWindowCreated.Window.AddAgentClicked -= WindowOnAddAgentClicked;
                mainWindowCreated.Window.AddMessageClicked -= WindowOnAddMessageClicked;
                mainWindowCreated.Window.AddInterceptorAgentClicked -= WindowOnAddInterceptorAgentClicked;
                mainWindowCreated.Window.ConnectFileClicked -= WindowOnConnectFileClicked;
                mainWindowCreated.Window.GenerateClassesClicked -= WindowOnGenerateClassesClicked;
                mainWindowCreated.Window.ExportImageClicked -= WindowOnExportImageClicked;
                mainWindowCreated.Window.SelectedTreeViewItemChanged -= WindowOnSelectedTreeViewItemChanged;
                mainWindowCreated.Window.AddInterceptorAgentClicked -= WindowOnAddInterceptorAgentClicked;
                mainWindowCreated.Window.AddMessageDecoratorClicked -= WindowOnAddMessageDecoratorClicked;
            }
        }
    }
}
