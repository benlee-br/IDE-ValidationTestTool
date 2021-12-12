using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WpfTestApp.ViewModelBase;

namespace IDEToolBox.ColorConverString
{
    public class ColorConverStringViewModel : ObservableObject
    {
        private string _colorNumberString = "-8355585";
        private string _colorString = "#15871b";
        public Color SampleColor
        {
            get; set;
        }
        /// <summary>
        /// Apply Double Sigmoid Ct cut off
        /// </summary>
        public string ColorString
        {
            get { return _colorString; }
            set
            {
                _colorString = value;
                //Properties.Settings.Default.DoubleSigmoidCt = _colorString;
                RaisePropertyChangedEvent("ColorString");
            }
        }
        private ObservableCollection<String> _recentColors = new ObservableCollection<string>();
        public ObservableCollection<string> RecentColors
        {
            get { return _recentColors; }
            set
            {
                _recentColors = value;
                RaisePropertyChangedEvent("RecentColors");
            }
        }
        
        public string ColorNumberString
        {

            get { return _colorNumberString; }
            set
            {
                _colorNumberString = value;
                RaisePropertyChangedEvent("ColorNumberString");
            }
        }
        string _copyText = string.Empty;
        public string CopyText
        {

            get { return _copyText; }
            set
            {
                _copyText = value;
                RaisePropertyChangedEvent("CopyText");
            }
        }
        string[] colors = new string[] { "#b3a2d5", "#8fc5f9", "#2974c0", "#df3936", "#fecc29", "#f96681", "#f65224", "#7a2628", "#7a2628" };
        /// <summary>
        /// 
        /// </summary>
        public ColorConverStringViewModel()
        {
            RaisePropertyChangedEvent("ColorNumberString");
            ColorString = ConvetyColorSystem(_colorNumberString);
            foreach (var htmlColor in colors)
                _recentColors.Add(htmlColor);
        }
        #region Commands

        #endregion
        public void ReserseConvertNotationString(string colorHexString)
        {
            Color color = (Color)ColorConverter.ConvertFromString(colorHexString);

            var drawColor = System.Drawing.Color.FromArgb(color.R, color.G, color.B);
            ColorNumberString = drawColor.ToArgb().ToString();
            ColorString = ConvetyColorSystem(ColorNumberString);

            Clipboard.SetText(ColorNumberString);
            CopyText = $"Copied color number ({ColorNumberString}) to ClipBoard for Paste.";
        }


        private string ConvetyColorSystem(string colorNumberString)
        {
            var drawColor = System.Drawing.Color.FromArgb(int.Parse(colorNumberString));
            string colorHexString = "#15871b";
            if (drawColor != null)
            {
                colorHexString = HexConverter(drawColor);
            }
            return colorHexString;
        }


        private static String HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }
    }
}
