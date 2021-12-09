using System;
using System.Collections;
using System.Reflection;
using System.Xml.Serialization;
using BioRad.Common.Xml;
using BioRad.Common.Xml.FieldMappings;
namespace BioRad.Common.Xml
{
	#region Documentation Tags
	/// <summary>
	/// The main entry point for using xml to type mappings. This is intended
	/// as a thin wrapper around the XML serialization classes, specifically to
	/// construct an XmlAttributeOverrides from a collection of rules (which 
	/// themselves are wrappers around XmlAttributes classes).
	/// </summary>
	/// <remarks>
	/// The name "XmlToTypeMapping" is used instead of the simpler "XmlTypeMapping" to
	/// avoid conflicts with that class, which is an existing System.Xml class.
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
	///			<item name="vssfile">$Workfile: XmlToTypeMapping.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Xml/XmlToTypeMapping.cs $</item>
	///			<item name="vssrevision">$Revision: 7 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class XmlToTypeMapping
	{
        #region Member Data

		/// <summary>
		/// A static map of XmlAttributeOverrides objects by type is maintained to cache type mapping
		/// results. This is to allow the overrides to be manually set to "ignore" everything, only 
		/// turning things on when explicitly requested by the calling code.
		/// </summary>
		private static Hashtable s_OverridesByType = new Hashtable();

		/// <summary>
		/// A table holding multiple hashtables similar to m_FieldMappings, each
		/// mapped by a derived type. If any members in this mapping have a different
		/// derived type than the base class (e.g., they're base class members), then
		/// their attribute override needs to point to the derived class, not the 
		/// mapped class.
		/// </summary>
		private Hashtable m_DerivedTypeMappingsByType = new Hashtable();

		/// <summary>
		/// The list of all rules stored in the set.
		/// </summary>
		private Hashtable m_FieldMappings = new Hashtable();
		/// <summary>
		/// The list of all dependent (nested) sets.
		/// </summary>
		private Hashtable m_NestedTypeMappings = new Hashtable();
		/// <summary>
		/// The type for which this ruleset is appropriate.
		/// </summary>
		private Type m_MappedType;
		/// <summary>
		/// The name of the element under which this type will be mapped.
		/// </summary>
		private string m_XmlElementName;
		#endregion

        #region Accessors
		/// <summary>
		/// Accessors for element name.
		/// </summary>
		public string XmlElementName
		{
			get
			{
				return m_XmlElementName;
			}
			set
			{
			    m_XmlElementName = value;
			}
		}

		/// <summary>
		/// Accessors for mapped type.
		/// </summary>
		public Type MappedType
		{
			get
			{
				return m_MappedType;
			}
			set
			{
				m_MappedType = value;
			}
		}

		/// <summary>
		/// Accessors for field mappings.
		/// </summary>
		public ICollection FieldMappings
		{
			get{return this.m_FieldMappings;}
		}

        #endregion


        #region Constructors and Destructor
		/// <summary>
		/// Creates an instance, setting the mapped type and element name.
		/// </summary>
		/// <param name="mappedType">The type to which this mapping ruleset applies.</param>
		/// <param name="elementName">The name for the root element of the type.</param>
		public XmlToTypeMapping(Type mappedType, string elementName)
		{
			this.m_MappedType = mappedType;
			this.m_XmlElementName = elementName;
		}
        #endregion

        #region Methods
		/// <summary>
		/// Convenience method for adding a rule to map a field as an element.
		/// </summary>
		/// <param name="xmlName">The element name</param>
		/// <param name="fieldName">The field to map</param>
		/// <param name="fieldType">The type of the field to map</param>
		public void AddElementMapping(string xmlName, string fieldName, 
			Type fieldType)
		{
		    ElementFieldMapping fieldMapping = 
				new ElementFieldMapping(xmlName, fieldName, fieldType);
		    AddFieldMapping(fieldMapping);
		}
		/// <summary>
		/// Convenience method for adding a rule to map a field as an attribute.
		/// </summary>
		/// <param name="xmlName">The attribute name</param>
		/// <param name="fieldName">The field to map</param>
		/// <param name="fieldType">The type of the field to map</param>
		public void AddAttributeMapping(string xmlName, string fieldName, 
			Type fieldType)
		{
			AttributeFieldMapping fieldMapping = 
				new AttributeFieldMapping(xmlName, fieldName, fieldType);
		    AddFieldMapping(fieldMapping);
		}
		/// <summary>
		/// A convenience method for adding a rule to map a collection.
		/// This assumes that a collection element wrapper goes around each
		/// collection field, and that all elements within the collection are of
		/// the same type.
		/// </summary>
		/// <param name="collectionXmlName">The name of the collection element.</param>
		/// <param name="collectionFieldName">The name of the collection field to map.</param>
		/// <param name="itemXmlName">The name of individual
		/// elements within the collection element.</param>
		/// <param name="itemType">The type of individual object
		/// within the collection returned by accessing the collection field.</param>
		public void AddCollectionMapping(string collectionXmlName, string collectionFieldName, 
			string itemXmlName, Type itemType)
		{
		    AddCollectionMapping(collectionXmlName, collectionFieldName, itemXmlName,
				new Type[]{itemType});
		}

		/// <summary>
		/// A convenience method for adding a rule to map a collection.
		/// This assumes that a collection element wrapper goes around each
		/// collection field, that elements within the collection may
		/// be of different types, and that those elements all have the same
		/// name.
		/// </summary>
		/// <param name="collectionXmlName">The name of the collection element.</param>
		/// <param name="collectionFieldName">The name of the collection field to map.</param>
		/// <param name="itemXmlName">The name of individual
		/// elements within the collection element.</param>
		/// <param name="itemTypes">An array of types allowed in the
		/// collection.</param>
		public void AddCollectionMapping(string collectionXmlName, string collectionFieldName, 
			string itemXmlName, Type[] itemTypes)
		{
		    CollectionFieldMapping fieldMapping = 
				new CollectionFieldMapping(collectionXmlName,
				collectionFieldName, itemXmlName, itemTypes);
		    AddFieldMapping(fieldMapping);
		}

		/// <summary>
		/// A convenience method for adding a rule to map a collection.
		/// This assumes that a collection element wrapper goes around each
		/// collection field, that elements within the collection may
		/// be of different types, and that those elements may have different
		/// names. The array of types should match the array of names, so
		/// that each type is named the correspondingly indexed name in the
		/// name array.
		/// </summary>
		/// <param name="collectionXmlName">The name of the collection element.</param>
		/// <param name="collectionFieldName">The name of the collection field to map.</param>
		/// <param name="itemXmlNames">The names of individual
		/// elements within the collection element.</param>
		/// <param name="itemTypes">An array of types allowed in the
		/// collection.</param>
		public void AddCollectionMapping(string collectionXmlName, string collectionFieldName, 
			string[] itemXmlNames, Type[] itemTypes)
		{
		    CollectionFieldMapping fieldMapping = 
				new CollectionFieldMapping(collectionXmlName,
				collectionFieldName, itemXmlNames, itemTypes);
		    AddFieldMapping(fieldMapping);
		}

		/// <summary>
		/// A convenience method for adding an "ignore" mapping. THis is the same as
		/// using an "XmlIgnore" attribute on the field. It means that the field will
		/// not be handled for either serialization or deserialization.
		/// </summary>
		/// <param name="fieldNameToIgnore"></param>
		public void AddIgnoreMapping(string fieldNameToIgnore)
		{
			IgnoreFieldMapping mapping = new IgnoreFieldMapping(fieldNameToIgnore);
			AddFieldMapping(mapping);
		}

		/// <summary>
		/// Adds a mapping.
		/// </summary>
		/// <param name="mapping">The mapping to add.</param>
		public void AddFieldMapping(IXmlToFieldMapping mapping)
		{
			mapping.XmlAttributeAdded += new XmlToTypeMappingEventHandler(
				this.OnXmlAttributeAdded);
			string key = GetFieldMappingKey(mapping);
			if (m_FieldMappings.ContainsKey(key))
			{
				m_FieldMappings.Remove(key);
			}
			m_FieldMappings.Add(key, mapping);
		}

		/// <summary>
		/// Adds another mapping, used to represent a nested object. Object
		/// hierarchies are represented this way.
		/// </summary>
		/// <param name="typeMapping">The mapping to add.</param>
		public void AddNestedTypeMapping(XmlToTypeMapping typeMapping)
		{
			// register for events
			typeMapping.XmlAttributeAdded += new XmlToTypeMappingEventHandler(
				this.OnXmlAttributeAdded);
			// add it
		    m_NestedTypeMappings.Add(GetTypeMappingKey(typeMapping), typeMapping);
		}

		/// <summary>
		/// Creates a key used to store field mappings.
		/// </summary>
		/// <param name="mapping">The mapping to get key for</param>
		/// <returns>The key</returns>
		private string GetFieldMappingKey(IXmlToFieldMapping mapping)
		{
			/*
			string xmlName = mapping.XmlName;
			string xmlType = mapping.XmlType.ToString();
			string fieldName = mapping.FieldName;
			string fieldType = "";
			if (mapping.FieldType != null)
				fieldType = mapping.FieldType.FullName;
			return xmlName + ":" + xmlType + ":" + fieldName + ":" + fieldType;
			*/
			return mapping.FieldName;
		}
		/// <summary>
		/// Creates a key used to store nested type mappings.
		/// </summary>
		/// <param name="mapping">The nested type mapping.</param>
		/// <returns>The key.</returns>
		private string GetTypeMappingKey(XmlToTypeMapping mapping)
		{
			string xmlName = mapping.XmlElementName;
			string typeName = "";
			if (mapping.MappedType != null)
				typeName = mapping.MappedType.FullName;
			return xmlName + ":" + typeName;
		}

		/// <summary>
		/// Converts the contents, including all rules, to an 
		/// overrides object.
		/// </summary>
		/// <remarks>
		/// THis method version allows you to work on an existing overrides.
		/// </remarks>
		/// <returns>The resultant overrides set.</returns>
		public XmlAttributeOverrides ToAttributeOverrides(XmlAttributeOverrides existing)
		{
			//TODO: implement caching so that this isn't necessary each time
			SetAllUnmappedFieldsToIgnore();

			foreach (IXmlToFieldMapping mapping in m_FieldMappings.Values)
			{
				XmlAttributes attrs = mapping.ToXmlAttributes();
				
				Type type = GetDeclaringTypeForMember(this.MappedType, mapping.FieldName);
				// See if override has been done already. This is possible if a type
				// aggregates types that derive from a common base type. In particular,
				// Add threw an exception when base type properties were ignored explicitly
				// multiple times.
				XmlAttributes current = existing[type, mapping.FieldName];
				// TODO: Could it somehow be verified that the attempted override is indeed
				// a duplicate of an earlier override and throw an exception if not?
				if (current == null)
				{
					// add to overrides
					existing.Add(type, mapping.FieldName, attrs);
				}
				//else
				//{
					// TODO: Remove this trace when fix verified by Drew
				//	Console.WriteLine(String.Format("Skipping mapping for {0} field {1}.", type.ToString(), mapping.FieldName));
				//}
			}
			// add the rule for the base type itself
			XmlAttributes rootAttrs = new XmlAttributes();
			XmlRootAttribute rootAttr = new XmlRootAttribute(this.XmlElementName);
			//rootAttr.Namespace = null;
			rootAttrs.Xmlns = false;
			rootAttrs.XmlRoot = rootAttr;
			existing.Add(this.MappedType, rootAttrs);
			// add rules from all dependent rulesets
			foreach (XmlToTypeMapping nestedMapping in m_NestedTypeMappings.Values)
			{
				existing = nestedMapping.ToAttributeOverrides(existing);
			}
			return existing;
		}

		/// <summary>
		/// Forces all serializable fields that don't have mappings to be set to 
		/// "ignore"; this prevents unwanted serialization of unspecified fields.
		/// This is necessary, since the .NET xml serialization will automatically
		/// serialize everything using a default method of serialization (everything is
		/// an element), which may conflict with or get confused with the mappings
		/// specified by this type mapping. By turning everything off unless it is
		/// explicitly requested, confusing and unwanted results are avoided.
		/// </summary>
		private void SetAllUnmappedFieldsToIgnore()
		{
			// iterate through all fields
			FieldInfo[] mappableFields = this.m_MappedType.GetFields(
				BindingFlags.Public | BindingFlags.Instance);
			foreach (FieldInfo fi in mappableFields)
			{
				IgnoreFieldMapping ignoreMapping = new IgnoreFieldMapping(fi.Name);
				string mappingKey = GetFieldMappingKey(ignoreMapping);
				if (! this.m_FieldMappings.ContainsKey(mappingKey))
				{
					this.m_FieldMappings.Add(mappingKey, ignoreMapping);
				}
			}
		
			// iterate through all properties
			PropertyInfo[] mappableProps = this.m_MappedType.GetProperties(
				BindingFlags.Public | BindingFlags.Instance);
			foreach (PropertyInfo pi in mappableProps)
			{
				IgnoreFieldMapping ignoreMapping = new IgnoreFieldMapping(pi.Name);
				//Console.WriteLine("Declaring type for {0}.{1}={2}",
				//	this.m_MappedType.FullName, pi.Name, pi.DeclaringType);

				string mappingKey = GetFieldMappingKey(ignoreMapping);
				if (! this.m_FieldMappings.ContainsKey(mappingKey))
				{
					this.m_FieldMappings.Add(mappingKey, ignoreMapping);
				}
			}
		}

		private Type GetDeclaringTypeForMember(Type owningType, string memberName)
		{
			MemberInfo[] mis = owningType.GetMember(memberName);
			//TODO: add proper exception
			if (mis == null || mis.Length != 1)
				throw new Exception("Can't find " +  memberName + " in type " + owningType.FullName);
			MemberInfo mi = mis[0];
			// if the declaring type isn't the first type given, navigate up
			// the type hierarchy to find the declaring type.
			if (! mi.DeclaringType.Equals(owningType))
			{
				Type baseType = owningType.BaseType;
				while (! baseType.Equals(typeof(System.Type)))
				{
					if (mi.DeclaringType.Equals(baseType))
					{
						return baseType;
					}
					baseType = baseType.BaseType;
				}
			}
			// otherwise, if the declaring type is the first type given, or it
			// couldn't be found otherwise, return the first type given.
			return owningType;
		}

		/// <summary>
		/// Adds a mapping for a given type.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="mapping"></param>
		private void AddMappingForType(Type type, IXmlToFieldMapping mapping)
		{
			Hashtable fieldMappingsByType = (Hashtable) this.m_DerivedTypeMappingsByType[type];
			if (fieldMappingsByType == null)
			{
				fieldMappingsByType = new Hashtable();
			}
			fieldMappingsByType.Add(type, mapping);
		}


		/// <summary>
		/// Converts the contents of this ruleset to an overrides object.
		/// </summary>
		/// <returns>Attribute overrides that the ruleset represents.</returns>
		public XmlAttributeOverrides ToAttributeOverrides()
		{
			XmlAttributeOverrides or = new XmlAttributeOverrides();
			return this.ToAttributeOverrides(or);
		}

        #endregion

		#region Delegates and Events

		/// <summary>
		/// An event indicating that an XmlAttribute was added while creating 
		/// an XmlAttributes object. Useful for debugging.
		/// This is actually raised by events processed by the field mappings.
		/// </summary>
		public event XmlToTypeMappingEventHandler XmlAttributeAdded;
		#endregion

		#region Event Handlers
		/// <summary>
		/// When this event is fired by a field mapping, it gets refired for this
		/// type mapping.
		/// </summary>
		/// <param name="sender">event sender</param>
		/// <param name="args">event args</param>
		public void OnXmlAttributeAdded(object sender, XmlToFieldMappingEventArgs args)
		{
			if (XmlAttributeAdded != null)
				XmlAttributeAdded(sender, args);
		}
		#endregion
	}

	/// <summary>
	/// Delegate specifying how the events raised by the xml library should be handled.
	/// </summary>
	public delegate void XmlToTypeMappingEventHandler(object sender,
		XmlToFieldMappingEventArgs args);

}
