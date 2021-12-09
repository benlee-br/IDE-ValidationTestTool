using System;

namespace BioRad.Common
{
	#region Documentation Tags
	///<summary>
	///This enumeration specifies the Marker shape of data points  of PCR Amplification 
	///Chart, include None, circle, square, triangle, diamond, cross and random 
	///</summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:</item>
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
	///			<item name="vssfile">$Workfile: ChartMarkerShape.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/ChartMarkerShape/ChartMarkerShape.cs $</item>
	///			<item name="vssrevision">$Revision: 1 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Blie $</item>
	///			<item name="vssdate">$Date: 8/27/04 3:32p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public enum ChartDataPointMarkerShape: int
	{
		/// <summary>None marker.</summary>
		None = 0,
		/// <summary>Circle marker.</summary>
		Circle,
		/// <summary>Square marker.</summary>
		Square,
		/// <summary>Triangle marker.</summary>
		Triangle,
		/// <summary>Diamond marker.</summary>
		Diamond,
		/// <summary>Cross marker.</summary>
		Cross,
		/// <summary>Random marker.</summary>
		Random,
		/// <summary>Reserved value.</summary>
		Reserved1,
		/// <summary>Reserved value.</summary>
		Reserved2
	}

}
