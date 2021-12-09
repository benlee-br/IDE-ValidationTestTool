using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// ApplicationPath Class provides directory path information for the current process.
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Ralph Ansell</item>
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
	///			<item name="vssfile">$Workfile: ApplicationPath.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Common/ApplicationPath.cs $</item>
	///			<item name="vssrevision">$Revision: 72 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Thouser $</item>
	///			<item name="vssdate">$Date: 2/09/11 5:23p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public sealed partial class ApplicationPath
	{
		#region Constants
		private static readonly string c_InstrumentDirectoryName = "Instrument";
		private static readonly string c_TempDirectoryName = "Temp";
		private static readonly string c_ConfigDirectoryName = "Config";
		private static readonly string c_IstImagesDirectoryName = "Images\\IstImages\\";
		private static readonly string c_SoftwareUpdatesServerLocalMirrorFolderName = "Firmware";
		/// <summary>
		/// Log file extension.
		/// </summary>
		private const string c_LogFileExtension = @".xml";
		#endregion

		#region Accessors
		/// <summary>Get the upgrade code for application.</summary>
		/// <remarks>Upgrade code is used to identify this application.</remarks>
		public static string ApplicationUpgradeCode
		{
			get
			{
				string upgradeCode = string.Empty;
				if (ConfigurationManager.AppSettings["UpgradeCode"] != null)
					upgradeCode = ConfigurationManager.AppSettings["UpgradeCode"].ToString();

				return upgradeCode;
			}
		}

        /// <summary>Get the upgrade code for CFX Opus application.</summary>
        /// <remarks>Upgrade code is used to identify this application.</remarks>
        public static string CFXOpusUpgradeCode
        {
            get
            {
                string upgradeCode = string.Empty;
                if (ConfigurationManager.AppSettings["CfxOpusUpgradeCode"] != null)
                    upgradeCode = ConfigurationManager.AppSettings["CfxOpusUpgradeCode"].ToString();

                return upgradeCode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string UserDocsUpgradeCode
        {
            get
            {
                string upgradeCode = string.Empty;
//TODO: We could move the following upgradeCode to app.config and enable code below.
//That said, CFX Maestro 1.1 branch does not really need it since other CFX Editions don't use Automatic updates.
#if false
                if (ConfigurationManager.AppSettings["UserDocsCode"] != null)
                    upgradeCode = ConfigurationManager.AppSettings["UserDocsCode"].ToString();
#else
                upgradeCode = "C2399CB7-5106-43C9-B824-7C5DDC76F57D";
#endif
                return upgradeCode;
            }
        }

        /// <summary>Get the upgrade code for prime pcr files.</summary>
        public static string PrimePcrUpgradeCode
		{
			get
			{
				string upgradeCode = string.Empty;
				if (ConfigurationManager.AppSettings["PrimePcrUpgradeCode"] != null)
					upgradeCode = ConfigurationManager.AppSettings["PrimePcrUpgradeCode"].ToString();

				return upgradeCode;
			}
		}
		/// <summary>
		/// Path to the local mirror of the software website's upgrades folder.
		/// </summary>
		public static string SoftwareUpdatesServerLocalMirrorPath
		{
			get
			{
				return Path.Combine(Path.GetDirectoryName(ApplicationPath.ExecutablePath), c_SoftwareUpdatesServerLocalMirrorFolderName);
			}
		}
		/// <summary>
		/// Get software updates URL.
		/// </summary>
		public static string SoftwareUpdatesURL
		{
			get
			{
				string url = "SoftwareUpdatesURL";

				if (ConfigurationManager.AppSettings[url] != null)
				{
					url = ConfigurationManager.AppSettings[url].ToString();
					url = url.Trim();
					url = url.TrimEnd('/');
					url = url + '/';
					Uri u = new Uri(url, UriKind.Absolute);
					url = u.AbsoluteUri;
				}
				else
				{
					Debug.Assert(false, "Cannot find software updates URL in app.config.");
					url = string.Empty;
				}
				return url;
			}
		}
		/// <summary>
		/// Filename on updates server for updates config file.
		/// </summary>
		public static string SoftwareUpdatesConfigFileName
		{
			get
			{
				string configKey = "SoftwareUpdatesConfigFilename";
				if (ConfigurationManager.AppSettings[configKey] != null)
				{
					return ConfigurationManager.AppSettings[configKey];
				}
				else
				{
					Debug.Assert(false, "Filename is not in config file.");
					return "updates.xml.bin";
				}
			}
		}
		/// <summary>
		/// Get software updates local path.
		/// </summary>
		public static string SoftwareUpdatesLocalPath
		{
			get
			{
				return Path.Combine(ApplicationPath.InstrumentDirectory, "SoftwareUpdates");
			}
		}
		/// <summary>
		/// Instrument directory path.
		/// </summary>
		public static string InstrumentDirectory
		{
			get
			{
				string path =
					 Path.Combine(ApplicationPath.CommonApplicationDataPath(), c_InstrumentDirectoryName);
				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);
				Debug.Assert(Directory.Exists(path));
				return path;
			}
		}
        /// <summary>
        /// Return ApplicationName unless it is "BioRadCFXManager" in which case we return "CFX Maestro"
        /// </summary>
        static public string ApplicationNameForLog
        {
            get
            {
                //fix for TT #864-[CFX] Log file display  “CFX M... Standard Edition started.” Should be “CFX Maestro
                string name = ApplicationName;
                if (name.ToUpper() == "BIORADCFXMANAGER") //DNL DO NOT LOCALIZE
                {
                    name = "CFX Maestro"; //DNL DO NOT LOCALIZE
                }
                
                return name;
            }
        }
		/// <summary>
		/// Returns the application name as set in the current application domain.
		/// If not set, returns executable w/ out path or extension.
		/// </summary>
		static public string ApplicationName
		{
			get
			{
				string name = AppDomain.CurrentDomain.SetupInformation.ApplicationName;
				if (name == null)
				{
					name = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
				}
				else
					name = Path.GetFileNameWithoutExtension(name);

				return name;
			}
		}
		/// <summary>
		/// Returns the full path of the configuration file for the current application domain.
		/// </summary>
		static public string ConfigFilePath
		{
			get
			{
				return GetFullPath(ConfigFileName);
			}
		}

		/// <summary>
		/// Returns the file name (including extension) of the configuration file for the current
		/// application domain.
		/// </summary>
		static public string ConfigFileName
		{
			get
			{
				return AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
			}
		}

		/// <summary>
		/// Returns the full directory path of the application base location.
		/// </summary>
		static public string DirectoryPath
		{
			get
			{
				return AppDomain.CurrentDomain.BaseDirectory;
			}
		}
		/// <summary>
		/// The fully qualified path of the application executable.
		/// </summary>
		static public string ExecutablePath
		{
			get
			{
				// TODO: If application started via another process, that process is the
				// executable. 
				return Process.GetCurrentProcess().MainModule.FileName;
			}
		}
		/// <summary>
		/// Returns the file name and extension of the application executable.
		/// </summary>
		static public string ExecutableFileNameWithoutExtension
		{
			get
			{
				return Path.GetFileNameWithoutExtension(ExecutablePath);

			}
		}
        /// <summary>
        /// 
        /// </summary>
        static public bool GeneExpressionTrace
        {
            get
            {
                return File.Exists(Path.Combine(ApplicationPath.CommonApplicationDataPath(), "GeneExpressionTrace"));
            }
        }
        /// <summary>
        /// Returns the file name of the application without the extension.
        /// </summary>
        static public string ExecutableFileName
		{
			get
			{
				return Path.GetFileName(ExecutablePath);

			}
		}

		/// <summary>
		/// Returns the file version of the application executable file.
		/// </summary>
		public static string ApplicationFileVersion
		{
			get
			{
				return ApplicationVersionInfo.FileVersion;
			}
		}

		/// <summary>
		/// Returns the file version of the application executable file.
		/// </summary>
		public static FileVersionInfo ApplicationVersionInfo
		{
			get
			{
				// TODO: This gets the version of the process that started the application,
				// not the application itself.
				return Process.GetCurrentProcess().MainModule.FileVersionInfo;
			}
		}
		/// <summary>
		/// Creates the application temp directory if it does not exists.
		/// </summary>
		/// <returns>Full path to the application temp directory.</returns>
		public static string TempDirectory
		{
			get
			{
				// Does the user have an existing temp directory?
				string tempPath = Path.Combine(CommonApplicationDataPath(), c_TempDirectoryName);
				if (!Directory.Exists(tempPath))
				{
					Directory.CreateDirectory(tempPath);
				}
				return tempPath;
			}
		}
		/// <summary>
		/// Creates the All User Shared Docs temp directory if it does not exists.
		/// </summary>
		/// <returns>Full path to the All User Shared Docs temp directory.</returns>
		public static string TempDirectoryAllUsersSharedDocs
		{
			get
			{
				// Does the user have an existing temp directory?
				string tempPath = Path.Combine(AllUsersSharedDocumentsPath(), c_TempDirectoryName);
				if (!Directory.Exists(tempPath))
				{
					Directory.CreateDirectory(tempPath);
				}
				return tempPath;
			}
		}
		/// <summary>
		/// Get the application configuration directory for "All Users", not where application installed.
		/// </summary>
		/// <returns>Full path to the application configuration directory.</returns>
		public static string ConfigDirectory
		{
			get
			{
				string path = Path.Combine(CommonApplicationDataPath(), c_ConfigDirectoryName);
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				return path;
			}
		}

		/// <summary>
		/// Get the IST images source directory
		/// </summary>
		public static string IstImagesDirectory
		{
			get
			{
				return Path.Combine(ApplicationPath.DirectoryPath, c_IstImagesDirectoryName);
			}
		}
		/// <summary>The full path to the content folder. This folder contains the predesigned and custom run
		/// set files in sub-folders.The names for the sub-folders can be obtained using 
		/// ApplicationStateData.Setting.ContentFolderNamePreDesMouse,
		/// ApplicationStateData.Setting.ContentFolderNamePreDesHuman and 
		/// ApplicationStateData.Setting.ContentFolderNameCustom</summary>
		public static string ContentDirectory
		{
			get
			{
				string path = Path.Combine(AllUsersSharedDocumentsPath(),
					ApplicationStateData.GetInstance[ApplicationStateData.Setting.ContentFolderName] as string);
				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);

				return path;
			}
		}
#endregion

#region Constructors and Destructor
		/// <summary>
		/// Private constructor prevents construction.
		/// </summary>
		private ApplicationPath()
		{
		}
#endregion

#region Methods
		/// <summary>
		/// Returns special folder path "CommonApplicationData" combined with our application.
		/// e.g. "\Documents and Settings\All Users\Application Data\Bio-Rad\CFX".
		/// The folder "Bio-Rad\CFX" is set in applicationSettings.xml file.
		/// </summary>
		/// <returns></returns>
		public static string CommonApplicationDataPath()
		{
			string path = Path.Combine(Environment.GetFolderPath(
				 Environment.SpecialFolder.CommonApplicationData),
				 ApplicationStateData.GetInstance[
				 ApplicationStateData.Setting.ApplicationDataPath].ToString());
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			return path;
		}
		/// <summary>
		/// Returns special folder path "LIMSProtocolCommonApplicationData" combined with our application.
		/// e.g. "\Documents and Settings\All Users\Application Data\Bio-Rad\CFX" with sub-folder 
        /// "..\LIMS\LIMS Protocols".
        ///</summary>
		/// <returns></returns>
		public static string LIMSProtocolCommonApplicationDataPath()
		{
            string path = AllUsersSharedDocumentsPath();
			path = Path.Combine(path, "LIMS\\LIMS Protocols");
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			return path;
		}
		/// <summary>
		/// Returns special folder path "LIMSCommonApplicationData" combined with our application.
        /// e.g. "\Documents and Settings\All Users\Application Data\Bio-Rad\CFX" with sub-folder 
        /// "..\LIMS".
		/// </summary>
		/// <returns></returns>
		public static string LIMSCommonApplicationDataPath()
		{
            string path = AllUsersSharedDocumentsPath();
			path = Path.Combine(path, "LIMS");
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			return path;
		}
        /// <summary>
        /// Returns special folder path "PrimePCRCommonApplicationData" combined with our application.
        /// e.g. "\Documents and Settings\All Users\Application Data\Bio-Rad\CFX" with the sub-folder "..\PrimePCR" .
        /// </summary>
        /// <returns></returns>
        public static string PrimePCRCommonApplicationDataPath()
        {
            string path = AllUsersSharedDocumentsPath();
            path = Path.Combine(path, "PrimePCR");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
		/// <summary>
		/// Returns All Users profile's Shared Documents folders path for current application.
		/// e.g. "\Documents and Settings\All Users\Documents\Bio-Rad\CFX".
		/// The folder "Bio-Rad\CFX" is set in applicationSettings.xml file.
		/// </summary>
		/// <returns></returns>
		public static string AllUsersSharedDocumentsPath()
		{
            //PW: string not to be localized.
            string path;

            if (ApplicationStateData.GetInstance.IsMacApplication)
            {
                path = GetUnixHomeTest.GetDefaultSaveLocation();
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            else
            {
                //Default to make sure it works in XP and W7.
                string allUsersFolder = Path.Combine(
                 Environment.GetEnvironmentVariable("ALLUSERSPROFILE"),
                 "Documents");
                //Change for W7
                string publicFolder = Environment.GetEnvironmentVariable("PUBLIC");
                if (publicFolder != null && !string.IsNullOrEmpty(publicFolder))
                {
                    allUsersFolder = Path.Combine(publicFolder, "Documents");
                }

                path = Path.Combine(allUsersFolder,
                    ApplicationStateData.GetInstance[
                    ApplicationStateData.Setting.ApplicationDataPath].ToString());
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            
            return path;
		}

		/// <summary>
		/// Convert filename into fully qualified path that defines the location of the file. 
		/// Appends the argument, filename, to directory path of the directory of the current
		/// application.
		/// </summary>
		/// <remarks>If filename is rooted, it is unchanged.</remarks>
		/// <param name="filename">File name with extension.</param>
		/// <returns>Returns fully qualified path.</returns>
		static public string GetFullPath(string filename)
		{
			//The Exe is always located in the directory path
			return Path.Combine(DirectoryPath, filename);
		}
		/// <summary>
		/// Get absolute path to MJ globber executable.
		/// </summary>
		/// <returns></returns>
		static public string GetGlobberExeAbsoultePath()
		{
			return ApplicationPath.GetFullPath("BioRad.Glob.exe"); ;
		}
		/// <summary>
		/// Get absolute path to globber2 executable.
		/// </summary>
		/// <returns></returns>
		static public string GetGlobber2ExeAbsoultePath()
		{
			return Path.Combine(DirectoryPath, "BioRadGlob2.exe");
		}
        /// <summary>
        /// Get absolute path to Batch Analyzer executable.
        /// </summary>
        /// <returns></returns>
        static public string GetBatchAnalyzerExeAbsoultePath()
        {
            return Path.Combine(DirectoryPath, "BioRadCFXBatchAnalyzer.exe");
        }

		/// <summary>
		/// Get simulated connected instruments folder path.
		/// </summary>
		/// <remarks>
		/// This folder contains two files. Each file contains connection information
		/// about connected instrument discovered via enumeration.
		/// If folder does not exists it is created.
		/// </remarks>
		/// <returns>Exsiting folder path of simulated connected instruments.</returns>
		public static string SimulatedConnectedInstrumentsFolderPath()
		{
			string path = Path.Combine(CommonApplicationDataPath(),
			  ApplicationStateData.GetInstance[
			  ApplicationStateData.Setting.SimulatedConnectedInstrumentsFolderName].ToString());
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			return path;
		}
		/// <summary>
		/// Get miniopticon instrument connected instrument filename. This file contains data for each miniopticon connected instruments.
		/// </summary>
		/// <returns></returns>
		public static string MiniOpticonConnectedInstrumentsFilename()
		{
			return ApplicationStateData.GetInstance[
			  ApplicationStateData.Setting.MiniOpticonConnectedInstrumentsFilename].ToString();
		}
		/// <summary>
		/// Get CFX instrument connected instrument filename. This file contains data for each CFX connected instruments.
		/// </summary>
		/// <returns></returns>
		public static string FlagshipConnectedInstrumentsFilename()
		{
			return ApplicationStateData.GetInstance[
			  ApplicationStateData.Setting.FlagshipConnectedInstrumentsFilename].ToString();
		}
		/// <summary>
		/// Get locust instrument connected instrument filename. This file contains data for each locust connected instruments.
		/// </summary>
		/// <returns></returns>
		public static string LocustConnectedInstrumentsFilename()
		{
			return ApplicationStateData.GetInstance[
			  ApplicationStateData.Setting.LocustConnectedInstrumentsFilename].ToString();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CFX3GConnectedInstrumentsFilename()
        {
            return ApplicationStateData.GetInstance[
              ApplicationStateData.Setting.CFX3GConnectedInstrumentsFilename].ToString();
        }
        /// <summary>
        /// Gets information about the running application process and some system data.
        /// Does not include perfomance counters.
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationProcessInfo()
		{
			Process myProcess = Process.GetCurrentProcess();
			StringBuilder sb = new StringBuilder(1000);
			sb.Append("Application Process Information;");
			sb.Append("; Process.ProcessName: "); sb.Append(myProcess.ProcessName.ToString());
			sb.Append("; Process.TotalProcessorTime: "); sb.Append(myProcess.TotalProcessorTime.ToString());
			sb.Append("; Process.UserProcessorTime: "); sb.Append(myProcess.UserProcessorTime.ToString());
			sb.Append("; Process.StartTime: "); sb.Append(myProcess.StartTime.ToString());
			sb.Append("; Process.BasePriority: "); sb.Append(myProcess.BasePriority.ToString());
			sb.Append("; Process.HandleCount: "); sb.Append(myProcess.HandleCount.ToString());
			sb.Append("; Process.Threads: "); sb.Append(myProcess.Threads.Count.ToString());
			sb.Append("; Process.PrivateMemorySize: "); sb.Append((myProcess.PrivateMemorySize64 / 1024).ToString());
			sb.Append("; Process.WorkingSet: "); sb.Append((myProcess.WorkingSet64 / 1024).ToString());
			sb.Append("; Process.PeakWorkingSet: "); sb.Append((myProcess.PeakWorkingSet64 / 1024).ToString());
			sb.Append("; Process.MinWorkingSet: "); sb.Append(((int)(myProcess.MinWorkingSet) / 1024).ToString());
			sb.Append("; Process.MaxWorkingSet: "); sb.Append(((int)(myProcess.MaxWorkingSet) / 1024).ToString());
			sb.Append("; Process.NonpagedSystemMemorySize: "); sb.Append((myProcess.NonpagedSystemMemorySize64 / 1024).ToString());
			sb.Append("; Process.PagedSystemMemorySize: "); sb.Append((myProcess.PagedSystemMemorySize64 / 1024).ToString());
			sb.Append("; Process.PagedMemorySize: "); sb.Append((myProcess.PagedMemorySize64 / 1024).ToString());
			sb.Append("; Process.PeakPagedMemorySize: "); sb.Append((myProcess.PeakPagedMemorySize64 / 1024).ToString());
			sb.Append("; Process.VirtualMemorySize: "); sb.Append((myProcess.VirtualMemorySize64 / 1024).ToString());
			sb.Append("; Process.PeakVirtualMemorySize: "); sb.Append((myProcess.PeakVirtualMemorySize64 / 1024).ToString());

			sb.Append("; Environment.OSVersion.VersionString: "); sb.Append(Environment.OSVersion.VersionString);
			sb.Append("; Environment.WorkingSet: "); sb.Append(Environment.WorkingSet.ToString());
			sb.Append("; Environment.CommandLine: "); sb.Append(Environment.CommandLine);
			foreach (string arg in Environment.GetCommandLineArgs())
			{
				sb.Append("; Environment.GetCommandLineArgs: "); sb.Append(arg);
			}
			sb.Append("; Environment.SpecialFolder.CommonApplicationData: "); sb.Append(
				Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
			sb.Append("; Environment.TickCount: "); sb.Append(Environment.TickCount);

			//TODO:
			//Size on disk
			//memory
			//processor

			int workerThreads, completionPortThreads;
			ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
			sb.Append("; ThreadPool.GetMaxThreads - workerThreads: "); sb.Append(workerThreads.ToString() +
				 " completionPortThreads: "); sb.Append(completionPortThreads.ToString());
			ThreadPool.GetAvailableThreads(out workerThreads, out  completionPortThreads);
			sb.Append("; ThreadPool.GetAvailableThreads - workerThreads: "); sb.Append(workerThreads.ToString() +
				 " completionPortThreads: "); sb.Append(completionPortThreads.ToString());

			long totalMem = GC.GetTotalMemory(false);
			sb.Append("; GC.GetTotalMemory: "); sb.Append((totalMem / 1024).ToString());

			return sb.ToString();
		}
		/// <summary>Gets information about the running application.</summary>
		/// <remarks>Expects "ClientApplicationFileName" and "AcquisitionApplicationFileName"
		/// to be defined in application configuration file. Expects executable files to be
		/// in the current application domain's base directory.</remarks>
		/// <param name="clientAppName">The name of the client application file (from
		/// application configuration file).</param>
		/// <param name="clientAppVersion">Version of the client application executable file.</param>
		/// <param name="daServerAppName">The name of the data acquisition application file (from
		/// application configuration file).</param>
		/// <param name="daServerAppVersion">Version of the data acquisition executable</param>
		public static void GetApplicationInfo(out string clientAppName,
			out string clientAppVersion, out string daServerAppName,
			out string daServerAppVersion)
		{
			ArrayList processNamesArray = new ArrayList();

			// TODO: This will only find application executables that reside together
			// Could it be modified to get file locations for processes?

			// Fix for Bug 10613 - use the ClientApplicationFileName and FlagshipServerName
			/*clientAppName = string.Concat(Process.GetCurrentProcess().ProcessName, ".exe");
			daServerAppName = 
				ApplicationStateData.GetInstance
				[ApplicationStateData.Setting.AcquisitionApplicationFileName].ToString();
			 * */
			clientAppName = ApplicationStateData.GetInstance
				[ApplicationStateData.Setting.ClientApplicationFileName].ToString();
			daServerAppName = ApplicationStateData.GetInstance
				[ApplicationStateData.Setting.FlagshipServerName].ToString();
			processNamesArray.Add(clientAppName);
			processNamesArray.Add(daServerAppName);

			clientAppVersion = string.Empty;
			daServerAppVersion = string.Empty;

			ApplicationFilesInfo[] applicationFilesInfoArray = FileUtilities.GetFileVersionInfo
				(DirectoryPath,
				(string[])processNamesArray.ToArray(typeof(string)));

			foreach (ApplicationFilesInfo appInfo in applicationFilesInfoArray)
			{
				if (appInfo.Name.Trim() == clientAppName.Trim())
					clientAppVersion = appInfo.Version.Trim();
				else if (appInfo.Name.Trim() == daServerAppName.Trim())
					daServerAppVersion = appInfo.Version.Trim();
			}
		}
		/// <summary>Gets information about the running client application.</summary>
		/// <remarks>Expects "ClientApplicationFileName" to be defined in application 
		/// configuration file. Expects executable files to be in the current 
		/// application domain's base directory.</remarks>
		/// <param name="clientAppName">The name of the client application file (from
		/// application configuration file).</param>
		/// <param name="clientAppVersion">Version of the client application executable file.</param>
		public static void GetClientApplicationInfo(out string clientAppName,
			out string clientAppVersion)
		{
			ArrayList processNamesArray = new ArrayList();

			// TODO: This will only find application executables that reside together
			// Could it be modified to get file locations for processes?
			clientAppName = string.Concat(Process.GetCurrentProcess().ProcessName, ".exe");
			processNamesArray.Add(clientAppName);

			clientAppVersion = string.Empty;

			ApplicationFilesInfo[] applicationFilesInfoArray = FileUtilities.GetFileVersionInfo
				(DirectoryPath,
				(string[])processNamesArray.ToArray(typeof(string)));

			foreach (ApplicationFilesInfo appInfo in applicationFilesInfoArray)
			{
				if (appInfo.Name.Trim() == clientAppName.Trim())
					clientAppVersion = appInfo.Version.Trim();
			}
		}
		/// <summary>Retrieve the assembly version information of dll and exe files in the 
		/// current application domain's base directory and the bin folder under it.</summary>
		/// <returns>List of application file info objects.</returns>
		public static ApplicationFilesInfo[] GetApplicationInfo()
		{
			ArrayList applicationFilesInfoArray = new ArrayList();
			Hashtable fileExtension = new Hashtable();
			fileExtension.Add(".dll", ".dll");
			fileExtension.Add(".exe", ".exe");

			applicationFilesInfoArray.AddRange(FileUtilities.GetFileVersionInfo
				(DirectoryPath, fileExtension));

			// check if there is a bin folder also
			string binFolder = Path.Combine(DirectoryPath, "bin");
			if (Directory.Exists(binFolder))
			{
				applicationFilesInfoArray.AddRange(FileUtilities.GetFileVersionInfo
					(binFolder, fileExtension));
			}
			return (ApplicationFilesInfo[])applicationFilesInfoArray.ToArray
				(typeof(ApplicationFilesInfo));

		}
		/// <summary>
		/// Returns the history log file path as per the app settings.
		/// </summary>
		/// <returns>the history log file path as per the app settings.</returns>
		public static string GetDefaultHistoryLogPath()
		{
			string str = string.Empty;
			// Fix for Bug 3757 - using Path.Combine instead of a stringBuilder
			// Path.Combine automatically puts the slashes in the path as and when needed
			if ((ConfigurationManager.AppSettings["LogFilesPath"] != null) &&
				 (ConfigurationManager.AppSettings
			["ApplicationEventLogFileName"] != null))
			{
				str = Path.Combine(ApplicationPath.CommonApplicationDataPath(),
						  ConfigurationManager.AppSettings["LogFilesPath"]);
				if (!Directory.Exists(str))
					Directory.CreateDirectory(str);

				str = Path.Combine(str, ConfigurationManager.AppSettings
			  ["ApplicationEventLogFileName"]);
				str = string.Concat(str, c_LogFileExtension);
			}
			return str;
		}
		/// <summary>
		/// Get application log file name.
		/// </summary>
		/// <returns>Application log file name with file extension.</returns>
		public static string ApplicationEventLogFileName()// bug 1152
		{
			return ConfigurationManager.AppSettings["ApplicationEventLogFileName"].ToString();
		}
		/// <summary>
		/// Gets free disk space for app data folder's drive.
		/// </summary>
		/// <param name="driveLetter">like C: (format needed as stated here)</param>
		/// <returns>Disk free space in MB</returns>
		public static long GetFreeDiskSpace(string driveLetter)
		{
			ManagementObject disk = new ManagementObject(
				string.Concat("win32_logicaldisk.deviceid='",
				driveLetter, "'"));
			disk.Get();
			//free space in MB
			return long.Parse(disk["FreeSpace"].ToString()) / 1000000;
		}

#endregion
	}
}
