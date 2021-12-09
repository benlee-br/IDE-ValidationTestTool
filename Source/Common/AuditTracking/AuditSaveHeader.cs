using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Linq;
using BioRad.Common.Xml;

namespace BioRad.Common.AuditTracking
{
	/// <summary>Enum that describes the type of event that generated the AuditSaveHeader.</summary>
	public enum AuditEventType
	{
		/// <summary>Unassigned.</summary>
		Unassigned,
		/// <summary>Run changes - start, complete, abort etc.</summary>
		RunStatus,
		/// <summary>Data File Save - Unsigned</summary>
		UnsignedSave,
		/// <summary>Data File Save - Signed</summary>
		SignedSave,
		/// <summary>Pre-run changes to the protocol.</summary>
		PreRunProtocolModification
	}

	#region Documentation Tags
	/// <summary>Contains information about one or more changes commited by a certain 
	/// user at a certain time. </summary>
	/// <remarks></remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Tom Houser</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">????</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	#endregion

	[Serializable]
	public partial class AuditSaveHeader : System.Collections.IEnumerable, ICloneable
	{
		#region Constants
		#region Xml Tags
		/// <summary>The main tag for the AuditSaveHeader Information.</summary>
		public static readonly string c_AuditHeader = "AuditHeader";
		private const string c_User = "User";
		private const string c_UserFullName = "UserFullName";
		private const string c_ComputerName = "ComputerName";
		private const string c_Comments = "Comments";
		private const string c_Date = "Date";
		private const string c_Signature = "Signature";
		private const string c_SignatureComments = "SignatureComment";
		private const string c_ClientApplication = "Application";
		private const string c_ClientApplicationVersion = "ApplicationVersion";
		private const string c_GUID = "GUID";
		private const string c_Version = "Version";
		private const string c_Type = "Type";
		private const string c_AuditChangesArray = "AuditChanges";
		private const string c_AuditChange = "AuditChange";
		#endregion
		#endregion

		#region Member Data
		/// <summary>
		/// Electronic signature of user.
		/// </summary>
		private string m_Signature;
		/// <summary>
		/// Comment for meaning of signature.
		/// </summary>
		private string m_SignatureComment;
		/// <summary>
		/// The time at which the change was saved.
		/// </summary>
		private System.DateTime m_Time;
		/// <summary>
		/// The user who performed the change.
		/// </summary>
		private string m_User;
		/// <summary>
		/// The comment attached to the change by the user.
		/// </summary>
		private string m_Comment;
		/// <summary>
		/// The application in which the change was made.
		/// </summary>
		private string m_Application;
		/// <summary>
		/// The user's full name
		/// </summary>
		private string m_FullUserName;
		/// <summary>
		/// The computer's name
		/// </summary>
		private string m_MachineName;
		/// <summary>
		/// The application version
		/// </summary>
		private string m_ApplicationVersion;
		/// <summary>
		/// The GUID associated with this log item.
		/// </summary>
		private string m_GUID;
		/// <summary>
		/// The version of the datafile in which this log is written.  This is
		/// incremented each time the user saves the file, thus m_Version essentially
		/// means "the number of times this datafile has been changed".  This is useful
		/// for matching a hard-copy report, on which the version will be reported,
		/// to a specific datafile version.
		/// </summary>
		private int m_Version;
		/// <summary>
		/// The individual changes performed by the user.
		/// </summary>
		private List<AuditChangeDetail> m_Changes;
		/// <summary>
		/// The type of event that generated the AuditSaveHeader.
		/// </summary>
		private AuditEventType m_AuditType;
		#endregion

		#region Accessors
		/// <summary>
		/// Electronic signature of user.
		/// </summary>
		public string Signature
		{
			get{return m_Signature;}
			set{ m_Signature = value; }
		}
		/// <summary>
		/// Comment for meaning of signature.
		/// </summary>
		public string SignatureComment
		{
			get{return m_SignatureComment;}
			set{ m_SignatureComment = value; }
		}
		/// <summary>
		/// The time at which the change was saved.
		/// </summary>
		public System.DateTime Time
		{
			get{ return this.m_Time; }
			set{ this.m_Time = value; }
		}
		/// <summary>
		/// The user who performed the change.
		/// </summary>
		public string User
		{
			get{ return this.m_User; }
			set{ this.m_User = value; }
		}
		/// <summary>
		/// The comment attached to the change by the user.
		/// </summary>
		public string Comment
		{
			get{ return this.m_Comment; }
			set{ this.m_Comment = value; }
		}
		/// <summary>
		/// The application in which the change was made.
		/// </summary>
		public string Application
		{
			get{ return this.m_Application; }
			set{ this.m_Application = value; }
		}
		/// <summary>
		/// The user's full name
		/// </summary>
		public string FullUserName
		{
			get{ return this.m_FullUserName; }
			set{ this.m_FullUserName = value; }
		}
		/// <summary>
		/// The computer's name
		/// </summary>
		public string MachineName
		{
			get{ return this.m_MachineName; }
			set{ this.m_MachineName = value; }
		}
		/// <summary>
		/// The application version
		/// </summary>
		public string ApplicationVersion
		{
			get{ return this.m_ApplicationVersion; }
			set{ this.m_ApplicationVersion = value; }
		}
		/// <summary>
		/// The GUID associated with this log item.
		/// </summary>
		public string GUID
		{
			get{ return this.m_GUID; }
			set{ this.m_GUID = value; }
		}
		/// <summary>
		/// The version of the datafile in which this log is written.  This is
		/// incremented each time the user saves the file, thus m_Version essentially
		/// means "the number of times this datafile has been saved".  This is useful
		/// for matching an exported report, on which the version will be printed,
		/// to a specific datafile version.
		/// </summary>
		public int Version
		{
			get{ return m_Version; }
			set{ m_Version = value; }
		}
		/// <summary>
		/// Returns the LogItemChangeDetail at given index
		/// </summary>
		public AuditChangeDetail this[ int i ]
		{
			get{ return (AuditChangeDetail)m_Changes[ i ]; }
		}
		/// <summary>
		/// The type of event that generated the AuditSaveHeader.
		/// </summary>
		public AuditEventType AuditType
		{
			get { return this.m_AuditType;}
			set { this.m_AuditType = value;}
		}
        #endregion

        #region Constructors and Destructor
        /// <summary>Creates a signed instance of the AuditSaveHeader class.</summary>
        /// <param name="user">The user name (DOMAIN\USERNAME)</param>
        /// <param name="userFullName">The user's full name.</param>
        /// <param name="machineName">The computer name.</param>
        /// <param name="comments">Comments associated with save/signature event.</param>
        public AuditSaveHeader(string user, string userFullName, string machineName,
            string comments)
        {
            m_Changes = new List<AuditChangeDetail>();

            string clientAppName;
            string clientAppVersion;

            ApplicationPath.GetClientApplicationInfo
                (out clientAppName, out clientAppVersion);
            this.m_Application = clientAppName;
            // Fix for Bug 4238 - add the deloyment tag information
            string deploymentTag = string.Empty;
            try
            {
                if (ApplicationStateData.GetInstance != null &&
                    ApplicationStateData.GetInstance[ApplicationStateData.Setting.DeploymentTag] != null)
                {
                    deploymentTag = ApplicationStateData.GetInstance[ApplicationStateData.Setting.DeploymentTag];
                }
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
            }

            this.m_ApplicationVersion = !string.IsNullOrEmpty(clientAppVersion) ? clientAppVersion : string.Empty;
            if (!string.IsNullOrEmpty(deploymentTag))
            {
                this.m_ApplicationVersion = StringUtility.FormatString(Properties.Resources.AppVersion_2,//"{0}. ({1})."
                    clientAppVersion, deploymentTag);
            }

            this.m_Comment = comments;
            this.m_FullUserName = userFullName;
            this.m_GUID = Guid.NewGuid().ToString();
            this.m_MachineName = machineName;
            this.m_Time = DateTime.Now;
            this.m_User = user;

            // Use the properties to set these field.
            this.m_Signature = string.Empty;
            this.m_SignatureComment = string.Empty;
        }
		#endregion

		#region Methods
		/// <summary>
		/// Implementation of the IEnumerator interface.
		/// </summary>
		/// <returns>
		/// An enumerator that can be used to iterate over all the "change" elements 
		/// contained by this object.
		/// </returns>
		public IEnumerator GetEnumerator()
		{
			return m_Changes.GetEnumerator();
		}
        /// <summary>Adds a change to this LogItem_Root, which will by default be displayed 
        /// after any previously added changes.</summary>
        /// <returns>Nothing.</returns>
        /// <remarks>Adds by reference, so care must be taken to ensure nobody modifies it 
        /// behind this object's back.</remarks>
        public void AddChange(AuditChangeDetail change)
        {
            // MSTR-570 SE: Changes not being made globally to all the floures
            // Below if statement was merged in from 4.1 Maestro release.
            // Removed to match 4.0 Maestro release.
            //Fixed defect MSTR - 730, Audit trail displays repeating logs.
            if (!ChangeExists(change))//US1157 prevent duplicate items from being added.
                m_Changes.Add(change);
        }
        /// <summary>Adds a collection of changes to this LogItem_Root, which will by default 
        /// be displayed after any previously added
        /// changes.</summary>
        /// <returns>Nothing.</returns>
        /// <remarks>Adds by reference, so care must be taken to ensure nobody modifies it 
        /// behind this object's back.</remarks>
        public void AddChanges( AuditChangeDetail[] changes )
		{
			m_Changes.AddRange( changes );
		}
		/// <summary>Removes all the change details from the collection.</summary>
		public void ClearChanges()
		{
			this.m_Changes.Clear();
		}
		/// <summary>returns the number of changes contained in this object.</summary>
		/// <returns>Number of changes.</returns>
		public int NumChanges
		{
			get{ return m_Changes.Count; }
		}
		/// <summary>Create a clone of the AuditSaveHeader object.</summary>
		/// <returns>The cloned AuditSaveHeader object.</returns>
		public object Clone()
		{
			AuditSaveHeader clonedObject = (AuditSaveHeader) this.MemberwiseClone();

			if(this.m_Changes != null)
			{
				clonedObject.m_Changes = new List<AuditChangeDetail>();
				foreach(AuditChangeDetail changeDetail in this.m_Changes)
				{
					clonedObject.AddChange((AuditChangeDetail) changeDetail.Clone());
				}
			}
			return clonedObject;
		}
        /// <summary>
        /// US1157 check if change already exists.
        /// </summary>
        /// <param name="change"></param>
        /// <returns></returns>
        private bool ChangeExists(AuditChangeDetail change)
        {
            var changeFound = (from c in m_Changes
                               where (c.Auditable == change.Auditable &&
                               c.NewValue == change.NewValue &&
                               c.OldValue == change.OldValue &&
                               c.Description == change.Description)
                               select c);
            if (changeFound.Count() > 0)
                return true;
            else
                return false;
        }
        /// <summary>Checks if the changes collection contains an object with the passed in auditableItem value.</summary>
        /// <param name="auditableItem">The AuditableItems value to check.</param>
        /// <returns>A bool value.</returns>
        public bool AuditableChangeExists(AuditableItems.Auditable auditableItem)
		{
			var changeFound = (from change in m_Changes
							   where change.Auditable == auditableItem
							   select change);
			if (changeFound.Count() > 0)
				return true;
			else
				return false;
		}
		/// <summary>Returns a localized string for UI display which specifies
		///  only the information contained directly in the header, without any
		///  change detail information.</summary>
		/// <returns>The string specified in the summary comments.</returns>
		public string ToHeaderString()
		{
			string s = string.Empty;
			if(this.m_AuditType.Equals(AuditEventType.UnsignedSave))
			{
                s = StringUtility.FormatString(Properties.Resources.SaveHeaderString_8,
					m_Version, m_User, m_FullUserName, m_Time, m_MachineName, 
					m_Application, m_ApplicationVersion, m_Comment );
			}
			else if(this.m_AuditType.Equals(AuditEventType.SignedSave))
			{
                s = StringUtility.FormatString(Properties.Resources.SignedSaveHeaderString_9,
					m_Version, m_User, m_FullUserName, m_Time, m_MachineName, 
					m_Application, m_ApplicationVersion, m_SignatureComment, m_Comment );
			}
			return s;
		}
        
		/// <summary>Returns a localized string suitable for displaying 
		/// the information contained in the object.</summary>
		/// <returns>The string specified in the summary comments.</returns>
		public override string ToString()
		{
			System.Text.StringBuilder s = new System.Text.StringBuilder();
			s.Append( "* " );
			s.Append( ToHeaderString() );

			string signatureString;
			if( this.m_Signature == "" )
			{
                signatureString = StringUtility.FormatString(Properties.Resources.NoSignatureString);
			}
			else
			{
                signatureString = StringUtility.FormatString(Properties.Resources.SignatureString, 
					this.m_Signature,
					this.m_SignatureComment );
			}
			s.Append( " " );
			s.Append( signatureString );

			for( int i = 0; i < m_Changes.Count; ++ i )
				s.Append( "\n" ).Append( "\t* " ).Append( m_Changes[ i ].ToString() );

			return s.ToString();
		}
		/// <summary>Returns a string similar to ToString() but also annotated in Rich 
		/// Text Format.  The values of the fields of the object are bolded.</summary>
		/// <returns>The string specified in the summary comments.</returns>
		public string ToRtfString()
		{
			StringBuilder[] stringParameters = new StringBuilder[ 8 ];
			for( int i = 0; i < stringParameters.GetLength( 0 ); ++ i )
				stringParameters[ i ] = new StringBuilder();
			stringParameters[ 0 ].Append( this.m_Version );
			stringParameters[ 1 ].Append( this.m_User );
			stringParameters[ 2 ].Append( this.m_FullUserName );
			stringParameters[ 3 ].Append( this.m_Time );
			stringParameters[ 4 ].Append( this.m_MachineName );
			stringParameters[ 5 ].Append( this.m_Application );
			stringParameters[ 6 ].Append( this.m_ApplicationVersion );
			stringParameters[ 7 ].Append( this.m_Comment );
			for( int i = 0; i < stringParameters.GetLength( 0 ); ++ i )
			{
				// RTF will eat any "\" characters unless we force a literal interpretation with 
				//  another "\".
				for( int j = 0; j < stringParameters[ i ].Length; ++ j )
					if( stringParameters[ i ][ j ] == '\\' )
					{
						stringParameters[ i ].Insert( j, "\\" );
						++ j;
					}
				// Make it bold.
				stringParameters[ i ].Insert( 0, "{\\b " );
				stringParameters[ i ].Append( "}" );
			}
			StringBuilder s = new StringBuilder();
			s.Append( "{\\rtf1\n" );
			s.Append( "{\\b * }" );
            s.Append(StringUtility.FormatString(Properties.Resources.SaveHeaderString_8,
				stringParameters[ 0 ].ToString(),
				stringParameters[ 1 ].ToString(),
				stringParameters[ 2 ].ToString(),
				stringParameters[ 3 ].ToString(),
				stringParameters[ 4 ].ToString(),
				stringParameters[ 5 ].ToString(),
				stringParameters[ 6 ].ToString(),
				stringParameters[ 7 ].ToString() ) );
			string signatureString;
			if( this.m_Signature == "" )
			{
                signatureString = StringUtility.FormatString(Properties.Resources.NoSignatureString);
			}
			else
			{
				stringParameters = new StringBuilder[ 2 ];
				for( int i = 0; i < stringParameters.GetLength( 0 ); ++ i )
					stringParameters[ i ] = new StringBuilder();
				stringParameters[ 0 ].Append( this.m_Signature );
				stringParameters[ 1 ].Append( this.m_SignatureComment );
				for( int i = 0; i < stringParameters.GetLength( 0 ); ++ i )
				{
					// RTF will eat any "\" characters unless we force a literal interpretation with 
					//  another "\".
					for( int j = 0; j < stringParameters[ i ].Length; ++ j )
						if( stringParameters[ i ][ j ] == '\\' )
						{
							stringParameters[ i ].Insert( j, "\\" );
							++ j;
						}
					stringParameters[ i ].Insert( 0, "{\\b " );
					stringParameters[ i ].Append( "}" );
				}
                signatureString = StringUtility.FormatString(Properties.Resources.SignatureString, 
					stringParameters[ 0 ].ToString(),
					stringParameters[ 1 ].ToString() );
			}
			s.Append( " " );
			s.Append( signatureString );
			for( int i = 0; i < m_Changes.Count; ++ i )
			{
				s.Append( "\\par\n     {\\b * }" );
				s.Append( ((AuditChangeDetail)m_Changes[ i ]).ToRtfString() );
			}
			s.Append( "}\n" );

			return s.ToString();
		}
		/// <summary>Returns a string similar to ToString() but also annotated in Rich 
		/// Text Format.  The values of the fields of the object are bolded.</summary>
		/// <remarks>Returns a short version of the RTF string.</remarks>
		/// <param name="includeChangeDetails">Include the change details in the 
		/// returned string?</param>
		/// <returns>The string specified in the summary comments.</returns>
		public string ToShortRtfString(bool includeChangeDetails)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{\\rtf1\n");
			stringBuilder.Append("{\\b ");
			stringBuilder.Append(this.Comment);
			stringBuilder.Append("}");

			if (includeChangeDetails)
			{
				foreach (AuditChangeDetail changeDetail in this.m_Changes)
				{
					stringBuilder.Append("\\par *");
					stringBuilder.Append(changeDetail.ToShortRtfString());
				}
			}

			return stringBuilder.ToString();
		}
		#region Xml Methods
		/// <summary>Exports state of object as an XML string.</summary>
		/// <returns>XML string describing state of object data.</returns>
		public string ToXml()
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = false;
			settings.OmitXmlDeclaration = true;
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			StringBuilder output = new StringBuilder();
			XmlWriter writer = XmlWriter.Create(output, settings);
			XmlReaderSettings readerSettings = new XmlReaderSettings();
			readerSettings.ConformanceLevel = ConformanceLevel.Fragment;
			XmlReader reader = null;

			writer.WriteStartElement(c_AuditHeader);

			writer.WriteAttributeString(c_User, this.User);
			writer.WriteAttributeString(c_UserFullName, this.FullUserName);
			writer.WriteAttributeString(c_ComputerName, this.MachineName);
			writer.WriteAttributeString(c_Comments, this.Comment);
			writer.WriteAttributeString(c_Date, StringUtility.DateTimeToUtcString(this.Time));
			writer.WriteAttributeString(c_Signature, this.Signature);
			writer.WriteAttributeString(c_SignatureComments, this.SignatureComment);
			writer.WriteAttributeString(c_ClientApplication, this.Application);
			writer.WriteAttributeString(c_ClientApplicationVersion, this.ApplicationVersion);
			writer.WriteAttributeString(c_GUID, this.GUID);
			writer.WriteAttributeString(c_Version, this.Version.ToString());
			writer.WriteAttributeString(c_Type, this.AuditType.ToString());

			int numChanges = this.NumChanges;
			for (int index = 0; index < numChanges; index++)
			{
				reader = XmlReader.Create(new StringReader(this[index].ToXml()),
					readerSettings);
				writer.WriteNode(reader, true);
			}
			writer.WriteEndElement();

			writer.Close();

			return output.ToString();
		}
		/// <summary>Exports state of object as an XML string.</summary>
		/// <param name="eventType">The audit event type</param>
		/// <param name="changeDetail">The AuditChangeDetail object.</param>
		/// <returns>XML string describing state of object data.</returns>
		public static string ToShortXml(AuditEventType eventType, 
			AuditChangeDetail changeDetail)
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = false;
			settings.OmitXmlDeclaration = true;
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			XmlReaderSettings readerSettings = new XmlReaderSettings();
			readerSettings.ConformanceLevel = ConformanceLevel.Fragment;
			StringBuilder output = new StringBuilder();
			XmlWriter writer = XmlWriter.Create(output, settings);
			XmlReader reader = null;

			writer.WriteStartElement(c_AuditHeader);
			writer.WriteAttributeString(c_Date, StringUtility.DateTimeToUtcString
				(DateTime.Now));
			writer.WriteAttributeString(c_Type, eventType.ToString());
			if (changeDetail != null)
			{
				reader = XmlReader.Create(new StringReader(changeDetail.ToXml()),
					readerSettings);
				writer.WriteNode(reader, true);
			}

			writer.WriteEndElement();
			writer.Close();

			return output.ToString();
		}
		/// <summary>Creates an AuditSaveHeader object from the passed in parameters.</summary>
		/// <param name="auditString">The audit string</param>
		/// <param name="userName">The user name.</param>
		/// <param name="userFullName">The user full name.</param>
		/// <param name="machineName">The machine name.</param>
		/// <returns>An AuditSaveHeader object.</returns>
		public static AuditSaveHeader FromShortXml(string auditString,
			string userName, string userFullName, string machineName)
		{
			AuditSaveHeader auditSaveHeader = null;

			XmlReaderSettings settings = new XmlReaderSettings();
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			settings.IgnoreWhitespace = true;
			XmlReader reader = XmlReader.Create(new System.IO.StringReader(auditString),
			   settings);
						
			reader.Read();
			if (StringUtility.StringMatch(reader.Name, c_AuditHeader))
			{
				auditSaveHeader = new AuditSaveHeader(userName, userFullName,
				machineName, string.Empty);
				auditSaveHeader.Time = StringUtility.W3DateStringToDateTime
					(XMLUtility.ReadAttribute(reader, c_Date, c_AuditHeader));
				auditSaveHeader.AuditType = (AuditEventType) Enum.Parse
					(typeof(AuditEventType), 
					XMLUtility.ReadAttribute(reader, c_Type, c_AuditHeader));
				AuditChangeDetail changeDetail = new AuditChangeDetail();
				if (changeDetail.FromXml(reader.ReadInnerXml()))
					auditSaveHeader.AddChange(changeDetail);
			}
			reader.Close();

			return auditSaveHeader;
		}
		/// <summary>
		/// Deserializes from an XML serialization.
		/// </summary>
		/// <param name="xml">the string which contains an appropriate XML fragment.</param>
		/// <returns>true if successful deserialization</returns>
		public bool FromXml(string xml)
		{
			try
			{

				XmlReaderSettings settings = new XmlReaderSettings();
				settings.ConformanceLevel = ConformanceLevel.Fragment;
				settings.IgnoreWhitespace = true;
				XmlReader reader = XmlReader.Create(new System.IO.StringReader(xml),
				   settings);

				if (!reader.ReadToFollowing(c_AuditHeader))
					return false;

				string user = string.Empty;
				string userFullName = string.Empty;
				string computerName = string.Empty;
				string comments = string.Empty;

				user = XMLUtility.ReadAttribute(reader, c_User, c_AuditHeader);
				userFullName = XMLUtility.ReadAttribute(reader, c_UserFullName, c_AuditHeader);
				computerName = XMLUtility.ReadAttribute(reader, c_ComputerName, c_AuditHeader);
				comments = XMLUtility.ReadAttribute(reader, c_Comments, c_AuditHeader);

				this.m_User = user;
				this.m_FullUserName = userFullName;
				this.m_MachineName = computerName;
				this.m_Comment = comments;
				
				this.m_Time = StringUtility.W3DateStringToDateTime(XMLUtility.ReadAttribute
					(reader, c_Date, c_AuditHeader));
				this.m_Signature = XMLUtility.ReadAttribute(reader, c_Signature,
					c_AuditHeader);
				this.m_SignatureComment = XMLUtility.ReadAttribute(reader,
					c_SignatureComments, c_AuditHeader);
				this.m_Application = XMLUtility.ReadAttribute(reader,
					c_ClientApplication, c_AuditHeader);
				this.m_ApplicationVersion = XMLUtility.ReadAttribute(reader,
					c_ClientApplicationVersion, c_AuditHeader);
				this.m_GUID = XMLUtility.ReadAttribute(reader, c_GUID, c_AuditHeader);
				this.m_Version = int.Parse(XMLUtility.ReadAttribute(reader, c_Version,
					c_AuditHeader));
				string auditType = (reader.GetAttribute(c_Type));
				this.m_AuditType = (auditType == null) ?
					AuditEventType.Unassigned :
					(AuditEventType)Enum.Parse(typeof(AuditEventType), auditType);

				// there may be no change details and so we need to check that before we start reading
				if (!reader.IsEmptyElement)
				{
					reader.Read();
					while (!reader.EOF)
					{
						switch (reader.NodeType)
						{
							case XmlNodeType.Element:
								{
									if (StringUtility.StringMatch(reader.Name,
										c_AuditChange))
									{
										AuditChangeDetail changeDetail = new AuditChangeDetail();
										changeDetail.FromXml(reader.ReadOuterXml());

										// add to the changes collection
										this.AddChange(changeDetail);
									}
									break;
								}
							case XmlNodeType.EndElement:
								reader.Read();
								break;
						}
					}
				}

				reader.Close();
				return true;
			}
			catch
			{
				return false;
			}
		}
		#endregion
		#endregion
	}
}
