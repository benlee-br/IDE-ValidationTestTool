using System;

namespace BioRad.Common.Devices
{
	#region Documentation Tags
	/// <summary>
	/// Stores gain levels with sequence used for for exposure calculator logic.
	/// </summary>
	/// <remarks>
	/// Use Sequence to implement selection of values.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Pramod Walse</item>
	///			<item name="review">Last design/code review:1/14/04, Pramod Walse</item>
	///			<item name="conformancereview">Conformance review:2/17/04, Pramod Walse</item>
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
	///			<item name="vssfile">$Workfile: GainLevel.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Devices/GainLevel.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	[Serializable]
	public partial class GainLevel : ICloneable
	{
		#region Member Data
		/// <summary>
		/// Gain Value;
		/// </summary>
		private int m_Gain;
		#endregion

		#region Accessors
		/// <summary>
		/// Gain Value;
		/// </summary>
		public int Gain
		{
			set{m_Gain = value;}
			get{return m_Gain;}
		}

		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Default Empty Constructor.
		/// </summary>
		public GainLevel()
		{
		}
		#endregion

		#region Methods

		/// <summary>Create a clone of the GainLevel object.</summary>
		/// <returns>An object of the GainLevel type.</returns>
		public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion

	}
}
