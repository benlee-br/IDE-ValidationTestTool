using System;
using System.Management;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace BioRad.Common.UsbDevice
{
	#region Documentation Tags
	/// <summary>
	/// Detect USB devices attach/dettach events.
	/// </summary>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Ralph Ansell</item>
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
    ///			<item name="vssfile">$Workfile: UsbDetect.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/USB/UsbDetect.cs $</item>
	///			<item name="vssrevision">$Revision: 1 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 2/02/10 1:28p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public class UsbDetect : IDisposable
    {
        #region Member Data
        private object m_Lock = new object();
        private ManagementEventWatcher m_CreationEvents = null;
		private ManagementEventWatcher m_DeletionEvents = null;
		private ManagementScope m_Scope;
        private EventArrivedEventHandler m_CreationHandler;
        private EventArrivedEventHandler m_DeletionHandler;
		#endregion

        #region Delegates and Events
        /// <summary>USB attach/detach change event.</summary>
        public event UsbChangeDelegate UsbDeviceChangeEvent;
        /// <summary>USB change delegate.</summary>
        /// <param name="args">Event arguments.</param>
        public delegate void UsbChangeDelegate(UsbChangeEventArgs args);
        #endregion

		#region Constructors and Destructor
		/// <summary>
        /// Initializes a new instance of the USBDetect class.
		/// </summary>
		public UsbDetect()
		{
            Init("root\\CIMV2");
		}
        /// <summary>
        /// Initializes a new instance of the USBDetect class.
        /// </summary>
        public UsbDetect(string path)//todo ???
        {
            Init(path);
        }
		#endregion

		#region Methods
        private void Init(string path)
        {
            m_Scope = new ManagementScope(path);
            this.SubScribeToCreationEvents(UsbAttachEvent);
            this.SubScribeToDeletionEvents(UsbDetachEvent);
        }
        private void AsyncFireChangeEvent(UsbChangeEventArgs.UsbEventType type, string deviceInstanceID)
        {
            UsbChangeEventArgs args = new UsbChangeEventArgs(type, deviceInstanceID);
            ThreadPool.QueueUserWorkItem(new WaitCallback(SendUsbChangeEvent), args);
        }
        private void SendUsbChangeEvent(object args)
        {
            UsbChangeDelegate handler;
            lock (m_Lock)
            {
                handler = UsbDeviceChangeEvent;
            }
            if (handler != null)//is there any subscribers?
            {
                handler(args as UsbChangeEventArgs);
            }
        }
		/// <summary>
		/// Subscribe to USB attach events.
		/// </summary>
		private void SubScribeToCreationEvents(EventArrivedEventHandler handler)
		{
			WqlEventQuery q = new WqlEventQuery();
			q.EventClassName = "__InstanceCreationEvent";
			q.WithinInterval = new TimeSpan(0, 0, 3);
			q.Condition = @"TargetInstance ISA 'Win32_USBControllerDevice' ";
            m_CreationHandler = handler;
			m_CreationEvents = new ManagementEventWatcher(m_Scope, q);
			m_CreationEvents.EventArrived += new EventArrivedEventHandler(handler);
		    m_CreationEvents.Start();
		}
		/// <summary>
        /// Subscribe to USB detach events.
		/// </summary>
		private void SubScribeToDeletionEvents(EventArrivedEventHandler handler)
		{
			WqlEventQuery q = new WqlEventQuery();
			q.EventClassName = "__InstanceDeletionEvent";
			q.WithinInterval = new TimeSpan(0, 0, 3);
			q.Condition = @"TargetInstance ISA 'Win32_USBControllerDevice' ";
            m_DeletionHandler = handler;
			m_DeletionEvents = new ManagementEventWatcher(m_Scope, q);
			m_DeletionEvents.EventArrived += new EventArrivedEventHandler(handler);
			m_DeletionEvents.Start(); 
		}
		/// <summary>
		/// Stop listening to USB events.
		/// </summary>
		private void StopListening()
		{
			if (m_CreationEvents != null)
			{
                m_CreationEvents.EventArrived -=
                    new EventArrivedEventHandler(m_CreationHandler);
				m_CreationEvents.Stop();
				m_CreationEvents.Dispose();
				m_CreationEvents = null;
			}

			if (m_DeletionEvents != null)
			{
                m_DeletionEvents.EventArrived -=
                    new EventArrivedEventHandler(m_DeletionHandler);
				m_DeletionEvents.Stop();
				m_DeletionEvents.Dispose();
				m_DeletionEvents = null;
			}
		}
		/// <summary>
		/// Releases the resources used by the Diagnostics Log Service.
		/// </summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			Dispose(true);
		}
		/// <summary>
		/// Releases the resources used by the diagnostics logs and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing"></param>
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				StopListening();
			}
		}
		#endregion

        #region Events
        private void UsbAttachEvent(object sender, System.Management.EventArrivedEventArgs e)
        {
            System.Management.ManagementBaseObject mbo =
                (System.Management.ManagementBaseObject)e.NewEvent["TargetInstance"];
            using (System.Management.ManagementObject o = new
                System.Management.ManagementObject(mbo["Dependent"].ToString()))
            {
                o.Get();
                string deviceInstanceID = o.Properties["DeviceID"].Value.ToString();

                AsyncFireChangeEvent(UsbChangeEventArgs.UsbEventType.attach, deviceInstanceID);
            }
        }
        private void UsbDetachEvent(object sender, System.Management.EventArrivedEventArgs e)
        {
            System.Management.ManagementBaseObject mbo =
                 (System.Management.ManagementBaseObject)e.NewEvent["TargetInstance"];

            string path = mbo["Dependent"].ToString();
            string deviceInstanceID = path.Replace("\\\\", "\\");

            AsyncFireChangeEvent(UsbChangeEventArgs.UsbEventType.detach, deviceInstanceID);
        }

        #endregion
    }
}
