using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Reflection;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>
	/// Class Summary
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
	///			<item name="vssfile">$Workfile: TraceAppender.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/TraceAppender.cs $</item>
	///			<item name="vssrevision">$Revision: 3 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class TraceAppender : XMLAppender
	{
		#region Constants
        #endregion

        #region Member Data
		/// <summary>
		/// 
		/// </summary>
		private IFormat m_Output = null;
		/// <summary>
		/// 
		/// </summary>
		private TraceEx m_Trace = new TraceEx();
        #endregion

        #region Accessors
        #endregion

		#region Delegates and Events
		#endregion

        #region Constructors and Destructor
		/// <summary>
		/// 
		/// </summary>
		public TraceAppender(IFormat format, string filename) : base(filename)
		{
			m_Trace.SetVerbose();
			m_Output = format;
		}
        #endregion

        #region Methods
		/// <summary>
		/// Close appender and release resources.
		/// </summary>
		public override void Close()
		{
			System.Diagnostics.Trace.Close();
			base.Close();
		}
		/// <summary>
		/// Append a log entry appender specific way.
		/// </summary>
		/// <param name="logItem"></param>
		[MethodImplAttribute(MethodImplOptions.Synchronized)]
		public override void Append(DiagnosticsLogItem logItem)
		{
			m_Trace.SetVerbose();
			m_Trace.WriteVerbose(m_Output.Format(logItem));
			base.Append(logItem);
		}
        #endregion

		#region Event Handlers
		#endregion
	}
}
