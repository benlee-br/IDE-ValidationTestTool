using System;
using System.Runtime.Serialization;
namespace BioRad.Common.Services
{
	#region Documentation Tags
	/// <summary>
	/// Exception thrown when an unregistered service is requested in the service provider..
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
	///			<item name="vssfile">$Workfile: ServiceNotRegisteredException.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Services/ServiceNotRegisteredException.cs $</item>
	///			<item name="vssrevision">$Revision: 8 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public partial class ServiceNotRegisteredException : ApplicationException
	{
		#region Constructors and Destructor
		/// <summary>
		/// Constructor that accepts a message and inner exception.
		/// </summary>
		/// <param name="message">Exception message</param>
		/// <param name="inner">Inner exception to nest</param>
		public ServiceNotRegisteredException(string message, 
			Exception inner) : base(message, inner)
		{}
		/// <summary>
		/// Constructor that accepts a message.
		/// </summary>
		/// <param name="message">Exception message</param>
		public ServiceNotRegisteredException(string message) 
			: base(message)
		{}
		#endregion

	}
}
