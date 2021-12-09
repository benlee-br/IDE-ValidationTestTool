using System;
using System.Globalization;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// This class should be used for any numeric values that represent volume. Values that
	/// represent volume should be treated as (value/unit) pairs. A value that 
	/// represent a volume has little meaning without knowing the units of the value.
	/// This class provides for the automatic conversion of expressions with mixed volume units. 
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
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
	///			<item name="vssfile">$Workfile: VolumeType.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/VolumeType.cs $</item>
	///			<item name="vssrevision">$Revision: 19 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	[Serializable]
	public partial class VolumeType : ICloneable, IComparable
	{ 
		#region Constants
		/// <summary>
		/// Volume units supported.
		/// </summary>
		public enum Units //todo:need to make sure all units are supported that are in GUI.
		{ 
			/// <summary>
			///  A metric unit of volume equal to one millionth of a liter.
			/// </summary>
			MicroLiter=0,
			/// <summary>
			/// A metric unit of volume equal to one thousandth of a liter.
			/// </summary>
			MilliLiter
		};
		#endregion

		#region Member Data
		/// <summary>
		/// Number of decimal digits of accuracy for volume.
		/// </summary>
		private int m_Digits = 10;
		/// <summary>
		/// Minimum volume value.
		/// </summary>
		private double m_MinValue = double.MinValue;
		/// <summary>
		/// Maximum volume value.
		/// </summary>
		private double m_MaxValue = double.MaxValue;
		/// <summary>
		/// Machine representation of volume.
		/// </summary>
		private double m_Value = double.MinValue;
		/// <summary>
		/// Volume units.
		/// </summary>
		private Units m_Units = Units.MicroLiter;
		/// <summary>
		/// Conversion factors between volume units.
		/// </summary>
		static private double[,] m_ConfFactor = new double[,] {
				{1.0, 1.0/1000.0}, 
				{1000.0, 1.0} 
				};
		#endregion

		#region Accessors
		/// <summary>
		/// Get current volume units or change to new volume units.
		/// Value is convert to new volume units when changing the units.
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
				// convert min/max to new units first.
				m_MinValue *= d;
				m_MaxValue *= d;
				Volume *= d;
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
		public double Volume 
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
		/// Represents the smallest possible value.
		/// </summary>
		public double Min 
		{
			get
			{
				return m_MinValue;
			}
		}

		/// <summary>
		/// Represents the largest possible value.
		/// </summary>
		public double Max
		{
			get
			{
				return m_MaxValue;
			}
		}	

		/// <summary>
		/// Represents the number of decimal digits of accuracy.
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
		/// Initializes a new instance of the VolumeType class.
		/// </summary>summary>
		public VolumeType()
		{
			m_Value = 0;
			m_Units = VolumeType.Units.MicroLiter;
			m_MinValue = double.MinValue;
			m_MaxValue = double.MaxValue;
			m_Digits = 10;
		}	
		/// <summary>
		/// Explicitly initialize a new instance of the VolumeType class.
		/// </summary>
		/// <param name="defaultValue">Initial value.</param>
		/// <param name="units">Volume units.</param>
		/// <param name="minVolume">Minimum volume in specified volume units.</param>
		/// <param name="maxVolume">Maximum volume in specified volume units.</param>
		/// <param name="digits">Number digits of accuracy.</param>
		public VolumeType(double defaultValue, VolumeType.Units units, double minVolume, double maxVolume, int digits)
		{
			m_Units = units;
			m_MinValue = minVolume;
			m_MaxValue = maxVolume;
			m_Digits = digits;
			Volume = defaultValue;
		}
		/// <summary>
		/// Explicitly initialize a new instance of the VolumeType class.
		/// </summary>
		/// <param name="defaultValue">Initial value.</param>
		/// <param name="units">Volume units.</param>
		public VolumeType(double defaultValue, VolumeType.Units units)
		{
			m_Units = units;
			Volume = defaultValue;
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
		/// <exception cref="System.ArgumentException">Thrown when obj is not a VolumeType</exception>
		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;
			if (obj is VolumeType) 
				return Compare(this, (VolumeType)obj);
			else
				throw new ArgumentException("argument is not a VolumeType");
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
		/// Any instance of VolumeType, regardless of its value, is considered greater than a null reference.
		/// Parameter must be an instance of VolumeType and physical quantities must be the same
		///  or a null reference; otherwise, an exception is thrown.
		/// </remarks>
		private static int Compare(VolumeType lhs, VolumeType rhs)
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

			if ( FloatingPoint.equal(lhs.Volume, rhsValue, epsilon) )
				return 0;
			else if ( lhs.Volume > rhsValue )
				return 1;
			else
				return -1;
		}
		/// <summary>
		/// Returns the current value for object converted to specified volume units. Does not change volume units
		/// of object.
		/// </summary>
		/// <param name="unit">Volume Units.</param>
		/// <returns>Volume value of object convert to specified volume units.</returns>
		public double GetAs(Units unit)
		{
			double d = m_ConfFactor[(int)this.Unit,(int)unit];
			return 	m_Value * d;
		}
		/// <summary>
		/// Compares two volume objects for equality. Volume objects do not have to
		/// be the same units. Right hand side object is converted to left hand side units before comparison.
		/// </summary>
		/// <param name="lhs">Left hand side operand.</param>
		/// <param name="rhs">Right hand side operand.</param>
		/// <returns>true if equal, otherwise false.</returns>
		public static bool operator==(VolumeType lhs, VolumeType rhs)
		{
			return ( Compare(lhs, rhs) == 0 ) ? true : false;
		}

		/// <summary>
		/// Compares two volume objects for inequality. Volume objects do not have to
		/// be the same scale. Right hand side object is converted to left hand side units before comparison.
		/// </summary>
		/// <param name="lhs">Left hand side operand.</param>
		/// <param name="rhs">Right hand side operand.</param>
		/// <returns>true if not equal, otherwise false.</returns>
		public static bool operator!=(VolumeType lhs, VolumeType rhs)
		{
			return ( Compare(lhs, rhs) != 0 ) ? true : false;
		}
		/// <summary>
		/// Converts from one volume units to another.
		/// </summary>
		/// <param name="from">Volume value to convert from.</param>
		/// <param name="to">Volume value to convert to.</param>
		/// <returns>returns converted value.</returns>
		static private double Convert(VolumeType from, VolumeType to)
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
		/// A string that represents the current objedts value.
		/// </summary>
		/// <returns>string</returns>
		public override string ToString()
		{
			double d = Math.Round(m_Value, Digits);
			return d.ToString();
		}
		/// <summary>
		/// A string that represents the current object value with its units.
		/// </summary>
		/// <param name="provider">Formatting for numeric conversion. Must be
		/// type CultureInfo for units to be included.</param>
		/// <returns>string formatted using given format provider</returns>
		public string ToStringUnits(IFormatProvider provider)
		{
			// Units are be provided by string resource for globalization.
			double d = Math.Round(m_Value, Digits);
			if (provider is CultureInfo)
			{
				if( m_Units == Units.MicroLiter ) 
				{
                    return StringUtility.FormatString
                    (Properties.Resources.ResourceManager.GetString
                    ("Microliter_1", provider as CultureInfo), d);
                    //StringResource sr = CommonStrResNames.Instance.GetStringResource(
                    //    CommonStrResNames.Names.Microliter_1, d);
                    //return sr.Localize((CultureInfo) provider);
				}
				else if( m_Units == Units.MilliLiter ) 
				{
                    return StringUtility.FormatString
                   (Properties.Resources.ResourceManager.GetString
                   ("Milliliter_1", provider as CultureInfo), d);
                    //StringResource sr = CommonStrResNames.Instance.GetStringResource(
                    //    CommonStrResNames.Names.Milliliter_1, d);
                    //return sr.Localize((CultureInfo)provider);
				}
			}
			return m_Value.ToString(provider);
		}

		/// <summary>
		/// A string that represents the current object value with its units.
		/// </summary>
		/// <remarks>Formatted using current culture</remarks>
		/// <returns>string</returns>
		public string ToStringUnits()
		{
			return ToStringUnits(CultureInfo.CurrentCulture);
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
		/// <param name="obj">VolumeType object</param>
		/// <returns>Returns true if the obj is equal to comparand.</returns>
		public override bool Equals(Object obj) 
		{
			if (obj != null && obj is VolumeType) 
			{
				VolumeType p = (VolumeType)obj;
				return (this == p);
			}
			return false;
		}
		/// <summary>Create a clone of the VolumeType object.</summary>
		/// <returns>The cloned VolumeType object.</returns>
		public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion
	} 
}
