using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

using BioRad.Common.Xml;
using BioRad.Common.Utilities;
using System.Xml.Linq;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Object to contain application options.
	/// </summary>
	/// <remarks>
	/// 
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Tom H</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href=""></see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: ApplicationOptions.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/ApplicationOptions/ApplicationOptions.cs $</item>
	///			<item name="vssrevision">$Revision: 13 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Thouser $</item>
	///			<item name="vssdate">$Date: 12/02/10 11:04a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public sealed class ApplicationOptions : BioRadXmlSerializableBase
	{
		#region Members
		/// <summary>
		/// Whether to show startup wizard at app startup.  Note that this is only used by Precision Melt. Cfx Manager keeps
		/// this flag as a user preference.
		/// </summary>
		private bool m_ShowStartupWizardAtAppStartup = true;
		/// <summary>
		/// email options.
		/// </summary>
		private EmailOptions m_EmailOptions = new EmailOptions();
		/// <summary>
		/// LIMS options.
		/// </summary>
		private LIMSOptions m_LIMSOptions = new LIMSOptions();
		/// <summary>
		/// Custom colors for use with color dialogs
		/// </summary>
		private List<int> m_CustomColors = new List<int>();
		/// <summary>
		/// Whether to show the software updates dialog automatically if updates are available.
		/// </summary>
		private bool m_UpdateNotification = true;
		/// <summary>
		/// Whether to show notification upon app startup that there is no internet connection.
		/// </summary>
		private bool m_NoInternetConnectionNotification = true;

		/// <summary>The folder name for the content run set files.</summary>
		private string m_ContentFolder = string.Empty;
		/// <summary>
		/// Whether to launch software in security edition mode.
		/// </summary>
		private bool m_LaunchSecurityEdition = false;
		/// <summary>
		/// Automatic export options.
		/// </summary>
		private AutomaticGridExportOptions m_AutoExportOptions = new AutomaticGridExportOptions();
		/// <summary>
		/// Automatic export options.
		/// </summary>
		private RDMLExportOptions m_RDMLExportOptions = new RDMLExportOptions();
		/// <summary>
		/// standard curve info collection.  Typed as IBioRadXmlSerializable since its assembly can't be referenced here.
		/// </summary>
		private IBioRadXmlSerializable m_StandardCurveInfoCollectionForFsd = null;

        private string m_SeegeneDestinationFolder = string.Empty;
        private int m_SeegeneNormalizationType = 0;
		#endregion

		#region Accessors
        /// <summary>
        /// 
        /// </summary>
        public int SeegeneNormalizationType
        {
            get { return m_SeegeneNormalizationType; }
            set { m_SeegeneNormalizationType = value; }
        }
        /// <summary>
        /// Destination Folder for Seegene Export in analysis.
        /// </summary>
        public string SeegeneDestinationFolder
        {
            get { return m_SeegeneDestinationFolder; }
            set { m_SeegeneDestinationFolder = value; }
        }
		/// <summary>
		/// Whether to show startup wizard at app startup.  Note that this is only used by Precision Melt, Cfx Manager keeps
		/// this flag as a user preference.
		/// </summary>
		public bool ShowStartupWizardAtAppStartup
		{
			get { return m_ShowStartupWizardAtAppStartup; }
			set { m_ShowStartupWizardAtAppStartup = value; }
		}
		/// <summary>
		/// email options.  Setting this to null will throw an ArgumentNullException.
		/// </summary>
		public EmailOptions EmailOptions
		{
			get { return m_EmailOptions; }
			set
			{
				if (value == null)
					throw new ArgumentNullException();

				m_EmailOptions = value;
			}
		}
		/// <summary>
		/// email options.  Setting this to null will throw an ArgumentNullException.
		/// </summary>
		public LIMSOptions LIMSOptions
		{
			get { return m_LIMSOptions; }
			set
			{
				if (value == null)
					throw new ArgumentNullException();

				m_LIMSOptions = value;
			}
		}
		/// <summary>
		/// Custom colors for use with color dialogs
		/// </summary>
		public List<int> CustomColors
		{
			get { return m_CustomColors; }
			set
			{
				if (value == null)
					value = new List<int>();
				m_CustomColors = value;
			}
		}
		/// <summary>
		/// Whether to show the software updates dialog automatically if updates are available.
		/// </summary>
		public bool UpdateNotification
		{
			get { return m_UpdateNotification; }
			set { m_UpdateNotification = value; }
		}
		/// <summary>
		/// Whether to show notification upon app startup that there is no internet connection.
		/// </summary>
		public bool NoInternetConnectionNotification
		{
			get { return m_NoInternetConnectionNotification; }
			set { m_NoInternetConnectionNotification = value; }
		}
		/// <summary>The folder name for the content run set files.</summary>
		public string ContentFolder
		{
			get { return m_ContentFolder; }
			set { m_ContentFolder = value; }
		}
		/// <summary>
		/// Whether to launch software in security edition mode.
		/// </summary>
		public bool LaunchSecurityEdition
		{
			get { return m_LaunchSecurityEdition; }
			set { m_LaunchSecurityEdition = value; }
		}
		/// <summary>
		/// Automatic export options.
		/// </summary>
		public AutomaticGridExportOptions AutoExportOptions
		{
			get { return m_AutoExportOptions; }
			set { m_AutoExportOptions = value; }
		}
		/// <summary>
		/// Automatic export options.
		/// </summary>
		public RDMLExportOptions RDMLExportOptions
		{
			get { return m_RDMLExportOptions; }
			set { m_RDMLExportOptions = value; }
		}
		/// <summary>
		/// 
		/// </summary>
		public IBioRadXmlSerializable StandardCurveInfoCollectionForFsd
		{
			get { return m_StandardCurveInfoCollectionForFsd; }
			set { m_StandardCurveInfoCollectionForFsd = value; }
		}
		#endregion

		#region Constructors
		/// <summary>Initializes a new instance of the ApplicationOptions class.</summary>
		public ApplicationOptions() { }

		/// <summary>
		/// FromXml deserializer.
		/// </summary>
		/// <param name="fromXml"></param>
		public ApplicationOptions(string fromXml)
		{
			FromXml(fromXml);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Return a deep clone.
		/// </summary>
		/// <returns>deep clone.</returns>
		public ApplicationOptions DeepClone()
		{
			return new ApplicationOptions(ToXml());
		}
		/// <summary>Value based (rather than reference based) equality test.  True if value equals, false if not.</summary>
		/// <param name="that">object to compare to.</param>
		/// <returns>true if value equals, false if not</returns>
		public bool ValueEquals(ApplicationOptions that)
		{
			return this.ToXml().Equals(that.ToXml());
		}
		#endregion

		#region Xml Serialization
		// Serialization version.
		private const int c_SerializationVersion = 9;

		// Serialization Ids
		private const string c_SerializationId = "ApplicationOptions";
		private const string c_SerializationVersionId = "SerVersion";
		private const string c_EmailOptionsId = "EmailOptions";
		private const string c_LIMSOptionsId = "LIMSOptions";
		private const string c_ShowStartupWizardAtAppStartupId = "ShowWizAtStart";
		private const string c_CustomColorsId = "CustomColors";
		private const string c_NotifyOfSoftwareUpdates = "NotifyUpdates";
		private const string c_NotifyOfNoInternetConnection = "NotifyNoInternet";
		private const string c_ContentFolder = "ContentFolder";
		private const string c_ContentFolderNamePreDesMouse = "ContentMouse";
		private const string c_ContentFolderNamePreDesHuman = "ContentHuman";
		private const string c_ContentFolderNameCustom = "ContentCustom";
		private const string c_LaunchSecurityEdition = "LaunchSecurityEdition";
		private const string c_AutoExportOptions = "AutoExportOptions";
		private const string c_RDMLExportOptions = "RDMLExportOptions";
		private const string c_StdCurveInfos = "StdCurveInfos";

		/// <summary>Deserialization constructor.</summary>
		public ApplicationOptions(XElement element)
		{
			FromXElement(element);
		}
		/// <summary>Gets the serialization id of the most derived class</summary>
		/// <returns>The serialization id of the most derived class</returns>
		protected override string GetSerializationId()
		{
			return c_SerializationId;
		}
		/// <summary>Fill a table in with sufficient information to represent the state of the object.</summary>
		/// <returns>a state table which will be populated by this method.  This state
		/// table will be of the most recent version.</returns>
		protected override SerializationState ToSerializationState()
		{
			SerializationState state = new SerializationState();
			state.AddInt(c_SerializationVersionId, c_SerializationVersion);

			state.AddIBioRadXmlSerializable(c_EmailOptionsId, m_EmailOptions);
			state.AddBool(c_ShowStartupWizardAtAppStartupId, m_ShowStartupWizardAtAppStartup);
			state.AddBool(c_LaunchSecurityEdition, m_LaunchSecurityEdition);
			state.AddIBioRadXmlSerializable(c_LIMSOptionsId, m_LIMSOptions);
			state.AddInts(c_CustomColorsId, m_CustomColors);
			state.AddBool(c_NotifyOfSoftwareUpdates, m_UpdateNotification);
			state.AddBool(c_NotifyOfNoInternetConnection, m_NoInternetConnectionNotification);
			state.AddIBioRadXmlSerializable(c_AutoExportOptions, m_AutoExportOptions);
			state.AddIBioRadXmlSerializable(c_RDMLExportOptions, m_RDMLExportOptions);
			//Content folders
			state.AddString(c_ContentFolder, m_ContentFolder);
			if (m_StandardCurveInfoCollectionForFsd != null)
				state.AddIBioRadXmlSerializable_Typed(c_StdCurveInfos, m_StandardCurveInfoCollectionForFsd);

			return state;
		}
		/// <summary>populate this instance from a SerializationState.</summary>
		/// <param name="state">state of the object</param>
		protected override void FromSerializationState(SerializationState state)
		{
			UpgradeSerializationState(state);

			// Validate that the version is the most recent, as guaranteed by UpgradeSerializationState().
			int version = state.GetInt(c_SerializationVersionId);
			if (version != c_SerializationVersion)
				throw new XmlSerializationException("State dictionary is not of the current version.");

			// Populate this object's state from the state dictionary
			m_EmailOptions = (EmailOptions)state.GetIBioRadXmlSerializable(c_EmailOptionsId, typeof(EmailOptions));
			m_ShowStartupWizardAtAppStartup = state.GetBool(c_ShowStartupWizardAtAppStartupId);
			m_LaunchSecurityEdition = state.GetBool(c_LaunchSecurityEdition);
			m_LIMSOptions = (LIMSOptions)state.GetIBioRadXmlSerializable(c_LIMSOptionsId, typeof(LIMSOptions));
			m_CustomColors = state.GetInts(c_CustomColorsId);
			m_UpdateNotification = state.GetBool(c_NotifyOfSoftwareUpdates);
			m_NoInternetConnectionNotification = state.GetBool(c_NotifyOfNoInternetConnection);
			m_AutoExportOptions = (AutomaticGridExportOptions)state.GetIBioRadXmlSerializable(c_AutoExportOptions, typeof(AutomaticGridExportOptions));
			m_RDMLExportOptions = (RDMLExportOptions)state.GetIBioRadXmlSerializable(c_RDMLExportOptions, typeof(RDMLExportOptions));
			//Content folder
			m_ContentFolder = state.GetString(c_ContentFolder);
			m_StandardCurveInfoCollectionForFsd = null;
			if (state.ContainsKey(c_StdCurveInfos))
				m_StandardCurveInfoCollectionForFsd = state.GetIBioRadXmlSerializable_Typed(c_StdCurveInfos);
		}
		/// <summary>Upgrades a SerializationState of any version, to the current version.</summary>
		/// <param name="state">persisted state to upgrade.</param>
		private void UpgradeSerializationState(SerializationState state)
		{
			int version = state.GetInt(c_SerializationVersionId);
			if (version < 1 || version > c_SerializationVersion)
			{
				// This is an unknown version.
				throw new XmlSerializationException(string.Format(CultureInfo.CurrentCulture,
						Properties.Resources.XmlSerializationUnknownSerializedVersion, version));
			}

			if (version == 1)
			{
				// Version 2 adds "show wizard at startup" flag.
				state.AddBool(c_ShowStartupWizardAtAppStartupId, true);

				// We are now consistent with version 2 state dictionaries.
				int newVersion = 2;
				state.AddInt(c_SerializationVersionId, newVersion);
				version = newVersion;
			}
			if (version == 2)
			{
				state.AddIBioRadXmlSerializable(c_LIMSOptionsId, m_LIMSOptions);

				// We are now consistent with version 3 state dictionaries.
				int newVersion = 3;
				state.AddInt(c_SerializationVersionId, newVersion);
				version = newVersion;
			}
			if (version == 3)
			{
				state.AddInts(c_CustomColorsId, new List<int>());

				// We are now consistent with version 4 state dictionaries.
				int newVersion = 4;
				state.AddInt(c_SerializationVersionId, newVersion);
				version = newVersion;
			}
			if (version == 4)
			{
				state.AddBool(c_NotifyOfSoftwareUpdates, true);

				// We are now consistent with version 5 state dictionaries.
				int newVersion = 5;
				state.AddInt(c_SerializationVersionId, newVersion);
				version = newVersion;
			}
			if (version == 5)
			{
				state.AddBool(c_NotifyOfNoInternetConnection, true);

				// We are now consistent with version 6 state dictionaries.
				int newVersion = 6;
				state.AddInt(c_SerializationVersionId, newVersion);
				version = newVersion;
			}

			if (version == 6)
			{
				// content folders path
				state.AddString(c_ContentFolder, ApplicationPath.ContentDirectory);

				state.AddBool(c_LaunchSecurityEdition, false);

				int newVersion = 7;
				state.AddInt(c_SerializationVersionId, newVersion);
				version = newVersion;
			}

			if (version == 7)
			{
				state.AddIBioRadXmlSerializable(c_AutoExportOptions, new AutomaticGridExportOptions());

				int newVersion = 8;
				state.AddInt(c_SerializationVersionId, newVersion);
				version = newVersion;
			}

			if (version == 8)
			{
				state.AddIBioRadXmlSerializable(c_RDMLExportOptions, new RDMLExportOptions());

				int newVersion = 9;
				state.AddInt(c_SerializationVersionId, newVersion);
				version = newVersion;
			}
			Debug.Assert(version == c_SerializationVersion);
		}
		#endregion Xml Serialization
	}

	/// <summary>
	/// static class to expose methods to load and save ApplicationOptions from and to their persisted store.
	/// </summary>
	public sealed class PersistedApplicationOptions
	{
		// Defaults for when persisted ApplicationOptions does not exist.
		private const string c_DefaultSmtpServer = "smtp.gmail.com";
		private const int c_DefaultPort = 587;
		private const bool c_DefaultUseSsl = true;
		private const bool c_DefaultUseDefaultFromAddress = true;
		private const bool c_DefaultRequiresAuthentication = true;
		private const string c_DefaultUserNameDomain = "@gmail.com";

		// Base filename of persisted store.
		private const string c_FileName = "ApplicationOptions.xml";

		// Lock to synchronize access to the file.
		private static object s_FileAccessLock = new object();

		/// <summary>Object used to provide singleton access to ApplicationOptions object.</summary>
		private static ApplicationOptions m_ApplicationOptions = null;

		/// <summary>
		/// Get reference to ApplicationOptions object.
		/// </summary>
		public static ApplicationOptions GetInstance
		{
			get
			{
				if (m_ApplicationOptions == null)
					m_ApplicationOptions = PersistedApplicationOptions.GetPersistedApplicationOptions();
				return m_ApplicationOptions;
			}
		}

		/// <summary>
		/// Retrieve the persisted application options.
		/// </summary>
		/// <returns></returns>
		/// // MERGE_REVIEW - Changed to public so that RunCommandLine can access it
		public static ApplicationOptions GetPersistedApplicationOptions()
		{
			try
			{
				lock (s_FileAccessLock)
				{
					string filePath = GetFilePath();

					if (File.Exists(filePath) == false)
						return GetDefaultApplicationOptions();

					Encoding encoding = Encoding.ASCII;
					byte[] bytes = File.ReadAllBytes(filePath);
					if (bytes.Length >= 2 && bytes[0] == 255 && bytes[1] == 254)
						encoding = Encoding.Unicode;
					string serializedApplicationOptions = File.ReadAllText(filePath, encoding);

					return new ApplicationOptions(serializedApplicationOptions);
				}
			}
			catch
			{
				Debug.Assert(false, "Exception thrown trying to unserialize app options.");
				return GetDefaultApplicationOptions();
			}
		}

		private static ApplicationOptions GetDefaultApplicationOptions()
		{
			ApplicationOptions ao = new ApplicationOptions();

			EmailOptions eo = ao.EmailOptions;
			eo.SmtpServerName = c_DefaultSmtpServer;
			eo.Port = c_DefaultPort;
			eo.UseSsl = c_DefaultUseSsl;
			eo.UseDefaultFromAddress = c_DefaultUseDefaultFromAddress;
			eo.AuthenticationRequired = c_DefaultRequiresAuthentication;
			eo.UserName = "<" + Properties.Resources.YourAccountName + ">" + c_DefaultUserNameDomain;

			// content folder
			ao.ContentFolder = ApplicationPath.ContentDirectory;
			if (!Directory.Exists(ao.ContentFolder))
				Directory.CreateDirectory(ao.ContentFolder);

			// make sure the directories exist
			//Other
			ao.LaunchSecurityEdition = false;
			return ao;
		}

		/// <summary>
		/// Store the persisted application options.
		/// </summary>
		/// <param name="ao"></param>
		public static void PersistApplicationOptions(ApplicationOptions ao)
		{
			lock (s_FileAccessLock)
			{
				string serializedApplicationOptions = ao.ToXml();
				File.WriteAllText(GetFilePath(), serializedApplicationOptions, Encoding.Unicode);
				//Update static object
				m_ApplicationOptions = ao;
			}
		}

		/// <summary>
		/// Get full path to file which contains the serialized ApplicationOptions.
		/// </summary>
		/// <returns></returns>
		private static string GetFilePath()
		{
			string fileFolder = ApplicationPath.ConfigDirectory;
			string filePath = System.IO.Path.Combine(fileFolder, c_FileName);
			return filePath;
		}
	}
}
