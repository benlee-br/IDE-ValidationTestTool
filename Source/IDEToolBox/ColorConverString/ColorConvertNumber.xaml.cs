using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IDEToolBox.ColorConverString
{
    /// <summary>
    /// Interaction logic for ColorConvertNumber.xaml
    /// </summary>
    public partial class ColorConvertNumber : UserControl
    {
        public ColorConvertNumber()
        {
            InitializeComponent();
            DataContext = _colorConverStringViewModel;
        }
        ColorConverStringViewModel _colorConverStringViewModel = new ColorConverStringViewModel();

        private void ConvertButtonClicked(object sender, RoutedEventArgs e)
        {
            _colorConverStringViewModel.ReserseConvertNotationString(ColorNotation.Text);
        }
    }
}
