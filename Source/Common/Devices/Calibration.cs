using System;
using System.Text;
using System.Collections;

namespace BioRad.Common.Devices
{
	#region Documentation Tags	
	/// <summary>
	/// This class defines a calibration value and expected range 
	/// so we can verify that the value is within specs.
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Serge Taran</item>
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
	///			<item name="vssfile">$Workfile: Calibration.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/Devices/Calibration.cs $</item>
	///			<item name="vssrevision">$Revision: 21 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Staran $</item>
	///			<item name="vssdate">$Date: 2/13/06 6:39p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion	
	[Serializable]
	public class OneValue
	{		
		#region Constants
		/// <summary>
		/// Types
		/// </summary>
		public enum Type
		{
			/// <summary>
			/// Integer, 4 bytes
			/// </summary>
			Int04,
			/// <summary>
			/// Float, 4 bytes
			/// </summary>
			Flt04,
			/// <summary>
			/// Short string (4 char)
			/// </summary>
			Str04,
			/// <summary>
			/// Short string (8 bytes)
			/// </summary>
			Str08,
			/// <summary>
			/// Long string (16 bytes)
			/// </summary>
			Str16
		}
		/// <summary>
		/// Allowed units
		/// </summary>
		public enum Units 
		{
			/// <summary>
			/// No unit
			/// </summary>
			None,
			/// <summary>
			/// Percentage
			/// </summary>
			Percent,
			/// <summary>
			/// Pixels
			/// </summary>
			Pixels,
			/// <summary>
			/// Degrees
			/// </summary>
			Degrees,
			/// <summary>
			/// Counts
			/// </summary>
			Count,
			/// <summary>
			/// a well name (B12,D8...)
			/// </summary>
			Well,
			/// <summary>
			/// a generic string (such as a user name for instance)
			/// </summary>
			String,
			/// <summary>
			/// milli seconds
			/// </summary>
			ms,
			/// <summary>
			/// Duration in minutes
			/// </summary>
			Minutes
		};

		/// <summary>
		/// Upper limit for Max double value
		/// </summary>
		public const double c_UpperLimit=1e20;
		/// <summary>
		/// Default error separator
		/// </summary>
		public const string c_ErrorSeparator="***";
		#endregion
		#region Member Data
		/// <summary>
		/// Non zero value indicates that the item should be saved as calibration data 
		/// (value indicates calibration item number)
		/// </summary>
		private int m_CalibrationId;	
		/// <summary>
		/// Defines the type of underlying data (string, integer, floating point ...)
		/// </summary>
		private Type m_DataType;
		/// <summary>
		/// Description (optional)
		/// </summary>
		private string m_Description;
		/// <summary>
		/// Minimum value accepted
		/// </summary>
		private double m_Min;
		/// <summary>
		/// Maximum value accepted
		/// </summary>
		private double m_Max;
		/// <summary>
		/// This flag set to false if the value is out of range (true otherwise)
		/// </summary>
		private bool m_Pass;
		/// <summary>
		/// Optional precision wanted when displaying data in reports
		/// </summary>
		private int m_Precision;
		/// <summary>
		/// Value to check
		/// </summary>
		private string m_String;
		/// <summary>
		/// Unit
		/// </summary>
		private Units m_Unit;
		/// <summary>
		/// Value to check
		/// </summary>
		private double m_Value;
		/// <summary>
		/// More info about the value (optional)
		/// </summary>
		private string m_Where;
		/// <summary>
		/// Flag to tell if the value stored is relevant (ie computed already or not)
		/// </summary>
		private bool m_Loaded;
		/// <summary>
		/// Non zero values tell if the value comes from a given section
		/// </summary>
		private int m_Section;
		/// <summary>
		/// Tells if the min and max values have been set (flag is true) or not (flag is false)
		/// It is useful to know because min and max values can now be set from an external XML file
		/// (we want to make sure that the range for every calibration value is properly loaded)
		/// </summary>
		private bool m_RangeDefined;
		/// <summary>
		/// static String returned by GetErrorString(). 
		/// </summary>
		private static string m_ErrorSeparator=c_ErrorSeparator;
		#endregion
		#region Accessors
		/// <summary>
		/// Accessor for the Calibration Id field
		/// </summary>
		public int CalibrationId
		{
			get { return this.m_CalibrationId;}
			set { this.m_CalibrationId = value;}
		}

		/// <summary>
		/// Accessor for the Computed field
		/// </summary>
		public bool Loaded
		{
			get { return this.m_Loaded;}
			set { this.m_Loaded = value;}
		}

		/// <summary>
		/// Accessor for the type field
		/// </summary>
		public Type DataType 
		{
			get { return this.m_DataType;}
			set { this.m_DataType = value;}
		}

		/// <summary>
		/// Accessor for the description field
		/// </summary>
		public string Description
		{
			get { return this.m_Description;}
			set { this.m_Description = value;}
		}

		/// <summary>
		/// Accessor for the min field
		/// </summary>
		public double Min
		{
			get { return this.m_Min;}
			set { this.m_Min = value;}
		}

		/// <summary>
		/// Accessor for the max field
		/// </summary>
		public double Max
		{
			get { return this.m_Max;}
			set { this.m_Max = value;}
		}

		/// <summary>
		/// Accessor for the precision field
		/// </summary>
		public int Precision
		{
			get { return this.m_Precision;}
			set { this.m_Precision = value;}
		}

		/// <summary>
		/// Accessor for the optional section field
		/// </summary>
		public int Section
		{
			get { return this.m_Section;}
			set { this.m_Section = value;}
		}

		/// <summary>
		/// Accessor for the string field
		/// </summary>
		public string String
		{
			get { return this.m_String;}
			set { this.m_String = value;}
		}

		/// <summary>
		/// Accessor for the precision field
		/// </summary>
		public Units Unit
		{
			get { return this.m_Unit;}
			set { this.m_Unit = value;}
		}

		/// <summary>
		/// Accessor for the value field
		/// </summary>
		public double Value
		{
			get { return this.m_Value;}
			set { this.m_Value = value;}
		}

		/// <summary>
		/// Accessor for the where field
		/// </summary>
		public string Where
		{
			get { return this.m_Where;}
			set { this.m_Where = value;}
		}
		/// <summary>
		/// Accessor for the RangeDefined flag
		/// </summary>
		public bool RangeDefined
		{
			get { return this.m_RangeDefined;}
			set { this.m_RangeDefined = value;}
		}
		#endregion
		#region Delegates and Events
		#endregion
		#region Constructors and Destructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public OneValue()
		{
			this.m_DataType=Type.Int04;
			this.m_Pass=true;
			this.m_Value=0.0;
			this.m_String="";
			this.m_Min=0.0;
			this.m_Max=0.0;
			this.m_Unit=Units.None;
			this.m_Description="";
			this.m_Where="";
			this.m_Precision=0;
			this.m_CalibrationId=0;
			this.m_Loaded=false;
			this.m_Section=0;
			this.m_RangeDefined=false;
		}
		#endregion
		#region Methods
		/// <summary>
		/// Check that the value is within range
		/// Set m_Pass to true if the value is within range, false otherwise
		/// </summary>
		/// <returns>return true if value within range, false otherwise</returns>
		public bool Check()
		{
			//Don't check any range if the field is a string
			if (!Numeric()) 
			{
				return true;
			}
			//Otherwise, check range and return false if outside boundaries
			if (m_Value<m_Min || m_Value>m_Max)
			{
				m_Pass=false;
			}
			else
			{
				m_Pass=true;
			}
			return m_Pass;
		}

		/// <summary>
		/// Tell if the field is numeric or not
		/// </summary>
		/// <returns>true if numeric, false otherwise</returns>
		public bool Numeric()
		{
			switch (m_DataType)	
			{
				case Type.Flt04:return true;
				case Type.Int04:return true;
				case Type.Str04:return false;
				case Type.Str08:return false;
				case Type.Str16:return false;
				default://Unhandled case, should never happen
					return false;
			}
		}

		/// <summary>
		/// Return a string that helps describe the state of the result. 
		/// For instance is it loaded or not, or in range
		/// </summary>
		/// <returns>string</returns>
		public string GetErrorString()
		{
			StringBuilder returned=new StringBuilder(5);
			if (!m_Loaded)
			{
				returned.Append("?");
			}
			if (!this.Check())
			{
				returned.Append(m_ErrorSeparator);
			}
			return returned.ToString();
		}

		/// <summary>
		/// Get single value corresponding to it's internal 4 bytes binary representation 
		/// </summary>
		/// <param name="integerValue"></param>
		/// <param name="singleEquivalent"></param>
		public void GetFlt04FromInt04(int integerValue,ref Single singleEquivalent)
		{
			//Note: It would have been handy to use the BinaryReader's ReadSingle() method here, 
			//but it would require use file streams for this conversion
			
			byte[] b = new byte[4];
			b[0]=(byte)(integerValue & 0xff);
			b[1]=(byte)((integerValue >>8) & 0xff);
			b[2]=(byte)((integerValue >>16) & 0xff);
			b[3]=(byte)((integerValue >>24) & 0xff);

			// Declare an array of 1 single
			System.Single[] s = new System.Single[1];

			// Copy 4 bytes->single
			Buffer.BlockCopy(b, 0, s, 0, b.Length);

			singleEquivalent=s[0];			
		}

		/// <summary>
		/// Split single value into 4 bytes then pack into a 32bit integer
		/// </summary>
		/// <param name="singleValue"></param>
		/// <param name="integerEquivalent"></param>
		public void GetInt04FromFlt04(Single singleValue,ref int integerEquivalent)
		{
			// Allocate the byte array and fill.
			byte[] b = new byte[4];

			// Declare an array of 1 single
			System.Single[] s = new System.Single[1];
			s[0]=singleValue;

			// Copy single->4 bytes
			Buffer.BlockCopy(s, 0, b, 0, b.Length);
			
			integerEquivalent=b[0] | (b[1]<<8) | (b[2]<<16) | (b[3]<<24);
		}

		/// <summary>
		/// Get 32 bits integer equivalent for 4 character long string 
		/// </summary>
		/// <param name="stringValue"></param>
		/// <param name="integerEquivalent"></param>
		public void GetInt04FromStr04(string stringValue,ref int integerEquivalent)
		{			
			byte[] b = new byte[4];
			//Read string into byte array. Check length to avoid referencing invalid indexed positions, 
			b[0]=(byte)(stringValue.Length>=1?stringValue[0]:0);
			b[1]=(byte)(stringValue.Length>=2?stringValue[1]:0);
			b[2]=(byte)(stringValue.Length>=3?stringValue[2]:0);
			b[3]=(byte)(stringValue.Length>=4?stringValue[3]:0);
							
			//Convert back to int
			integerEquivalent=b[0] | (b[1]<<8) | (b[2]<<16) | (b[3]<<24);
		}

		/// <summary>
		/// Get 4 char string corresponding to it's internal 4 bytes binary representation 
		/// </summary>
		/// <param name="integerValue"></param>
		/// <param name="stringEquivalent"></param>
		public void GetStr04FromInt04(int integerValue,ref string stringEquivalent)
		{			
			byte[] b = new byte[4];
			b[0]=(byte)(integerValue & 0xff);
			b[1]=(byte)((integerValue >>8) & 0xff);
			b[2]=(byte)((integerValue >>16) & 0xff);
			b[3]=(byte)((integerValue >>24) & 0xff);
			
			char[] sarray=new char[4];
			sarray[0]=(char)b[0];
			sarray[1]=(char)b[1];
			sarray[2]=(char)b[2];
			sarray[3]=(char)b[3];
				
			//Convert back to 4 character long string
			stringEquivalent = new string(sarray);
		}

		/// <summary>
		/// Reset value to zero (if numeric) or empty string (if string)
		/// </summary>
		public void ResetValueOnly()
		{
			this.m_Value=0.0;
			this.m_String="";
		}

		/// <summary>
		/// Set the error separator used by GetErrorString(...)
		/// </summary>
		/// <param name="stringSeparator">if null, set to default, otherwise set to the specified string</param>
		public void SetErrorSeparator(string stringSeparator)
		{
			if (stringSeparator==null)
			{
				//Set back to default when provided value is null
				OneValue.m_ErrorSeparator=c_ErrorSeparator;
			}
			else
			{
				//Set to specified string if one is provided
				OneValue.m_ErrorSeparator=stringSeparator;			
			}
		}

		/// <summary>
		/// Initialize fields with newer values
		/// </summary>
		/// <param name="newType"></param>
		/// <param name="newValue"></param>
		/// <param name="newString"></param>
		/// <param name="newUnit"></param>
		/// <param name="newMinValue"></param>
		/// <param name="newMaxValue"></param>
		/// <param name="newPrecision">Optional precision wanted when generating reports</param>
		/// <param name="newDescription"></param>
		/// <param name="newWhere"></param>
		/// <param name="calibrationId"></param>
		/// <param name="section">Specify from which section the value is coming from (optional)</param>
		public void Set(Type newType,double newValue,string newString,OneValue.Units newUnit,
			double newMinValue,double newMaxValue,
			int newPrecision,
			string newDescription,string newWhere,
			int calibrationId,
			int section)
		{			
			m_DataType=newType; 
			m_Value=newValue;	
			m_String=newString;
			m_Min=newMinValue;
			m_Max=newMaxValue;
			m_Unit=newUnit;
			m_Description=newDescription;
			m_Where=newWhere;
			m_Precision=newPrecision;
			m_CalibrationId=calibrationId;
			m_Section=section;
		}
		#endregion
		#region Event Handlers
		#endregion
	}

	#region Documentation Tags
	/// <summary>
	/// This class handles plain text and Coma Separated Values (CSV) reports generation
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Serge Taran</item>
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
	///			<item name="vssfile">$Workfile: Calibration.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/Devices/Calibration.cs $</item>
	///			<item name="vssrevision">$Revision: 21 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Staran $</item>
	///			<item name="vssdate">$Date: 2/13/06 6:39p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	[Serializable]
	public class Report
	{
		#region Constants
		/// <summary>
		/// Separator used for Csv files
		/// </summary>
		const string c_CsvSeparator=",";
		/// <summary>
		/// Default file extension for csv files
		/// </summary>
		public const string c_CsvExtension=".csv";
		/// <summary>
		/// Default file extension for plain text files
		/// </summary>
		public const string c_TextExtension=".txt";
		/// <summary>
		/// Display double as an integer (no decimal point)
		/// </summary>
		public const int c_NoPrecision=0;
		/// <summary>
		/// Number of digits after decimal point to display for a double with low precision
		/// </summary>
		public const int c_LowPrecision=3;
		/// <summary>
		/// Number of digits after decimal point to display for a double with high precision
		/// </summary>
		public const int c_HighPrecision=10;		
		#endregion
		#region Member Data
		/// <summary>
		/// String to store report
		/// </summary>		
		private StringBuilder m_ReportSb = new StringBuilder();	
		/// <summary>
		/// Saving mode. When set to true, embbed the separator. Otherwise don't. This allows generation of the
		/// same report in two slighlty differents ways.
		/// </summary>
		private bool m_ModePlain=true;
		#endregion
		#region Accessors
		#endregion
		#region Delegates and Events
		#endregion
		#region Constructors and Destructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public Report()
		{
			ReportReset();
		}
		#endregion
		#region Methods
		/// <summary>
		/// Append formatted title to report
		/// </summary>
		/// <param name="title">title to incorporate</param>
		public void AppendColumn(string title)
		{
			AppendString(FormatStringWithFiller(title,20,' '));
		}

		/// <summary>
		/// Append formatted Dotted column to report
		/// </summary>
		/// <param name="newString">New string to append</param>
		public void AppendDottedColumn(string newString)
		{
			AppendString(FormatStringWithFiller(newString,36,'.'));
		}

		/// <summary>
		/// Append cellContent double (no precision specified)
		/// </summary>
		/// <param name="cellToAppend">double to append</param>
		public void AppendDouble(double cellToAppend)
		{
			AppendString(cellToAppend.ToString());
		}

		/// <summary>
		/// Append a formatted double (with a given level of precision) to report
		/// </summary>
		/// <param name="doubleValue">Double to append</param>
		/// <param name="precision">Expected precision (from 1 to 20 significant digits)</param>
		/// <param name="flagForErrors">if true insert a special character to show error condition</param>
		/// <param name="flagForErrorsString">if flagForErrors is true, embed that string</param>
		public void AppendDouble(double doubleValue,int precision,bool flagForErrors,string flagForErrorsString)
		{
			StringBuilder sb=new StringBuilder();

			if (flagForErrors)
			{
				sb.Append(flagForErrorsString); //"*");
			}

			if (precision==0)
			{
				//Treat as an integer
				sb.Append(doubleValue.ToString());
			}
			else
			{
				sb.Append(doubleValue.ToString("f"+precision.ToString()));
			}
			AppendString(FormatString(sb.ToString()));
		}

		/// <summary>
		/// Append a formatted string to report
		/// </summary>
		/// <param name="text">string to append</param>
		public void AppendFormattedString(string text)
		{
			AppendString(FormatString(text));
		}

		/// <summary>
		/// Append a formatted integer to report
		/// </summary>
		/// <param name="integerValue">Integer value to append</param>
		public void AppendInteger(int integerValue)
		{
			AppendString(FormatString(integerValue.ToString()));
		}

		/// <summary>
		/// Append a string to report. Depending on m_ModePlain, we either export the string or the string+Csv separator
		/// </summary>
		/// <param name="newString">string to append</param>
		public void AppendString(string newString)
		{
			m_ReportSb.Append(newString);

			//If export mode is plain text, we only export the string, 
			//otherwise we export the string+Separator
			if (!m_ModePlain)
			{
				m_ReportSb.Append(c_CsvSeparator);
			}
		}

		/// <summary>
		/// Return a formatted boolean result
		/// </summary>
		/// <param name="booleanValue">Boolean value to incorporate</param>
		/// <returns>Formatted string to return</returns>
		private string FormatBoolean(bool booleanValue)
		{
			return FormatString(booleanValue?"YES":"NO");
		}

		/// <summary>
		/// Return a formatted string
		/// </summary>
		/// <param name="text">string to incorporate</param>
		/// <returns>Formatted string to return</returns>
		private string FormatString(string text)
		{
			return FormatStringWithFiller(text,20,' ');
		}

		/// <summary>
		/// Return formatted string for report
		/// </summary>
		/// <param name="text">string to incorporate</param>
		/// <param name="numberOfCharsAfter">number of characters to append after the text</param>
		/// <param name="filler">character to use as a filler</param>
		/// <returns>Formatted string to return</returns>
		public string FormatStringWithFiller(string text,int numberOfCharsAfter,char filler)
		{
			StringBuilder sb = new StringBuilder();	
			sb.Append(" ");
			sb.Append(text);
			while (sb.Length<numberOfCharsAfter) sb.Append(filler);
			return sb.ToString();
		}

		/// <summary>
		/// Retrieve report content as a string
		/// </summary>
		/// <returns></returns>
		public string GetReport()
		{
			return m_ReportSb.ToString();
		}

		/// <summary>
		/// In this mode, a Csv separator is appended to the report each time a new cell is inserted
		/// </summary>
		public void ModeCsv()
		{
			m_ModePlain=false;
		}

		/// <summary>
		/// In this mode, no Csv separator is appended to the report each time a new cell is inserted
		/// </summary>
		public void ModePlain()
		{
			m_ModePlain=true;
		}

		/// <summary>
		/// Append a newline to report
		/// </summary>
		public void NewLine()
		{			
			m_ReportSb.Append("\r\n");
		}

		/// <summary>
		/// Reset report
		/// </summary>
		public void ReportReset()
		{
			m_ReportSb.Remove(0,m_ReportSb.Length);
		}
		#endregion
		#region Event Handlers
		#endregion
	}

	#region Documentation Tags
	/// <summary>
	/// This class manages calibration lists
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Serge Taran</item>
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
	///			<item name="vssfile">$Workfile: Calibration.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/Devices/Calibration.cs $</item>
	///			<item name="vssrevision">$Revision: 21 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Staran $</item>
	///			<item name="vssdate">$Date: 2/13/06 6:39p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion	
	[Serializable]
	public class CalibrationList
	{		
		#region Constants		
		#endregion
		#region Member Data		
		/// <summary>
		/// ArrayList of rules
		/// </summary>
		private ArrayList m_Rules;		
		#endregion
		#region Accessors
		/// <summary>
		/// Return number of elements
		/// </summary>
		public int Count
		{
			get
			{
				return m_Rules.Count;
			}
		}
		/// <summary>
		/// Accessor for the ArrayList
		/// </summary>
		public OneValue[] Rules
		{
			get
			{
				return (OneValue[]) m_Rules.ToArray(typeof(OneValue));
			}
		}
		/// <summary>
		/// Accessor
		/// </summary>
		public OneValue this[int index]
		{
			get
			{
				if (index>=0 && index<Count)
					return (OneValue)m_Rules[index];
				else
					throw new ApplicationException("bad index.");
			}
		}
		#endregion
		#region Delegates and Events
		#endregion
		#region Constructors and Destructor		
		/// <summary>
		/// Default constructor
		/// </summary>
		public CalibrationList()
		{
			m_Rules=new ArrayList(0);
			ResetList();
		}
		#endregion
		#region Methods		
		/// <summary>
		/// Add a new numeric calibration parameter to the list (please call SetVal(handle,newValue) 
		/// to initialize the value)
		/// </summary>		
		/// <param name="description">Name</param>
		/// <param name="section">optional section information</param>
		/// <param name="type">Data type</param>		
		/// <param name="unit">Unit used</param>		
		/// <param name="newPrecision">Optional precision expected</param>		
		/// <param name="description2">optional description field</param>
		/// <param name="calibrationId">non zero value if the item has to be stored as camera calibration data</param>		
		/// <returns>corresponding handle</returns>
		public int Add(string description,
			int section,
			OneValue.Type type,OneValue.Units unit,						
			int newPrecision,
			string description2,int calibrationId)
		{						
			OneValue newRule=new OneValue();			
			newRule.Set(type,0.0,"",unit,0.0,0.0,newPrecision,description,
				description2,calibrationId,section);
			m_Rules.Add(newRule);
			//Returns handle
			return this.Count-1;
		}

		/// <summary>
		/// Add a new string calibration parameter to the list (please call SetStr(handle,string) to initialize the string)
		/// </summary>		
		/// <param name="description">Name</param>
		/// <param name="section">optional section information</param>		
		/// <param name="type">Data type</param>
		/// <param name="unit">Unit used</param>		
		/// <param name="description2">optional description field</param>
		/// <param name="calibrationId">non zero value if the item has to be stored as camera calibration data</param> 		
		/// <returns>corresponding handle</returns>
		public int Add(string description,
			int section,
			OneValue.Type type,OneValue.Units unit,			
			string description2,int calibrationId)
		{						
			OneValue newRule=new OneValue();
			newRule.Set(type,0.0,"",unit,0.0,0.0,0,description,description2,calibrationId,section);
			m_Rules.Add(newRule);
			//returns handle
			return this.Count-1;
		}

		/// <summary>
		/// Get the type corresponding to the calibration parameter specified
		/// </summary>
		/// <param name="index">parameter to query</param>
		/// <param name="type">corresponding type, if found</param>
		/// <returns>true if success, false otherwise</returns>
		public bool GetCalibrationType(int index, ref OneValue.Type type)
		{
			Results results=new Results();

			//Make sure index is in range
			if (index>=0 && index<Count)
			{								
				type=Rules[index].DataType;
				return true;
			}			
			return false;
		}

		/// <summary>
		/// Get the CalibrationId string corresponding to the index specified
		/// </summary>
		/// <param name="index">Index value to retrieve</param>
		/// <returns></returns>
		virtual public string GetEnumDescription(int index)
		{					
			//Make sure index is in range
			if (index>=0 && index<Count)
			{				
				return Rules[index].CalibrationId.ToString();
			}
			else
			{
				return "???";
			}
		}

		/// <summary>
		/// Get index (if available) corresponding to the calibration value specified as argument
		/// </summary>
		/// <param name="calibrationValue">calibration value to search for</param>
		/// <param name="index">corresponding index value if found</param>
		/// <returns>true if a corresponding index was found, false otherwise</returns>
		public bool GetIndexCorrespondingTo(int calibrationValue,ref int index)
		{					
			int i=0;
			//Loop through list to search for any occurence
			while (i<Count)
			{				
				if (Rules[i].CalibrationId==calibrationValue)
				{
					//If found update index value and exit
					index=i;
					return true;
				}
				i++;
			}
			//Nothing found, return false
			return false;
		}

		/// <summary>
		/// Get the calibration parameter from the list corresponding to the index specified 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="valueRead">OneValue to get</param>
		/// <returns>true if success, false otherwise</returns>
		public bool GetItem(int index,ref OneValue valueRead)
		{
			//Make sure index is in range
			if (index>=0 && index<Count)
			{				
				valueRead=Rules[index];
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// Set min and max values
		/// </summary>
		/// <param name="index">Index value</param>
		/// <param name="newMin">new minimum value</param>
		/// <param name="newMax">new maximum value</param>
		/// <returns>true if this particular indexed value was already set, false otherwise</returns>
		public bool SetMinMax(int index,double newMin,double newMax)
		{
			bool setTwice=false;
			if (index>=0 && index<Count)
			{				
				Rules[index].Min=newMin;
				Rules[index].Max=newMax;
				if (Rules[index].RangeDefined) 
				{
					setTwice=true;
				}
				Rules[index].RangeDefined=true;
			}			
			return setTwice;
		}

		/// <summary>
		/// Check range for all the values and build a report. Return number of errors found.
		/// </summary>
		/// <param name="report">Report to build</param>
		/// <param name="onlyShowStoppers">if true, will only dump the items that have been loaded
		/// and failed</param>
		/// <param name="modeCsv">true to get a Csv report, false for a plain text report</param>
		/// <returns>Number of errors found (0 if none)</returns>
		public int GetReport(ref string report,bool onlyShowStoppers,bool modeCsv)
		{
			int index=0;
			int counterInconsistencies=0;
			report="";
			Report localReport=new Report();
			if (modeCsv) 
			{
				localReport.ModeCsv();
			}
			
			localReport.AppendString("Item");
			
			bool newFieldsEnabled=true;
			if (newFieldsEnabled)
			{
				localReport.AppendString("Loaded");
				localReport.AppendString("Section");
			}
			localReport.AppendString("Status");
			localReport.AppendString("Unit");
			localReport.AppendString("Value");
			localReport.AppendString("Min");
			localReport.AppendString("Max");

			localReport.AppendString("Description");
			localReport.AppendString("Description2");
			localReport.AppendString("CalId");
			localReport.AppendString("CalName");
			localReport.NewLine();

			while (index<this.Count)
			{
				//If we only want to record the showstoppers
				if (onlyShowStoppers)
				{
					if (Rules[index].Loaded==true
						&& Rules[index].Check()==false
						)
					{
						//... we make sure the item is loaded and failed check
					}
					else
					{
						//if not the case, we skip that item and loop to the next
						index++;
						continue;
					}					
				}

				localReport.AppendInteger(index+1);
				if (newFieldsEnabled)
				{
					localReport.AppendColumn(Rules[index].Loaded?"Y":"N");
					localReport.AppendInteger(Rules[index].Section);
				}

				if (this.Rules[index].Check()==false)
				{
					counterInconsistencies++;
					localReport.AppendString("FAIL ");
				}
				else
				{
					localReport.AppendString("     ");
				}				

				localReport.AppendString(Rules[index].Unit.ToString());

				if (Rules[index].Numeric()==true)
				{
					localReport.AppendDouble(Rules[index].Value,Report.c_HighPrecision,true,Rules[index].GetErrorString());
					localReport.AppendDouble(Rules[index].Min,Report.c_HighPrecision,false,"");
					localReport.AppendDouble(Rules[index].Max,Report.c_HighPrecision,false,"");
				}
				else
				{
					localReport.AppendString(Rules[index].GetErrorString()+Rules[index].String);
					localReport.AppendString("");
					localReport.AppendString("");
				}
			
				localReport.AppendColumn(Rules[index].Description);
				localReport.AppendColumn(Rules[index].Where);
				localReport.AppendInteger(Rules[index].CalibrationId);
				
				//Only display m_CalibrationId when relevant (ie value is non zero)
				if (Rules[index].CalibrationId>0)
				{										
					int val=Rules[index].CalibrationId;
					localReport.AppendString("("+ GetEnumDescription(val)+")");
				}
				else
				{
					localReport.AppendString("");
				}
				
				localReport.NewLine();
				index++;
			}
			
			report=localReport.GetReport();
			return counterInconsistencies;
		}

		/// <summary>
		/// Reset the Computed flag for all values in the set.
		/// Because this flag is set to true whenever SetVal(...) is called, it allows 
		/// the user to see if all the values in the set where initialized or not
		/// </summary>
		public void ResetComputedFlagForAll()
		{
			int index=0; 
			while (index<Count)
			{
				Rules[index].Loaded=false; 
				index++;
			}
		}
		/// <summary>
		/// return the number of unloaded items
		/// </summary>
		public int GetTotalUnloaded()
		{
			int index=0; 
			int total=0;
			while (index<Count)
			{
				if (Rules[index].Loaded==false) total++;
				index++;
			}
			return total;
		}


		/// <summary>
		/// Empty the calibration list
		/// </summary>
		public void ResetList()
		{
			m_Rules.Clear();			
		}

		/// <summary>
		/// Set calibration item corresponding to the index specified
		/// </summary>
		/// <param name="index"></param>
		/// <param name="newValue">OneValue to set</param>
		/// <returns>true if success, false otherwise</returns>
		public bool SetItem(int index,OneValue newValue)
		{
			//Make sure index is in range
			if (index>=0 && index<Count)
			{				
				Rules[index]=newValue;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Set calibration item specified by index with the string provided
		/// </summary>
		/// <param name="index">specify item</param>
		/// <param name="newValue">new string</param>
		/// <returns>true if success, false if index invalid or type is numeric</returns>
		public bool SetStr(int index,string newValue)
		{
			//Make sure index is in range
			if (index>=0 && index<Count)
			{				
				//Only update if the underlying data stored is a string
				switch(Rules[index].DataType)	
				{
					case OneValue.Type.Flt04:return false;
					case OneValue.Type.Int04:return false;
					case OneValue.Type.Str04:Rules[index].String=newValue; Rules[index].Loaded=true; return true;
					case OneValue.Type.Str08:Rules[index].String=newValue; Rules[index].Loaded=true; return true;
					case OneValue.Type.Str16:Rules[index].String=newValue; Rules[index].Loaded=true; return true;
					default: return false;
				}					
			}
			return false;
		}

		/// <summary>
		/// Set calibration item specified by index with the numerical value provided
		/// </summary>
		/// <param name="index">specify item</param>
		/// <param name="newValue">new numeric value</param>
		/// <returns>true if success, false if index invalid or type is not numeric</returns>
		public bool SetVal(int index,double newValue)
		{
			//Make sure index is in range
			if (index>=0 && index<Count)
			{
				//Only update if the underlying data stored is numeric
				switch(Rules[index].DataType)	
				{
					case OneValue.Type.Flt04:Rules[index].Value=newValue; Rules[index].Loaded=true; return true;
					case OneValue.Type.Int04:Rules[index].Value=newValue; Rules[index].Loaded=true; return true;
					case OneValue.Type.Str04: return false;
					case OneValue.Type.Str08: return false;
					case OneValue.Type.Str16: return false;
					default: return false;
				}
			}
			return false;
		}
		#endregion
		#region Event Handlers
		#endregion
	}
}
