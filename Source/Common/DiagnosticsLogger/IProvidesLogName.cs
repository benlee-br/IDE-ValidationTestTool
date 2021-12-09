using System;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>
	/// Classes which implement this interface provide a log name which identifies
	/// a log for exception and information diagnostic logging.
	/// </summary>
	/// <remarks>
	/// All PCR data classes which are linked to a specific optical data file log
	/// must implement this interface.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Lisa von Schlegell</item>
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
	///			<item name="vssfile">$Workfile: IProvidesLogName.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/DiagnosticsLogger/IProvidesLogName.cs $</item>
	///			<item name="vssrevision">$Revision: 1 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Lvs $</item>
	///			<item name="vssdate">$Date: 10/01/03 5:10p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public interface IProvidesLogName
	{
        #region Accessors
		/// <summary>
		/// Name of log. Log could be an XML file, an optical data file, or other
		/// persisted log.
		/// </summary>
		string LogName { get; }
        #endregion
	}
}
