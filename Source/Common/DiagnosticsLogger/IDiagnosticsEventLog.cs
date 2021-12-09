using System;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>
	/// Occurs when an entry is written to an diagnostic log on the local computer.
	/// </summary>
	/// <remarks>
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
	///			<item name="vssfile">$Workfile: IDiagnosticsEventLog.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/DiagnosticsLogger/IDiagnosticsEventLog.cs $</item>
	///			<item name="vssrevision">$Revision: 11 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 9/19/03 1:39p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public interface IDiagnosticsEventLog
	{
		#region Delegates and Events
		/// <summary>
		/// Occurs when an diagnostics log entry is written to an diagnostics log on the local computer.
		/// </summary>
		event DiagnosticsLogWriteEventHandler DiagnosticsLogWriteEvent;
		#endregion

		#region Methods
		#endregion
	}
	/// <summary>
	/// Delegate type defining the prototype of the callback method that subscribers must implement.
	/// </summary>
	/// <param name="sender">Object initiating the event.</param>
	/// <param name="args">Entry written object.</param>
	public delegate void DiagnosticsLogWriteEventHandler(object sender, DiagnosticsLogItem args);
}
