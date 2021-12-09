using System;
using BioRad.Common.Xml;

namespace BioRad.Common.Services.Config
{
	#region Documentation Tags
	/// <summary>
	/// A class that holds config mappings for all of the service provider config
	/// classes.
	/// This allows the config mappings to be contained in one place.
	/// </summary>
	/// <remarks>
	/// The exposed methods are not static, so that this may be subclassed and the methods
	/// overridden.
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
	///			<item name="vssfile">$Workfile: ServiceProviderXmlToTypeMappingInfo.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Services/Config/ServiceProviderXmlToTypeMappingInfo.cs $</item>
	///			<item name="vssrevision">$Revision: 6 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public partial class ServiceProviderXmlToTypeMappingInfo
	{
        #region Methods
		/// <summary>
		/// Provides mapping for configuration provider config.
		/// </summary>
		/// <returns>The type mapping.</returns>
		public virtual XmlToTypeMapping GetConfigurationProviderConfigTypeMapping()
		{
			XmlToTypeMapping mapping = new XmlToTypeMapping(typeof(ConfigurationProviderConfig),
				"configurationProvider");
			mapping.AddCollectionMapping("parameters", "Parameters",
				"parameter", typeof(ServiceConfigParameter));
			mapping.AddAttributeMapping("className", "ClassName", typeof(string));
			mapping.AddAttributeMapping("assemblyName", "AssemblyName", typeof(string));

			return mapping;
		}
		/// <summary>
		/// Provides mapping for service config.
		/// </summary>
		/// <returns>The type mapping.</returns>
		public virtual XmlToTypeMapping GetServiceConfigMapping()
		{
			XmlToTypeMapping mapping = new XmlToTypeMapping(typeof(ServiceConfig),
				"service-config");
			mapping.AddCollectionMapping("parameters", "Parameters",
				"parameter", typeof(ServiceConfigParameter));
			mapping.AddCollectionMapping("configurationProviders", "ConfigurationProviders",
				"configurationProvider", typeof(ConfigurationProviderConfig));
			mapping.AddAttributeMapping("interfaceName", "InterfaceName", typeof(string));
			mapping.AddAttributeMapping("serviceAlias", "ServiceAlias", typeof(string));
			mapping.AddAttributeMapping("className", "ClassName", typeof(string));
			mapping.AddAttributeMapping("assemblyName", "AssemblyName", typeof(string));
			mapping.AddAttributeMapping("description", "Description", typeof(string));
			mapping.AddAttributeMapping("factoryAlias", "FactoryAlias", typeof(string));
			return mapping;
		}

		/// <summary>
		/// Provides mapping for service parameter config.
		/// </summary>
		/// <returns>The type mapping.</returns>
		public virtual XmlToTypeMapping GetServiceConfigParameterMapping()
		{
			XmlToTypeMapping mapping = new XmlToTypeMapping(typeof(ServiceConfigParameter),
				"parameter");
			mapping.AddAttributeMapping("name", "Name", typeof(string));
			mapping.AddAttributeMapping("value", "Value", typeof(string));
			mapping.AddAttributeMapping("typeName", "TypeName", typeof(string));
			return mapping;
		}

		/// <summary>
		/// Provides mapping for service factory config.
		/// </summary>
		/// <returns>The type mapping.</returns>
		public virtual XmlToTypeMapping GetServiceFactoryConfigMapping()
		{
			XmlToTypeMapping mapping = new XmlToTypeMapping(typeof(ServiceFactoryConfig),
				"parameter");
			mapping.AddAttributeMapping("factoryAlias", "FactoryAlias", typeof(string));
			mapping.AddAttributeMapping("className", "ClassName", typeof(string));
			mapping.AddAttributeMapping("assemblyName", "AssemblyName", typeof(string));
			mapping.AddAttributeMapping("isRegisteredAtRuntime", "IsRegisteredAtRuntime", typeof(bool));
			mapping.AddCollectionMapping("parameters", "Parameters",
				"parameter", typeof(ServiceConfigParameter));
			return mapping;
		}

		/// <summary>
		/// Provides mapping for service provider config.
		/// </summary>
		/// <returns>The type mapping.</returns>
		public virtual XmlToTypeMapping GetServiceProviderConfigMapping()
		{
			XmlToTypeMapping mapping = new XmlToTypeMapping(typeof(ServiceProviderConfig),
				"service-provider");
			mapping.AddCollectionMapping("serviceFactories", "FactoryConfigs",
				"serviceFactory", typeof(ServiceFactoryConfig));
			mapping.AddCollectionMapping("services", "ServiceConfigs",
				"service", typeof(ServiceConfig));
			// add nested mappings
			mapping.AddNestedTypeMapping(GetServiceFactoryConfigMapping());
			mapping.AddNestedTypeMapping(GetServiceConfigParameterMapping());
			mapping.AddNestedTypeMapping(GetServiceConfigMapping());
			mapping.AddNestedTypeMapping(GetConfigurationProviderConfigTypeMapping());
			return mapping;
		}
		#endregion

	}
}
