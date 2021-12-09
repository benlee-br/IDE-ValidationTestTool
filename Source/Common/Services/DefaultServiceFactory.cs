using System;
using System.Text;
using BioRad.Common.Services.Config;
using BioRad.Common.Reflection;
namespace BioRad.Common.Services
{
	#region Documentation Tags
	/// <summary>
	/// Basic factory class for instantiating application services.
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
	///			<item name="vssfile">$Workfile: DefaultServiceFactory.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Services/DefaultServiceFactory.cs $</item>
	///			<item name="vssrevision">$Revision: 13 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class DefaultServiceFactory : IServiceFactory
	{
		#region Member Data
		/// <summary>
		/// AppUtil class used internally.
		/// </summary>
		protected ReflectionUtil m_util = new ReflectionUtil();
		/// <summary>
		/// Array of configuration parameters.
		/// </summary>
		private ServiceConfigParameter[] m_parameters;
		#endregion

		#region Accessors
		/// <summary>
		/// Accessors for parameters
		/// </summary>
		public ServiceConfigParameter[] Parameters
		{
			get{return this.m_parameters;}
			set{this.m_parameters = value;}
		}
		/// <summary>
		/// Implementation of FactoryId accessor from IApplicationServiceFactory.
		/// </summary>
		public string FactoryId
		{
			get{return "DefaultServiceFactory";}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Implementation of IApplicationServiceFactory.CreateService.
		/// This implementation instantiates a service based on the type information
		/// stored in the supplied ServiceConfig class. Each parameter stored in 
		/// ServiceConfig.Parameters will be used to set a correspondingly named
		/// property value (using set accessor) in the instantiated service.
		/// This is the simplest method for configuring services, and the most common.
		/// Service configs (entries in the 'application-services.xml' file, for
		/// example, only need to make sure that the parameter names in the configuration
		/// match property accessor names in the service implementation class.
		/// </summary>
		/// <param name="serviceConfig">ServiceConfig to use for creating a service
		/// instance</param>
		/// <returns>The instantiated service instance, with all properties set from
		/// ServiceConfig.Parameters</returns>
		public virtual IService CreateService(
			ServiceConfig serviceConfig)
		{
			// instantiate class
			IService service = InstantiateService(serviceConfig);
			// set parameters
			SetProperties(service, serviceConfig);
			return service;
		}
		/// <summary>
		/// Utility method that actual instantiates the service class based on the
		/// ServiceConfig's type information.
		/// </summary>
		/// <param name="serviceConfig">ServiceConfig to use for creating a service
		/// instance</param>
		/// <returns>The instantied service</returns>
		protected IService InstantiateService(
			ServiceConfig serviceConfig)
		{
			IService service = null;
			object serviceObject = null;
			try
			{
				serviceObject = m_util.InstantiateObject(
					serviceConfig.ClassName,
					serviceConfig.AssemblyName);
			}
			catch (Exception e)
			{
				throw new ServiceLoadException(this,
                    StringUtility.FormatString(Properties.Resources.ServiceInstantiationFailure_2,
					serviceConfig.ClassName, serviceConfig.AssemblyName),
					e);
			}
			// check to make sure object retrieved isn't null
			if (serviceObject == null)
			{
				throw new ServiceLoadException(this,
                    StringUtility.FormatString(Properties.Resources.ServiceInstantiationFailureNullClass_2,
					serviceConfig.ClassName, serviceConfig.AssemblyName));
			}
			// check to make sure that the object retrieved is an IService
			if (serviceObject is IService)
			{
				service = (IService) serviceObject;
			}
			else
			{
				throw new ServiceLoadException(this,
                    StringUtility.FormatString(Properties.Resources.ClassNotAService_2,
					serviceConfig.ClassName, serviceConfig.AssemblyName));
			}
			return service;
		}

		/// <summary>
		/// Utility method for setting properties. Uses internal AppUtil class.
		/// </summary>
		/// <param name="service">Service for which properties
		/// will be set</param>
		/// <param name="config">ServiceConfig that contains property values to
		/// use</param>
		protected void SetProperties(IService service,
			ServiceConfig config)
		{
			m_util.SetPropertiesWithParameters(service, config.Parameters);
		}
		#endregion

	}
}
