using System;
using System.Xml.Serialization;
namespace BioRad.Common.Services.Config
{
	#region Documentation Tags
	/// <summary>
	/// Configuration object for a service factory.
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
	///			<item name="vssfile">$Workfile: ServiceFactoryConfig.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Services/Config/ServiceFactoryConfig.cs $</item>
	///			<item name="vssrevision">$Revision: 8 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public partial class ServiceFactoryConfig
	{
		#region Member Data
		/// <summary>
		/// Optional alias used to identify the factory at runtime 
		/// in the provider.
		/// </summary>
		private string m_FactoryAlias;
		/// <summary>
		/// Fully-qualified class name of the implementation of
		/// IApplicationServiceFactory.
		/// </summary>
		private string m_ClassName;
		/// <summary>
		/// Assembly name (without ".dll") of the implementation of
		/// IApplicationServiceFactory.
		/// </summary>
		private string m_AssemblyName;
		/// <summary>
		/// Flag indicating if the factory is registered at
		/// runtime (default is for factory to be registered at 
		/// initialization time). Factories are registered at runtime if they 
		/// contain contextual information that cannot be configured in the service
		/// provider. Instead, they are created at runtime as necessary and
		/// then registered into the already-initialized service provider.
		/// </summary>
		private bool m_IsRegisteredAtRuntime;
		/// <summary>
		/// Parameters used to configure the factory for initialization.
		/// </summary>
		private ServiceConfigParameter[] m_Parameters;
		#endregion

		#region Accessors
		/// <summary>
		/// Accessors for factoryAlias
		/// </summary>
		[XmlAttribute("factoryAlias")]
		public string FactoryAlias
		{
			get{return this.m_FactoryAlias;}
			set{this.m_FactoryAlias = value;}
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
		/// Accessors for isRegisteredAtRuntime
		/// </summary>
		[XmlAttribute("isRegisteredAtRuntime")]
		public bool IsRegisteredAtRuntime
		{
			get{return this.m_IsRegisteredAtRuntime;}
			set{this.m_IsRegisteredAtRuntime = value;}
		}
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
		#endregion
	}
}
