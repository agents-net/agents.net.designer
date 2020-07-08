using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;

namespace MinWpfApp {
        public partial class MainWindow {
         public MainWindow() {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
             var graph = new Graph();

            graph.AddEdge("A", "B");
            graph.AddEdge("B", "C");
            graph.AddEdge("C", "A");
            graph.Attr.LayerDirection = LayerDirection.LR;
            graphControl.Graph = graph; 
            graphControl.MouseDoubleClick+=GraphControlOnMouseDoubleClick;
        }

        private void GraphControlOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {var graph = new Graph();

            graph.AddEdge("A", "B");
            graph.AddEdge("B", "C");
            graph.AddEdge("C", "A");
            graph.AddEdge("C", "D");
            graph.Attr.LayerDirection = LayerDirection.LR;
            graphControl.Graph = graph; 
        }
        }
}