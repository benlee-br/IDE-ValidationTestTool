using System;
using System.Collections.Generic;
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

        /// <summary>
        /// 
        /// </summary>
        public ColorConverStringViewModel()
        {
            RaisePropertyChangedEvent("ColorNumberString");
            ColorString = ConvetyColorSystem(_colorNumberString);
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
