using System;
using System.Globalization;
using System.Diagnostics;
using BioRad.Common.Xml;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;

namespace BioRad.Common.Xml
{
	#region Documentation Tags
	/// <summary>
	/// Object to contain a collection of generic persistable data.
	/// </summary>
	/// <remarks>
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
	///			<item name="vssfile">$Workfile: PersistableData.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Xml/XmlSerialization/PersistableData.cs $</item>
	///			<item name="vssrevision">$Revision: 3 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Thouser $</item>
	///			<item name="vssdate">$Date: 7/30/10 4:10p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public class PersistableData : Dictionary<string, IBioRadXmlSerializable>, IBioRadXmlSerializable
	{
		#region Constructors
		/// <summary></summary>
		public PersistableData()
			: base()
		{
		}
		/// <summary></summary>
		/// <param name="a"></param>
		public PersistableData(Dictionary<string, IBioRadXmlSerializable> a)
			: base(a)
		{
		}
		#endregion

		#region Xml Serialization
		// Serialization version.
		private const int c_SerializationVersion = 2;

		// Serialization Ids
		private const string c_SerializationId = "PersistedData";
		private const string c_SerializationVersionId = "SerVersion";
		private const string c_PersistedKeyPrefix = "PD_";

		/// <summary>Deserialization constructor.</summary>
		public PersistableData(XElement element)
		{
			FromXElement(element);
		}
		/// <summary></summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public static PersistableData CreateFromXml(string xml)
		{
			PersistableData pd = new PersistableData();
			pd.FromXml(xml);
			return pd;
		}
		/// <summary></summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public static PersistableData CreateFromXElement(XElement xml)
		{
			PersistableData pd = new PersistableData();
			pd.FromXElement(xml);
			return pd;
		}
		/// <summary>Provides an xml string which encodes the state of the object.</summary>
		/// <returns>xml string</returns>
		public string ToXml()
		{
			return ToXElement().ToString(SaveOptions.DisableFormatting);
		}
		/// <summary></summary>
		/// <returns></returns>
		public XElement ToXElement()
		{
			SerializationState state = ToSerializationState();
			return state.ToXElement(c_SerializationId, GetType());
		}
		/// <summary></summary>
		/// <param name="xml"></param>
		public void FromXml(string xml)
		{
			SerializationState state = SerializationState.CreateFromXml(xml, c_SerializationId, true);
			FromSerializationState(state);
		}
		/// <summary></summary>
		/// <param name="element"></param>
		public void FromXElement(XElement element)
		{
			SerializationState state = SerializationState.CreateFromXml(element, c_SerializationId, true);
			FromSerializationState(state);
		}
		/// <summary>Fill a table in with sufficient information to represent the state of the object.</summary>
		/// <returns>a state table which will be populated by this method.  This state
		/// table will be of the most recent version.</returns>
		private SerializationState ToSerializationState()
		{
			SerializationState state = new SerializationState();
			state.AddInt(c_SerializationVersionId, c_SerializationVersion);

			foreach (string key in this.Keys)
			{
				string stateId = XmlConvert.EncodeLocalName(string.Format("{0}{1}", c_PersistedKeyPrefix, key));
				state.AddIBioRadXmlSerializable_Typed(stateId, this[key]);
			}

			return state;
		}
		/// <summary>populate this instance from a SerializationState.</summary>
		/// <param name="state">state of the object</param>
		private void FromSerializationState(SerializationState state)
		{
			UpgradeSerializationState(state);

			// Validate that the version is the most recent, as guaranteed by UpgradeSerializationState().
			int version = state.GetInt(c_SerializationVersionId);
			if (version != c_SerializationVersion)
				throw new XmlSerializationException("State dictionary is not of the current version.");

			Clear();
			foreach (string key in state.GetKeys())
			{
				string decodedKey = XmlConvert.DecodeName(key);
				if (decodedKey.Length < c_PersistedKeyPrefix.Length || decodedKey.Substring(0, c_PersistedKeyPrefix.Length) != c_PersistedKeyPrefix)
					continue;
				this[decodedKey.Substring(c_PersistedKeyPrefix.Length)] = state.GetIBioRadXmlSerializable_Typed(key);
			}
		}
		/// <summary>
		/// Upgrades a state table of any version, to the current version.
		/// </summary>
		/// <param name="state">persisted state to upgrade.</param>
		private void UpgradeSerializationState(SerializationState state)
		{
			int version = state.GetInt(c_SerializationVersionId);
			if (version < 1 || version > c_SerializationVersion)
			{
				// This is an unknown version.
				throw new XmlSerializationException(string.Format(CultureInfo.CurrentCulture,
						"Unknown serialized version", version));
			}

			if (version == 1)
			{
				string entriesId_V1 = "Entries";

				// Read version 1 style
				Dictionary<string, IBioRadXmlSerializable> entries = new Dictionary<string, IBioRadXmlSerializable>();
				string entriesAsXml = state.GetXElement(entriesId_V1).ToString(SaveOptions.DisableFormatting);
				Dictionary<string, string> entriesDictionary = XMLUtility.XmlToDictionary(entriesAsXml, entriesId_V1);
				foreach (KeyValuePair<string, string> kvp in entriesDictionary)
				{
					IBioRadXmlSerializable value = XMLUtility.CreateInstanceFromTypedXml(kvp.Value);
					if (value != null)
						entries[kvp.Key] = value;
				}

				// convert to version 2 style
				state.Remove(entriesId_V1);
				foreach (string key in entries.Keys)
				{
					string stateId = XmlConvert.EncodeLocalName(string.Format("{0}{1}", c_PersistedKeyPrefix, key));
					state.AddIBioRadXmlSerializable_Typed(stateId, entries[key]);
				}

				// We are now consistent with version 2 state dictionaries.
				int newVersion = 2;
				state.AddInt(c_SerializationVersionId, newVersion);
				version = newVersion;
			}

			Debug.Assert(version == c_SerializationVersion);
		}
		#endregion
	}
}
