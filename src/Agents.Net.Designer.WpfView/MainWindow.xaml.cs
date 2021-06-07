using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using ModifierKeys = System.Windows.Input.ModifierKeys;
using TreeViewItem = Agents.Net.Designer.ViewModel.TreeViewItem;

namespace Agents.Net.Designer.WpfView
{
    public partial class MainWindow : IDisposable
    {
        public GraphViewer GraphViewer { get; } = new();
        private readonly FieldInfo text;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            SetBinding(GraphProperty, "Graph");
            text = typeof(VNode).GetField("FrameworkElementOfNodeForLabel", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static readonly DependencyProperty GraphProperty =
            DependencyProperty.Register(
                "Graph", typeof(Graph),
                typeof(MainWindow), 
                new FrameworkPropertyMetadata(GraphChangedCallback)
            );

        private static void GraphChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //TODO Try fix Graph loading here?
            ((MainWindow)d).GraphViewer.Graph = (Graph) e.NewValue;
            ((MainWindow) d).PatchNodeLabelSize();
        }

        //For svg conversion
        private void PatchNodeLabelSize()
        {
            foreach (VNode node in GraphViewer.Entities.OfType<VNode>())
            {
                if (text?.GetValue(node) is FrameworkElement element)
                {
                    ((Node) node.DrawingObject).Label.Width = element.Width;
                    ((Node) node.DrawingObject).Label.Height = element.Height;
                }
            }
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
                case Key.I:
                    if (Keyboard.Modifiers.HasFlag(ModifierKeys.Alt) && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                    {
                        OnAddInterceptorAgentClicked();
                    }
                    break;
                case Key.D:
                    if (Keyboard.Modifiers.HasFlag(ModifierKeys.Alt) && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                    {
                        OnAddMessageDecoratorClicked();
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

        private void AddInterceptorAgentOnClick(object sender, RoutedEventArgs e)
        {
            OnAddInterceptorAgentClicked();
        }

        private void AddMessageOnClick(object sender, RoutedEventArgs e)
        {
            OnAddMessageClicked();
        }

        private void AddMessageDecoratorOnClick(object sender, RoutedEventArgs e)
        {
            OnAddMessageDecoratorClicked();
        }

        private void ConnectFileOnClick(object sender, RoutedEventArgs e)
        {
            ConnectFile();
        }

        private void ConnectFile()
        {
            OpenFileDialog openFileDialog = new()
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
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Image (*.svg)|*.svg",
                RestoreDirectory = true,
                CheckFileExists = false,
            };
            if (openFileDialog.ShowDialog(this) == true)
            {
                OnExportImageClicked(new ExportImageArgs(openFileDialog.FileName));
            }
        }

        private void GenerateClassesOnClick(object sender, RoutedEventArgs e)
        {
            GenerateClasses();
        }

        private void GenerateClasses()
        {
            VistaFolderBrowserDialog openFileDialog = new()
            {
                ShowNewFolderButton = true
            };
            if (openFileDialog.ShowDialog(this) == true)
            {
                OnGenerateClassesClicked(new GenerateClassesArgs(openFileDialog.SelectedPath));
            }
        }

        private void TreeViewOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            OnSelectedTreeViewItemChanged(new SelectedTreeViewItemChangedArgs(e.NewValue as TreeViewItem));
        }

        public event EventHandler<EventArgs> AddMessageClicked; 
        public event EventHandler<EventArgs> AddMessageDecoratorClicked; 
        public event EventHandler<EventArgs> AddAgentClicked;
        public event EventHandler<EventArgs> AddInterceptorAgentClicked;
        public event EventHandler<ConnectFileArgs> ConnectFileClicked;
        public event EventHandler<ExportImageArgs> ExportImageClicked;
        public event EventHandler<GenerateClassesArgs> GenerateClassesClicked;
        public event EventHandler<SelectedTreeViewItemChangedArgs> SelectedTreeViewItemChanged; 

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

        protected virtual void OnExportImageClicked(ExportImageArgs e)
        {
            ExportImageClicked?.Invoke(this, e);
        }

        protected virtual void OnSelectedTreeViewItemChanged(SelectedTreeViewItemChangedArgs e)
        {
            SelectedTreeViewItemChanged?.Invoke(this, e);
        }

        public void Dispose()
        {
            Loaded -= OnLoaded;
        }

        protected virtual void OnAddInterceptorAgentClicked()
        {
            AddInterceptorAgentClicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnAddMessageDecoratorClicked()
        {
            AddMessageDecoratorClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}