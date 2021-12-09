using System;
using System.Globalization;
using System.Runtime.Serialization;

using BioRad.Common.DiagnosticsLogger;

namespace BioRad.Common.ApplicationExceptions
{
	#region Documentation Tags
	/// <summary>
	/// Exception type for user notification of the wrapped exception.
	/// Use this type when creating an exception for notification purposes only (exceptions
	/// that have been caught at the top level and are being reported to the user.)
	/// This exception should not be thrown.
	/// </summary>
	/// <remarks>
	/// Constructor requires a contained inner exception (the exception that is being notified).
	/// Message property will combine this exception's message with the inner exception message.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Lisa von Schlegell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="conformancereview">Conformance review:</item>
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
	///			<item name="vssfile">$Workfile: LoggableNotifiedException.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ApplicationExceptions/LoggableNotifiedException.cs $</item>
	///			<item name="vssrevision">$Revision: 5 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	[Serializable]
	public partial class LoggableNotifiedException : LoggableApplicationException
	{
		#region Accessors
		/// <summary>
		/// Override combines message with inner exception message.
		/// </summary>
		public override string Message
		{
			get
			{
				//StringResource sr;
				if (base.Message != String.Empty)
                    return StringUtility.FormatString(Properties.Resources.ExceptionNotified_2, base.Message,
					 this.InnerException.Message);
				else
                    return StringUtility.FormatString(Properties.Resources.ExceptionNotified_1,
						this.InnerException.Message);
				//return sr.ToString();
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
        public LoggableNotifiedException(Object sender, string message, Exception ignoredException)
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
        public LoggableNotifiedException(Object sender, string message, LoggableApplicationException ignoredException)
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
        public LoggableNotifiedException(Object sender, DiagnosticSeverity ds, DiagnosticTag dt,
            string message, Exception ignoredException)
            : base(sender, ds, dt, message, ignoredException)
        {
        }

        /// <summary>
		/// De-serialization constructor. Required for remoted exceptions.
		/// </summary>
		/// <param name="info">Holds the data needed to deserialize the object.</param>
		/// <param name="context">Context for the serialization stream.</param>
		protected LoggableNotifiedException(SerializationInfo info, StreamingContext context): base (info, context)
		{
		}
		#endregion
	}
}
