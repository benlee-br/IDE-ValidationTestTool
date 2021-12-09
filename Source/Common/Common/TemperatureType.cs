using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// This class represent temperatures.
	/// </summary>
	/// <remarks>
	/// Values that represent temperatures should be treated as (value/unit) pairs. A value that 
	/// represent a temperature has little meaning without knowing the units of the value.
	/// This class provides for the automatic conversion of expressions with mixed temperature units. 
	/// Range checking is done when changing the temperature. Throws application exception on range error.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Ralph Ansell</item>
	///			<item name="review">Last design/code review:</item>
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
	///			<item name="vssfile">$Workfile: TemperatureType.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/TemperatureType.cs $</item>
	///			<item name="vssrevision">$Revision: 35 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Pwalse $</item>
	///			<item name="vssdate">$Date: 11/30/07 4:27p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	[Serializable]
	[TypeConverter(typeof(TemperatureType.TemperatureTypeConverter))]
	public partial class TemperatureType : ICloneable, IComparable
	{
		/// <summary>
		/// Type converter for TemperatureType.
		/// </summary>
		public class TemperatureTypeConverter : TypeConverter 
		{
			/// <summary>
			/// Overrides the CanConvertFrom method of TypeConverter.
			/// The ITypeDescriptorContext interface provides the context for the conversion. 
			/// </summary>
			/// <param name="context"></param>
			/// <param name="sourceType">type to convert from</param>
			/// <returns>true if source type is a string</returns>
			public override bool CanConvertFrom(ITypeDescriptorContext context, 
				Type sourceType) 
			{
      
				if (sourceType == typeof(string)) 
				{
					return true;
				}
				return base.CanConvertFrom(context, sourceType);
			}
			/// <summary>
			/// Overrides the ConvertFrom method of TypeConverter.
			/// </summary>
			/// <param name="context"></param>
			/// <param name="culture">conversion is performed using this culture's format provider</param>
			/// <param name="value">value to convert (must be string)</param>
			/// <returns>TemperatureType object parsed from value string</returns>
			public override object ConvertFrom(ITypeDescriptorContext context, 
				CultureInfo culture, object value) 
			{
				if (value is string) 
				{
					return TemperatureType.Parse((string)value, culture);
				}
				return base.ConvertFrom(context, culture, value);
			}

			/// <summary>
			/// Overrides the ConvertTo method of TypeConverter to convert a
			/// TemperatureType object to a string using the ToString overload
			/// that takes a CultureInfo parameter.
			/// </summary>
			/// <param name="context"></param>
			/// <param name="culture">conversion is performed using this culture's format provider</param>
			/// <param name="value">value to convert (must be TemperatureType)</param>
			/// <param name="destinationType">must be string</param>
			/// <returns>string representation of object compatible with ConvertFrom</returns>
			public override object ConvertTo(ITypeDescriptorContext context, 
				CultureInfo culture, object value, Type destinationType) 
			{  
				if (destinationType == typeof(string)) 
				{
					return ((TemperatureType) value).ToString(culture);
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

		}

		#region Constants
		/// <summary>
		/// 
		/// </summary>
		public static readonly string c_TemperatureDefaultFormat = "#0.0";
		/// <summary>
		/// 
		/// </summary>
		public static readonly string c_TemperatureFormatNoDecimals = "#0";
		/// <summary>
		/// 
		/// </summary>
		private static readonly string c_FahrenheitSymbol = "°F";
		/// <summary>
		/// 
		/// </summary>
		private static readonly string c_CelciusSymbol = "°C";
		/// <summary>
		/// Supported Scales
		/// </summary>
		public enum Scales 
		{ 
			/// <summary>
			/// Celsius scale.
			/// </summary>
			Celcius=0, 
			/// <summary>
			/// Fahrenheit scale.
			/// </summary>
			Fahrenheit=1 
		};
		#endregion

		#region Member Data
		/// <summary>
		/// Machine representation for value of temperature.
		/// </summary>
		private double m_Value;
		/// <summary>
		/// Machine representation for scale of temperature.
		/// </summary>
		private Scales m_Scale = Scales.Celcius;
		/// <summary>
		/// Strategy pattern used for conversion between different scales.
		/// </summary>
		private abstract class Strategy
		{
			/// <summary>
			/// Abstract method for derived class to provide actual implementation for temperature conversion.
			/// </summary>
			/// <param name="arg">A temperature value to be converted. </param>
			/// <returns>Returns converted value.</returns>
			abstract public double Convert(double arg);
		}

		/// <summary>
		///  Celsius to Fahrenheit conversion = (1.8 * Celsius + 32.0).
		/// </summary>
		private class Celcius2Fahrenheit : Strategy
		{
			override public double Convert(double arg)
			{
				return 1.8 * arg + 32.0;
			}
		}

		/// <summary>
		/// Fahrenheit to Celsius conversion = (Fahrenheit - 32.0) / 1.8).
		/// </summary>
		private class Fahrenheit2Celcius : Strategy
		{
			override public double Convert(double arg)
			{
				return (arg - 32.0) / 1.8;
			}
		}
		/// <summary>
		/// Number of decimal digits for each scale.
		/// </summary>
		private int[] m_Digits = new int[] {1, 1};
		/// <summary>
		/// Minimum value for each scale.
		/// </summary>
		private double[] m_MinValue = new double[] {double.MinValue, double.MinValue};
		/// <summary>
		/// Maximum value for each scale.
		/// </summary>
		private double[] m_MaxValue = new double[] {double.MaxValue, double.MaxValue};
		/// <summary>
		/// Conversion method for each scale.
		/// </summary>
		private static Strategy[,] m_Conversions = new Strategy[,] {
					{null, new Celcius2Fahrenheit()},
					{new Fahrenheit2Celcius(), null}
															};
		#endregion

		#region Accessors
		/// <summary>
		/// A string that represents the units of the temperature.
		/// </summary>
		public string Symbol
		{
			get
			{
				return GetSymbol(m_Scale);
			}
		}
//		/// <summary>
//		/// A string that represents the value of the temperature.
//		/// </summary>
//		public string TemperatureDefaultFormat
//		{
//			get
//			{
//				return m_Value.ToString("#0.0");
//			}
//		}
		/// <summary>
		/// The get property returns a new object that's a member-by-member duplicate of the original object. 
		/// The set property does a member-by-member assignment.
		/// </summary>
		public TemperatureType Temperature 
		{
			get
			{
				return (TemperatureType)this.MemberwiseClone();
			}
			set
			{
				this.m_Value    = value.m_Value;
				this.m_Scale    = value.m_Scale;
				this.m_MinValue = value.m_MinValue;
				this.m_MaxValue = value.m_MaxValue;
				this.m_Digits   = value.m_Digits;
			}
		}
		/// <summary>
		/// The get property returns current scale. 
		/// The set property changes the scale. Temperature value is converted to new scale.
		/// </summary>
		public Scales Scale 
		{
			get
			{
				return m_Scale;
			}
			set
			{
				double t = Convert(this, value);//1. get new temperature as new scale.
				m_Scale = value;//2. change the scale.
				SetTemperature(t);//3. now change current temperature to new temperature.
			}
		}
		/// <summary>
		/// Represents the smallest possible value.
		/// </summary>
		public double Min 
		{
			get
			{
				return m_MinValue[(int)m_Scale];
			}
			set
			{
				m_MinValue[(int)m_Scale]=value;
			}
		}
		/// <summary>
		/// Represents the largest possible value.
		/// </summary>
		public double Max
		{
			get
			{
				return m_MaxValue[(int)m_Scale];
			}
			set
			{
				m_MaxValue[(int)m_Scale]=value;
			}
		}	
		/// <summary>
		/// Represents the number of decimal digits of accuracy.
		/// </summary>
		public int Digits 
		{
			get
			{
				return m_Digits[(int)m_Scale];
			}
		}
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Initializes a new instance of the TemperatureType class.  Default scale is set to Celsius.
		/// Temperature value is set zero.
		/// </summary>
		public TemperatureType()
		{
			m_Scale = TemperatureType.Scales.Celcius;//must set scale 1st. accessors use scale as index.
			SetTemperature(Min);
		}
		/// <summary>
		/// Explicitly initialize a new instance of the temperature class for a specific scale.
		/// </summary>
		/// <param name="scale">Temperature scale.</param>
		public TemperatureType(Scales scale)
		{
			m_Scale = scale;//must set scale 1st. accessors use scale as index.
			SetTemperature(Min);
		}
		/// <summary>
		/// Explicitly initialize a new instance of the temperature class.
		/// </summary>
		/// <param name="defaultValue">Initial default value.</param>
		/// <param name="scale">Temperature scale.</param>
		public TemperatureType(double defaultValue, Scales scale)
		{
			m_Scale = scale;//must set scale 1st. accessors use scale as index.
			SetTemperature(defaultValue);
		}
		/// <summary>
		/// Explicitly initialize a new instance of the temperature class.
		/// </summary>
		/// <param name="defaultValue">Initial default value.</param>
		/// <param name="scale">Temperature scale.</param>
		/// <param name="minTemp">Minimum temperature in specified scale.</param>
		/// <param name="maxTemp">Maximum temperature in specified scale.</param>
		/// <param name="digits">Number of digits of accuracy.</param>
		public TemperatureType(double defaultValue, Scales scale, double minTemp, double maxTemp, int digits)
		{
			m_Scale = scale;//must set scale 1st. accessors use scale as index.
			m_MinValue[(int)m_Scale] = minTemp;
			m_MaxValue[(int)m_Scale] = maxTemp;
			m_Digits[(int)m_Scale] = digits;
			SetTemperature(defaultValue);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Compares this instance to a specified object and returns an indication of their relative values.
		/// </summary>
		/// <param name="obj">An object to compare, or a null reference.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and value.
		/// <list type="bullet">
		/// <item>Less than zero - This instance is less than obj.</item>
		/// <item>Zero - This instance is equal to obj.</item>
		/// <item>Greater than zero - This instance is greater than obj.</item>
		/// </list>
		/// </returns>
		/// <exception cref="System.ArgumentException">Thrown when obj is not a TemperatureType</exception>
		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;
			if (obj is TemperatureType) 
				return Compare(this, (TemperatureType)obj);
			else
				throw new ArgumentException("argument is not a TemperatureType");
		}
		/// <summary>
		/// Compares the two operands and returns an indication of their relative values.
		/// </summary>
		/// <param name="lhs">Left hand side operand.</param>
		/// <param name="rhs">Right hand side operand.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and value.
		/// <list type="bullet">
		/// <item>Less than zero - This instance is less than obj.</item>
		/// <item>Zero - This instance is equal to obj.</item>
		/// <item>Greater than zero - This instance is greater than obj.</item>
		/// </list>
		/// </returns>
		/// <remarks>
		/// Any instance of TemperatureType, regardless of its value, is considered greater than a null reference.
		/// Parameter must be an instance of TemperatureType and physical quantities must be the same
		///  or a null reference; otherwise, an exception is thrown.
		/// </remarks>
		private static int Compare(TemperatureType lhs, TemperatureType rhs)
		{
			if ( (object)lhs == (object)rhs )
				return 0;
			if ( (object)lhs == null && (object)rhs != null )
				return -1;
			if ( (object)lhs != null && (object)rhs == null )
				return 1;

			int digits = Math.Max(lhs.Digits, rhs.Digits);
			double epsilon = Math.Pow(.1,digits);

			double rhsValue = Convert(rhs, lhs.Scale);

			if ( FloatingPoint.equal(lhs.m_Value, rhsValue, epsilon) )
				return 0;
			else if ( lhs.m_Value > rhsValue )
				return 1;
			else
				return -1;
		}
		/// <summary>
		/// Get current value as the built-in type used to represent a temperature. 
		/// </summary>
		/// <remarks>
		/// Restrict usage of this method to only when you really need the machine representation of the 
		/// temperature. Since this separates the value from its units.
		/// </remarks>
		public double GetTemperature()
		{
			return m_Value;
		}
		/// <summary>
		/// Set the built-in type used to represent a temperature. Scale does not change.
		/// </summary>
		/// <param name="temperature">New temperature.</param>
		/// <exception cref="System.ApplicationException">
		/// Throws application exception on range error.
		/// </exception>
		public void SetTemperature(double temperature)
		{
			if (  temperature < Min )
				throw new RangeErrorException().LowerBound;
			if ( temperature > Max )
				throw new RangeErrorException().UpperBound;

			m_Value = temperature;
		}
		/// <summary>
		/// Get current value as the built-in type used to represent a temperature for a specific scale. 
		/// </summary>
		/// <param name="scale">Scale to get value as.</param>
		/// <returns>Returns value as a double. Note that this seperates the value from its scale.</returns>
		/// <remarks>
		/// Restrict usage of this method to only when you really need the machine representation of the 
		/// temperature. Since this separates the value from its units.
		/// </remarks>
		public double GetAs(Scales scale)
		{
			return Convert(this, scale);
		}
		/// <summary>
		/// Compares two temperature objects for equality. Temperature objects do not have to
		/// be the same scale.
		/// </summary>
		/// <param name="lhs">Left hand side operand.</param>
		/// <param name="rhs">Right hand side operand.</param>
		/// <returns>true if equal, otherwise false.</returns>
		public static bool operator==(TemperatureType lhs, TemperatureType rhs)
		{
			if ( (object)lhs == (object)rhs )
				return true;
			if ( (object)lhs == null && (object)rhs == null )
				return true;
			if ( (object)lhs == null && (object)rhs != null )
				return false;
			if ( (object)lhs != null && (object)rhs == null )
				return false;

			int digits = Math.Max(lhs.Digits, rhs.Digits);
			double d = Math.Pow(.1,digits);
			return FloatingPoint.equal(lhs.m_Value, Convert(rhs, lhs.Scale), d);
		}
		/// <summary>
		/// Compares two temperature objects for inequality. Temperature objects do not have to
		/// be the same scale.
		/// </summary>
		/// <param name="lhs">Left hand side operand.</param>
		/// <param name="rhs">Right hand side operand.</param>
		/// <returns>true if not equal, otherwise false.</returns>
		public static bool operator!=(TemperatureType lhs, TemperatureType rhs)
		{
			return !(lhs == rhs);
		}
		/// <summary>
		/// Overrides the GetHashCode method.
		/// Uses the builtin types hash code.
		/// </summary>
		/// <returns>A hash value for the current object.</returns>
		public override int GetHashCode()
		{
			return m_Value.GetHashCode();
		}
		/// <summary>
		/// A string representation of the object.
		/// </summary>
		/// <returns>string</returns>
		public override string ToString() 
		{
			return ToString(c_TemperatureDefaultFormat, m_Scale, false);
		}		
		/// <summary>
		/// Formatted temperature.
		/// </summary>
		/// <param name="formatstring">format</param>
		/// <param name="scale">null defaults to ojects current scale.</param>
		/// <param name="withSymbol">true to get temperature with symbol.</param>
		/// <returns>Formatted temperature converted to specified scale, plus/minus symbol.</returns>
		public string ToString(string formatstring, Scales scale, bool withSymbol)
		{
			double d = Convert(this, scale);
			StringBuilder sb = new StringBuilder();
			if ( formatstring == null )
				sb.Append( d.ToString(c_TemperatureDefaultFormat) );
			else
				sb.Append( d.ToString(formatstring) );

			if ( withSymbol )
			{
				sb.Append( " " );
				sb.Append( GetSymbol(scale) );
			}

			return sb.ToString();
		}
		/// <summary>
		/// Temperature symbol.
		/// </summary>
		/// <param name="scale">null defaults to ojects current scale symbol.</param>
		/// <returns>Temperature symbol for specified scale.</returns>
		public static string GetSymbol(Scales scale)
		{
			string symbol = "";
			if( scale == Scales.Fahrenheit ) 
				symbol = c_FahrenheitSymbol;
			else if( scale == Scales.Celcius ) 
				symbol = c_CelciusSymbol;
			return symbol;
		}
		/// <summary>
		/// A parse-able string that represents the current Object in the
		/// given culture.
		/// </summary>
		/// <returns>string</returns>
		public string ToString(CultureInfo culture) 
		{
			double d = Math.Round(m_Value, Digits);
			return String.Format(culture, "{0},{1},{2},{3},{4}", d, this.Scale.ToString(), this.Min, this.Max, this.Digits);
		}
		/// <summary>
		/// Overrides the Equals method.
		/// </summary>
		/// <param name="obj">TemperatureType object</param>
		/// <returns>Returns true if the obj is equal to comparand.</returns>
		public override bool Equals(Object obj) 
		{
			if (obj is TemperatureType) 
			{
				TemperatureType p = (TemperatureType)obj;
				return (this == p);
			}
			return false;
		}
		/// <summary>
		/// Converts from one temperature scale to another.
		/// </summary>
		/// <param name="from">Temperature value to convert from.</param>
		/// <param name="toScale">Temperature value to convert to.</param>
		/// <returns>returns converted value.</returns>
		private static double Convert(TemperatureType from, Scales toScale)
		{
			double t = from.m_Value;
			if ( from.Scale != toScale )
				t = m_Conversions[(int)from.Scale, (int)toScale].Convert(from.m_Value);
			return t;
		}
		/// <summary>Create a clone of the TemperatureType object.</summary>
		/// <returns>The cloned TemperatureType object.</returns>
		public object Clone()
		{
			return this.MemberwiseClone();
		}

		/// <summary>
		/// Parse this type from string
		/// </summary>
		/// <param name="definition">string of the form: value, [scale [, min, max]] 
		/// where scale = Celcius (default) or Fahrenheit, and value, min and max are doubles</param>
		/// <returns>new custom type</returns>
		public static TemperatureType Parse(string definition)
		{
			return Parse(definition, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Parse this type from string using give format provider.
		/// </summary>
		/// <param name="definition">string of the form: value, [scale [, min, max, [digits]]] 
		/// where scale = Celcius (default) or Fahrenheit, and value, min and max are doubles,
		/// digits (of precision) are integer (default =1)</param>
		/// <param name="formatProvider">used specify culture of format conversion</param>
		/// <returns>new custom type</returns>
		public static TemperatureType Parse(string definition, IFormatProvider formatProvider)
		{
			TemperatureType temp;
			Scales scale = Scales.Celcius;
			double defaultValue, min, max;
			int digits = 1;
			if (definition.Trim() == String.Empty)
				temp = new TemperatureType();
			else
			{
				string [] settings = definition.Split(',');
//				foreach (string str in settings)
//				{
//					Console.WriteLine(str);
//				}
				defaultValue = ((IConvertible)settings[0]).ToDouble(formatProvider);
				switch (settings.Length)
				{
					case 1:
						temp = new TemperatureType(defaultValue,scale);
						break;
					case 2:
						scale  = (Scales) Enum.Parse(typeof(Scales), settings[1], true);
						temp = new TemperatureType(defaultValue,scale);
						break;
					case 4:
						scale  = (Scales) Enum.Parse(typeof(Scales), settings[1], true);
						min =  ((IConvertible)settings[2]).ToDouble(formatProvider);
						max =  ((IConvertible)settings[3]).ToDouble(formatProvider);
						temp = new TemperatureType(defaultValue,scale, min, max, digits);
						break;
					case 5:
						scale  = (Scales) Enum.Parse(typeof(Scales), settings[1], true);
						min =  ((IConvertible)settings[2]).ToDouble(formatProvider);
						max =  ((IConvertible)settings[3]).ToDouble(formatProvider);
						digits = ((IConvertible)settings[4]).ToInt32(formatProvider);
						temp = new TemperatureType(defaultValue,scale, min, max, digits);
						break;
					default:
                        throw new FormatException
                            (StringUtility.FormatString(Properties.Resources.ParseFailure_2, typeof(TemperatureType).Name, definition)); 
				}
			}
			return temp;
		}
		#endregion
	}
}
