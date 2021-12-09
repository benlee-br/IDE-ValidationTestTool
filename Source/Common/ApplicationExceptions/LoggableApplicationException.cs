using System;
using System.Globalization;
using System.Runtime.Serialization;
using BioRad.Common.DiagnosticsLogger;

namespace BioRad.Common.ApplicationExceptions
{
	#region Documentation Tags
	/// <summary>LoggableApplication exceptions may be thrown when a run-time fault is
    /// detected by any component.  This class carries essential information about 
    /// exceptions unique to iQ2G operations.  Exceptions occurring during data 
    /// acquisition will be persisted with the experimental data. Exception message
    /// can be re-localized.
	/// This exception implements <see cref="ISerializable"/>, so is remoteable.
	/// <para>This class uses the DiagnosticsLogger package (<see cref="AboutDiagnosticLogger"/>).</para>
	/// </summary>
	/// <remarks>
	/// If a field is added to this class be sure to explicitly serialize and de-serialize
	/// the field.
	/// <para>Currently, exceptions thrown when attempting to log an exception are ignored.</para>
	/// <para>Derived types must at minimum implement constructors with and without an
	/// embedded exception and a deserialization constructor.</para>
	/// <para>If a derived type defines additional field(s), it must also provide an override of the
	/// <see cref="LoggableApplicationException.GetObjectData"/> method.
	/// </para>
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:DSpyr, Lisa von Schlegell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">902</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: LoggableApplicationException.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/ApplicationExceptions/LoggableApplicationException.cs $</item>
	///			<item name="vssrevision">$Revision: 30 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ssarup $</item>
	///			<item name="vssdate">$Date: 5/23/06 12:33p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	[Serializable]
	public class LoggableApplicationException: ApplicationException, IDeserializationCallback, IProvidesLogName
	{
		#region Constants
		/// <summary>
		/// For testing only!: log name to use to bypass diagnostic logging entirely
		/// </summary>
		public static readonly string DoNotLogName = "TestOnlyDoNotLog";
		/// <summary>
		/// Serialization ID tag for m_Severity
		/// </summary>
		private const string c_SeverityTag = @"LoggableApplicationException.Severity";
		/// <summary>
		/// Serialization ID tag for m_Tag
		/// </summary>
		private const string c_TagTag = @"LoggableApplicationException.Tag";
		/// <summary>
		/// Serialization ID tag for m_LogName
		/// </summary>
		private const string c_LogNameTag = @"LoggableApplicationException.LogName";
		/// <summary>
        /// Serialization ID tag for m_Message
        /// </summary>
        private const string c_MessageTag = @"LoggableApplicationException.Message";
		/// <summary>
		/// HResult error code (msb). Used to construct <see cref="Exception.HResult"/>.
		/// </summary>
		private const uint c_E = 0x80000000;
		/// <summary>
		/// HResult warning code (currently same as error). Used to construct <see cref="Exception.HResult"/>.
		/// </summary>
		private const uint c_W = 0x80000000;
		/// <summary>
		/// HResult informational code bit (msb). Used to construct <see cref="Exception.HResult"/>.
		/// </summary>
		private const uint c_I = 0x00000000;
		/// <summary>
		/// Facility code for user defined exceptions. Used to construct <see cref="Exception.HResult"/>.
		/// </summary>
		private const int c_FACILITY_ITF = 0x40000;
		/// <summary>This is the beginning of the range for user-defined fault return
		/// codes.  It is consistent with the user-available range of Microsoft's HResult
		/// codes.</summary>
		protected const int c_ErrorCodeUserDefinedBase = 0x200;
		/// <summary>
		/// All error codes are defined here. Derived classes may use a specific error
		/// code to set the HResult in their constructor by using the <see cref="LoggableApplicationException.MakeAppHResult"/>
		/// method. NOTE: Use of <see cref="Exception.HResult"/> is not encouraged by Microsoft .NET guidelines!
		/// </summary>
		protected enum ApplicationErrors 
		{
			/// <summary>
			/// Generic application error code.
			/// </summary>
			ApplicationError,
			// Add further error codes here. To preserve error code values, add only to
			// the end of the list.
		}

		#endregion

		#region Member Data
		/// <summary>Diagnostics log identifier.</summary>
		private string m_LogName;
        ///// <summary>String resource object containing a localizable message.</summary>
        //private StringResource m_MessageResource;
        private string m_Message;
		/// <summary>Exception severity.</summary>
		private DiagnosticSeverity m_Severity;
		/// <summary>Tag (unique type identifier) of the diagnostic event.  It identifies 
		/// the subsystem or operation associated with the event.</summary>
		private DiagnosticTag m_Tag;
		#endregion

		#region Accessors
		/// <summary>HResult for this exception object.</summary>
		/// <remarks>This field is exposed only for use by the DiagnosticsLogger package.</remarks>
		public int FaultCode
		{
			get
			{return this.HResult;}
		}

		/// <summary>Diagnostics log identifier for this exception object.</summary>
		public string LogName
		{
			get
			{return m_LogName;}
		}

		/// <summary>
		/// Overrides Message accessor to return localized string.
		/// </summary>
		/// <remarks>If message string resource is null, returns the empty string.</remarks>
		public override string Message
		{ 
			get 
			{ 
				if (this.m_Message != null)
					return this.m_Message;
				else 
					return String.Empty;
			}
		}

        ///// <summary>String resource object containing a localizable message.</summary>
        //public virtual StringResource MessageResource
        //{
        //    get
        //    {return m_MessageResource;}
        //}

		/// <summary>Exception severity.</summary>
		public DiagnosticSeverity Severity
		{
			get
			{return m_Severity;}
		}

		/// <summary>Tag (unique type identifier) of the diagnostic event.  It identifies 
		/// the subsystem or operation associated with the event.</summary>
		public DiagnosticTag Tag
		{
			get
			{return m_Tag;}
		}
		#endregion

		#region Constructors and Destructor
        /// <summary>
        /// Construct and log a loggable application exception from the given parameters.
        /// The sender parameter is used to determine the appropriate log.
        /// </summary>
        /// <remarks>All derived types should provide an implementation of this constructor.
        /// Derived constructors should defer to the appropriate non-logging constructor, then
        /// explicitly log the exception.</remarks>
        /// <param name="sender">Object originating the exception. If object implements
        /// <see cref="IProvidesLogName"/> the exception is logged to that log, otherwise it is logged
        /// to the default log.</param>
        /// <param name="ds">Severity of event.</param>
        /// <param name="dt">Tag identifying originating subsystem or operation.</param>
        /// <param name="message">The localized message.</param>
        public LoggableApplicationException(Object sender, DiagnosticSeverity ds, DiagnosticTag dt,
            string message): this(sender, ds, dt, message, (Exception)null)
        {
        }

        /// <summary>
        /// Construct and log a loggable application exception from the given parameters.
        /// The sender parameter is used to determine the appropriate log.
        /// </summary>
        /// <remarks>All derived types should provide an implementation of this constructor.
        /// Derived constructors should defer to the appropriate non-logging constructor, then
        /// explicitly log the exception.</remarks>
        /// <param name="sender">Object originating the exception. If object implements
        /// <see cref="IProvidesLogName"/> the exception is logged to that log, otherwise it is logged
        /// to the default log.</param>
        /// <param name="ds">Severity of event.</param>
        /// <param name="dt">Tag identifying originating subsystem or operation.</param>
        /// <param name="message">The localized message.</param>
        /// <param name="innerException">Inner exception for this exception.</param>
        public LoggableApplicationException(Object sender, DiagnosticSeverity ds, DiagnosticTag dt,
            string message, Exception innerException)
            : this(ds, dt, message, innerException, String.Empty, false)
        {
            this.Log(sender);
        }

        /// <summary>
        /// De-serialization constructor. Required for remoted exceptions.
        /// </summary>
        /// <remarks>All derived types should provide an implementation of this constructor.
        /// If no additional fields are defined derived constructors can simply defer to this base
        /// constructor. If additional fields are defined in the derived type they must be
        /// explicitly deserialized as well.</remarks>
        /// <param name="info">Holds the data needed to deserialize the object.</param>
        /// <param name="context">Context for the serialization stream.</param>
        protected LoggableApplicationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // deserialize simple fields
            m_Severity = (DiagnosticSeverity)info.GetValue(c_SeverityTag, typeof(DiagnosticSeverity));
            m_Tag = (DiagnosticTag)info.GetValue(c_TagTag, typeof(DiagnosticTag));
            m_LogName = info.GetString(c_LogNameTag);
            m_Message = info.GetString(c_MessageTag);
        }

        /// <summary>
        /// Construct a loggable application exception from the given parameters.
        /// This constructor should only be used to explicitly select a log.
        /// In general, it is preferable to pass in a sender parameter and allow the
        /// constructor to determine the appropriate log.
        /// </summary>
        /// <param name="ds">Severity of event.</param>
        /// <param name="dt">Tag identifying originating subsystem or operation.</param>
        /// <param name="message">The localized message.</param>
        /// <param name="logName">Log identifier for logging exeption.</param>
        public LoggableApplicationException(DiagnosticSeverity ds, DiagnosticTag dt,
            string message, string logName)
            : this(ds, dt, message, (Exception)null, logName, true)
        {
        }


        /// <summary>
        /// Construct a loggable application exception from the given parameters.
        /// This constructor should only be used to explicitly select a log.
        /// In general, it is preferable to pass in a sender parameter and allow the
        /// constructor to determine the appropriate log.
        /// </summary>
        /// <param name="ds">Severity of event.</param>
        /// <param name="dt">Tag identifying originating subsystem or operation.</param>
        /// <param name="message">The localized message.</param>
        /// <param name="logName">Log identifier for logging exeption.</param>
        /// <param name="innerException">Inner exception for this exception.</param>
        public LoggableApplicationException(DiagnosticSeverity ds, DiagnosticTag dt,
            string message, Exception innerException,
            string logName)
            : this(ds, dt, message, innerException, logName, true)
        {
        }

        /// <summary>
        /// Construct a loggable application exception from the given parameters.
        /// Private constructor used by public constructors.
        /// </summary>
        /// <param name="ds">Severity of event.</param>
        /// <param name="dt">Tag identifying originating subsystem or operation.</param>
        /// <param name="message">The localized message.</param>
        /// <param name="innerException">Inner exception for this exception.</param>
        /// <param name="logName">Log identifier for logging exeption.</param>
        /// <param name="logException">true if exception is to be logged.</param>
        private LoggableApplicationException(DiagnosticSeverity ds, DiagnosticTag dt,
            string message, Exception innerException,
            string logName, bool logException)
            : base(String.Empty, innerException)
        {
            m_Severity = ds;
            m_Tag = dt;
            m_LogName = logName;
            m_Message = (message == null) ? string.Empty : message;
            this.HResult = MakeAppHResult(ds, ApplicationErrors.ApplicationError);
            if (logException)
            {
                // Log exception to the indicated log file.
                this.Log();
            }
        }
        #endregion

        #region Methods
        /// <summary>
		/// Serialization method. Required for remoted exceptions. Calls base method,
		/// then explicitly serializes object fields.
		/// </summary>
		/// <param name="info">Used to serialize object data. </param>
		/// <param name="context">Context for the serialization stream.</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(c_SeverityTag, m_Severity);
			info.AddValue(c_TagTag, m_Tag);
			info.AddValue(c_LogNameTag, m_LogName);
			info.AddValue(c_MessageTag, m_Message);
		}

        ///// <summary>
        ///// Localize message text to a specific culture. Message property remains
        ///// relocalized.
        ///// </summary>
        ///// <param name="culture">Culture for which to localize message (null for current culture)</param>
        ///// <returns>Localized Message property.</returns>
        //public virtual string Localize(CultureInfo culture)
        //{
        //    this.MessageResource.Localize(culture);
        //    return this.Message;
        //}

        ///// <summary>
        ///// Localize message to current culture. Message property remains relocalized.
        ///// </summary>
        ///// <returns>Localized Message property.</returns>
        //public string Localize()
        //{
        //    return this.Localize(null);
        //}

		/// <summary>
		/// Log exception to indicated log (helper method).
		/// <para>Override this method to intercept all exception logging.</para>
		/// </summary>
		/// <param name="dl">log</param>
		protected virtual void Log(DiagnosticsLog dl)
		{
			dl.LoggableException(this);
		}

		/// <summary>
		/// Log exception to sender's log (or default log if sender does not
		/// support IProvidesLogName). Updates LogName property from sender's log name and
		/// calls Log().
		/// </summary>
		/// <param name="sender">used to select log</param>
		protected virtual void Log(Object sender)
		{
			this.SetLogName(sender);
			this.Log();
		}

		/// <summary>
		/// Log exception to exception's log (helper method). Gets log reference from exception
		/// log name and calls Log(DiagnosticsLog). Catches and ignores errors.
		/// <para>Override this method to prevent logging from the deserialization
		/// constructor.</para>
		/// </summary>
		protected virtual void Log()
		{
			if (m_LogName != DoNotLogName)
			{
				// Log exception to the indicated log file.
				try
				{
					DiagnosticsLog dl = 
						DiagnosticsLogService.GetService.GetDiagnosticsLog(m_LogName);
					Log(dl);
				}
				catch
				{
					// Ignore errors, usually due to problems getting a diagnostics log
					// when running from NUnit.
					// TODO: revisit this if diagnostics log supports a more fail-safe method
					// of gettting a default log
				}
			}
		}

		/// <summary>
		/// Creates an HResult compatible with Microsoft's guidelines. HResult values are
		/// used by COM interop, and should map to a specific exception type. Derived exceptions
		/// should set the HResult value in their constructor.
		/// </summary>
		/// <remarks>HResults are used in COM interop. HResults have 3 fields: severity, 
		/// facililty, and code. The facility for applications is FACILITY_ITF 
		/// (ie 4 in bits 16.. 28; 0x40000. For applications, code will be in the range
		/// 0x200 .. 0xFFFF (lower 16 bits). Severity is upper 1 or maybe 3 bits. 
		/// To follow Microsoft naming convention HResult errors would be ITF_E_ErrorName,
		/// ITF_I_ErrorName, ITF_W_ErrorName for error, information and warning errors 
		/// respectively.
 		/// </remarks>
		/// <param name="ds"></param>
		/// <param name="error">error enumeration (0..0xFDFF), will be mapped to error code
		/// in the range 0x200 .. 0xFFFF.</param>
		/// <returns>HResult</returns>
		protected static int MakeAppHResult(DiagnosticSeverity ds, ApplicationErrors error)
		{
			// Map HResult into application FACILITY range
			uint hResult = c_FACILITY_ITF;
			// convert error code to range 0x200  .. 0xFFFF;
			int code = (int) error;
			code += c_ErrorCodeUserDefinedBase;
			code &= 0xFFFF;
			// Add error code to HResult
			hResult |= (uint) code;
			// Add severity code to HResult (top 1 or 2 bits).
			switch (ds)
			{
				case DiagnosticSeverity.Fatal: 
				case DiagnosticSeverity.Serious: 
					hResult |= c_E;
					break;
				case DiagnosticSeverity.Warning: 
				case DiagnosticSeverity.Unassigned:
					hResult |= c_W;
					break;
				case DiagnosticSeverity.Info:
					hResult |= c_I;
					break;
			}
			// Prevent overflow error by use of "unchecked"
			return unchecked((int) hResult);
		}

		/// <summary>
		/// Log exception after deserialization. This is an explicit interface method.
		/// Calls Log().
		/// </summary>
		/// <param name="sender">The object that initiated the callback (not implemented
		/// in .NET yet)</param>
		void IDeserializationCallback.OnDeserialization(Object sender)
		{
			this.Log();
		}

		/// <summary>
		/// Set LogName property from sender object, if sender supports <see cref="IProvidesLogName"/>.
		/// If not, LogName set to the empty string.
		/// </summary>
		/// <param name="sender">used to select log</param>
		protected void SetLogName(Object sender)
		{
			IProvidesLogName loggingSender = sender as IProvidesLogName;
			if (loggingSender != null)
			{
				// Get log name from sender
				m_LogName = loggingSender.LogName;
			}
			else
			{
				// Set log name to empty string by default
				m_LogName = String.Empty;
			}
		}
		#endregion
    }
}
