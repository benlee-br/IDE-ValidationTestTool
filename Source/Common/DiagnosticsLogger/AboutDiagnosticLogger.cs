using System;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>
	/// Diagnostic Log (DLS) Service is a tool to help in the analysis of problems with an application when they occur. 
	/// The DLS provides an application with the capability to record 
	/// information about important software and hardware events.
	/// DLS provides the developer the capability to log statements to a variety of output targets.
	/// It is possible to tailor the amount of logging statements without modifying the application binary. 
	/// The DLS is designed so that log statements can remain in shipped code without incurring a high performance 
	/// cost. At the same time, log output can be so voluminous that it quickly becomes overwhelming. 
	/// One of the distinctive features of the DLS is the support for multiple loggers. 
	/// Using these loggers it is possible to selectively control which log statements are output at arbitrary 
	/// granularity. 
	/// 
	/// <para>
	/// Features
	///		<list type="bullet">
	///			<item>Multiple logs.</item>
	///			<item>
	///			Event notifications when data is written to a log.
	///			</item>
	///			<item>
	///			String, non case sensitive name, used to create a relationship between a source of events 
	///			and a log.
	///			</item>
	///			<item>Log automatically created when first referenced.</item>
	///			<item>Multiple components can use same log.</item>
	///			<item>Enumerator for iterating log names.</item>
	///			<item>Default log for application.</item>
	///			<item>XML log configuration file.</item>	
	///			<item>Level specific log methods.</item>
	///			<item>Thread safe.</item>
	///		</list>
	///	</para>
	/// </summary>
	/// 
	/// <remarks>
	/// The services of the diagnostic logger package are implemented by the following three classes: <see href="Reference\DiagLogger.wmf">Class Diagram</see>,
	///	<list type="bullet">
	///	<item>DiagnosticsLogService class</item>	
	///	<item>DiagnosticsLog class</item>
	///	<item>DiagnosticsLogItem class</item>
	/// </list>
	/// Because logging functions are general purpose, you must decide what information is appropriate to log. 
	/// Generally, you should log only information that could be useful in diagnosing a hardware or software problem. 
	/// <para>
	/// Deciding what to log:
	///	<list type="bullet">
	///	<item>
	///	Resource problems. Low-memory situation (caused by a code bug or inadequate memory) that 
	///	degrades performance, logging a warning event when memory allocation fails might provide a clue about what
	///	went wrong. 
	/// </item>
	///	<item>
	///	Hardware problems. If a device driver encounters a time-out, communications failure, 
	///	or a data error from a serial interface, logging information about these events can 
	///	help the system administrator diagnose hardware problems. 
	///	</item>
	///	<item>
	///	Information events.
	///	</item>
	///	User logging on/off, opening a database, starting a file transfer, cannot access file, etc. 
	///	</list>
	///	</para>
	///	
	/// <para>
	/// Writing messages:
	///	<list type="bullet">
	///	<item>
	///	Remember that your audience is administrators and users who are trying to troubleshoot a problem. 
	///	A message should contain all the information needed to interpret what caused the problem and what to do 
	///	to correct the problem.
	/// </item>
	///	<item>
	///	Do not use tabs or commas in the message text.
	///	Many organizations import these files into databases and the extra formatting characters require manual 
	///	manipulation.
	///	</item>
	///	<item>
	///	Diagnostic logging consumes resources such as disk space and processor time. 
	///	The amount of disk space that an event log requires and the overhead for an application that 
	///	logs events depend on how much information you choose to log. 
	///	This is why it is important to log only essential information. 
	///	It is also good to place diagnostic logging calls in an error path in the code rather than in the main code path, 
	///	which would reduce performance.
	///	</item>
	///	</list>
	///	</para>	
	///	
	///	<para>
	///	Log file format:
	///	<list type="bullet">
	///	<item>
	///	Many applications record errors and events in various proprietary error logs. 
	///	These proprietary error logs have different formats and display different user interfaces. 
	///	Moreover, you cannot merge the data to provide a complete report. 
	///	Therefore, you need to check a variety of sources to diagnose problems.
	///	Diagnostic log service provides a standard, centralized way for a application to 
	///	record important software and hardware events. The logging service stores events from various 
	///	sources in a single and/or multiple log files.
	///	</item>
	///	<item>
	///	Diagnostic log files are written in Extensible Markup Language (XML).
	///	XML provides a way to describe structured data. 
	///	XML is simple, platform-independent, and a widely adopted standard. 
	///	</item>
	///	<item>
	///	The separation of content from presentation is a core design principle for XML. 
	///	XML completely separates the notion of the markup from its intended visual presentation, 
	///	allowing different data <c>"views"</c> to be easly supported.
	///	</item>
	///	</list>
	///	</para>
	///	
	///	<para>
	/// Once an application has been deployed it may not be possible to utilise development and debugging tools. 
	/// An administrator can use effective logging systems to diagnose and fix many configuration issues. 
	/// Experience indicates that logging is an important component of the development cycle. 
	/// It offers several advantages. It provides precise context about the execution of the application. 
	/// Once inserted into the code, the generation of logging output requires no human intervention. 
	/// Moreover, log output can be saved in persistent medium to be studied at a later time. 
	/// In addition to its use in the development cycle, a sufficiently rich logging package can also be 
	/// viewed as an auditing tool. 
	/// </para>
	/// <para>
	/// References to a log is obtained using the <c>GetDiagnosticsLog</c> method in class DiagnosticsLogService. 
	/// The GetDiagnosticsLog method takes the name of the log as a parameter. 
	/// Calling the GetDiagnosticsLog method with the same name will always return 
	/// a reference to the exact same log object. 
	/// </para>
	/// <example>
	/// For example, in: 
	/// <code>
	/// DiagnosticsLog a = GetDiagnosticsLog("mylog");
	/// DiagnosticsLog b = GetDiagnosticsLog("mylog");
	/// DiagnosticsLog c = GetDiagnosticsLog("somelog");
	///	</code>
	///	a and b refer to exactly the same log object. While c refers to a different log object.
	/// </example>	
	/// <para>
	/// Thus, it is possible to configure a log and then to retrieve the same instance 
	/// somewhere else in the code without passing around references (see naming logs below). 
	/// </para>
	/// <para>
	/// Naming logs: You are totally free in choosing the names of your logs. Some suggestiong follows:
	/// <list type="bullet">
	///	<item>
	///	Using fully qualified name of the Type, including the namespace, is a useful and straightforward 
	///	approach of defining logs. 
	/// <example>
	/// For example, in: 
	/// <code>
	/// GetDiagnosticsLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
	///	</code>
	/// </example>	
	///	</item>
	///	<item>
	///	Name logs by functional areas. For example, "acquisition", "ExperimentalData", etc.. 
	///	</item>
	/// </list>
	/// </para>
	/// <para>
	/// Logs can be created at any time simply by calling the GetDiagnosticsLog method and/or by thru a configuration file. 
	/// </para>
	/// <para>
	/// Logs may be assigned levels. The following levels are defined in order of increasing priority:
	/// <list type="bullet">
	///	<item>ALL</item>
	///	<item>DEBUG</item>
	///	<item>INFO</item>
	///	<item>WARN</item>
	///	<item>SERIOUS</item>
	///	<item>FATAL</item>
	///	<item>OFF</item>
	/// </list>
	/// If a given log is not assigned a level, then the default level is ALL. 
	/// </para>
	/// <para>
	/// The following is the series of steps and checks that a messages goes through while being logged. 
	/// For the purposes of this example we will document an INFO level message being logged on 
	/// the default application log.
	/// <list type="bullet">
	///	<item>
	/// The user logs a message using the Info method on the default application log by obtained using a call to 
	/// the static DefaultLog accessor. 
	/// <example>
	/// For example:
	/// <code>
	/// DiagnosticsLogService.DefaultLog.Info("Application Started."); 
	///	</code>
	/// </example>	
	/// </item>
	/// The Diagnostics logger service provides level specific logging methods (i.e. Debug, Info, Warn, Error, etc..). 
	///	<item>
	/// The log level is compared to the message level to determine if the message can be logged. 
	/// If the message level is below the log level the message is not logged. 
	/// <example>
	/// For example: If the log level is changed to INFO as follows and we write a message that has a level of
	/// DEBUG, the message will not be written to the log.
	/// <code>
	/// DiagnosticsLogService.DefaultLog.LoggingLevel = INFO; 
	/// DiagnosticsLogService.DefaultLog.Debug("some text."); //message NOT be written to log.
	///	</code>
	/// </example>
	///	</item>
	/// </list>
	/// </para>
	/// <example>
	/// How to access diagnostic log services.
	/// <code>
	/// DiagnosticsLogService dls = DiagnosticsLogService.GetService;
	///	</code>
	/// </example>	
	/// <example>
	/// How to create and write to a log.
	/// <code>
	/// // Get reference to the diagnostic logger service.
	/// DiagnosticsLogService dls = DiagnosticsLogService.GetService;
	/// 	
	/// DiagnosticsLogItem dli = null;
	///
	/// // Get reference to the default application log.
	/// DiagnosticsLog defaultAppLog = dls.GetDiagnosticsLog(null);// null of empty string.
	/// // or use static accessor.
	/// defaultAppLog = DiagnosticsLogService.DefaultLog; 
	/// 
	/// // write a info level message to the log.
	/// dli = defaultAppLog.Info("some message");
	/// 
	/// // Create log object (named MyLog) programmatically. 
	/// DiagnosticsLog dl = dls.GetDiagnosticsLog("MyLog");
	/// // Set logging level.
	/// dl.LoggingLevel = DiagnosticLevel.ALL;
	/// 
	/// // Following statements show the various logging level methods for writing log entries.
	/// dli = dl.Debug("message goes here");
	/// 
	/// dli = dl.Info("message goes here");
	/// 
	/// dli = dl.Error("message goes here");
	/// 
	/// dli = dl.Warn("message goes here");
	/// 
	/// try
	/// {
	/// }
	/// catch(Exception ex)
	/// {
	///		dl.SeriousError("message", ex);// takes an exception object.
	///	}
	/// 
	/// try
	/// {
	/// }
	/// catch(Exception ex)
	/// {
	///		dl.Exception(ex);// takes an exception object.
	///	}
	///	</code>
	/// </example>	
	/// <example>
	/// How to enumerate logs managed by service.
	/// <code>
	/// foreach(DiagnosticsLog log in DiagnosticsLogService.GetService)
	/// {
	///		Console.WriteLine(log.logName);
	/// }
	///	</code>
	/// </example>	
	/// <example>
	/// How to receive DiagnosticsLogEvent events whenever an entry is written to any log.
	/// <code>
	/// // Get reference to service.
	/// DiagnosticsLogService dls = DiagnosticsLogService.GetService;
	/// // Subscribe to entry written events.
	/// dls.DiagnosticsLogWriteEvent += new DiagnosticsLogWriteEventHandler(OnDiagnosticsLogEvent); 
	/// 
	/// // Handler for events.
	/// void OnDiagnosticsLogEvent(object sender, DiagnosticsLogItem e)
	/// {
	///		// Get log name.
	///		string logname = e.LogName;
	/// }
	///	</code>
	/// </example>	
	/// <example>
	/// How to receive DiagnosticsLogEvent events whenever an entry is written to a specific logger.
	/// <code>
	/// //Get reference to your log, "MyLog".
	/// DiagnosticsLog dl = dls.GetDiagnosticsLog("MyLog");
	/// dl.DiagnosticsLogEvent += new DiagnosticsLogWriteEventHandler(OnDiagnosticsMyLogEvent);
	/// 
	/// // Event handler
	/// void OnDiagnosticsMyLogEvent(object sender, DiagnosticsLogItem e)
	/// {
	///		// Get log name.
	///		string logname = e.LogName;
	///		
	///		// Get the written log entry.
	///		DiagnosticsLogItem dli = e.LogEntry;
	/// }
	///	</code>
	/// </example>	
	/// <para>
	/// Configuration file:
	/// Inserting log requests into the application code requires a fair amount of planning and effort. 
	/// Consequently, even moderately sized applications will have thousands of logging statements 
	/// embedded within their code. Given their number, it becomes imperative to manage these log statements 
	/// without the need to modify them manually. 
	/// The Diagnostic logger is fully configurable programmatically. However, 
	/// loggers can be managed using configuration files. Configuration files are written in XML. 
	/// </para>
	/// <para>
	/// Log files:
	/// Each process has a default log file. 
	/// All log entries from all logs managed by the diagnostic service are written 
	/// to the default application log file. 
	/// A seperate log file can be created for a log. This can only be done 
	/// via the configuration file by setting the "file" attribute to the name of the log file
	/// you want.
	/// </para>
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Ralph Ansell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">530</see> Diagnostics Logger.
	///			</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">557</see> Diagnostics Logger Package.
	///			</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">627</see> Diagnostics Logger Service, see class <see cref="DiagnosticsLogService"/>
	///			</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">626</see>, see class <see cref="DiagnosticsLog"/>
	///			</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">628</see>, <see cref="DiagnosticsLogItem"/>
	///			</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">909</see>, <see cref="DiagnosticLevel"/>
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\DiagLogger.wmf">Class Diagram</see>
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: AboutDiagnosticLogger.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/AboutDiagnosticLogger.cs $</item>
	///			<item name="vssrevision">$Revision: 13 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	sealed public partial class AboutDiagnosticLogger
	{
	}
}
