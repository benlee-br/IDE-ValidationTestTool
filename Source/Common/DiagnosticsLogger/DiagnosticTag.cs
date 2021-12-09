using System;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>
	/// The diagnostic tag specifies the unique type of a diagnostics log item.
	/// In many cases, each tag is associated with a specific emitting class.  These 
	/// values will be persistent.  Therefore, when creating new constants, assign a 
	/// unique number.
	/// Note: Diagnostics tags are *not* globalized.
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:</item>
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
	///			<item name="vssfile">$Workfile: DiagnosticTag.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/DiagnosticTag.cs $</item>
	///			<item name="vssrevision">$Revision: 7 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Pwalse $</item>
	///			<item name="vssdate">$Date: 9/13/06 10:16p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public enum DiagnosticTag : int
	{
		/// <summary>Unassigned variable semantics.</summary>
		Unassigned,
		/// <summary> Diagnostic log item for logged exceptions.</summary>
		EXCEPTION,
		/// <summary>Instrument services.</summary>
		IS,
		/// <summary>Plate setup header.</summary>
		PS_HEADER,
		/// <summary>Plate setup selected fluorophor.</summary>
		PS_SEL_FLUOR,
		/// <summary>Plate setup well info.</summary>
		PS_WELL_INFO,
		/// <summary>Plate setup well sample.</summary>
		PS_WELL_SAMPLE,
		/// <summary>Protocol stage.</summary>
		P_STAGE,
		/// <summary>Security tag.</summary>
		Security,
		/// <summary>User aborted the run tag.</summary>
		UserAbortRun,
		/// <summary>System information</summary>
		SystemInfo
	};
}
