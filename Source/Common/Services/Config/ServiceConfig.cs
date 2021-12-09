using System;
using System.Xml.Serialization;
namespace BioRad.Common.Services.Config
{
	#region Documentation Tags
	/// <summary>
	/// Class representing the configuration of an service instantiated
	/// and potentially cached in the application registry.
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
	///			<item name="vssfile">$Workfile: ServiceConfig.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Services/Config/ServiceConfig.cs $</item>
	///			<item name="vssrevision">$Revision: 8 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public partial class ServiceConfig
	{
		#region Member Data
		/// <summary>
		/// Parameters used to configure the service
		/// for initialization.
		/// </summary>
		private ServiceConfigParameter[] m_Parameters;
		/// <summary>
		/// Configuration provider configurations for the service.
		/// </summary>
		private ConfigurationProviderConfig[] m_ConfigurationProviders;
		/// <summary>
		/// Name of the interface this service implements. Usually this will be
		/// the same as the class name, but this allows the implementation class
		/// of the service it differ from the interface.
		/// </summary>
		private string m_InterfaceName;
		/// <summary>
		/// Fully-qualified class name of the implementation of
		/// IService.
		/// </summary>
		private string m_ClassName;
		/// <summary>
		/// Assembly name (without ".dll") of the implementation of
		/// IService.
		/// </summary>
		private string m_AssemblyName;
		/// <summary>
		/// Service description, only used for clarity in service configuration.
		/// </summary>
		private string m_Description;
		/// <summary>
		/// An optional alias used to reference the service, in addition to
		/// the interface name. If no alias is specified, the interface name 
		/// is used. This comes in handy when dealing with service dependencies
		/// (not yet implemented), so that you only need to reference an alias in
		/// a dependency configuration rather than a fully-qualified interface.
		/// </summary>
		private string m_ServiceAlias;
		/// <summary>
		/// An alias used to reference the factory configured for the service
		/// provider that will be used to instantiate the service.
		/// </summary>
		private string m_FactoryAlias;
		#endregion
		
		#region Accessors
		/// <summary>
		/// Accessors for parameters
		/// </summary>
		[XmlArray("parameters")]
		[XmlArrayItem("parameter")]
		public ServiceConfigParameter[] Parameters
		{
			get{return this.m_Parameters;}
			set{this.m_Parameters = value;}
		}

		/// <summary>
		/// Accessors for configurationProviders
		/// </summary>
		[XmlArray("configurationProviders")]
		[XmlArrayItem("configurationProvider")]
		public ConfigurationProviderConfig[] ConfigurationProviders
		{
			get{return this.m_ConfigurationProviders;}
			set{this.m_ConfigurationProviders = value;}
		}
		/// <summary>
		/// Accessors for interfaceName
		/// </summary>
		[XmlAttribute("interfaceName")]
		public string InterfaceName
		{
			get{return this.m_InterfaceName;}
			set{this.m_InterfaceName = value;}
		}
		/// <summary>
		/// Accessors for serviceAlias
		/// </summary>
		[XmlAttribute("serviceAlias")]
		public string ServiceAlias
		{
			get{return this.m_ServiceAlias;}
			set{this.m_ServiceAlias = value;}
		}
		/// <summary>
		/// Accessors for className
		/// </summary>
		[XmlAttribute("className")]
		public string ClassName
		{
			get{return this.m_ClassName;}
			set{this.m_ClassName = value;}
		}
		/// <summary>
		/// Accessors for assemblyName
		/// </summary>
		[XmlAttribute("assemblyName")]
		public string AssemblyName
		{
			get{return this.m_AssemblyName;}
			set{this.m_AssemblyName = value;}
		}
		/// <summary>
		/// Accessors for description
		/// </summary>
		[XmlAttribute("description")]
		public string Description
		{
			get{return this.m_Description;}
			set{this.m_Description = value;}
		}
		/// <summary>
		/// Accessors for factoryAlias
		/// </summary>
		[XmlAttribute("factoryAlias")]
		public string FactoryAlias
		{
			get{return this.m_FactoryAlias;}
			set{this.m_FactoryAlias = value;}
		}	
		#endregion
	}
}
