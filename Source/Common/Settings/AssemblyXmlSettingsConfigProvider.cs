using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using BioRad.Common;
using BioRad.Common.Services;
using BioRad.Common.Services.Config;
using BioRad.Common.Xml;



namespace BioRad.Common.Settings
{
	#region Documentation Tags
	/// <summary>
	/// Reads configuration elements from an XML file embedded as a resource segment
	/// within the configured assembly.
	/// </summary>
	/// <remarks>
	/// Configuration elements are contained within a SettingsConfig
	/// object defined by the XML file.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Lisa von Schlegell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="conformancereview">Conformance review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">1595</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: AssemblyXmlSettingsConfigProvider.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Settings/AssemblyXmlSettingsConfigProvider.cs $</item>
	///			<item name="vssrevision">$Revision: 9 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ssarup $</item>
	///			<item name="vssdate">$Date: 7/12/07 4:13a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class AssemblyXmlSettingsConfigProvider : AbstractXmlConfigurationProvider, ISettingsConfigurationProvider
	{
		#region Member Data
		private Assembly m_ResourceAssembly;
		private SettingLevels m_Level = SettingLevels.Factory;
		#endregion

		#region Accessors
		#region ISettingsConfigurationProvider Members
		/// <summary>
		/// Get/Set the setting level for configuration elements supplied by this
		/// configuration provider. Default is Factory.
		/// </summary>
		public SettingLevels SettingLevel
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
		#endregion

		/// <summary>
		/// Identifies assembly containing the resource. Set by constructor. 
		/// </summary>
		public Assembly ResourceAssembly  { get { return m_ResourceAssembly;}}
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Parameterized constructor explicitly sets name of embedded resource file.
		/// Resource assembly set to current assembly (will be assembly of
		/// derived type when constructor is called by a derived type constructor).
		/// Setting level will be "factory".
		/// </summary>
		/// <param name="fileName">name of embedded file containing configuration resources</param>
		public AssemblyXmlSettingsConfigProvider(string fileName)
		{
			this.FileName = fileName;
			m_ResourceAssembly = this.GetType().Assembly;

		}

		/// <summary>
		/// Parameterized constructor explicitly sets resource assembly and name of
		/// embedded resource file. Setting level will be "factory".
		/// </summary>
		/// <param name="assembly">assembly containing configuration resources</param>
		/// <param name="fileName">name of embedded file containing configuration resources</param>
		public AssemblyXmlSettingsConfigProvider(Assembly assembly, string fileName)
		{
			this.FileName = fileName;
			m_ResourceAssembly = assembly;
		}

		/// <summary>
		/// Parameterized constructor explicitly sets resource assembly, name of
		/// embedded resource file, and setting level of configuration elements
		/// (factory, lab or user).
		/// </summary>
		/// <param name="assembly">assembly containing configuration resources</param>
		/// <param name="fileName">name of embedded file containing configuration resources</param>
		/// <param name="level">setting level of configuration elements (factory, lab or user).</param>
		public AssemblyXmlSettingsConfigProvider(Assembly assembly, string fileName, SettingLevels level)
		{
			this.FileName = fileName;
			m_ResourceAssembly = assembly;
			this.SettingLevel = level;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Read a configuration object from an XML file embedded as a resource 
		/// segment within the current assembly, and return configuration elements 
		/// held in this object.
		/// </summary>
		/// <returns>Configuration elements</returns>
		public override System.Collections.ICollection GetConfigurationElements()
		{
			ArrayList configElements = new ArrayList();
			try
			{
				XmlReader reader = null;
				XmlDocument document = null;
				// Open embedded XML file as a stream
				using (Stream stream = this.ResourceAssembly.GetManifestResourceStream(this.FileName))
				{
					try
					{
						reader = new XmlTextReader(stream);
						document = new XmlDocument();
						document.Load(reader);
						XmlNode docElement = document.DocumentElement;
						XmlNodeList elements = docElement.SelectNodes("/settings-service/settings/setting");
						foreach (XmlNode node in elements)
						{
							SettingsConfig.SettingConfig  settingConfig = 
								new SettingsConfig.SettingConfig();
							settingConfig.SettingName = 
								node.Attributes["name"] != null ? node.Attributes["name"].Value : null;
							settingConfig.SettingType = 
								node.Attributes["type"] != null ? node.Attributes["type"].Value : null;
							settingConfig.SettingValue = 
								node.Attributes["value"] != null ? node.Attributes["value"].Value : null;

							//Add configured rendererConfig
							configElements.Add(settingConfig);
						}
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
				}
			}
			catch(Exception ex)
			{
				throw new ServiceLoadException(this,
                    StringUtility.FormatString(Properties.Resources.ConfigLoadFailure_1, 
					new object[]{this.FileName}
					), ex);
			}
			return configElements;
		}

		/// <summary>Gets the version for the settings file.</summary>
		/// <returns>A string object</returns>
		public string GetVersion()
		{
			string version = string.Empty;

			XmlReader reader = null;
			XmlDocument document = null;

			// Fix for Bug 5364 - retrieve the version from the settings file
			try
			{
				// Open embedded XML file as a stream
				using (Stream stream = this.ResourceAssembly.GetManifestResourceStream(this.FileName))
				{
					reader = new XmlTextReader(stream);
					document = new XmlDocument();
					document.Load(reader);
					XmlNode docElement = document.DocumentElement;
					XmlNode versionNode = 
						docElement.SelectSingleNode("/settings-service/version");
					if (versionNode != null)
					{
						version = versionNode.InnerText;
					}
				}
			}
			catch (Exception ex)
			{
				throw new ServiceLoadException(this,
					StringUtility.FormatString(Properties.Resources.SettingsFileVersionNotFound_1,
					this.FileName), ex);
			}
			finally
			{
				// always make sure file is closed
				if (reader != null)
				{
					reader.Close();
				}
				document = null;
			}
			return version;
		}
	
		#endregion

	}
}
