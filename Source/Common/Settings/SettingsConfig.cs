using System;
using System.ComponentModel;
using System.Globalization;

namespace BioRad.Common.Settings
{
	#region Documentation Tags
	/// <summary>
	/// Type defining configuration objects used to configure the Settings service.
	/// Settings are typically simple data objects.
	/// </summary>
	/// <remarks>
	/// Setting type GetSetting method uses reflection to instantiate a configured setting.
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
	///			<item name="vssfile">$Workfile: SettingsConfig.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Settings/SettingsConfig.cs $</item>
	///			<item name="vssrevision">$Revision: 3 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ssarup $</item>
	///			<item name="vssdate">$Date: 7/12/07 4:13a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class SettingsConfig
	{
		/// <summary>
		/// Setting configuration object. Supplies information allowing the 
		/// service to construct an setting provided by the service.
		/// </summary>
		[Serializable] public class  SettingConfig
		{
			#region Member Data
			/// <summary>
			/// Tag name of the constructed object in the service.
			/// </summary>
			private string m_SettingName;
			/// <summary>
			/// Name of type to be constructed.
			/// </summary>
			private string m_SettingType;
			/// <summary>
			/// Value of the object to be constructed.
			/// </summary>
			private string m_SettingValue;
			#endregion

			#region Accessors
			/// <summary>
			/// Name used to access setting from service.
			/// </summary>
			public string SettingName
			{
				get{return this.m_SettingName;}
				set{this.m_SettingName = value;}
			}
			/// <summary>
			/// Type of setting -  if type is a system type or defined in the current
			/// assembly, type name can be unqualified. Otherwise, it must be qualified
			/// by defining assembly name.
			/// </summary>
			public string SettingType
			{
				get{return this.m_SettingType;}
				set{this.m_SettingType = value;}
			}
			/// <summary>
			/// Value of setting (setting type must be type-convertible from this string)
			/// </summary>
			public string SettingValue
			{
				get{return this.m_SettingValue;}
				set{this.m_SettingValue = value;}
			}
			#endregion

			#region Constructors and Destructor
			/// <summary>
			/// Default no-argument constructor, used by configuration provider.
			/// </summary>
			public SettingConfig()
			{
			}

			/// <summary>
			/// Constructor used to explicitly create a configuration element.
			/// </summary>
			/// <param name="settingName">Object will accessed by this name.</param>
			/// <param name="settingValue">Object value.</param>
			/// <param name="settingType">name of type of created object object creation.</param>
			public SettingConfig(string settingName, string settingValue, string settingType)
			{
				this.SettingName = settingName;
				this.SettingType= settingType;
				this.SettingValue = settingValue;
			}
			#endregion

			#region Methods
			/// <summary>
			/// Instantiate and initialize the setting defined by this object using
			/// invariant culture.
			/// May throw exception if setting cannot be instantiated or initialized.
			/// </summary>
			/// <returns>a setting (value object)</returns>
			public object GetSetting()
			{ 
				// Instantiate the setting
				object setting = null;
				// Type convert parameter value before assignment
				// Use explictly-defined type if its provided. This must
				// be a system type.
				Type settingType = this.GetSettingType();
				// Look for a type converter associated with the setting type
				// TODO: It would be more efficient to use IConvertible - should that
				// be first default?
				TypeConverter typeConverter = TypeDescriptor.GetConverter(settingType);
				if (typeConverter != null)
				{
					try
					{
						setting = typeConverter.ConvertFromInvariantString(this.SettingValue);
					}
					catch
					{
						// Ignore conversion errors from type converter, fall through
						// to using IConvertable interface.
					}
				}
				if (setting == null)
				{
					// Conversion may throw exception
					setting = Convert.ChangeType(this.SettingValue, settingType, CultureInfo.InvariantCulture);
				}
				return setting;
			}

			/// <summary>
			/// Using SettingType, determines the type of the configured setting.
			/// Case-sensitive, may throw exception if type not found.
			/// </summary>
			/// <returns>Type of setting (default is string)</returns>
			public Type GetSettingType()
			{
				// default setting type is string
				if ((this.SettingType == null) || (this.SettingType == String.Empty))
					return (typeof(string));
				else
				{
					return Type.GetType(this.SettingType, true, false);
				}
			}
			#endregion
		}

		#region	Member Data
		/// <summary>
		/// Array of service configuration elements.
		/// </summary>
		private SettingConfig[] m_SettingConfigs;
		// Fix for Bug 5364
		private string m_Version;
		#endregion

		#region	Accessors
		/// <summary>
		/// Accessor for of service configuration elements.
		/// </summary>
		public SettingConfig[] SettingConfigs
		{
			get{return this.m_SettingConfigs;}
			set{this.m_SettingConfigs = value;}
		}

        // Fix for Bug 5364
		/// <summary>The version for the settings.</summary>
		public string Version
		{
			get { return m_Version; }
			set { m_Version = value; }
		}
		#endregion
	}
}
