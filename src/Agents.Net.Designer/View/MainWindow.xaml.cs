using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Agents.Net.Designer.ViewModel;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;

namespace Agents.Net.Designer.View
{
    public partial class MainWindow : IDisposable
    {
        private readonly GraphViewer graphViewer = new GraphViewer();

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
            ((MainWindow)d).graphViewer.Graph = (Graph) e.NewValue;
        }

        public Graph Graph
        {
            get => (Graph)GetValue(GraphProperty);
            set => SetValue(GraphProperty, value);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            graphViewer.BindToPanel(GraphViewerPanel);
        }

        public void Dispose()
        {
            Loaded -= OnLoaded;
        }
    }
}