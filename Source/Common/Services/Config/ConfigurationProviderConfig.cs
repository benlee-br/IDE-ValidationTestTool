using System;
using System.Xml.Serialization;
namespace BioRad.Common.Services.Config
{
    /// <summary>Class containing the information needed to configure a configuration
    /// provider for use in the service provider.</summary>
    /// <remarks>Authors:</remarks>
    /// <remarks>Last design/code review:</remarks>
	public partial class ConfigurationProviderConfig
    {
        #region Member Data
		/// <summary>
		/// Parameters used to configure the configuration provider
		/// for initialization.
		/// </summary>
		private ServiceConfigParameter[] m_Parameters;
		/// <summary>
		/// Fully-qualified class name of the implementation of
		/// IConfigurationProvider.
		/// </summary>
		private string m_ClassName;
		/// <summary>
		/// Assembly name (without ".dll") of the implementation of
		/// IConfigurationProvider.
		/// </summary>
		private string m_AssemblyName;
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
        #endregion

        #region Constructors and Destructor
        /// <summary>
        /// Default no-argument constructor
        /// </summary>
        public ConfigurationProviderConfig()
        {
        }
        #endregion
    }
}
