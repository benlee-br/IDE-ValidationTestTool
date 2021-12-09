using System;
using System.Text;
using System.Diagnostics;

namespace BioRad.Common.PhysicalQuanities
{
	#region Documentation Tags
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// 
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:</item>
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
	///			<item name="vssfile">$Workfile: GramType.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/PhysicalQuanities/GramType.cs $</item>
	///			<item name="vssrevision">$Revision: 2 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 3/30/10 8:21a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	[Serializable]
	public class GramType : MetricBaseUnit
	{
		//#region Constants
		//#endregion

		//#region Menber Data
		//#endregion

		//#region Accessors
		//#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Initializes a new instance of the GramType class.
		/// </summary>
		/// <remarks>
		/// Objects of this type represent unit of mass.
		/// </remarks>
		/// <param name="initialValue">Initial value in units specified.</param>
		/// <param name="unit">Metric prefix units of initial value.</param>
		public GramType(double initialValue, MetricBaseUnit.Prefix unit)
			: base(initialValue, unit, "kg")
		{
		}
		/// <summary>
		/// Initializes a new instance of the GramType class from another instance of a GramType class with new units.
		/// </summary>
		/// <param name="gramType">Existing GramType instance.</param>
		/// <param name="units">New units</param>
		public GramType(GramType gramType, MetricBaseUnit.Prefix units)
			: base(gramType.m_Value, gramType.m_Units, "kg")
		{
#if DEBUG // save orig values.
			double d = gramType.m_Value;
			MetricBaseUnit.Prefix u = gramType.m_Units;
#endif
			// Convert to new units.
			MetricBaseUnit mbu = Convert(gramType.m_Value, gramType.m_Units, units);
			m_Value = mbu.Value;
			m_Units = mbu.Units;

#if DEBUG // convert back and compare.
			mbu = Convert(m_Value, m_Units, u);
			double d1 = mbu.Value;
			Debug.Assert(d == d1);
#endif

		}
		#endregion

		//#region Methods
		//#endregion
	}
}
