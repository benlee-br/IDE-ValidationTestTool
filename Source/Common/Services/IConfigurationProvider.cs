using System;
using System.Collections;
namespace BioRad.Common.Services
{
	#region Documentation Tags
	/// <summary>
	/// A configuration provider returns a set of configuration elements for an
	/// IService. Each IService may be registered with multiple configuration providers.
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
	///			<item name="vssfile">$Workfile: IConfigurationProvider.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/Services/IConfigurationProvider.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dmcauliffe $</item>
	///			<item name="vssdate">$Date: 7/03/03 2:21p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public interface IConfigurationProvider
	{
		#region Methods
		/// <summary>
		/// Gets configuration elements that this provider provides. The return
		/// signature is an ICollection because there should be no restriction of
		/// what type of configuration objects a service may require.
		/// For example, a permission service may be registered with an implementation
		/// of this interface that pulls a set of permission config objects from an xml
		/// file. Another implementation may pull the same permission config objects
		/// from a database.
		/// </summary>
		/// <returns>The configuration elements.</returns>
		ICollection GetConfigurationElements();
		#endregion
	}
}
