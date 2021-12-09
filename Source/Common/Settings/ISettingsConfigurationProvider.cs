using System;
using BioRad.Common.Services;

namespace BioRad.Common.Settings
{
	#region Documentation Tags
	/// <summary>
	/// Configuration providers for settings services must implement this interface
	/// if their elements are to be loaded.
	/// </summary>
	/// <remarks>
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
	///			<item name="vssfile">$Workfile: ISettingsConfigurationProvider.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/Settings/ISettingsConfigurationProvider.cs $</item>
	///			<item name="vssrevision">$Revision: 1 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Lvs $</item>
	///			<item name="vssdate">$Date: 12/08/04 1:15p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public interface ISettingsConfigurationProvider : IConfigurationProvider
	{
		#region Accessors
		/// <summary>
		/// Determines the setting level configuration elements are provided for. This
		/// allows a hierarchical loading of settings.
		/// </summary>
		SettingLevels SettingLevel { get; }
        #endregion
	}
}
