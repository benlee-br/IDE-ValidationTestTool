using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

using BioRad.Common.Xml;
using BioRad.Common.Utilities;
using System.Xml.Linq;

namespace BioRad.Common
{
	/// <summary>
	/// class to contain email server options.
	/// </summary>
	public class EmailOptions : BioRadXmlSerializableBase
	{
		//#region Constants
		//#endregion

		#region Member Data
		/// <summary>Smtp server name</summary>
		private string m_SmtpServerName = string.Empty;
		/// <summary>Port number for SMTP server.</summary>
		private int m_Port = 0;
		/// <summary>Whether to use Ssl in the SMTP server.</summary>
		private bool m_UseSsl = false;
		/// <summary>Whether to use a default "from" address.</summary>
		private bool m_UseDefaultFromAddress = true;
		/// <summary>"from" email address to use for outgoing mails, if m_UseDefaultFromAddress is false.</summary>
		private string m_FromAddress = string.Empty;
		/// <summary>Whether authentication is required for the Smtp server</summary>
		private bool m_AuthenticationRequired;
		/// <summary>Smtp server user name</summary>
		private string m_UserName = string.Empty;
		/// <summary>Password for the Smtp server user name</summary>
		private string m_Password = string.Empty;
		/// <summary>Email address for sending test emails</summary>
		private string m_TestEmail = string.Empty;
		/// <summary>Whether to include an attachment in the test email.</summary>
		private bool m_IncludeAttachmentInTestEmail = false;
		/// <summary>Size in MB of attachment for test email.</summary>
		private double m_AttachmentSizeForTestEmail = 0.5;
		/// <summary>SMTP server timeout, in minutes.</summary>
		private double m_Timeout = 10.0;
		/// <summary>Whether the server settings have been successfully tested.</summary>
		private bool m_SuccessfullyTested = false;
		#endregion

		#region Accessors
		/// <summary>Smtp server name. Cannot be set to null.</summary>
		public string SmtpServerName
		{
			get { return m_SmtpServerName; }
			set 
			{
				if (value == null)
					throw new ArgumentNullException();
				m_SmtpServerName = value; 
			}
		}
		/// <summary>Port number for SMTP server.</summary>
		public int Port
		{
			get { return m_Port; }
			set { m_Port = value; }
		}
		/// <summary>Whether to use Ssl in the SMTP server.</summary>
		public bool UseSsl
		{
			get { return m_UseSsl; }
			set { m_UseSsl = value; }
		}
		/// <summary>Whether to use a default "from" address.</summary>
		public bool UseDefaultFromAddress
		{
			get { return m_UseDefaultFromAddress; }
			set { m_UseDefaultFromAddress = value; }
		}
		/// <summary>"from" email address to use for outgoing mails, if UseDefaultFromAddress
		/// is false.</summary>
		public string FromAddress
		{
			get { return m_FromAddress; }
			set 
			{
				if (value == null)
					throw new ArgumentNullException();
				m_FromAddress = value; 
			}
		}
		/// <summary>Whether authentication is required for the Smtp server</summary>
		public bool AuthenticationRequired
		{
			get { return m_AuthenticationRequired; }
			set { m_AuthenticationRequired = value; }
		}
		/// <summary>Smtp server login. Cannot be set to null.</summary>
		public string UserName
		{
			get { return m_UserName; }
			set 
			{
				if (value == null)
					throw new ArgumentNullException();
				m_UserName = value; 
			}
		}
		/// <summary>Password for the Smtp server. Cannot be set to null.</summary>
		public string Password
		{
			get { return m_Password; }
			set 
			{
				if (value == null)
					throw new ArgumentNullException();
				m_Password = value; 
			}
		}
		/// <summary>Email address for sending test emails. Cannot be set to null.</summary>
		public string TestEmail
		{
			get { return m_TestEmail; }
			set 
			{
				if (value == null)
					throw new ArgumentNullException();
				m_TestEmail = value; 
			}
		}
		/// <summary>Whether to include attachment in the test email.</summary>
		public bool IncludeAttachmentInTestEmail
		{
			get { return m_IncludeAttachmentInTestEmail; }
			set { m_IncludeAttachmentInTestEmail = value; }
		}
		/// <summary>Size in MB of attachment for test email.</summary>
		public double AttachmentSizeForTestEmail
		{
			get { return m_AttachmentSizeForTestEmail; }
			set { m_AttachmentSizeForTestEmail = value; }
		}
		/// <summary>SMTP server timeout, in minutes.</summary>
		public double Timeout
		{
			get { return m_Timeout; }
			set { m_Timeout = value; }
		}
		/// <summary>Whether the server settings have been successfully tested.</summary>
		public bool SuccessfullyTested
		{
			get { return m_SuccessfullyTested; }
			set { m_SuccessfullyTested = value; }
		}
		#endregion

		//#region Delegates and Events
		//#endregion

		#region Constructors and Destructor

		/// <summary>Initializes a new instance of the EmailOptions class.</summary>
		public EmailOptions() { }

		/// <summary>
		/// FromXml deserializer.
		/// </summary>
		/// <param name="fromXml"></param>
		public EmailOptions(string fromXml)
		{
			FromXml(fromXml);
		}

		#endregion

		#region Methods
		/// <summary>
		/// deep clone.
		/// </summary>
		/// <returns>deep clone.</returns>
		public EmailOptions DeepClone()
		{
			return new EmailOptions(this.ToXml());
		}
		/// <summary>
		/// Determine whether server settings (rather than everything about the object) are equal.
		/// </summary>
		/// <param name="that">object to compare to</param>
		/// <returns>true if the server settings are equal between this and the provided object.</returns>
		public bool AreServerSettingsEqual(EmailOptions that)
		{
			if (m_SmtpServerName != that.m_SmtpServerName)
				return false;
			if (m_Port != that.m_Port)
				return false;
			if (m_UseSsl != that.m_UseSsl)
				return false;
			if (m_UseDefaultFromAddress != that.m_UseDefaultFromAddress)
				return false;
			if (m_FromAddress != that.m_FromAddress)
				return false;
			if (m_AuthenticationRequired != that.m_AuthenticationRequired)
				return false;
			if (m_UserName != that.m_UserName)
				return false;
			if (m_Password != that.m_Password)
				return false;

			return true;
		}
		/// <summary>Value based (rather than reference based) equality test.  True if value equals, false if not.</summary>
		/// <param name="that">object to compare to.</param>
		/// <returns>true if value equals, false if not</returns>
		public bool ValueEquals(EmailOptions that)
		{
			return this.ToXml().Equals(that.ToXml());
		}
		#endregion

		#region Xml Serialization
		// Serialization version.
		private const int c_SerializationVersion = 1;

		// Serialization Ids
		private const string c_SerializationId = "EmailOptions";
		private const string c_SerializationVersionId = "SerVersion";
		private const string c_SmtpServerNameId = "SmtpServerName";
		private const string c_SmtpServerPortId = "SmtpServerPort";
		private const string c_UseSslId = "UseSsl";
		private const string c_UseDefaultFromAddressId = "UseDefaultFromAddress";
		private const string c_FromAddressId = "FromAddress";
		private const string c_AuthenticationRequiredId = "AuthenticationRequired";
		private const string c_UserNameId = "UserName";
		private const string c_PasswordId = "Password";
		private const string c_TestEmailId = "TestEmail";
		private const string c_IncludeAttachmentInTestEmailId = "IncludeAttachment";
		private const string c_AttachmentSizeForTestEmailId = "AttachmentSize";
		private const string c_TimeoutId = "Timeout";
		private const string c_SuccessfullyTestedId = "SuccessfullyTested";

		/// <summary>Deserialization constructor.</summary>
		public EmailOptions(XElement element) 
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

			state.AddString(c_SmtpServerNameId, m_SmtpServerName);
			state.AddInt(c_SmtpServerPortId, m_Port);
			state.AddBool(c_UseSslId, m_UseSsl);
			state.AddBool(c_UseDefaultFromAddressId, m_UseDefaultFromAddress);
			state.AddString(c_FromAddressId, m_FromAddress);
			state.AddBool(c_AuthenticationRequiredId, m_AuthenticationRequired);
			state.AddString(c_UserNameId, m_UserName);
			state.AddString(c_PasswordId, FileCryptor.GetInstance.EncryptToString(m_Password));
			state.AddString(c_TestEmailId, m_TestEmail);
			state.AddBool(c_IncludeAttachmentInTestEmailId, m_IncludeAttachmentInTestEmail);
			state.AddDouble(c_AttachmentSizeForTestEmailId, m_AttachmentSizeForTestEmail);
			state.AddDouble(c_TimeoutId, m_Timeout);
			state.AddBool(c_SuccessfullyTestedId, m_SuccessfullyTested);
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
			m_SmtpServerName = state.GetString(c_SmtpServerNameId);
			m_Port = state.GetInt(c_SmtpServerPortId);
			m_UseSsl = state.GetBool(c_UseSslId);
			m_UseDefaultFromAddress = state.GetBool(c_UseDefaultFromAddressId);
			m_FromAddress = state.GetString(c_FromAddressId);
			m_AuthenticationRequired = state.GetBool(c_AuthenticationRequiredId);
			m_UserName = state.GetString(c_UserNameId);
			m_Password = FileCryptor.GetInstance.DecryptFromString(state.GetString(c_PasswordId));
			m_TestEmail = state.GetString(c_TestEmailId);
			m_IncludeAttachmentInTestEmail = state.GetBool(c_IncludeAttachmentInTestEmailId);
			m_AttachmentSizeForTestEmail = state.GetDouble(c_AttachmentSizeForTestEmailId);
			m_Timeout = state.GetDouble(c_TimeoutId);
			m_SuccessfullyTested = state.GetBool(c_SuccessfullyTestedId);
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
						"Unknown serialized version", version));
			}

			//// Example:
			//if (version == 1)
			//{
			//   // Upgrade version 1 SerializationState to version 2

			//   // We are now consistent with version 2 state dictionaries.
			//   int newVersion = 2;
			//   state.AddInt(c_SerializationVersionId, newVersion);
			//   version = newVersion;
			//}

			Debug.Assert(version == c_SerializationVersion);
		}
		#endregion Xml Serialization
		//#region Event Handlers
		//#endregion
	}
}
