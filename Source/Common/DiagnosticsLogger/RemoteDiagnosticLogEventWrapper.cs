using System;
using System.Runtime.Remoting.Messaging;

using BioRad.Common.Remoting;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>
	/// Class Summary
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Ralph Ansell</item>
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
	///			<item name="vssfile">$Workfile: RemoteDiagnosticLogEventWrapper.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/RemoteDiagnosticLogEventWrapper.cs $</item>
	///			<item name="vssrevision">$Revision: 6 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class RemoteDiagnosticLogEventsWrapper : RemoteEventsWrapper, IRemoteEventsWrapper
	{
		#region Members
		// Remoted events are received on thread-pool threads. Locks are used to
		// prevent following event from overrunning current event handling.

		/// <summary>
		/// Object used to synchronize diagnostic log item received event handler
		/// </summary>
		private object m_LockDiagnosticLogItemReceived = new object();
		#endregion

		#region Delegates and Events
		/// <summary>
		/// Wrapped diagnostic log event.
		/// </summary>
		public event DiagnosticsLogWriteEventHandler DiagnosticsLogWriteEvent;
		#endregion

		#region Methods
		/// <summary>
		/// Defers to type-safe method.
		/// </summary>
		/// <param name="obj">Object defining events</param>
		void IRemoteEventsWrapper.ConnectEvents(Object obj)
		{
			ConnectEvents((IDiagnosticsEventLog) obj);
		}

		/// <summary>
		/// Registers to listen for remote diagnostic log write events.
		/// </summary>
		/// <param name="log">A diagnostic logger object.</param>
		public void ConnectEvents(IDiagnosticsEventLog log)
		{
			// register the wrapper event handler with the logger.
			log.DiagnosticsLogWriteEvent += 
				new DiagnosticsLogWriteEventHandler(DiagnosticLogItemReceived);
			base.ConnectEvents(log);
		}

		/// <summary>
		/// Defers to type-safe method.
		/// </summary>
		/// <param name="obj">Object defining events</param>
		void IRemoteEventsWrapper.DisconnectEvents(Object obj)
		{
			DisconnectEvents((IDiagnosticsEventLog) obj);
		}

		/// <summary>
		/// Unregisters listening for remote diagnostic log write events.
		/// </summary>
		/// <param name="log">A diagnostic logger object.</param>
		public void DisconnectEvents(IDiagnosticsEventLog log)
		{
			// unregister the wrapper event handler with the logger.
			log.DiagnosticsLogWriteEvent -= 
				new DiagnosticsLogWriteEventHandler(DiagnosticLogItemReceived);
			base.DisconnectEvents(log);
		}
		#endregion

		#region Event Handlers
		// TODO: These classes are tagged "one way" so that server won't fault on a failure
		// to connect to a registered listener. A better implementation would be to
		// remove the attribute and have server detect and detach clients who are no
		// longer accessible (this is important only in the case of a long-running server).

		/// <summary>
		/// Wrapper event handler forwards the diagnostic log write event to the client.
		/// </summary>
		/// <param name="sender" >Reference to the object originating this event.</param>
		/// <param name="args" >Remote diagnostic logger object.</param>
		[OneWay]
		public void DiagnosticLogItemReceived(object sender, DiagnosticsLogItem args)
		{
			lock (m_LockDiagnosticLogItemReceived)
			{
				try
				{
					if (DiagnosticsLogWriteEvent != null)
					{
						DiagnosticsLogWriteEvent(sender, args);
					}
				}
				catch
				{
					// Ignore event handling exceptions - do not pass them back to the server
				}
			}
		}
		#endregion
	}
}
