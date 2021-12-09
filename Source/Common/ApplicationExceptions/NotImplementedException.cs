using System;
using System.Runtime.Serialization;
using BioRad.Common.DiagnosticsLogger;
using BioRad.Common.StringResources;

namespace BioRad.Common.ApplicationExceptions
{
	#region Documentation Tags
	/// <summary>
	/// Throw this exception when code accesses an unimplemented method.
	/// </summary>
	/// <remarks>
	///	For use during development.
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
	///			<item name="vssfile">$Workfile: NotImplementedException.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/ApplicationExceptions/NotImplementedException.cs $</item>
	///			<item name="vssrevision">$Revision: 1 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Lvs $</item>
	///			<item name="vssdate">$Date: 11/03/03 4:09p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	[Serializable]
	public class NotImplementedException: LoggableApplicationException
	{
        #region Constructors and Destructor
		/// <summary>
		/// Construct exception from the given parameters.
		/// This constructor does not log the exception! Log name is set to an empty
		/// string. This constructor should only be used when exception information is
		/// to be logged explicitly, or when testing exception independently of logging.
		/// </summary>
		/// <param name="ds">Severity of event.</param>
		/// <param name="dt">Tag identifying originating subsystem or operation.</param>
		/// <param name="message">String resouce containing localized message.</param>
		public NotImplementedException(DiagnosticSeverity ds, DiagnosticTag dt, 
			StringResource message) : base ( ds, dt, message)
		{
		}

		/// <summary>
		/// Construct and log exception from the given parameters.
		/// The sender parameter is used to determine the appropriate log.
		/// </summary>
		/// <param name="sender">Object originating the exception. If object implements
		/// IProvidesLogName the exception is logged to that log, otherwise it is logged
		/// to the default log.</param>
		/// <param name="ds">Severity of event.</param>
		/// <param name="dt">Tag identifying originating subsystem or operation.</param>
		/// <param name="message">String resouce containing localized message.</param>
		public NotImplementedException(Object sender, DiagnosticSeverity ds, DiagnosticTag dt, 
			StringResource message) : base(sender, ds, dt, message)
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
		/// <param name="message">String resouce containing localized message.</param>
		/// <param name="innerException">Inner exception for this exception.</param>
		public NotImplementedException(Object sender, DiagnosticSeverity ds, DiagnosticTag dt, 
			StringResource message, Exception innerException)
			: base(sender, ds, dt, message, innerException)
		{
		}
	
		/// <summary>
		/// De-serialization constructor. Required for remoted exceptions.
		/// </summary>
		/// <param name="info">Holds the data needed to deserialize the object.</param>
		/// <param name="context">Context for the serialization stream.</param>
		protected NotImplementedException(SerializationInfo info, StreamingContext context): base (info, context)
		{
		}
		#endregion
		#region Methods
		/// <summary>
		/// Override adds boilerplate to message.
		/// </summary>
		public override string Message
		{
			get
			{
				return String.Format(ApplicationExceptionsStrResNames.GetStringResourceValue(
					ApplicationExceptionsStrResNames.NotImplementedMessage),base.Message);
			}
		}

		#endregion
	}
}
