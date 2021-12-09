using System;
using System.Globalization;
using System.Runtime.Serialization;

using BioRad.Common.DiagnosticsLogger;

namespace BioRad.Common.ApplicationExceptions
{
	#region Documentation Tags
	/// <summary>
	/// Exception type for ignored exceptions (exceptions that are caught and not rethrown).
	/// Use this type when creating an exception for logging purposes only.
	/// This exception should not be thrown.
	/// </summary>
	/// <remarks>
	/// Constructor requires a contained inner exception (the exception that is being ignored).
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Lisa von Schlegell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="conformancereview">Conformance review: LvS 9/14/04</item>
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
	///			<item name="vssfile">$Workfile: LoggableIgnoredException.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ApplicationExceptions/LoggableIgnoredException.cs $</item>
	///			<item name="vssrevision">$Revision: 6 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	[Serializable]
	public partial class LoggableIgnoredException : LoggableApplicationException
	{
		#region Accessors
		/// <summary>
		/// Override prepends "exception ignored" message to message string.
		/// </summary>
		public override string Message
		{
			get
			{
                return StringUtility.FormatString(Properties.Resources.ExceptionIgnored_1, base.Message);
			}
		}

		#endregion

		#region Constructors and Destructor
        /// <summary>
        /// Construct and log a loggable application exception from the given parameters.
        /// The sender parameter is used to determine the appropriate log.
        /// </summary>
        /// <remarks>Exception severity is set to Serious, tag is set to EXCEPTION.</remarks>
        /// <param name="sender">Object originating the exception. If object implements
        /// IProvidesLogName the exception is logged to that log, otherwise it is logged
        /// to the default log.</param>
        /// <param name="message">The localized message.</param>
        /// <param name="ignoredException">Inner exception for this exception.</param>
        public LoggableIgnoredException(Object sender, string message, Exception ignoredException)
            : this(sender, DiagnosticSeverity.Serious, DiagnosticTag.EXCEPTION, message, ignoredException)
        {
        }

        /// <summary>
        /// Construct and log a loggable application exception from the given parameters.
        /// The sender parameter is used to determine the appropriate log.
        /// </summary>
        /// <remarks>Exception severity is set to that of the inner exception, tag is set to 
        /// EXCEPTION.</remarks>
        /// <param name="sender">Object originating the exception. If object implements
        /// IProvidesLogName the exception is logged to that log, otherwise it is logged
        /// to the default log.</param>
        /// <param name="message">The localized message.</param>
        /// <param name="ignoredException">Inner exception for this exception.</param>
        public LoggableIgnoredException(Object sender, string message, LoggableApplicationException ignoredException)
            : this(sender, ignoredException.Severity, DiagnosticTag.EXCEPTION, message, ignoredException)
        {
        }

        /// <summary>
        /// Construct and log a loggable application exception from the given parameters.
        /// The sender parameter is used to determine the appropriate log.
        /// </summary>
        /// <remarks>Use this constructor to control exception tag and severity explicitly.</remarks>
        /// <param name="sender">Object originating the exception. If object implements
        /// IProvidesLogName the exception is logged to that log, otherwise it is logged
        /// to the default log.</param>
        /// <param name="ds">Severity of event.</param>
        /// <param name="dt">Tag identifying originating subsystem or operation.</param>
        /// <param name="message">The localized message.</param>
        /// <param name="ignoredException">Inner exception for this exception.</param>
        public LoggableIgnoredException(Object sender, DiagnosticSeverity ds, DiagnosticTag dt,
            string message, Exception ignoredException)
            : base(sender, ds, dt, message, ignoredException)
        {
        }
        /// <summary>
		/// De-serialization constructor. Required for remoted exceptions.
		/// </summary>
		/// <param name="info">Holds the data needed to deserialize the object.</param>
		/// <param name="context">Context for the serialization stream.</param>
		protected LoggableIgnoredException(SerializationInfo info, StreamingContext context): 
			base (info, context)
		{
		}
		#endregion
	}
}
