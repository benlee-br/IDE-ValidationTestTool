using System;
using System.Xml.Serialization;
using System.Xml;
using BioRad.Common.Xml.FieldMappings;

namespace BioRad.Common.Xml.FieldMappings
{
	#region Documentation Tags
	/// <summary>
	/// A rule specifying that a type's field should be mapped to an element.
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
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
	///			<item name="vssfile">$Workfile: ElementFieldMapping.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Xml/FieldMappings/ElementFieldMapping.cs $</item>
	///			<item name="vssrevision">$Revision: 2 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class ElementFieldMapping : AbstractXmlToFieldMapping
	{
        #region Accessors
		/// <summary>
		/// Accessor for node type.
		/// </summary>
		public override XmlNodeType XmlType
		{
			get{return XmlNodeType.Element;}
		}
        #endregion

        #region Constructors and Destructor
		/// <summary>
		/// Default no-arg constructor.
		/// </summary>
		public ElementFieldMapping(){}

		/// <summary>
		/// Constructor that uses the most important fields.
		/// </summary>
		/// <param name="nodeName">Node name.</param>
		/// <param name="mappedFieldName">Mapped field name.</param>
		/// <param name="mappedFieldType">Mapped field type.</param>
		public ElementFieldMapping(string nodeName, string mappedFieldName, Type mappedFieldType)
		{
			this.XmlName = nodeName;
			this.FieldName = mappedFieldName;
			this.FieldType = mappedFieldType;
		}

		#endregion

        #region Methods
		/// <summary>
		/// Converts the contents of this rule to a set of xml attributes for serialization.
		/// </summary>
		/// <returns>The resultant collection of attributes.</returns>
		public override XmlAttributes ToXmlAttributes()
		{
			XmlAttributes attributes = new XmlAttributes();
			// determine how to add attrs for node based on node type
			XmlElementAttribute element = new XmlElementAttribute(XmlName, FieldType);
			attributes.XmlElements.Add(element);
			this.FireAttributeAdded(this.XmlName, this.XmlType, this.FieldName, this.FieldType);;
			return attributes;
		}
		#endregion

	}
}
