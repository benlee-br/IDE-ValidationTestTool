using System;

namespace BioRad.Reports
{
	#region Documentation Tags
	/// <summary>Empty class used to represent no report data in the reports.</summary>
	/// <remarks></remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Shabnam</item>
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
	///			<item name="vssfile">$Workfile: NoReportData.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Reports/NoReportData.cs $</item>
	///			<item name="vssrevision">$Revision: 3 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class NoReportData
	{
		#region Methods
		/// <summary>
		/// Override returns empty string.
		/// </summary>
		/// <returns>Empty string.</returns>
		public override string ToString()
		{
			return String.Empty;
		}
		#endregion
	}
}
