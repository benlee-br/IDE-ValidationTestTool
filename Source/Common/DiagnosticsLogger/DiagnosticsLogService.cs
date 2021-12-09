using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.Remoting.Messaging;

using BioRad.Common.Services;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>
	/// The DiagnosticsLogService class provides a single point of access to an application
	/// for the management and access to diagnostics logger objects. 
	/// </summary>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Ralph Ansell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">627</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\DiagLogger.wmf">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: DiagnosticsLogService.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/DiagnosticsLogService.cs $</item>
	///			<item name="vssrevision">$Revision: 80 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 4/04/07 2:05p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public partial class DiagnosticsLogService : AbstractService, IDisposable, IDiagnosticsEventLog
    {
		#region Constants
		/// <summary>
		/// Log file extension.
		/// </summary>
		private const string c_LogFileExtension = @".xml";
		/// <summary>
		/// XML text reader log files.
		/// </summary>
		internal const Int32 c_VersionXmlTextReaderLogFiles = 0;
		/// <summary>
		/// Encrypted log files.
		/// </summary>
		internal const Int32 c_VersionEncyrptedLogFiles = 1;
		/// <summary>
		/// Current version of log files.
		/// </summary>
		internal const Int32 c_VersionCurrent = c_VersionEncyrptedLogFiles;
//		/// <summary>
//		/// Directory name for log files. 
//		/// The logs directory is sub directory of application directory of the process.
//		/// </summary>
//		private const string c_LogDirectory = @"Logs";
		#endregion

        #region Member Data
		//todo:how often is new log file created.
		/// <summary>
		/// Application default logger object.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Default logger object writes log entries to a file in well formed XML.
		/// </para>
		/// </remarks>
		private DiagnosticsLog m_AppLogger; // Fix for Bug 3331 // = DefaultLog;
		/// <summary>
		/// Static private instance, used to implement singleton pattern if service
		/// fails.
		/// </summary>
		private static DiagnosticsLogService s_DiagnosticsLogService = null;
		/// <summary>
		/// Private Object used purely for synchronization.
		/// </summary>
		private Object objLock = new Object();
		/// <summary>
		/// Track whether Dispose has been called.
		/// </summary>
		private bool m_Disposed = false;
        /// <summary>
        /// Service list of DiagnosticsLog objects.  
        /// </summary>
		private Hashtable m_ObjectServiceList;
		/// <summary>
		/// If true then the overriding level is applied to all loggers.
		/// </summary>
		private bool m_ApplyOverridingLevel = false;
		/// <summary>
		/// The overriding level.
		/// </summary>
		private DiagnosticLevel m_OverridingLevel = DiagnosticLevel.ALL;
        #endregion

        #region Accessors
		/// <summary>
		/// Static accessor for getting reference to DiagnosticsLogService object.
		/// </summary>
		/// <remarks>
		/// If the service provider fails to give use a diagnostic log service object then
		/// instantiate our own. We must always return a valid diagnostic log service object.
		/// </remarks>
		/// <returns>
		/// Returns an object that represents the DiagnosticsLogService service for the application.
		/// </returns>		
		public static DiagnosticsLogService GetService
		{
			get
			{
				if ( s_DiagnosticsLogService == null )// already have one?
				{
					try
					{
						// try and get from service provider.
						ServiceProvider serviceProvider = ServiceProvider.GetInstance(); 
						s_DiagnosticsLogService 
							= (DiagnosticsLogService)serviceProvider.GetService(typeof(DiagnosticsLogService));
					}
					catch(Exception ex)
					{
						s_DiagnosticsLogService = null;
						Console.WriteLine(ex.Message);
						// service provider failed. not required to have a configuration file.
					}
					finally
					{
						// if service provider failed, then instantiate the diagnostic log service as singleton.
						if ( s_DiagnosticsLogService == null )
							s_DiagnosticsLogService = GetInstance();
					}
				}
				return s_DiagnosticsLogService;
			}
		}
		/// <summary>Creates the default log object and returns it.</summary>
		/// <remarks>The object created sets the overriding level if that 
		/// has been specified</remarks>
		private DiagnosticsLog DefaultLog
		{
			// Fix for Bug 3331 - this cannot be static because it 
			// needs to access m_ApplyOverridingLevel and m_OverridingLevel
			get 
			{
				// if the default logger does not exist - create it
				if (this.m_AppLogger == null)
				{
					DiagnosticLevel level = DiagnosticLevel.ALL;

					// if the level needs to be overriden
					if(this.m_ApplyOverridingLevel)
						level = this.m_OverridingLevel;

					this.m_AppLogger = new DiagnosticsLog(
						ApplicationPath.ApplicationName, level, new EncryptedAppender(MakePath(null))
						);
				}
				return this.m_AppLogger;
			}
		}
		/// <summary>
		/// Get number of diagnostics loggers.
		/// </summary>
		public int Count
		{
			get{return m_ObjectServiceList.Count;}
		}
        #endregion

		#region Delegates and Events
		/// <summary>
		/// The DiagnosticsLogEvent event enables an application to receive notification when an event is written to 
		/// any log. 
		/// </summary>
		public event DiagnosticsLogWriteEventHandler DiagnosticsLogWriteEvent;
		#endregion

        #region Constructors and Destructor
        /// <summary>
		/// Initializes a new instance of the DiagnosticsLogService class.
		/// </summary>
		/// <remarks>
		/// Must be public for service provider.
		/// </remarks>
		public DiagnosticsLogService()
		{
			m_ObjectServiceList = new Hashtable();
		}
		/// <summary>
		/// Ensures that resources are freed and other cleanup operations are 
		/// performed when the garbage collector.
		/// </summary>
		~DiagnosticsLogService()
		{
			Dispose (false);
		}
        #endregion

        #region Methods
		/// <summary>
		/// Convert older log files to latest version of log files.
		/// </summary>
		/// <param name="filename"></param>
		public static void MaybeConvertLogFile(string filename)
		{
			if ( filename == null )
				throw new ArgumentNullException();

			// Ralph 03/03/2006, Not related to defect 4078 but was found while fixing defect
			// If this method is called with an absolute or relative path then we do not want 
			// to change the path.
			string fileNameExt = string.Empty;
			string fullpath = filename;

			// Check if the filename has an extension.
			if ( Path.GetExtension(filename).Length == 0 )
			{
				fileNameExt = string.Concat(filename, c_LogFileExtension);
				fullpath = Path.Combine(ApplicationPath.CommonApplicationDataPath(), 
					Path.Combine("Logs", fileNameExt));
			}

			if ( File.Exists(fullpath) )
			{
				//Check for older version log files. if older version then start a new log file
				//and backup previous log file.
				int version = c_VersionCurrent;

				// Get version of log file.
				LogEnumerator logEnumerator = new LogEnumerator(fullpath);
				version = logEnumerator.Version; 
				logEnumerator.Dispose();

				if ( version < c_VersionCurrent )
				{
					LogFiles.ManageHistoryLogFileSize(filename, 0);
				}
			}
		}
		/// <summary>
		/// Writes an entry with the given message text and type
		/// to the application event log.
		/// </summary>
		/// <param name="message">The string to write to the event log.</param>
		/// <param name="type">One of the EventLogEntryType values.</param>
		public static void WriteEntryApplicationEventLog(string message, EventLogEntryType type)
		{
			// add login failures to application event log
			// Create the source, if it does not already exist.
			StringBuilder sb = 
				new StringBuilder(
                ApplicationStateData.GetInstance.ProductName);
			sb.Append(" ");
			sb.Append(
				Process.GetCurrentProcess().MainModule.FileVersionInfo.FileVersion);
			string source = sb.ToString();
			string logname = "Application";
			if ( !EventLog.SourceExists(source) )
				EventLog.CreateEventSource(source, logname);

			EventLog ev = 
				new EventLog(
				logname, 
				System.Environment.MachineName,
				source);
			ev.WriteEntry(message, type);
			ev.Close();
		}
		/// <summary>
		/// Singleton instance provider for diagnostic logger service.
		/// </summary>
		/// <returns>Class singleton instance</returns>
		private static DiagnosticsLogService GetInstance()
		{
			if (s_DiagnosticsLogService == null)
				s_DiagnosticsLogService = new DiagnosticsLogService();
			return s_DiagnosticsLogService;
		}
		/// <summary>
		/// Releases the resources used by the Diagnostics Log Service.
		/// </summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			Dispose(true);
		}
		/// <summary>
		/// Releases the resources used by the diagnostics logs and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing"></param>
		private void Dispose(bool disposing)
		{
			lock(objLock)//Synchronize access.
			{
				// Check to see if Dispose has already been called.
				if( !m_Disposed )
				{
					m_Disposed = true;   

					if ( m_ObjectServiceList != null )
					{
						DiagnosticsLog log;
						foreach (DictionaryEntry entry in m_ObjectServiceList)
						{
							log = (DiagnosticsLog)entry.Value;
							if ( log == null )
								continue;

							log.Close();
						}
						m_ObjectServiceList.Clear();
						m_ObjectServiceList = null;
					}
				}
			}
		}
		/// <summary>
		/// Flush any remaining diagnostic loggers entries.
		/// </summary>
		public void Close()
		{
			Dispose();
		}
		/// <summary>
		/// Makes full file path from argument file name in application directory. 
		/// </summary>
		/// <remarks>If the logName is empty, returns the path to the default history log
		/// file, else returns the path to the Logs folder\logName.</remarks>
		/// <param name="logName">Log name</param>
		/// <returns>full file path. Returns an empty string if the path cannot be determined.</returns>
		public static string MakePath(string logName)
		{
			// Fix for Bug 3757 - using Path.Combine instead of a stringBuilder

			//create default file name
			string str = string.Empty;

			if ( ConfigurationManager.AppSettings["LogFilesPath"] != null &&
                ConfigurationManager.AppSettings["ApplicationEventLogFileName"] != null)
			{
				str = Path.Combine(ApplicationPath.CommonApplicationDataPath(),
                    ConfigurationManager.AppSettings["LogFilesPath"]);
				if ( !Directory.Exists(str) )
					Directory.CreateDirectory(str);

				if ( logName == null || logName.Length == 0 )//use default log?
				{
                    str = Path.Combine(str, ConfigurationManager.AppSettings
						["ApplicationEventLogFileName"]);
				}
				else
				{
					str = Path.Combine(str, logName);
				}
			}
			else
			{
				// use the application path and create a default log file under the 
				// directory path for the application
				str = Path.Combine(ApplicationPath.CommonApplicationDataPath(), "DefaultLog");
			}

			// concatenate the .xml extension
			str = string.Concat(str, c_LogFileExtension);
			return str;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="level"></param>
		private void SetAllLoggersLevel(DiagnosticLevel level)
		{
			DiagnosticsLog log;
			foreach (DictionaryEntry entry in m_ObjectServiceList)
			{
				log = (DiagnosticsLog)entry.Value;
				if ( log == null )
					continue;

				log.LoggingLevel = level;
			}
		}
		/// <summary>
		/// Make key 
		/// </summary>
		/// <param name="logname"></param>
		/// <param name="appenderName"></param>
		/// <returns></returns>
		private string MakeKey(string logname, string appenderName)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(logname);
			sb.Append("-");
			sb.Append(appenderName);
			return sb.ToString();
		}
		/// <summary>
		/// Loads configuration elements for the service into whatever mechanisms
		/// the service chooses to use for caching those elements.
		/// </summary>
		/// <exception cref="ApplicationException">
		/// Throws Non-ConfiguredLog type in configuration file, duplicate log name or existing file is not a log file.
		/// </exception>
		/// <param name="configurationElements"></param>
		public override void Load(ICollection configurationElements)
		{
			Debug.Assert(m_ObjectServiceList != null);

			m_ObjectServiceList.Clear();
			string fullpath = "";

			// Add configured logs to service list.
			bool b1stItem = true;
			foreach (object item in configurationElements)
			{
				if ( item is ConfiguredLog)
				{
					ConfiguredLog cl = (ConfiguredLog)item;

					if ( b1stItem )//1st item contains data that applies to all loggers.
					{
						b1stItem = false;
						m_ApplyOverridingLevel = false;
						if ( string.Compare(cl.Override, "yes", true) == 0 )
						{
							m_ApplyOverridingLevel = true;
						}
						m_OverridingLevel = cl.LevelAllLogs;
					}
					else
					{
						// check for duplicate log.
						string key = MakeKey(cl.Name, m_AppLogger.Appender.Filename);
						DiagnosticsLog dl = (DiagnosticsLog)m_ObjectServiceList[key];
						if ( dl != null )
						{
							//todo: localize
							StringBuilder sb = new StringBuilder();
							sb.Append("Duplicate log name, ");
							sb.Append(cl.Name);
							sb.Append(", found in logger configuration file.");
							ApplicationException ex = new ApplicationException(sb.ToString());
							if ( m_AppLogger != null )
							{
								m_AppLogger.Exception(ex);//write exception to default log file.
							}
							throw ex;
						}

						ILogAppender appender = null;
						fullpath = cl.File;
						if ( fullpath.Length == 0 )
						{	// Use default appender.
							appender = (ILogAppender)m_AppLogger.Appender.Clone();
						}
						else
						{
							fullpath = MakePath(cl.File);
							appender = new EncryptedAppender(fullpath);

							// If existing file then make sure it is a Bio-Rad log file.
							if ( !LogEnumerator.IsBioRadLogFile(fullpath) )
							{
								//todo: localize
								StringBuilder sb = new StringBuilder();
								sb.Append("File: ");
								sb.Append(fullpath);
								sb.Append(", is not a Bio-Rad log file.");
								ApplicationException ex = new ApplicationException(sb.ToString());
								if ( m_AppLogger != null )
									m_AppLogger.Exception(ex);
								throw ex;
							}
						}

						// Create the logger and add to service list.
						dl = new DiagnosticsLog(cl.Name, cl.Level, appender);
						m_ObjectServiceList.Add(
							MakeKey(dl.LogName, dl.Appender.Filename), dl);
					}
				}
				else
				{
					//todo: localize
					ApplicationException ex = new ApplicationException("Non-ConfiguredLog item in configuration file "
						+ "provided to DiagnosticsLoggerService. This service requires that "
						+ "configuration elements be of type ConfiguredLog.");
					if ( m_AppLogger != null )
					{
						m_AppLogger.Exception(ex);//write exception to default log file.
					}
					throw ex;
				}
			}

			// Service subscribes to DiagnosticsLogEvents from all the loggers.
			DiagnosticsLog log;
			foreach (DictionaryEntry entry in m_ObjectServiceList)
			{
				log = (DiagnosticsLog)entry.Value;
				if ( log == null )
					continue;

				log.DiagnosticsLogWriteEvent += 
					new DiagnosticsLogWriteEventHandler(OnDiagnosticsLogEvent); 
			}

			// Add default application logger to service list only after subscribing to events
			// from all other loggers. We do not want to subscribe to events from the default logger.
			if ( m_AppLogger != null )
				m_ObjectServiceList.Add(
					MakeKey(m_AppLogger.LogName, m_AppLogger.Appender.Filename), 
					m_AppLogger);
			else
			{
				// Fix for Bug 3331
				m_AppLogger = DefaultLog;
				//todo : localize
				//throw new ApplicationException("No default application logger.");
			}
		}
		/// <summary>
		/// Create new logger and add to list of loggers.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="appender"></param>
		/// <returns></returns>
		private DiagnosticsLog CreateDiagnosticsLog(string name, ILogAppender appender)
		{
			if ( name == null || name.Length == 0 || appender == null )
				throw new ArgumentNullException();

			string key = MakeKey(name, appender.Filename);

			DiagnosticsLog dl = new DiagnosticsLog(name, DiagnosticLevel.ALL, appender);

			m_ObjectServiceList[key] = dl;

			// Service subscribes to DiagnosticsLogEvents from all loggers.
			dl.DiagnosticsLogWriteEvent += 
				new DiagnosticsLogWriteEventHandler(OnDiagnosticsLogEvent); 

			if ( m_ApplyOverridingLevel )
				dl.LoggingLevel = m_OverridingLevel;

			return dl;
		}
		/// <summary>
		/// Get diagnostic logger with specified appender. 
		/// If logger does not exists then one is created.
		/// </summary>
		/// <param name="name">
		/// Non case sensitive log name used to create a relationship between a source of events and the log. 
		/// </param>
		/// <param name="appender">Reference to appender.</param>
		/// <returns>
		/// Reference to diagnostic logger object with specified appender.
		/// </returns>		
		public DiagnosticsLog GetDiagnosticsLog(string name, ILogAppender appender)
		{
			if ( name == null || name.Length == 0 || appender == null )
				throw new ArgumentNullException();

			// Check if logger already exists.
			string key = MakeKey(name, appender.Filename);
			DiagnosticsLog dl = (DiagnosticsLog)m_ObjectServiceList[key];

			// Create the logger and add to service list.
			if ( dl == null )
				dl = CreateDiagnosticsLog(name, appender);

			return dl;
		}
		/// <summary>
		/// Get reference to existing logger object that matches argument name.
		/// If no exiting logger with argument name exists then a new logger is created
		/// with argument name and default appender.
		/// To create a logger that does not use the default appender 
		/// call method CreateDiagnosticsLog(string name, ILogAppender appender) first
		/// to create your logger then call this method to get a reference to your logger.
		/// </summary>
		/// <param name="name">
		/// Non case sensitive log name used to create a relationship between a source of events and the log. 
		/// </param>
		/// <returns>
		/// Reference to a DiagnosticsLog logger object that uses default appender.
		/// </returns>
		public DiagnosticsLog GetDiagnosticsLog(string name)
		{
			// Fix for Bug 3331
			if(m_AppLogger == null)
				m_AppLogger = DefaultLog;

			Debug.Assert(m_ObjectServiceList != null);

			DiagnosticsLog dl = null;

			// Null or empty string returns default application logger object.
			string logName = name;
			if ( logName == null || logName.Length == 0 || 
				string.Compare(logName, m_AppLogger.LogName, true) == 0 )
			{
				dl = m_AppLogger;//default application logger.
			}
			else
			{
				// Check for existing logger object.
				string key = MakeKey(logName, m_AppLogger.Appender.Filename);
				dl = (DiagnosticsLog)m_ObjectServiceList[key];

				// Log name not found in service list. Create new logger object and add to service list.
				// new log entries will be written to default log file.
				if ( dl == null )
				{
					dl = CreateDiagnosticsLog(name, 
						(ILogAppender)m_AppLogger.Appender.Clone());
				}
			}
			Debug.Assert(dl != null);//should never happen.

			if ( m_ApplyOverridingLevel )
				dl.LoggingLevel = m_OverridingLevel;
	
			return dl;
		}
        #endregion

        #region Event Handlers
		/// <summary>
		/// Diagnostics log event handler. Catches log entry events from all other loggers and
		/// write log entry to default logger. 
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Diagnostics log event argument.</param>
		void OnDiagnosticsLogEvent(object sender, DiagnosticsLogItem e)
		{
			Debug.Assert(e != null);
			Debug.Assert( m_AppLogger != null);
			Debug.Assert( string.Compare(m_AppLogger.LogName,e.LogName,true) != 0 );
			m_AppLogger.FireLogEntry(e);
			if (DiagnosticsLogWriteEvent != null) 
			{
				DiagnosticsLogWriteEvent(this, e);
			}
		}
        #endregion
    }
}
