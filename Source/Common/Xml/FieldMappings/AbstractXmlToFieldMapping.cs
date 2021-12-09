using System;
using System.Xml.Serialization;
using System.Xml;
using BioRad.Common.Xml;

namespace BioRad.Common.Xml.FieldMappings
{
	#region Documentation Tags
	/// <summary>
	/// A base class for XmlToType rules, which specify how a field on a type should be 
	/// mapped in Xml.
	/// </summary>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Drew McAuliffe</item>
	///			<item name="review">Last design/code review:</item>
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
	///			<item name="vssfile">$Workfile: AbstractXmlToFieldMapping.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Xml/FieldMappings/AbstractXmlToFieldMapping.cs $</item>
	///			<item name="vssrevision">$Revision: 2 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public abstract partial class AbstractXmlToFieldMapping : IXmlToFieldMapping
	{
        #region Member Data
		/// <summary>
		/// Name for the Xml node.
		/// </summary>
		private string m_XmlName;
		/// <summary>
		/// Name of the field to be mapped.
		/// </summary>
		private string m_FieldName;
		/// <summary>
		/// Type of the field to be mapped.
		/// </summary>
		private Type m_FieldType;
		#endregion

        #region Accessors
		/// <summary>
		/// Accessors for node name.
		/// </summary>
		public string XmlName
		{
			get
			{
				return m_XmlName;
			}
			set
			{
			    m_XmlName = value;
			}
		}

		/// <summary>
		/// Accessors for mapped field name.
		/// </summary>
		public string FieldName
		{
			get
			{
				return m_FieldName;
			}
			set
			{
			    m_FieldName = value;
			}
		}
		/// <summary>
		/// Accessors for mapped field type.
		/// </summary>
		public Type FieldType
		{
			get
			{
				return m_FieldType;
			}
			set
			{
			    m_FieldType = value;
			}
		}
        #endregion

        #region Methods
		/// <summary>
		/// Converts the contents of this rule to an xml override for serialization.
		/// </summary>
		/// <returns>An XmlAttributes holding the attributes that this mapping corresponds
		/// to.</returns>
		public abstract XmlAttributes ToXmlAttributes();

		/// <summary>
		/// Accessor for node type.
		/// </summary>
		public abstract XmlNodeType XmlType
		{
			get;
		}
		#endregion

		#region Delegates and Events
		/// <summary>
		/// Event indicating that an attribute was added (code attribute
		/// marking a type with instructions for xml mapping; not an
		/// attribute in an xml element).
		/// </summary>
		public event XmlToTypeMappingEventHandler XmlAttributeAdded;
		#endregion

		#region Event Handlers
		/// <summary>
		/// Fires the XmlAttributeAdded event.
		/// </summary>
		/// <param name="xmlName">Xml name of node.</param>
		/// <param name="nodeType">Xml node type.</param>
		/// <param name="fieldName">Mapped type field name.</param>
		/// <param name="fieldType">Mapped type field type.</param>
		protected void FireAttributeAdded(string xmlName, XmlNodeType nodeType, 
			string fieldName, Type fieldType)
		{
			if (XmlAttributeAdded != null)
			{
			    XmlToFieldMappingEventArgs args = new XmlToFieldMappingEventArgs(xmlName,
					nodeType, fieldName, fieldType);
				XmlAttributeAdded(this, args);
			}
		}
		#endregion
	}
}
