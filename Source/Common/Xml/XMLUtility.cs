using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using BioRad.Common.Common;

namespace BioRad.Common.Xml
{
	#region Documentation Tags
	/// <summary>Utility class to help in XML Read / Write operations.
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Shabnam</item>
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
	///			<item name="vssfile">$Workfile: XMLUtility.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Xml/XMLUtility.cs $</item>
	///			<item name="vssrevision">$Revision: 21 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Thouser $</item>
	///			<item name="vssdate">$Date: 5/14/10 11:00a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class XMLUtility
	{
		#region Constructors
		private static List<Duple<string, string>> s_EncodingPairs;
		/// <summary>Static constructor.</summary>
		static XMLUtility()
		{
			s_EncodingPairs = new List<Duple<string, string>>();
			s_EncodingPairs.Add(new Duple<string, string>("<", "&lt;"));
			s_EncodingPairs.Add(new Duple<string, string>(">", "&gt;"));
			s_EncodingPairs.Add(new Duple<string, string>("&", "&amp;"));
			s_EncodingPairs.Add(new Duple<string, string>("'", "&apos;"));
			s_EncodingPairs.Add(new Duple<string, string>("\"", "&quot;"));
		}
		#endregion Constructors

		#region Methods
		/// <summary>
		/// The first step is to define a method that verifies that the characters in a 
		/// string are valid characters, according to the W3C specification. 
		/// This method is used to verify that the strings written out by the writer are correct. 
		/// The specific tests done on the character strings are: 
		/// <list type="bullet">
		/// <item>
		/// Verify that no character has a hex value greater than 0xFFFD, or less than 0x20.
		/// </item>
		/// <item>
		/// Check that the character is not equal to the tab (\t), the newline (\n), the carriage return (\r), 
		/// or is an invalid XML character below the range of 0x20. 
		/// If any of these characters occur, an exception is thrown. 
		/// </item>
		/// </list>
		/// </summary>
		/// <param name="stringValue">The string to check for invalid
		/// Unicode characters</param>
		public static string CheckUnicodeString(string stringValue)
		{
			string newString = System.Security.SecurityElement.Escape(stringValue);
            for (int i = 0; i < newString.Length; ++i)
			{
                if (newString[i] > 0xFFFD)
				{
					throw new Exception("Invalid Unicode");
				}
                else if (newString[i] < 0x20 && newString[i] != '\t' &
                    newString[i] != '\n' & newString[i] != '\r')
				{
					throw new Exception("Invalid Xml Characters");
				}
                //else if (stringValue[i] == '�')//DEGREE_SYMBOL 176 = 0xb0
				//{
				//	throw new Exception("Invalid Xml Characters (0xb0)");
				//}
			}
            return newString;
		}

		/// <summary>Reads an attribute from the passed in node.</summary>
		/// <param name="node">The XmlNode to read from.</param>
		/// <param name="attributeName">The name of the attribute.</param>
		/// <returns>The attribute's value.</returns>
		public static string ReadAttribute(XmlNode node, string attributeName)
		{
			XmlAttribute attr = node.Attributes[attributeName];
			if (attr == null)
			{
                //PW: An exception here causes thousands of exceptions slowing down the reading.
                //Let the caller handle the empty string rather.
                /*
                throw new XmlException
						  (StringUtility.FormatString(Properties.Resources.XmlAttributeNotFound_2,
					node.Name.Trim(), attributeName));                */
                return String.Empty; 
			}

			return attr.Value.Trim();
		}
		/// <summary>Reads an attribute from the current position using the 
		/// XmlTextReader object.</summary>
		/// <param name="reader">The XmlTextReader object to use.</param>
		/// <param name="attributeName">The name of the attribute.</param>
		/// <param name="nodeName">The node to which the attribute belongs.</param>
		/// <returns>The attribute's value.</returns>
		public static string ReadAttribute(XmlTextReader reader, string attributeName,
			string nodeName)
		{
			string text = null;
			text = reader.GetAttribute(attributeName);
			if (text == null)	// the attribute was not found
			{
                //PW: An exception here causes thousands of exceptions slowing down the reading.
                //Let the caller handle the empty string rather.
                /*
                throw new XmlException
						  (StringUtility.FormatString(Properties.Resources.XmlAttributeNotFound_2,
					nodeName, attributeName));
                */
                return String.Empty;
			}
			else
			{
				return text;
			}
		}

		/// <summary>Reads an attribute from the current position using the 
		/// XmlTextReader object.</summary>
		/// <param name="reader">The XmlReader object to use.</param>
		/// <param name="attributeName">The name of the attribute.</param>
		/// <param name="nodeName">The node to which the attribute belongs.</param>
		/// <returns>The attribute's value.</returns>
		public static string ReadAttribute(XmlReader reader, string attributeName,
			string nodeName)
		{
			string text = null;
			text = reader.GetAttribute(attributeName);
			if (text == null)	// the attribute was not found
			{
                //PW: An exception here causes thousands of exceptions slowing down the reading.
                //Let the caller handle the empty string rather.
                /*
                throw new XmlException
						  (StringUtility.FormatString(Properties.Resources.XmlAttributeNotFound_2,
					nodeName, attributeName));
                */
                return String.Empty;
			}
			else
			{
				return text;
			}
		}
		/// <summary>Decodes a value as encoded by EncodeAttributeValue.</summary>
		/// <param name="encodedValue"></param>
		/// <returns></returns>
		public static string DecodeAttributeValue(string encodedValue)
		{
			if (encodedValue == null)
				return null;
			foreach (Duple<string, string> encodingPair in s_EncodingPairs)
				encodedValue = encodedValue.Replace(encodingPair.Item2, encodingPair.Item1);
			return encodedValue;
		}
		/// <summary>Encodes a string so that it is suitable for use as an attribute value.  Uses &lt; in place of less-than, etc.</summary>
		/// <param name="valueToEncode"></param>
		/// <returns></returns>
		public static string EncodeAttributeValue(string valueToEncode)
		{
			if (valueToEncode == null)
				return null;
			StringBuilder encodedBuilder = new StringBuilder();
			foreach (char c in valueToEncode)
			{
				bool found = false;
				foreach (Duple<string, string> encodingPair in s_EncodingPairs)
				{
					if (c == encodingPair.Item1[0])
					{
						encodedBuilder.Append(encodingPair.Item2);
						found = true;
						break;
					}
				}
				if (found == false)
					encodedBuilder.Append(c);
			}
			return encodedBuilder.ToString();
		}
		#region Xml serialization utility methods

		private const int c_DictionarySerializationVersion = 1;
		private const int c_ArraySerializationVersion = 1;
		private const string c_ArraySerializationId = "Ar";
		private const int c_PackedArraySerializationVersion = 1;
		private const string c_PackedArraySerializationId = "PAr";
		private const string c_VersionAttributeId = "V";
		private const string c_ArrayElementId = "I";

		/// <summary>
		/// Returns an xml node representing this string-to-string dictionary.  The dictionary's values must be valid 
		/// Xml element values.
		/// </summary>
		/// <param name="dic"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string DictionaryToXml(ref Dictionary<string, string> dic, string name)
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			StringBuilder xmlBuilder = new StringBuilder();

			using (XmlWriter writer = XmlWriter.Create(xmlBuilder, settings))
			{
				writer.WriteStartElement(name);
				writer.WriteAttributeString(c_VersionAttributeId,
						IntToXml(c_DictionarySerializationVersion));
				foreach (string key in dic.Keys)
				{
					string elementName = XmlConvert.EncodeLocalName(key);
					writer.WriteStartElement(elementName);
                    writer.WriteRaw(dic[key]);
                    writer.WriteEndElement();
				}
				writer.WriteEndElement();
				writer.Close();
			}
			return xmlBuilder.ToString();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string DictionaryToXmlFriendly(ref Dictionary<string, string> dic, string name)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            StringBuilder xmlBuilder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(xmlBuilder, settings))
            {
                writer.WriteStartElement(name);
                writer.WriteAttributeString(c_VersionAttributeId,
                        IntToXml(c_DictionarySerializationVersion));
                foreach (string key in dic.Keys)
                {
                    string elementName = XmlConvert.EncodeLocalName(key);
                    writer.WriteStartElement(elementName);

                    string x = XMLUtility.StringToXml(dic[key]);
                    writer.WriteRaw(x);

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Close();
            }
            return xmlBuilder.ToString();
        }

        /// <summary>
        /// Returns an xml node representing this string-to-string dictionary.  The dictionary's values must be valid 
        /// Xml element values.
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string DictionaryToAttributesXml(ref Dictionary<string, string> dic, string name)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            StringBuilder xmlBuilder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(xmlBuilder, settings))
            {
                writer.WriteStartElement(name);
                foreach (string key in dic.Keys)
                {
                    string attributeName = XmlConvert.EncodeLocalName(key);
                    writer.WriteAttributeString(attributeName, dic[key]);
                }
                writer.WriteEndElement();
                writer.Close();
            }
            return xmlBuilder.ToString();
        }

        /// <summary>
        /// Returns a string-to-string Dictionary from an xml string as returned by DictionaryToXml.
        /// </summary>
        /// <param name="xml">xml</param>
        /// <param name="name">name of this encoding (as given to DictionaryToXml)</param>
        /// <returns>dictionary or null if failure.</returns>
        public static Dictionary<string, string> XmlToAttributesDictionary(string xml, string name)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            Dictionary<string, string> dict = null;
            using (XmlReader reader = XmlReader.Create(new StringReader(xml), settings))
            {
                reader.Read();
                if (string.Compare(name, reader.Name) != 0)
                {
                    throw new XmlSerializationException(string.Format("Incorrect serialization id. Expected \"{0}\", found \"{1}\".", name, reader.Name));
                }

                if (!reader.HasAttributes)
                    return dict;

                if (reader.MoveToFirstAttribute())
                    dict.Add(reader.Name, reader.Value);
                while (reader.MoveToNextAttribute())
                {
                    dict.Add(reader.Name, reader.Value);
                }

            }

            return dict;
        }

        ///
	    public static XElement ListToXml<T>(List<T> list, string name)
        {
            if (list == null || list.Count == 0) return new XElement(name, string.Empty);
            var doc = new XDocument();
            using (XmlWriter writer = doc.CreateWriter())
            {
                XmlSerializer serializer = new XmlSerializer(list.GetType());
                serializer.Serialize(writer, list);
            }

            return new XElement(name, doc.Root.Elements());
        }

        ///
	    public static List<T> XmlToList<T>(XElement elem, string name)
        {
            if (elem == null || elem.IsEmpty || ! elem.HasElements) return null;
            var objs = elem.Elements(name);
            var serializer = new XmlSerializer(typeof(T));
            return objs.Select(c => (T)serializer.Deserialize(c.CreateReader())).ToList();
        }

        /// <summary>
        /// Returns an xml node representing this string-to-string dictionary.  The dictionary's values must be valid 
        /// Xml element values.
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ListTupleToXml(ref List<Tuple<string, string>> dic, string name)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            StringBuilder xmlBuilder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(xmlBuilder, settings))
            {
                writer.WriteStartElement(name);
                writer.WriteAttributeString(c_VersionAttributeId,
                        IntToXml(c_DictionarySerializationVersion));
                foreach (Tuple<string, string> t in dic)
                {
                    string elementName = XmlConvert.EncodeLocalName(t.Item1);
                    writer.WriteStartElement(elementName);
                    writer.WriteRaw(t.Item2);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Close();
            }
            return xmlBuilder.ToString();
        }
        /// <summary>
        /// Returns a string-to-string Dictionary from an xml string as returned by DictionaryToXml.
        /// </summary>
        /// <param name="xml">xml</param>
        /// <param name="name">name of this encoding (as given to DictionaryToXml)</param>
        /// <returns>dictionary or null if failure.</returns>
        public static List<Tuple<string, string>> XmlToListDictionary(string xml, string name)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            List<Tuple<string, string>> dict = null;
            using (XmlReader reader = XmlReader.Create(new StringReader(xml), settings))
            {
                reader.Read();
                if (string.Compare(name,reader.Name) != 0)
                {
                    throw new XmlSerializationException(string.Format("Incorrect serialization id. Expected \"{0}\", found \"{1}\".", name, reader.Name));
                }
                string versionString = reader.GetAttribute(c_VersionAttributeId);
                int version = XmlToInt(versionString);
                dict = new List<Tuple<string, string>>();
                if (version == 1)
                {
                    if (reader.IsEmptyElement)
                        return dict;
                    reader.ReadStartElement();
                    while (reader.NodeType != XmlNodeType.EndElement)
                    {
                        string elementName = reader.Name;
                        string key = XmlConvert.DecodeName(elementName);
                        string value = reader.ReadInnerXml();
                        dict.Add(new Tuple<string, string>(key, value));
                    }
                }
                else
                {
                    Debug.Assert(false);// todo error handling.
                    return null;
                }
            }

            return dict;
        }
		/// <summary>
		/// Returns a string-to-string Dictionary from an xml string as returned by DictionaryToXml.
		/// </summary>
		/// <param name="xml">xml</param>
		/// <param name="name">name of this encoding (as given to DictionaryToXml)</param>
		/// <returns>dictionary or null if failure.</returns>
		public static Dictionary<string, string> XmlToDictionary(string xml, string name)
		{
			return XmlToDictionary(xml, new string[] { name }, true);
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Dictionary<string, string> XmlToDictionaryFriendly(string xml, string name)
        {
            return XmlToDictionaryFriendly(xml, new string[] { name }, true);
        }
        /// <summary>
        /// Returns a string-to-string Dictionary from an xml string as returned by DictionaryToXml.
        /// </summary>
        /// <param name="xml">xml</param>
        /// <param name="names">names of this encoding (as given to DictionaryToXml), one of which must be valid.  Use this if the serialization
        /// id has changed via versioning, and pass the current and all previous names.</param>
        /// <returns>dictionary or null if failure.</returns>
        public static Dictionary<string, string> XmlToDictionary(string xml, IList<string> names)
		{
			return XmlToDictionary(xml, names, true);
		}
		/// <summary>
		/// Returns a string-to-string Dictionary from an xml string as returned by DictionaryToXml.
		/// </summary>
		/// <param name="xml">xml</param>
		/// <param name="names">valid names of this encoding (as given to DictionaryToXml).  Can be null if checkName is false.</param>
		/// <param name="checkName">Whether to validate the name against the given name.  If false, then name is allowed
		/// to be null.</param>
		/// <returns>dictionary or null if failure.</returns>
		private static Dictionary<string, string> XmlToDictionary(string xml, IList<string> names, bool checkName)
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			settings.IgnoreWhitespace = true;
			settings.IgnoreComments = true;
			Dictionary<string, string> dict = null;
			using (XmlReader reader = XmlReader.Create(new StringReader(xml), settings))
			{
				reader.Read();
				if (checkName && names.Contains(reader.Name) == false)
				{
					throw new XmlSerializationException(string.Format("Incorrect serialization id. Expected \"{0}\", found \"{1}\".", names[0], reader.Name));
				}
				string versionString = reader.GetAttribute(c_VersionAttributeId);
				int version = XmlToInt(versionString);
				dict = new Dictionary<string, string>();
				if (version == 1)
				{
					if (reader.IsEmptyElement)
						return dict;
					reader.ReadStartElement();
					while (reader.NodeType != XmlNodeType.EndElement)
					{
						string elementName = reader.Name;
						string key = XmlConvert.DecodeName(elementName);
						string value = reader.ReadInnerXml();
						dict[key] = value;
					}
				}
				else
				{
					Debug.Assert(false);// todo error handling.
					return null;
				}
			}

			return dict;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="names"></param>
        /// <param name="checkName"></param>
        /// <returns></returns>
        private static Dictionary<string, string> XmlToDictionaryFriendly(string xml, IList<string> names, bool checkName)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            Dictionary<string, string> dict = null;
            using (XmlReader reader = XmlReader.Create(new StringReader(xml), settings))
            {
                reader.Read();
                if (checkName && names.Contains(reader.Name) == false)
                {
                    throw new XmlSerializationException(string.Format("Incorrect serialization id. Expected \"{0}\", found \"{1}\".", names[0], reader.Name));
                }
                string versionString = reader.GetAttribute(c_VersionAttributeId);
                int version = XmlToInt(versionString);
                dict = new Dictionary<string, string>();
                if (version == 1)
                {
                    if (reader.IsEmptyElement)
                        return dict;
                    reader.ReadStartElement();
                    while (reader.NodeType != XmlNodeType.EndElement)
                    {
                        string elementName = reader.Name;
                        string key = XmlConvert.DecodeName(elementName);
                        string value = reader.ReadInnerXml();
                        dict[key] = XMLUtility.XmlToString(value);
                    }
                }
                else
                {
                    Debug.Assert(false);// todo error handling.
                    return null;
                }
            }

            return dict;
        }
        /// <summary>
        /// Transforms a dictionary[string, int] into an xml value string.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static string DictionaryOfStringIntToXml(Dictionary<string, int> dict)
		{
			List<string> datasetIndexesKeysAsXml = new List<string>();
			List<string> datasetIndexesValuesAsXml = new List<string>();
			foreach (KeyValuePair<string, int> kvp in dict)
			{
				datasetIndexesKeysAsXml.Add(StringToXml(kvp.Key));
				datasetIndexesValuesAsXml.Add(IntToXml(kvp.Value));
			}
			string allKeysInOneString = StringsToXml(datasetIndexesKeysAsXml);
			string allValuesInOneString = StringsToXml(datasetIndexesValuesAsXml);
			string allKvpsInOneString = StringsToXml(new string[] { 
				allKeysInOneString, allValuesInOneString });
			return allKvpsInOneString;
		}
		/// <summary>
		/// Reverses the transform of DictionaryOfStringIntToXml().
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public static Dictionary<string, int> XmlToDictionaryOfStringInt(string xml)
		{
			List<string> keysAndValuesStrings = XmlToStrings(xml);
			List<string> keysStrings = XmlToStrings(keysAndValuesStrings[0]);
			List<string> valuesStrings = XmlToStrings(keysAndValuesStrings[1]);
			Dictionary<string, int> d = new Dictionary<string, int>();
			for (int i = 0; i < keysStrings.Count; i += 1)
			{
				string key = XmlToString(keysStrings[i]);
				int value = XmlToInt(valuesStrings[i]);
				d[key] = value;
			}
			return d;
		}
		/// <summary>
		/// transform int to xml value string.  Culture invariant.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public static string IntToXml(int a)
		{
			return a.ToString(CultureInfo.InvariantCulture);
		}
		/// <summary>
		/// transform a culture invariant xml value string to an int.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static int XmlToInt(string s)
		{
			return Convert.ToInt32(s, CultureInfo.InvariantCulture);
		}
		/// <summary>
		/// transform float to xml value string.  Culture invariant.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public static string FloatToXml(float a)
		{
			return a.ToString(CultureInfo.InvariantCulture);
		}
		/// <summary>
		/// transform a culture invariant xml value string to float.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static float XmlToFloat(string s)
		{
			return Convert.ToSingle(s, CultureInfo.InvariantCulture);
		}
		/// <summary>
		/// Transform double to xml value string. culture invariant.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public static string DoubleToXml(double a)
		{
			return a.ToString(CultureInfo.InvariantCulture);
		}
		private static readonly string c_NanAsString = double.NaN.ToString(CultureInfo.InvariantCulture);
		/// <summary>
		/// Transform a culture invariant xml value string to double.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static double XmlToDouble(string s)
		{
			if (s == c_NanAsString)
				return double.NaN;
			return Convert.ToDouble(s, CultureInfo.InvariantCulture);
		}
		/// <summary>
		/// transform bool to xml value string.  Culture invariant.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public static string BoolToXml(bool a)
		{
			return a.ToString(CultureInfo.InvariantCulture);
		}
		/// <summary>
		/// Transform a culture invariant xml value string to bool.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static bool XmlToBool(string s)
		{
			return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
		}
		/// <summary>
		/// Transform DateTime to xml value string.  Culture invariant.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public static string DateTimeToXml(DateTime a)
		{
			a = a.ToUniversalTime();
			return a.ToString("r", CultureInfo.InvariantCulture);
		}
		/// <summary>
		/// Transform a culture invariant xml value string to DateTime.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static DateTime XmlToDateTime(string s)
		{
			DateTime a = DateTime.Parse(s, CultureInfo.InvariantCulture);
			return a.ToLocalTime();
		}
		/// <summary>
		/// Transforms a string to an xml-friendly text, i.e. replacing less than and greater than 
		/// with appropriate escape sequences etc.
		/// </summary>
		/// <param name="s"></param>
		/// <returns>xml-compatible string</returns>
		public static string StringToXml(string s)
		{
			if (s != null)
			{
				string illegalChar = new string(new char[] { (char)0x1f });
				if (s.Contains(illegalChar))
					s = s.Replace(illegalChar, "");
			}

			XmlWriterSettings settings = new XmlWriterSettings();
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			StringBuilder sb = new StringBuilder();
			using (XmlWriter writer = XmlWriter.Create(sb, settings))
			{
				writer.WriteString(s);
				writer.Flush();
			}
			return sb.ToString();
		}
		/// <summary>
		/// Reverses transformations applied by StringToXml.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns>string as it was originally passed to StringToXml()</returns>
		public static string XmlToString(string xml)
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			string s;
			using (XmlReader reader = XmlReader.Create(new StringReader(xml), settings))
			{
				reader.Read();
				s = reader.ReadString();
			}
			return s;
		}
		/// <summary>
		/// Transforms a collection of strings to an Xml node
		/// </summary>
		/// <param name="strings">string collection</param>
		/// <returns>string containing an Xml node encoding the string collection</returns>
		public static string StringsToXml(IEnumerable<string> strings)
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			StringBuilder xmlBuilder = new StringBuilder();
			using (XmlWriter writer = XmlWriter.Create(xmlBuilder, settings))
			{
				writer.WriteStartElement(c_ArraySerializationId);
				writer.WriteAttributeString(c_VersionAttributeId, IntToXml(c_ArraySerializationVersion));
				foreach (string s in strings)
				{
					writer.WriteStartElement(c_ArrayElementId);
					writer.WriteRaw(s);
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
				writer.Close();
			}
			return xmlBuilder.ToString();
		}
		/// <summary>
		/// Transforms an Xml string into a string array.
		/// </summary>
		/// <param name="xml">Xml string encoding a string array</param>
		/// <returns>string array decoded from xml</returns>
		public static List<string> XmlToStrings(string xml)
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			settings.IgnoreWhitespace = true;
			settings.IgnoreComments = true;
			List<string> strings = null;
			using (XmlReader reader = XmlReader.Create(new StringReader(xml), settings))
			{
				reader.Read();
				if (reader.Name.Equals(c_ArraySerializationId) == false)
				{
					Debug.Assert(false);
					return strings;
				}
				string versionString = reader.GetAttribute(c_VersionAttributeId);
				int version = XmlToInt(versionString);
				if (version == 1)
				{
					strings = new List<string>();
					if (reader.IsEmptyElement)
						return strings;
					reader.ReadStartElement();
					while (reader.NodeType != XmlNodeType.EndElement)
					{
						Debug.Assert(reader.Name.Equals(c_ArrayElementId));
						string value = reader.ReadInnerXml();
						strings.Add(value);
					}
				}
				else
				{
					Debug.Assert(false);
					return strings;
				}
			}

			return strings;
		}
		/// <summary>Delimiter for arrays which are packed into one element.</summary>
		private const char c_PackedArrayElementDelimiter = ';';
		/// <summary>
		/// Attempts to pack a collection of strings into a single delimited Xml element.  The strings
		/// in the collection must not contain the delimiter, which is ';', and they must be valid Xml
		/// values.  If those preconditions are not met, this method will call StringsToXml() and the 
		/// returned Xml string will have one element per string in the collection.
		/// </summary>
		/// <returns>A string containing an Xml element encoding the string collection.</returns>
		public static string StringsToPackedXml(IEnumerable<string> strings)
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
					// Note to calling code: call StringsToXml() instead.
					Debug.Assert(false);
					return StringsToXml(strings);
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

			// Write the packed string to Xml
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			StringBuilder xmlBuilder = new StringBuilder();
			using (XmlWriter writer = XmlWriter.Create(xmlBuilder, settings))
			{
				writer.WriteStartElement(c_PackedArraySerializationId);
				writer.WriteAttributeString(c_VersionAttributeId,
						IntToXml(c_PackedArraySerializationVersion));
				writer.WriteRaw(packedArray.ToString());
				writer.WriteEndElement();
				writer.Close();
			}
			return xmlBuilder.ToString();
		}
		/// <summary>
		/// Transforms an xml string to a collection of strings, reversing StringsToPackedXml()
		/// </summary>
		/// <param name="xml">string as provided by StringsToPackedXml()</param>
		/// <returns>collection of strings</returns>
		public static List<string> PackedXmlToStrings(string xml)
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			settings.IgnoreWhitespace = true;
			settings.IgnoreComments = true;
			List<string> strings = null;
			using (XmlReader reader = XmlReader.Create(new StringReader(xml), settings))
			{
				reader.Read();
				// Test if this is a packed or non-packed collection of strings.
				if (reader.Name.Equals(c_ArraySerializationId))
				{
					// This is a non-packed collection of strings as from StringsToXml().
					return XmlToStrings(xml);
				}
				// Ensure that this is a packed collection of strings.
				if (reader.Name.Equals(c_PackedArraySerializationId) == false)
				{
					Debug.Assert(false);
					return strings;
				}
				string versionString = reader.GetAttribute(c_VersionAttributeId);
				int version = XmlToInt(versionString);
				if (version == 1)
				{
					strings = new List<string>();
					if (reader.IsEmptyElement)
						return strings;
					string packedStrings = reader.ReadInnerXml();
					string[] unpackedStrings = packedStrings.Split(c_PackedArrayElementDelimiter);
					strings = new List<string>(unpackedStrings);
				}
				else
				{
					Debug.Assert(false);
					return strings;
				}
			}

			return strings;
		}

		private static string c_TypeInfoId = "___TypeInfo";
		/// <summary>
		/// Returns an instance of a IBioRadXmlSerializable from the given serialized state.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public static IBioRadXmlSerializable CreateInstanceFromTypedXml(string xml)
		{
			try
			{
				string[] typeInfoStrings = GetTypeInfoFromXml(xml);
				typeInfoStrings[0] = StripEverythingButAssemblyNameFromAssemblyFullName(typeInfoStrings[0]);
				if (typeInfoStrings[0] == "BioRad.CustomControls")
				{
					// BioRad.CustomControls was deprecated; does not exist any longer.  Don't want to throw an exception here because it disrupts the program
					//  flow when being debugged.  
					return null;
				}
				object o = Activator.CreateInstance(typeInfoStrings[0], typeInfoStrings[1]).Unwrap();
				IBioRadXmlSerializable instance = o as IBioRadXmlSerializable;
				instance.FromXml(xml);
				return instance;
			}
			catch
			{
				return null;
			}
		}
		internal static string StripEverythingButAssemblyNameFromAssemblyFullName(string assemblyFullName)
		{
			return assemblyFullName.Split(',')[0];
		}
		/// <summary></summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public static IBioRadXmlSerializable CreateInstanceFromTypedXml(XElement xml)
		{
			try
			{
				Type type = SerializationState.GetTypeInfoFromXElement(xml);
				if (type == null)
					return null;
				object o = Activator.CreateInstance(type, xml);
				IBioRadXmlSerializable instance = o as IBioRadXmlSerializable;
				return instance;
			}
			catch
			{
				return null;
			}
		}
		/// <summary></summary>
		/// <param name="state"></param>
		/// <param name="type"></param>
		public static void AddTypeInfoToStateDictionary(Dictionary<string, string> state, Type type)
		{
			string[] typeInfoStrings = new string[2];
			typeInfoStrings[0] = StripEverythingButAssemblyNameFromAssemblyFullName(type.Assembly.FullName);
			typeInfoStrings[1] = type.FullName;
			state[c_TypeInfoId] = StringToXml(string.Join(";", typeInfoStrings));
		}
		private static string[] GetTypeInfoFromXml(string xml)
		{
			Dictionary<string, string> stateDictionary = XmlToDictionary(xml, null, false);
			if (stateDictionary.ContainsKey(c_TypeInfoId) == false)
				throw new ApplicationException("No type information is contained in this xml.");

			string[] typeInfoStrings = stateDictionary[c_TypeInfoId].Split(';');
			return typeInfoStrings;
		}
		#endregion
		#endregion
	}
	/// <summary>
	/// Exception for use with Xml Serialization.
	/// </summary>
	public class XmlSerializationException : Exception
	{
		/// <summary>
		/// Constructs a new XmlSerializationException, containing a given message.
		/// </summary>
		/// <param name="message">Exception message.  This should be localized.</param>
		public XmlSerializationException(string message) : base(message) { }
	}
}
