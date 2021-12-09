using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
namespace BioRad.Common.Services.Config
{
	#region Documentation Tags
	/// <summary>
	/// Configuration object representing how a registry is
	/// configured.
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
	///			<item name="vssfile">$Workfile: ServiceProviderConfig.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Services/Config/ServiceProviderConfig.cs $</item>
	///			<item name="vssrevision">$Revision: 8 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	[XmlRoot(ElementName="service-provider")]
	public partial class ServiceProviderConfig
	{
		#region Member Data
		/// <summary>
		/// Array of service factory configurations.
		/// </summary>
		private ServiceFactoryConfig[] m_factoryConfigs;
		/// <summary>
		/// Array of service configurations.
		/// </summary>
		private ServiceConfig[] m_serviceConfigs;
		#endregion

		#region Accessors
		/// <summary>
		/// Accessors for serviceFactories
		/// </summary>
		[XmlArray("serviceFactories")]
		[XmlArrayItem("serviceFactory")]
		public ServiceFactoryConfig[] FactoryConfigs
		{
			get{return this.m_factoryConfigs;}
			set{this.m_factoryConfigs = value;}
		}
		/// <summary>
		/// Accessors for services
		/// </summary>
		[XmlArray("services")]
		[XmlArrayItem("service")]
		public ServiceConfig[] ServiceConfigs
		{
			get{return this.m_serviceConfigs;}
			set{this.m_serviceConfigs = value;}
		}
		#endregion	
	}
}
