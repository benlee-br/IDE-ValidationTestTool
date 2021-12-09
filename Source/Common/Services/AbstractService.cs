using System;
using System.Collections;
namespace BioRad.Common.Services
{
	#region Documentation Tags
	/// <summary>
	/// Abstract implementation of IService that provides convenience methods for 
	/// services. Most services should subclass this class rather than directly 
	/// implement the IService interface.
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
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
	///			<item name="vssfile">$Workfile: AbstractService.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Services/AbstractService.cs $</item>
	///			<item name="vssrevision">$Revision: 6 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public abstract partial class AbstractService : IService
    {
        #region Member Data
		/// <summary>
		/// List of currently initialized configuration providers.
		/// </summary>
		protected ArrayList m_ConfigurationProviders = new ArrayList();
        #endregion

        #region Accessors
        #endregion

        #region Methods
		/// <summary>
		/// Default Load method. Gets configuration elements from call to 
		/// GetConfigurationElements method, then calls overloaded Load
		/// method that takes a collection.
		/// </summary>
		public virtual void Load()
		{
			ICollection configElements = GetConfigurationElements();
			Load(configElements);
		}
		/// <summary>
		/// This version is abstract; it is up to each service to determine how to
		/// load its configuration elements.
		/// </summary>
		/// <param name="configurationElements"></param>
		public abstract void Load(ICollection configurationElements);

		/// <summary>
		/// This gets all configuration elements from all loaded IConfigurationProviders.
		/// </summary>
		/// <returns></returns>
		public virtual ICollection GetConfigurationElements()
		{
			ArrayList elements = new ArrayList();
			foreach (IConfigurationProvider provider in m_ConfigurationProviders)
			{
				elements.AddRange(provider.GetConfigurationElements());
			}
			return elements;
		}

		/// <summary>
		/// Registers a configuration provider by adding it to the internal list of providers.
		/// </summary>
		/// <param name="provider"></param>
		public void RegisterConfigurationProvider(IConfigurationProvider provider)
		{
			m_ConfigurationProviders.Add(provider);
		}

		/// <summary>
		/// Helper method useful for controlling configuration providers when testing.
		/// Resets the internal list of configuration providers so that new ones can
		/// be added.
		/// </summary>
		public void ResetConfigurationProviders()
		{
			m_ConfigurationProviders = new ArrayList();
		}

        #endregion
    }
}
