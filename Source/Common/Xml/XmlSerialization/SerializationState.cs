using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Diagnostics;
using System.Text;

namespace BioRad.Common.Xml
{
	#region Documentation Tags
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// 
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:</item>
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
	///			<item name="vssfile">$Workfile: SerializationState.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Xml/XmlSerialization/SerializationState.cs $</item>
	///			<item name="vssrevision">$Revision: 8 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Thouser $</item>
	///			<item name="vssdate">$Date: 5/11/10 10:53a $</item>
	///		</list>
	/// </archiveinformation>

	#endregion
	public class SerializationState
	{
		#region Constants
		private const int c_DictionarySerializationVersion = 1;
		private const int c_ArraySerializationVersion = 1;
		private const string c_ArraySerializationId = "Ar";
		private const int c_PackedArraySerializationVersion = 1;
		private const string c_PackedArraySerializationId = "PAr";
		private const char c_PackedArrayElementDelimiter = ';';
		private const string c_VersionAttributeId = "V";
		private const string c_ArrayElementId = "I";
		private const string c_TypeInfoId = "___TypeInfo";
		private const string c_NullElementName = "_Null";
		#endregion

		#region Members
		private Dictionary<string, object> m_State = new Dictionary<string, object>();
		#endregion

		#region Constructors
		/// <summary></summary>
		public SerializationState()
		{
		}
		#endregion

		#region Static Methods
		/// <summary></summary>
		/// <param name="xml"></param>
		/// <param name="name"></param>
		/// <param name="checkName"></param>
		/// <returns></returns>
		public static SerializationState CreateFromXml(string xml, string name, bool checkName)
		{
			SerializationState state = new SerializationState();
			state.FromXml(xml, name, checkName);
			return state;
		}
		/// <summary></summary>
		/// <param name="xml"></param>
		/// <param name="name"></param>
		/// <param name="checkName"></param>
		/// <returns></returns>
		public static SerializationState CreateFromXml(Stream xml, string name, bool checkName)
		{
			SerializationState state = new SerializationState();
			state.FromXml(xml, name, checkName);
			return state;
		}
		/// <summary></summary>
		/// <param name="xml"></param>
		/// <param name="name"></param>
		/// <param name="checkName"></param>
		/// <returns></returns>
		public static SerializationState CreateFromXml(XElement xml, string name, bool checkName)
		{
			SerializationState state = new SerializationState();
			state.FromXElement(xml, name, checkName);
			return state;
		}
		/// <summary></summary>
		/// <param name="element"></param>
		/// <param name="type"></param>
		private static void AddTypeInfoToXElement(XElement element, Type type)
		{
			string[] typeInfoStrings = new string[2];
			typeInfoStrings[0] = XMLUtility.StripEverythingButAssemblyNameFromAssemblyFullName(type.Assembly.FullName);
			typeInfoStrings[1] = type.FullName;
			string typeInfoString = string.Join(";", typeInfoStrings);
			element.Add(new XElement(c_TypeInfoId, typeInfoString));
		}
		/// <summary></summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static Type GetTypeInfoFromXElement(XElement element)
		{
			List<XElement> typeInfoElements = new List<XElement>(element.Elements(c_TypeInfoId));
			if (typeInfoElements.Count == 0)
				return null;
			if (typeInfoElements.Count > 1)
				throw new XmlSerializationException("type info error");
			XElement typeInfoElement = typeInfoElements[0];
			string typeInfoString = (string)typeInfoElement.Value;
			string[] typeInfoStrings = typeInfoString.Split(';');
			typeInfoStrings[0] = XMLUtility.StripEverythingButAssemblyNameFromAssemblyFullName(typeInfoStrings[0]);
			string fullyQualifiedTypeName = string.Join(", ", new string[] { typeInfoStrings[1], typeInfoStrings[0] });
			Type type = Type.GetType(fullyQualifiedTypeName, false);
			return type;
		}
		#endregion

		#region Methods
		/// <summary></summary>
		/// <returns></returns>
		public int GetCount()
		{
			return m_State.Count;
		}
		/// <summary></summary>
		/// <param name="name"></param>
		/// <param name="type">type that this object encodes.  Can be null.  If null, dynamic instance creation based on the xml will
		/// throw.</param>
		/// <returns></returns>
		public string ToXml(string name, Type type)
		{
			return ToXElement(name, type).ToString(SaveOptions.DisableFormatting);
		}
		/// <summary></summary>
		/// <param name="xml"></param>
		/// <param name="name"></param>
		/// <param name="checkName"></param>
		/// <returns></returns>
		public void FromXml(string xml, string name, bool checkName)
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.CloseInput = true;
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			settings.IgnoreComments = true;
			settings.IgnoreWhitespace = true;
			using (StringReader sr = new StringReader(xml))
			{
				using (XmlReader reader = XmlReader.Create(sr, settings))
				{
					FromXmlReader(reader, new string[] { name }, checkName);
				}
			}
		}
		/// <summary></summary>
		/// <param name="xml"></param>
		/// <param name="name"></param>
		/// <param name="checkName"></param>
		public void FromXml(Stream xml, string name, bool checkName)
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.CloseInput = false;
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			settings.IgnoreComments = true;
			settings.IgnoreWhitespace = true;
			using (XmlReader reader = XmlReader.Create(xml, settings))
			{
				FromXmlReader(reader, new string[] { name }, checkName);
			}
		}
		/// <summary></summary>
		/// <param name="reader"></param>
		/// <param name="names"></param>
		/// <param name="checkName"></param>
		private void FromXmlReader(XmlReader reader, IList<string> names, bool checkName)
		{
			XElement element = XElement.Load(reader, LoadOptions.None);
			FromXElement(element, names, true);
		}
		/// <summary>Transform this SerializationState to an XElement.</summary>
		/// <param name="name">Id of the object who's state is contained by this SerializationState</param>
		/// <param name="type">The type of object who's state is contained by this SerializationState</param>
		/// <returns>XElement representing this SerializationState</returns>
		public XElement ToXElement(string name, Type type)
		{
			XElement xe = new XElement(
				name,
				new XAttribute(
					c_VersionAttributeId,
					c_DictionarySerializationVersion));

			foreach (string key in m_State.Keys)
			{
				object value = m_State[key];
				Debug.Assert(value is string || value is XElement, "Value must be string or XElement.");
				xe.Add(new XElement(key, value));
			}

			if (type != null)
				AddTypeInfoToXElement(xe, type);

			return xe;
		}
		/// <summary></summary>
		/// <param name="element"></param>
		/// <param name="name"></param>
		/// <param name="checkName"></param>
		public void FromXElement(XElement element, string name, bool checkName)
		{
			FromXElement(element, new string[] { name }, checkName);
		}
		/// <summary>recreate the SerializationState encoded by the given XElement, as created by ToXElement(...).</summary>
		/// <param name="element">element as returned by ToXElement(...).</param>
		/// <param name="names">Id of the object encoded by the XElement, as passed to ToXElement().</param>
		/// <param name="checkName">Whether to check the name or ignore it.  If true, an exception will be thrown if one
		/// of the provided names does not match the name in the XElement.  If false, no check is performed.</param>
		public void FromXElement(XElement element, ICollection<string> names, bool checkName)
		{
			if (checkName && names.Contains(element.Name.LocalName) == false)
			{
				throw new XmlSerializationException(element.Name.LocalName);
			}

			m_State = new Dictionary<string, object>();
	
			foreach (XElement childElement in element.Elements())
			{
				if (childElement.HasElements)
				{
					List<XElement> grandChildElements = new List<XElement>(childElement.Elements());
					if (grandChildElements.Count != 1)
						throw new XmlSerializationException("object nodes must be one element.");
					m_State[childElement.Name.ToString()] = grandChildElements[0];
					continue;
				}
				else
					m_State[childElement.Name.ToString()] = childElement.Value;
			}
		}
		/// <summary></summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ContainsKey(string key)
		{
			return m_State.ContainsKey(key);
		}
		/// <summary>Remove a piece of data from this SerializationState.</summary>
		/// <param name="key">key for the data to remove.</param>
		public void Remove(string key)
		{
			m_State.Remove(key);
		}
		/// <summary>Returns a list of all the keys stored in the state.</summary>
		/// <returns></returns>
		public List<string> GetKeys()
		{
			return new List<string>(m_State.Keys);
		}
		#endregion

		#region Add/Get Methods
		/// <summary></summary>
		/// <param name="id"></param>
		/// <param name="a"></param>
		public void AddInt(string id, int a)
		{
			m_State[id] = XMLUtility.IntToXml(a);
		}
		/// <summary></summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public int GetInt(string id)
		{
			return XMLUtility.XmlToInt((string)m_State[id]);
		}

		/// <summary></summary>
		/// <param name="id"></param>
		/// <param name="a"></param>
		public void AddDouble(string id, double a)
		{
			m_State[id] = XMLUtility.DoubleToXml(a);
		}
		/// <summary></summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public double GetDouble(string id)
		{
			return XMLUtility.XmlToDouble((string)m_State[id]);
		}

        /// <summary></summary>
        /// <param name="id"></param>
        /// <param name="a"></param>
	    public void AddFloat(string id, float a)
	    {
            m_State[id] = XMLUtility.FloatToXml(a);
	    }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <returns></returns>
	    public float GetFloat(string id)
	    {
            return XMLUtility.XmlToFloat((string)m_State[id]);
	    }

		/// <summary></summary>
		/// <param name="id"></param>
		/// <param name="a"></param>
		public void AddString(string id, string a)
		{
			m_State[id] = a;
		}
		/// <summary></summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public string GetString(string id)
		{
			return (string)m_State[id];
		}

		/// <summary></summary>
		/// <param name="id"></param>
		/// <param name="a"></param>
		public void AddBool(string id, bool a)
		{
			m_State[id] = XMLUtility.BoolToXml(a);
		}
		/// <summary></summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool GetBool(string id)
		{
			return XMLUtility.XmlToBool((string)m_State[id]);
		}

		/// <summary></summary>
		/// <param name="id"></param>
		/// <param name="a"></param>
		public void AddDateTime(string id, DateTime a)
		{
			m_State[id] = XMLUtility.DateTimeToXml(a);
		}
		/// <summary></summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public DateTime GetDateTime(string id)
		{
			return XMLUtility.XmlToDateTime((string)m_State[id]);
		}

		/// <summary></summary>
		/// <param name="id"></param>
		/// <param name="element"></param>
		public void AddXElement(string id, XElement element)
		{
			m_State[id] = element;
		}
		/// <summary></summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public XElement GetXElement(string id)
		{
			return (XElement)m_State[id];
		}

		/// <summary></summary>
		/// <param name="id"></param>
		/// <param name="a"></param>
		public void AddInts(string id, IList<int> a)
		{
			StringBuilder sb = new StringBuilder();
			foreach (int i in a)
				sb.Append(XMLUtility.IntToXml(i)).Append(';');
			if (sb.Length > 0)
				sb.Remove(sb.Length - 1, 1);
			m_State[id] = sb.ToString();
		}
		/// <summary></summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public List<int> GetInts(string id)
		{
			string intsAsString = (string)(m_State[id]);
			string[] intsAsStrings = new string[0];
			if (intsAsString.Length > 0)
				intsAsStrings = intsAsString.Split(';');
			List<int> ints = new List<int>();
			foreach (string s in intsAsStrings)
				ints.Add(XMLUtility.XmlToInt(s));
			return ints;
		}

		/// <summary></summary>
		/// <param name="id"></param>
		/// <param name="a"></param>
		public void AddBools(string id, IList<bool> a)
		{
			StringBuilder sb = new StringBuilder();
			foreach (bool i in a)
				sb.Append(XMLUtility.BoolToXml(i)).Append(';');
			if (sb.Length > 0)
				sb.Remove(sb.Length - 1, 1);
			m_State[id] = sb.ToString();
		}
		/// <summary></summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public List<bool> GetBools(string id)
		{
			string boolsAsString = (string)(m_State[id]);
			string[] boolsAsStrings = new string[0];
			if (boolsAsString.Length > 0)
				boolsAsStrings = boolsAsString.Split(';');
			List<bool> bools = new List<bool>();
			foreach (string s in boolsAsStrings)
				bools.Add(XMLUtility.XmlToBool(s));
			return bools;
		}

		/// <summary></summary>
		/// <param name="id"></param>
		/// <param name="a"></param>
		public void AddFloats(string id, IList<float> a)
		{
			StringBuilder sb = new StringBuilder();
			foreach (float i in a)
				sb.Append(XMLUtility.FloatToXml(i)).Append(';');
			if (sb.Length > 0)
				sb.Remove(sb.Length - 1, 1);
			m_State[id] = sb.ToString();
		}
		/// <summary></summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public List<float> GetFloats(string id)
		{
			string floatsAsString = (string)(m_State[id]);
			string[] floatsAsStrings = new string[0];
			if (floatsAsString.Length > 0)
				floatsAsStrings = floatsAsString.Split(';'); 
			List<float> floats = new List<float>();
			foreach (string s in floatsAsStrings)
				floats.Add(XMLUtility.XmlToFloat(s));
			return floats;
		}

		/// <summary></summary>
		/// <param name="id"></param>
		/// <param name="a"></param>
		public void AddDoubles(string id, IList<double> a)
		{
			StringBuilder sb = new StringBuilder();
			foreach (double i in a)
				sb.Append(XMLUtility.DoubleToXml(i)).Append(';');
			if (sb.Length > 0)
				sb.Remove(sb.Length - 1, 1);
			m_State[id] = sb.ToString();
		}
		/// <summary></summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public List<double> GetDoubles(string id)
		{
			string doublesAsString = (string)(m_State[id]);
			string[] doublesAsStrings = new string[0];
			if (doublesAsString.Length > 0)
				doublesAsStrings = doublesAsString.Split(';'); 
			List<double> doubles = new List<double>();
			foreach (string s in doublesAsStrings)
				doubles.Add(XMLUtility.XmlToDouble(s));
			return doubles;
		}

		/// <summary></summary>
		/// <param name="id"></param>
		/// <param name="a"></param>
		public void AddStrings(string id, IEnumerable<string> a)
		{
			XElement element = new XElement(c_ArraySerializationId);
			element.Add(new XAttribute(c_VersionAttributeId, XMLUtility.IntToXml(c_ArraySerializationVersion)));
			foreach (string s in a)
				element.Add(new XElement(c_ArrayElementId, s));
			m_State[id] = element;
		}
		/// <summary></summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public List<string> GetStrings(string id)
		{
			XElement element = (XElement)(m_State[id]);
			if (string.Compare(element.Name.LocalName, c_ArraySerializationId, false) != 0)
				throw new XmlSerializationException("wrong id for serialized array.");
			int version = XMLUtility.XmlToInt(element.Attribute(c_VersionAttributeId).Value);

			if (version == 1)
			{
				List<string> strings = new List<string>();
				foreach (XElement item in element.Elements())
					strings.Add(item.Value);
				return strings;
			}
			else
			{
				throw new XmlSerializationException("Unknown version.");
			}
		}

		/// <summary></summary>
		/// <param name="id"></param>
		/// <param name="strings"></param>
		public void AddPackedStrings(string id, IEnumerable<string> strings)
		{
			char[] illegalValueChars = new char[]{
					'\r',
					'\n',
					'&',
					'<',
					'>', 
					c_PackedArrayElementDelimiter};

			// Verify that the strings are legal
			foreach (string s in strings)
			{
				if (s.IndexOfAny(illegalValueChars) >= 0)
				{
					// Note to calling code: call AddStrings() instead.
					Debug.Assert(false);
					AddStrings(id, strings);
				}
			}

			// Create the packed string
			StringBuilder packedArray = new StringBuilder();
			foreach (string s in strings)
			{
				packedArray.Append(s);
				packedArray.Append(c_PackedArrayElementDelimiter);
			}
			// Remove the final delimiter, if it exists.
			if (packedArray.Length > 0)
				packedArray.Remove(packedArray.Length - 1, 1);

			XElement element = new XElement(c_PackedArraySerializationId);
			element.Add(new XAttribute(c_VersionAttributeId, XMLUtility.IntToXml(c_PackedArraySerializationVersion)));
			element.Value = packedArray.ToString();

			m_State[id] = element;
		}
		/// <summary></summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public List<string> GetPackedStrings(string id)
		{
			XElement element = (XElement)(m_State[id]);

			if (element.Name.LocalName.Equals(c_ArraySerializationId))
			{
				// This is a non-packed collection of strings as from StringsToXml().
				return GetStrings(id);
			}
			// Ensure that this is a packed collection of strings.
			if (element.Name.LocalName.Equals(c_PackedArraySerializationId) == false)
			{
				Debug.Assert(false);
				return null;
			}
			int version = XMLUtility.XmlToInt(element.Attribute(c_VersionAttributeId).Value);
			if (version == 1)
			{
				List<string> strings = new List<string>();
				string packedStrings = element.Value;
				string[] unpackedStrings = packedStrings.Split(c_PackedArrayElementDelimiter);
				strings = new List<string>(unpackedStrings);
				return strings;
			}
			else
			{
				Debug.Assert(false);
				return null;
			}
		}

		/// <summary>Adds an IBioRadXmlSerializable object to this state object.</summary>
		/// <param name="id">id for this object</param>
		/// <param name="a">object to be added</param>
		public void AddIBioRadXmlSerializable(string id, IBioRadXmlSerializable a)
		{
			XElement element = a.ToXElement();
			element.Element(c_TypeInfoId).Remove();
			m_State[id] = element;
		}
		/// <summary>Gets a deserialized IBioRadXmlSerializable object, as added by AddIBioRadXmlSerializable().</summary>
		/// <param name="id">id for this object, as passed to the Add method.</param>
		/// <param name="type">Type of the object to get.</param>
		/// <returns>The object.</returns>
		public IBioRadXmlSerializable GetIBioRadXmlSerializable(string id, Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type cannot be null.");

			XElement stateElement = (XElement)(m_State[id]);

			IBioRadXmlSerializable newObject = Activator.CreateInstance(type, stateElement) as IBioRadXmlSerializable;
			if (newObject == null)
				throw new XmlSerializationException("cannot create new instance");
			return newObject;
		}
		/// <summary>Adds a collection of IBioRadXmlSerializable objects.  Any or all elements can be null.</summary>
		/// <param name="id"></param>
		/// <param name="a"></param>
		public void AddIBioRadXmlSerializables(string id, IList<IBioRadXmlSerializable> a)
		{
			AddIBioRadXmlSerializables_Aux(id, a, false);
		}
		/// <summary>Adds a collection of IBioRadXmlSerializable objects.  Any or all elements can be null.</summary>
		/// <param name="id"></param>
		/// <param name="a"></param>
		public void AddIBioRadXmlSerializables_Typed(string id, IList<IBioRadXmlSerializable> a)
		{
			AddIBioRadXmlSerializables_Aux(id, a, true);
		}
		private void AddIBioRadXmlSerializables_Aux(string id, IList<IBioRadXmlSerializable> a, bool addTypeInfo)
		{
			XElement e = new XElement(c_ArraySerializationId); 

			foreach (IBioRadXmlSerializable x in a)
			{
				if (x == null)
				{
					e.Add(new XElement(c_NullElementName));
				}
				else
				{
					XElement elem = x.ToXElement();
					if (addTypeInfo == false)
						elem.Element(c_TypeInfoId).Remove();
					e.Add(elem);
				}
			}

			AddXElement(id, e);
		}
		/// <summary></summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public List<IBioRadXmlSerializable> GetIBioRadXmlSerializables_Typed(string id)
		{
			return GetIBioRadXmlSerializables_Aux(id, null);
		}
		/// <summary></summary>
		/// <param name="id"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public List<IBioRadXmlSerializable> GetIBioRadXmlSerializables(string id, Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type cannot be null.");

			return GetIBioRadXmlSerializables_Aux(id, type);			
		}
		private List<IBioRadXmlSerializable> GetIBioRadXmlSerializables_Aux(string id, Type type)
		{
			XElement e = (XElement)(m_State[id]);
			List<IBioRadXmlSerializable> objects = new List<IBioRadXmlSerializable>();
			foreach (XElement child in e.Elements())
			{
				if (child.Name == c_NullElementName)
				{
					objects.Add(null);
					continue;
				}
				IBioRadXmlSerializable newObject = null;
				if (type == null)
					newObject = XMLUtility.CreateInstanceFromTypedXml(child);
				else
					newObject = Activator.CreateInstance(type, child) as IBioRadXmlSerializable;
				if (newObject == null)
					throw new XmlSerializationException("cannot create new instance");
				objects.Add(newObject);
			}
			return objects;
		}
		/// <summary>Adds an IBioRadXmlSerializable object to this state object.  The type name of the object is stored,
		/// and used when deserializing the object.  This makes the Xml slightly larger, and causes class name changes to
		/// break deserialization (the object will not be able to be instantiated by the framework since the name of the
		/// assembly and class will no longer refer to an existing object).  This allows deserialization
		/// of the object by code which does not know the exact type of the object.</summary>
		/// <param name="id"></param>
		/// <param name="a"></param>
		public void AddIBioRadXmlSerializable_Typed(string id, IBioRadXmlSerializable a)
		{
			m_State[id] = a.ToXElement();
		}
		/// <summary>Retrieves a deserialized IBioRadXmlSerializable object.</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IBioRadXmlSerializable GetIBioRadXmlSerializable_Typed(string id)
		{
			XElement stateElement = (XElement)(m_State[id]);
			return XMLUtility.CreateInstanceFromTypedXml(stateElement);
		}

		/// <summary>Adds a SerializationState object.</summary>
		/// <param name="id"></param>
		/// <param name="a"></param>
		public void AddSerializationState(string id, SerializationState a)
		{
			m_State[id] = a.ToXElement("State", null);
		}
		/// <summary>Gets a SerializationState object.</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public SerializationState GetSerializationState(string id)
		{
			XElement element = (XElement)m_State[id];
			SerializationState ss = SerializationState.CreateFromXml(element, "State", true);
			return ss;
		}
		#endregion
	}
}
