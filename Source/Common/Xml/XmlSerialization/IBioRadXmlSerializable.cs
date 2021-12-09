using System;
using System.Xml.Linq;
using BioRad.Common.Xml;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Interface for serializing to and from Xml.
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
	///			<item name="vssfile">$Workfile: IBioRadXmlSerializable.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Xml/XmlSerialization/IBioRadXmlSerializable.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Thouser $</item>
	///			<item name="vssdate">$Date: 12/08/09 2:56p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public interface IBioRadXmlSerializable
	{
		/// <summary>Creates an XElement which encodes the state of the object.</summary>
		/// <returns>Element containing the object's state.</returns>
		XElement ToXElement();
		/// <summary>Deserialize from an XElement.</summary>
		/// <param name="element">element containing the object state</param>
		void FromXElement(XElement element);
		/// <summary>Fills the object with the state encoded within the given string, as created by 
		/// IBioRadXmlSerializable.ToXml().</summary>
		/// <param name="xml">Encoded state as created by IBioRadXmlSerializable.ToXml()</param>
		void FromXml(string xml);
		/// <summary>Gets a serialized xml string representing the object.</summary>
		/// <returns>A serialized xml string representing the object.</returns>
		string ToXml();
	}

	/// <summary>
	/// Base class for objects which support xml serialization.
	/// </summary>
	public abstract class BioRadXmlSerializableBase : IBioRadXmlSerializable
	{
		/// <summary>Derived clases should return their serialization ID.</summary>
		/// <returns></returns>
		protected abstract string GetSerializationId();

		/// <summary>Provides an xml string which encodes the state of the object.</summary>
		/// <returns>xml string</returns>
		public string ToXml()
		{
			return ToXml(true);
		}
		/// <summary>Provides an xml string which encodes the state of the object.</summary>
		/// <param name="includeTypeInfo">Whether to include type info in the xml.  Type info is used for dynamic object instantiation 
		/// only, as in CreateInstanceFromTypedXml(...).  Otherwise type info is ignored.</param>
		/// <returns></returns>
		public string ToXml(bool includeTypeInfo)
		{
			return ToXElement(includeTypeInfo).ToString(SaveOptions.DisableFormatting);
		}
		/// <summary></summary>
		/// <returns></returns>
		public XElement ToXElement()
		{
			SerializationState state = ToSerializationState();
			return state.ToXElement(GetSerializationId(), GetType());
		}
		/// <summary></summary>
		/// <param name="includeTypeInfo">Whether to include type info in the serialized state.  If false, the object cannot
		/// be re-created using the dynamic instantiation</param>
		/// <returns></returns>
		public XElement ToXElement(bool includeTypeInfo)
		{
			SerializationState state = ToSerializationState();
			return state.ToXElement(GetSerializationId(), (includeTypeInfo ? this.GetType() : null));
		}
		/// <summary></summary>
		/// <param name="xml"></param>
		public void FromXml(string xml)
		{
			SerializationState state = SerializationState.CreateFromXml(xml, GetSerializationId(), true);
			FromSerializationState(state);
		}
		/// <summary></summary>
		/// <param name="element"></param>
		public void FromXElement(XElement element)
		{
			SerializationState state = SerializationState.CreateFromXml(element, GetSerializationId(), true);
			FromSerializationState(state);
		}
		/// <summary>Fill a SerializationState in with sufficient information to represent the state of the object.</summary>
		/// <returns>a state table which will be populated by this method.  This state
		/// table will be of the most recent version.</returns>
		protected abstract SerializationState ToSerializationState();
		/// <summary>populate this instance from a SerializationState.</summary>
		/// <param name="state">state of the object</param>
		protected abstract void FromSerializationState(SerializationState state);
	}
}

	// Code template for objects derived from BioRadXmlSerializableBase:
#if false
#region Xml Serialization
		// Serialization version.
		private const int c_SerializationVersion = 1;

		// Serialization Ids
		private const string c_SerializationId = "<class name>"; // Fill in: class name or abbreviation of it.
		private const string c_SerializationVersionId = "SerVersion";

		/// <summary>Deserialization constructor.</summary>
		public <Classname>(XElement element) // Fill in: class name (this is a constructor)
		{
			FromXElement(element);
		}
		/// <summary>Gets the serialization id of the most derived class</summary>
		/// <returns>The serialization id of the most derived class</returns>
		protected override string GetSerializationId()
		{
			return c_SerializationId;
		}
		/// <summary>Fill a table in with sufficient information to represent the state of the object.</summary>
		/// <returns>a state table which will be populated by this method.  This state
		/// table will be of the most recent version.</returns>
		protected override SerializationState ToSerializationState()
		{
			SerializationState state = new SerializationState();
			state.AddInt(c_SerializationVersionId, c_SerializationVersion);

			// Fill in: set this object's data into the state object.

			return state;
		}
		/// <summary>populate this instance from a SerializationState.</summary>
		/// <param name="state">state of the object</param>
		protected override void FromSerializationState(SerializationState state)
		{
			UpgradeSerializationState(state);

			// Validate that the version is the most recent, as guaranteed by UpgradeSerializationState().
			int version = state.GetInt(c_SerializationVersionId);
			if (version != c_SerializationVersion)
				throw new XmlSerializationException("State dictionary is not of the current version.");

			// Fill in: retreive this object's data from the state object.
		}
		/// <summary>Upgrades a SerializationState of any version, to the current version.</summary>
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

			//// Example:
			//if (version == 1)
			//{
			//   // Upgrade version 1 SerializationState to version 2

			//   // We are now consistent with version 2 state dictionaries.
			//   int newVersion = 2;
			//   state.AddInt(c_SerializationVersionId, newVersion);
			//   version = newVersion;
			//}

			Debug.Assert(version == c_SerializationVersion);
		}
#endregion Xml Serialization
#endif

	// Code template for IBioRadXmlSerializable objects which do not derive from BioRadXmlSerializableBase
#if false
#region Xml Serialization
		// Serialization version.
		private const int c_SerializationVersion = 1;

		// Serialization Ids
		private const string c_SerializationId = "<class name>"; // Fill in: class name
		private const string c_SerializationVersionId = "SerVersion";

		/// <summary>Deserialization constructor.</summary>
		public <Classname>(XElement element) // Fill in: class name (this is a constructor)
		{
			FromXElement(element);
		}
		/// <summary>Provides an xml string which encodes the state of the object.</summary>
		/// <returns>xml string</returns>
		public string ToXml()
		{
			return ToXElement().ToString(SaveOptions.DisableFormatting);
		}
		/// <summary>Gets an XElement encoding the state of this object.</summary> 
		/// <returns>An XElement encoding the state of this object.</returns>
		public XElement ToXElement()
		{
			SerializationState state = ToSerializationState();
			return state.ToXElement(c_SerializationId, GetType());
		}
		/// <summary>Deserialize from an Xml string as created by ToXml().</summary>
		/// <param name="xml">Xml string as created by ToXml().</param>
		public void FromXml(string xml)
		{
			SerializationState state = SerializationState.CreateFromXml(xml, c_SerializationId, true);
			FromSerializationState(state);
		}
		/// <summary>Deserializes from the given XElement, as created by ToXElement().</summary>
		/// <param name="element">An XElement, as created by ToXElement().</param>
		public void FromXElement(XElement element)
		{
			SerializationState state = SerializationState.CreateFromXml(element, c_SerializationId, true);
			FromSerializationState(state);
		}
		/// <summary>Fill a table in with sufficient information to represent the state of the object.</summary>
		/// <returns>a state table which will be populated by this method.  This state
		/// table will be of the most recent version.</returns>
		protected SerializationState ToSerializationState()
		{
			SerializationState state = new SerializationState();
			state.AddInt(c_SerializationVersionId, c_SerializationVersion);

			// Fill in: set this object's data into the state object.

			return state;
		}
		/// <summary>populate this instance from a SerializationState.</summary>
		/// <param name="state">state of the object</param>
		protected void FromSerializationState(SerializationState state)
		{
			UpgradeSerializationState(state);

			// Validate that the version is the most recent, as guaranteed by UpgradeSerializationState().
			int version = state.GetInt(c_SerializationVersionId);
			if (version != c_SerializationVersion)
				throw new XmlSerializationException("State dictionary is not of the current version.");

			// Fill in: retrieve this object's data from the state object.
		}
		/// <summary>Upgrades a SerializationState of any version, to the current version.</summary>
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

			//// Example:
			//if (version == 1)
			//{
			//   // Change SerializationState to version 2 format.

			//   // We are now consistent with version 2 state dictionaries.
			//   int newVersion = 2;
			//   state.AddInt(c_SerializationVersionId, newVersion);
			//   version = newVersion;
			//}

			Debug.Assert(version == c_SerializationVersion);
		}
#endregion Xml Serialization
#endif

#if false //add this for backwards comaptibility with old IBioRadXmlSerializable objects.
		/// <summary>Creates serialized xml string.</summary>
		/// <returns>Xml string containing the object's state.</returns>
		public string ToXml()
		{
			return ToXml(true);
		}
		/// <summary>Deserialize from an XElement.</summary>
		/// <param name="element">element containing the object state</param>
		public void FromXElement(XElement element)
		{
			FromXml(element.ToString(SaveOptions.DisableFormatting));
		}
		/// <summary>Creates an XElement which encodes the state of the object.</summary>
		/// <returns>Element containing the object's state.</returns>
		public XElement ToXElement()
		{
			return XElement.Parse(ToXml(true));
		}
#endif
