using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using BioRad.Common;
using BioRad.Common.ApplicationExceptions;
using BioRad.Common.Services;
using BioRad.Common.Xml;

namespace BioRad.Common.Settings
{
	#region Documentation Tags
	/// <summary>
	/// Abstract base class for settings services. Exposes a settings collection
	/// which contains objects constructed using loaded configuration objects. Service
	/// is constructed with an initial collection of settings. Configured providers can
	/// be used to override these default settings.
	/// </summary>
	/// <remarks>
	/// Settings are accessed via an enum by converting the enum to a string of the form 
	/// enum-type-name!enum-string-value. Enums other than the Settings enum may be used,
	/// but the default Validate() method will not ensure they are configured.
	/// <para>
	/// Derived types should override Validate method and FactorySettings accessor.
	/// </para></remarks>
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
	///			<item name="vssfile">$Workfile: AbstractSettingsService.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Settings/AbstractSettingsService.cs $</item>
	///			<item name="vssrevision">$Revision: 25 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Pwalse $</item>
	///			<item name="vssdate">$Date: 7/23/07 4:40p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public abstract partial class AbstractSettingsService : AbstractService, ISettingsService
	{
		#region Contained Classes
		#region Documentation Tags
		/// <summary>
		/// Type allowing settings in the containing class to be accessed via
		/// enum. Can throw ArgumentException.
		/// </summary>
		/// <remarks>
		/// Although this allows derived types to expose Settings collection, as a contained
		/// type it can't be put into an interface without exposing private members of
		/// the SettingsService. Settings are exposed directly by service,
		/// it may not be useful to package collection as a contained collection.
		/// </remarks>
		#endregion
		protected class SettingsCollection
		{
			#region Member Data
			/// <summary>
			/// The allowed key type.
			/// </summary>
			private Type m_KeyType;
			/// <summary>
			/// The key join string used to create keys from enum type name and value.
			/// </summary>
			private string m_KeyJoinString;
			/// <summary>
			/// A collection of all configured settings, keyed by string value of enum.
			/// </summary>
			private Hashtable m_Settings = new Hashtable();
			#endregion

			#region Constructors and Destructor
			/// <summary>
			/// Constructor for the group collection class.
			/// </summary>
			/// <param name="keyType">Enum type of key used for the collection.</param>
			/// <param name="keyJoinString">string used to create key name by joining
			/// key type with key value.</param>
			public SettingsCollection(Type keyType, string keyJoinString)
			{
				m_KeyType = keyType;
				m_KeyJoinString = keyJoinString;
			}
			#endregion

			#region Accessors					
			/// <summary>
			/// Indexer to the settings collection in the containing object,
			/// keyed by enum. Can throw ArgumentException. Returns clone if possible.
			/// </summary>
			public object this[Enum key, Type type] 
			{
				get 
				{ 
					if (!m_Settings.ContainsKey(GetKey(key)))
					{
						// throw an error if setting is not found in service
                        throw new ArgumentException(StringUtility.FormatString(Properties.Resources.NoSetting_1,
                            GetKey(key)), "key");
					}
					object obj = m_Settings[GetKey(key)];
					if (type.IsAssignableFrom(obj.GetType()))
					{
						if (obj is ICloneable)
							return ((ICloneable) obj).Clone();
						else
							return obj;
					}
					else
					{
						// throw an error if setting is not of correct type
                        throw new InvalidCastException
                        (StringUtility.FormatString(Properties.Resources.InvalidSettingType_2,
                            GetKey(key), type));
					}
				}
				set
				{ 
					if (!m_Settings.ContainsKey(GetKey(key)))
					{
						// throw an error if setting is not found in service
                        throw new ArgumentException
                            (StringUtility.FormatString(Properties.Resources.NoSetting_1,
                            GetKey(key)), "key");
					}
					m_Settings[GetKey(key)] = value;
				}
			}

			/// <summary>
			/// Get the count of groups in the containing object.
			/// </summary>
			public int Count 
			{
				get 
				{ 
					return m_Settings.Count; 
				}
			}

			/// <summary>
			/// Gets setting names
			/// </summary>
			public ICollection Keys
			{
				get
				{
					return m_Settings.Keys;
				}
			}
			#endregion

			#region Methods
			/// <summary>
			/// Add a setting to collection with given enumeration key.
			/// </summary>
			/// <param name="key">key for the setting in the collection</param>
			/// <param name="setting">setting to add to collection (may be any type)</param>
			/// <param name="allowOverride">if true, duplicate settings are overriden</param>
			public void Add(Enum key, object setting, bool allowOverride)
			{
				this.Add (this.GetKey(key), setting, allowOverride);
			}

			/// <summary>
			/// Add a setting to collection with given string key.
			/// </summary>
			/// <param name="key">key for the given setting in the collection</param>
			/// <param name="setting">setting to add to collection (may be any type)</param>
			/// <param name="allowOverride">if true, duplicate settings are overriden</param>
			public void Add(string key, object setting, bool allowOverride)
			{
				if (allowOverride && m_Settings.ContainsKey(key))
				{
					m_Settings.Remove(key);
				}
				m_Settings.Add(key, setting);
			}

			/// <summary>
			/// Add a setting to collection with given enumeration key.
			/// </summary>
			/// <remarks>If setting is not of given type throws InvalidCastException.</remarks>
			/// <param name="key">key for the setting in the collection</param>
			/// <param name="setting">setting to add to collection (must be of given type).</param>
			/// <param name="type">Expected type of setting.</param>
			/// <param name="allowOverride">if true, duplicate settings are overriden</param>
			public void Add(Enum key, object setting, Type type, bool allowOverride)
			{
				this.Add (this.GetKey(key), setting, type, allowOverride);
			}

			/// <summary>
			/// Add a setting to collection with given string key.
			/// </summary>
			/// <remarks>If setting is not assignable from given type throws InvalidCastException.</remarks>
			/// <param name="key">key for the given setting in the collection</param>
			/// <param name="setting">Setting to add to collection.</param>
			/// <param name="type">Expected type of setting.</param>
			/// <param name="allowOverride">if true, duplicate settings are overriden</param>
			public void Add(string key, object setting, Type type, bool allowOverride)
			{
				if (type.IsAssignableFrom(setting.GetType()))
				{
					this.Add(key, setting, allowOverride);
				}
				else
				{
					// throw an error if setting is not of correct type
                    throw new InvalidCastException
                    (StringUtility.FormatString(Properties.Resources.InvalidSettingType_2,
                        GetKey(key), type));
				}
			}

			/// <summary>
			/// Dispose of all contained rules and clear the collection.
			/// </summary>
			internal void Clear()
			{
				foreach (object setting in this)
				{
					if (setting is IDisposable)
					{
						((IDisposable)setting).Dispose();
					}
				}
				m_Settings.Clear();
			}

			/// <summary>
			/// Query whether indicated setting is available from this
			/// service.
			/// </summary>
			/// <param name="key">setting identifier</param>
			/// <returns>true if setting is in settings collection</returns>
			public bool ContainsKey(Enum key) 
			{
				return m_Settings.ContainsKey(GetKey(key)); 
			}

			/// <summary>
			/// Enumeration support. Enumerates the key/value pairs in the settings
			/// collection.
			/// </summary>
			/// <returns>An enumerator for the settings collection.</returns>
			public IDictionaryEnumerator GetEnumerator()
			{
				return m_Settings.GetEnumerator();
			}

			/// <summary>
			/// Converts key to string accessor for collection.
			/// Accessor is enum-type-name_enum-string-name.
			/// </summary>
			/// <param name="key">setting enumeration</param>
			/// <returns>key string equivalent</returns>
			public string GetKey(Enum key)
			{
				return String.Format("{0}{1}{2}", key.GetType().Name, m_KeyJoinString, key.ToString());
			}

			/// <summary>
			/// Converts string to string accessor for collection.
			/// Accessor is enum-type-name!enum-string-name, where enum-type-name
			/// default is "Settings".
			/// </summary>
			/// <param name="name">setting name as string</param>
			/// <returns>key string equivalent</returns>
			public string GetKey(string name)
			{
				Enum key = null;
				if (name.IndexOf(m_KeyJoinString) < 0)
				{
					try
					{
						key = (Enum) Enum.Parse(m_KeyType, name, false);
					}
					catch
					{
						// Ignore errors; key will be name string.
					}
				}
				if (key == null)
					return name;
				else
					return GetKey(key);
			}

			/// <summary>
			/// Remove a setting with given enumeration key from collection.
			/// </summary>
			/// <remarks>No effect if key not in collection.</remarks>
			/// <param name="key">key for the setting in the collection</param>
			public void Remove(Enum key)
			{
				this.Remove (this.GetKey(key));
			}

			/// <summary>
			/// Remove a setting with given string key from collection.
			/// </summary>
			/// <remarks>No effect if key not in collection.</remarks>
			/// <param name="key">key for the given setting in the collection</param>
			public void Remove(string key)
			{
				if (m_Settings.ContainsKey(key))
				{
					m_Settings.Remove(key);
				}
			}
			#endregion
		}		
		#endregion

		#region Constants
		/// <summary>
		/// String used to join enum-type-name with enum-value as a key in
		/// contained collection.
		/// </summary>
		public readonly string KeyJoinString = "!";
		#endregion

		#region Member Data
		/// <summary>
		/// Indexed settings collection property - may be indexed by enum and type. 
		/// Returns null if no match found. Settings for each level are aggregated to this
		/// collection.
		/// </summary>
		protected readonly SettingsCollection m_Settings;
		/// <summary>
		/// Array of indexed settings collections, one per setting level (factory, lab, user).
		/// </summary>
		protected readonly SettingsCollection[] m_LevelSettings = new SettingsCollection[3];

        // Fix for Bug 5364
		private string m_Version;
		#endregion

		#region Accessors
		/// <summary>
		/// Indexer to the settings collection keyed by enum. Can throw ArgumentException if 
		/// setting not found.
		/// </summary>
		/// <remarks>Override to control interface implementation.</remarks>
		protected object this[Enum key] 
		{
			get { return this[key, typeof(object)]; }
		}

		/// <summary>
		/// Indexer to the settings collection keyed by enum. 
		/// Can throw ArgumentException if setting is not found or is not assignable to 
		/// given type. Returned object guaranteed to be assignable to given type.
		/// Override to process value obtained from settings.
		/// </summary>
		/// <remarks>Override to control interface implementation.</remarks>
		protected object this[Enum key, Type type]
		{
			get
			{
				return m_Settings[key, type];
			}
		}

		/// <summary>
		/// Indexer to the settings collection keyed by enum. Can throw ArgumentException if 
		/// setting not found.
		/// </summary>
		/// <remarks>Explicit interfact implementation. Override for type safety.</remarks>
		object ISettingsService.this[Enum key]
		{
			get 
			{ 
				// NOTE: Will not call derived type accessor - override if that is required
				if ((key.GetType() != this.KeyType) && !(key is CommonFactorySettings))
				{
                    throw new InvalidCastException
                        (StringUtility.FormatString(Properties.Resources.InvalidParameterType_2, "key", key.GetType()));
				}
				return this[key];
			}
		}

		/// <summary>
		/// Indexer to the settings collection keyed by enum. 
		/// Can throw ArgumentException if setting is not found or is not assignable to 
		/// given type. Returned object guaranteed to be assignable to given type.
		/// </summary>
		/// <remarks>Explicit interfact implementation. Override for type safety.</remarks>
		object ISettingsService.this[Enum key, Type type]
		{
			get
			{
				// NOTE: Will not call derived type accessor - override if that is required
				if ((key.GetType() != this.KeyType) && !(key is CommonFactorySettings))
				{
                    throw new InvalidCastException
                    (StringUtility.FormatString(Properties.Resources.InvalidParameterType_2, "key", key.GetType()));
				}
				return this[key, type];
			}
		}

		/// <summary>
		/// Indexer to settings collection at a given level. If no setting is defined at the given level,
		/// will return the default setting for that level. Set establishes a setting override
		/// at the indicated level. Set is not valid for Factory setting level, and will
		/// throw InvalidOperationException. 
		/// </summary>
		/// <remarks>Override to control interface implementation.</remarks>
		protected object this[Enum key, SettingLevels level] 
		{ 
			get
			{
				return this[key, level, typeof(object)];
			}
			set
			{
				this[key, level, typeof(object)] = value;
			}
		}

		/// <summary>
		/// Indexer to settings collection at a given level. If no setting is defined at the given level,
		/// will return the default setting for that level. Set establishes a setting override
		/// at the indicated level. Set is not valid for Factory setting level and will throw
		/// InvalidOperationException.
		/// </summary>
		/// <remarks>Explicit interfact implementation. Override for type safety.</remarks>
		object ISettingsService.this[Enum key, SettingLevels level] 
		{ 
			get
			{
				// NOTE: Will not call derived type accessor - override if that is required
				if ((key.GetType() != this.KeyType) && !(key is CommonFactorySettings))
				{
                    throw new InvalidCastException
                    (StringUtility.FormatString(Properties.Resources.InvalidParameterType_2, "key", key.GetType()));
				}
				return this[key, level];
			}
			set
			{
				// NOTE: Will not call derived type accessor - override if that is required
				if ((key.GetType() != this.KeyType) && !(key is CommonFactorySettings))
				{
                    throw new InvalidCastException
                    (StringUtility.FormatString(Properties.Resources.InvalidParameterType_2, "key", key.GetType()));
				}
				this[key, level] = value;
			}
		}
		
		/// <summary>
		/// Indexer to settings collection at a given level. If no setting is defined at the given level,
		/// will return the default setting for that level. Set establishes a setting override
		/// at the indicated level. Set is not valid for Factory setting level, and will throw
		/// InvalidOperationException.
		/// Returned object guaranteed to be assignable to given type.
		/// </summary>
		/// <remarks>Override to control interface implementation.</remarks>
		protected object this[Enum key, SettingLevels level, Type type] 
		{ 
			get
			{
				object setting = null;
				switch (level)
				{
					case SettingLevels.Factory:
						// Factory settings are always present
						setting = this.m_LevelSettings[(int) level][key, type];
						break;
					case SettingLevels.Lab:
						// Lab setting may override factory setting
						if (!this.m_LevelSettings[(int) level].ContainsKey(key))
						{
							// Return factory setting, no lab setting
							setting = this.m_LevelSettings[(int) SettingLevels.Factory][key, type];
						}
						else
						{
							// return lab setting
							setting = this.m_LevelSettings[(int) level][key, type];
						}
						break;
					case SettingLevels.User:
						// user setting is the same as the current setting
						setting = m_Settings[key, type];
						break;
				}
				return setting;
			}
			set
			{
				// Override setting at given level, not valid for factory settings
				if (level != SettingLevels.Factory)
				{
					// apply lab or user setting override
					this.m_LevelSettings[(int) level].Add(key, value, type, true);
					// Update current settings
					Update(key);
				}
				else
				{
					// throw an error if attempting to override a factory setting.
                    throw new InvalidOperationException
                    (StringUtility.FormatString(Properties.Resources.FactorySettingChangeNotAllowed_1,
                        key));
				}
			}
		}

		/// <summary>
		/// Indexer to settings collection at a given level. If no setting is defined at the given level,
		/// will return the default setting for that level.
		/// Object guaranteed to be assignable to given type.
		/// </summary>
		/// <remarks>Explicit interfact implementation. Override for type safety.</remarks>
		object ISettingsService.this[Enum key, SettingLevels level, Type type] 
		{ 
			get
			{
				// NOTE: Will not call derived type accessor - override if that is required
				if ((key.GetType() != this.KeyType) && !(key is CommonFactorySettings))
				{
                    throw new InvalidCastException
                    (StringUtility.FormatString(Properties.Resources.InvalidParameterType_2, "key", key.GetType()));
				}
				return this[key, level, type];
			}
		}

		/// <summary>
		/// Override to supply type of default enum key. Type is used to construct
		/// access string when accessing a setting using a string value rather
		/// than an enum type.
		/// </summary>
		/// <returns>type of enum key used to access settings in implemented service</returns>
		protected abstract Type KeyType {get; }

		/// <summary>
		/// Validates instance and returns result.
		/// </summary>
		public bool IsValid
		{
			get
			{
				bool valid = false;
				try
				{
					this.Validate();
					valid = true;
				}
				catch
				{
					// Ignore exceptions to return valid==false
				}
				return valid;
			}
		}

		/// <summary>
		/// Public access to the setting names
		/// </summary>
		public ICollection SettingNames
		{
			get
			{
				return m_Settings.Keys;
			}
		}
				
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Default constructor. Service configuration is not loaded at this point.
		/// Initializes indexer access to settings.
		/// </summary>
		public AbstractSettingsService()
		{
			m_Settings = new SettingsCollection(this.KeyType, this.KeyJoinString);
			for (int i = 0; i < m_LevelSettings.Length; i++)
			{
				m_LevelSettings[i] = new SettingsCollection(this.KeyType, this.KeyJoinString);
			}
		}

		/// <summary>
		/// Finalize method - for garbage collection.
		/// </summary>
		~AbstractSettingsService()
		{
			Dispose(false);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Determines whether configuration elements supplied by registered configuration
		/// providers are allowed for the given setting level.
		/// </summary>
		/// <remarks>Derived types may override explicitly allow or disallow registered 
		/// configuration providers. This implementation checks factory settings for
		/// CommonFactorySettings key.</remarks>
		/// <param name="level">factory, lab or user</param>
		/// <returns>true if factory setting exists and is true.</returns>
		protected virtual bool AllowOverride(SettingLevels level)
		{
			bool allow =  false;
			try
			{
				// get factory settings
				SettingsCollection settings = this.m_LevelSettings[(int)SettingLevels.Factory];
				switch (level)
				{
					case SettingLevels.Factory:
						allow = (bool) settings[CommonFactorySettings.AllowOverrideFactorySettings, typeof(bool)];
						break;
					case SettingLevels.Lab:
						allow = (bool) settings[CommonFactorySettings.AllowOverrideLabSettings, typeof(bool)];
						break;
					case SettingLevels.User:
						allow = (bool) settings[CommonFactorySettings.AllowOverrideUserSettings, typeof(bool)];
						break;
				}
			}
			catch
			{
				// Ignore exceptions if setting is not defined, defaulting to false
			}
			return allow;
		}

		/// <summary>
		/// Clears setting override at given level. Setting will
		/// revert to its default value (either Lab or Factory setting).
		/// </summary>
		/// <remarks>Not valid for Factory settings, will throw InvalidOperationException.
		/// No exception is thrown if setting is not defined for given level.
		/// <para>Override to control interface definition.</para></remarks>
		/// <param name="key">Setting identifier</param>
		/// <param name="level">Level (Lab or User) of setting to be cleared</param>
		protected virtual void Clear(Enum key, SettingLevels level)
		{
			if (!(level == SettingLevels.Factory))
			{
				this.m_LevelSettings[(int) level].Remove(key);
				Update(key);
			}
			else
			{
				// throw an error if attempting to override a factory setting.
                throw new InvalidOperationException
                (StringUtility.FormatString(Properties.Resources.FactorySettingChangeNotAllowed_1,
                    key));
			}
		}

		/// <summary>
		/// Clears setting override at given level. Setting will
		/// revert to its default value (either Lab or Factory setting).
		/// </summary>
		/// <remarks>Explicit interface definition. 
		/// <para>Not valid for Factory settings, will throw InvalidOperationException.
		/// No exception is thrown if setting is not defined for given level.</para></remarks>
		/// <param name="key">Setting identifier</param>
		/// <param name="level">Level (Lab or User) of setting to be cleared</param>
		void ISettingsService.Clear(Enum key, SettingLevels level)
		{
			// NOTE: Will not call derived type accessor - override if that is required
			if ((key.GetType() != this.KeyType) && !(key is CommonFactorySettings))
			{
                throw new InvalidCastException
                (StringUtility.FormatString(Properties.Resources.InvalidParameterType_2, "key", key.GetType()));
			}
			this.Clear(key, level);
		}

		/// <summary>
		/// Clears all setting overrides at given level.
		/// </summary>
		/// <remarks>Not valid for Factory settings, will throw InvalidOperationException.</remarks>
		/// <param name="level">Level (Lab or User) of setting to be cleared</param>
		public virtual void Clear(SettingLevels level)
		{
			if (!(level == SettingLevels.Factory))
			{
				this.m_LevelSettings[(int) level].Clear();
				Update();
			}
			else
			{
				// throw an error if attempting to override a factory setting.
                throw new InvalidOperationException
                (StringUtility.FormatString(Properties.Resources.FactorySettingChangeNotAllowed));
			}
		}


		/// <summary>
		/// Explicitly dispose contained rules.
		/// </summary>
		public void Close()
		{
			Dispose();
		}

		/// <summary>
		/// Test if configured settings collection contains an item identified by key.
		/// </summary>
		/// <param name="key">setting enumeration identifier</param>
		/// <returns></returns>
		public bool ContainsKey(Enum key) 
		{
			return m_Settings.ContainsKey(key); 
		}

		/// <summary>
		/// Explicitly dispose contained rules.
		/// </summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			Dispose(true);
		}

		/// <summary>
		/// Dispose contained settings when explicitly disposing.
		/// Override to clean up a derived type. Call base method if overriden.
		/// </summary>
		/// <param name="disposing">True if explictly disposing</param>
		protected virtual void Dispose(bool disposing)
		{
			lock (this)
			{
				// When explicitly disposing, clear settings collection.
				// This will dispose all contained settings.
				if (disposing)
				{
					if (m_Settings != null)
					{
						m_Settings.Clear();
					}
				}
			}
		}

		/// <summary>
		/// Enumeration support. Enumerates the key/value elements in the configured
		/// settings collection.
		/// </summary>
		/// <returns>An enumerator for the configured settings collection.</returns>
		public IEnumerator GetEnumerator()
		{
			return this.m_Settings.GetEnumerator();
		}

		/// <summary>
		/// Override gets a collection of configuration elements for each setting level.
		/// </summary>
		/// <returns>dictionary containing collections of configuration elements for each levels</returns>
		public override ICollection GetConfigurationElements()
		{
			Hashtable levels = new Hashtable();
			foreach (SettingLevels level in Enum.GetValues(typeof(SettingLevels)))
			{
				levels.Add(level, GetConfigurationElements(level));
			}
			return levels;
		}

		/// <summary>
		/// Override gets a collection of configuration elements for a given setting level.
		/// </summary>
		/// <param name="level">setting level (factory, lab, user)</param>
		/// <returns>collection of configuration elements for given level</returns>
		protected virtual ICollection GetConfigurationElements(SettingLevels level)
		{
			ArrayList elements = new ArrayList();
			foreach (ISettingsConfigurationProvider provider in m_ConfigurationProviders)
			{
				if (provider.SettingLevel == level)
				{
					elements.AddRange(provider.GetConfigurationElements());
				}
			}
			return elements;
		}

		/// <summary>
		/// Get default collection of configuration elements for a given setting level.
		/// </summary>
		/// <remarks>Override in derived type to add default settings.
		/// These settings can be supplanted by settings from registered configuration
		/// providers.</remarks>
		/// <param name="level">setting level (factory, lab, user)</param>
		/// <returns>null (empty set)</returns>
		protected virtual ICollection GetDefaultConfigurationElements(SettingLevels level)
		{
			return null;
		}

		/// <summary>
		/// Gets the default value for the specified setting, and returns the setting
		/// level at which the default value is defined.
		/// </summary>
		/// <remarks>For factory settings, the default value is equivalent to the value itself.
		/// <para>Override to control interface.</para></remarks>
		/// <param name="key">Setting identifier</param>
		/// <param name="level">Setting level</param>
		/// <param name="defaultLevel">Level at which default value is currently defined 
		/// (either Lab or Factory).</param>
		/// <returns>Default value for setting. If the setting at the given level were cleared,
		/// this is the value that the setting would take on.</returns>
		protected virtual object GetDefaultSetting(Enum key, SettingLevels level, out SettingLevels defaultLevel)
		{
			object setting = null;
			// Default for both factory and lab settings is factory setting
			defaultLevel = SettingLevels.Factory;
			// Factory settings are always present
			setting = this.m_LevelSettings[(int)SettingLevels.Factory][key, typeof(object)];
			if ((level == SettingLevels.User) && m_LevelSettings[(int)SettingLevels.Lab].ContainsKey(key))
			{
				// Return lab setting as default if it exists.
				// If not, default will be factory setting
				setting = this.m_LevelSettings[(int) SettingLevels.Lab][key, typeof(object)];
				defaultLevel = SettingLevels.Lab;
			}
			return setting;
		}

		/// <summary>
		/// Gets the default value for the specified setting.
		/// </summary>
		/// <remarks>For factory settings, the default value is equivalent to the value itself.
		/// <para>Override to control interface.</para></remarks>
		/// <param name="key">Setting identifier</param>
		/// <param name="level">Setting level</param>
		/// <returns>Default value for setting. If the setting at the given level were cleared,
		/// this is the value that the setting would take on.</returns>
		protected virtual object GetDefaultSetting(Enum key, SettingLevels level)
		{
			// default level is unused
			SettingLevels defaultLevel;
			return this.GetDefaultSetting(key, level, out defaultLevel);
		}

		/// <summary>
		/// Get setting for given level.
		/// </summary>
		/// <remarks>Override to control interface.</remarks>
		/// <param name="key">Setting identifier</param>
		/// <param name="level">Setting level</param>
		/// <param name="isDefault">True if setting is a default value rather than an explicit
		/// setting override at this level.</param>
		/// <returns>setting value</returns>
		protected virtual object GetSetting(Enum key, SettingLevels level, out bool isDefault)
		{
			object setting = this[key, level];
			isDefault = !this.m_LevelSettings[(int) level].ContainsKey(key);
			return setting;
		}

		/// <summary>
		/// Gets the default value for the specified setting, and returns the setting
		/// level at which the default value is defined.
		/// </summary>
		/// <remarks>For factory settings, the default value is equivalent to the value itself.
		/// <para>Explicit interface definition. Override for type safety.</para>
		/// </remarks>
		/// <param name="key">Setting identifier</param>
		/// <param name="level">Setting level</param>
		/// <param name="defaultLevel">Level at which default value is currently defined 
		/// (either Lab or Factory).</param>
		/// <returns>Default value for setting. If the setting at the given level were cleared,
		/// this is the value that the setting would take on.</returns>
		object ISettingsService.GetDefaultSetting(Enum key, SettingLevels level, out SettingLevels defaultLevel)
		{
			// NOTE: Will not call derived type accessor - override if that is required
			if ((key.GetType() != this.KeyType) && !(key is CommonFactorySettings))
			{
                throw new InvalidCastException
                (StringUtility.FormatString(Properties.Resources.InvalidParameterType_2, "key", key.GetType()));
			}
			return this.GetDefaultSetting(key, level, out defaultLevel);
		}

		/// <summary>
		/// Gets the default value for the specified setting, and returning the setting
		/// level at which the default value is defined.
		/// </summary>
		/// <remarks>For factory settings, the default value is equivalent to the value itself.
		/// <para>Explicit interface definition. Override for type safety.</para> </remarks>
		/// <param name="key">Setting identifier</param>
		/// <param name="level">Setting level</param>
		/// <returns>Default value for setting. If the setting at the given level were cleared,
		/// this is the value that the setting would take on.</returns>
		object ISettingsService.GetDefaultSetting(Enum key, SettingLevels level)
		{
			// NOTE: Will not call derived type accessor - override if that is required
			if ((key.GetType() != this.KeyType) && !(key is CommonFactorySettings))
			{
                throw new InvalidCastException
                (StringUtility.FormatString(Properties.Resources.InvalidParameterType_2, "key", key.GetType()));
			}
			return this.GetDefaultSetting(key, level);
		}

		// Fix for Bug 5364
		/// <summary>Get the version for the settings file.</summary>
		/// <param name="level">The Settings level.</param>
		/// <returns>A string object.</returns>
		public abstract string GetVersion(SettingLevels level);
				
		/// <summary>
		/// Get setting for given level.
		/// </summary>
		/// <remarks>Explicit interface definition. Override for type safety</remarks>
		/// <param name="key">Setting identifier</param>
		/// <param name="level">Setting level</param>
		/// <param name="isDefault">True if setting is a default value rather than an explicit
		/// setting override at this level.</param>
		/// <returns>setting value</returns>
		object ISettingsService.GetSetting(Enum key, SettingLevels level, out bool isDefault)
		{
			// NOTE: Will not call derived type accessor - override if that is required
			if ((key.GetType() != this.KeyType) && !(key is CommonFactorySettings))
			{
                throw new InvalidCastException
                    (StringUtility.FormatString(Properties.Resources.InvalidParameterType_2, "key", key.GetType()));
			}
			return this.GetSetting(key, level, out isDefault);
		}

		/// <summary>
		/// Derived services can use this method to convert a defined Enum
		/// to an identifier. Left public for testing.
		/// </summary>
		/// <param name="key">Enum setting selector</param>
		/// <returns>String value the service will use to access the setting.</returns>
		public string GetNameFromKey(Enum key)
		{
			return this.m_Settings.GetKey(key);
		}

		/// <summary>
		/// Derived services can use this method to convert a given string
		/// to an identifier. 
		/// </summary>
		/// <param name="name">setting selector</param>
		/// <returns>String value the service will use to access the setting.</returns>
		protected string GetNameFromString(string name)
		{
			return this.m_Settings.GetKey(name);
		}

		/// <summary>
		/// Load service. Clears settings collection, then loads factory, lab and user
		/// settings. 
		/// </summary>
		/// <param name="configurationCollection">Dictionary keyed by heading level containing
		/// collections of SettingsConfig.SettingConfig objects.</param>
		public override void Load(ICollection configurationCollection)
		{
            // Fix for Bug 5364
			this.m_Version = GetVersion(SettingLevels.Factory);

			m_Settings.Clear();
			IDictionary dictionary = configurationCollection as IDictionary;
			Load(dictionary, SettingLevels.Factory);
			Load(dictionary, SettingLevels.Lab);
			Load(dictionary, SettingLevels.User);
		}

		/// <summary>
		/// Load settings for a given level. First loads default settings for the level
		/// (provided by derived type), then adds configured settings from the configuration
		/// collection for that level. Each level is saved individually as well as aggregated
		/// to settings collection.
		/// Attempts to instantiate all settings, but will afterwards throw 
		/// ServiceLoadException if a setting fails to instantiate.
		/// </summary>
		/// <remarks>Will throw InvalidOperationException if configuration elements are
		/// provided but only default configuration is allowed for the given level. This
		/// can provide security for sensitive settings.</remarks>
		/// <param name="configurationCollection">Dictionary keyed by heading level containing
		/// collections of SettingsConfig.SettingConfig objects.</param>
		/// <param name="level">setting level to load.</param>
		protected void Load(IDictionary configurationCollection, SettingLevels level)
		{
			// Pre-load default settings for this level.
			Load(level, this.GetDefaultConfigurationElements(level), false);
			// Add configured settings, which may override default settings
			if ((configurationCollection != null) && configurationCollection.Contains(level))
			{
				ICollection configurationElements = (ICollection) configurationCollection[level];
				if ((configurationElements != null) && (configurationElements.Count > 0))
				{
					// Throw exception if configuration override is disallowed for this level
					if (!this.AllowOverride(level))
					{
						// TODO: Localize enum
                        throw new InvalidOperationException
                        (StringUtility.FormatString(Properties.Resources.DefaultConfigurationOnly_2,
                            this.GetType().Name, level));
					}
					Load(level, configurationElements , true);
				}
			}
		}

		/// <summary>
		/// Instantiate settings from configuration elements and add them to the settings
		/// collection for the indicated level, as well as to the
		/// service's Settings collection. May throw a ServiceLoad exception.
		/// </summary>
		/// <param name="level">factory, lab or user settings collection is to be loaded</param>
		/// <param name="configurationElements">collection of SettingsConfig.SettingConfig objects</param>
		/// <param name="allowOverride">If true, setting may override an already configured setting for this level.</param>
		protected virtual void Load(SettingLevels level, ICollection configurationElements, bool allowOverride)
		{
			if (configurationElements == null) return;
			ServiceLoadException ex = null;
			// Create setting using configuration elements, and add
			// to settings collection
			foreach (SettingsConfig.SettingConfig config in configurationElements)
			{
				object setting;
				try
				{
					// Create setting from configuration
					setting = config.GetSetting();
					// Add the setting to the collection maintained for given level
					m_LevelSettings[(int)level].Add(this.GetNameFromString(config.SettingName), setting, allowOverride);
					// Add the setting to the service to be accessed by identifier
					m_Settings.Add(this.GetNameFromString(config.SettingName), setting, true);
				}
				catch( Exception e)
				{
					// Save exception to be re-thrown after all settings are
					// loaded
					ex = new ServiceLoadException(this,
                        StringUtility.FormatString(Properties.Resources.ServiceLoadFailure_2,
						this.GetType().Name, config.SettingName), e);
				}
			}
			if (ex != null)
			{
				throw ex;
			}
		}
		/// <summary>
		/// Save the setting overrides for the given level to the given store.
		/// </summary>
		/// <remarks>May throw IO related exceptions.</remarks>
		/// <param name="level">Setting level</param>
		/// <param name="fileName">Full path with file name.</param>
		public virtual void Save(SettingLevels level, string fileName)
		{
			ArrayList configElements = new ArrayList();
			SettingsConfig.SettingConfig config;
			// Build a list of configuration elements for the settings at this level

			// Make sure the ToString() methods called on all our preferences use the
			// invariant culture...
			System.Globalization.CultureInfo currentCulture = 
				System.Threading.Thread.CurrentThread.CurrentCulture;
			try
			{
				System.Threading.Thread.CurrentThread.CurrentCulture = new 
					System.Globalization.CultureInfo( "" );
				foreach (DictionaryEntry de in this.m_LevelSettings[(int) level])
				{
					// Have to special case TemperatureType here, because changing TemperatureType's
					//  ToString() method could have unforseeable consequences on the rest of the
					//  codebase.  TemperatureType.ToString() does not serialize the whole object,
					//  but this overload does.  Would like to change ToString() to behave correctly
					//  instead.
					string valueString;
					if( de.Value.GetType() == typeof( BioRad.Common.TemperatureType ) )
						valueString = ((TemperatureType)(de.Value)).ToString( 
							CultureInfo.InvariantCulture);
					else
						valueString = de.Value.ToString();
					config = 
						new SettingsConfig.SettingConfig(
						de.Key.ToString(), 
						valueString, 
						string.Concat(de.Value.GetType().FullName, ",", de.Value.GetType().Assembly.GetName().Name)
						);
					configElements.Add(config);
				}
			}
			catch(Exception e)
			{
				System.Diagnostics.Debug.Assert( false );
				System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
				throw e;
			}
			System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;

			SettingsConfig serviceConfig = new SettingsConfig();

            // Fix for Bug 5364
			serviceConfig.Version = this.m_Version;
			serviceConfig.SettingConfigs = 
				(SettingsConfig.SettingConfig[])
				configElements.ToArray(typeof(SettingsConfig.SettingConfig));

			// Create deserializer for configuration object
			XmlSettingsConfigTypeMapping mappingInfo = new 
				XmlSettingsConfigTypeMapping();
			XmlToTypeSerializer serializer = 
				new XmlToTypeSerializer(mappingInfo.GetSettingsConfigMapping());

			bool encrypt = true; // set to false for debugging
			if( encrypt )
				serializer.SerializeEncrypted( serviceConfig, fileName );
			else
				serializer.Serialize( serviceConfig, fileName );
		}

		/// <summary>
		/// Update all current settings from Factory, Lab and User settings.
		/// </summary>
		protected virtual void Update()
		{
			// Clear settings, then copy in factory settings
			m_Settings.Clear();
			foreach (DictionaryEntry de in this.m_LevelSettings[(int)SettingLevels.Factory])
			{
				m_Settings.Add((string)de.Key, de.Value, false);
			}
			// Apply lab setting overrides
			foreach (DictionaryEntry de in this.m_LevelSettings[(int)SettingLevels.Lab])
			{
				m_Settings.Add((string) de.Key, de.Value, true);
			}
			// Apply user setting overrides
			foreach (DictionaryEntry de in this.m_LevelSettings[(int)SettingLevels.User])
			{
				m_Settings.Add((string)de.Key, de.Value, true);
			}
		}

		/// <summary>
		/// Update all current settings from Factory, Lab and User settings.
		/// </summary>
		protected virtual void RestoreFactorySettingsOnly()
		{
			// Clear settings, then copy in factory settings
			m_Settings.Clear();
			foreach (DictionaryEntry de in this.m_LevelSettings[(int)SettingLevels.Factory])
			{
				m_Settings.Add((string)de.Key, de.Value, false);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		protected virtual void ApplyLabSettingsOnly()
		{
			// Apply lab setting overrides
			foreach (DictionaryEntry de in this.m_LevelSettings[(int)SettingLevels.Lab])
			{
				m_Settings.Add((string) de.Key, de.Value, true);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		protected virtual void ApplyUserSettingsOnly()
		{
			// Apply user setting overrides
			foreach (DictionaryEntry de in this.m_LevelSettings[(int)SettingLevels.User])
			{
				m_Settings.Add((string)de.Key, de.Value, true);
			}
		}

		/// <summary>
		/// Update given setting from Factory, Lab and User values for that setting.
		/// </summary>
		/// <param name="key"></param>
		protected virtual void Update(Enum key)
		{
			// Restore factory setting for key
			m_Settings.Add(key, m_LevelSettings[(int) SettingLevels.Factory][key, typeof(object)], true);
			// Override factory setting with lab setting, if any
			if (m_LevelSettings[(int) SettingLevels.Lab].ContainsKey(key))
			{
				m_Settings.Add(key, m_LevelSettings[(int) SettingLevels.Lab][key, typeof(object)], true);
			}
			// Apply user setting override, if any
			if (m_LevelSettings[(int) SettingLevels.User].ContainsKey(key))
			{
				m_Settings.Add(key, m_LevelSettings[(int) SettingLevels.User][key, typeof(object)], true);
			}
		}

		/// <summary>
		/// Validate that all settings defined in the CommonFactorySettings Enum are configured.
		/// Override to validate derived service by calling Validate(Enum). Override should
		/// call base Validate().
		/// </summary>
		public virtual void Validate()
		{
			Validate(typeof(CommonFactorySettings));
		}

		/// <summary>
		/// Validate that all settings defined by the given Enum are configured.
		/// May throw ApplicationException on failure (TODO: ValidationException).
		/// </summary>
		/// <param name="keyType">Enum type providing setting names</param>
		protected void Validate(Type keyType)
		{
			object setting;
			foreach (string str in Enum.GetNames(keyType))
			{
				try
				{
					setting = 
						((ISettingsService)this)[(Enum) Enum.Parse(keyType, str, false)];
				}
				catch (Exception ex)
				{
					// TODO: Create Validation exception?
                    string s = StringUtility.FormatString(Properties.Resources.ValidationFailed_1, 
						new object [] {String.Format("{0} {1}",this.GetType().Name, str)});
					throw new ApplicationException(s, ex);
				}
			}
		}
		#endregion
	}
}
