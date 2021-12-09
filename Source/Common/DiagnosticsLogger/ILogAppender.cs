using System;
using System.Collections;
using System.Collections.Specialized;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>
	/// Implement this interface to create your own strategy for log entries.
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Ralph Ansell</item>
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
	///			<item name="vssfile">$Workfile: ILogAppender.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/ILogAppender.cs $</item>
	///			<item name="vssrevision">$Revision: 7 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 7/01/08 8:30a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public interface ILogAppender :  ICloneable
	{
		#region Accessors
		/// <summary>The Diagnostics file name</summary>
		string Filename { get;}
		#endregion

		#region Methods
        /// <summary>Open an existing appender and truncate so that its size is zero bytes.</summary>
        void Truncate();
		/// <summary>
		/// Closes the appender and releases resources.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Releases any resources allocated within the appender such as file handles, 
		/// network connections, etc.
		/// </para>
		/// <para>
		/// It is a programming error to append to a closed appender.
		/// </para>
		/// </remarks>
		void Close();
		/// <summary>
		/// Appends the log entry in Appender specific way.
		/// </summary>
		/// <param name="logItem">The log entry.</param>
		/// <remarks>
		/// <para>
		/// This method is called to log a message into this appender.
		/// </para>
		/// </remarks>
		void Append(DiagnosticsLogItem logItem);
        #endregion
	}
}
