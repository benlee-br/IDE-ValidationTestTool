using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Resources;

using BioRad.Common.Xml;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Global static methods.
	/// </summary>
	/// <remarks>
	/// A place to put those global static methods.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Ralph Ansell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href=""></see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: GlobalMethods.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Common/GlobalMethods.cs $</item>
	///			<item name="vssrevision">$Revision: 14 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Thouser $</item>
	///			<item name="vssdate">$Date: 2/15/11 3:33p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public static partial class GlobalMethods
	{
		#region Methods
		/// <summary>
		/// Creates a color which is an interpolation between two colors
		/// </summary>
		/// <param name="color1">first color to interpolate from</param>
		/// <param name="color2">second color to interpolate from</param>
		/// <param name="proportion">fraction which specifies the proportions the colors are to be mixed with.
		/// 0 means all color 1.  1 means all color 2.  .5 means half and half.  Etc.  This value must be
		/// between 0 and 1.</param>
		/// <returns>mixed (interpolated) color.</returns>
		public static Color MixColors(Color color1, Color color2, float proportion)
		{
			if (proportion < 0 || proportion > 1)
				throw new ArgumentException("proportion must be between 0 and 1 inclusive.");

			byte A = System.Convert.ToByte(color1.A + (color2.A - color1.A) * proportion);
			byte R = System.Convert.ToByte(color1.R + (color2.R - color1.R) * proportion);
			byte G = System.Convert.ToByte(color1.G + (color2.G - color1.G) * proportion);
			byte B = System.Convert.ToByte(color1.B + (color2.B - color1.B) * proportion);
			Color mixedColor = Color.FromArgb(A, R, G, B);
			return mixedColor;
		}
		/// <summary>
		/// Returns an interpolated color from within a three-control-point range of colors.
		/// </summary>
		/// <param name="lowestColor">lowest 'control point' color.</param>
		/// <param name="midColor">middle 'control point' color.</param>
		/// <param name="highestColor">highest 'control point' color.</param>
		/// <param name="d">fraction of the way from lowest to highest to interpolate from.</param>
		/// <returns>interpolated color</returns>
		public static Color MixColors(Color lowestColor, Color midColor, Color highestColor, float d)
		{
			if (d <= 0.5f)
				return MixColors(lowestColor, midColor, d * 2);
			else
				return MixColors(midColor, highestColor, (d - 0.5f) * 2);
		}
		/// <summary>
		/// Convert object to int type.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static int ToInt(object obj)//todo:Ralph change the method here so they do not throw exceptions
		{
			if (obj == null)
				throw new ArgumentNullException("obj");
			if (obj is string)
			{
				double number;
				if (IsNumeric(obj as string, out number))
                    return Convert.ToInt32(obj as string);
				else
					throw new System.FormatException();
			}

			return Convert.ToInt32(obj);
		}
		/// <summary>
		/// Convert object to float type.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static float ToFloat(object obj)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");
			if (obj is string)
			{
				double number;
				if (IsNumeric(obj as string, out number))
                    return (float)number;
				else
					throw new System.FormatException();
			}

			return Convert.ToSingle(obj);
		}
        /// <summary>
        /// Convert object to float type.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double ToDouble(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (obj is string)
            {
                double number;
                if (IsNumeric(obj as string, out number))
                    return number;
                else
                    throw new System.FormatException();
            }

            return Convert.ToDouble(obj);
        }
		/// <summary>
		/// Determine if a string represents a numeric value.
		/// </summary>
		/// <param name="str">Reference to a string.</param>
		/// <returns>True if string represents a numeric value.</returns>
		public static bool IsNumeric(string str)
		{
			if (str == null)
				return false;

			double result;
			if (double.TryParse(str, 
				System.Globalization.NumberStyles.Any,
                System.Globalization.NumberFormatInfo.CurrentInfo,//Defect 8575
				out result))
				return true;
			else
				return false;
		}
		/// <summary>Determines whether a string contains a number, and parses
		/// the number if so.</summary>
		/// <param name="Expression">the expression to check</param>
		/// <param name="number">the number parsed, if applicable</param>
		/// <returns> whether the input is a number. </returns>
		public static bool IsNumeric(object Expression, out double number)
		{
			number = double.NaN;

			if (Expression == null)
				return false;

			bool isNum = Double.TryParse(
				Convert.ToString(Expression),
				System.Globalization.NumberStyles.Any,
                System.Globalization.NumberFormatInfo.CurrentInfo,//Defect 8575
				out number);

			return isNum;
		}

		/// <summary>Extension method for Dictionaries analogous to List.ConvertAll.  Converts both keys and values.</summary>
		/// <typeparam name="K"></typeparam>
		/// <typeparam name="V"></typeparam>
		/// <typeparam name="K2"></typeparam>
		/// <typeparam name="V2"></typeparam>
		/// <param name="source"></param>
		/// <param name="keyConverter"></param>
		/// <param name="valueConverter"></param>
		/// <returns></returns>
		public static Dictionary<K2, V2> ConvertAll<K, V, K2, V2>(
			this Dictionary<K, V> source,
			Converter<K, K2> keyConverter,
			Converter<V, V2> valueConverter)
		{
			Dictionary<K2, V2> convertedDictionary = new Dictionary<K2, V2>();
			foreach (KeyValuePair<K, V> entry in source)
				convertedDictionary.Add(keyConverter(entry.Key), valueConverter(entry.Value));
			return convertedDictionary;
		}
		/// <summary>Extension method for Dictionaries which converts values from one type to another.</summary>
		/// <typeparam name="K"></typeparam>
		/// <typeparam name="V"></typeparam>
		/// <typeparam name="V2"></typeparam>
		/// <param name="source"></param>
		/// <param name="valueConverter"></param>
		/// <returns></returns>
		public static Dictionary<K, V2> ConvertValues<K, V, V2>(
			this Dictionary<K, V> source,
			Converter<V, V2> valueConverter)
		{
			Dictionary<K, V2> convertedDictionary = new Dictionary<K, V2>();
			foreach (KeyValuePair<K, V> entry in source)
				convertedDictionary.Add(entry.Key, valueConverter(entry.Value));
			return convertedDictionary;
		}
		/// <summary>
		/// Gets the parent form of a control, or null if it does not have a parent form.
		/// </summary>
		/// <param name="c">control to get the parent form of.</param>
		/// <returns>parent form of the control, or null if there is none.</returns>
		public static Form GetParentForm(Control c)
		{
			if (c == null)
				return null;

			Control potentialParent = c;
			while (potentialParent is Form == false && potentialParent.Parent != null)
				potentialParent = potentialParent.Parent;

			return potentialParent as Form;
		}
		/// <summary></summary>
		/// <returns></returns>
		public static BioRad.Common.Common.Duple<DialogResult, Color> ShowColorDialog(Color initialColor)
		{
			using (ColorDialog cd = new ColorDialog())
			{
				cd.CustomColors = PersistedApplicationOptions.GetInstance.CustomColors.ToArray();
				cd.AllowFullOpen = true;
				cd.AnyColor = true;
				cd.ShowHelp = false;
				cd.Color = initialColor;
				DialogResult result = cd.ShowDialog();
				if (result != DialogResult.OK)
					return new BioRad.Common.Common.Duple<DialogResult,Color>(result, Color.Empty);
				ApplicationOptions ao = PersistedApplicationOptions.GetInstance;
				ao.CustomColors = new List<int>(cd.CustomColors);
				PersistedApplicationOptions.PersistApplicationOptions(ao);
				return new BioRad.Common.Common.Duple<DialogResult, Color>(result, cd.Color);
			}
		}
		#endregion
	}

	/// <summary>
	/// IMPORTANT: To be used only with Using statements.
	/// 
	/// Sets Invariant Culture into the current thread in constructor, and sets back
	/// to what it was in Dispose.
	/// </summary>
	public class InvariantCultureSetter : IDisposable
	{
		private CultureInfo m_CurrentCulture;
		private CultureInfo m_CurrentUICulture;

		/// <summary>
		/// Caches current culture for current thread, and sets culture to invariant.
		/// </summary>
		public InvariantCultureSetter()
		{
			m_CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
			m_CurrentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;

			System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
			System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
		}

		#region IDisposable Members
		/// <summary>Resets the cached culture.</summary>
		public void Dispose()
		{
			System.Threading.Thread.CurrentThread.CurrentUICulture = m_CurrentUICulture;
			System.Threading.Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
		}
		#endregion
	}
    /// <summary>
    /// </summary>
    public class ResourceStrings// US157 - TA184
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rm"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        static public ReadOnlyDictionary<string, string> GetResourceStrings(System.Resources.ResourceManager rm, CultureInfo culture)
        {
            // save current culture information to restore later.
            CultureInfo currentCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            CultureInfo currentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            Dictionary<string, string> resourceStrings = new Dictionary<string, string>();
            try
            {
                using (ResourceSet resourceSet = rm.GetResourceSet(culture, true, true))
                {
                    if (resourceSet != null)
                    {
                        IDictionaryEnumerator enumerator = resourceSet.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            resourceStrings.Add(enumerator.Key.ToString(), enumerator.Value.ToString());
                        }
                    }
                }
            }
            catch
            {
                resourceStrings = new Dictionary<string, string>();
                throw;
            }
            finally
            {
                // Restore cultures
                System.Threading.Thread.CurrentThread.CurrentUICulture = currentUICulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
            }
            return new ReadOnlyDictionary<string, string>(resourceStrings);
        }
    }

	/// <summary>
	/// General purpose void-void delegate.
	/// </summary>
	public delegate void VoidVoidDelegate();

	/// <summary>
	/// General purpose delegate that takes a string and returns void.
	/// </summary>
	/// <param name="s"></param>
	public delegate void VoidStringDelegate(string s);

	/// <summary>interface for objects which contain view settings, such as window layouts or grid sorts.</summary>
	public interface IViewSettings : IBioRadXmlSerializable
	{
		/// <summary>Whether this object has been changed sufficiently from a given previous version to warrant the user being
		/// asked to save changes.  This can be a simple equality comparison or it can contain logic to skip unimportant
		/// differences.</summary>
		/// <param name="originalState">The original (unchanged) view state to compare to.</param>
		/// <returns>true if the user should be asked to save changes, false if not.</returns>
		bool ShouldAskToSaveChanges(IViewSettings originalState);
	}

	/// <summary></summary>
	public class BoolEventArgs : EventArgs
	{
		private bool m_Value = true;

		/// <summary></summary>
		public bool Value
		{
			get { return m_Value; }
			set { m_Value = value; }
		}

		/// <summary></summary>
		/// <param name="value"></param>
		public BoolEventArgs(bool value)
		{
			m_Value = value;
		}
	}
}
