using System;
using System.Collections;
namespace BioRad.Common.Services
{
	#region Documentation Tags
	/// <summary>
	/// A service provides functionality to the application. It is accessible via the
	/// ServiceProvider class, which contains all configuration information necessary
	/// to instantiate (and potentially cache) the service.
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
	///			<item name="vssfile">$Workfile: IService.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/Services/IService.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dmcauliffe $</item>
	///			<item name="vssdate">$Date: 7/03/03 2:21p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public interface IService
	{
		#region Methods
		/// <summary>
		/// loads the service using the default load mechanism.
		/// </summary>
		void Load();

		/// <summary>
		/// Loads the service using an externally supplied collection of
		/// configuration elements (useful for testing).
		/// </summary>
		/// <param name="configurationElements">Collection of configuration elements
		/// to use for loading</param>
		void Load(ICollection configurationElements);
		
		/// <summary>
		/// All services must provide a way for a configuration provider to be registered.
		/// This is used by the service provider when initializing services.
		/// </summary>
		/// <param name="provider">Configuration provider implementation to
		/// register</param>
		void RegisterConfigurationProvider(IConfigurationProvider provider);
		#endregion
	}
}
