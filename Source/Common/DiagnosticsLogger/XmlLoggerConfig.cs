using System;
using System.Collections;
using System.IO;
using System.Xml;
using BioRad.Common.Services;
using BioRad.Common.Utilities;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>
	/// Configuration provider for the DeviceManagerService
	/// that uses an XML file to supply devices.
	/// </summary>
	/// <remarks>
	/// Reads XML config file to load the list of logs.
	/// XML File Name: DiagnosticsLoggerConfiguration.xml.	/// </remarks>
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
	///			<item name="vssfile">$Workfile: XmlLoggerConfig.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/XmlLoggerConfig.cs $</item>
	///			<item name="vssrevision">$Revision: 18 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Staran $</item>
	///			<item name="vssdate">$Date: 7/02/07 5:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public partial class XmlLoggerConfig : IConfigurationProvider
	{
		#region Constants
		#endregion

		#region Member Data
		/// <summary>
		/// Stores Xml configuration file name for the devices.
		/// </summary>
		private string m_FileName;
		#endregion

		#region Accessors
		/// <summary>
		/// Gets/Sets Xml configuration file name for the Diagnostics Logger.
		/// </summary>
		public string FileName
		{
			get
			{
                return ApplicationPath.GetFullPath(m_FileName);
			}
			set{m_FileName = value;}
		}
		#endregion

		#region Delegates and Events
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Initializes a new instance of the XmlLoggerConfig class.
		/// </summary>
		public XmlLoggerConfig()
		{
		}
		#endregion

		#region Methods
		/// <summary>
		/// Gets configuration elements from the expected configuration file.
		/// </summary>
		/// <returns>Collection of devices configured from config file.</returns>
		public ICollection GetConfigurationElements()
		{
			ArrayList logs = new ArrayList();
			XmlReader reader = null;
			XmlDocument document = null;
			try
			{
				string fileName = this.FileName;
				if ( File.Exists(fileName) )//this configuration file is optional.
				{
					using (Stream fs = 
							   FileCryptor.GetInstance.DecryptFileContentsToStream(fileName))
					{
						// Ralph 11.16.2005
						reader = new XmlTextReader(fs, XmlNodeType.Element, 
							new XmlParserContext(null, null, string.Empty, XmlSpace.None));
						//reader = new XmlTextReader(fs);
						//end
						document = new XmlDocument();
						document.Load(reader);
						XmlNode docElement = document.DocumentElement;
						XmlNodeList elements = docElement.SelectNodes("/logs");
						foreach (XmlNode node in elements)
						{
							ConfiguredLog item = new ConfiguredLog();
							item.LevelAllLogs = DiagnosticLevel.ALL;
							if ( node.Attributes["level"] != null )
							{
								item.LevelAllLogs = (DiagnosticLevel)Enum.Parse(typeof(DiagnosticLevel), node.Attributes["level"].Value);
							}

							item.Override = "no";//todo: localize.
							if ( node.Attributes["override"] != null )
							{
								item.Override = node.Attributes["override"].Value;
							}
							logs.Add(item);
						}

						elements = docElement.SelectNodes("/logs/log");
						foreach (XmlNode node in elements)
						{
							ConfiguredLog item = new ConfiguredLog();

							// Catch missing attribute names in configuration file.
							// Note: If name of the attribute doesn't exiists null returned.
							item.Name = "";
							if ( node.Attributes["name"] != null )
							{
								item.Name = node.Attributes["name"].Value;
							}
					
							item.DisplayName = "<unknown>";//todo: localize.
							if ( node.Attributes["displayname"] != null )
							{
								item.DisplayName = node.Attributes["displayname"].Value;
							}
				
							item.File = "";
							if ( node.Attributes["file"] != null )
							{
								item.File = node.Attributes["file"].Value;
							}
				
							item.Level = DiagnosticLevel.ALL;
							if ( node.Attributes["level"] != null )
							{
								item.Level = (DiagnosticLevel)Enum.Parse(typeof(DiagnosticLevel), node.Attributes["level"].Value);
							}

							logs.Add(item);
						}
					}
				}
			}
			catch (Exception e)
			{	//todo: localize.
				//Fix for Bug 3331
				DiagnosticsLogService.GetService.GetDiagnosticsLog
					(WellKnownLogName.DiagLoggerConfig).Exception("Diagnostic configuration file.", e);

				//DiagnosticsLogService.DefaultLog.Exception("Diagnostic configuration file.", e);//log the exception.
				throw;
			}
			finally
			{
				// always make sure file is closed
				if(reader != null)
				{
					reader.Close();
				}
				document = null;
			}
			return logs;
		}
		
		#endregion

		#region Event Handlers
		#endregion
	}
}
