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
	///			<item name="vssfile">$Workfile: CollectionFieldMapping.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Xml/FieldMappings/CollectionFieldMapping.cs $</item>
	///			<item name="vssrevision">$Revision: 2 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class CollectionFieldMapping : AbstractXmlToFieldMapping
	{
		#region Member Data
		/// <summary>
		/// Node name for individual elements of the collection. This is used when
		/// each element has a different name for each type. The order of node names should be 
		/// the same as the order of the types supplied.
		/// </summary>
		private string[] m_ItemXmlNames;
		/// <summary>
		/// The types for each possible node in the collection. If each type has a different
		/// name in the collection, it should have a correspondingly indexed member of the
		/// "collectionItemNodeNames" array as its node name. Otherwise, all types will share
		/// the same node name, provided by "collectionItemNodeName".
		/// </summary>
		private Type[] m_ItemTypes;
		#endregion

        #region Accessors
		/// <summary>
		/// Accessor for node type.
		/// </summary>
		public override XmlNodeType XmlType
		{
			get{return XmlNodeType.Element;}
		}
		/// <summary>
		/// Accessors for collection item types.
		/// </summary>
		public Type[] ItemTypes
		{
			get
			{
				return m_ItemTypes;
			}
			set
			{
			    m_ItemTypes = value;
			}
		}

		/// <summary>
		/// Accessors for collection item node names.
		/// </summary>
		public string[] ItemXmlNames
		{
			get
			{
				return m_ItemXmlNames;
			}
			set
			{
			    m_ItemXmlNames = value;
			}
		}
        #endregion

        #region Constructors and Destructor
		/// <summary>
		/// Default no-arg constructor.
		/// </summary>
		public CollectionFieldMapping(){}

		/// <summary>
		/// Constructor that takes in a node name, mapped field, a single name to use
		/// for all collection elements, and an array of acceptable types for those
		/// elements.
		/// </summary>
		/// <param name="collectionXmlName">Name of the XML element for the collection.</param>
		/// <param name="collectionFieldName">Name of the collection field in the mapped type.</param>
		/// <param name="itemXmlName">Xml name of individual items in the
		/// collection.</param>
		/// <param name="itemTypes">Allowable types in the collection field.</param>
		public CollectionFieldMapping(string collectionXmlName, string collectionFieldName, 
			string itemXmlName, Type[] itemTypes)
		{
			this.XmlName = collectionXmlName;
			this.FieldName = collectionFieldName;
			this.ItemTypes = itemTypes;
			this.ItemXmlNames = new string[]{itemXmlName};
		}

		/// <summary>
		/// Constructor that takes a node name, mapped field, an array of node names,
		/// and an array of types. The node name array should match the type array,
		/// with each indexed node name referring to the correspondingly indexed
		/// type.
		/// </summary>
		/// <param name="collectionXmlName">Name of the XML element for the collection.</param>
		/// <param name="collectionFieldName">Name of the collection field in the mapped type.</param>
		/// <param name="itemXmlNames">Xml names of individual items in the
		/// collection (corresponds to types)</param>
		/// <param name="itemTypes">Allowable types in the collection field
		/// (corresponds to names).</param>
		public CollectionFieldMapping(string collectionXmlName, string collectionFieldName, 
			string[] itemXmlNames, Type[] itemTypes)
		{
			this.XmlName = collectionXmlName;
			this.FieldName = collectionFieldName;
			this.ItemTypes = itemTypes;
			this.ItemXmlNames = itemXmlNames;
		}

		/// <summary>
		/// Null-safe getter for item name count.
		/// </summary>
		private int ItemXmlNameCount
		{
			get
			{
				if (this.ItemXmlNames == null)
					return 0;
				else
					return this.ItemXmlNames.Length;
			}
		}
		/// <summary>
		/// Null-safe getter for item type count.
		/// </summary>
		private int ItemTypeCount
		{
			get
			{
				if (this.ItemTypes == null)
					return 0;
				else
					return this.ItemTypes.Length;
			}
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
			// Add an attribute to name the collection node
			XmlArrayAttribute collection = new XmlArrayAttribute(this.XmlName);
			attributes.XmlArray = collection;
			this.FireAttributeAdded(this.XmlName, this.XmlType, this.FieldName, this.FieldType);
			// if there is more than one type but only one name, you need to
			// put the name as its own attribute.
			if (this.ItemTypeCount > 1 && this.ItemXmlNameCount == 1)
			{
				XmlArrayItemAttribute nameAttr = new XmlArrayItemAttribute(
					this.ItemXmlNames[0]);
				attributes.XmlArrayItems.Add(nameAttr);
				this.FireAttributeAdded(this.ItemXmlNames[0], XmlNodeType.Element,
					this.FieldName, null);
			}
			// iterate through type array
			for (int i = 0; i < this.ItemTypes.Length; i++)
			{
				Type type = this.ItemTypes[i];
				if (this.ItemXmlNameCount == 1 && this.ItemTypeCount > 1)
				{
					// if there's only one name, you add an array item attribute with only
					// the type specified.
					XmlArrayItemAttribute typeAttr = new XmlArrayItemAttribute(type);
					attributes.XmlArrayItems.Add(typeAttr);
					this.FireAttributeAdded(this.ItemXmlNames[0], XmlNodeType.Element,
						this.FieldName, type);
				}
				else
				{
					// otherwise, add a array item attribute with both name and type.
					string name = this.ItemXmlNames[i];
					XmlArrayItemAttribute nameAndTypeAttr = new XmlArrayItemAttribute(name, type);
					attributes.XmlArrayItems.Add(nameAndTypeAttr);
					this.FireAttributeAdded(name, XmlNodeType.Element,
						this.FieldName, type);
				}

			}
			return attributes;
		}
		#endregion

	}
}
