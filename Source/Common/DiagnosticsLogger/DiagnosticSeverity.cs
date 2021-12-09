using System;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>
	/// The diagnostic severity specifies the general severity of a diagnostics
	/// item.  The string equivalents of this enum are globalized.
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
	///			<item name="vssfile">$Workfile: DiagnosticSeverity.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/DiagnosticsLogger/DiagnosticSeverity.cs $</item>
	///			<item name="vssrevision">$Revision: 6 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ssarup $</item>
	///			<item name="vssdate">$Date: 12/19/05 6:28p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public enum DiagnosticSeverity : int
	{
		/// <summary>
		/// Unassigned variable semantics.
		/// </summary>
		Unassigned, 
		/// <summary>
		/// Informational diagonostic items indicates infrequent but significant
		/// successful operations, or exceptions that do not imply an error situation.
		/// </summary>
		Info, 
		/// <summary>
		/// Warning diagnostic items indicate non-critical errors which
		/// are unlikely to cause loss of work or to compromise results.
		/// </summary>
		Warning, 
		/// <summary>
		/// Serious diagnostic items indicate serious errors which could
		/// affect work in progress or compromise results.
		/// </summary>
		Serious,
		/// <summary>
		/// Fatal diagnostic items indicate unrecoverable errors.
		/// </summary>
		Fatal
	};
}
