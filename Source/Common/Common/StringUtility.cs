using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>Class to provide basic string methods like converting to double and 
	/// compare methods.</summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Shabnam</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="conformancereview">Conformance review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">Replace this text with ID</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: StringUtility.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/StringUtility.cs $</item>
	///			<item name="vssrevision">$Revision: 18 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Blie $</item>
	///			<item name="vssdate">$Date: 11/12/07 5:27p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class StringUtility
	{
		#region Constants
		/// <summary>Token for storing double.MinValue, to avoid rounding errors.</summary>
		public static readonly string c_DoubleMinToken = "DOUBLE.MIN";
		/// <summary>Token for storing double.MaxValue, to avoid rounding errors.</summary>
		public static readonly string c_DoubleMaxToken = "DOUBLE.MAX";
		#endregion
		
        #region Methods

        /// <summary>
        /// Convert string to secure string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static System.Security.SecureString ToSecureString(string input)
        {
            System.Security.SecureString secure = new System.Security.SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }
        /// <summary>
        /// Convert secure string to string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToNonSecureString(System.Security.SecureString input)
        {
            string returnValue = string.Empty;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }
		/// <summary>Returns a string representation of the DateTime.ToString value.</summary>
		/// <remarks>Converts the DateTime to a string using the InvariantCulture.</remarks>
		/// <param name="dateTime">The value of the date.</param>
		/// <returns>A string.</returns>
		public static string DateTimeToString(DateTime dateTime)
		{
			return dateTime.ToString(CultureInfo.InvariantCulture);
		}
		/// <summary>Returns a string representation of the W3Date string value.</summary>
		/// <remarks>Converts the DateTime to a UTC date string.</remarks>
		/// <param name="dateTime">The value of the date.</param>
		/// <returns>A string.</returns>
		public static string DateTimeToUtcString(DateTime dateTime)
		{
            return W3CDateTime.ToUtc(dateTime, CultureInfo.InvariantCulture);
		}
		/// <summary>Returns a string representation of the double value.</summary>
		/// <remarks>Converts the double to a string using the InvariantCulture.</remarks>
		/// <param name="doubleValue">The value of the double.</param>
		/// <returns>A string.</returns>
		public static string DoubleToString(double doubleValue)
		{
			return doubleValue.ToString(CultureInfo.InvariantCulture);
		}
		/// <summary>Returns a string representation of the double value
		/// using the InvariantCulture.</summary>
		/// <param name="doubleValue">The value of the double.</param>
		/// <param name="cultureInfo">The CultureInfo object to use.</param>
		/// <returns>A string.</returns>
		public static string DoubleToString(double doubleValue, CultureInfo cultureInfo)
		{
			return doubleValue.ToString(cultureInfo);
		}
		/// <summary>Returns a string representation of the double value.</summary>
		/// <remarks>Converts the double to a string using the InvariantCulture.</remarks>
		/// <param name="doubleValue">The value of the double.</param>
		/// <param name="format">The format to use.</param>
		public static string DoubleToString(double doubleValue, string format)
		{
			if (!string.IsNullOrEmpty(format))
				return doubleValue.ToString(format);
			else
				return DoubleToString(doubleValue);
		}
		/// <summary>Returns a string representation of the double value
		/// using the InvariantCulture.</summary>
		/// <param name="doubleValue">The value of the double.</param>
		/// <param name="format">The format to use.</param>
		/// <param name="cultureInfo">The CultureInfo object to use.</param>
		/// <returns>A string.</returns>
		public static string DoubleToString(double doubleValue, string format, 
			CultureInfo cultureInfo)
		{
			if (!string.IsNullOrEmpty(format))
				return doubleValue.ToString(format, cultureInfo);
			else
				return DoubleToString(doubleValue, cultureInfo);
		}
		/// <summary>Returns a string representation of the float value.</summary>
		/// <remarks>Converts the double to a string using the InvariantCulture.</remarks>
		/// <param name="floatValue">The value of the float.</param>
		/// <returns>A string.</returns>
		public static string FloatToString(float floatValue)
		{
			return floatValue.ToString(CultureInfo.InvariantCulture);
		}
		/// <summary>Returns a string representation of the double value.</summary>
		/// <remarks>Converts the double to a string using the InvariantCulture.</remarks>
		/// <param name="floatValue">The value of the float.</param>
		/// <param name="format">The format to use.</param>
		public static string FloatToString(float floatValue, string format)
		{
			if (!string.IsNullOrEmpty(format))
				return floatValue.ToString(format);
			else
				return DoubleToString(floatValue);
		}
		/// <summary>Returns a string representation of the double value
		/// using the InvariantCulture.</summary>
		/// <param name="floatValue">The value of the float.</param>
		/// <param name="format">The format to use.</param>
		/// <param name="cultureInfo">The CultureInfo object to use.</param>
		/// <returns>A string.</returns>
		public static string FloatToString(float floatValue, string format,
			CultureInfo cultureInfo)
		{
			if (!string.IsNullOrEmpty(format))
				return floatValue.ToString(format, cultureInfo);
			else
				return DoubleToString(floatValue, cultureInfo);
		}
        /// <summary>
        /// Return a formatted string corresponding to the given list, with each element
        /// delimited by a localized delimiter.
        /// </summary>
        /// <remarks>List is localized for the current culture. Objects are converted
        /// to strings using the ToString() method.</remarks>
        /// <param name="objects">array of objects</param>
        /// <returns>Empty string if list is null or empty, otherwise string with each
        /// delimiters between each element.</returns>
        public static string FormatList(object[] objects)
        {
            // Empty list returns empty string
            if (objects == null || objects.Length == 0)
                return String.Empty;
            string delimiter = StringUtility.FormatString(Properties.Resources.ListPostDelimiter_1);
            // Start w/ first item
            StringBuilder sb = new StringBuilder(objects[0].ToString());
            for (int i = 1; i < objects.Length; i++)
            {
                // Append delimiter and subsequent items
                sb.AppendFormat(delimiter, objects[i].ToString());
            }
            return sb.ToString();
        }

		/// <summary>
		/// Return a formatted string corresponding to the given list, with each element
		/// delimited by a localized delimiter.
		/// </summary>
		/// <remarks>List is localized for the current culture. Objects are converted
		/// to strings using the ToString() method.</remarks>
		/// <param name="objects">array of objects</param>
		/// <returns>Empty string if list is null or empty, otherwise string with each
		/// delimiters between each element.</returns>
		public static string StringListToString(List<String> objects)
		{
			StringBuilder sb = new StringBuilder();
			char delimiter = ',';
			foreach (String item in objects)
			{
				sb.Append(item);
				sb.Append(delimiter);
			}
			if (sb.Length > 1)
				sb.Remove(sb.Length - 1, 1);  // remove the last ","

			return sb.ToString();
		}

		/// <summary>
		/// Parse string which is merged from strings  with demiliter ','
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		static public List<string> StringListParse(string source)
		{
			List<string> stringList = new List<string>();
			char delimiter = ',';
			if (!string.IsNullOrEmpty(source))
			{
				string[] itemsstring = source.Split(delimiter);
				foreach (string item in itemsstring)
				{
					stringList.Add(item);
				}
			}
			return stringList;
		}


		/// <summary>
		/// Parse string which is merged from strings  with demiliter ','
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		static public List<int> IntListParse(string source)
		{
			List<int> intList = new List<int>();
			char delimiter = ',';
			if (!string.IsNullOrEmpty(source))
			{
				string[] itemsstring = source.Split(delimiter);
				foreach (string item in itemsstring)
				{
					int intItem = int.Parse(item);
					intList.Add(intItem);
				}
			}
			return intList;
		}

        /// <summary>Replaces the format item in a specified String with the text 
        /// equivalent of the value of a corresponding Object instance in a specified 
        /// array. </summary>
        /// <param name="format">A String containing zero or more format items.</param>
        /// <param name="parameters">An Object array containing zero or more objects to format. </param>
        /// <returns>A copy of format in which the format items have been replaced by 
        /// the String equivalent of the corresponding instances of Object in args. 
        /// </returns>
        public static string FormatString(string format, params object[] parameters)
        {
            if (parameters.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(format, parameters);
                return sb.ToString();
            }
            else
                return format;
        }

		/// <summary>Inserts a space after every character number (as specified by 
		/// insertPosition).</summary>
		/// <param name="originalString">The original string object.</param>
		/// <param name="insertPosition">The number of characters to insert 
		/// a space after</param>
		/// <returns>A string with the inserted space</returns>
		public static string InsertSpaceInString(string originalString, 
			int insertPosition)
		{
			if(originalString.Length < insertPosition)
				return originalString;

			StringBuilder sb = new StringBuilder();
			for(int position = 0; position < originalString.Length;)
			{
				if((position + insertPosition) > originalString.Length)
					insertPosition = originalString.Length - position;

				sb.Append(originalString.Substring(position, insertPosition));
				sb.Append(" ");
				position += insertPosition;
			}

			return sb.ToString();

		}
		/// <summary>Tests if string1 matches string2. Does a case insensitive string
		/// comparison.</summary>
		/// <param name="string1">The first string to compare.</param>
		/// <param name="string2">The second string to compare.</param>
		/// <returns>True if the strings are the case, else false.</returns>
		public static bool StringMatch(string string1, string string2)
		{
			return (string.Compare(string1, string2, true).Equals(0))? true : false;
		}
		/// <summary>Converts a string to a DateTime object</summary>
		/// <remarks>Converts the string using the InvariantCulture</remarks>
		/// <param name="dateTime">The string to convert.</param>
		/// <returns>A DateTime object</returns>
		public static DateTime StringToDateTime(string dateTime)
		{
			DateTime dateTimeObject = DateTime.Parse(dateTime, CultureInfo.InvariantCulture);
			return dateTimeObject;
		}

		/// <summary>Converts a W3Date string to a DateTime object</summary>
		/// <param name="dateTime">The string to convert.</param>
		/// <returns>A DateTime object</returns>
		public static DateTime W3DateStringToDateTime(string dateTime)
		{
			if (string.IsNullOrEmpty(dateTime))
			{
				return new DateTime(1900, 1, 1);
			}
			else
                return W3CDateTime.ToLocal(dateTime, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts a string to a float.</summary>
		/// <remarks>Converts the string using the InvariantCulture</remarks>
		/// <param name="stringValue">The string to convert.</param>
		/// <returns>A float.</returns>
		public static double StringToDouble(string stringValue)
		{
			if ((stringValue == null) || (stringValue.Length.Equals(0)) ||
				StringMatch(stringValue, double.NaN.ToString(CultureInfo.InvariantCulture)))
			{
				return double.NaN;
			}
			else
			{
				return double.Parse(stringValue, CultureInfo.InvariantCulture);
			}
		}

        /// <summary>Converts a string to a float.</summary>
        /// <remarks>Converts the string using the InvariantCulture</remarks>
        /// <param name="stringValue">The string to convert.</param>
        /// <param name="culture">The Culture to use when parsing the string</param>
        /// <returns>A double.</returns>
        public static double StringToDouble(string stringValue, CultureInfo culture)
        {
            if ((stringValue == null) || (stringValue.Length.Equals(0)) ||
                StringMatch(stringValue, double.NaN.ToString(culture)))
            {
                return double.NaN;
            }
            else
            {
                return double.Parse(stringValue, culture);
            }
        }

		/// <summary>Converts a string to a float, accounting for min and max
		/// values with tokens to avoid rounding issues.</summary>
		/// <remarks>Converts the string using the InvariantCulture</remarks>
		/// <param name="stringValue">The string value</param>
		/// <returns>A float object set to the value of the stringValue.</returns>
		public static float StringToFloat(string stringValue)
		{
			if ((stringValue == null) || (stringValue.Length.Equals(0)) ||
				(stringValue.Equals(float.NaN.ToString(CultureInfo.InvariantCulture))))
			{
				return float.NaN;
			}
			else
			{
				return float.Parse(stringValue, CultureInfo.InvariantCulture);
			}
		}
		/// <summary>Converts a string to a float, accounting for min and max
		/// values with tokens to avoid rounding issues.</summary>
		/// <remarks>Converts the string using the InvariantCulture</remarks>
		/// <param name="stringValue">The string value</param>
		/// <param name="culture">The Culture to use when parsing the string</param>
		/// <returns>A float object set to the value of the stringValue.</returns>
		public static float StringToFloat(string stringValue, CultureInfo culture)
		{
			if ((stringValue == null) || (stringValue.Length.Equals(0)) ||
				(stringValue.Equals(float.NaN.ToString(culture))))
			{
				return float.NaN;
			}
			else
			{
				return float.Parse(stringValue, culture);
			}
		}

		/// <summary>Converts a string to a TemperatureType object</summary>
		/// <remarks>Converts the string using the InvariantCulture</remarks>
		/// <param name="temperatureValue">The value for the temperature.</param>
		/// <param name="temperatureScale">The scale for the temperature.</param>
		/// <returns>A TemperatureType object</returns>
		public static TemperatureType StringToTemperatureType(string temperatureValue, 
			TemperatureType.Scales temperatureScale)
		{
			TemperatureType temperatureObject = new TemperatureType();
			temperatureObject.Scale = temperatureScale;
			temperatureObject.SetTemperature(StringToDouble(temperatureValue));
			return temperatureObject;
		}
		
		/// <summary>Converts a string to a TimeType object</summary>
		/// <remarks>Converts the string using the InvariantCulture</remarks>
		/// <param name="timeValue">The value for the time.</param>
		/// <param name="timeUnit">The unit for the time.</param>
		/// <returns>A TimeType object</returns>
		public static TimeType StringToTimeType(string timeValue, TimeType.Units timeUnit)
		{
			double time = StringToDouble(timeValue);
			TimeType timeObject = new TimeType();
			timeObject.Unit = timeUnit;
			timeObject.Time = time;

			return timeObject;
		}

		/// <summary>Converts a string to a VolumeType object</summary>
		/// <remarks>Converts the string using the InvariantCulture</remarks>
		/// <param name="volumeValue">The value for the time.</param>
		/// <param name="unitValue">The unit for the volume.</param>
		/// <returns>A VolumeType object</returns>
		public static VolumeType StringToVolumeType(string volumeValue, string unitValue)
		{
			VolumeType volume = new VolumeType();
			VolumeType.Units unit = (unitValue.Length.Equals(0)) ?
				VolumeType.Units.MicroLiter :
				(VolumeType.Units) Enum.Parse(typeof(VolumeType.Units),
				unitValue);
			volume.Unit = unit;
			volume.Volume = (volumeValue.Length.Equals(0)) ? double.NaN :
				StringToDouble(volumeValue);

			return volume;

		}
		#endregion
	}
}
