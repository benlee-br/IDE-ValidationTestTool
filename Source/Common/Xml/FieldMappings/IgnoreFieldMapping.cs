using System;
using System.Xml.Serialization;
using System.Xml;
using BioRad.Common.Xml.FieldMappings;

namespace BioRad.Common.Xml.FieldMappings
{
	#region Documentation Tags
	/// <summary>
	/// Creates a field mapping that forces the field in question to be ignored in XML
	/// output.
	/// </summary>
	/// <remarks>
	/// Use of the ignore mapping is extremely important. Until the default behavior of the
	/// type mapping changes, every field that doesn't have an explicit mapping will use the
	/// default mapping, which may not be desired. Use of this mapping will get rid of 
	/// unwanted fields.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Drew McAuliffe</item>
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
	///			<item name="vssfile">$Workfile: IgnoreFieldMapping.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Xml/FieldMappings/IgnoreFieldMapping.cs $</item>
	///			<item name="vssrevision">$Revision: 2 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public partial class IgnoreFieldMapping: AbstractXmlToFieldMapping
	{
		#region Constants
		/// <summary>
		/// Constant used to indicate that the attr is being ignored.
		/// Used when attribute events are fired.
		/// </summary>
		public const string c_IgnoreXmlName = "-ignoring-";
		#endregion
		#region Accessors
		/// <summary>
		/// Accessor for node type.
		/// </summary>
		public override XmlNodeType XmlType
		{
			get{return XmlNodeType.None;}
		}
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Default no-arg constructor.
		/// </summary>
		public IgnoreFieldMapping(){}

		/// <summary>
		/// Constructor that uses the most important fields.
		/// </summary>
		/// <param name="mappedFieldName">Mapped field name.</param>
		public IgnoreFieldMapping(string mappedFieldName)
		{
			this.FieldName = mappedFieldName;
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
			attributes.XmlIgnore = true;
			this.FireAttributeAdded(c_IgnoreXmlName, this.XmlType, this.FieldName, this.FieldType);
			return attributes;
		}
		#endregion
	}
}
