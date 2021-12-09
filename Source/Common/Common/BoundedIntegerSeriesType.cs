using System;
using System.Diagnostics;
using System.Collections;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Class BoundedIntegerSeriesType supports a discrete sequential range of whole numbers.
	/// Supports a simple iteration over sequence of numbers.
	/// Throws application exception on range error.
	/// </summary>
	/// <remarks>
	/// TODO: This type is the original BoundedIntegerType, refactored to drop
	/// Value aspect and IComparable, with added a constructor taking BoundedIntegerType.
	/// Use this class to encapsulate the details for any sequential range of whole numbers.
	/// Note: This class can not be serialized using class XmlSerializer.
	/// Because enumerator is not a separate type, concurrent enumerations are not supported.
	/// Enumerator will be in the state it was previously.
	/// </remarks>
	/// <example>
	/// The following example illustrates the use of this class. 
	/// Lower bound is 0, upper bound is 8 and the length is 9.
	/// i equals the following values: 0, 1, 2, 3, 4, 5, 6, 7, 8
	/// <code>
	/// BoundedIntegerSeriesType Stages = new BoundedIntegerSeriesType(0, 9);
	/// foreach(int i in Stages)
	/// {
	/// }
	/// </code>
	/// </example>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Ralph Ansell</item>
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
	///			<item name="vssfile">$Workfile: BoundedIntegerSeriesType.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/Common/BoundedIntegerSeriesType.cs $</item>
	///			<item name="vssrevision">$Revision: 1 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Lvs $</item>
	///			<item name="vssdate">$Date: 5/25/04 11:04a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	[Serializable]
	public class BoundedIntegerSeriesType : IEnumerator, IEnumerable, ICloneable
	{
		#region Constants
		/// <summary>
		/// Absoulte minimum.
		/// </summary>
		private static readonly int c_MinValue = Int16.MinValue;
		/// <summary>
		/// Absoulte maximum.
		/// </summary>
		private static readonly int c_MaxValue = Int16.MaxValue;
		#endregion

		#region Member Data
		/// <summary>
		/// Lower bound of sequence. 
		/// </summary>
		protected int m_LowerBound = c_MinValue;
		/// <summary>
		/// Upper bound of sequence.
		/// </summary>
		protected int m_UpperBound = c_MaxValue;
		/// <summary>
		/// Enumerator index.
		/// </summary>
		private int m_EnumeratorIndex = 0;
		#endregion

		#region Accessors
		/// <summary>
		/// Returns number of sequential elements in object.
		/// </summary>
		public int Length
		{
			get
			{
				return (UpperBound-LowerBound+1);
			}
		}
		/// <summary>
		/// Get/Set the lower bound.
		/// </summary>
		/// <exception cref="System.ApplicationException">
		/// Throws application exception on out of range error.
		/// </exception>
		public int LowerBound 
		{
			get
			{
				return m_LowerBound;
			}
		}
		/// <summary>
		/// Get/Set the upper bound.
		/// </summary>
		/// <exception cref="System.ApplicationException">
		/// Throws application exception on out of range error.
		/// </exception>
		public int UpperBound
		{
			get
			{
				return m_UpperBound;
			}
		}	
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Initializes a new default instance of the BoundedIntegerSeriesType class.
		/// Lower and upper bounds define full range of possible Int16 values.
		/// </summary>
		public BoundedIntegerSeriesType()
		{
			Debug.Assert(m_LowerBound <= m_UpperBound);
			Reset();
		}

		/// <summary>
		/// Initializes a new instance of the BoundedIntegerSeriesType class
		/// using the lower and upper bounds of the given BoundedIntegerType.
		/// Enumerator is reset.
		/// </summary>
		/// <param name="bounded">defines lower and upper bounds of sequence.</param>
		public BoundedIntegerSeriesType(BoundedIntegerType bounded)
		{
			m_LowerBound = bounded.GetLowerBound;
			m_UpperBound = bounded.GetUpperBound;
			Debug.Assert(m_LowerBound <= m_UpperBound);
			Reset();
		}

		/// <summary>
		/// Initializes a new explicit instance of the BoundedIntegerSeriesType class.
		/// </summary>
		/// <remarks>
		/// Upper bound of sequence is the lower bound plus the number of discrete values in the
		/// sequence minus one.
		/// <example>
		/// Example: 
		/// The below BoundedIntegerSeriesType represents the sequence 0, 1, 2, 3, 4, 5, 6 ,7 ,8.
		/// <code>
		/// BoundedIntegerSeriesType i = new BoundedIntegerSeriesType(0, 9);
		///	</code>
		/// </example>	
		/// </remarks>
		/// <param name="lowerbound">Lower bound value of sequence.</param>
		/// <param name="length">Number of sequential values.</param>
		public BoundedIntegerSeriesType(int lowerbound, int length)
		{
			m_LowerBound = lowerbound;
			m_UpperBound = m_LowerBound + length - 1;
			Reset();
			Debug.Assert(m_LowerBound <= m_UpperBound);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Returns the enumeration of the value in the collection.
		/// </summary>
		/// <returns>An enumerator describing the value in the collection.
		/// </returns>
		public IEnumerator GetEnumerator() 
		{
			return (IEnumerator)this;
		}
		/// <summary>
		/// Advances the enumerator to the next element of the collection.
		/// </summary>
		/// <returns>true if the enumerator was successfully advanced to the next element; 
		/// false if the enumerator has passed the end of the collection.
		/// </returns>
		public bool MoveNext()
		{
			m_EnumeratorIndex++;
			if ( m_EnumeratorIndex > m_UpperBound )
				return false;
			return true;
		}
		/// <summary>
		/// Sets the enumerator to its initial position, which is before the first element in the collection.
		/// </summary>
		public void Reset()
		{
			m_EnumeratorIndex = m_LowerBound-1;
		}
		/// <summary>
		/// Gets the current enumerated value.
		/// </summary>
		public object Current
		{
			get 
			{
				return(m_EnumeratorIndex);
			}
		}
		/// <summary>
		/// Current enumerated value as a string.
		/// </summary>
		/// <returns>String that represents the current object.</returns>
		public override string ToString()
		{
			return m_EnumeratorIndex.ToString();
		}
		/// <summary>Create a clone of the BoundedIntegerSeriesType object.</summary>
		/// <returns>The cloned BoundedIntegerSeriesType object.</returns>
		public object Clone()
		{
			return this.MemberwiseClone();
		}
		#endregion
	}
}
