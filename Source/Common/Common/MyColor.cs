using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace BioRad.Common
{
    /// <summary>
    /// Named colors.
    /// </summary>
    public class NamedColors
    {
        int m_ColorsIndex = 0;

        static string[] m_NameObjectColors =
        {
            "Lime",
            "Purple","Green","Blue","Pink","Brown","Red",
            "LightBlue","Teal","Orange","LightGreen","Magenta","Yellow",
            "SkyBlue","Gray","LimeGreen","SandyBrown","Violet","DarkGreen",
            "Turquoise","Lavender","DarkBlue","Tan","Cyan","Gold",
            "ForestGreen","Fuchsia","Chocolate","DarkSeaGreen","Maroon","Olive",
            "Salmon","Beige","RoyalBlue","DarkRed","DarkOrange","DarkGray",
            "HotPink","OrangeRed","MediumBlue","DeepPink","CornflowerBlue","GreenYellow",
            "LightSlateGray","SeaGreen","LawnGreen","Indigo","DimGray","LightPink"
            //"Lime","Green","SkyBlue","Magenta",
            //"Blue","Yellow","LightYellow",
            //"Cyan","Red","OrangeRed","DeepPink","Fuchsia",
            //"HotPink","Tan","Pink","DarkOrange",
            //"SandyBrown","Orange","MediumBlue",
            //"RoyalBlue","CornflowerBlue","Gold","DodgerBlue","Aquav",
            //"DimGray","MidnightBlue","LightCyan","SteelBlue",
            //"LightSlateGray","PowderBlue","BlueViolet",
            //"SlateBlue","Gray","LightSteelBlue",
            //"DarkGray","DarkViolet","DarkGreen"
        };

        /// <summary>
        /// 
        /// </summary>
        static public string[] NameObjectColors
        {
            get
            {
                return m_NameObjectColors;
            }
        }
        /// <summary>
        /// Get next named color.
        /// </summary>
        /// <returns></returns>
        public Color Next()
        {
            Color color = Color.SpringGreen;
            m_ColorsIndex++;
            m_ColorsIndex = m_ColorsIndex % m_NameObjectColors.Length;
            color = Color.FromName(m_NameObjectColors[m_ColorsIndex]);
            return color;
        }
    }

    /// <summary>
    /// Create distinct random colors.
    /// </summary>
    public class RandomColors
    {
        int m_ColorsIndex = 0;

        List<Color> m_Colors = new List<Color>();
        
        /// <summary>
        /// Create distinct colors array. Exclude system colors.
        /// </summary>
        /// <param name="numColors">Number colors.</param>
        /// <param name="shuffle">true to shuffle color array</param>
        /// <param name="seed">random seed value.</param>
        public RandomColors(int numColors, bool shuffle, int seed)
        {
            m_ColorsIndex = 0;
            Random r = new Random(seed);
            int s = 100;// increase for lighter colors.
            int e = 240;// increase for darker colors.
            int a = 250;
            if (numColors < 10)
                numColors = 10;

            //Color mix = Color.Lime;
            //int red = r.Next(256);
            //int green = r.Next(256);
            //int blue = r.Next(256);

            //// mix the color
            //if (mix != null)
            //{
            //    red = (red + mix.R) / 2;
            //    green = (green + mix.G) / 2;
            //    blue = (blue + mix.B) / 2;
            //}
            //m_Colors.Add(Color.FromArgb(red, green, blue));


            for (int i = 0; i < numColors; i++)
            {
                Color c;
                int retry = 1000;
                do
                {
                    byte red = (byte)r.Next(s, e);
                    byte green = (byte)r.Next(s, e);
                    byte blue = (byte)r.Next(s, e);
                    c = Color.FromArgb(a, red, green, blue);
                } while (Exists(c) && c.IsSystemColor && retry-- > 0);

                m_Colors.Add(c);
            }

            if (shuffle)
                Shuffle(m_Colors);

#if DEBUG
            for (int i = 0; i < m_Colors.Count; i++)
            {
                for (int j = 0; j < m_Colors.Count; j++)
                {
                    if (i == j)
                        continue;
                    if (m_Colors[i] == m_Colors[j])
                        Debug.Assert(false);
                }
            }
#endif
        }
        bool Exists(Color c)
        {
            for (int i = 0; i < m_Colors.Count; i++)
            {
                if (m_Colors[i] == c)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Color GenerateRandomColor(int seed)
        {
            Color mix = Color.Lime;
            Random random = new Random(seed);
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);

            // mix the color
            if (mix != null)
            {
                red = (red + mix.R) / 2;
                green = (green + mix.G) / 2;
                blue = (blue + mix.B) / 2;
            }

            Color color = Color.FromArgb(red, green, blue);
            return color;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Color Next()
        {
            Color color = Color.SpringGreen;
            m_ColorsIndex++;
            m_ColorsIndex = m_ColorsIndex % m_Colors.Count;
            color = m_Colors[m_ColorsIndex];
            return color;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Shuffle()
        {
            Shuffle(m_Colors);
        }
        void Shuffle<T>(IList<T> list)
        {
            Random rng = new Random(9);//TT837
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        List<Color> GetGradientColors(Color start, Color end, int steps, int firstStep, int lastStep)
        {
            var colorList = new List<Color>();
            if (steps <= 0 || firstStep < 0 || lastStep > steps - 1)
                return colorList;

            double aStep = (end.A - start.A) / steps;
            double rStep = (end.R - start.R) / steps;
            double gStep = (end.G - start.G) / steps;
            double bStep = (end.B - start.B) / steps;

            for (int i = firstStep; i < lastStep; i++)
            {
                var a = start.A + (int)(aStep * i);
                var r = start.R + (int)(rStep * i);
                var g = start.G + (int)(gStep * i);
                var b = start.B + (int)(bStep * i);
                colorList.Add(Color.FromArgb(a, r, g, b));
            }

            return colorList;
        }
        Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }
    }
}
