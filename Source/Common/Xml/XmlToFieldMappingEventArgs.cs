using System;
using System.Xml.Serialization;
using System.Xml;
namespace BioRad.Common.Xml
{
	#region Documentation Tags
	/// <summary>
	/// An event args class used with the debugging events raised by the
	/// xml library.
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
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
	///			<item name="vssfile">$Workfile: XmlToFieldMappingEventArgs.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Xml/XmlToFieldMappingEventArgs.cs $</item>
	///			<item name="vssrevision">$Revision: 2 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class XmlToFieldMappingEventArgs : EventArgs
	{
        #region Member Data
		/// <summary>
		/// Name of xml node.
		/// </summary>
		private string m_XmlName;
		/// <summary>
		/// TYpe of xml node.
		/// </summary>
		private XmlNodeType m_XmlNodeType;
		/// <summary>
		/// Field name for which attribute is created.
		/// </summary>
		private string m_FieldName;
		/// <summary>
		/// Field type for field for which attribute is created.
		/// </summary>
		private Type m_FieldType;
		#endregion

        #region Accessors
		/// <summary>
		/// Accessors for xml name.
		/// </summary>
		public string XmlName
		{
			get { return m_XmlName; }
			set { m_XmlName = value; }
		}

		/// <summary>
		/// Accessors for xml type.
		/// </summary>
		public XmlNodeType XmlNodeType
		{
			get { return m_XmlNodeType; }
			set { m_XmlNodeType = value; }
		}

		/// <summary>
		/// Accessors for field name.
		/// </summary>
		public string FieldName
		{
			get { return m_FieldName; }
			set { m_FieldName = value; }
		}

		/// <summary>
		/// Accessors for field type.
		/// </summary>
		public Type FieldType
		{
			get { return m_FieldType; }
			set { m_FieldType = value; }
		}

		#endregion

        #region Constructors and Destructor
		/// <summary>
		/// Default constructor for initializing all fields.
		/// </summary>
		/// <param name="xmlName">xml name</param>
		/// <param name="xmlNodeType">xml node type</param>
		/// <param name="fieldName">field name</param>
		/// <param name="fieldType">field type</param>
	    public XmlToFieldMappingEventArgs(string xmlName, XmlNodeType xmlNodeType, 
			string fieldName, Type fieldType)
	    {
	        this.m_XmlName = xmlName;
	        this.m_XmlNodeType = xmlNodeType;
	        this.m_FieldName = fieldName;
	        this.m_FieldType = fieldType;
	    }
        #endregion

		#region Methods
		/// <summary>
		/// Override of to string.
		/// </summary>
		/// <returns>Entire contents, comma separated.</returns>
		public override string ToString()
		{
			return "xmlName=" + m_XmlName + ","
				+ "nodeType=" + m_XmlNodeType + ","
				+ "fieldName=" + m_FieldName + ","
				+ "fieldType=" + m_FieldType;
		}

		#endregion

	}
}
