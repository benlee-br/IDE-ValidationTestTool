using System;
using BioRad.Common.Services.Config;
namespace BioRad.Common.Services
{
	#region Documentation Tags
	/// <summary>
	/// Interface specifying the definition of a factory that instantiates
	/// application services.
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
	///			<item name="vssfile">$Workfile: IApplicationServiceFactory.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/Services/IApplicationServiceFactory.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dmcauliffe $</item>
	///			<item name="vssdate">$Date: 7/03/03 2:21p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public interface IServiceFactory
	{

		#region Accessors
		/// <summary>
		/// Returns the id associated with this factory 
		/// (usually the impl class name)
		/// </summary>
		string FactoryId
		{
			get;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Instantiates an application service based on the 
		/// service configuration.
		/// </summary>
		/// <param name="serviceConfig">ServiceConfig to use for creating
		/// a service</param>
		/// <returns>Fully configured service instance</returns>
		IService CreateService(ServiceConfig serviceConfig);
		#endregion



	}
}
