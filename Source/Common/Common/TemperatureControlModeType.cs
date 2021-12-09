using System;
using BioRad.Common;

namespace BioRad.PCR.ThermalCycler
{
	#region Documentation Tags
	/// <summary>
	/// Thermal cycler temperature control mode states.
	/// </summary>
	/// <remarks>
	/// Temperature control mode defaults to Algorithmic mode.
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
	///			<item name="vssfile">$Workfile: TemperatureControlModeType.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/Common/TemperatureControlModeType.cs $</item>
	///			<item name="vssrevision">$Revision: 10 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 10/01/04 8:16a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	[Serializable]
	public class TemperatureControlModeType
	{ 
		#region Member Data
		/// <summary>
		/// Generic Temperature control monitoring modes.
		/// </summary>
		public enum Mode 
		{
			/// <summary>Unassigned value.</summary>
			Unassigned = 0,
			/// <summary>
			/// Algorithmic.
			/// </summary>
			Algorithmic=8, 
			/// <summary>
			/// Sample probe.
			/// </summary>
			Sample=9,
			/// <summary>
			/// Block.
			/// </summary>
			Block=10,
		};
		/// <summary>
		/// 
		/// </summary>
		private Mode m_Value;
		#endregion

		#region Accessors
		/// <summary>
		/// Get/Set value for this object. Range checking.
		/// </summary>
		public Mode Value 
		{
			get
			{
				return m_Value;
			}
			set
			{
				m_Value = value;
			}
		}
		/// <summary>
		/// Get/Set value for this object. Range checking. Throws application execption
		/// on range error.
		/// </summary>
		/// <exception cref="System.ApplicationException">
		/// Throws application exception on out of range error.
		/// </exception>
		public int IntValue 
		{
			set
			{
				bool bValidValue = false;
				foreach(int i in Enum.GetValues(typeof(Mode)))
				{
					if ( i == (int)value )
					{
						bValidValue = true;
						break;
					}
				}
				if ( !bValidValue )
					throw new RangeErrorException().Range;

				m_Value = (Mode)value;
			}
		}
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Initializes a new instance of the TemperatureControlModeType class.
		/// </summary>
		public TemperatureControlModeType() 
		{
			m_Value = Mode.Unassigned;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Get string representation for current run state.
		/// </summary>
		/// <returns>string representation of value.</returns>
		public override string ToString()
		{
			return Enum.GetName(typeof(Mode), m_Value);
		}
		#endregion
	}
}
