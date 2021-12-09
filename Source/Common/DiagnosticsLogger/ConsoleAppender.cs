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
	/// The XMLAppender class writes information about important software or hardware events to a log file.	
	/// </summary>
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
	///			<item name="vssfile">$Workfile: ConsoleAppender.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/ConsoleAppender.cs $</item>
	///			<item name="vssrevision">$Revision: 5 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 7/01/08 8:30a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class ConsoleAppender : ILogAppender
	{
		#region Constants
        #endregion

        #region Member Data
		/// <summary>
		/// 
		/// </summary>
		private IFormat m_Output = null;
        #endregion

        #region Accessors
		/// <summary>
		/// 
		/// </summary>
		public long Count
		{
			get
			{
				return 0;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public object Current
		{
			get
			{
				return null;
			}
		}
		/// <summary>
		/// Get/Set file name for this appender object.
		/// </summary>
		public string Filename
		{
			get{return string.Empty;}
		}
        #endregion

		#region Delegates and Events
		#endregion

        #region Constructors and Destructor
		/// <summary>
		/// 
		/// </summary>
		public ConsoleAppender(IFormat format)
		{
			m_Output = format;
		}
        #endregion

        #region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual object Clone()
		{
			return null;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator() 
		{
			return null;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool MoveNext()
		{
			return false;
		}
		/// <summary>
		/// 
		/// </summary>
		public void Reset()
		{
		}
		/// <summary>
		/// Get the number of log entries in the log file for specified log name.
		/// </summary>
		[MethodImplAttribute(MethodImplOptions.Synchronized)]
		public long CountLogEntries(string forLogName)
		{
			long count = 0;
			return count;
		}
		/// <summary>
		/// Indexer for direct access to DiagnosticsLogItem object.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">
		/// Throws invalid operation excetion.
		/// </exception>
		/// <remarks>
		/// Returns the DiagnosticsLogItem object for the argument index.
		/// </remarks>
		public DiagnosticsLogItem this[long index]
		{
			[MethodImplAttribute(MethodImplOptions.Synchronized)]
			get 
			{
				return null;
			}
		}
		/// <summary>
		/// Close appender and release resources.
		/// </summary>
		public virtual void Close()
		{
		}
        /// <summary>Open an existing appender and truncate so that its size is zero bytes.</summary>
        public virtual void Truncate()
        {
        }
		/// <summary>
		/// Append a log entry appender specific way.
		/// </summary>
		/// <param name="logItem"></param>
		[MethodImplAttribute(MethodImplOptions.Synchronized)]
        public virtual void Append(DiagnosticsLogItem logItem)
		{
			Console.WriteLine(m_Output.Format(logItem));
		}
        #endregion

		#region Event Handlers
		#endregion
	}
}
