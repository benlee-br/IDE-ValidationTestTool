using System;
using System.Collections;
using BioRad.Common.ApplicationExceptions;
using BioRad.Common.Services;

namespace BioRad.Common.Settings
{
	#region Documentation Tags
	/// <summary>
	/// Interface for service providing application-wide settings.
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
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
	///			<item name="vssfile">$Workfile: ISettingsService.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Settings/ISettingsService.cs $</item>
	///			<item name="vssrevision">$Revision: 7 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ssarup $</item>
	///			<item name="vssdate">$Date: 7/12/07 4:13a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public interface ISettingsService : IService, IEnumerable, IDisposable, IValidatable	
	{
		#region Accessors
		/// <summary>
		/// Indexer to the settings collection keyed by enum. Returned object
		/// is guaranteed to be assignable to given type. Returned object is cloned
		/// if possible.
		/// </summary>
		object this[Enum key, Type type] { get; }

		/// <summary>
		/// Indexer to the settings collection keyed by enum. Returned object
		/// is cloned if possible.
		/// </summary>
		object this[Enum key] { get; }

		/// <summary>
		/// Indexer to settings collection at a given level. If no setting is defined at the given level,
		/// will return the default setting for that level. Set establishes a setting override
		/// at the indicated level. Set is not valid for Factory setting level, and will throw
		/// InvalidOperationException.
		/// </summary>
		object this[Enum key, SettingLevels level] { get; set;}

		/// <summary>
		/// Indexer to settings collection at a given level. If no setting is defined at the given level,
		/// will return the default setting for that level.
		/// Object guaranteed to be assignable to given type.
		/// </summary>
		object this[Enum key, SettingLevels level, Type type] { get; }
		#endregion

		#region Methods
		/// <summary>
		/// Clears setting override at given level.
		/// </summary>
		/// <remarks>Not valid for Factory settings, will throw InvalidOperationException.</remarks>
		/// <remarks>No exception is thrown if setting is not defined for given level.</remarks>
		/// <param name="key">Setting identifier</param>
		/// <param name="level">Level (Lab or User) of setting to be cleared</param>
		void Clear(Enum key, SettingLevels level);

		/// <summary>
		/// Clears all setting overrides at given level.
		/// </summary>
		/// <remarks>Not valid for Factory settings, will throw InvalidOperationException.</remarks>
		/// <param name="level">Level (Lab or User) of setting to be cleared</param>
		void Clear(SettingLevels level);

		/// <summary>
		/// Implement Close as for Dispose.
		/// </summary>
		void Close();

		/// <summary>
		/// Get setting for given level.
		/// </summary>
		/// <param name="key">Setting identifier</param>
		/// <param name="level">Setting level</param>
		/// <param name="isDefault">True if setting is a default value rather than an explicit
		/// setting override at this level.</param>
		/// <returns>setting value</returns>
		object GetSetting(Enum key, SettingLevels level, out bool isDefault);

		/// <summary>
		/// Gets the default value for the specified setting, and returns the setting
		/// level at which the default value is defined.
		/// </summary>
		/// <remarks>For factory settings, the default value is equivalent to the value itself.
		/// </remarks>
		/// <param name="key">Setting identifier</param>
		/// <param name="level">Setting level</param>
		/// <param name="defaultLevel">Level at which default value is currently defined 
		/// (either Lab or Factory).</param>
		/// <returns>Default value for setting. If the setting at the given level were cleared,
		/// this is the value that the setting would take on.</returns>
		object GetDefaultSetting(Enum key, SettingLevels level, out SettingLevels defaultLevel);

		/// <summary>
		/// Gets the default value for the specified setting, and returning the setting
		/// level at which the default value is defined.
		/// </summary>
		/// <remarks>For factory settings, the default value is equivalent to the value itself.
		/// </remarks>
		/// <param name="key">Setting identifier</param>
		/// <param name="level">Setting level</param>
		/// <returns>Default value for setting. If the setting at the given level were cleared,
		/// this is the value that the setting would take on.</returns>
		object GetDefaultSetting(Enum key, SettingLevels level);
		// Fix for Bug 5364
		/// <summary>Get the version for the settings file.</summary>
		/// <param name="level">The Settings level.</param>
		/// <returns>A string object.</returns>
		string GetVersion(SettingLevels level);
		/// <summary>
		/// Save the setting overrides for the given level to the given store.
		/// </summary>
		/// <param name="level">Setting level</param>
		/// <param name="fileName">path to stored file</param>
		void Save(SettingLevels level, string fileName);
		#endregion
	}

	/// <summary>
	/// Setting services support these levels of settings. Settings are applied in order
	/// to the exposed settings collection.
	/// </summary>
	/// <remarks>Not all derived settings services will support each level.</remarks>
	public enum SettingLevels: int
	{
		/// <summary>
		/// Factory settings as installed "out of the box"
		/// </summary>
		Factory = 0,
		/// <summary>
		/// Overrides to factory settings which affect all users of the system.
		/// </summary>
		Lab = 1,
		/// <summary>
		/// Overrides to factory and lab settings for a given user.
		/// </summary>
		User = 2
	}

	/// <summary>
	/// These settings are common to all settings services. They are used in default factory
	/// settings to control subsequent loading.
	/// </summary>
	public enum CommonFactorySettings
	{
		/// <summary>
		/// Set true to allow default factory settings to be overridden by registered
		/// configuration providers. For maximum security this should be false.
		/// </summary>
		AllowOverrideFactorySettings,
		/// <summary>
		/// Set true to allow default lab settings to be overridden by registered
		/// configuration providers. For maximum security this should be false.
		/// </summary>
		AllowOverrideLabSettings,
		/// <summary>
		/// Set true to allow default user settings to be overridden by registered
		/// configuration providers. For maximum security this should be false.
		/// </summary>
		AllowOverrideUserSettings
	}
}
