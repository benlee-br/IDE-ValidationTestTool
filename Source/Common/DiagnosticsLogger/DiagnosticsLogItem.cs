using System;
using System.Resources;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using BioRad.Common;
using BioRad.Common.ApplicationExceptions;
using BioRad.Common.Xml;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>
	/// The XmlTextWriter implementation does not do the following: 
	/// <list type="bullet">
	/// <item>
	/// The XmlTextWriter does not verify that element or attribute names are valid. 
	/// </item>
	/// <item>
	/// The XmlTextWriter writes Unicode characters in the range 0x0 to 0x20, 
	/// and the characters 0xFFFE and 0xFFFF, which are not XML characters. 
	/// </item>
	/// </list>
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Ralph Ansell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">Replace this text with ID</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\DiagLogger.wmf">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: DiagnosticsLogItem.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/DiagnosticsLogItem.cs $</item>
	///			<item name="vssrevision">$Revision: 49 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 11/12/07 2:42p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	internal partial class LogTextWriter : XmlTextWriter
	{
		/// <summary>
		/// Text content.
		/// </summary>
		private StringWriter m_StringWriter;
		/// <summary>
		/// Get text content.
		/// </summary>
		public StringWriter GetStringWriter
		{
			get
			{
				return m_StringWriter;
			}
		}
		/// <summary>
		/// Initializes a new instance of the LogTextWriter class.
		/// </summary>
		/// <param name="stringWriter"></param>
		public LogTextWriter(StringWriter stringWriter)
			: base(stringWriter)
		{
			m_StringWriter = stringWriter;
		}
        private static string GetFilteredString(string text)
        {
            string newString = System.Security.SecurityElement.Escape(text);
            UTF8Encoding utf8 = new UTF8Encoding();
            Byte[] encodedBytes = utf8.GetBytes(newString);
            String decodedString = utf8.GetString(encodedBytes, 0, encodedBytes.Length);
            return decodedString;
        }
		/// <summary>
		/// Writes the given text content.
		/// </summary>
		/// <param name="value"></param>
		public override void WriteString(string value)
		{
			if (value != null)
			{
                //string str = GetFilteredString(value);
                base.WriteString(value);
			}
		}
		/// <summary>
		/// To verify that the names written by the WriteStartElement method are valid, 
		/// the EncodeLocalName method of the XmlConvert class is used. 
		/// The EncodeLocalName method ensures that the characters in the name are valid by converting any 
		/// invalid character to a valid representation. 
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="localName"></param>
		/// <param name="ns"></param>
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			base.WriteStartElement(prefix, XmlConvert.EncodeLocalName(localName), ns);
		}
	}

	#region Documentation Tags
	/// <summary>
	/// The internal representation of log entries. 
	/// Diagnostic log entries of various kinds are converted to diagnostic log
	/// item. 
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Ralph Ansell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">628</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\DiagLogger.wmf">Class Diagram</see>  
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: DiagnosticsLogItem.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/DiagnosticsLogItem.cs $</item>
	///			<item name="vssrevision">$Revision: 49 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 11/12/07 2:42p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	[Serializable]
	public partial class DiagnosticsLogItem : EventArgs, ICloneable
	{
		#region Constants
		/// <summary>
		/// Newline string defined for this environment.
		/// </summary>
		private static readonly string NewLine = System.Environment.NewLine;
		#endregion

		#region Member Data
		/// <summary>
		/// User defined coded numerical value.
		/// </summary>
		private Int32 m_Data = 0;
		/// <summary>
		/// Diagnostic Level.
		/// </summary>
		private DiagnosticLevel m_Level = DiagnosticLevel.FATAL;
		/// <summary>
		/// Full name of assembly where entry was written.  
		/// </summary>
		private string m_AssemblyNameFull = "";
		/// <summary>
		/// Description of the diagnostic event.
		/// </summary>
		private string m_Message = "";
		/// <summary>
		/// Name (part of name-value pair) of localized message.  Used to convert
		/// localized message strings back to English.
		/// </summary>
		private string m_MessageName = "";
		/// <summary>
		/// Severity Name
		/// </summary>
		private string m_SeverityName = "";
		/// <summary>
		/// Severity of the diagnostic event.
		/// </summary>
		private DiagnosticSeverity m_Severity = DiagnosticSeverity.Unassigned;
		/// <summary>
		/// Tag Name
		/// </summary>
		private string m_TagName = "";
		/// <summary>
		/// Tag (unique type identifier) of the diagnostic event.
		/// </summary>
		private DiagnosticTag m_Tag = DiagnosticTag.Unassigned;
		/// <summary>
		/// Time stamp.
		/// </summary>
		private string m_TimeStamp = "";
		/// <summary>
		/// Exception object.
		/// </summary>
		private Exception m_Exception = null;
		/// <summary>
		/// Log Name.
		/// </summary>
		private string m_LogName = "";
		#endregion

		#region Accessors
		/// <summary>
		/// Get/Set User defined coded numerical value..
		/// </summary>
		public Int32 Data
		{
			get
			{
				return m_Data;
			}
			set
			{
				m_Data = value;
			}
		}
		/// <summary>
		/// Get/Set the <see cref="DiagnosticLevel" /> of the logging event.
		/// </summary>
		public DiagnosticLevel Level
		{
			get
			{
				return m_Level;
			}
			set
			{
				m_Level = value;
			}
		}
		/// <summary>
		/// Get/Set the name of the logger that logged the event.
		/// </summary>
		public string LogName
		{
			get
			{
				return m_LogName;
			}
			set
			{
				m_LogName = value;
			}
		}
		/// <summary>
		/// Get/Set the exception object for this event.
		/// </summary>
		public Exception GetException
		{
			get
			{
				return m_Exception;
			}
			set
			{
				m_Exception = value;
			}
		}
		/// <summary>
		/// Get/Set the time of the logging event.
		/// </summary>
		public string TimeStamp
		{
			get
			{
				return m_TimeStamp;
			}
			set
			{
				m_TimeStamp = value;
			}
		}
		/// <summary>
		/// Get/Set Full name of assembly where entry was written.
		/// </summary>
		public string AssemblyNameFull
		{
			get
			{
				return m_AssemblyNameFull;
			}
			set
			{
				m_AssemblyNameFull = value;
			}
		}
		/// <summary>
		/// Get/Set description of the diagnostic event.
		/// </summary>
		public string Message
		{
			get
			{
				return m_Message;
			}
			set
			{
				m_Message = value;
			}
		}
		/// <summary>
		/// Get/Set name (part of name-value pair) of localized message.  Used to convert
		/// localized message strings back to English.
		/// </summary>
		public string MessageName
		{
			get
			{
				return m_MessageName;
			}
			set
			{
				m_MessageName = value;
			}
		}
		/// <summary>
		/// Get/Set severity of the diagnostic event <see cref="DiagnosticSeverity"/>.
		/// </summary>
		public DiagnosticSeverity Severity
		{
			get
			{
				return m_Severity;
			}
			set
			{
				m_Severity = value;
			}
		}
		/// <summary>
		/// Get trace of call stack from point of exception throw to point of catch.
		/// (exceptions only).
		/// </summary>
		public string StackTrace
		{
			get
			{
				return RenderException(GetException);
			}
		}
		/// <summary>
		/// Get/Set tag (unique type identifier) of the diagnostic event  <see cref="DiagnosticTag"/>.
		/// </summary>
		public DiagnosticTag Tag
		{
			get
			{
				return m_Tag;
			}
			set
			{
				m_Tag = value;
			}
		}
		/// <summary>
		/// Get localized diagnostic tag string representation.
		/// </summary>
		public string TagName
		{
			get
			{
				return m_Tag.ToString();//todo: localize
			}
		}
		/// <summary>
		/// Get localized diagnostic severity string representation.
		/// </summary>
		public string SeverityName
		{
			get
			{
				return m_Severity.ToString();//todo: localize
			}
		}
		/// <summary>
		/// Get localized diagnostic level string representation. 
		/// </summary>
		public string LevelName
		{
			get
			{
				return m_Level.ToString();//todo: localize
			}
		}
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Initializes a new instance of the DiagnosticsLogItem class.
		/// </summary>
		internal DiagnosticsLogItem()
		{
		}
		/// <summary>
		/// Initializes a new instance of the DiagnosticsLogItem class.
		/// </summary>
		/// <param name="logName">Name of the logger.</param>
		/// <param name="assemblyName">Full assembly name of calling method that logged the event.</param>
		/// <param name="ds">Diagnostic severity.</param>
		/// <param name="dt">Diagnostic tag.</param>
		/// <param name="message">Diagnostic message.</param>
		/// <param name="messageName">
		/// Name (part of name-value pair) of localized message.  Used to convert
		/// localized message strings back to English.
		/// </param>
		/// <param name="ex">The exception object for this event.</param>
		/// <param name="level"></param>
		/// <param name="data">User defined coded numerical value.</param>
		/// <remarks>
		/// Time stamps log entry.
		/// </remarks>
		internal DiagnosticsLogItem(
				string logName,
				string assemblyName,
				DiagnosticSeverity ds,
				DiagnosticTag dt,
				string message,
				string messageName,
				Exception ex,
				DiagnosticLevel level, Int32 data)
		{
			m_TimeStamp = ISO8601DateTime.ToString(DateTime.Now);

			m_Data = data;
			m_Level = level;
			m_LogName = logName;
			m_Severity = ds;
			m_SeverityName = SeverityName;
			m_Tag = dt;
			m_TagName = TagName;
			m_Message = message;
			m_MessageName = messageName;
			m_AssemblyNameFull = assemblyName;
			m_Exception = ex;
		}
		/// <summary>
		/// Initializes a new instance of the DiagnosticsLogItem class.
		/// </summary>
		/// <param name="logName"></param>
		/// <param name="dateTime"></param>
		/// <param name="assemblyName"></param>
		/// <param name="severityName"></param>
		/// <param name="tagName"></param>
		/// <param name="message"></param>
		/// <param name="messageName"></param>
		/// <param name="stackTrace"></param>
		/// <param name="level"></param>
		/// <param name="data">User defined coded numerical value.</param>
		public DiagnosticsLogItem(
				string logName,
				string dateTime,
				string assemblyName,
				string severityName,
				string tagName,
				string message,
				string messageName,
				string stackTrace,
				string level,
				string data)
		{
			m_Data = Convert.ToInt32(data);
			m_Level = GetLevel(level);
			m_LogName = logName;

			m_Severity = GetSeverity(severityName);
			m_SeverityName = SeverityName;

			m_Tag = GetTag(tagName);
			m_TagName = TagName;

			m_TimeStamp = dateTime;
			m_Message = message;
			m_MessageName = messageName;
			m_AssemblyNameFull = assemblyName;
			if ((stackTrace != null) && (stackTrace.Length > 0))
				m_Exception = new Exception(stackTrace);
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="DiagnosticsLogItem" /> class 
		/// with serialized data.
		/// </summary>
		/// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data.</param>
		/// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
		protected DiagnosticsLogItem(SerializationInfo info, StreamingContext context)
		{
			m_TimeStamp = DateTime.Now.ToString(System.Globalization.DateTimeFormatInfo.InvariantInfo);
			m_Severity = (DiagnosticSeverity)info.GetInt32("Severity");
			m_SeverityName = SeverityName;
			m_Tag = (DiagnosticTag)info.GetInt32("Tag");
			m_TagName = TagName;
			m_Message = info.GetString("Message");
			m_MessageName = info.GetString("MessageName");
			m_AssemblyNameFull = info.GetString("AssemblyNameFull");
			m_Exception = new Exception(info.GetString("StackTrace"));
			m_Level = (DiagnosticLevel)info.GetInt32("Level");
			m_LogName = info.GetString("LogName");
			m_Data = info.GetInt32("Data");
		}
		#endregion

		#region Methods
		/// <summary>
		/// Serializes this object into the <see cref="SerializationInfo" /> provided.
		/// </summary>
		/// <param name="info">The <see cref="SerializationInfo" /> to populate with data.</param>
		/// <param name="context">The destination for this serialization.</param>
		/// <remarks>
		/// </remarks>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("TimeStamp", m_TimeStamp);
			info.AddValue("Message", Message);
			info.AddValue("MessageName", MessageName);
			info.AddValue("AssemblyNameFull", AssemblyNameFull);
			info.AddValue("Tag", m_Tag);
			info.AddValue("Severity", m_Severity);
			info.AddValue("StackTrace", StackTrace);
			info.AddValue("LogName", m_LogName);
			info.AddValue("Level", m_Level);
			info.AddValue("Data", m_Data);
		}
		/// <summary>
		/// Get level from level name.
		/// </summary>
		/// <param name="fromLevelName">Level name</param>
		/// <returns></returns>
		public DiagnosticLevel GetLevel(string fromLevelName)
		{
			DiagnosticLevel level = DiagnosticLevel.ALL;
			DiagnosticLevel[] arrLevels = (DiagnosticLevel[])Enum.GetValues(typeof(DiagnosticLevel));
			foreach (DiagnosticLevel l in arrLevels)
			{
				if (l.ToString() == fromLevelName)
				{
					level = l;
					break;
				}
			}
			return level;
		}
		/// <summary>
		/// Get diagnostic tag from tag name
		/// </summary>
		/// <param name="fromTagName">Tag name</param>
		/// <returns></returns>
		public DiagnosticTag GetTag(string fromTagName)
		{
			DiagnosticTag tag = DiagnosticTag.Unassigned;
			DiagnosticTag[] tagArray = (DiagnosticTag[])Enum.GetValues(typeof(DiagnosticTag));
			foreach (DiagnosticTag t in tagArray)
			{
				if (t.ToString() == fromTagName)
				{
					tag = t;
					break;
				}
			}
			return tag;
		}
		/// <summary>
		/// Get diagnostic severity from severity name.
		/// </summary>
		/// <param name="fromSeverityName">everity Name</param>
		/// <returns></returns>
		public DiagnosticSeverity GetSeverity(string fromSeverityName)
		{
			DiagnosticSeverity s = DiagnosticSeverity.Unassigned;
			DiagnosticSeverity[] dsArray = (DiagnosticSeverity[])Enum.GetValues(typeof(DiagnosticSeverity));
			foreach (DiagnosticSeverity ds in dsArray)
			{
				if (ds.ToString() == fromSeverityName)
				{
					s = ds;
					break;
				}
			}
			return s;
		}
		/// <summary>
		/// Get formatted XML element string that represents the current Object.
		/// </summary>
		/// <returns>string</returns>
		public string ToXmlString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Length = 0;

			StringWriter stringWriter = new StringWriter();
			LogTextWriter writer = new LogTextWriter(stringWriter);

			writer.WriteStartElement(XmlElementName.Log.ToString());

			writer.WriteStartElement(XmlElementName.LgNm.ToString());
			writer.WriteString(LogName);
			writer.WriteEndElement();

			writer.WriteStartElement(XmlElementName.Level.ToString());
			writer.WriteString(LevelName);
			writer.WriteEndElement();

			writer.WriteStartElement(XmlElementName.TS.ToString());
			writer.WriteString(TimeStamp);
			writer.WriteEndElement();

			writer.WriteStartElement(XmlElementName.ANm.ToString());
			writer.WriteString(AssemblyNameFull);
			writer.WriteEndElement();

			writer.WriteStartElement(XmlElementName.Sev.ToString());
			writer.WriteString(SeverityName);
			writer.WriteEndElement();

			writer.WriteStartElement(XmlElementName.Data.ToString());
			writer.WriteString(Data.ToString());
			writer.WriteEndElement();

			writer.WriteStartElement(XmlElementName.Tag.ToString());
			writer.WriteString(TagName);
			writer.WriteEndElement();

			writer.WriteStartElement(XmlElementName.MsgNm.ToString());
			writer.WriteString(MessageName);
			writer.WriteEndElement();

			writer.WriteStartElement(XmlElementName.Msg.ToString());
			writer.WriteString(Message);
			writer.WriteEndElement();

			writer.WriteStartElement(XmlElementName.Stack.ToString());
			writer.WriteString(StackTrace);
			writer.WriteEndElement();

			writer.WriteEndElement();//end parent tag
			stringWriter.Flush();
			sb.Append(stringWriter.ToString());
			writer.Close();
			return sb.ToString();
		}
		/// <summary>
		/// Render the exception into a string
		/// </summary>
		/// <param name="ex">the exception to render</param>
		/// <returns>
		/// the string representation of the exception
		/// </returns>
		/// <remarks>
		/// <para>
		/// Renders the exception type, message, and stack trace. Any nested
		/// exceptions are also rendered.
		/// </para>
		/// </remarks>
		public string RenderException(Exception ex)
		{
            return ExceptionDump.ExceptionToString(ex);
		}
		/// <summary>
		/// Return a string array representation of this object.
		/// </summary>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(StringUtility.FormatString(Properties.Resources.EventTimeLabel));
			sb.Append(": ");
			sb.Append(m_TimeStamp);
			sb.Append("\n");

			sb.Append(StringUtility.FormatString(Properties.Resources.SeverityLabel));
			sb.Append(": ");
			sb.Append(SeverityName);
			sb.Append("\n");

			sb.Append(StringUtility.FormatString(Properties.Resources.TagLabel));
			sb.Append(": ");
			sb.Append(TagName);
			sb.Append("\n");

			sb.Append(StringUtility.FormatString(Properties.Resources.MessageLabel));
			sb.Append(": ");
			sb.Append(m_Message);
			sb.Append("\n");

            if (GetException != null)// TestTrack 36
            {
                sb.Append(StringUtility.FormatString(Properties.Resources.ExceptionHeading));
                sb.Append(": ");
                sb.Append(RenderException(GetException));
                sb.Append("\n");
            }

			return sb.ToString();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion
	}
}
