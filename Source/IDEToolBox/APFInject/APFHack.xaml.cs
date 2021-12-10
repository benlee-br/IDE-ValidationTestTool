using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using IDEToolBox.APFInject;

namespace IDEToolBox
{
    /// <summary>
    /// Interaction logic for APFHack.xaml
    /// </summary>
    public partial class APFHackControl : System.Windows.Controls.UserControl
    {
        public APFHackControl()
        {
            InitializeComponent();
            DataContext = _injectViewModel;
        }
        APFInjectViewModel _injectViewModel = new APFInjectViewModel();
        private void BrowseButtonClicked(object sender, RoutedEventArgs e)
        {
            var dgl = new System.Windows.Forms.FolderBrowserDialog();
            dgl.SelectedPath = _injectViewModel.SourcePath;
            if (dgl.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _injectViewModel.SourcePath = dgl.SelectedPath;
                _injectViewModel.ResultOutputPath = System.IO.Path.Combine(dgl.SelectedPath, "Output");
                if (Directory.Exists(_injectViewModel.ResultOutputPath)) 
                {
                    Directory.CreateDirectory(_injectViewModel.ResultOutputPath);
                }
            }
        }
        private void ShowMenuDrop(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var injectViewModel = DataContext as APFInjectViewModel;
            MenuItem contextMenuItem = (MenuItem)sender;

            switch (contextMenuItem.Name)
            {
                case "GotoOutputFolder":
                    if (Directory.Exists(injectViewModel.ResultOutputPath))
                        Process.Start(injectViewModel.ResultOutputPath);
                    else
                        MessageBox.Show($"Folder ({injectViewModel.ResultOutputPath}) not found.", "IDE Test Tool");
                    break;

            }
        }
    }
}
