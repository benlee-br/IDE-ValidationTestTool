using System;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>
	/// Defines the set of levels recognised by the system.
	/// </summary>
	/// <remarks>
	/// Predefined set of levels.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Ralph Ansell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">Replace this text with ID</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\DiagLogger.wmf">Class Diagram</see>
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: DiagnosticLevel.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/DiagnosticsLogger/DiagnosticLevel.cs $</item>
	///			<item name="vssrevision">$Revision: 6 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 9/17/03 3:19p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	[Serializable]
	public enum DiagnosticLevel : int
	{ 
		/// <summary>
		/// The <c>ALL</c> level designates the lowest level possible.
		/// </summary>
		ALL=0,
		/// <summary>
		/// The <c>DEBUG</c> level designates fine-grained informational events that are most 
		/// useful to debug an application.
		/// </summary>
		DEBUG,
		/// <summary>
		/// The <c>INFO</c> level designates informational messages that highlight 
		/// the progress of the application at coarse-grained level.
		/// </summary>
		INFO,
		/// <summary>
		/// The <c>WARN</c> level designates potentially harmful situations.
		/// </summary>
		WARN,
		/// <summary>
		/// The <c>ERROR</c> level designates error events that might still
		///  allow the application to continue running.
		/// </summary>
		SERIOUS,
		/// <summary>
		/// The <c>Exception</c> level designates very severe events.
		/// </summary>
		FATAL,
		/// <summary>
		/// The <c>OFF</c> level designates a higher level than all the rest.
		/// </summary>
		OFF
	}
}
