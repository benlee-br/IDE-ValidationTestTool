using System;
namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// This class contains methods for handling floating point errors such as comparing two
	/// floating point values for equality.
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
	///			<item name="vssfile">$Workfile: FloatingPoint.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/FloatingPoint.cs $</item>
	///			<item name="vssrevision">$Revision: 9 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	[Serializable]
	public partial class FloatingPoint
	{

		#region Constructors and Destructor
		/// <summary>
		/// Initializes a new default instance of the FloatingPoint class.
		/// </summary>
		public FloatingPoint()
		{
		}
		#endregion

		#region Methods
		/// <summary>
		/// Representation of the value zero.
		/// </summary>
		/// <returns>Representation of the value zero.</returns>
		public static double zero()       
		{
			const double zero = (double)0;
			return zero;
		}
		/// <summary>
		/// Representation of the value one.
		/// </summary>
		/// <returns>Representation of the value one.</returns>
		public static double one()       
		{
			const double one = (double)1;
			return one;
		}
		/// <summary>
		/// It is very important to realize that any binary floating-point system can represent
		/// only a finite number of floating-point values in exact form. All other values must be 
		/// approximated by the closest representable value.		
		/// This method compares two values for equality within the specified epsilon.
		/// </summary>
		/// <param name="lhs">Left hand side value.</param>
		/// <param name="rhs">Right hand side value.</param>
		/// <param name="epsilon">Epsilon represents the smallest positive value greater than zero.</param>
		/// <returns>True if values are equal within epsilon.</returns>
		public static bool equal(double lhs, double rhs, double epsilon)
		{
			if (lhs == rhs)
				return true; // we are done here.

			if ( double.IsNaN(lhs) && double.IsNaN(rhs) )
				return true;

			if ( double.IsNaN(lhs) && !double.IsNaN(rhs) )
				return false;

			if ( !double.IsNaN(lhs) && double.IsNaN(rhs) )
				return false;

			if ( (lhs < zero()) != (rhs < zero()) )
				return false;// signs are different.

			double abs_lhs = Math.Abs(lhs); 
			double abs_rhs = Math.Abs(rhs); 

			// lhs's closeness to 0.
			if ( zero() == lhs ) 
				return (epsilon >= abs_rhs); // check if |rhs| is "close enough" to 0.

			// rhs's closeness to 0.
			if ( zero() == rhs ) 
				return (epsilon >= abs_lhs); // check if |lhs| is "close enough" to 0. 

			// closeness to each other
			if (double.MinValue > abs_rhs) 
			{ 
				// |b| is undistinguishable from 0
				if (double.MinValue > abs_lhs) 
					return true; 
				else
					return false;// |lhs| is distinguishable from 0

			} 
			else if (double.MinValue < abs_lhs) 
			{
				// |lhs| and |rhs| are distinguishable from 0
				// avoid overflow if |lhs| is very large and |rhs| is very small
				if ( (abs_rhs < one()) && (abs_lhs > abs_rhs * double.MaxValue) ) 
					return false;

				// avoid underflow if |lhs| is very small and |rhs| is very large 
				if ( (abs_rhs > one()) && (abs_lhs < abs_rhs * double.MinValue) ) 
					return false;

				double diff = abs_lhs - abs_rhs;
				diff = (zero() > diff ? -diff : diff);
				if ( diff <= epsilon )//they're equal.
					return true;
			} 
			else 
			{
				// |rhs| is distinguishable from 0
				// |lhs| is undistinguishable from 0 
				return false;
			};

			// they're different 
			return false;
		}
		
		/// <summary>
		/// Return a string corresponding to floatingPointValue. 
		/// String is formatted according to format. "None" will be returned if it is a NaN value
		/// </summary>
		/// <param name="floatingPointValue">double value</param>
		/// <param name="format">Format </param>
		/// <returns></returns>
		static public string DisplayString(double floatingPointValue,string format)
		{
			if (double.IsNaN(floatingPointValue))
			{
				//TODO: localize string?
				return "None";
			}
			else
			{
				return floatingPointValue.ToString(format);
			}
		}

		#endregion
	}
}
