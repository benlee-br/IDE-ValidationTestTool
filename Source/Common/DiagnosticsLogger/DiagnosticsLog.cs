using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Reflection;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;

using BioRad.Common;
using BioRad.Common.ApplicationExceptions;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>
	/// The DiagnosticsLog class writes information about important software or hardware events to a log appender.	
	/// </summary>
	/// <remarks>
	/// Because the logging functions are general purpose, you must decide what 
	/// information is appropriate to log. Generally, you should log only information that could be useful 
	/// in diagnosing a hardware or software problem. 
	/// Diagnostics logging is not intended to be used as a tracing tool.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Ralph Ansell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">626</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\DiagLogger.wmf">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: DiagnosticsLog.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/DiagnosticsLog.cs $</item>
	///			<item name="vssrevision">$Revision: 77 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 1/16/08 2:19p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	[Serializable]
	public partial class DiagnosticsLog : MarshalByRefObject, IDiagnosticsEventLog, IDisposable
	{
		#region Constants
		/// <summary>
		/// Warning for Log entry timeout in ms
		/// </summary>
		private int c_TimeoutForLogEntry = 1500;
		/// <summary>
		/// Warning for Remote Event timeout in ms
		/// </summary>
		private int c_TimeoutForRemoteEvent = 1500;
		#endregion
		#region Member Data
		/// <summary>
		/// List of appender objects.  
		/// </summary>
		private ILogAppender m_LogAppender = null;
		/// <summary>
		/// Logging level.
		/// </summary>
		private DiagnosticLevel m_Level = DiagnosticLevel.ALL;
		/// <summary>
		/// Track whether Dispose has been called.
		/// </summary>
		private bool m_Disposed = false;
		/// <summary>
		/// Logger name.
		/// </summary>
		private string m_Name = null;
		/// <summary>
		/// the logging level for remote diagnostic items
		/// </summary>
		private DiagnosticLevel m_RemoteDiagnosticLevel = DiagnosticLevel.OFF;
		#endregion

		#region Accessors
		/// <summary>
		/// Get/Set logging level.
		/// </summary>
		public DiagnosticLevel LoggingLevel
		{
			get { return m_Level; }

			[MethodImplAttribute(MethodImplOptions.Synchronized)]
			set { m_Level = value; }
		}
		/// <summary>
		/// Get log name used to create a relationship between a source of events and a log. 
		/// </summary>
		public string LogName
		{
			get { return m_Name; }

			[MethodImplAttribute(MethodImplOptions.Synchronized)]
			set { m_Name = value; }
		}
		/// <summary>
		/// Get/Set appender for this logger.
		/// </summary>
		public ILogAppender Appender
		{
			get { return m_LogAppender; }
			set { m_LogAppender = value; }
		}
		#endregion

		#region Delegates and Events
		/// <summary>
		/// The DiagnosticsLogEvent event enables an application to receive notification when an event is written to 
		/// this log. 
		/// </summary>
		public event DiagnosticsLogWriteEventHandler DiagnosticsLogWriteEvent;
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Initializes a new instance of the DiagnosticsLog class.
		/// </summary>
		/// <param name="logName">Log name.</param>
		/// <param name="level">Logging level.</param>
		/// <param name="appender">The log appender for this logger object.</param>
		/// <remarks>
		/// Seperate log file can be created by setting the createLogFile to true. This can only be done
		/// thru the configuration file.
		/// </remarks>
		internal DiagnosticsLog(string logName,
								DiagnosticLevel level,
									  ILogAppender appender)
		{
			System.Diagnostics.Debug.Assert(appender != null);

			m_LogAppender = appender;//todo clone?
			m_Name = logName;
			m_Level = level;

			string strLevel = ConfigurationManager.AppSettings["LogLevelForEDOInHistoryLog"];
			// get the logging level for remote diagnostic items
			if ((strLevel == null) || (strLevel.Length == 0))
				m_RemoteDiagnosticLevel = DiagnosticLevel.OFF;
			else
				m_RemoteDiagnosticLevel = (DiagnosticLevel)Enum.Parse(typeof(DiagnosticLevel), strLevel);

		}
		/// <summary>
		/// Releases unmanaged resources and performs 
		/// other cleanup operations before the object is reclaimed by garbage collection.
		/// </summary>
		~DiagnosticsLog()
		{
			Dispose(false);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Releases all resources used by the object.
		/// </summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			Dispose(true);
		}
		/// <summary>
		/// Releases the unmanaged resources used by the object and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; 
		/// false to release only unmanaged resources. 
		/// </param>
		/// <remarks>
		/// This method is called by the public Dispose() method and the Finalize method. 
		/// Dispose() invokes the protected Dispose(Boolean) method with the disposing parameter set to true. 
		/// Finalize invokes Dispose with disposing set to false.
		/// When the disposing parameter is true, this method releases all resources held by any managed objects that 
		/// this Component references. This method invokes the Dispose() method of each referenced object.
		/// </remarks>
		protected virtual void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!m_Disposed)
			{
				m_Disposed = true;
			}
		}
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
		public void Close()
		{
			m_LogAppender.Close();
			Dispose();
		}
		/// <summary>
		/// todo
		/// </summary>
		/// <param name="item"></param>
		internal void FireLogEntry(DiagnosticsLogItem item)
		{
			if (DiagnosticsLogWriteEvent != null)
			{
				//Publish the event to all subscribers.
				DiagnosticsLogWriteEvent(this, item);
			}
		}
		/// <summary>
		/// Append new log entry.
		/// </summary>
		/// <param name="logItem">Log entry object.</param>
		[MethodImplAttribute(MethodImplOptions.Synchronized)]
		public void Append(DiagnosticsLogItem logItem)
		{
			m_LogAppender.Append(logItem);
		}
		/// <summary>
		/// Append DiagnosticsLogItem item to end of log file.
		/// </summary>
		/// <param name="item">Diagnostic item to add.</param>
		/// <param name="level">Logging level</param>
		/// <remarks>
		/// Logs only entries whose level is greater than or equal to the level of the logger.
		/// </remarks>
		private void Add(DiagnosticsLogItem item, DiagnosticLevel level)
		{
			if (level >= LoggingLevel)
			{
				PerfTimer timer = new PerfTimer();
				timer.Start();

                // Defect 8531 - Override DiagnosticSeverity.Serious with DiagnosticSeverity.Warning.
                if ( item.Severity == DiagnosticSeverity.Serious )
                    item.Severity = DiagnosticSeverity.Warning;

				Append(item);

				TimeType t = timer.Stop();
				if (t.GetAs(TimeType.Units.MilliSeconds) > c_TimeoutForLogEntry)
				{
					item.Message = String.Format(
						"DiagnosticLog.Add - took longer than {0} msec to append a log entry to the log file - it took " +
						t.GetAs(TimeType.Units.MilliSeconds).ToString() + "msec - " +
						item.TimeStamp + " " + item.Message, c_TimeoutForLogEntry);

					//set some info for this message
					item.Level = DiagnosticLevel.WARN;
					item.TimeStamp = ISO8601DateTime.ToString(DateTime.Now);
					Append(item);
				}

				timer.Start();

				FireLogEntry(item);

				t = timer.Stop();
				if (t.GetAs(TimeType.Units.MilliSeconds) > c_TimeoutForRemoteEvent)
				{
					item.Message = String.Format(
						"DiagnosticLog.Add - took longer than {0} msec to fire the remote event - it took " +
						t.GetAs(TimeType.Units.MilliSeconds).ToString() + "msec - " +
						item.TimeStamp + " " + item.Message, c_TimeoutForRemoteEvent);

					item.TimeStamp = ISO8601DateTime.ToString(DateTime.Now);
					Append(item);

				}

				timer = null;
			}
		}
		/// <summary>
		/// Append new diagnostics log entry.
		/// </summary>
		/// <param name="logName"></param>
		/// <param name="level">Log entry level.</param>
		/// <param name="ds">Diagnostic severity</param>
		/// <param name="dt">Diagnostics tag.</param>
		/// <param name="message">The message of this event (localized).</param>
		/// <param name="messageName">Name (part of name-value pair) for message.</param>
		/// <param name="assemblyNameFull">Full name of calling assembly.</param>
		/// <param name="data"></param>
		/// <param name="ex">The exception object for this event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		/// <remarks>
		/// Use this method when you need to specify all the parameters for the log entry.
		/// </remarks>
		public DiagnosticsLogItem AddLogEntry(string logName,
												DiagnosticLevel level,
												DiagnosticSeverity ds,
												DiagnosticTag dt,
												string message,
												string messageName,
												string assemblyNameFull,
												Int32 data,
												Exception ex)
		{
			DiagnosticsLogItem item = new DiagnosticsLogItem(logName,
																assemblyNameFull,
																ds,
																dt,
																message,
																messageName,
																ex,
																level, data);

			Add(item, level);
			return item;
		}
		/// <summary>
		/// Append new diagnostics log entry with a specified date time.
		/// </summary>
		/// <param name="dateTime">Date time to assign to log entry.</param>
		/// <param name="logName">Name of log.</param>
		/// <param name="level">Log entry level.</param>
		/// <param name="ds">Diagnostic severity</param>
		/// <param name="dt">Diagnostics tag.</param>
		/// <param name="message">The message of this event (localized).</param>
		/// <param name="messageName">Name (part of name-value pair) for message.</param>
		/// <param name="assemblyNameFull">Full name of calling assembly.</param>
		/// <param name="data">Anything</param>
		/// <param name="ex">The exception object for this event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		/// <remarks>
		/// Use this method when you need to specify all the parameters for the log entry.
		/// </remarks>
		private DiagnosticsLogItem AddLogEntry(DateTime dateTime,
															 string logName,
															 DiagnosticLevel level,
															 DiagnosticSeverity ds,
															 DiagnosticTag dt,
															 string message,
															 string messageName,
															 string assemblyNameFull,
															 Int32 data,
															 Exception ex)
		{
			DiagnosticsLogItem item = new DiagnosticsLogItem(logName,
																				 assemblyNameFull,
																				 ds,
																				 dt,
																				 message,
																				 messageName,
																				 ex,
																				 level, data);
			// override the default time stamp with time stamp specified.
			item.TimeStamp = ISO8601DateTime.ToString(dateTime);
			Add(item, level);
			return item;
		}
		/// <summary>
		/// Append new diagnostics log entry.
		/// </summary>
		/// <param name="item">Diagnostics log item</param>
		/// <returns></returns>
		internal DiagnosticsLogItem AddLogEntry(DiagnosticsLogItem item)
		{
			Add(item, item.Level);
			return item;
		}
		/// <summary>
		/// Append new diagnostics log entry.
		/// </summary>
		/// <param name="ex">The exception object for this event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		/// <remarks>
		/// The following fields are automatically set for the caller as follows:
		/// <list type="bullet">
		/// <item>Current time.</item>
		///	<item>Assembly full name is set to assembly full name of the calling method.</item>
		///	<item>Diagnostic severity set to type exception set to Serious</item>
		///	<item>Diagnostic tag set to unassigned.</item>
		///	<item>Message name is empty.</item>
		///	<item>Diagnostic Level for event set to SERIOUS.</item>
		///	</list>
		/// </remarks>
		public DiagnosticsLogItem Exception(Exception ex)
		{
            
			return AddLogEntry(LogName, DiagnosticLevel.SERIOUS,
								DiagnosticSeverity.Serious,
								DiagnosticTag.Unassigned,
                                ExceptionDump.ExceptionToString(ex),
								"",
								Assembly.GetCallingAssembly().FullName,
								0,
								ex);
		}
		/// <summary>
		/// Append new diagnostics log entry.
		/// </summary>
		/// <param name="message">The message for this event.</param>
		/// <param name="ex">The exception object for this event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		/// <remarks>
		/// The following fields are automatically set for the caller as follows:
		/// <list type="bullet">
		/// <item>Current time.</item>
		///	<item>Assembly full name is set to assembly full name of the calling method.</item>
		///	<item>Diagnostic severity set to type exception set to Serious</item>
		///	<item>Diagnostic tag set to unassigned.</item>
		///	<item>Message name is empty.</item>
		///	<item>Diagnostic Level for event set to SERIOUS.</item>
		///	</list>
		/// </remarks>
		public DiagnosticsLogItem Exception(string message, Exception ex)
		{
			return AddLogEntry(LogName, MapSeverityToLevel(DiagnosticSeverity.Serious),
								DiagnosticSeverity.Serious,
								DiagnosticTag.Unassigned,
								message,
								"",
								Assembly.GetCallingAssembly().FullName,
								0,
								ex);
		}
		/// <summary>
		/// Append new diagnostics log entry.
		/// </summary>
		/// <param name="ex">The Loggable exception object for this event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		/// <remarks>
		/// The following fields are automatically set for the caller as follows:
		/// <list type="bullet">
		/// <item>Current time.</item>
		///	<item>Diagnostic Level for event set to ALL.</item>
		///	</list>
		/// </remarks>
		public DiagnosticsLogItem LoggableException(LoggableApplicationException ex)
		{
			// TODO: Log fields need to be reworked. FaultCode may be dropped.
			// MessageResource needs to be saved. Assembly is now exception
			// Source property. These are temporary measures to allow logging to continue.
			return AddLogEntry(LogName, MapSeverityToLevel(ex.Severity),
								ex.Severity,
								ex.Tag,
								ex.Message,
								ex.Message,
								ex.Source,
								ex.FaultCode,
								ex);
		}
		/// <summary>
		/// Append new diagnostics log entry.
		/// </summary>
		/// <param name="message">The message for this event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		/// <remarks>
		/// The following fields are automatically set for the caller as follows:
		/// <list type="bullet">
		/// <item>Current time.</item>
		///	<item>Assembly full name is set to assembly full name of the calling method.</item>
		///	<item>Diagnostic severity set to type exception set to Info</item>
		///	<item>Diagnostic tag set to unassigned.</item>
		///	<item>Message name is empty.</item>
		///	<item>Diagnostic Level for event set to INFO.</item>
		///	</list>
		/// </remarks>
		public DiagnosticsLogItem Info(string message)
		{
			return AddLogEntry(LogName, DiagnosticLevel.INFO,
					DiagnosticSeverity.Info,
					DiagnosticTag.Unassigned,
					message,
					"",
					Assembly.GetCallingAssembly().FullName,
					0,
					null);
		}
		/// <summary>
		/// Append new diagnostics log entry with specific date time.
		/// </summary>
		/// <param name="message">The message for this event.</param>
		/// <param name="dateTime">Date and time of event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		public DiagnosticsLogItem Info(DateTime dateTime, string message)
		{
			return AddLogEntry(dateTime,
									  LogName,
									  DiagnosticLevel.INFO,
									  DiagnosticSeverity.Info,
									  DiagnosticTag.Unassigned,
									  message,
									  "",
									  Assembly.GetCallingAssembly().FullName,
									  0,
									  null);
		}
		/// <summary>
		/// Append new diagnostics log entry.
		/// </summary>
		/// <param name="message">The message for this event.</param>
		/// <param name="tag">The DiagnosticTag for the log message.</param>
		/// <param name="ex">The exception object for this event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		/// <remarks>
		/// The following fields are automatically set for the caller as follows:
		/// <list type="bullet">
		/// <item>Current time.</item>
		///	<item>Assembly full name is set to assembly full name of the calling method.</item>
		///	<item>Diagnostic severity set to type exception set to Info</item>
		///	<item>Diagnostic tag set to unassigned.</item>
		///	<item>Message name is empty.</item>
		///	<item>Diagnostic Level for event set to INFO.</item>
		///	</list>
		/// </remarks>
		public DiagnosticsLogItem Info(string message, DiagnosticTag tag, Exception ex)
		{
			return AddLogEntry(LogName, DiagnosticLevel.INFO,
				DiagnosticSeverity.Info,
				tag,
				message,
				"",
				Assembly.GetCallingAssembly().FullName,
				0,
				ex);
		}
		/// <summary>Returns a DiagnosticsLogItem with the specified string and
		/// DiagnosticsLevel of DEBUG.</summary>
		/// <param name="message">The message for the log entry</param>
		/// <returns>A DiagnosticsLogItem object</returns>
		public DiagnosticsLogItem GetDebugLogItem(string message)
		{
			return new DiagnosticsLogItem(LogName,
				Assembly.GetCallingAssembly().FullName,
				DiagnosticSeverity.Info,
				DiagnosticTag.Unassigned,
				message, string.Empty, null,
				DiagnosticLevel.DEBUG, new Int32());
		}

		/// <summary>Returns a DiagnosticsLogItem with the specified string and
		/// DiagnosticsLevel of DEBUG.</summary>
		/// <param name="message">The message for the log entry</param>
		/// <param name="exception">The exception object associated with the log entry</param>
		/// <returns>A DiagnosticsLogItem object</returns>
		public DiagnosticsLogItem GetDebugLogItem(string message,
			Exception exception)
		{
			return new DiagnosticsLogItem(LogName,
				Assembly.GetCallingAssembly().FullName,
				DiagnosticSeverity.Info,
				DiagnosticTag.Unassigned,
				message, string.Empty, exception,
				DiagnosticLevel.DEBUG, new Int32());
		}

		/// <summary>Returns a DiagnosticsLogItem with the specified string and
		/// DiagnosticsLevel of INFO.</summary>
		/// <param name="message">The message for the log entry</param>
		/// <returns>A DiagnosticsLogItem object</returns>
		public DiagnosticsLogItem GetInfoLogItem(string message)
		{
			return new DiagnosticsLogItem(LogName,
				Assembly.GetCallingAssembly().FullName,
				DiagnosticSeverity.Info,
				DiagnosticTag.Unassigned,
				message, string.Empty, null,
				DiagnosticLevel.INFO, new Int32());
		}

		/// <summary>Returns a DiagnosticsLogItem with the specified string and
		/// DiagnosticsLevel of INFO.</summary>
		/// <param name="message">The message for the log entry</param>
		/// <param name="exception">The exception object associated with the log entry</param>
		/// <returns>A DiagnosticsLogItem object</returns>
		public DiagnosticsLogItem GetInfoLogItem(string message, Exception exception)
		{
			return new DiagnosticsLogItem(LogName,
				Assembly.GetCallingAssembly().FullName,
				DiagnosticSeverity.Info,
				DiagnosticTag.Unassigned,
				message, string.Empty, exception,
				DiagnosticLevel.INFO, new Int32());
		}

		/// <summary>Returns a DiagnosticsLogItem with the specified string and
		/// DiagnosticsLevel of WARNING.</summary>
		/// <param name="message">The message for the log entry</param>
		/// <param name="exception">The exception object associated with the log entry</param>
		/// <returns>A DiagnosticsLogItem object</returns>
		public DiagnosticsLogItem GetWarningLogItem(string message,
			Exception exception)
		{
			return new DiagnosticsLogItem(LogName,
				Assembly.GetCallingAssembly().FullName,
				DiagnosticSeverity.Warning,
				DiagnosticTag.Unassigned,
				message, string.Empty, exception,
				DiagnosticLevel.WARN, new Int32());
		}

		/// <summary>Returns a DiagnosticsLogItem with the specified string and
		/// DiagnosticsLevel of WARNING.</summary>
		/// <param name="message">The message for the log entry</param>
		/// <returns>A DiagnosticsLogItem object</returns>
		public DiagnosticsLogItem GetWarningLogItem(string message)
		{
			return new DiagnosticsLogItem(LogName,
				Assembly.GetCallingAssembly().FullName,
				DiagnosticSeverity.Warning,
				DiagnosticTag.Unassigned,
				message, string.Empty, null,
				DiagnosticLevel.WARN, new Int32());
		}

		/// <summary>Returns a DiagnosticsLogItem with the specified string and
		/// DiagnosticsLevel of SERIOUS.</summary>
		/// <param name="message">The message for the log entry</param>
		/// <returns>A DiagnosticsLogItem object</returns>
		public DiagnosticsLogItem GetSeriousErrorLogItem(string message)
		{
			return new DiagnosticsLogItem(LogName,
				Assembly.GetCallingAssembly().FullName,
				DiagnosticSeverity.Serious,
				DiagnosticTag.Unassigned,
				message, string.Empty, null,
				DiagnosticLevel.SERIOUS, new Int32());
		}

		/// <summary>Returns a DiagnosticsLogItem with the specified string and
		/// DiagnosticsLevel of SERIOUS.</summary>
		/// <param name="message">The message for the log entry</param>
		/// <param name="exception">The exception object associated with the log entry</param>
		/// <returns>A DiagnosticsLogItem object</returns>
		public DiagnosticsLogItem GetSeriousErrorLogItem(string message,
			Exception exception)
		{
			return new DiagnosticsLogItem(LogName,
				Assembly.GetCallingAssembly().FullName,
				DiagnosticSeverity.Serious,
				DiagnosticTag.Unassigned,
				message, string.Empty, exception,
				DiagnosticLevel.SERIOUS, new Int32());
		}
		/// <summary>
		/// Append new diagnostics log entry.
		/// </summary>
		/// <param name="message">The message for this event.</param>
		/// <param name="ex">The exception object for this event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		/// <remarks>
		/// The following fields are automatically set for the caller as follows:
		/// <list type="bullet">
		/// <item>Current time.</item>
		///	<item>Assembly full name is set to assembly full name of the calling method.</item>
		///	<item>Diagnostic severity set to type exception set to Info</item>
		///	<item>Diagnostic tag set to unassigned.</item>
		///	<item>Message name is empty.</item>
		///	<item>Diagnostic Level for event set to INFO.</item>
		///	</list>
		/// </remarks>
		public DiagnosticsLogItem Info(string message, Exception ex)
		{
			return AddLogEntry(LogName, DiagnosticLevel.INFO,
								DiagnosticSeverity.Info,
								DiagnosticTag.Unassigned,
								message,
								"",
								Assembly.GetCallingAssembly().FullName,
								0,
								ex);
		}
		/// <summary>
		/// Append new diagnostics log entry.
		/// </summary>
		/// <param name="message">The message for this event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		/// <remarks>
		/// The following fields are automatically set for the caller as follows:
		/// <list type="bullet">
		/// <item>Current time.</item>
		///	<item>Assembly full name is set to assembly full name of the calling method.</item>
		///	<item>Diagnostic severity set to type exception set to Error</item>
		///	<item>Diagnostic tag set to unassigned.</item>
		///	<item>Message name is empty.</item>
		///	<item>Diagnostic Level for event set to Serious.</item>
		///	</list>
		/// </remarks>
		public DiagnosticsLogItem SeriousError(string message)
		{
			return AddLogEntry(LogName, DiagnosticLevel.SERIOUS,
								DiagnosticSeverity.Serious,
								DiagnosticTag.Unassigned,
								message,
								"",
								Assembly.GetCallingAssembly().FullName,
								0,
								null);
		}
		/// <summary>
		/// Append new diagnostics log entry with specific date time.
		/// </summary>
		/// <param name="message">The message for this event.</param>
		/// <param name="dateTime">Date and time of event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		public DiagnosticsLogItem SeriousError(DateTime dateTime, string message)
		{
			return AddLogEntry(dateTime,
									  LogName,
									  DiagnosticLevel.INFO,
									  DiagnosticSeverity.Info,
									  DiagnosticTag.Unassigned,
									  message,
									  "",
									  Assembly.GetCallingAssembly().FullName,
									  0,
									  null);
		}
		/// <summary>
		/// Append new diagnostics log entry.
		/// </summary>
		/// <param name="message">The message for this event.</param>
		/// <param name="tag">The DiagnosticTag for the log message.</param>
		/// <param name="ex">The exception object for this event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		/// <remarks>
		/// The following fields are automatically set for the caller as follows:
		/// <list type="bullet">
		/// <item>Current time.</item>
		///	<item>Assembly full name is set to assembly full name of the calling method.</item>
		///	<item>Diagnostic severity set to type exception set to Error</item>
		///	<item>Diagnostic tag set to unassigned.</item>
		///	<item>Message name is empty.</item>
		///	<item>Diagnostic Level for event set to Serious.</item>
		///	</list>
		/// </remarks>
		public DiagnosticsLogItem SeriousError(string message, DiagnosticTag tag,
			Exception ex)
		{
			return AddLogEntry(LogName, DiagnosticLevel.SERIOUS,
				DiagnosticSeverity.Serious,
				tag,
				message,
				"",
				Assembly.GetCallingAssembly().FullName,
				0,
				ex);
		}
		/// <summary>
		/// Append new diagnostics log entry.
		/// </summary>
		/// <param name="message">The message for this event.</param>
		/// <param name="ex">The exception object for this event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		/// <remarks>
		/// The following fields are automatically set for the caller as follows:
		/// <list type="bullet">
		/// <item>Current time.</item>
		///	<item>Assembly full name is set to assembly full name of the calling method.</item>
		///	<item>Diagnostic severity set to type exception set to Error</item>
		///	<item>Diagnostic tag set to unassigned.</item>
		///	<item>Message name is empty.</item>
		///	<item>Diagnostic Level for event set to Serious.</item>
		///	</list>
		/// </remarks>
		public DiagnosticsLogItem SeriousError(string message, Exception ex)
		{
			return AddLogEntry(LogName, DiagnosticLevel.SERIOUS,
								DiagnosticSeverity.Serious,
								DiagnosticTag.Unassigned,
								message,
								"",
								Assembly.GetCallingAssembly().FullName,
								0,
								ex);
		}
		/// <summary>
		/// Append new diagnostics log entry.
		/// </summary>
		/// <param name="message">The message for this event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		/// <remarks>
		/// The following fields are automatically set for the caller as follows:
		/// <list type="bullet">
		/// <item>Current time.</item>
		///	<item>Assembly full name is set to assembly full name of the calling method.</item>
		///	<item>Diagnostic severity set to type exception set to Unassigned</item>
		///	<item>Diagnostic tag set to unassigned.</item>
		///	<item>Message name is empty.</item>
		///	<item>Diagnostic Level for event set to DEBUG.</item>
		///	</list>
		/// </remarks>
		public DiagnosticsLogItem Debug(string message)
		{
			return AddLogEntry(LogName, DiagnosticLevel.DEBUG,
                                DiagnosticSeverity.Info,
								DiagnosticTag.Unassigned,
								message,
								"",
								Assembly.GetCallingAssembly().FullName,
								0,
								null);
		}
		/// <summary>
		/// Append new diagnostics log entry with specific date time.
		/// </summary>
		/// <param name="message">The message for this event.</param>
		/// <param name="dateTime">Date and time of event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		public DiagnosticsLogItem Debug(DateTime dateTime, string message)
		{
			return AddLogEntry(dateTime,
									  LogName,
									  DiagnosticLevel.INFO,
									  DiagnosticSeverity.Info,
									  DiagnosticTag.Unassigned,
									  message,
									  "",
									  Assembly.GetCallingAssembly().FullName,
									  0,
									  null);
		}
		/// <summary>
		/// Append new diagnostics log entry.
		/// </summary>
		/// <param name="message">The message for this event.</param>
		/// <param name="tag">The DiagnosticTag for the log message.</param>
		/// <param name="ex">The exception object for this event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		/// <remarks>
		/// The following fields are automatically set for the caller as follows:
		/// <list type="bullet">
		/// <item>Current time.</item>
		///	<item>Assembly full name is set to assembly full name of the calling method.</item>
		///	<item>Diagnostic severity set to type exception set to Unassigned</item>
		///	<item>Diagnostic tag set to unassigned.</item>
		///	<item>Message name is empty.</item>
		///	<item>Diagnostic Level for event set to DEBUG.</item>
		///	</list>
		/// </remarks>
		public DiagnosticsLogItem Debug(string message, DiagnosticTag tag, Exception ex)
		{
			return AddLogEntry(LogName, DiagnosticLevel.DEBUG,
                DiagnosticSeverity.Info,
				tag,
				message,
				"",
				Assembly.GetCallingAssembly().FullName,
				0,
				ex);
		}
		/// <summary>
		/// Append new diagnostics log entry.
		/// </summary>
		/// <param name="message">The message for this event.</param>
		/// <param name="ex">The exception object for this event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		/// <remarks>
		/// The following fields are automatically set for the caller as follows:
		/// <list type="bullet">
		/// <item>Current time.</item>
		///	<item>Assembly full name is set to assembly full name of the calling method.</item>
		///	<item>Diagnostic severity set to type exception set to Unassigned</item>
		///	<item>Diagnostic tag set to unassigned.</item>
		///	<item>Message name is empty.</item>
		///	<item>Diagnostic Level for event set to DEBUG.</item>
		///	</list>
		/// </remarks>
		public DiagnosticsLogItem Debug(string message, Exception ex)
		{
			return AddLogEntry(LogName, DiagnosticLevel.DEBUG,
                                DiagnosticSeverity.Info,
								DiagnosticTag.Unassigned,
								message,
								"",
								Assembly.GetCallingAssembly().FullName,
								0,
								ex);
		}
		/// <summary>
		/// Append new diagnostics log entry.
		/// </summary>
		/// <param name="message">The message for this event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		/// <remarks>
		/// The following fields are automatically set for the caller as follows:
		/// <list type="bullet">
		/// <item>Current time.</item>
		///	<item>Assembly full name is set to assembly full name of the calling method.</item>
		///	<item>Diagnostic severity set to type exception set to Warning</item>
		///	<item>Diagnostic tag set to unassigned.</item>
		///	<item>Message name is empty.</item>
		///	<item>Diagnostic Level set for event to WARN.</item>
		///	</list>
		/// </remarks>
		public DiagnosticsLogItem Warn(string message)
		{
			return AddLogEntry(LogName, DiagnosticLevel.WARN,
								DiagnosticSeverity.Warning,
								DiagnosticTag.Unassigned,
								message,
								"",
								Assembly.GetCallingAssembly().FullName,
								0,
								null);
		}
		/// <summary>
		/// Append new diagnostics log entry with specific date time.
		/// </summary>
		/// <param name="message">The message for this event.</param>
		/// <param name="dateTime">Date and time of event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		public DiagnosticsLogItem Warn(DateTime dateTime, string message)
		{
			return AddLogEntry(dateTime,
									  LogName,
									  DiagnosticLevel.INFO,
									  DiagnosticSeverity.Info,
									  DiagnosticTag.Unassigned,
									  message,
									  "",
									  Assembly.GetCallingAssembly().FullName,
									  0,
									  null);
		}
		/// <summary>
		/// Append new diagnostics log entry.
		/// </summary>
		/// <param name="message">The message for this event.</param>
		/// <param name="tag">The DiagnosticTag for the log message.</param>
		/// <param name="ex">The exception object for this event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		/// <remarks>
		/// The following fields are automatically set for the caller as follows:
		/// <list type="bullet">
		/// <item>Current time.</item>
		///	<item>Assembly full name is set to assembly full name of the calling method.</item>
		///	<item>Diagnostic severity set to type exception set to Warning</item>
		///	<item>Diagnostic tag set to unassigned.</item>
		///	<item>Message name is empty.</item>
		///	<item>Diagnostic Level set for event to WARN.</item>
		///	</list>
		/// </remarks>
		public DiagnosticsLogItem Warn(string message, DiagnosticTag tag, Exception ex)
		{
			return AddLogEntry(LogName, DiagnosticLevel.WARN,
				DiagnosticSeverity.Warning,
				tag,
				message,
				"",
				Assembly.GetCallingAssembly().FullName,
				0,
				ex);
		}
		/// <summary>
		/// Append new diagnostics log entry.
		/// </summary>
		/// <param name="message">The message for this event.</param>
		/// <param name="ex">The exception object for this event.</param>
		/// <returns>DiagnosticsLogItem object written to log file.</returns>
		/// <remarks>
		/// The following fields are automatically set for the caller as follows:
		/// <list type="bullet">
		/// <item>Current time.</item>
		///	<item>Assembly full name is set to assembly full name of the calling method.</item>
		///	<item>Diagnostic severity set to type exception set to Warning</item>
		///	<item>Diagnostic tag set to unassigned.</item>
		///	<item>Message name is empty.</item>
		///	<item>Diagnostic Level set for event to WARN.</item>
		///	</list>
		/// </remarks>
		public DiagnosticsLogItem Warn(string message, Exception ex)
		{
			return AddLogEntry(LogName, DiagnosticLevel.WARN,
								DiagnosticSeverity.Warning,
								DiagnosticTag.Unassigned,
								message,
								"",
								Assembly.GetCallingAssembly().FullName,
								0,
								ex);
		}

		/// <summary>Logs the message using the severity level.</summary>
		/// <param name="severity">The DiagnosticSeverity value.</param>
		/// <param name="message">The message to log.</param>
		/// <returns>A DiagnosticsLogItem object.</returns>
		public DiagnosticsLogItem Log(DiagnosticSeverity severity, string message)
		{
			return AddLogEntry(LogName, MapSeverityToLevel(severity), severity,
								DiagnosticTag.Unassigned,
								message,
								"",
								Assembly.GetCallingAssembly().FullName,
								0,
								null);
		}
		/// <summary>Write the PersistableHeader Information - the data is written out as a string
		/// in the message field. Use the PersistableHeader.Parse to convert the string 
		/// into a PersistableHeader object.</summary>
		/// <param name="persistableHeader"></param>
		/// <returns>A DiagnosticsLogItem</returns>
		public DiagnosticsLogItem WritePersistableHeader(string persistableHeader)
		{
			return AddLogEntry(WellKnownLogName.PersistableHeader,
				DiagnosticLevel.INFO,
				DiagnosticSeverity.Info,
				DiagnosticTag.Unassigned,
				persistableHeader,
				"",
				Assembly.GetCallingAssembly().FullName,
				0,
				null);
		}

		/// <summary>Returns a DiagnosticLevel appropriate for the DiagnosticSeverity.</summary>
		/// <param name="severity">The DiagnosticSeverity value.</param>
		/// <returns>DiagnosticLevel</returns>
		private DiagnosticLevel MapSeverityToLevel(DiagnosticSeverity severity)
		{
			switch (severity)
			{
				case DiagnosticSeverity.Info:
					return DiagnosticLevel.INFO;
				case DiagnosticSeverity.Warning:
					return DiagnosticLevel.WARN;
				case DiagnosticSeverity.Serious:
					return DiagnosticLevel.SERIOUS;
				case DiagnosticSeverity.Fatal:
					return DiagnosticLevel.FATAL;
				default:
					return DiagnosticLevel.DEBUG;
			}
		}
		#endregion

	}
}
