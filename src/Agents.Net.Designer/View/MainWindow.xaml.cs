using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Agents.Net.Designer.ViewModel;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;

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

        public event EventHandler<EventArgs> AddMessageClicked; 
        public event EventHandler<EventArgs> AddAgentClicked;

        protected virtual void OnAddMessageClicked()
        {
            AddMessageClicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnAddAgentClicked()
        {
            AddAgentClicked?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            Loaded -= OnLoaded;
        }
    }
}