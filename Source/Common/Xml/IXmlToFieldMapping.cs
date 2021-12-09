using System;
using System.Xml;
using System.Xml.Serialization;

namespace BioRad.Common.Xml
{
	#region Documentation Tags
	/// <summary>
	/// An interface representing a rule for xml type mappings.
	/// Each rule specifies how a field in a given type should be mapped to Xml.
	/// </summary>
	/// <remarks>
	/// This is actually a thin wrapper around XmlAttributes.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Drew McAuliffe</item>
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
	///			<item name="vssfile">$Workfile: IXmlToFieldMapping.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/Xml/IXmlToFieldMapping.cs $</item>
	///			<item name="vssrevision">$Revision: 1 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dmcauliffe $</item>
	///			<item name="vssdate">$Date: 7/01/04 1:16p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public interface IXmlToFieldMapping
	{
        #region Accessors
		/// <summary>
		/// Accessor for the node name of the rule (e.g., the xml element or attr name)
		/// </summary>
		string XmlName { get;}
		/// <summary>
		/// Accessor for the node type (element, attr, etc.)
		/// </summary>
		XmlNodeType XmlType{get;}
		/// <summary>
		/// Field name to be mapped
		/// </summary>
		string FieldName{get;}
		/// <summary>
		/// Accessor for the type of the field to be mapped.
		/// </summary>
		Type FieldType{get;}

        #endregion

        #region Methods
		/// <summary>
		/// Converts this rule to an xml attributes.
		/// </summary>
		/// <returns>This rule as a collection of xml attributes.</returns>
		XmlAttributes ToXmlAttributes();
        #endregion

		#region Events and Delegates
		/// <summary>
		/// An event indicating that an XmlAttribute was added while creating 
		/// an XmlAttributes object. Useful for debugging.
		/// </summary>
		event XmlToTypeMappingEventHandler XmlAttributeAdded;

		#endregion

	}
}
