using System;
using System.Runtime.Serialization;
using BioRad.Common.DiagnosticsLogger;

namespace BioRad.Common.Reflection
{
	#region Documentation Tags
	/// <summary>
	/// Custom exception for problems with reflection util.
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
	///			<item name="vssfile">$Workfile: ReflectionException.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Reflection/ReflectionException.cs $</item>
	///			<item name="vssrevision">$Revision: 3 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class ReflectionException : ApplicationException
	{
		#region Constructors and Destructor
		/// <summary>
		/// Constructor that accepts a message and inner exception.
		/// </summary>
		/// <param name="message">Exception message</param>
		/// <param name="inner">Inner exception to nest</param>
		public ReflectionException(string message,
			Exception inner) : base(message, inner)
		{
		}
		/// <summary>
		/// Constructor that accepts a message.
		/// </summary>
		/// <param name="message">Exception message</param>
		public ReflectionException(string message) 
			: base(message)
		{
		}
		#endregion
	}
}
