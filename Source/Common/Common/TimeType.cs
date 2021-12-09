using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// This class should be used to encapsulate a length of time. 
	/// A length of time consists of a numeric value and a corresponding unit.
	/// Values that represent a length of time should be treated as (value/unit) pairs. A value that 
	/// represent a length of time has little meaning without knowing the units of the value.
	/// This class provides for the automatic conversion of expressions with mixed length of time units. 
	/// </summary>
	/// <remarks>
	/// Is data better represented by a built-in type or a class that encapsulates the built-in type 
	/// representing the data? 
	/// <para>
	/// Whether a concept is better represented by a built-in type or a class has nothing whatsoever to 
	/// do with size and speed, but with type safety, clarity of expression, encapsulation, 
	/// decoupling, extensibility, maintainability and reusability. 
	/// </para>
	/// <para>
	/// Built-in types where designed to represent a unit of storage on a machine. 
	/// Define a class to fit the problem rather than use an existing built-in type that was designed to 
	/// represent a unit of storage on a machine.  																																																																																																													
	/// </para>
	/// How does one represent the data in an application? 
	/// Representing data as a built-in type will force boundary and precision error checking to 
	/// be distributed in the application software.
	/// More than likely no error checking will be coded leading 
	/// to the possibility of boundary and precision errors going undetected.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Ralph Ansell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="conformancereview">Conformance review: 2/02/04, Ralph Ansell.</item>
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
	///			<item name="vssfile">$Workfile: TimeType.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/TimeType.cs $</item>
	///			<item name="vssrevision">$Revision: 31 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	[Serializable]
	[TypeConverter(typeof(TimeType.TimeTypeConverter))]
	public partial class TimeType : ICloneable, IComparable
	{ 
		/// <summary>
		/// Type converter for TimeType.
		/// </summary>
		public class TimeTypeConverter : TypeConverter 
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
					return TimeType.Parse((string)value, culture);
				}
				return base.ConvertFrom(context, culture, value);
			}
/*
			/// <summary>
			/// Overrides the ConvertTo method of TypeConverter to convert a
			/// TimeType object to a string using the ToString overload
			/// that takes a CultureInfo parameter.
			/// </summary>
			/// <param name="context"></param>
			/// <param name="culture">conversion is performed using this culture's format provider</param>
			/// <param name="value">value to convert (must be TemperatureType)</param>
			/// <param name="destinationType">must be string</param>
			/// <returns>string representation of object compatible with ConvertFrom</returns>
			private override object ConvertTo(ITypeDescriptorContext context, 
				CultureInfo culture, object value, Type destinationType) 
			{  
				if (destinationType == typeof(string)) 
				{
					return ((TimeType)value).ToString(culture);
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
			*/
		}

		#region Constants
		/// <summary>
		/// Supported units of a length of time.
		/// </summary>
		public enum Units 
		{ 
			/// <summary>
			/// MilliSeconds.
			/// </summary>
			MilliSeconds=0,
			/// <summary>
			/// Seconds.
			/// </summary>
			Seconds, 
			/// <summary>
			/// Minutes.
			/// </summary>
			Minutes,
			/// <summary>
			/// Hours.
			/// </summary>
			Hours
		};
		#endregion

		#region Member Data
		/// <summary>
		/// Number of decimal digits of accuracy for a length of time.
		/// </summary>
		private int m_Digits = 10;
		/// <summary>
		/// Minimum length of time value.
		/// </summary>
		private double m_MinValue = double.MinValue;
		/// <summary>
		/// Maximum length of time value.
		/// </summary>
		private double m_MaxValue = double.MaxValue;
		/// <summary>
		/// Machine representation of a length of time.
		/// </summary>
		private double m_Value = double.MinValue;
		/// <summary>
		/// Length of time units.
		/// </summary>
		private Units m_Units = Units.Seconds;
		/// <summary>
		/// Conversion factors between lengths of time units.
		/// </summary>
		static private double[,] m_ConfFactor = new double[,] {
				{1.0,				1.0/1000.0,		1.0/(60.0*1000.0),	1.0/(3600.0*1000.0)}, 
				{1000.0,			1.0,			1.0/60.0,		1.0/3600.0}, 
				{60.0*1000.0,		60.0,			1.0,			1.0/60.0}, 
				{3600.0*1000.0,		3600.0,			60.0,			1.0} 
															};
		#endregion

		#region Accessors
		/// <summary>
		/// Get current length of time units or change to new length of time units.
		/// Value is convert to new length of time units when changing the units.
		/// </summary>
		public Units Unit 
		{
			get
			{
				return m_Units;
			}
			set
			{
				double d = m_ConfFactor[(int)this.Unit,(int)value];
				// if the min/max are not the default values 
				// convert them to the new units first 
				if(m_MinValue != double.MinValue)
					m_MinValue *= d;
				if(m_MaxValue != double.MaxValue)
					m_MaxValue *= d;

				Time *= d;
				m_Units = value;
			}
		}
		/// <summary>
		/// Get the current value as its built-in type representation. Units are not changed.
		/// Setting the value does not change the units.
		/// </summary>
		/// <exception cref="System.ApplicationException">
		/// Throws application exception on out of range error.
		/// </exception>
		public double Time 
		{
			get
			{
				return m_Value;
			}
			set
			{
				if (  value < m_MinValue )
					throw new RangeErrorException().LowerBound;
				if (  value > m_MaxValue )
					throw new RangeErrorException().UpperBound;

				m_Value = value;
			}
		}
		/// <summary>
		/// Represents the smallest possible legth of time.
		/// </summary>
		public double Min 
		{
			get
			{
				return m_MinValue;
			}
		}

		/// <summary>
		/// Represents the largest possible length of time.
		/// </summary>
		public double Max
		{
			get
			{
				return m_MaxValue;
			}
		}	

		/// <summary>
		/// Represents the number of decimal digits of accuracy for length of time.
		/// </summary>
		public int Digits 
		{
			get
			{
				return m_Digits;
			}
		}
		#endregion

		#region Constructors and Destructor	
		/// <summary>
		/// Initializes a new instance of the TimeType class. Units defaults to seconds.
		/// </summary>summary>
		public TimeType()
		{
			m_Value = 0;
			m_Units = TimeType.Units.Seconds;
			m_MinValue = double.MinValue;
			m_MaxValue = double.MaxValue;
			m_Digits = 10;
		}	
		/// <summary>
		/// Explicitly initialize a new instance of the TimeType class.
		/// </summary>
		/// <param name="defaultValue">Initial value for length of time.</param>
		/// <param name="units">Units of length of time.</param>
		public TimeType(double defaultValue, TimeType.Units units)
		{
			m_Units = units;
			Time = defaultValue;
			m_MinValue = double.MinValue;
			m_MaxValue = double.MaxValue;
			m_Digits = 10;
		}
		/// <summary>
		/// Explicitly initialize a new instance of the TimeType class.
		/// </summary>
		/// <param name="defaultValue">Initial value for length of time.</param>
		/// <param name="units">Units of length of time.</param>
		/// <param name="minTime">Minimum length of time for specified units.</param>
		/// <param name="maxTime">Maximum length of time for specified units.</param>
		/// <param name="digits">Number digits of accuracy.</param>
		public TimeType(double defaultValue, TimeType.Units units, double minTime, double maxTime, int digits)
		{//todo should remove this constructor at some future date.
			m_Units = units;
			m_MinValue = minTime;
			m_MaxValue = maxTime;
			m_Digits = digits;
			Time = defaultValue;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Returns the current length of time, converted to specified units. Does not change units
		/// of object.
		/// </summary>
		/// <param name="unit">Units.</param>
		/// <returns>Length of time convert to specified time units.</returns>
		public double GetAs(Units unit)
		{
			double d = m_ConfFactor[(int)this.Unit,(int)unit];
			return 	m_Value * d;
		}
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
		/// <exception cref="System.ArgumentException">Thrown when obj is not a TimeType</exception>
		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;
			if (obj is TimeType) 
				return Compare(this, (TimeType)obj);
			else
				throw new ArgumentException("argument is not a TimeType");
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
		/// Any instance of TimeType, regardless of its value, is considered greater than a null reference.
		/// Parameter must be an instance of TimeType and physical quantities must be the same
		///  or a null reference; otherwise, an exception is thrown.
		/// </remarks>
		private static int Compare(TimeType lhs, TimeType rhs)
		{
			if ( (object)lhs == (object)rhs )
				return 0;
			if ( (object)lhs == null && (object)rhs != null )
				return -1;
			if ( (object)lhs != null && (object)rhs == null )
				return 1;

			int digits = Math.Max(lhs.Digits, rhs.Digits);
			double epsilon = Math.Pow(.1,digits);

			double rhsValue = Convert(rhs, lhs);

			if ( FloatingPoint.equal(lhs.Time, rhsValue, epsilon) )
				return 0;
			else if ( lhs.Time > rhsValue )
				return 1;
			else
				return -1;
		}
		/// <summary>
		/// Compares two length of time objects for equality. Length of time objects do not have to
		/// be the same units. Right hand side object is converted to left hand side units before comparison
		/// is made.
		/// </summary>
		/// <param name="lhs">Left hand side operand.</param>
		/// <param name="rhs">Right hand side operand.</param>
		/// <returns>true if equal, otherwise false.</returns>
		public static bool operator==(TimeType lhs, TimeType rhs)
		{
			return ( Compare(lhs, rhs) == 0 ) ? true : false;
		}
		/// <summary>
		/// Compares two length of time objects for inequality. Length of ime objects do not have to
		/// be the same units. Right hand side object is converted to left hand side units before comparison is made.
		/// </summary>
		/// <param name="lhs">Left hand side operand.</param>
		/// <param name="rhs">Right hand side operand.</param>
		/// <returns>true if not equal, otherwise false.</returns>
		public static bool operator!=(TimeType lhs, TimeType rhs)
		{
			return ( Compare(lhs, rhs) != 0 ) ? true : false;
		}
		/// <summary>
		/// Compares two length of time objects for greater than. Length of time objects do not have to
		/// be the same units. Right hand side object is converted to left hand side units before comparison is made.
		/// </summary>
		/// <param name="lhs">Left hand side operand.</param>
		/// <param name="rhs">Right hand side operand.</param>
		/// <returns>true lhs is greater than rhs, otherwise false.</returns>
		public static bool operator>(TimeType lhs, TimeType rhs)
		{
			return ( Compare(lhs, rhs) > 0 ) ? true : false;
		}
		/// <summary>
		/// Compares two length of time objects for less than. Length of time objects do not have to
		/// be the same units. Right hand side object is converted to left hand side units before comparison is made.
		/// </summary>
		/// <param name="lhs">Left hand side operand.</param>
		/// <param name="rhs">Right hand side operand.</param>
		/// <returns>true lhs is less than rhs, otherwise false.</returns>
		public static bool operator<(TimeType lhs, TimeType rhs)
		{
			return ( Compare(lhs, rhs) < 0 ) ? true : false;
		}
		/// <summary>
		/// Adds two time objects. Time objects do not have to
		/// be the same units. Right hand side object is converted to left hand side units before addition is done.
		/// </summary>
		/// <param name="lhs">Left hand side operand.</param>
		/// <param name="rhs">Right hand side operand.</param>
		/// <returns>
		/// New time object equal to sum of two operands. Units of new time object is set to lhs operand units.
		/// </returns>
		public static TimeType operator+(TimeType lhs, TimeType rhs)
		{
			TimeType t = new TimeType();
			t.m_Value = double.NaN;
			if ( (object)lhs != null && (object)rhs != null )
			{
				t.Unit = lhs.Unit;
				t.Time = lhs.Time + Convert(rhs, lhs);
			}
			return t;
		}
		/// <summary>
		/// Subtract two time objects. Time objects do not have to
		/// be the same units. Right hand side object is converted to left hand side units before subtraction is done.
		/// </summary>
		/// <param name="lhs">Left hand side operand.</param>
		/// <param name="rhs">Right hand side operand.</param>
		/// <returns>
		/// New time object equal to difference of two operands. Units of new time object is set to lhs operand units.
		/// </returns>
		public static TimeType operator-(TimeType lhs, TimeType rhs)
		{
			TimeType t = new TimeType();
			t.m_Value = double.NaN;
			if ( (object)lhs != null && (object)rhs != null )
			{
				t.Unit = lhs.Unit;
				t.Time = lhs.Time - Convert(rhs, lhs);
			}
			return t;
		}
		/// <summary>
		/// Divides the first operand by its second. Time objects do not have to
		/// be the same units. Right hand side object is converted to left hand side units before subtraction is done.
		/// </summary>
		/// <param name="lhs">Left hand side operand.</param>
		/// <param name="rhs">Right hand side operand.</param>
		/// <returns>
		/// New time object equal to ratio of two operands. 
		/// Units of new time object is set to lhs operand units.
		/// </returns>
		public static TimeType operator/(TimeType lhs, TimeType rhs)
		{
			TimeType t = new TimeType();
			t.m_Value = double.NaN;

			if ( (object)lhs != null && (object)rhs != null )
			{
				t.Unit = lhs.Unit;
				double drhs = Convert(rhs, lhs);
				if ( drhs == 0 )
					throw new ApplicationException("divide by zero.");

				t.Time = lhs.Time / drhs;
			}
			return t;
		}
		/// <summary>
		/// Converts from one length of time units to another.
		/// </summary>
		/// <param name="from">Length of time value to convert from.</param>
		/// <param name="to">Length of time value to convert to.</param>
		/// <returns>returns converted value.</returns>
		static private double Convert(TimeType from, TimeType to)
		{
			double t = from.m_Value;
			if ( from.Unit != to.Unit )
			{
				double d = m_ConfFactor[(int)from.Unit,(int)to.Unit];
				t *= d;
			}
			return t;
		}
		/// <summary>
		/// Converts current time to HH:MM:SS,sss format HH=hours, MM=minutes, SS=seconds, sss=milliseconds.
		/// </summary>
		/// <returns>String representation of length of time.</returns>
		public string ToStringHourMinuteSecond()
		{
			TimeSpan ts = ToTimeSpan();
			return String.Format("{0:d2}:{1:d2}:{2:d2}", ts.Hours, ts.Minutes, ts.Seconds);
		}
		/// <summary>
		/// Converts current time to MM:SS,sss format MM=minutes, SS=seconds, sss=milliseconds.
		/// </summary>
		/// <returns>String representation of length of time.</returns>
		public string ToStringMinuteSecond()
		{
			TimeSpan ts;
			if(this.m_Value < 0)
			{
				TimeType newTime = this;
				newTime.Time = -(newTime.Time);
				ts = TimeSpan.FromMilliseconds(newTime.GetAs(TimeType.Units.MilliSeconds));	
				return String.Format("-{0:d2}:{1:d2}", (ts.Hours * 60) + ts.Minutes, ts.Seconds);
			}
			else
			{
				ts = ToTimeSpan();
				return String.Format("{0:d2}:{1:d2}", (ts.Hours * 60) + ts.Minutes, ts.Seconds);
			}
		}
		/// <summary>
		/// A string that represents the current Object.
		/// </summary>
		/// <returns>string</returns>
		public override string ToString()
		{
			double d = Math.Round(m_Value, Digits);
			return d.ToString();
		}
		/// <summary>
		/// Gets TimeSpan object equivalent to the current Object.
		/// </summary>
		/// <returns>TimeSpan object</returns>
		public TimeSpan ToTimeSpan()
		{
			return TimeSpan.FromMilliseconds(this.GetAs(TimeType.Units.MilliSeconds));
		}

		/// <summary>
		/// Gets DateTime object equivalent to the current Object.
		/// </summary>
		/// <returns>DateTime object</returns>
		public DateTime ToDateTime()
		{
			return new DateTime(this.ToTimeSpan().Ticks);
		}
		/// <summary>
		/// Overrides the GetHashCode method.
		/// Uses the built-in types hash code.
		/// </summary>
		/// <returns>A hash value for the current object.</returns>
		public override int GetHashCode()
		{
			return m_Value.GetHashCode();
		}
		/// <summary>
		/// Overrides the Equals method.
		/// </summary>
		/// <param name="obj">TemperatureType object</param>
		/// <returns>Returns true if the obj is equal to comparand.</returns>
		public override bool Equals(Object obj) 
		{
			if (obj != null && obj is TimeType) 
			{
				TimeType p = (TimeType)obj;
				return (this == p);
			}
			return false;
		}
		/// <summary>Create a clone of the TimeType object.</summary>
		/// <returns>The cloned TimeType object.</returns>
		public object Clone()
		{
			return this.MemberwiseClone();
		}
		/// <summary>
		/// Constructs a TimeType from a time indicated by a specified string.
		/// </summary>
		/// <param name="s">A string, <see cref="System.TimeSpan.Parse(string)"/> for specification of the form.</param>
		/// <param name="units">Units of the new TimeType object.</param>
		/// <returns>A TimeType that corresponds to s.</returns>
		public static TimeType Parse(string s, TimeType.Units units)
		{
			TimeSpan ts = TimeSpan.Parse(s);
			double v = ts.TotalMilliseconds;
			double d = m_ConfFactor[(int)TimeType.Units.MilliSeconds,(int)units];
			v *= d;
			return new TimeType(v,units);
		}
		/// <summary>
		/// Parse this type from string
		/// </summary>
		/// <param name="definition">string of the form: value, [scale [, min, max]] 
		/// where scale = Celcius (default) or Fahrenheit, and value, min and max are doubles</param>
		/// <returns>new custom type</returns>
		public static TimeType Parse(string definition)
		{
			return Parse(definition, CultureInfo.CurrentCulture);
		}
		/// <summary>
		/// Parse this type from string using give format provider.
		/// </summary>
		/// <param name="definition">string of the form: value, [unit [, min, max, [digits]]] 
		/// where unit = seconds (default) or anyother unit defined in TimeType.Units, 
		/// and value, min and max are doubles, digits (of precision) are integer (default =1)</param>
		/// <param name="formatProvider">used specify culture of format conversion</param>
		/// <returns>new custom type</returns>
		public static TimeType Parse(string definition, IFormatProvider formatProvider)
		{
			TimeType time;
			Units unit = Units.Seconds;
			double defaultValue, min, max;
			int digits = 1;
			if (definition.Trim() == String.Empty)
				time = new TimeType();
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
						time = new TimeType(defaultValue,unit);
						break;
					case 2:
						unit  = (Units) Enum.Parse(typeof(Units), settings[1], true);
						time = new TimeType(defaultValue,unit);
						break;
					case 4:
						unit  = (Units) Enum.Parse(typeof(Units), settings[1], true);
						min =  ((IConvertible)settings[2]).ToDouble(formatProvider);
						max =  ((IConvertible)settings[3]).ToDouble(formatProvider);
						time = new TimeType(defaultValue,unit, min, max, digits);
						break;
					case 5:
						unit  = (Units) Enum.Parse(typeof(Units), settings[1], true);
						min =  ((IConvertible)settings[2]).ToDouble(formatProvider);
						max =  ((IConvertible)settings[3]).ToDouble(formatProvider);
						digits = ((IConvertible)settings[4]).ToInt32(formatProvider);
						time = new TimeType(defaultValue,unit, min, max, digits);
						break;
					default:
                        throw new FormatException
                        (StringUtility.FormatString(Properties.Resources.ParseFailure_2, typeof(TimeType).Name, definition));
				}
			}
			return time;
		}
		#endregion
	} 
}
