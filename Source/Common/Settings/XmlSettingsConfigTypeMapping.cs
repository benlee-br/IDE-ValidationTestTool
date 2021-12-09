using System;
using BioRad.Common.Xml;

namespace BioRad.Common.Settings
{
	#region Documentation Tags
	/// <summary>
	/// Methods support adding attribute mappings for service configuration
	/// elements.
	/// </summary>
	/// <remarks>
	/// The exposed methods are not static, so that this may be subclassed and the methods
	/// overridden.
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
	///			<item name="vssfile">$Workfile: XmlSettingsConfigTypeMapping.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Settings/XmlSettingsConfigTypeMapping.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ssarup $</item>
	///			<item name="vssdate">$Date: 7/12/07 4:13a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public partial class XmlSettingsConfigTypeMapping
	{
        #region Methods
		/// <summary>
		/// Provides mapping for SettingsConfig type.
		/// </summary>
		/// <returns>The type mapping.</returns>
		public virtual XmlToTypeMapping GetSettingsConfigMapping()
		{
			XmlToTypeMapping mapping = new XmlToTypeMapping(typeof(SettingsConfig),
				"settings-service");
			mapping.AddCollectionMapping("settings", "SettingConfigs", "setting", 
				typeof(SettingsConfig.SettingConfig));
			mapping.AddNestedTypeMapping(GetSettingConfigMapping());

			// Fix for Bug 5364
			mapping.AddElementMapping("version", "Version", typeof(string));
			return mapping;
		}

		/// <summary>
		/// Provides mapping for SettingsConfig.SettingConfig type.
		/// </summary>
		/// <returns>The type mapping.</returns>
		public virtual XmlToTypeMapping GetSettingConfigMapping()
		{
			XmlToTypeMapping mapping = new XmlToTypeMapping(typeof(SettingsConfig.SettingConfig),
				"setting");
			mapping.AddAttributeMapping("name", "SettingName", typeof(string));
			mapping.AddAttributeMapping("value", "SettingValue", typeof(string));
			mapping.AddAttributeMapping("type", "SettingType", typeof(string));
			return mapping;
		}
		#endregion
	}
}
