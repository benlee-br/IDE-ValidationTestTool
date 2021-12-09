using System;
using System.Collections;
using System.Configuration;
using BioRad.Common.Reflection;
using BioRad.Common.Services.Config;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using BioRad.Common.Xml;
using BioRad.Common.Utilities;

namespace BioRad.Common.Services
{
	#region Documentation Tags
	/// <summary>
	/// A class that manages configured services and anything necessary to load them
	/// at runtime. This is the key class of the application service API. The 
	/// ServiceProvider maintains configuration information about every service that will
	/// be made available. Services are not loaded until they are first requested.
	/// Anything necessary to properly load the service (such as datasource refs,
	/// config parameters, etc.) should be made available to the service loading
	/// process via proper ServiceProvider configuration.
	/// Services that require special handling to load may use factory classes that
	/// are also configured in the ServiceProvider. These factories will provide whatever
	/// functionality is necessary to instantiate and load a service at runtime. 
	/// Usually factories are necessary when a service does not have a no-argument
	/// constructor.
	/// 
	/// In some special cases, factories themselves may require information necessary
	/// to load a service that is only available at runtime (such as a pointer to a
	/// web context or other runtime-only resource). To support this, the ServiceProvider
	/// allows factories to be configured that are expected to be registered at runtime.
	/// The factories can be created at runtime with the proper references to any
	/// runtime services and then registered with the ServiceProvider. Any services that 
	/// require these factories will use them when they are first loaded.
	/// 
	/// The service provider provides singleton access to itself.
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
	///			<item name="vssfile">$Workfile: ServiceProvider.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Services/ServiceProvider.cs $</item>
	///			<item name="vssrevision">$Revision: 36 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Staran $</item>
	///			<item name="vssdate">$Date: 7/02/07 5:26p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public partial class ServiceProvider
	{
		#region Constants
		/// <summary>
		/// Default configuration filename.
		/// </summary>
		private const string c_DefaultConfigFile = "application-services.xml";
		/// <summary>
		/// Key for the configuration filename value in app config.
		/// </summary>
		private const string c_ConfigFileKey = "ApplicationServicesFileName";
		#endregion

		#region Member Data
		/// <summary>
		/// Name of the file to use for configuration.
		/// </summary>
		private string m_ConfigFile;
		/// <summary>
		/// Hashtable of registered, loaded services.
		/// </summary>
		private Hashtable m_registeredServices;
		/// <summary>
		/// Hashtable of registered service configurations. Used to load services 
		/// upon request.
		/// </summary>
		private Hashtable m_registeredServiceConfigs;
		/// <summary>
		/// Hashtable of registered and loaded service factories.
		/// </summary>
		private Hashtable m_registeredServiceFactories;
		/// <summary>
		/// Hashtables of registered service factory configurations.
		/// Used to load factories upon request.
		/// </summary>
		private Hashtable m_registeredServiceFactoryConfigs;	
		/// <summary>
		/// Application utility that instantiates classes by name, 
		/// using reflection.
		/// </summary>
		private ReflectionUtil m_util = new ReflectionUtil();
		/// <summary>
		/// Static private instance, used to implement singleton pattern.
		/// </summary>
		private static ServiceProvider instance;
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Private constructor to force singleton access
		/// </summary>
		private ServiceProvider(){}
		#endregion

		#region Accessors
		/// <summary>
		/// Accessors for configuration file.
		/// </summary>
		public string ConfigFile
		{
			get
			{
				// Get the file name from AppSettings.
				if (this.m_ConfigFile == null)
				{
					// first, attempt to get the filename from application settings.
					string filename =
                        ConfigurationManager.AppSettings[c_ConfigFileKey];
					// if that doesn't work, use the default filename.
					if (filename == null )
					{
						filename = c_DefaultConfigFile;
					}
					// set the config file reference to this value.
					m_ConfigFile = filename;
				}
				return this.m_ConfigFile;
			}
			set{this.m_ConfigFile = value;}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Singleton instance provider.
		/// </summary>
		/// <returns>Static singleton instance</returns>
		public static ServiceProvider GetInstance()
		{
			if (instance == null)
			{
				instance = new ServiceProvider();
				instance.LoadServices();
			}
			return instance;
		}

		/// <summary>
		/// Similar to <see cref="GetInstance"/> but allows configuration 
		/// filename to be specified. This will reset the singleton instance,
		/// and subsequent calls to <see cref="GetInstance"/> will return the
		/// new instance, loaded from this configuration file.
		/// </summary>
		/// <remarks>
		/// This method is ONLY used for testing.
		/// </remarks>
		/// <param name="configFile">The name of the configuration file to
		/// use for loading services.</param>
		/// <returns>A newly created instance, loaded using the supplied 
		/// config file, which will also be returned on subsequent calls to
		/// <see cref="GetInstance"/>.</returns>
		public static ServiceProvider ReloadInstanceFromFile(string configFile)
		{
			instance = new ServiceProvider();
			instance.ConfigFile = configFile;
			instance.LoadServices();
			return instance;
		}

		/// <summary>
		/// Resets the singleton instance, so that it can be obtained via the 
		/// normal GetInstance method. This is used in conjunction with 
		/// <see cref="GetInstance"/> to control provider configuration
		/// for tests.
		/// </summary>
		/// <remarks>
		/// This method is ONLY used for testing.
		/// </remarks>
		public static void ResetInstance()
		{
			instance = null;
		}
		
		/// <summary>
		/// Returns the factory key used to register a factory configuration.
		/// This is the internal key used to uniquely identify the factory.
		/// </summary>
		/// <param name="config">The factory configuration.</param>
		/// <returns>The key</returns>
		private String GetFactoryKey(ServiceFactoryConfig config)
		{
			if (config.FactoryAlias != null)
				return config.FactoryAlias;
			else
				return config.ClassName;
		}
		/// <summary>
		/// Registers a factory configuration.
		/// </summary>
		/// <param name="config">The factory configuration.</param>
		private void RegisterFactoryConfig(ServiceFactoryConfig config)
		{
			m_registeredServiceFactoryConfigs.Add(config.ClassName, config);
			if (config.FactoryAlias != null)
				m_registeredServiceFactoryConfigs.Add(config.FactoryAlias, 
					config);
			// load either an instantiated factory or a placeholder for a factory
			// that gets registered at runtime.
			if (! (config.IsRegisteredAtRuntime))
			{
				LoadFactory(config);
			}
			else
			{
				m_registeredServiceFactories.Add(GetFactoryKey(config), null);
			}
		}
		/// <summary>
		/// Retrieves a service factory by identifier. This uses the unique identifier,
		/// though other methods may delegate to this method that take various identifers
		/// (such as classname, interface name, or alias).
		/// </summary>
		/// <param name="factoryIdentifier">The identifier (unique identifier).</param>
		/// <returns>The factory identified uniquely by the identifier.</returns>
		private IServiceFactory GetFactory(string factoryIdentifier)
		{
			IServiceFactory factory = null;
			if (! IsFactoryRegistered(factoryIdentifier))
			{
				throw new ServiceLoadException(this,
                    StringUtility.FormatString(Properties.Resources.FactoryNotRegisteredAtRuntime_1,
					factoryIdentifier));
			}
			else
			{		
				ServiceFactoryConfig config = 
					GetFactoryConfig(factoryIdentifier);
				if (! IsFactoryLoaded(config))
				{
					if (config.IsRegisteredAtRuntime)
					{
						throw new ServiceLoadException(this,
                            StringUtility.FormatString(Properties.Resources.FactoryNotRegisteredAtRuntime_1,
							factoryIdentifier));
					}
					else
					{
						throw new ServiceLoadException(this,
                            StringUtility.FormatString(Properties.Resources.FactoryNotRegistered_1,
							factoryIdentifier));				    
					}
				}
					
				factory = GetFactory(config);
			}
			return factory;		
		}
		/// <summary>
		/// Gets the registered factory for the supplied factory configuration.
		/// </summary>
		/// <param name="config">The factory configuration requested.</param>
		/// <returns>The actual factory.</returns>
		private IServiceFactory GetFactory(
			ServiceFactoryConfig config)
		{		
			return (IServiceFactory) 
				m_registeredServiceFactories[GetFactoryKey(config)];
		}	
		/// <summary>
		/// Returns the factory configuration registered under the unique identifier.
		/// </summary>
		/// <param name="factoryIdentifier">Unique factory identifier</param>
		/// <returns>The factory stored under that identifier</returns>
		private ServiceFactoryConfig GetFactoryConfig(string factoryIdentifier)
		{
			ServiceFactoryConfig config = 
				(ServiceFactoryConfig) 
				m_registeredServiceFactoryConfigs[factoryIdentifier];
			return config;		
		}	
		
		/// <summary>
		/// Checks to see if a factory is registered under the supplied name.
		/// The map of configurations is checked, since that's what stores duplicate
		/// mappings. The factory hashtable (of loaded factories) only stores things
		/// by one key value.
		/// </summary>
		/// <param name="factoryIdentifier">Unique factory identifier</param>
		/// <returns>True/false: is a factory registered under the supplied identifier?</returns>
		private bool IsFactoryRegistered(String factoryIdentifier)
		{
			return m_registeredServiceFactoryConfigs.ContainsKey(factoryIdentifier);
		}
	
		/// <summary>
		/// Checks to see if a factory has been loaded yet for the supplied config.
		/// Unlike with services, factories that aren't set as "runtime" are always
		/// loaded. This is really checking to make sure that runtime factories that
		/// are expected (configured) have actually been registered at runtime to the
		/// hashtable. 
		/// </summary>
		/// <param name="config">Factory configuration</param>
		/// <returns>True/false: is a factory registered under the supplied identifier,
		/// as determined from the factory configuration?</returns>
		private bool IsFactoryLoaded(ServiceFactoryConfig config)
		{
			String factoryIdentifier = GetFactoryKey(config);
			return ! (m_registeredServiceFactories
				[factoryIdentifier] == null);
		}
	
		/// <summary>
		/// Loads a factory for the supplied configuration and stores it in the
		/// map of registered factories.
		/// </summary>
		/// <param name="config">Factory configuration to use for loading</param>
		private void LoadFactory(ServiceFactoryConfig config)
		{
#if MYDEBUG
			PerfTimer timer = new PerfTimer();
			timer.Start();
#endif
			// instantiate class
			//MBCTMP Debug.WriteLine(String.Format("Loading factory class {0} at {1}", config.ClassName, DateTime.Now.ToLongTimeString()));
			IServiceFactory factory = (IServiceFactory)
				m_util.InstantiateObject(config.ClassName, config.AssemblyName);
			// set parameters
			m_util.SetPropertiesWithParameters(factory, config.Parameters);
			m_registeredServiceFactories.Add(GetFactoryKey(config), factory);	
			//MBCTMPDebug.WriteLine(String.Format("Factory class {0} loaded at {1}", config.ClassName, DateTime.Now.ToLongTimeString()));
#if MYDEBUG
			TimeType t = timer.Stop();
			Trace.Write("* load factory class" + config.ClassName + ": ");
			Trace.WriteLine(t.GetAs(TimeType.Units.MilliSeconds).ToString("########.00")); //MBCTMP
#endif
		}
		
		/// <summary>
		/// Loads an instance of a configuration provider using the provider configuration.
		/// WHile similar to other methods here that instantiate a class, like LoadFactory,
		/// this one always returns a new instance (configuration providers don't have 
		/// references stored here; they're always passed to the service).
		/// </summary>
		/// <param name="config">Configuration provider config object</param>
		/// <returns>Loaded IConfigurationProvider instance</returns>
		private IConfigurationProvider LoadConfigurationProviderInstance(
			ConfigurationProviderConfig config)
		{
#if MYDEBUG
			PerfTimer timer = new PerfTimer();
			timer.Start();
#endif
			// instantiate class
			//MBCTMP Debug.WriteLine(String.Format("Loading configuration provider {0} at {1}", config.ClassName, DateTime.Now.ToLongTimeString()));
			object providerObj = 
				m_util.InstantiateObject(config.ClassName, config.AssemblyName);
			if (!(providerObj is IConfigurationProvider))
			{
				throw new ServiceLoadException(this,
                    StringUtility.FormatString(Properties.Resources.ClassNotAService_2,
					new object[]{config.ClassName, config.AssemblyName}));
			}
			IConfigurationProvider provider = (IConfigurationProvider) providerObj;
			// set parameters
			m_util.SetPropertiesWithParameters(provider, config.Parameters);
			//MBCTMP Debug.WriteLine(String.Format("Configuration provider {0} loaded at {1}", config.ClassName, DateTime.Now.ToLongTimeString()));
#if MYDEBUG
			TimeType t = timer.Stop();
			Trace.Write("** load configuration provider " + config.ClassName + ": ");
			Trace.WriteLine(t.GetAs(TimeType.Units.MilliSeconds).ToString("########.00")); //MBCTMP
#endif
			// return result
			return provider;
		}

		/// <summary>
		/// Registers individual service configurations. Registration is done so that 
		/// that clients can use either the implementation class or the interface
		/// for a reference, or in case the config information is needed.
		/// A placeholder is also made in the services hashtable itself so that we
		/// know the difference between a service that hasn't been loaded yet and one
		/// that just isn't registered.
		/// Note that registering a service DOES NOT instantiate it; services are
		/// only initialized and instantiated when first used.
		/// </summary>
		/// <param name="config">Service config object to register</param>
		private void RegisterServiceConfig(ServiceConfig config)
		{
			// store configuration. Configurations are stored under both the
			// serviceInterface name and the implementationClassName.	
			m_registeredServiceConfigs.Add(config.ClassName, config);
			// only store under the interface name if it's different
			// from the impl class name.
			if (config.InterfaceName != null &&
				config.InterfaceName != config.ClassName)
			{
				m_registeredServiceConfigs.Add(config.InterfaceName, config);
			}
			// additionally, if an alias is supplied, that is used to register the config.
			if (config.ServiceAlias != null)
				m_registeredServiceConfigs.Add(config.ServiceAlias, config);
			// store a placeholder for the instantiated and loaded service
			m_registeredServices.Add(GetServiceKey(config), null);
		}

		/// <summary>
		/// Checks to see if a service is registered under the supplied name.
		/// The map of configurations is checked, since that's what stores duplicate
		/// mappings. The services hashtable (of loaded services) only stores things
		/// by one key value.
		/// </summary>
		/// <param name="serviceIdentifier">Unique identifier of service</param>
		/// <returns>True/false: is a service registered under the supplied
		/// identifier?</returns>
		private bool IsServiceRegistered(string serviceIdentifier)
		{
			return m_registeredServiceConfigs.ContainsKey(serviceIdentifier);
		}
		/// <summary>
		/// Checks to see if a service has been loaded yet for the supplied 
		/// configuration.
		/// </summary>
		/// <param name="config">Service config object</param>
		/// <returns>True/false: is a service registered under the supplied
		/// identifier, as determined from the supplied service config object?</returns>
		private bool IsServiceLoaded(ServiceConfig config)
		{
			string serviceIdentifier = GetServiceKey(config);
			return ! (m_registeredServices[serviceIdentifier] == null);
		}
		/// <summary>
		/// Loads the service associated with the supplied service configuration.
		/// This will instantiate and load the service (using any appropriate factories) 
		/// if it hasn't yet been instantiated or loaded. 
		/// </summary>
		/// <param name="config">The service configuration.</param>
		private void LoadService(ServiceConfig config)
		{
			IService service = null;
			IServiceFactory factory = null;
#if MYDEBUG
			Trace.WriteLine("**** Enter LoadService()");
			PerfTimer timer = new PerfTimer();
			timer.Start();	
#endif
			if (config.FactoryAlias == null)
				factory = new DefaultServiceFactory();
			else
				factory = GetFactory(config.FactoryAlias);
			service = factory.CreateService(config);
			// load config provider if specified
			foreach (ConfigurationProviderConfig providerConfig 
						 in config.ConfigurationProviders)
			{
				service.RegisterConfigurationProvider(
					LoadConfigurationProviderInstance(providerConfig));	
			}
#if MYDEBUG
			TimeType t = timer.Stop();
			timer.Start();	
#endif		
			// LOAD THE SERVICE!!!
			//MBC Debug.WriteLine(String.Format("Loading service {0} at {1}", config.ClassName, DateTime.Now.ToLongTimeString()));
			service.Load();
			//MBC Debug.WriteLine(String.Format("Service {0} loaded at {1}", config.ClassName, DateTime.Now.ToLongTimeString()));
#if MYDEBUG
			TimeType t2  = timer.Stop();
			Trace.Write("** create service " + config.ClassName + ": ");
			Trace.WriteLine(t.GetAs(TimeType.Units.MilliSeconds).ToString("########.00")); //MBCTMP

			Trace.Write("** load service " + config.ClassName + ": ");
			Trace.WriteLine(t2.GetAs(TimeType.Units.MilliSeconds).ToString("########.00")); //MBCTMP
#endif

			CollectionUtils.HashtableReplace(
				m_registeredServices, GetServiceKey(config), service);
		}
		/// <summary>
		/// Gets the service registered for the supplied service config
		/// object
		/// </summary>
		/// <param name="config">The service config object for which a
		/// service has been registered</param>
		/// <returns>The registered service instance</returns>
		private IService GetService(ServiceConfig config)
		{		
			return (IService) m_registeredServices[GetServiceKey(config)];
		}

		/// <summary>
		/// Retrieves configuration information for application services and uses it
		/// to load them into the hashtable, ready for loading and use upon request.
		/// This method is the "startup" method for the service provider.
		/// </summary>
		private void LoadServices()
		{
			// clear out the hashtables
			m_registeredServices = new Hashtable();
			m_registeredServiceConfigs = new Hashtable();
			m_registeredServiceFactories = new Hashtable();
			m_registeredServiceFactoryConfigs = new Hashtable();
			ServiceProviderConfig  providerConfig = GetServiceProviderConfiguration();
		
			try
			{
				// FACTORIES
				foreach (ServiceFactoryConfig config in providerConfig.FactoryConfigs)
				{
					this.RegisterFactoryConfig(config);
				}

				// SERVICES			
				// iterate through configurations, registering them as needed
				foreach (ServiceConfig config in providerConfig.ServiceConfigs)
				{
					RegisterServiceConfig(config);
				}
			}
			catch (Exception e)
			{
				throw new ServiceLoadException(this,
                    StringUtility.FormatString(Properties.Resources.ServiceLoadFailure), e);
			}
		}

		/// <summary>
		/// Retrieves the service registered under the supplied name (accessible using
		/// the interface name, the implementation class name, or a configured alias).
		/// Validation (checking for registration) is performed here against the un-
		/// altered identifier (before any configs are accessed). 
		/// </summary>
		/// <param name="serviceIdentifier">Unique service identifier</param>
		/// <returns>The service.</returns>
		public IService GetService(string serviceIdentifier)
		{
			IService service = null;
			if (! IsServiceRegistered(serviceIdentifier))
			{
				throw new ServiceNotRegisteredException(serviceIdentifier);
			}
			else
			{		
				ServiceConfig config = GetServiceConfig(serviceIdentifier);
				if (! IsServiceLoaded(config))
					LoadService(config);
				service = GetService(config);
			}
			return service;
		}
		/// <summary>
		/// Override of GetService that takes a Type, using the type name to
		/// retrieve the appropriate service.
		/// </summary>
		/// <param name="serviceType">The type used to identify the service.</param>
		/// <returns>The service.</returns>
		public IService GetService(Type serviceType)
		{
			return GetService(serviceType.FullName);
		}

		/// <summary>
		/// Returns the registered service configuration.
		/// </summary>
		/// <param name="serviceIdentifier">The unique internal identifier of the 
		/// service configuration.</param>
		/// <returns>The service configuration.</returns>
		private ServiceConfig GetServiceConfig(string serviceIdentifier)
		{
			ServiceConfig config = (ServiceConfig) 
				m_registeredServiceConfigs[serviceIdentifier];
			return config;		
		}

		/// <summary>
		/// Helper function that provides a centralized mechanism for determining how
		/// loaded services are keyed. Service configs are mapped to multiple keys
		/// to allow for multiple ways to get at services. Internally, the loaded
		/// services are all mapped once, to one key. That key is derived from the
		/// service config using the logic of this function.
		/// </summary>
		/// <param name="config">The service configuration.</param>
		/// <returns>The identifier used to uniquely identify the supplied
		/// service configuration.</returns>
		private string GetServiceKey(ServiceConfig config)
		{
			return config.ClassName;	
		}

		/// <summary>
		/// Loads configuration information into the ServiceProvider. This needs
		/// to be moved into a subclass or a delegated (pluggable) configuration
		/// provider. This class shouldn't have to worry about dealing with
		/// XmlSerialization, file io, or swapping config providers; that's why
		/// it needs to be moved.
		/// </summary>
		/// <returns>The service provider configuration object as returned from
		/// the 'application-services.xml' file (after deserialization)</returns>
		private ServiceProviderConfig GetServiceProviderConfiguration()
		{
			ServiceProviderXmlToTypeMappingInfo mappingInfo = new 
				ServiceProviderXmlToTypeMappingInfo();
			XmlToTypeSerializer serializer = 
				new XmlToTypeSerializer(mappingInfo.GetServiceProviderConfigMapping());

			string filename = this.ConfigFile;
			// first, check if this file exists in the application path.
            filename = ApplicationPath.GetFullPath(filename);
			if (!File.Exists(filename))
			{
				// then check locally
				if (!File.Exists(this.ConfigFile))
				{
					throw new ServiceLoadException(this,
                        StringUtility.FormatString(Properties.Resources.ServicesConfigFileNotFound_2,
						filename, this.ConfigFile));
				}
				else
				{
				    filename = this.ConfigFile;
				}
			}
   
			// Declare an service variable of the type to be deserialized.
			ServiceProviderConfig serviceProviderConfig = null;
			// Use the Deserialize method to restore the service's state with
			// data from the XML document. 
			// Decrypt contnets if encrypted to read the XML document.
			using(Stream decryptedStream = 
					  FileCryptor.GetInstance.DecryptFileContentsToStream(filename))
			{
				if(decryptedStream != null)
				{
					
					serviceProviderConfig = 
						(ServiceProviderConfig)serializer.Deserialize(decryptedStream);
				}
			}
			return serviceProviderConfig;
		}

	}
	#endregion


}
