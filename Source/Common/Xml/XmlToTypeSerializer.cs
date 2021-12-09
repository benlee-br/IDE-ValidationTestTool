using System;
using System.IO;
using System.Xml.Schema;
using System.Xml.Serialization;
using BioRad.Common.DiagnosticsLogger;
using BioRad.Common.Utilities;

namespace BioRad.Common.Xml
{
	#region Documentation Tags
	/// <summary>
	/// The class responsible for handling serialization using XmlToTypeRulesets. It
	/// also includes some helper functionality, such as the ability to generate a
	/// schema from a given ruleset.
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:</item>
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
	///			<item name="vssfile">$Workfile: XmlToTypeSerializer.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Xml/XmlToTypeSerializer.cs $</item>
	///			<item name="vssrevision">$Revision: 7 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ssarup $</item>
	///			<item name="vssdate">$Date: 1/23/07 1:32a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class XmlToTypeSerializer
	{
		#region Member Data
		/// <summary>
		/// The ruleset to use. You can't use this class without it.
		/// </summary>
		private XmlToTypeMapping m_Mapping;
		/// <summary>
		/// Flag indicating whether error handling should be used.
		/// </summary>
		private bool m_UseErrorHandling = false;
		/// <summary>
		/// Flag indicating whether schema validation should be used.
		/// </summary>
		private bool m_UseSchemaValidation = false;
		#endregion

		#region Accessors
		/// <summary>
		/// Accessors for the ruleset.
		/// </summary>
		public XmlToTypeMapping Mapping
		{
			get { return m_Mapping; }
			set { m_Mapping = value; }
		}

		/// <summary>
		/// Accessors for the error handling flag.
		/// </summary>
		public bool UseErrorHandling
		{
			get { return m_UseErrorHandling; }
			set { m_UseErrorHandling = value; }
		}
		/// <summary>
		/// Accessors for the schema validation flag.
		/// </summary>
		public bool UseSchemaValidation
		{
			get { return this.m_UseSchemaValidation; }
			set { this.m_UseSchemaValidation = value; }
		}
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Default constructor, takes a ruleset to use.
		/// </summary>
		/// <param name="mapping">The mapping used for serialization.</param>
		public XmlToTypeSerializer(XmlToTypeMapping mapping)
		{
			this.m_Mapping = mapping;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Registers event handlers for serialization on a given xml serializer.
		/// </summary>
		/// <param name="s">The serializer to listen to.</param>
		private void RegisterForErrorHandling(XmlSerializer s)
		{
			s.UnknownNode+= new 
				XmlNodeEventHandler(OnUnknownNode);
			s.UnknownAttribute+= new 
				XmlAttributeEventHandler(OnUnknownAttribute);		    
		}

		/// <summary>
		/// Serializes the supplied object to a file.
		/// </summary>
		/// <param name="objectToSerialize">The object to serialize.</param>
		/// <param name="fileName">The file name to which the object will
		/// be written.</param>
		public void Serialize(object objectToSerialize, string fileName)
		{
			using (FileStream stream = new FileStream(fileName, FileMode.Create))
			{
				Serialize(objectToSerialize, stream);
			}
		}

		/// <summary>
		/// Serializes the supplied object to a file stream.
		/// </summary>
		/// <param name="objectToSerialize">The object to serialize.</param>
		/// <param name="stream">The stream to which the object will be
		/// written.</param>
		public void Serialize(object objectToSerialize, FileStream stream)
		{
			XmlSerializer s = new XmlSerializer(m_Mapping.MappedType, m_Mapping.ToAttributeOverrides());
			// register for error handling
			//TODO: Make this optional
			if (m_UseErrorHandling)
			{
				RegisterForErrorHandling(s);
				// todo: check type.
			}

			using ( TextWriter writer = new StreamWriter(stream))
			{
				s.Serialize(writer, objectToSerialize);
			}
		}
		/// <summary>
		/// Serializes the supplied object to a specified file as encrypted data.
		/// </summary>
		/// <param name="objectToSerialize">The object to serialize.</param>
		/// <param name="fileName"></param>
		public void SerializeEncrypted(object objectToSerialize, string fileName)
		{
			// Fix for Bug 4095 - use the OpenFile method to remove the
			// readonly attribute if set
			using (FileStream fs = FileUtilities.OpenFile(fileName, FileMode.Create,
					   FileAccess.Write, FileShare.None))
			{
				// Write data to the XML document. 
				this.Serialize(objectToSerialize, fs);
			}
			//Encrypt file.
			//The memory encryption had failures, using file to file.
			FileCryptor.GetInstance.EncryptFile(fileName, fileName);
		}
		/// <summary>
		/// Deserializes an object from the supplied file.
		/// </summary>
		/// <param name="fileName">The name of the file to read from.</param>
		/// <returns>The deserialized object.</returns>
		public object Deserialize(string fileName)
		{
			object result = null;
			XmlSerializer s = new XmlSerializer(m_Mapping.MappedType, m_Mapping.ToAttributeOverrides());
			using (TextReader reader = new StreamReader(fileName))
			{
				result = s.Deserialize(reader);
			}
			return result;
		}

		/// <summary>
		/// Deserializes an object from the supplied stream.
		/// </summary>
		/// <param name="stream">the stream to read from.</param>
		/// <returns>The deserialized object.</returns>
		public object Deserialize(Stream stream)
		{
			object result = null;
			XmlSerializer s = new XmlSerializer(m_Mapping.MappedType, m_Mapping.ToAttributeOverrides());
			result = s.Deserialize(stream);
			return result;
		}

		/// <summary>
		/// Turns the ruleset into an xml schema file.
		/// </summary>
		/// <param name="fileName">The filename to which the schema should be written.</param>
		public void GenerateSchema(string fileName)
		{
			XmlSchemas schemas1 = new XmlSchemas();
			XmlSchemaExporter exporter = new XmlSchemaExporter(schemas1);
			XmlAttributeOverrides or = m_Mapping.ToAttributeOverrides();
			XmlReflectionImporter importer = new XmlReflectionImporter(or);
			XmlTypeMapping mapping = importer.ImportTypeMapping(m_Mapping.MappedType);
			exporter.ExportTypeMapping(mapping);

			XmlSchema schema = schemas1[0];
			using (TextWriter writer = new StreamWriter(fileName))
			{
				schema.Write(writer);
			}
		}

		#endregion

		#region Event Handlers
		/// <summary>
		/// Handles unknown nodes on a serializer.
		/// </summary>
		/// <param name="sender">Event sender</param>
		/// <param name="e">Event args</param>
		protected void OnUnknownNode
			(object sender, XmlNodeEventArgs e)
		{
			//TODO: handle unknown nodes, perhaps as a collection of errors
			DiagnosticsLogService service = DiagnosticsLogService.GetService;
			DiagnosticsLog log = service.GetDiagnosticsLog("XmlToTypeSerializer");

			log.SeriousError("XmlToTypeSerializer Unknown Node:" +   e.Name + "\t" + e.Text);
		}
		/// <summary>
		/// Handles unknown attributes on a serializer.
		/// </summary>
		/// <param name="sender">Event sender</param>
		/// <param name="e">Event args</param>
		protected void OnUnknownAttribute
			(object sender, XmlAttributeEventArgs e)
		{
			//TODO: handle unknown attrs, perhaps as a collection of errors
			System.Xml.XmlAttribute attr = e.Attr;

			DiagnosticsLogService service = DiagnosticsLogService.GetService;
			DiagnosticsLog log = service.GetDiagnosticsLog("XmlToTypeSerializer");
			log.SeriousError("XmlToTypeSerializer Unknown attribute " + attr.Name + "='" + attr.Value + "'");
		}
		#endregion
	}
}
