using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Agents.Net.Designer.ViewModel;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using Microsoft.Win32;

namespace Agents.Net.Designer.View
{
    public partial class MainWindow : IDisposable
    {
        public GraphViewer GraphViewer { get; } = new GraphViewer();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            SetBinding(GraphProperty, "Graph");
        }

        public static readonly DependencyProperty GraphProperty =
            DependencyProperty.Register(
                "Graph", typeof(Graph),
                typeof(MainWindow), 
                new FrameworkPropertyMetadata(GraphChangedCallback)
            );

        private static void GraphChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MainWindow)d).GraphViewer.Graph = (Graph) e.NewValue;
        }

        public Graph Graph
        {
            get => (Graph)GetValue(GraphProperty);
            set => SetValue(GraphProperty, value);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            GraphViewer.BindToPanel(GraphViewerPanel);
        }

        private void AddAgentOnClick(object sender, RoutedEventArgs e)
        {
            OnAddAgentClicked();
        }

        private void AddMessageOnClick(object sender, RoutedEventArgs e)
        {
            OnAddMessageClicked();
        }

        private void ConnectFileOnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Agents Model File (*.amodel)|*.amodel", 
                RestoreDirectory = true,
                CheckFileExists = false
            };
            if (openFileDialog.ShowDialog(this) == true)
            {
                OnConnectFileClicked(new ConnectFileArgs(openFileDialog.FileName));
            }
        }

        public event EventHandler<EventArgs> AddMessageClicked; 
        public event EventHandler<EventArgs> AddAgentClicked;
        public event EventHandler<ConnectFileArgs> ConnectFileClicked;

        protected virtual void OnAddMessageClicked()
        {
            AddMessageClicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnAddAgentClicked()
        {
            AddAgentClicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnConnectFileClicked(ConnectFileArgs e)
        {
            ConnectFileClicked?.Invoke(this, e);
        }

        public void Dispose()
        {
            Loaded -= OnLoaded;
        }
    }
}