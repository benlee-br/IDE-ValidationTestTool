using BioRad.Common.Xml;
using System;
using System.Text;
using System.Xml;

namespace BioRad.Common.AuditTracking
{
    #region Documentation Tags
    /// <summary>Contains information about the specific 
    /// change a user has made to a specific object. </summary>
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
	public partial class AuditChangeDetail : ICloneable
	{
		#region Constants
		private const string c_AuditChange = "AuditChange";
		private const string c_Auditable = "Auditable";
		private const string c_OldValue = "OldValue";
		private const string c_NewValue = "NewValue";
		private const string c_Description = "Description";
		#endregion

		#region Member Data
		/// <summary>
		/// The pre-change value of the changed object
		/// </summary>
		private string m_OldValue;
		/// <summary>
		/// The post-change value of the changed object
		/// </summary>
		private string m_NewValue;
		/// <summary>
		/// Any extra information needed for this object.
		/// </summary>
		private string m_Description;
		/// <summary>
		/// The Enum that defines the auditable objects.
		/// </summary>
		private AuditableItems.Auditable m_Auditable;
		#endregion

		#region Constructors
		/// <summary>Creates a new object of the AuditChangeDetail class</summary>
		public AuditChangeDetail()
		{
			m_OldValue = string.Empty;
			m_NewValue = string.Empty;
			m_Description = string.Empty;
			m_Auditable = AuditableItems.Auditable.Unassigned;
		}
		#endregion
		#region Accessors
		/// <summary>
		/// The pre-change value of the changed object
		/// </summary>
		public string OldValue
		{
			get{ return this.m_OldValue; }
			set{ this.m_OldValue = value; }
		}
		/// <summary>
		/// The post-change value of the changed object
		/// </summary>
		public string NewValue
		{
			get{ return this.m_NewValue; }
			set{ this.m_NewValue = value; }
		}
		/// <summary>
		/// Any extra information needed for this object.
		/// </summary>
		public string Description
		{
			get { return this.m_Description;}
			set { this.m_Description = value;}
		}
		/// <summary>
		/// The Enum that defines the auditable objects.
		/// </summary>
		public AuditableItems.Auditable Auditable
		{
			get { return this.m_Auditable;}
			set { this.m_Auditable = value;}
		}
		#endregion

		#region Methods
		/// <summary>Create a clone of the AuditChangeDetail object.</summary>
		/// <returns>The cloned AuditChangeDetail object.</returns>
		public object Clone()
		{
			return this.MemberwiseClone();
		}
		/// <summary>Create a plain-text string from the object's data</summary>
		/// <returns>the string</returns>
		public override string ToString()
		{
            return StringUtility.FormatString(Properties.Resources.ChangeDetailString, 
				GetChangedObjectDisplayString(),
				GetOldValueDisplayString(), GetNewValueDisplayString(), m_Description );
		}
		/// <summary>Create an RTF-annotated string from the object's data.</summary>
		/// <returns>the string</returns>
		public string ToRtfString()
		{
			StringBuilder[] stringParameters = new StringBuilder[ 4 ];
			for( int i = 0; i < stringParameters.GetLength( 0 ); ++ i )
				stringParameters[ i ] = new StringBuilder();
			stringParameters[ 0 ].Append( this.GetChangedObjectDisplayString() );
			stringParameters[ 1 ].Append( this.GetOldValueDisplayString() );
			stringParameters[ 2 ].Append( this.GetNewValueDisplayString() );
			stringParameters[ 3 ].Append( this.m_Description );
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
			StringBuilder s = new StringBuilder();
			s.Append( "{\\rtf1\n" );
            s.Append(StringUtility.FormatString(Properties.Resources.ChangeDetailString,
				stringParameters[ 0 ].ToString(),
				stringParameters[ 1 ].ToString(),
				stringParameters[ 2 ].ToString(),
				stringParameters[ 3 ].ToString() ) );
			s.Append( "}\n" );
			return s.ToString();
		}
		/// <summary>Create an RTF-annotated string from the object's data.</summary>
		/// <remarks>Creates a short RTF string</remarks>
		/// <returns>the string</returns>
		public string ToShortRtfString()
		{
			string changedObject = AuditableItems.GetItemLocalizedName( m_Auditable, false );
			if (!string.IsNullOrEmpty(this.m_Description))
			{
				return StringUtility.FormatString(Properties.Resources.ChangeDetailString,
					changedObject, GetOldValueDisplayString(), GetNewValueDisplayString(),
					this.m_Description);
			}
			else if (!string.IsNullOrEmpty(this.m_OldValue))
			{
				return StringUtility.FormatString(Properties.Resources.ChangeDetailShortString_3,
					changedObject,
					GetOldValueDisplayString(),
					GetNewValueDisplayString());
			}
			else
			{
				return StringUtility.FormatString(Properties.Resources.ChangeDetailShortString_2,
					changedObject, this.m_NewValue);
			}
		}
		/// <summary>Gets the localized name of the object</summary>
		/// <returns>Localized string</returns>
		private string GetChangedObjectDisplayString()
		{
			// Fix for Bug 4144 - if the old or new value for this change is an
			// empty string - the name will have a (*) added to the end
			bool containsEmptyValues = ((this.m_OldValue.Length.Equals(0)) ||
				(this.m_NewValue.Length.Equals(0))) ? true : false;
			return AuditableItems.GetItemLocalizedName( m_Auditable, containsEmptyValues );
		}
		/// <summary>localizes and rounds if necessary</summary>
		/// <returns></returns>
		public string GetOldValueDisplayString()
		{
			// localize and round/truncate if it's a floating point number.
			return GetDisplayString( OldValue );
		}
		/// <summary>localizes and rounds if necessary</summary>
		/// <returns></returns>
		public string GetNewValueDisplayString()
		{
			// localize and round/truncate if it's a floating point number.
			return GetDisplayString( NewValue );
		}
		/// <summary>Rounds, truncates, and localizes if the parameter string 
		/// contains a number. 
		/// Also breaks up very long words, as intelligently as possible,
		/// to account for problems with the spread column sizing and HTML 
		/// report sizing.</summary>
		/// <param name="a">input string to be converted</param>
		/// <returns>The display string</returns>
		private string GetDisplayString( string a )
		{
			double number;
			try
			{
				// MSTR-709 If file path then leave string as is.
				System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(System.IO.Path.GetDirectoryName(a));
				if ( di.Exists)
				{
					return a;
				}
			}
			catch(Exception ex)
			{
				string message = ex.ToString();
				//System.Diagnostics.Debug.Assert(false,message);
			}

			if (GlobalMethods.IsNumeric(a, out number))
			{
				// TFS 1961: turning off rounding in audit trail.
				//number = System.Math.Round( number, 3 ); 
				return number.ToString();
			}
			else
			{
				string[] words = a.Split( " ".ToCharArray() );
				int longestWord = 0;
				foreach( string word in words )
					longestWord = Math.Max( longestWord, word.Length );
				
				const int maxWordLength = 80;
				if( longestWord > maxWordLength )
				{
					// Fix for defect 4055: break up really long words.
					System.Text.StringBuilder ret = new System.Text.StringBuilder();

					foreach( string word in words )
					{
						if( word.Length > maxWordLength )
						{
							string subWord = word;
							while( subWord.Length > maxWordLength )
							{
								// If the word has any slashes within range, then it's probably a directory,
								// so we'll add a space after the last one within range of maxLength.
								int slashIndex = subWord.Substring( 0, maxWordLength ).
									LastIndexOfAny( "\\/".ToCharArray() );
								if( slashIndex >= 0 )
								{
									ret.Append( subWord.Substring( 0, slashIndex + 1 ) );
									ret.Append( " " );
									subWord = subWord.Substring( slashIndex + 1 );
								}
								else
								{
									// Just add a space arbitrarily at the max length cutoff.
									int index = maxWordLength - 1;
									ret.Append( subWord.Substring( 0, index + 1 ) );
									ret.Append( " " );
									subWord = subWord.Substring( index + 1 );
								}
							}
							ret.Append( subWord );
						}
						else
							ret.Append( word ).Append( " " );
					}
					return ret.ToString();
				}
					
				return a;
			}
		}

		#region Xml Methods
		/// <summary>Exports state of object as an XML string.</summary>
		/// <returns>XML string describing state of object data.</returns>
		public string ToXml()
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.OmitXmlDeclaration = true;
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			StringBuilder output = new StringBuilder();
			XmlWriter writer = XmlWriter.Create(output, settings);

			writer.WriteStartElement(c_AuditChange);

			// write the attributes
			writer.WriteAttributeString(c_Auditable, this.Auditable.ToString());
			writer.WriteAttributeString(c_OldValue, this.OldValue);
			writer.WriteAttributeString(c_NewValue, this.NewValue);
			writer.WriteAttributeString(c_Description, this.Description);

			writer.WriteEndElement();

			writer.Close();
			return output.ToString();
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

				if (!reader.ReadToFollowing(c_AuditChange))
					return false;

				this.m_Auditable = (AuditableItems.Auditable)Enum.Parse
					(typeof(AuditableItems.Auditable),
					XMLUtility.ReadAttribute(reader, c_Auditable, c_AuditChange));
				this.m_OldValue = XMLUtility.ReadAttribute(reader, c_OldValue, 
					c_AuditChange);
				this.m_NewValue = XMLUtility.ReadAttribute(reader, c_NewValue,
					c_AuditChange);
				this.m_Description = reader.GetAttribute(c_Description);
				if (string.IsNullOrEmpty(this.m_Description))
					this.m_Description = string.Empty;
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
