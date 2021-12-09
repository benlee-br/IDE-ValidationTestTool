using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Xml;
using BioRad.Common.ApplicationExceptions;
using BioRad.Common.DiagnosticsLogger;
using BioRad.Common.Utilities;
using System.Globalization;

namespace BioRad.Common
{
	/// <summary>Defines the application mode based on Conventional and 
	/// Real Time.</summary>
	public enum ApplicationMode : int
	{
		/// <summary>Conventional Mode with No attached instruments.</summary>
		ConventionalNoInstruments,
		/// <summary>Conventional Mode with attached instruments.</summary>
		ConventionalWithInstruments,
		/// <summary>Real Time Mode with No attached instruments.</summary>
		RealTimeNoInstruments,
		/// <summary>Real Time Mode with one attached instrument.</summary>
		RealTimeOneInstrument,
		/// <summary>Real Time Mode with four attached instruments.</summary>
		RealTimeFourInstruments,
		/// <summary>Mix of Real Time and Conventional Mode (instruments).</summary>
		RealTimeConventionalMix
	}

	#region Documentation Tags
	/// <summary>
	/// Application state data.
	/// </summary>
	/// <remarks>
	/// In some situations, a certain type of data needs to be available to all other objects in the application. 
	/// Singleton ensures a class only has one instance, and provides a global point of access to it. 
	/// </remarks>
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
	///			<item name="vssfile">$Workfile: ApplicationStateData.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Common/ApplicationStateData.cs $</item>
	///			<item name="vssrevision">$Revision: 133 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Vnguyen $</item>
	///			<item name="vssdate">$Date: 2/08/11 11:08a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class ApplicationStateData
	{
		#region Constants
		/// <summary>
		/// Nunit process name prefix.
		/// </summary>
		private const string c_NUnitProcessNamePrefix = "NUnit";
		/// <summary>
		/// OPDCheck application name.
		/// </summary>
		private const string c_OPDCheckAppName = "OPDCheck";
		/// <summary>
		/// AnalysisApp name.
		/// </summary>
		private const string c_AnalysisAppName = "AnalysisApp";
		/// <summary>The Application Settings config file name.</summary>
		private const string c_ApplicationSettingsConfigFileName
			= @"Config\applicationSettings.xml";
		/// <summary>string for the settings node</summary>
		private const string c_Setting = "setting";
		// Bug 11530
		/// <summary>Guids used to represent True and False value. Used in situations where we don't want the user
		/// to be able to change the value but we need an easy way to do it.</summary>
		private const string c_TrueGuid = "609f8851-6735-47c9-9a17-0004f9f04325";
		private const string c_FalseGuid = "9dce71f8-ed18-4bea-a49a-510e98ab358a";

		/// <summary>
		/// Application settings enum.
		/// NOTE: enum name should be same as tag name in config\applicationsettings.XML file.
		/// </summary>
		public enum Setting
		{
			/// <summary></summary>
			applicationModeIsRegulatory,
			/// <summary></summary>
			DeploymentTag,
			/// <summary>Tells if we are running in service  mode</summary>
			IsServiceMode,
			/// <summary>Tells if 384 instrument settings displayed in application.</summary>
			IsShow384InstrumentSettings,
			/// <summary></summary>
			IsMiniOSupported,
			/// <summary>
			/// Is 2 color CFX96 Supported ?
			/// </summary>
			IsCFX2Supported,
			/// <summary>
			/// Is locust instruments supported?
			/// </summary>
			IsLocustSupported,
            /// <summary>
            /// CFX 3G
            /// </summary>
            IsCFX3GSupported,
            /// <summary>
            /// Is real time satellite instrument supported?
            /// </summary>
            IsRealTimeSatelliteSupported,//task 1078
			/// <summary>Does the application support Real-Time instruments?</summary>
			CanDoRealTime,
			/// <summary></summary>
			platesetupfilefilters,
			/// <summary></summary>
			protocolfilefilters,
			/// <summary></summary>
			runsetfilefilters,
			/// <summary></summary>
			calibrationfilefilters,
			/// <summary></summary>
			datasetfilefilters,
			/// <summary></summary>
			HighResMeltFileFilters,
			/// <summary></summary>
			HighResMeltStudyFileFilters,
			/// <summary>zpcr filters (stand-alone zipped RIP)</summary>
			RunFileFilters,
			/// <summary></summary>
			opd_datasetfilefilters,
			/// <summary></summary>
			tad_datasetfilefilters,
			/// <summary>Defines all data files excluding gene study</summary>
			Alldatasetfilefilters,
			/// <summary>
			/// Defines all data files incl gene study
			/// </summary>
			AllDataAnalysisfilefilters,
			/// <summary>File Filter for log files</summary>
			AllLogsFilesFilter,
			/// <summary>File Filter for all PMA files</summary>
			AllPMAFileFilters,
			/// <summary></summary>
			allgxdfilefilters,
			/// <summary></summary>
			odm_datasetfilefilters,
			/// <summary></summary>
			gxdfilefilters,
			/// <summary></summary>
			genestudyfilefilters,
            /// <summary></summary>
            rdmlfilefilters,
            /// <summary>For LIMS csv file import *.prln</summary>
            LIMSFileFiler,
			/// <summary>LIMS file extension.</summary>
			LIMSFileExtension,
			/// <summary>For FSD LIMS csv file import and export (.csv files).</summary>
			FSDLIMSFileFilter,
			/// <summary>FSD LIMS file extension (.csv)</summary>
			FSDLIMSFileExtension,
			/// <summary></summary>
			style,
			// fix for Bug 10613 - setting not used anymore
			///// <summary></summary>
			//AcquisitionApplicationFileName,
			/// <summary></summary>
			ClientApplicationFileName,
			/// <summary></summary>
			DAServerEventLogFileName,
			/// <summary></summary>
			MaxLogSize,
			/// <summary></summary>
			SupportFilesPath,
			/// <summary></summary>
			XSLTFilesFolder,
			/// <summary></summary>
			ImagesDirectory,
			/// <summary></summary>
			MovingRectMaskSize,
			/// <summary></summary>
			MaskXMLFileName,
			/// <summary></summary>
			CameraIppXMLConfigFile,
			/// <summary></summary>
			SupportsPureDyeCalibration,
			/// <summary></summary>
			SupportsMyiQLegacyFiles,
			/// <summary></summary>
			SupportsiQLegacyFiles,
			/// <summary></summary>
			SecurityNWUserManagement,
			/// <summary></summary>
			SecurityRoleManagement,
			/// <summary></summary>
			PersistableSettingFilename,
            /// <summary>Link to user documentation in registry for Standard Edition</summary>
            UserDocsLinkSTD,
            /// <summary>Link to user documentation in registry for IDE Edition</summary>
            UserDocsLinkIDE,
            /// <summary>Link to user documentation in registry for Mac Edition</summary>
            UserDocsLinkMAC,
            /// <summary></summary>
            UserGuideFileSTD,
            /// <summary></summary>
            UserGuideFileIDE,
            /// <summary></summary>
            UserGuideFileMAC,
            /// <summary></summary>
            UserGuideFileSE,
			/// <summary>
            /// Help file CHM name.
			/// </summary>
			OnlineHelpFile,
            /// <summary>
            /// Help file CHM name for Security Edition
            /// </summary>
            OnlineHelpFileSE,
			/// <summary>Template Files folder</summary>
			TemplateFilesPath,
			/// <summary>Application data folder for current app.</summary>
			ApplicationDataPath,
			/// <summary>Application data folder for current app (running in SE mode)</summary>
			ApplicationDataPathSE,
			/// <summary>Enumerated connected instruments folder name.</summary>
			EnumeratedConnectedInstrumentsFolderName,
			/// <summary>Simulated connected instruments folder name.</summary>
			SimulatedConnectedInstrumentsFolderName,
			/// <summary>Name of executable used for instrument discovery.</summary>
			InstrumentDiscoveryServerName,
			/// <summary>Name of executable used for Flagship server.</summary>
			FlagshipServerName,
			/// <summary>Name of executable used for locust server.</summary>
			LocustServerName,
			/// <summary>MiniOpticon connected instruments XML file name.
			/// This file will contain all MiniOpticon connected instruments.</summary>
			MiniOpticonConnectedInstrumentsFilename,
			/// <summary>Flagship connected instruments XML file name</summary>
			FlagshipConnectedInstrumentsFilename,
			/// <summary>Locust connected instruments XML file name</summary>
			LocustConnectedInstrumentsFilename,
            /// <summary>
            /// 3G connected instruments XML file name
            /// </summary>
            CFX3GConnectedInstrumentsFilename,
            /// <summary>Specifies product names for displays and logging. </summary>
            ProductName,
			/// <summary>Product name for current app (running in SE mode)</summary>
			ProductNameSE,
            /// <summary>Product name Major x.x This is separate from EXE build number x.x.x.x Maestro can have Major 1.0 and EXE build number 4.x.x.x</summary>
            ProductNameMajor,
            /// <summary>Alternate R EXE location. CFX looks in CFX EXE folder when string is left empty</summary>
            NewRLocation,
			/// <summary>Diagnostic logger executable file name.</summary>
			DiagnosticLoggerExecutableFileName,
			/// <summary>Whether to do the plate read diagnostic test for max CV.</summary>
			DoPlateReadDiagnosticTestMaxCV,
			/// <summary>Whether to do the plate read diagnostic test for max ratio of max : avg.</summary>
			DoPlateReadDiagnosticTestMaxRatio,
			/// <summary>Whether to do the plate read diagnostic test for min ratio of min : avg.</summary>
			DoPlateReadDiagnosticTestMinRatio,
			/// <summary>Whether to do the plate read diagnostic test for min average RFU value.</summary>
			DoPlateReadDiagnosticTestMinAvg,
			/// <summary>threshold for max CV test.</summary>
			PlateReadDiagnosticTestMaxCV,
			/// <summary>threshold for max ratio test.</summary>
			PlateReadDiagnosticTestMaxRatio,
			/// <summary>threshold for min ratio test.</summary>
			PlateReadDiagnosticTestMinRatio,
			/// <summary>threshold for min avg test.</summary>
			PlateReadDiagnosticTestMinAvg,
			/// <summary>max CV for reference spots across all platereads.</summary>
			ReferenceSpotDiagnosticTestMaxCV,
			/// <summary>Number of significant digits to show in quant amp chart.</summary>
			QuantitationChartLogSpaceSignificantDigits,
			// Task 473 : Read calibration limits for QC from external file
			// Fix for Bug 7266 - 3. Remove the rule for checking signals from non-primary channel.
			/// <summary>Validate the fluorescence for the primary channel only for CFX calibration 
			/// data.</summary>
			CFXValidateFluorescenceForPrimaryChannelOnly,
			// Fix for Bug 7231 - new QC values for MO
			// Fix for Bug 8697 - add QC values for 384 wells
			/// <summary>The minimum fluorescence for loaded wells for CFX 96 wells instruments.</summary>
			CFX96MinFluorescenceForLoadedWells,
			/// <summary>The maximum fluorescence for loaded wells for CFX 96 wells instruments.</summary>
			CFX96MaxFluorescenceForLoadedWells,
			/// <summary>The minimum fluorescence for empty wells for CFX 96 wells instruments.</summary>
			CFX96MinFluorescenceForEmptyWells,
			/// <summary>The maximum fluorescence for empty wells for CFX 96 wells instruments.</summary>
			CFX96MaxFluorescenceForEmptyWells,
			/// <summary>The minimum ambient temperature for CFX 96 wells instruments.</summary>
			CFX96MinAmbientTemperature,
			/// <summary>The maximum ambient temperature for CFX 96 wells instruments.</summary>
			CFX96MaxAmbientTemperature,
			/// <summary>The minimum shuttle temperature for CFX 96 wells instruments.</summary>
			CFX96MinShuttleTemperature,
			/// <summary>The maximum shuttle temperature for CFX 96 wells instruments.</summary>
			CFX96MaxShuttleTemperature,
			// Fix for Bug 8307
			// Fix for Bug 8697 - add QC values for 384 wells
			/// <summary>Signal for wells containing dye at least 500 greater than signal 
			/// in empty well in that fluors primary channel only.</summary>
			CFX96LoadedWellsPrimaryChannelFluorescenceGreaterThanEmptyWells,
			/// <summary>The minimum fluorescence for loaded wells for CFX 384 wells instruments.</summary>
			CFX384MinFluorescenceForLoadedWells,
			/// <summary>The maximum fluorescence for loaded wells for CFX 384 wells instruments.</summary>
			CFX384MaxFluorescenceForLoadedWells,
			/// <summary>The minimum fluorescence for empty wells for CFX 384 wells instruments.</summary>
			CFX384MinFluorescenceForEmptyWells,
			/// <summary>The maximum fluorescence for empty wells for CFX 384 wells instruments.</summary>
			CFX384MaxFluorescenceForEmptyWells,
			/// <summary>The minimum ambient temperature for CFX 384 wells instruments.</summary>
			CFX384MinAmbientTemperature,
			/// <summary>The maximum ambient temperature for CFX 384 wells instruments.</summary>
			CFX384MaxAmbientTemperature,
			/// <summary>The minimum shuttle temperature for CFX 384 wells instruments.</summary>
			CFX384MinShuttleTemperature,
			/// <summary>The maximum shuttle temperature for CFX 384 wells instruments.</summary>
			CFX384MaxShuttleTemperature,
			// Fix for Bug 8307
			// Fix for Bug 8697 - add QC values for 384 wells
			/// <summary>Signal for wells containing dye at least 500 greater than signal 
			/// in empty well in that fluors primary channel only.</summary>
			CFX384LoadedWellsPrimaryChannelFluorescenceGreaterThanEmptyWells,
			// Fix for Bug 7266 - 3. Remove the rule for checking signals from non-primary channel.
			/// <summary>Validate the fluorescence for the primary channel only for MO calibration 
			/// data.</summary>
			MOValidateFluorescenceForPrimaryChannelOnly,
			/// <summary>The minimum fluorescence for loaded wells.</summary>
			MOMinFluorescenceForLoadedWells,
			/// <summary>The maximum fluorescence for loaded wells.</summary>
			MOMaxFluorescenceForLoadedWells,
			/// <summary>The minimum fluorescence for empty wells.</summary>
			MOMinFluorescenceForEmptyWells,
			/// <summary>The maximum fluorescence for empty wells.</summary>
			MOMaxFluorescenceForEmptyWells,
			/// <summary>The minimum ambient temperature.</summary>
			MOMinAmbientTemperature,
			/// <summary>The maximum ambient temperature.</summary>
			MOMaxAmbientTemperature,
			/// <summary>The minimum shuttle temperature.</summary>
			MOMinShuttleTemperature,
			/// <summary>The maximum shuttle temperature.</summary>
			MOMaxShuttleTemperature,
			// Fix for Bug 8307
			// Fix for Bug 8697 - add QC values for 384 wells
			/// <summary>Signal for wells containing dye at least 500 greater than signal 
			/// in empty well in that fluors primary channel only.</summary>
			MOLoadedWellsPrimaryChannelFluorescenceGreaterThanEmptyWells,
			/// <summary>Does the fluorescence from the primary channel for the fluor have to 
			/// be greater than the fluorescence from other channels?</summary>
			PrimaryChannelBrightnessGreaterThanOtherChannels,
			/// <summary>Does the fluorescence from the loaded wells have to be greater 
			/// than the fluorescence from empty wells?</summary>
			LoadedWellsFluorescenceGreaterThanEmptyWells,
			/// <summary>Allow an DCAL file with invalid values to be created in 
			/// the simulation mode.</summary>
			AllowInvalidDCALFileInSimulationMode,
			/// <summary>Allow an DCAL file with invalid values to be created in 
			/// the non - simulation mode.</summary>
			AllowInvalidDCALFileInNonSimulationMode,
            /// <summary>
            /// Set to true for Denali to do plate reads during protocol runs.
            /// Set to false for Glendale to do plate reads during protocol runs.
            /// </summary>
            FireAndForgetProtocolRuns,
            /// <summary>
            /// Set to true to generate run reports. Set to false to turn off run reports.
            /// </summary>
            RunReports,
            /// <summary>Thermal cycler run reports folder name.</summary>
            ThermalCyclerRunReportFolderName,
            /// <summary>Thermal cycler run reports number files limit before warning user.</summary>
            ThermalCyclerRunReportFileLimit,
            /// <summary>
            /// Set the comm API logging level. 
            /// 0 = No logging, 1 = Log errors when generated,
            /// 2 = Log errors when generated and received, 3 = Log information messages 
            /// </summary>
            CommApiLoggingLevel,

			/// <summary>Frontend Firmware Version</summary>
            C1000FrontendFirmwareVersion,
			/// <summary>Backend Firmware Version</summary>
			C1000BackendFirmwareVersion,
			/// <summary>Fx2 Firmware Version</summary>
			C1000Fx2FirmwareVersion,
			/// <summary>Dsp Firmware Version</summary>
			C1000DspFirmwareVersion,
			/// <summary>Motorized Lid Firmware Version</summary>
			C1000MotorizedLidFirmwareVersion,

			/// <summary>C1000 Touch thermal cycler Frontend Firmware Version</summary>
			C1000TouchFrontendFirmwareVersion,
			/// <summary>C1000 Touch thermal cyclerBackend Firmware Version</summary>
			C1000TouchBackendFirmwareVersion,
			/// <summary>C1000 Touch thermal cyclerFx2 Firmware Version</summary>
			C1000TouchFx2FirmwareVersion,
			/// <summary>C1000 Touch thermal cyclerDsp Firmware Version</summary>
			C1000TouchDspFirmwareVersion,
			/// <summary>C1000 Touch thermal cyclerMotorized Lid Firmware Version</summary>
			C1000TouchMotorizedLidFirmwareVersion,
            /// <summary>CFX Opus Software and Firmware Version</summary>
            CfxOpusSoftwareVersion,

			/// <summary>CFX Duet cyclerBackend Firmware Version</summary>
			CFX3GDuetBackendFirmwareVersion,
			/// <summary>CFX Duet cyclerDsp Firmware Version :scanner</summary>
			CFX3GDuetDspFirmwareVersion,

			/// <summary>Next thermal cycler Frontend Firmware Version</summary>
			NextFrontendFirmwareVersion,
            /// <summary>Next thermal cyclerBackend Firmware Version</summary>
            NextBackendFirmwareVersion,
            /// <summary>Next thermal cyclerFx2 Firmware Version</summary>
            NextFx2FirmwareVersion,
            /// <summary>Next thermal cyclerDsp Firmware Version</summary>
            NextDspFirmwareVersion,
            /// <summary>Next thermal cyclerMotorized Lid Firmware Version</summary>
            NextMotorizedLidFirmwareVersion,

            /// <summary>CFX2 Thermal Cycler Frontend Firmware Version</summary>
            CFX2CyclerFrontendFirmwareVersion,
			/// <summary>CFX2 Thermal Cycler Backend Firmware Version</summary>
			CFX2CyclerBackendFirmwareVersion,
			/// <summary>CFX2 Thermal Cycler Fx2 Firmware Version</summary>
			CFX2CyclerFx2FirmwareVersion,
			/// <summary>CFX2 Thermal Cycler Dsp Firmware Version</summary>
			CFX2CyclerDspFirmwareVersion,
			/// <summary>CFX2 Thermal Cycler Motorized Lid Firmware Version</summary>
			CFX2CyclerMotorizedLidFirmwareVersion,

			/// <summary>
			/// MiniOpticon minimum Cycler Version
			/// </summary>
			MiniOpticonCyclerVersion,
			/// <summary>
			/// MiniOpticon minimum DSP Version
			/// </summary>
			MiniOpticonDSPVersion,
			/// <summary>In the MO factory mode calibration, copy the FAM calibration to
			/// Sybr also?</summary>
			MOFactoryModeCopyFAMToSYBR,
			// Kona SRS269 
			// For MiniOpticon only, when performing FAM calibration, prompt user to copy FAM 
			// calibration to SYBR. Yes will create SYBR calibration and upload((overwrite 
			// factory/user calibration)  to shuttle. No would not create SYBR calibration.
			/// <summary>In the MO user mode calibration, prompt the user to copy the FAM calibration to
			/// Sybr also?</summary>
			MOUserModePromptUserCopyFAMToSYBR,
			/// <summary>The Plate types that cannot be calibrated in the MO factory mode.</summary>
			/// <remarks>A ; separated string with the plate names.</remarks>
			MOFactoryModePlateTypeNotAllowed,
			/// <summary>The Fluorohors that cannot be calibrated in the MO factory mode.</summary>
			/// <remarks>A ; separated string with the fluorophor names.</remarks>
			MOFactoryModeFluorsNotAllowed,
			/// <summary>Allow user to define new fluors and plates in the MO factory calibration
			/// mode.</summary>
			MOFactoryModeAllowNewFluorPlateCalibration,
			/// <summary>Allow user to define new fluors and plates in the CFX factory calibration
			/// mode.</summary>
			CFXFactoryModeAllowNewFluorPlateCalibration,
			/// <summary>
			/// Falg to use protocol step properties in Melt peak calculations
			/// R-D would decide the default..
			/// </summary>
			IsUseProtocolForMelt,
			/// <summary>Specifies product names for displays and logging in the system test mode. </summary>
			ProductNameSystemTest,
            /// <summary>Remote Monitor folder name</summary>
            RemoteMonitorFolderName,
            /// <summary>Remote Monitor file name</summary>
            RemoteMonitorFileName,
            /// <summary>File contains paths of remoted monitoring instruments</summary>
            RemoteConnectedInstrumentsFileName,
            /// <summary>All file extension to open when double-click or drop to app</summary>
            AllFilesToOpenFilter,
			/// <summary>Filters for all files that can be used to repeat a run.
			/// Used by the recent file manager.</summary>
			AllRecentRunsFileFilters,
			/// <summary>Filters for all files that can be analyzed. Used by the recent files manager.</summary>
			AllRecentAnalyzeFileFilters,
			/// <summary>Filters for all report template files. Used by the recent files manager.</summary>
			AllRecentReportTemplateFileFilters,
			// Fix for Bug 9096
			/// <summary>Plate types that are not allowed for CFX384 factory calibration</summary>
			CFX384FactoryModePlateTypeNotAllowed,
			/// <summary>Plate types that are default for CFX384 factory calibration</summary>
			CFX384FactoryDefaultPlateType,
            /// <summary>Plate types that are default for CFX48 factory calibration</summary>
            MOFactoryDefaultPlateType,
			/// <summary>
			/// Flag to set when ready for release build.
			/// </summary>
			IsReadyForReleaseBuild,
			/// <summary>The name of the Instrument Scheduler application.</summary>
			InstrumentSchedulerApplicationFileName,
            /// <summary>CommadnLine: Time out in second wait for instrument ready to run</summary>
            SecondsWaitForInstrumentReady,
            /// <summary>CommadnLine: Time out in second wait for report to complete</summary>
            SecondsWaitForReport,
            /// <summary>CommadnLine: Time out in second wait for AnalysisExport to complete</summary>
            SecondsWaitForAnalysisExport,
            /// <summary>CommadnLine: Time in second to periodically get instrument info with -i flag during a run</summary>
            SecondsPeriodicallyGetInstrumentInfo,
            /// <summary>CommadnLine: Time in second to ignore all machine object status after starting timer on non-cancellable errors</summary>
            SecondsWaitAfterStartTimerToIgnoreMachineStatus,
            /// <summary>CommadnLine: Time out in second waiting for recovery on non-cancellable errors</summary>
            SecondsWaitForRecoveryOnNonCancellableError,
            /// <summary> FSD APF directory setting </summary>
            APFDirectory,
            /// <summary> FSD APF directory setting </summary>
            applicationModeIsFSD,
            /// <summary> FSD Sample List File name setting </summary>
            FSDSampleListFile,
            /// <summary> FSD Kit Lot Info File name setting </summary>
            FSDKitLotInfoFile,
            /// <summary> FSD Robot extraction matrix file name setting </summary>
            FSDExtractionMatrixFile,
            /// <summary> FSD Robot extraction Certifications setting </summary>
            FSDExtractionCertifications,
            /// <summary>Defect 10930 (CFX 2.0 SRS162) Export\Import data files in RDML format.</summary>
            RDMLExportImport,
            /// <summary>RDML schema file 1.0 name.</summary>
            RDMLSchemaFile,
            /// <summary>RDML schema file 1.1 name.</summary>
            RDMLSchemaFileV11,
            /// <summary>Defect 10739 Software\Firmware updates via CFX</summary>
            CheckForSoftwareFirmwareUpdates,
            /// <summary>
            /// Defect 12674
            /// Setting for bypassing the HASP dongle check after CFX release build
            /// </summary>
            PerformCheckForDongle,
			/// <summary>The folder name for the Content run set files.</summary>
			ContentFolderName,
			/// <summary>The file extension for the Content run set file.</summary>
			ContentZipFileExtension,
			/// <summary>The file extension for the Content csv file.</summary>
			ContentCSVFileExtension,
			/// <summary>All the file extensions (filters) for the Content files.</summary>
			AllContentFilesFilter,
			/// <summary>Config file used to check/validate the content .csv files. Service mode only.</summary>
			ContentCheckConfigFile,
			/// <summary>List of folders on instrument not to show.</summary>
			FileExplorerSkipInstrumentFolders,
            /// <summary> Running on Mac OS? </summary>
            applicationModeIsMac,
        };
		#endregion

		#region Member Data
		/// <summary>Object used to provide singleton access to this object.</summary>
		private static ApplicationStateData m_ApplicationStateData = null;
		/// <summary>
		/// Current data analysis file name being analyzed.
		/// </summary>
		private string m_CurrentDataAnalysisFileBeingAnalyzed;
		/// <summary>
		/// Current data acquisition file name being acquired.
		/// </summary>
		private string m_CurrentDataAcquisitionFileBeingAcquired;
		/// <summary>
		/// Settings collection from "Config\applicationSettings.xml" file.
		/// </summary>
		private Hashtable m_Settings = null;
        private bool m_isSimulation = false;
		private bool m_isConsoleMode = false;
        private CultureInfo m_SelectedCulture = CultureInfo.CurrentCulture;
		#endregion

		#region Accessors
        /// <summary>
        /// changed by Locale setting
        /// </summary>
        public CultureInfo AppCurrentUICulture
        {
            get { return m_SelectedCulture;}
            set { m_SelectedCulture = value; }

        }
        /// <summary>Get thermal cycler run reports folder name.</summary>
        public string ThermalCyclerRunReportFolderName
        {
            get
            {
                if (m_Settings != null &&
                    m_Settings.ContainsKey("ThermalCyclerRunReportFolderName"))
                {
                    return m_Settings["ThermalCyclerRunReportFolderName"] as string;
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// Get thermal cycler run reports number files 
        /// limit before warning user to archive.
        /// </summary>
        public int ThermalCyclerRunReportFileLimit
        {
            get
            {
                int limit = 1000;
                if (m_Settings != null &&
                    m_Settings.ContainsKey("ThermalCyclerRunReportFileLimit"))
                {
                    limit = int.Parse(m_Settings["ThermalCyclerRunReportFileLimit"] as string);
                }
                return limit;
            }
        }
		/// <summary>Can the application handle Real-Time instruments.</summary>
		public bool CanDoRealTime
		{
			get
			{
				if (m_Settings != null &&
						  m_Settings.ContainsKey("CanDoRealTime"))
				{
					return bool.Parse(
						 m_Settings["CanDoRealTime"].ToString());
				}
				return false;
			}
		}
        /// <summary>Defect 10739 Software\Firmware upgrades via CFX</summary>
        public bool CheckForSoftwareFirmwareUpdates
        {
            get
            {
                try
                {
                    if (m_Settings != null && m_Settings.ContainsKey("CheckForSoftwareFirmwareUpdates"))
                    {
                        return bool.Parse(m_Settings["CheckForSoftwareFirmwareUpdates"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                    string message = ex.Message;
                }
                return false;
            }
        }
		/// <summary>Defect 10930 (CFX 2.0 SRS163) Real time data export.</summary>
		public bool RealTimeDataExport
        {
            get
            {
                try
                {
					if (File.Exists(Path.Combine(ApplicationPath.InstrumentDirectory, "EnableRealTimeDataExport")))
                    {
						return true;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                    string message = ex.Message;
                }
                return false;
            }
        }
		/// <summary>Defect 10930 (CFX 2.0 SRS163) Real time data export.</summary>
		public bool RealTimeDataExportViewer
		{
			get
			{
				try
				{
					if (File.Exists(Path.Combine(ApplicationPath.InstrumentDirectory, "EnableRealTimeDataExportViewer")))
					{
						return true;
					}
				}
				catch (Exception ex)
				{
					Debug.Assert(false, ex.Message);
					string message = ex.Message;
				}
				return false;
			}
		}
        /// <summary>Defect 10930 (CFX 2.0 SRS162) Export data file in RDML format.</summary>
        public bool RDMLExportImport
        {
            get
            {
                try
                {
                    if (m_Settings != null && m_Settings.ContainsKey("RDMLExportImport"))
                    {
                        return bool.Parse(m_Settings["RDMLExportImport"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                    string message = ex.Message;
                }
                return false;
            }
        }
        /// <summary>
        /// Get RDML schema file name. File should exists in same folder as client executable.
        /// </summary>
        public string RDMLSchemaFileName
        {
            get
            {
                try
                {
                    if (m_Settings != null && m_Settings.ContainsKey("RDMLSchemaFile"))
                    {
                        return m_Settings["RDMLSchemaFile"] as string;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                    string message = ex.Message;
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// Get RDML schema file name. File should exists in same folder as client executable.
        /// </summary>
        public string RDMLSchemaFileNameV11
        {
            get
            {
                try
                {
                    if (m_Settings != null && m_Settings.ContainsKey("RDMLSchemaFileV11"))
                    {
                        return m_Settings["RDMLSchemaFileV11"] as string;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                    string message = ex.Message;
                }
                return string.Empty;
            }
        }
		/// <summary>Is ready for release/release build.</summary>
		public bool IsReadyForReleaseBuild
		{
			get
			{
				if (m_Settings != null &&
						  m_Settings.ContainsKey("IsReadyForReleaseBuild"))
				{
					return bool.Parse(
						 m_Settings["IsReadyForReleaseBuild"].ToString());
				}
				return false;
			}
		}
        /// <summary>Option to perform dongle checking.</summary>
        public bool PerformCheckForDongle
        {
            get
            {
                if (m_Settings != null &&
                          m_Settings.ContainsKey("PerformCheckForDongle"))
                {
                    return bool.Parse(m_Settings["PerformCheckForDongle"].ToString());
                }
                return true;
            }
            set
            {
                m_Settings["PerformCheckForDongle"] = value.ToString();
            }
        }
		/// <summary>
		/// Get reference to this object.
		/// </summary>
		public static ApplicationStateData GetInstance
		{
			get
			{
				if (m_ApplicationStateData == null)
					m_ApplicationStateData = new ApplicationStateData();
				return m_ApplicationStateData;
			}
		}
		/// <summary>
		/// Returns full path of file currently being acquired or null if no file is being acquired.
		/// </summary>
		/// <remarks>
		/// ManageAcquisitionRun.cs maintains the current file name being acquired. 
		/// Sets the file name when a run has started successufully and
		/// sets the file name to null when the run completes. OpticalRunPrepare.cs will access
		/// this file name to determine if the user has selected a file that is being currently acquired.
		/// </remarks>
		public string CurrentDataAcquisitionFileBeingAcquired
		{
			get
			{
				lock (this)
				{
					return this.m_CurrentDataAcquisitionFileBeingAcquired;
				}
			}

			set
			{
				lock (this)
				{
					this.m_CurrentDataAcquisitionFileBeingAcquired = value;
				}
			}
		}
		/// <summary>
		/// Returns full path of file currently be analyzed or null if no file is being analyzed.
		/// </summary>
		public string CurrentDataAnalysisFileBeingAnalyzed
		{
			get
			{
				lock (this)
				{
					return this.m_CurrentDataAnalysisFileBeingAnalyzed;
				}
			}

			set
			{
				lock (this)
				{
					this.m_CurrentDataAnalysisFileBeingAnalyzed = value;
				}
			}
		}
        /// <summary>
        /// 
        /// </summary>
        public string NewRLocation
        {
            get
            {
                if (m_Settings != null &&
                         m_Settings.ContainsKey("NewRLocation"))
                {
                    return m_Settings["NewRLocation"].ToString();
                }
                return String.Empty;
            }         
        }
        /// <summary>
        /// Return Major string N.M (for instance 1.0 from CFX Maestro or 3.0 from CFX IDE). Use MajorWithEXEBuildNumber to get this string with EXE build number
        /// </summary>
        public string ProductNameMajor
        {
            get
            {
                if (m_Settings != null &&
                         m_Settings.ContainsKey("ProductNameMajor"))
                {
                    return m_Settings["ProductNameMajor"].ToString();
                }
                return String.Empty;
            }
            
        }

        /// <summary>
        /// EXE build number such as 4.x.x.x. 
        /// </summary>
        public string EXEBuildNumber
        {
            get
            {
                string applicationVersionInfo = ApplicationPath.ApplicationVersionInfo.FileVersion;
                return applicationVersionInfo;
            }
        }

        /// <summary>
        /// A displayable string with Major + EXE build number: for example "1.0 (4.x.x.x)"
        /// </summary>
        public string MajorWithEXEBuildNumber
        {
            get
            {
                string edition = "STD"; //Do not localize
                if (IsMacApplication)
                {
                    edition = "MAC"; //Do not localize
                }
                else if (IsRegulatory)
                {
                    edition = "CFR"; //Do not localize
                }
                else if (IsFSDApplication)
                {
                    edition = "IDE"; //Do not localize
                }
                string customizableString = String.Concat(edition," ",ProductNameMajor, " (", EXEBuildNumber, ")");
                return customizableString;
            }
        }
        /// <summary>
        /// Returns a displayable string with the application name
        /// </summary>
        public string ProductName
		{
			get
			{
				if (IsRegulatory)
				{
					if (m_Settings != null &&
						 m_Settings.ContainsKey("ProductNameSE"))
					{
						return m_Settings["ProductNameSE"].ToString();
					}
				}
				else
				{
					if (m_Settings != null &&
						 m_Settings.ContainsKey("ProductName"))
					{
						return m_Settings["ProductName"].ToString();
					}
				}
				//Fall back
				return Process.GetCurrentProcess().MainModule.FileVersionInfo.ProductName;
			}
			set
			{
				if (IsRegulatory)
					m_Settings["ProductNameSE"] = value;
				else
					m_Settings["ProductName"] = value;
			}
		}		

		/// <summary>
		/// Returns a displayable string with the application name (system test).
		/// </summary>
		public string ProductNameSystemTest
		{
			get
			{

				if (m_Settings != null &&
					 m_Settings.ContainsKey("ProductNameSystemTest"))
				{
					return m_Settings["ProductNameSystemTest"].ToString();
				}
				//Fall back
				return Process.GetCurrentProcess().MainModule.FileVersionInfo.ProductName;
			}
		}
		/// <summary>Returns true if the application is running in the regulatory mode.</summary>
		/// <remarks>The application is running in a mode where features required for 
		/// supporting 21 CFR Part 11 are available.</remarks>
		public bool IsRegulatory
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("applicationModeIsRegulatory"))
				{
					return bool.Parse(
						m_Settings["applicationModeIsRegulatory"].ToString());
				}
				return false;
			}
			set
			{
                if (m_Settings != null)
                    m_Settings["applicationModeIsRegulatory"] = value;
			}
		}
        /// <summary>
        /// Returns true if the application is running on the MAC OS.
        /// </summary>
        public bool IsMacApplication
        {
            get
            {
                if (m_Settings != null &&
                    m_Settings.ContainsKey("applicationModeIsMac"))
                {
                    return bool.Parse(
                        m_Settings["applicationModeIsMac"].ToString());
                }
                return false;
            }
            set
            {
                if (m_Settings != null)
                    m_Settings["applicationModeIsMac"] = value;
            }
        }
        /// <summary>Returns true if the application is running in the FSD mode.</summary>
        public bool IsFSDApplication
        {
            get
            {
                if (m_Settings != null &&
                    m_Settings.ContainsKey("applicationModeIsFSD"))
                {
                    return bool.Parse(
                        m_Settings["applicationModeIsFSD"].ToString());
                }
                return false;
            }
            set
            {
                if (m_Settings != null)
                    m_Settings["applicationModeIsFSD"] = value;
            }
        }
		/// <summary>
		/// Get an application setting.
		/// </summary>
		/// <remarks>
		/// Throws ApplicationException if setting not found.
		/// </remarks>
		public string this[ApplicationStateData.Setting applicationSetting]
		{
			get
			{
				string setting = applicationSetting.ToString();
				if (m_Settings.ContainsKey(setting))
					return (string)m_Settings[setting];
				else
				{
					string s = StringUtility.FormatString(Properties.Resources.ApplicationStateDataSettingsMismatch,
				 setting);
					throw new ApplicationException(s);
				}
			}
			set
			{
				string setting = applicationSetting.ToString();
				if (m_Settings.ContainsKey(setting))
					m_Settings[setting] = value;
				else
				{
					string s = StringUtility.FormatString(Properties.Resources.ApplicationStateDataSettingsMismatch,
				 setting);
					throw new ApplicationException(s);
				}

			}
		}
		#region C1000 firmware versions
		/// <summary>Get Frontend Firmware Version (Glendale)</summary>
		public string C1000FrontendFirmwareVersion
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("C1000FrontendFirmwareVersion"))
				{
					return m_Settings["C1000FrontendFirmwareVersion"] as string;
				}
				return string.Empty;
			}
		}
		/// <summary>Get Backend Firmware Version (HC12)</summary>
		public string C1000BackendFirmwareVersion
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("C1000BackendFirmwareVersion"))
				{
					return m_Settings["C1000BackendFirmwareVersion"] as string;
				}
				return string.Empty;
			}
		}
		/// <summary>Get Fx2 Firmware Version</summary>
		public string C1000Fx2FirmwareVersion
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("C1000Fx2FirmwareVersion"))
				{
					return m_Settings["C1000Fx2FirmwareVersion"] as string;
				}
				return string.Empty;
			}
		}
		/// <summary>Get Dsp Firmware Version</summary>
		public string C1000DspFirmwareVersion
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("C1000DspFirmwareVersion"))
				{
					return m_Settings["C1000DspFirmwareVersion"] as string;
				}
				return string.Empty;
			}
		}

		/// <summary>Get Motorized Lid Firmware Version</summary>
		public string C1000MotorizedLidFirmwareVersion
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("C1000MotorizedLidFirmwareVersion"))
				{
					return m_Settings["C1000MotorizedLidFirmwareVersion"] as string;
				}
				return string.Empty;
			}
		}
		#endregion

		#region C1000 Touch firmware versions
		/// <summary>Get Frontend Firmware Version (Glendale)</summary>
		public string C1000TouchFrontendFirmwareVersion
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("C1000TouchFrontendFirmwareVersion"))
				{
					return m_Settings["C1000TouchFrontendFirmwareVersion"] as string;
				}
				return string.Empty;
			}
		}
		/// <summary>Get Backend Firmware Version (HC12)</summary>
		public string C1000TouchBackendFirmwareVersion
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("C1000TouchBackendFirmwareVersion"))
				{
					return m_Settings["C1000TouchBackendFirmwareVersion"] as string;
				}
				return string.Empty;
			}
		}
		/// <summary>Get Fx2 Firmware Version</summary>
		public string C1000TouchFx2FirmwareVersion
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("C1000TouchFx2FirmwareVersion"))
				{
					return m_Settings["C1000TouchFx2FirmwareVersion"] as string;
				}
				return string.Empty;
			}
		}
		/// <summary>Get Dsp Firmware Version</summary>
		public string C1000TouchDspFirmwareVersion
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("C1000TouchDspFirmwareVersion"))
				{
					return m_Settings["C1000TouchDspFirmwareVersion"] as string;
				}
				return string.Empty;
			}
		}

		/// <summary>Get Motorized Lid Firmware Version</summary>
		public string C1000TouchMotorizedLidFirmwareVersion
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("C1000TouchMotorizedLidFirmwareVersion"))
				{
					return m_Settings["C1000TouchMotorizedLidFirmwareVersion"] as string;
				}
				return string.Empty;
			}
		}

        /// <summary>Get CFX Software version</summary>
        public string CfxOpusSoftwareVersion
        {
            get
            {
                if (m_Settings != null &&
                    m_Settings.ContainsKey("CfxOpusSoftwareVersion"))
                {
                    return m_Settings["CfxOpusSoftwareVersion"] as string;
                }
                return string.Empty;
            }
        }
		#endregion

		#region CFX3G Duet firmware versions (Cycler & Scanner)
		/// <summary>Get Backend Firmware Version (Cycler)</summary>
		public string CFX3GDuetBackendFirmwareVersion
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("CFX3GDuetBackendFirmwareVersion"))
				{
					return m_Settings["CFX3GDuetBackendFirmwareVersion"] as string;
				}
				return string.Empty;
			}
		}
		/// <summary>Get Frontend Firmware Version (Scanner)</summary>
		public string CFX3GDuetDspFirmwareVersion
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("CFX3GDuetDspFirmwareVersion"))
				{
					return m_Settings["CFX3GDuetDspFirmwareVersion"] as string;
				}
				return string.Empty;
			}
		}
		#endregion

		#region Next firmware versions
		/// <summary>Get Frontend Firmware Version (Glendale)</summary>
		public string NextFrontendFirmwareVersion
        {
            get
            {
                if (m_Settings != null &&
                    m_Settings.ContainsKey("NextFrontendFirmwareVersion"))
                {
                    return m_Settings["NextFrontendFirmwareVersion"] as string;
                }
                return string.Empty;
            }
        }
        /// <summary>Get Backend Firmware Version (HC12)</summary>
        public string NextBackendFirmwareVersion
        {
            get
            {
                if (m_Settings != null &&
                    m_Settings.ContainsKey("NextBackendFirmwareVersion"))
                {
                    return m_Settings["NextBackendFirmwareVersion"] as string;
                }
                return string.Empty;
            }
        }
        /// <summary>Get Fx2 Firmware Version</summary>
        public string NextFx2FirmwareVersion
        {
            get
            {
                if (m_Settings != null &&
                    m_Settings.ContainsKey("NextFx2FirmwareVersion"))
                {
                    return m_Settings["NextFx2FirmwareVersion"] as string;
                }
                return string.Empty;
            }
        }
        /// <summary>Get Dsp Firmware Version</summary>
        public string NextDspFirmwareVersion
        {
            get
            {
                if (m_Settings != null &&
                    m_Settings.ContainsKey("NextDspFirmwareVersion"))
                {
                    return m_Settings["NextDspFirmwareVersion"] as string;
                }
                return string.Empty;
            }
        }

        /// <summary>Get Motorized Lid Firmware Version</summary>
        public string NextMotorizedLidFirmwareVersion
        {
            get
            {
                if (m_Settings != null &&
                    m_Settings.ContainsKey("NextMotorizedLidFirmwareVersion"))
                {
                    return m_Settings["NextMotorizedLidFirmwareVersion"] as string;
                }
                return string.Empty;
            }
        }
        #endregion

        #region CFX2 Thermal Cycler firmware versions
        /// <summary>Get Frontend Firmware Version (Glendale)</summary>
        public string CFX2CyclerFrontendFirmwareVersion
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("CFX2CyclerFrontendFirmwareVersion"))
				{
					return m_Settings["CFX2CyclerFrontendFirmwareVersion"] as string;
				}
				return string.Empty;
			}
		}
		/// <summary>Get Backend Firmware Version (HC12)</summary>
		public string CFX2CyclerBackendFirmwareVersion
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("CFX2CyclerBackendFirmwareVersion"))
				{
					return m_Settings["CFX2CyclerBackendFirmwareVersion"] as string;
				}
				return string.Empty;
			}
		}
		/// <summary>Get Fx2 Firmware Version</summary>
		public string CFX2CyclerFx2FirmwareVersion
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("CFX2CyclerFx2FirmwareVersion"))
				{
					return m_Settings["CFX2CyclerFx2FirmwareVersion"] as string;
				}
				return string.Empty;
			}
		}
		/// <summary>Get Dsp Firmware Version</summary>
		public string CFX2CyclerDspFirmwareVersion
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("CFX2CyclerDspFirmwareVersion"))
				{
					return m_Settings["CFX2CyclerDspFirmwareVersion"] as string;
				}
				return string.Empty;
			}
		}

		/// <summary>Get Motorized Lid Firmware Version</summary>
		public string CFX2CyclerMotorizedLidFirmwareVersion
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("CFX2CyclerMotorizedLidFirmwareVersion"))
				{
					return m_Settings["CFX2CyclerMotorizedLidFirmwareVersion"] as string;
				}
				return string.Empty;
			}
		}
		#endregion

		#region MiniOpticon firmware versions
		/// <summary>Get MiniOpticon Cycler Version</summary>
		public string MiniOpticonCyclerVersion
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("MiniOpticonCyclerVersion"))
				{
					return m_Settings["MiniOpticonCyclerVersion"] as string;
				}
				return string.Empty;
			}
		}

		/// <summary>Get MiniOpticon DSP Version</summary>
		public string MiniOpticonDSPVersion
		{
			get
			{
				if (m_Settings != null &&
					m_Settings.ContainsKey("MiniOpticonDSPVersion"))
				{
					return m_Settings["MiniOpticonDSPVersion"] as string;
				}
				return string.Empty;
			}
		}
		#endregion

		/// <summary>Get RemoteMonitor folder name</summary>
        public string RemoteMonitorFolderName
        {
            get
            {
                if (m_Settings != null &&
                    m_Settings.ContainsKey("RemoteMonitorFolderName"))
                {
                    return m_Settings["RemoteMonitorFolderName"] as string;
                }
                return string.Empty;
            }
        }
        /// <summary>Get RemoteMonitor file name</summary>
        public string RemoteMonitorFileName
        {
            get
            {
                if (m_Settings != null &&
                    m_Settings.ContainsKey("RemoteMonitorFileName"))
                {
                    return m_Settings["RemoteMonitorFileName"] as string;
                }
                return string.Empty;
            }
        }
        /// <summary>Get RemoteConnectedInstruments file name</summary>
        public string RemoteConnectedInstrumentsFileName
        {
            get
            {
                if (m_Settings != null &&
                    m_Settings.ContainsKey("RemoteConnectedInstrumentsFileName"))
                {
                    return m_Settings["RemoteConnectedInstrumentsFileName"] as string;
                }
                return string.Empty;
            }
        }
        /// <summary>App running is simulation mode?</summary>
        public bool IsSimulation
        {
            set { m_isSimulation = value; }
            get { return m_isSimulation; }
        }
		/// <summary>App running in console mode?</summary>
		public bool IsConsoleMode
		{
			set { m_isConsoleMode = value; }
			get { return m_isConsoleMode; }
		}
		/// <summary>App running in CommandManager API mode?</summary>
		public bool IsCMAPI { set; get; }
		
        /// <summary>
        /// CommadnLine: Time out in second wait for instrument ready to run 
        /// </summary>
        public int SecondsWaitForInstrumentReady
        {
            get
            {
                int wait = 70;// default
                if (m_Settings != null &&
                    m_Settings.ContainsKey("SecondsWaitForInstrumentReady"))
                {
                    wait = int.Parse(m_Settings["SecondsWaitForInstrumentReady"] as string);
                }
                return wait;
            }
        }
        /// <summary>
        /// CommadnLine: Time out in second wait for report to complete
        /// </summary>
        public int SecondsWaitForReport
        {
            get
            {
                int wait = 600;// default
                if (m_Settings != null &&
                    m_Settings.ContainsKey("SecondsWaitForReport"))
                {
                    wait = int.Parse(m_Settings["SecondsWaitForReport"] as string);
                }
                return wait;
            }
        }
        /// <summary>
        /// CommadnLine: Time out in second wait for AnalysisExport to complete
        /// </summary>
        public int SecondsWaitForAnalysisExport
        {
            get
            {
                int wait = 600;// default
                if (m_Settings != null &&
                    m_Settings.ContainsKey("SecondsWaitForAnalysisExport"))
                {
                    wait = int.Parse(m_Settings["SecondsWaitForAnalysisExport"] as string);
                }
                return wait;
            }
        }
        /// <summary>
        /// CommadnLine: Time in second to periodically get instrument info with -i flag during a run
        /// </summary>
        public int SecondsPeriodicallyGetInstrumentInfo
        {
            get
            {
                int wait = 30;// default
                if (m_Settings != null &&
                    m_Settings.ContainsKey("SecondsPeriodicallyGetInstrumentInfo"))
                {
                    wait = int.Parse(m_Settings["SecondsPeriodicallyGetInstrumentInfo"] as string);
                }
                return wait;
            }
        }
        /// <summary>
        /// CommadnLine: Time in second to ignore all machine object status after starting timer on non-cancellable errors
        /// </summary>
        public int SecondsWaitAfterStartTimerToIgnoreMachineStatus
        {
            get
            {
                int wait = 90;  // default
                if (m_Settings != null &&
                    m_Settings.ContainsKey("SecondsWaitAfterStartTimerToIgnoreMachineStatus"))
                {
                    wait = int.Parse(m_Settings["SecondsWaitAfterStartTimerToIgnoreMachineStatus"] as string);
                }
                return wait;
            }
        }
        /// <summary>
        /// CommadnLine: Time out in second waiting for recovery on non-cancellable errors
        /// </summary>
        public int SecondsWaitForRecoveryOnNonCancellableError
        {
            get
            {
                int wait = 100;  // default
                if (m_Settings != null &&
                    m_Settings.ContainsKey("SecondsWaitForRecoveryOnNonCancellableError"))
                {
                    wait = int.Parse(m_Settings["SecondsWaitForRecoveryOnNonCancellableError"] as string);
                }
                return wait;
            }
        }
		
		// Bug 11530
		/// <summary>Determines if the application allows MeltCurve in the protocol.</summary>
		public bool IsMeltCurveAllowed
		{
			get
			{
				return GetBoolFromGuid(ConfigurationManager.AppSettings["MC"].ToString());
			}
		}		
		#endregion

		#region Constructors and Destructor
		/// <summary>Constructs the ApplicationStateData object and sets the 
		/// application configuration.</summary>
		private ApplicationStateData()
		{
			m_CurrentDataAnalysisFileBeingAnalyzed = null;
			m_CurrentDataAcquisitionFileBeingAcquired = null;

			// Get the application settings
			GetApplicationSettings();
		}
		#endregion

		#region Methods
        /// <summary>
        /// 
        /// </summary>
        public bool MiniOSupportedOverride
        {
            get
            {
                if (m_Settings != null &&
                          m_Settings.ContainsKey("MiniOSupportedOverride"))
                {
                    return bool.Parse(m_Settings["MiniOSupportedOverride"].ToString());
                }
                return false;
            }
            set
            {
                m_Settings["MiniOSupportedOverride"] = value.ToString();
            }
        }
		/// <summary>
		/// Returns true if the MiniOpticon is supported.
		/// </summary>
		/// <returns>true if it is the case</returns>
        public bool IsMiniOSupported
        {
            get
            {
                //TT898 [Regression] MiniOpticon is blocked in Security edition on Windows 7
                //if (IsRegulatory)//TT305 no mini in security edition
                //    return false;

                bool isMiniOSupported = bool.Parse(GetInstance[Setting.IsMiniOSupported]);
                if (!ComputerOSInfo.IsWindows7())// CFX US43 MiniOpticon will not be supported on Windows 8.1 or 10
                {
                    isMiniOSupported = MiniOSupportedOverride;
                }
                return isMiniOSupported;
            }
            set
            {
                if (m_Settings != null)
                {
                    m_Settings[Setting.IsMiniOSupported] = value.ToString();
                }
            }
        }
		/// <summary>
		/// Returns true if the CFX2 (2 color CFX96) is supported.
		/// </summary>
		/// <returns>true if it is the case</returns>
		public bool IsCFX2Supported()
		{
			return bool.Parse(GetInstance[Setting.IsCFX2Supported]);
		}
		/// <summary>
		/// Returns true if Locust instruments is supported.
		/// </summary>
		/// <returns>true if it is the case</returns>
		public bool IsLocustSupported()
		{
			return bool.Parse(GetInstance[Setting.IsLocustSupported]);
		}
        /// <summary>
        /// CFX 3G Instrument
        /// </summary>
        /// <returns>True if 3G instruments supported.</returns>
        public bool IsCFX3GSupported()
        {
            return bool.Parse(GetInstance[Setting.IsCFX3GSupported]);
        }
        /// <summary>
        /// 
        /// </summary>
        public bool LocustSupported
        {
            get
            {
                return IsLocustSupported();
            }
            set
            {
                if (m_Settings != null)
                    m_Settings["IsLocustSupported"] = value.ToString();
            }
        }
        /// <summary>
        /// CFX 3G Instrument
        /// </summary>
        public bool CFX3GSupported
        {
            get
            {
                return IsCFX3GSupported();
            }
            set
            {
                if (m_Settings != null)
                    m_Settings["IsCFX3GSupported"] = value.ToString();
            }
        }
        /// <summary>
        /// Returns true if real time satellite instrument is supported.
        /// </summary>
        /// <returns>true if it is the case</returns>
        public bool IsRealTimeSatelliteSupported()//task 1078
		{
			return bool.Parse(GetInstance[Setting.IsRealTimeSatelliteSupported]);
		}
		/// <summary>
		/// Returns true if the running Service Mode.
		/// </summary>
		/// <returns>true if it is the case</returns>
		public bool IsServiceMode()
		{
			return bool.Parse(GetInstance[Setting.IsServiceMode]);
		}
		/// <summary>Tells if 384 instrument settings displayed in application.</summary>
		/// <returns>true if it is the case</returns>
		public bool IsShow384InstrumentSettings()
		{
			return bool.Parse(GetInstance[Setting.IsShow384InstrumentSettings]);
		}
		
		/// <summary>
		/// Set Application to the Service Mode, should be used only when service user logs in.
		/// </summary>
		/// <returns>true if it is the case</returns>
		public void SetToServiceMode()
		{
			//Set Service mode to true.
			GetInstance[Setting.IsServiceMode] = "true";
		}

		/// <summary>
		/// Determine if a file is the file currenly being acquired.
		/// </summary>
		/// <param name="fileFullPath">Full path of file to check.</param>
		/// <returns>returns true if specified file is currently being acquired.</returns>
		public bool IsFileBeingAcquired(string fileFullPath)
		{
			lock (this)
			{
				return (string.Compare(
					m_CurrentDataAcquisitionFileBeingAcquired,
					fileFullPath,
					true) == 0);
			}
		}
		/// <summary>
		/// Determine if a file is the file currenly being analyzed.
		/// </summary>
		/// <param name="fileFullPath">Full path of file to check.</param>
		/// <returns>returns true if specified file is currently being analyzed.</returns>
		public bool IsFileBeingAnalyzed(string fileFullPath)
		{
			lock (this)
			{
				return (string.Compare(
					m_CurrentDataAnalysisFileBeingAnalyzed,
					fileFullPath,
					true) == 0);
			}
		}

		/// <summary>Reads the application settings configuration file and sets the configuration
		/// member data</summary>
		private void GetApplicationSettings()
		{
			string fileName = Path.Combine(ApplicationPath.DirectoryPath,
				c_ApplicationSettingsConfigFileName);
			XmlTextReader reader = null;
			try
			{
				//Decrypt, if encrypted.
				using (Stream stream = FileCryptor.GetInstance.DecryptFileContentsToStream(fileName))
				{
					//Store settings in hash table.
					m_Settings = new Hashtable();
					XmlParserContext parser = new XmlParserContext(null, null, string.Empty, XmlSpace.None);
					reader = new XmlTextReader(stream, XmlNodeType.Element, parser);
					while (reader.Read())
					{
						if (StringUtility.StringMatch(reader.Name, c_Setting))
						{
							while (reader.MoveToNextAttribute())
							{
								if (!m_Settings.ContainsKey(reader.Name))
								{
                                    m_Settings.Add(reader.Name, reader.Value);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
				}

				// Now validate settings with enum.
				string setting;
				ApplicationStateData.Setting[] settings =
					(ApplicationStateData.Setting[])
					Enum.GetValues(typeof(ApplicationStateData.Setting));

				// check number settings read against enum length.
				if (settings.Length != m_Settings.Count)
				{
					string s = StringUtility.FormatString(Properties.Resources.ApplicationStateDataSettingsMismatchCount);
					throw new ApplicationException(s);
				}

				foreach (ApplicationStateData.Setting e in settings)
					setting = this[e];//indexer throws exception if not valid setting.
			}
		}
		
		// Bug 11530
		/// <summary>Compares the passed in Guid with the true/false guids defined and returns a bool value.</summary>
		/// <remarks>Returns false if the passed in string does not match the defined guid strings.</remarks>
		/// <param name="guid">The guid string to check.</param>
		/// <returns>A boolean value.</returns>
		public bool GetBoolFromGuid(string guid)
		{
			if (guid == c_TrueGuid)
				return true;
			else if (guid == c_FalseGuid)
				return false;
			else
				// fallback
				return false;
		}
		/// <summary>
		/// Get list of instrument folder not to show in file explorer.
		/// </summary>
		public string FileExplorerSkipInstrumentFolders//task 1161
		{
			get
			{
				if (m_Settings != null && m_Settings.ContainsKey("FileExplorerSkipInstrumentFolders"))
				{
					return m_Settings["FileExplorerSkipInstrumentFolders"] as string;
				}
				return string.Empty;
			}
		}
		#endregion
	}
}

