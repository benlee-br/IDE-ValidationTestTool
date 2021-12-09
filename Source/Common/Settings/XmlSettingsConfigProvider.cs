using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Globalization;
using BioRad.Common;
using BioRad.Common.Services;
using BioRad.Common.Xml;

namespace BioRad.Common.Settings
{
	#region Documentation Tags
	/// <summary>
	/// Reads configuration elements from an external XML file.
	/// </summary>
	/// <remarks>
	/// Configuration elements are contained within a SettingsConfig
	/// object defined by the XML file. Use this configuration provider when
	/// overriding default settings for a particular application.
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
	///			<item name="vssfile">$Workfile: XmlSettingsConfigProvider.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Settings/XmlSettingsConfigProvider.cs $</item>
	///			<item name="vssrevision">$Revision: 11 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Pwalse $</item>
	///			<item name="vssdate">$Date: 7/31/07 10:58p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class XmlSettingsConfigProvider : AbstractXmlConfigurationProvider, ISettingsConfigurationProvider
	{
		#region Member Data
        // Fix for Bug 5364
        /// <summary>The version for the settings.</summary>
		private string m_Version;
		#endregion

		#region Accessors
        // Fix for Bug 5364
		/// <summary>The version for the settings.</summary>
		public string Version
		{
			get { return m_Version; }
		}
		#endregion
		
        #region Constructors and Destructor
		/// <summary>
		/// Default constructor. Configuration elements filename must be set 
		/// using property before GetConfigurationElements is called.
		/// </summary>
		public XmlSettingsConfigProvider()
		{
		}

		/// <summary>
		/// Constructor to explicitly assign Filename. Setting level will be Factory.
		/// </summary>
		/// <param name="fileName">XML configuration file name.</param>
		public XmlSettingsConfigProvider(string fileName)
		{
			this.FileName = fileName;
		}

		/// <summary>
		/// Constructor to explicitly assign Filename and setting level.
		/// </summary>
		/// <param name="fileName">XML configuration file name.</param>
		/// <param name="level">setting level of configuration elements (factory, lab or user).</param>
		public XmlSettingsConfigProvider(string fileName, SettingLevels level)
		{
			this.FileName = fileName;
			this.SettingLevel = level;
		}
		#endregion

		#region Member Data
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
		#endregion

		#region Methods
		/// <summary>
		/// Gets business rule service configuration elements. 
		/// </summary>
		/// <returns>The configuration elements.</returns>
		public override ICollection GetConfigurationElements()
		{
			ArrayList configElements = new ArrayList();
			// Create deserializer for configuration object
			XmlSettingsConfigTypeMapping mappingInfo = new 
				XmlSettingsConfigTypeMapping();
			XmlToTypeSerializer serializer = 
				new XmlToTypeSerializer(mappingInfo.GetSettingsConfigMapping());
			string fileName;
			// Convert file name to valid path, ensuring factory level settings file exists
			fileName = this.GetValidPathForFile(this.SettingLevel == SettingLevels.Factory);
			if (!File.Exists(fileName))
			{
				// If user or lab setting configuration file does not exist, 
				// return empty configuration element set. 
				// This is not an error condition for the settings service.
				return configElements;
			}
			try
			{
				// Decrypt contents of the XML document.
				Stream decryptedStream = BioRad.Common.Utilities.FileCryptor.GetInstance.
					DecryptFileContentsToStream(fileName);
				if(decryptedStream != null)
				{
					SettingsConfig serviceConfig = (SettingsConfig)serializer.Deserialize
						(decryptedStream);

					configElements.AddRange(serviceConfig.SettingConfigs);
					decryptedStream.Close();
				}
			}
			catch (Exception ex)
			{
				throw new ServiceLoadException(this,
                    StringUtility.FormatString(Properties.Resources.ConfigLoadFailure_1, 
					new object[]{this.FileName}
					), ex);
			}
			return configElements;
		}

        // Fix for Bug 5364
		/// <summary>Gets the version for the settings from the specified file.</summary>
		/// <returns>The file name.</returns>
        public string GetVersion(string fileName)
        {
            Stream decryptedStream = null;
            XmlSettingsConfigTypeMapping mappingInfo = null;
            XmlToTypeSerializer serializer = null;

			try
			{
				// Create deserializer for configuration object
				mappingInfo = new XmlSettingsConfigTypeMapping();
				serializer = new XmlToTypeSerializer(mappingInfo.GetSettingsConfigMapping());

				// Decrypt contents of the XML document.
				decryptedStream = BioRad.Common.Utilities.FileCryptor.GetInstance.
					DecryptFileContentsToStream(fileName);
				if (decryptedStream != null)
				{
					SettingsConfig serviceConfig = (SettingsConfig)serializer.Deserialize
						(decryptedStream);

					// Fix for Bug 5364
					// set the version
					this.m_Version = serviceConfig.Version;
				}
			}
			catch 
			{ 
				//for any exception loading the user pref file, reset preferences to recover.
				this.m_Version = "0.00";
			}
            finally
            {
                if(decryptedStream != null)
                    decryptedStream.Close();
            }
            return this.m_Version;
        }
		#endregion
	}
}
