using System;
using System.Xml.Serialization;
namespace BioRad.Common.Services.Config
{
	#region Documentation Tags
	/// <summary>
	/// Class holding config information for a parameter used by
	/// application services.
	/// </summary>
	/// <remarks>
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
	///			<item name="vssfile">$Workfile: ServiceConfigParameter.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Services/Config/ServiceConfigParameter.cs $</item>
	///			<item name="vssrevision">$Revision: 9 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	[Serializable]
	public partial class ServiceConfigParameter
	{
		#region Member Data
		/// <summary>
		/// Name of the configuration parameter.
		/// </summary>
		private string m_ParamName;
		/// <summary>
		/// Configuration parameter value.
		/// </summary>
		private string m_ParamValue;
		/// <summary>
		/// Fully-qualified typename of the parameter, used for casting.
		/// </summary>
		private string m_TypeName;
		#endregion
		
		#region Accessors
		/// <summary>
		/// Accessors for paramName
		/// </summary>
		[XmlAttribute("name")]
		public string Name
		{
			get{return this.m_ParamName;}
			set{this.m_ParamName = value;}
		}
		/// <summary>
		/// Accessors for paramValue
		/// </summary>
		[XmlAttribute("value")]
		public string Value
		{
			get{return this.m_ParamValue;}
			set{this.m_ParamValue = value;}
		}
		/// <summary>
		/// Accessors for typeName
		/// </summary>
		[XmlAttribute("typeName")]
		public string TypeName
		{
			get{return this.m_TypeName;}
			set{this.m_TypeName = value;}
		}
		#endregion
	}
}
