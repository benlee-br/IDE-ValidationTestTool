using System;
using System.Management;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Collections;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Class Summary
	/// </summary>
	/// <remarks>
	/// SRS788 - Denali3 software should check if Opticon Monitor is using MiniOpticon device.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="conformancereview">Conformance review:</item>
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
	///			<item name="vssfile">$Workfile: ProcessWatcher.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/ProcessWatcher.cs $</item>
	///			<item name="vssrevision">$Revision: 9 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 1/05/07 1:55p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class ProcessWatcher : IDisposable
	{
		#region Member Data
		private ManagementEventWatcher m_ProcessStart;
		private ManagementEventWatcher m_ProcessStop;
		private ArrayList m_Processes2Watch;
        private bool m_WatchedProcessRunning;
        private object sync = new object();
        private EventArrivedEventHandler m_EventArrivedEventHandler;
		#endregion

		#region Accessors
		/// <summary>
		/// Returns true if atleast one of the processes to watch is running.
		/// </summary>
		public bool IsWatchedProcessRunning
		{
            get { return m_WatchedProcessRunning; }
		}
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Initializes a new instance of the ProcessWatcher class.
		/// </summary>
        public ProcessWatcher(EventArrivedEventHandler eventArrivedEventHandler)
		{
            m_EventArrivedEventHandler = eventArrivedEventHandler;
            m_Processes2Watch = new ArrayList(0);
            m_WatchedProcessRunning = false;

			// watch for stopped processes.
			m_ProcessStop = new ManagementEventWatcher(
				new EventQuery("select * from Win32_ProcessStopTrace"));
			m_ProcessStop.EventArrived += new EventArrivedEventHandler(ProcessStopHandler);
			m_ProcessStop.Start();

			// watch for newly started processes.
			m_ProcessStart = new ManagementEventWatcher(
				 new EventQuery("select * from Win32_ProcessStartTrace"));
			m_ProcessStart.EventArrived += new EventArrivedEventHandler(ProcessStartHandler);
			m_ProcessStart.Start();
		}
		#endregion

		#region Methods
        /// <summary>
        /// Add friendly name of a process to watch.
        /// </summary>
        public void Add(string processName)
        {
            if (processName == null || processName.Length == 0)
                throw new ArgumentNullException("processName");
            lock (sync)
            {
                m_Processes2Watch.Add(processName);
            }
        }
		/// <summary>
		/// Explicitly releases all resources used by this object.
		/// </summary>
		public void Dispose()
		{
			// Call the overridden Dispose method that contains common cleanup code
			// Pass true to indicate that it is called from Dispose
			Dispose(true);
			// Prevent subsequent finalization of this object. This is not needed 
			// because managed and unmanaged resources have been explicitly released
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// Releases the unmanaged resources used by the object and optionally 
		/// releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; 
		/// false to release only unmanaged resources. 
		/// </param>
		/// <remarks>
		/// Dispose(bool disposing) executes in two distinct scenarios.
		/// If disposing equals true, the method has been called directly
		/// or indirectly by a user's code. Managed and unmanaged resources
		/// can be disposed.
		/// If disposing equals false, the method has been called by the 
		/// runtime from inside the finalizer and you should not reference 
		/// other objects. Only unmanaged resources can be disposed.
		/// </remarks>
		internal void Dispose(bool disposing)
		{
			if (disposing)
			{
			}

            if (m_Processes2Watch != null)
			{
                m_Processes2Watch.Clear();
                m_Processes2Watch = null;
			}

			if (m_ProcessStart != null)
			{
				m_ProcessStart.EventArrived -= new EventArrivedEventHandler(ProcessStartHandler);
				m_ProcessStart.Dispose();
				m_ProcessStart = null;
			}

			if (m_ProcessStop != null)
			{
				m_ProcessStop.EventArrived -= new EventArrivedEventHandler(ProcessStopHandler);
				m_ProcessStop.Dispose();
				m_ProcessStop = null;
			}
		}
		/// <summary>
		/// Check if any watched processes are running.
		/// </summary>
		/// <returns>True if atleast one process to watch is running.</returns>
        public bool AnyWatchedProcessesRunning()
        {
            lock (sync)
            {
				m_WatchedProcessRunning = false;

                foreach (string watch in m_Processes2Watch)
                {
                    Process[] processes = Process.GetProcessesByName(
                        Path.GetFileNameWithoutExtension(watch));

                    if (processes.Length > 0)
                    {
                        m_WatchedProcessRunning = true;
                        break;
                    }
                }
            }
			return m_WatchedProcessRunning;
        }
		#endregion

		#region Event Handlers
		private void ProcessStartHandler(object sender, EventArrivedEventArgs e)
		{
            //PropertyDataCollection props = e.NewEvent.Properties;
            //Console.WriteLine("Process ID: {0};\tProcess Name: {1}",
            //    props["ProcessID"].Value, props["ProcessName"].Value);
            m_EventArrivedEventHandler(sender, e);
		}
        private void ProcessStopHandler(object sender, EventArrivedEventArgs e)
        {
            m_EventArrivedEventHandler(sender, e);
        }
		#endregion
	}
}
