using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            FolderBrowserDialog dgl = new FolderBrowserDialog();
            dgl.SelectedPath = _injectViewModel.SourcePath;
            if (dgl.ShowDialog() == DialogResult.OK)
            {
                _injectViewModel.SourcePath = dgl.SelectedPath;
                _injectViewModel.ResultOutputPath = System.IO.Path.Combine(dgl.SelectedPath, "Output");
                if (Directory.Exists(_injectViewModel.ResultOutputPath)) 
                {
                    Directory.CreateDirectory(_injectViewModel.ResultOutputPath);
                }


            }

        }
    }
}
