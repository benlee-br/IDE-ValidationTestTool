using System;

namespace BioRad.Common.Remoting
{
	#region Documentation Tags
	/// <summary>
	/// Interface definition for remote events wrappers.
	/// </summary>
	/// <remarks>
	/// Specific remote events wrappers should derive from RemoteEventsWrapper.cs and
	/// use explicit interface member implementations for type safety.
	/// TODO: Add disconnect; possibly cache all connected objects for a full disconnect.
	/// Implement disposable pattern to disconnect all events.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Lisa von Schlegell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">1035</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: IRemoteEventsWrapper.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/Remoting/IRemoteEventsWrapper.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Lvs $</item>
	///			<item name="vssdate">$Date: 7/29/04 1:20p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public interface IRemoteEventsWrapper
	{

        #region Methods
		/// <summary>
		/// Remote events wrappers should connect to object events within
		/// this method. Use explicit interface definition for type safety.
		/// (See RemoteEventsWrapper abstract base class for an example).
		/// </summary>
		/// <param name="obj">Object defining events.</param>
		void ConnectEvents(Object obj);
		/// <summary>
		/// Remote events wrappers should disconnect from object events within
		/// this method. Use explicit interface definition for type safety.
		/// (See RemoteEventsWrapper abstract base class for an example).
		/// </summary>
		/// <param name="obj">Object defining events.</param>
		void DisconnectEvents(Object obj);
		/// <summary>
		/// Remote events wrappers should disconnect from all events within
		/// this method. RemoteEventsWrapper abstract base class provides an
		/// implementation for this.
		/// </summary>
		void DisconnectAllEvents();
		#endregion

	}
}
