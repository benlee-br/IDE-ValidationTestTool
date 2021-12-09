using System;
using System.Text;
using System.Diagnostics;

namespace BioRad.Common.PhysicalQuanities
{
	#region Documentation Tags
	/// <summary>
	/// The amount of a substance.
	/// </summary>
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
	///			<item name="vssfile">$Workfile: MoleType.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/PhysicalQuanities/MoleType.cs $</item>
	///			<item name="vssrevision">$Revision: 3 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 3/30/10 8:21a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	[Serializable]
	public class MoleType : MetricBaseUnit
	{
		//#region Constants
		//#endregion

		//#region Menber Data
		//#endregion

		//#region Accessors
		//#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Initializes a new instance of the MoleType class.
		/// </summary>
		/// <remarks>
		/// Objects of this type represent the amount of a substance in units of moles.
		/// </remarks>
		/// <param name="initialValue">Initial value in units specified.</param>
		/// <param name="unit">Metric prefix units of initial value.</param>
		public MoleType(double initialValue, MetricBaseUnit.Prefix unit)
			: base(initialValue, unit, "mol")
		{
		}
		/// <summary>
		/// Initializes a new instance of the MoleType class from another instance of a MoleType class with new units.
		/// </summary>
		/// <param name="moleType">Existing MoleType instance.</param>
		/// <param name="units">New units</param>
		public MoleType(MoleType moleType, MetricBaseUnit.Prefix units)
			: base(moleType.m_Value, moleType.m_Units, "mol")
		{
#if DEBUG // save orig values.
			double d = moleType.m_Value;
			MetricBaseUnit.Prefix u = moleType.m_Units;
#endif
			// Convert to new units.
			MetricBaseUnit mbu = Convert(moleType.m_Value, moleType.m_Units, units);
			m_Value = mbu.Value;
			m_Units = mbu.Units;

#if DEBUG // convert back and compare.
			mbu =  Convert(m_Value, m_Units, u);
			double d1 = mbu.Value;
			Debug.Assert(d == d1);
#endif

		}
		#endregion

		//#region Methods
		//#endregion
	}
}
