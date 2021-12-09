using System;

namespace BioRad.Common.Services
{
	#region Documentation Tags
	/// <summary>
	/// Services are application-level components that provide discrete functionality.
	/// A service will typically be used when there is some functionality that needs 
	/// to be provided to a system that can be cleanly componentized, and which will
	/// require its own specific configuration. In addition, services are usually
	/// accessed in a singleton-style fashion, so that they only need to be configured
	/// once per application instance, and so that all access to that portion of
	/// functionality goes through the same interface.
	/// Examples of services include
	/// <list type="bullet">
	/// <item>A service for handling data access (persistence)</item>
	/// <item>A service that communicates with external hardware</item>
	/// <item>A service that manages a UI framework</item>
	/// </list>
	/// Basically, any part of the system that can be cleanly decoupled from other parts,
	/// that needs to manage its own configuration, and that optionally requires
	/// singleton-style access is a candidate for a service.
	/// </summary>
	/// <remarks>
	/// There are two key components to the service framework:
	/// <list type="bullet">
	/// <item><see cref="IService"/>: This interface defines a class as a service, 
	/// hooking it into the service framework and providing the necessary lifecycle
	/// methods.</item>
	/// <item><see cref="ServiceProvider"/>: A singleton class that provides an entry
	/// point into the service framework. The service provider is responsible for 
	/// managing the cache of services in an application. It takes care of configuring
	/// services into the framework and providing them their own unique configuration.
	/// It also provides services upon request to client code.</item>
	/// </list>
	/// Services are configured at two levels. First, the service framework is configured
	/// by a service provider configuration file (eventually, other configuration
	/// mechanisms will be supported as well). The service provider configuration
	/// file (by default, "application-services.xml") is an XML file that specifies
	/// what services the framework should load at initialization time, as well
	/// as the following information about each service:
	/// <list type="bullet">
	/// <item>Type information, such as interface and class name, used to identify
	/// the service in the service provider registry.</item>
	/// <item>Assembly name information, used in conjunction with the type information
	/// to reflectivly instantiate the service at runtime.</item>
	/// <item>Configuration for specific properties of a service, so that, for example,
	/// if "ServiceXXX" has property called "HeightSetting", this can be specified int
	/// the service provider config, and it will be set to the value specified at 
	/// runtime.</item>
	/// <item>Configuration provider information for the service. Many services have
	/// their own specific configuration files (for example, a device service could
	/// have a file that lists the expected devices). In the service provider
	/// config, those services have a configuration provider specified, along with
	/// any properties needed by the configuration provider. For example, a service
	/// that uses a configuration file would specify something like 
	/// "MyServiceFileConfigurationProvider" as the provider type, and optionally
	/// specify the filename as a property of the configuration provider.</item>
	/// </list>
	/// The second level of configuration is service-specific configuration. This is not
	/// dictacted by the service framework, since it is specific to each service. The
	/// framework does provide flexible support for just about any kind of unique
	/// configuration needs, via a pluggable configuration provider mechanism, described
	/// above. Services that require their own additional configuration 
	/// beyond the basic property setting supported in the service provider config file
	/// should define an implementation of <see cref="IConfigurationProvider"/> that
	/// properly loads the necessary configuration. This can come from a file, db, or
	/// any other source. Once the provider is created, it can be specified in the
	/// service provider configuration, along with any necessary properties. It will
	/// then be used by the <see cref="ServiceProvider"/> when it loads a service to
	/// load the service up with it's necessary configuration.
	/// <para>
	/// One thing to note about service loading is that services are loaded only
	/// upon request (lazy loading). When the <see cref="ServiceProvider"/> is first
	/// accessed, it initializes itself using the service provider configuration file.
	/// Only service configuration data is loaded; the services themselves are not. A
	/// service will only be loaded when it is first requested. At that time, the 
	/// actual <see cref="IService"/> implementation will be instantiated via reflection,
	/// have its configured properties set, and then use any specified
	/// <see cref="IConfigurationProvider"/> to load up service configuration data.
	/// </para>
	/// <para>
	/// When creating a service, it is recommended that you subclass the provided
	/// abstract class, <see cref="AbstractService"/>. This class handles most of the
	/// low-level plumbing necessary for services to work. If you must directly
	/// implement <see cref="IService"/>, you must make sure to duplicate all of this
	/// plumbing.
	/// </para>
	/// <seealso cref="ServiceProvider"/>
	/// <seealso cref="IService"/>
	/// <seealso cref="AbstractService"/>
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
	///			<item name="vssfile">$Workfile: AboutServices.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Services/AboutServices.cs $</item>
	///			<item name="vssrevision">$Revision: 2 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public sealed partial class AboutServices
	{
	}
}
