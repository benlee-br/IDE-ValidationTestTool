using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Diagnostics;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>
	/// Xml elements name tags for diagnostic logger file format.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Ralph Ansell</item>
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
	///			<item name="vssfile">$Workfile: LoggerXmlTags.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/LoggerXmlTags.cs $</item>
	///			<item name="vssrevision">$Revision: 15 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public enum XmlElementName : int
	{ 
		/// <summary>
		/// The one and only one root-level element.
		/// </summary>
		BioRadDiagnosticLogFile=0,
		/// <summary>
		///Log entry element tag.
		/// </summary>
		//LogEntry,
		Log,
		/// <summary>
		/// Assembly Name element tag.
		/// </summary>
		//AssemblyName,
		ANm,
		/// <summary>
		/// Severity element tag.
		/// </summary>
		//Severity,
		Sev,
		/// <summary>
		/// Tag element tag.
		/// </summary>
		Tag,
		/// <summary>
		/// Message element tag
		/// </summary>
		//Message,
		Msg,
		/// <summary>
		/// Message name element tag
		/// </summary>
		//MessageName,
		MsgNm,
		/// <summary>
		/// Time stamp element tag.
		/// </summary>
		//TimeStamp,
		TS,
		/// <summary>
		/// Stack trace element tag.
		/// </summary>
		//StackTrace,
		Stack,
		/// <summary>
		/// Name of logger.
		/// </summary>
		//LogName,
		LgNm,
		/// <summary>
		/// Diagnostic Level
		/// </summary>
		Level,
		/// <summary>
		/// long data value defined by caller.
		/// </summary>
		Data
	};
	#region Documentation Tags
	/// <summary>
	/// Xml helpers to create well formed element tags.
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
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
	///			<item name="vssfile">$Workfile: LoggerXmlTags.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/LoggerXmlTags.cs $</item>
	///			<item name="vssrevision">$Revision: 15 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	internal partial class LoggerXmlTags
	{
		#region Constants
		/// <summary>
		/// Tag types.
		/// </summary>
		private enum TagType
		{ 
			/// <summary>
			/// Start tag
			/// </summary>
			Start=0,
			/// <summary>
			/// End tag.
			/// </summary>
			End, 
		};
        #endregion

		#region Member Data
		/// <summary>
		/// Array containing the start and end element for a tag.
		/// </summary>
		private string[] m_StartEndElements = null;
		#endregion

		#region Accessors
		/// <summary>
		/// 
		/// </summary>
		public string StartTag
		{
			get
			{
				return m_StartEndElements[(int)TagType.Start];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string EndTag
		{
			get
			{
				return m_StartEndElements[(int)TagType.End];
			}
		}
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="tag"></param>
		public LoggerXmlTags(XmlElementName tag)
		{
			string str = tag.ToString();
			MakeElementTags(str);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Makes start and end element tags for argument.
		/// </summary>
		/// <param name="tag">Element name</param>
		private void MakeElementTags(string tag)
		{
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter writer = new XmlTextWriter(stringWriter);
			writer.WriteStartElement(tag);
			writer.WriteString(",");
			writer.WriteEndElement();
			stringWriter.Flush();
			string s = stringWriter.ToString();
			m_StartEndElements = s.Split(',');
			writer.Close();	
		}
		#endregion
	}
}
