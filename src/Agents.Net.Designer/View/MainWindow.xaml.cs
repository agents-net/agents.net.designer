using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Agents.Net.Designer.ViewModel;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using ModifierKeys = System.Windows.Input.ModifierKeys;

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

        protected override void OnKeyUp(KeyEventArgs e)
        {
            e.Handled = true;
            switch (e.Key)
            {
                case Key.O:
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        ConnectFile();
                    }
                    break;
                case Key.E:
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        ExportImage();
                    }
                    break;
                case Key.G:
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        GenerateClasses();
                    }
                    break;
                case Key.A:
                    if (Keyboard.Modifiers.HasFlag(ModifierKeys.Alt) && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                    {
                        OnAddAgentClicked();
                    }
                    break;
                case Key.M:
                    if (Keyboard.Modifiers.HasFlag(ModifierKeys.Alt) && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                    {
                        OnAddMessageClicked();
                    }
                    break;
                default:
                    e.Handled = false;
                    break;
            }
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
            ConnectFile();
        }

        private void ConnectFile()
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

        private void ExportImageOnClick(object sender, RoutedEventArgs e)
        {
            ExportImage();
        }

        private void ExportImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image (*.png)|*.png",
                RestoreDirectory = true,
                CheckFileExists = false,
            };
            if (openFileDialog.ShowDialog(this) == false)
            {
                return;
            }

            GraphViewer.DrawImage(openFileDialog.FileName);
        }

        private void GenerateClassesOnClick(object sender, RoutedEventArgs e)
        {
            GenerateClasses();
        }

        private void GenerateClasses()
        {
            VistaFolderBrowserDialog openFileDialog = new VistaFolderBrowserDialog
            {
                ShowNewFolderButton = true
            };
            if (openFileDialog.ShowDialog(this) == true)
            {
                OnGenerateClassesClicked(new GenerateClassesArgs(openFileDialog.SelectedPath));
            }
        }

        public event EventHandler<EventArgs> AddMessageClicked; 
        public event EventHandler<EventArgs> AddAgentClicked;
        public event EventHandler<ConnectFileArgs> ConnectFileClicked;
        public event EventHandler<GenerateClassesArgs> GenerateClassesClicked;

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

        protected virtual void OnGenerateClassesClicked(GenerateClassesArgs e)
        {
            GenerateClassesClicked?.Invoke(this, e);
        }

        public void Dispose()
        {
            Loaded -= OnLoaded;
        }
    }
}