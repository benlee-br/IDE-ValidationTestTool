using System;
using System.IO;
using System.ServiceProcess;
using System.Diagnostics;
using System.Text;
using System.Configuration;

using System.Threading;
using BioRad.Common.DiagnosticsLogger;

namespace BioRad.Common.Enumeration
{
    #region Documentation Tags
    /// <summary>
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
    ///			<item name="vssfile">$Workfile: EnumeratorServiceController.cs $</item>
    ///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Enumeration/EnumeratorServiceController.cs $</item>
    ///			<item name="vssrevision">$Revision: 38 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Blie $</item>
    ///			<item name="vssdate">$Date: 7/23/09 12:19p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

	public partial class DiscoveryServer : IDisposable
	{
		#region Constants
		#endregion

		#region Member Data
		private Process m_ServerProcess;
        private EnumeratorWatcher m_Watcher;
        private NewEnumerationInfoEventHandler m_Handler;
		private DiagnosticsLog m_Log;
		private string m_ServerName;
        #endregion

        #region Accessors
        /// <summary>
        /// Determine if instrument discovery server is running.
        /// </summary>
        public bool IsServerRunning
        {
            get{return (m_ServerProcess != null) ? true : true;}
        }
        #endregion

        #region Constructors and Destructor
		/// <summary>
		///Initializes a new instance of the EnumeratorServiceController class.       
		/// </summary>
		/// <param name="serverName"></param>
        /// <param name="enumeratorHandler"></param>
        /// <param name="OnProcessExit"></param>
		public DiscoveryServer(
			string serverName,
			NewEnumerationInfoEventHandler enumeratorHandler,
            EventHandler OnProcessExit)
        {
            if (enumeratorHandler == null)
                throw new ArgumentNullException("enumeratorHandler");
			if (serverName == null)
				throw new ArgumentNullException("serverName");

			m_ServerName = serverName;
			m_Log = DiagnosticsLogService.GetService.
					GetDiagnosticsLog("DiscoveryServer");

            m_Handler = enumeratorHandler;

            m_ServerProcess = null;
			MaybeStartInstrumentDiscoveryServer(OnProcessExit);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Force a new enumeration of MiniOpticons instruments.
        /// </summary>
        public void ForceEnumeration()
        {
            if ( m_Watcher != null )
                m_Watcher.NewEnumeration();
        }
		/// <summary>
		/// Start instrument discovery server if not running.
		/// </summary>
        private void MaybeStartInstrumentDiscoveryServer(EventHandler OnProcessExit)
        {
            bool simulation = false;
            if (ConfigurationManager.AppSettings["simulation"] != null)
                simulation = true;

            string path = EnumerationUtils.GetEnumeratedConnectedInstrumentsFolderName();
            if (simulation)
                path = ApplicationPath.SimulatedConnectedInstrumentsFolderPath();
            string connectedInstrumentsFolderName = path;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // Subscribe to enumerator change events.
            m_Watcher = new EnumeratorWatcher(
                path,
                ApplicationPath.MiniOpticonConnectedInstrumentsFilename(),
                Blocks.MiniOpticon);
            m_Watcher.EnumerationEvent +=
                new NewEnumerationInfoEventHandler(m_Handler);

            path = Path.Combine(ApplicationPath.DirectoryPath, m_ServerName);

            bool forceEnumeration = true;

            if (File.Exists(path))
            {
                Process[] processList = Process.GetProcessesByName
                    (Path.GetFileNameWithoutExtension(m_ServerName));

                if (processList.Length == 0)
                {
                    if (!simulation)
                    {
                        // If the server is not running and we have a connected instrument file then
                        // delete file the connected instrument file. The server updates this file 
                        // periodically with connect instrument data. If the server is not running then
                        // this file may contain wrong data.
                        string file = Path.Combine(connectedInstrumentsFolderName, ApplicationPath.MiniOpticonConnectedInstrumentsFilename());
                        if (File.Exists(file))
                            FileUtilities.DeleteFile(file);
                    }


                    m_ServerProcess = new Process();
                    m_ServerProcess.StartInfo.FileName = path;

                    // Enumeration directory must exists before starting MiniOpticon discovery server.
                    // MiniOpticon server will discovery what MiniOpticon devices are connected
                    // and write to the following folder and file name.
                    path = EnumerationUtils.GetEnumeratedConnectedInstrumentsFolderName();
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    StringBuilder sb = new StringBuilder();
                    sb.Append('"');//#1
                    sb.Append(path);
                    sb.Append('"');

                    sb.Append(' ');//#2
                    sb.Append('"');
                    sb.Append(ApplicationPath.MiniOpticonConnectedInstrumentsFilename());
                    sb.Append('"');

                    string arg3 = string.Format("simulated:{0};CyclerVersion:{1};DSPVersion:{2}",
                        ApplicationStateData.GetInstance.IsSimulation.ToString(),
                        ApplicationStateData.GetInstance.MiniOpticonCyclerVersion,
                        ApplicationStateData.GetInstance.MiniOpticonDSPVersion);
                    sb.Append(' ');//#3
                    sb.Append('"');
                    sb.Append(arg3);//defect 10120
                    sb.Append('"');

                    // Instrument directory must exists before starting server.
                    // Server and client use the instrument directory for communication.
                    // Tell server location of instrument directory.
                    if (!Directory.Exists(ApplicationPath.InstrumentDirectory))
                    {
                        DirectoryInfo info =
                            Directory.CreateDirectory(ApplicationPath.InstrumentDirectory);
                    }
                    sb.Append(' ');//#4
                    sb.Append('"');//instrument directory
                    sb.Append(ApplicationPath.InstrumentDirectory);
                    sb.Append('"');

					// Added regulator flag
					sb.Append(' ');//#5
					sb.Append('"');//instrument directory
					sb.Append(ApplicationStateData.GetInstance.IsRegulatory);
					sb.Append('"');

					// need 1st argument to be able to run this program. 
                    m_ServerProcess.StartInfo.Arguments = sb.ToString();
					m_ServerProcess.StartInfo.CreateNoWindow = true;
					m_ServerProcess.StartInfo.UseShellExecute = false;
                    m_ServerProcess.EnableRaisingEvents = true;
                    m_ServerProcess.Exited += new EventHandler(OnProcessExit);

                    this.m_Log.Debug(
                      string.Format(Properties.Resources.DNL_StartMiniOpticonServerArguments, sb.ToString()));

                    if (!m_ServerProcess.Start())
                        m_ServerProcess = null;
                    else
                        forceEnumeration = false;
                }
                else
                    m_ServerProcess = processList[0];
            }

            if (forceEnumeration)
                m_Watcher.NewEnumeration();
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
                if (m_Watcher != null)
                {
                    // unsubscribe
                    m_Watcher.EnumerationEvent -=
                        new NewEnumerationInfoEventHandler(m_Handler);
                    m_Watcher.Dispose();
                }
                m_Watcher = null;

                // Kill the instrument discovery process.
                if (m_ServerProcess != null && !m_ServerProcess.HasExited)
                    m_ServerProcess.Kill();
			}
        }
        #endregion
    }
}
