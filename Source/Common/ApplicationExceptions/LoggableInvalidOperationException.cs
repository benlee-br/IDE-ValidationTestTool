using System;
using System.Globalization;
using System.Runtime.Serialization;
using BioRad.Common.DiagnosticsLogger;

namespace BioRad.Common.ApplicationExceptions
{
	#region Documentation Tags
	/// <summary>
	/// Loggable exception thrown for invalid operations.
	/// </summary>
	/// <remarks>
	/// Use as for <see cref="InvalidOperationException"/>.
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
	///			<item name="vssfile">$Workfile: LoggableInvalidOperationException.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ApplicationExceptions/LoggableInvalidOperationException.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	[Serializable]
	public partial class LoggableInvalidOperationException : LoggableApplicationException
	{
		#region Constructors and Destructor
        /// <summary>
        /// Construct and log exception from the given parameters.
        /// The sender parameter is used to determine the appropriate log.
        /// </summary>
        /// <param name="sender">Object originating the exception. If object implements
        /// IProvidesLogName the exception is logged to that log, otherwise it is logged
        /// to the default log.</param>
        /// <param name="ds">Severity of event.</param>
        /// <param name="dt">Tag identifying originating subsystem or operation.</param>
        /// <param name="message">The localized message.</param>
        public LoggableInvalidOperationException(Object sender, DiagnosticSeverity ds, DiagnosticTag dt,
            string message)
            : base(sender, ds, dt, message)
        {
        }

        /// <summary>
        /// Construct and log a loggable application exception from the given parameters.
        /// The sender parameter is used to determine the appropriate log.
        /// </summary>
        /// <param name="sender">Object originating the exception. If object implements
        /// IProvidesLogName the exception is logged to that log, otherwise it is logged
        /// to the default log.</param>
        /// <param name="ds">Severity of event.</param>
        /// <param name="dt">Tag identifying originating subsystem or operation.</param>
        /// <param name="message">The localized message.</param>
        /// <param name="innerException">Inner exception for this exception.</param>
        public LoggableInvalidOperationException(Object sender, DiagnosticSeverity ds, DiagnosticTag dt,
            string message, Exception innerException)
            : base(sender, ds, dt, message, innerException)
        {
        }
        
        /// <summary>
		/// De-serialization constructor. Required for remoted exceptions.
		/// </summary>
		/// <param name="info">Holds the data needed to deserialize the object.</param>
		/// <param name="context">Context for the serialization stream.</param>
		protected LoggableInvalidOperationException(SerializationInfo info, StreamingContext context): 
		base (info, context)
		{
		}
		#endregion
	}
}
