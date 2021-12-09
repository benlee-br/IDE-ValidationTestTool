using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Globalization;
using System.Text;
using BioRad.Common;

namespace BioRad.Reports
{
	#region Documentation Tags
	/// <summary>
	/// Decodes and manipulates report tag strings. Report tags are strings which
	/// identify a report data item and (optionally) its formatting.
	/// </summary>
	/// <remarks>
	/// Currently supports the following data types: simple value types, bitmap objects, one or
	/// two dimensional arrays containing simple value types, one dimensional array containing
	/// one dimensional arrays of simple value types.
	/// <para>Data is formatted using an optional format string. Formatting is as defined for
	/// the String.Format() method, using the current culture. If format string is empty,
	/// uses the default format for the data type.</para>
	/// <para>Tag strings are parsed using an invariant culture format provider.</para>
	/// <para>Valid tag strings are formatted as \tag\, where tag is one of the following:</para>
	/// <list type="bullet">
	/// <item><description>alphanumeric string (A-Z,0-9 only) = Name</description>
	/// <para>A name ending with _H indicates a heading in a table.</para></item>
	/// <item><description>Name;"... " where " ... " is a format string enclosed by quotes. Note
	/// that format string may not include semi-colon or back-slash characters.</description></item>
	/// <item><description>Name[]</description></item>
	/// <item><description>Name[i] where i is a positive number or 0</description></item>
	/// <item><description>Name[];" ... "</description></item>
	/// <item><description>Name[i];" ... "</description></item>
	/// <item><description>Name[i][]</description></item>
	/// <item><description>Name[i][j] where j is a positive number or 0</description></item>
	/// <item><description>Name[i][]," ... "</description></item>
	/// <item><description>Name[i][j];" ... "</description></item>
	/// </list></remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Lisa von Schlegell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="conformancereview">Conformance review: LvS 9/14/04</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">679</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: ReportTag.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Reports/ReportTag.cs $</item>
	///			<item name="vssrevision">$Revision: 10 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class ReportTag
	{
		#region Constants
		private const string c_AlphaNumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
		private const string c_Digits = "0123456789";
		/// <summary>
		/// In Subscripts array, signals an empty subscript. Valid only for
		/// final subscript.
		/// </summary>
		public static readonly int EmptySubscript = -1;
		/// <summary>
		/// Delimits format string. This is character value because string value seems
		/// to contain an escape character as well as a quote.
		/// </summary>
		public static readonly char FormatDelimiter = '"';
		/// <summary>
		/// Separates format string from name.
		/// </summary>
		public static readonly string FormatSeparater = ";";
		/// <summary>
		/// Decoration appended to name which indicates a heading associated with
		/// a table of the same name.
		/// </summary>
		public static readonly string HeadingDecoration = "_H";
		/// <summary>
		/// Subscript bracket (left) delimiting array subscript
		/// </summary>
		public static readonly string LeftSubscriptDelimiter = "[";
		/// <summary>
		/// Subscript bracket (right) delimiting array subscript
		/// </summary>
		public static readonly string RightSubscriptDelimiter = "]";
		/// <summary>
		/// Report tag delimiter character (leading and trailing)
		/// </summary>
		public static readonly string TagDelimiter = @"\";

		private static NoReportData s_NoReportData = new NoReportData();
		#endregion

		#region Member Data
		/// <summary>
		/// Collection of default formatting strings for use when a tag
		/// does not have an explicit format string. Keyed by type name.
		/// </summary>
		private static StringDictionary s_DefaultTagFormats;
		private string m_Name = String.Empty;
		private string m_Format = String.Empty;
		private int [] m_Subscripts = new int [] {};
		#endregion

		#region Accessors
		/// <summary>
		/// Format string to be applied to the report data. If set to Null, defaults
		/// to the empty string.
		/// </summary>
		public string FormatString
		{
			get { return m_Format;}
			set 
			{ 
				if (value == null) 
					m_Format = String.Empty;
						else 
					m_Format = value;
			}
		}

		/// <summary>
		/// Indicates whether this report tag represents an array.
		/// </summary>
		public bool IsArray
		{
			get { return (m_Subscripts.Length > 0); }
		}

		/// <summary>
		/// Indicates whether this report tag represents a column heading within table.
		/// True if name ends with HeadingDecoration string.
		/// </summary>
		public bool IsHeading
		{
			get { return m_Name.EndsWith(HeadingDecoration); }
		}

		/// <summary>
		/// Indicates whether this report tag represents an array with an unbounded subscript
		/// </summary>
		public bool IsUnboundedArray
		{
			get 
			{
				return (this.IsArray && 
					(m_Subscripts[0] == EmptySubscript));
			}
		}

		/// <summary>
		/// Report tag name. This name is used to identify report data in a collection.
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Report tag base name. This name is used to identify report data in a collection
		/// when tag indicates a special heading identifier. If IsHeading is false, BaseName
		/// will be the same as Name.
		/// </summary>
		public string BaseName
		{
			get 
			{ 
				if (!IsHeading) 
				{
					// Base name is the same as name unless name ends with heading decoration
					// string
					return m_Name;
				}
				else
				{
					// return the name with heading decoration string removed.
					return m_Name.Substring(0, m_Name.Length - HeadingDecoration.Length);
				}
			}
		}

		/// <summary>
		/// Returns number of dimensions in array represented by this tag, or zero
		/// if tag does not represent and array.
		/// </summary>
		public int Rank
		{
			get
			{
				if (this.IsArray)
					return m_Subscripts.Length;
				else
					return 0;
			}
		}

		/// <summary>
		/// Set unbounded index (if any) to value.
		/// </summary>
		public int UnboundedIndex
		{
			set
			{
				if (this.IsUnboundedArray)
				{
					m_Subscripts[0] = value;
				}
			}
		}
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Parameterized constructor initializes object.
		/// </summary>
		/// <param name="name">Report tag name, used to identify data</param>
		/// <param name="format">Report tag format string, used to format data</param>
		/// <param name="subscripts">Optional list of subscripts used to select data
		/// elements within a table. Final subscript may be -1 to indicate tag is to be
		/// replicated on subsequent lines to match data dimension</param>
		public ReportTag(string name, string format, params int [] subscripts)
		{
			m_Name = name;
			m_Format = format;
			m_Subscripts = subscripts;
		}

		/// <summary>
		/// Construct an empty tag. Private to force explicit request for empty
		/// tag.
		/// </summary>
		private ReportTag()
		{
		}

		/// <summary>
		/// Static constructor initializes collection of default format strings.
		/// </summary>
		static ReportTag()
		{
			InitializeDefaultTagFormats();
		}
		#endregion

		#region Methods
		/// <summary>
		/// Get data from array corresponding to this tags indices.
		/// </summary>
		/// <param name="array">array</param>
		/// <returns>data or null if array doesn't match tag indices</returns>
		public object ArrayData(Array array)
		{
			object data = null;
			if (this.IsArray && !this.IsUnboundedArray)
			{
				if  (this.Rank == array.Rank)
				{
					// Data is an array - possibly multi-dimensional
					try
					{
						data = array.GetValue( this.m_Subscripts);
					}
					catch
					{
						// ignore any index out of range errors
					}
				}
				else if (((this.Rank -1) == array.Rank) && array.GetType().GetElementType().IsArray)
				{
					// Data is an array of arrays - pull data from sub-array
					try
					{
						// create a subscript array that does not include the last
						int [] subscripts = new int [this.Rank -1];
						for (int i=0; i< this.Rank -1; i++) subscripts[i] = m_Subscripts[i];
						// get the data sub-array from data array
						object subArray = array.GetValue(subscripts);
						if (subArray is Array)
						{
							// get data from index within sub-array
							data = ((Array)subArray).GetValue(m_Subscripts[this.Rank-1]);
						}
					}
					catch
					{
						// ignore any index out of range errors
					}
				}
			}
			return data;
		}

		/// <summary>
		/// Get data from table corresponding to this tags indices.
		/// </summary>
		/// <param name="table">sortable table</param>
		/// <returns>data or null if table doesn't match tag indices</returns>
		public object ArrayData(SortableTable table)
		{
			// Return special value if table is empty
			if (table.Empty) return s_NoReportData;

			object data = null;
			if (this.IsArray && !this.IsUnboundedArray && (this.Rank == 2))
			{
				// table data is 2-dimensional
				try
				{
					data = table[m_Subscripts[0], m_Subscripts[1]];
				}
				catch
				{
					// ignore any index out of range errors
				}
			}
			return data;
		}

		/// <summary>
		/// Construct an empty report tag. Name will be the empty string.
		/// </summary>
		/// <returns>An empty report tag.</returns>
		public static ReportTag EmptyTag()
		{
			return new ReportTag();
		}

		/// <summary>
		/// Format data using default format.
		/// </summary>
		/// <param name="data">data to format</param>
		/// <returns>formatted data as string</returns>
		public static string Format(object data)
		{
			return Format(GetDefaultFormat(data), data);
		}

		/// <summary>
		/// Format data using given format string.
		/// </summary>
		/// <param name="format">format string</param>
		/// <param name="data">data to format</param>
		/// <returns>formatted data as string</returns>
		public static string Format(string format, object data)
		{
			if ((format == String.Empty) || (data is NoReportData))
				// empty format string or no report data, convert data to string 
				// with no formatting
				return data.ToString();
			if (data.GetType().IsValueType)
				// for value types, format directly 
				return String.Format(format, data);
			if (data.GetType().IsArray)
			{
				// TODO: This does not work! Data is treated as an object rather than
				// params array.
				// format array of value types
				return String.Format(format, data);
			}
			// convert data to string and format
			return String.Format(format, data.ToString());
		}

		/// <summary>
		/// Recursively search for a default format string for given
		/// type, trying base types in turn. Returns the empty string if
		/// no default format string found.
		/// </summary>
		/// <param name="type">type to find format string for</param>
		/// <returns>default format string</returns>
		private static string GetDefaultFormat(Type type)
		{
			string str = String.Empty;
			if (s_DefaultTagFormats.ContainsKey(type.Name))
			{
				str = s_DefaultTagFormats[type.Name];
			}
			else if (type.BaseType != null)
			{
				str = GetDefaultFormat(type.BaseType);
			}
			return str;
		}

		/// <summary>
		/// Recursively search for a default format string for type of given
		/// object, trying base types in turn. Returns the empty string if
		/// no default format string found.
		/// </summary>
		/// <param name="obj">object to find format string for</param>
		/// <returns>default format string</returns>
		private static string GetDefaultFormat(object obj)
		{
			return GetDefaultFormat (obj.GetType());
		}

		/// <summary>
		/// Convenience method used to validate data type is supported for tag
		/// replacement.
		/// </summary>
		/// <param name="dataType">any type</param>
		/// <returns>true if data type is bitmap, string, value type, 
		/// or array of strings or value types</returns>
		private static bool IsDataTypeAllowed(Type dataType)
		{
			// TODO: Should we allow data that is neither a value type nor a string?
			if (dataType.IsValueType || (dataType == typeof(string)) ||( dataType == typeof(Bitmap)))
				return true;
			if (dataType.IsArray && (dataType.GetArrayRank() == 1) &&
				(dataType.GetElementType().IsValueType || (dataType.GetElementType() == typeof(string))))
				return true;
			return false;
		}

		/// <summary>
		/// Checks that data type is appropriate match for this tag.
		/// </summary>
		/// <param name="data">any data object</param>
		/// <returns>true if data is null, bitmap, string, value type or if is array and dimensions match, 
		/// else false</returns>
		public bool Match(object data)
		{
			//Fix for bug 2251 - Exception thrown when rendering reports
			//2005-01-22 (ST) Exception was thrown when data was null and this.IsArray was true
			if ( (data==null) || (data is NoReportData))
			{
				// Null data matches any tag
				return true;
			}

			if (this.IsArray)
			{
				// tag is array

				// tag matches sortable table if tag is two-D array
				if ((data is SortableTable) && (this.Rank == 2))
					return true;

				// data must otherwise be an array to match tag
				Type type = data.GetType();
				// ensure that array rank matches and element type is a value type
				Type elType = type.GetElementType();
				// allow array whose indices match, whose elements are either string or value types
				// or arrays of string or value types. Allowing an array of arrays lets the secondary
				// array be formated w/ a single format string
				if (type.IsArray && (type.GetArrayRank() == this.Rank) && IsDataTypeAllowed(elType))
					return true;
				// allow array of arrays if secondary array is an array of value types or strings,
				// when indices match.
				if (type.IsArray && (type.GetArrayRank() == (this.Rank -1) && 
					elType.IsArray && IsDataTypeAllowed(elType)))
					return true;
			}
			else
			{
				// tag is not array and data is not null - verify data type is allowed
				if (IsDataTypeAllowed(data.GetType()))
					return true;
			}
			// mismatch between tag and data
			return false;
		}

		/// <summary>
		/// Parse a tag string into a ReportTag object. May throw FormatException or 
		/// ArgumentNullException.
		/// </summary>
		/// <remarks>Valid tag strings are formatted as :tag:, where tag is one of the following:
		/// <list type="bullet">
		/// <item><description>alphanumeric string (A-Z,0-9 only) = Name</description></item>
		/// <item><description>Name,"... " where " ... " is a format string enclosed by quotes</description></item>
		/// <item><description>Name[]</description></item>
		/// <item><description>Name[i] where i is a positive number or 0</description></item>
		/// <item><description>Name[]," ... "</description></item>
		/// <item><description>Name[i]," ... "</description></item>
		/// <item><description>Name[i][]</description></item>
		/// <item><description>Name[i][j] where j is a positive number or 0</description></item>
		/// <item><description>Name[i][]," ... "</description></item>
		/// <item><description>Name[i][j]," ... "</description></item>
		/// </list></remarks>
		/// <param name="tag">tag string.</param>
		/// <returns></returns>
		public static ReportTag Parse(string tag)
		{
			if (tag == null)
			{
                string sr = StringUtility.FormatString(BioRad.Common.Properties.Resources.Reports_NullTag);
				throw new ArgumentNullException(sr);
			}
			if (tag == String.Empty)
			{
                string sr = StringUtility.FormatString(BioRad.Common.Properties.Resources.Reports_EmptyTag);
				throw new FormatException(sr);
			}
			string name = String.Empty;
			string format = String.Empty;
			int [] subscripts = new int [] {};
			bool failed = false;
			// remove tag delimiters from beginnning and ending of tag, and split into name/format string 
			string [] tagStrings = tag.Trim(TagDelimiter.ToCharArray()).Split(FormatSeparater.ToCharArray());
			// validate tag contains name and, optionally, format string; parse format string.
			switch (tagStrings.Length)
			{
				case 1:
					// Name only, no format string
					break;
				case 2:
					// format string provided. Ensure it starts/ends with quotes and
					// remove them
					// HACK: This test fails to find quotes - temporarily removed and quotes
					// are assumed
					int firstIndex = tagStrings[1].IndexOf('"');
					int lastIndex = tagStrings[1].LastIndexOf('"');
					if (
//						(tagStrings[1].IndexOf(FormatDelimiter) == 0) && 
//						(tagStrings[1].LastIndexOf(FormatDelimiter) == tagStrings[1].Length-1) &&
						(tagStrings[1].Length >=2))
					{
						format = tagStrings[1].Substring(1, tagStrings[1].Length - 2);
					}
					else
					{
						failed = true;
					}
					break;
				default:
					failed = true;
					break;
			}
			if (!failed)
			{
				// parse name and, optionally, subscripts
				name = tagStrings[0];
				tagStrings = name.Split(LeftSubscriptDelimiter.ToCharArray());
				switch (tagStrings.Length)
				{
					case 1:
						// name only
						break;
					case 2:
					case 3:
						// name and one or two subscripts; retrieve name
						name = tagStrings[0];
						subscripts = new int [tagStrings.Length -1];
						string subscript;
						// parse subscripts
						for (int i = 1; i< tagStrings.Length; i++)
						{
							// validate subscript is delimited properly
							if (tagStrings[i].EndsWith(RightSubscriptDelimiter))
							{
								// get subscript and validate it contains only digits
								subscript = tagStrings[i].TrimEnd(RightSubscriptDelimiter.ToCharArray());
								if (subscript.TrimStart(c_Digits.ToCharArray()) == String.Empty)
								{
									if (subscript == String.Empty)
									{
										// the first subscript may be empty
										if (i==1)
											subscripts[i-1] = EmptySubscript;
										else
											// empty subscript is not first subscript
											failed = true;
									}
									else
										// set non-empty subscript
										subscripts[i-1] = Convert.ToInt32(subscript, CultureInfo.InvariantCulture);
								}
								else
									// subscript contains non-digit characters
									failed = true;
							}
							else
								// subscript is not delimited properly
								failed = true;
						}
						break;
					default:
						// too many subscripts, only 1 or 2 allowed
						failed = true;
						break;
				}
			}
			if (!failed)
			{
				// validate name is not empty and contains only alpha-numeric characters
				if ((name == String.Empty) ||
					(name.ToUpper().TrimStart(c_AlphaNumeric.ToCharArray()) != String.Empty))
					failed = true;
			}
			if (failed)
			{
				// failed to parse tag; throw exception
                string sr = StringUtility.FormatString(BioRad.Common.Properties.Resources.Reports_InvalidTag_1, tag);
				throw new FormatException(sr);
			}
			// tag parsed successfully; return report tag object
			return new ReportTag(name, format, subscripts);
		}

		/// <summary>
		/// Initialize collection of default format strings, keyed by type name.
		/// </summary>
		private static void InitializeDefaultTagFormats()
		{
			s_DefaultTagFormats = new StringDictionary();
			// TODO: Currently no default formats. Evaluate to drop feature or
			// or define defaults. Also, how to handle derived types.
		}

		/// <summary>
		/// Set default format string for given type, overriding current default format
		/// string for that type if any.
		/// </summary>
		/// <param name="type">type name is used as key.</param>
		/// <param name="str">format string value</param>
		public static void SetDefaultFormat(Type type, string str)
		{
			if (s_DefaultTagFormats.ContainsKey(type.Name))
			{
				s_DefaultTagFormats.Remove(type.Name);
			}
			s_DefaultTagFormats.Add(type.Name, str);
		}

		/// <summary>
		/// Set default format string for type of given oject, overriding current default 
		/// format string for that type if any.
		/// </summary>
		/// <param name="obj">object's type name is used as key.</param>
		/// <param name="str">format string value</param>
		public static void SetDefaultFormat(object obj, string str)
		{
			SetDefaultFormat(obj.GetType(), str);
		}

		/// <summary>
		/// Override returns tag as string.
		/// </summary>
		/// <remarks>Valid tag strings are formatted as :tag:, where tag is one of the following:
		/// <list type="bullet">
		/// <item><description>alphanumeric string (A-Z,0-9 only) = Name</description></item>
		/// <item><description>Name,"... " where " ... " is a format string enclosed by quotes</description></item>
		/// <item><description>Name[]</description></item>
		/// <item><description>Name[i] where i is a positive number or 0</description></item>
		/// <item><description>Name[]," ... "</description></item>
		/// <item><description>Name[i]," ... "</description></item>
		/// <item><description>Name[][j] where j is a positive number or 0</description></item>
		/// <item><description>Name[i][j] where i and j are positive numbers or 0</description></item>
		/// <item><description>Name[][j]," ... "</description></item>
		/// <item><description>Name[i][j]," ... "</description></item>
		/// </list></remarks>
		/// <returns>report tag</returns>
		public override string ToString()
		{
			// Starting delimiter
			StringBuilder sb = new StringBuilder(TagDelimiter);
			// name
			sb.Append(this.Name);
			if (this.IsArray)
			{
				// Append all subscripts if tag represents an array
				foreach (int i in m_Subscripts)
				{
					if (i < 0)
					{
						// Empty brackets when subscript is -1
						sb.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}",
							LeftSubscriptDelimiter, RightSubscriptDelimiter);
					}
					else
					{
						// Brackets with subscript value
						sb.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}{2}",
							LeftSubscriptDelimiter, i, RightSubscriptDelimiter);
					}
				}
			}
			// Append format string if any
			if (this.FormatString != String.Empty)
			{
				sb.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", 
					FormatSeparater, FormatDelimiter, this.FormatString, FormatDelimiter);
			}
			// Ending delimiter.
			sb.Append(TagDelimiter);
			return sb.ToString();
		}
		#endregion
	}
}
