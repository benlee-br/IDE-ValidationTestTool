using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;

namespace BioRad.Common.Remoting

{
	#region Documentation Tags
	/// <summary>
	/// Abstract base class for classes that wrap events for subscription across remoting
	/// boundaries.  Derived classes are instantiated in the client and marshalled
	/// to the server.  To receive events, the client must call the connect method
	/// and then bind local handler(s) to the wrapped event(s). Client can unsubscribe
	/// by calling disconnect either for all objects or for a particular object.
	/// </summary>
	/// <remarks>
	/// Derived classes should explicitly implement IRemoteEventsWrapper for type-safe
	/// ConnnectEvents() and DisconnectEvents() methods. 
	/// Maintains a collection of connected objects, used for disconnecting all events. 
	/// Calling Dispose method explicitly disconnects all events.
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
	///			<item name="vssfile">$Workfile: RemoteEventsWrapper.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Remoting/RemoteEventsWrapper.cs $</item>
	///			<item name="vssrevision">$Revision: 6 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public abstract partial class RemoteEventsWrapper : MarshalByRefObject, IRemoteEventsWrapper
	{

		#region Member Data
		private ArrayList m_Connected = new ArrayList();
		#endregion
		
		#region Methods
		/// <summary>
		/// Base class implementation of ConnectEvents adds object to collection of
		/// connected objects.
		/// </summary>
		/// <remarks>Override to immplement event connection. When overridden in derived type 
		/// call base method.</remarks>
		/// <param name="obj">Event handlers are attached to this object's events.</param>
		protected virtual void ConnectEvents(Object obj)
		{
			m_Connected.Add(obj);
		}

		/// <summary>
		/// Explicit interface definition.
		/// </summary>
		/// <remarks>supports call to derived type from base type</remarks>
		/// <param name="obj">Event handlers are attached to this object's events.</param>
		void  IRemoteEventsWrapper.ConnectEvents(Object obj)
		{
			ConnectEvents(obj);
		}

		/// <summary>
		/// Base class implementation of DisconnectEvents removes object from collection
		/// of connected objects.
		/// </summary>
		/// <remarks>Override to implement event disconnection. When overridden in derived 
		/// type call base method</remarks>
		/// <param name="obj">Event handlers are removed from this object's events.</param>
		protected virtual void  DisconnectEvents(Object obj)
		{
			if (m_Connected.Contains(obj))
				m_Connected.Remove(obj);
		}

		/// <summary>
		/// Explicit interface definition.
		/// </summary>
		/// <remarks>supports call to derived type from base type</remarks>
		/// <param name="obj">Event handlers are attached to this object's events.</param>
		void  IRemoteEventsWrapper.DisconnectEvents(Object obj)
		{
			DisconnectEvents(obj);
		}

		/// <summary>
		/// Base class implementation of DisconnectEvents calls DisconnectEvents for each
		/// object in the collection.
		/// </summary>
		/// <remarks>When overridden in derived type call base method. It is not
		/// generally necessary to override this method.</remarks>
		public virtual void  DisconnectAllEvents()
		{
			foreach (object obj in m_Connected)
				((IRemoteEventsWrapper) this).DisconnectEvents(obj);
		}

		/// <summary>
		/// Thread-safe dispose disconnects events for all connected objects.
		/// </summary>
		/// <remarks>when overridden in derived type call base method</remarks>
		public void Dispose()
		{
			// No finalize required so Dispose is called explicitly only
			lock (this)
			{
				try
				{
					// only dispose once
					if (m_Connected != null)
					{
						DisconnectAllEvents();
						m_Connected = null;
					}
				}
				catch
				{
					// Ignore errors
				}
			}
		}

		/// <summary>Override this objects remoting lifetime.</summary>
		/// <returns>Null to extend lifetime indefinitely.</returns>
		public override object InitializeLifetimeService()
		{
			// This RemoteEventsWrapper will live forever
			return null;
		}
		#endregion

	}
}
