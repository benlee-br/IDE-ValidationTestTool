using System;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>
	/// Well known logger names.
	/// </summary>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Ralph Ansell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">Replace this text with ID</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\DiagLogger.wmf">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: WellKnownLogName.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/WellKnownLogName.cs $</item>
	///			<item name="vssrevision">$Revision: 39 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ssarup $</item>
	///			<item name="vssdate">$Date: 12/26/07 1:29a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public partial class WellKnownLogName
	{
		#region Constants
		/// <summary>
		/// Well known logger name for thermal cycler.
		/// </summary>
		public static readonly string ThermalCycler = "TC";
		/// <summary>
		///  Well known logger name for thermal cycler status thread.
		/// </summary>
		public static readonly string ThermalCyclerThread = "TC Thread";

		/// <summary>
		/// Well known logger name for the TC Unmanaged wrapper class.
		/// </summary>
		public static readonly string TCyclerWrapper = "TC Wrapper";
		/// <summary>
		/// Well known logger name for run-time executive.
		/// </summary>
		public static readonly string RunTimeExec = "RTE";
		/// <summary>
		/// Well known logger name for Image Acquisition thread.
		/// </summary>
		public static readonly string ImageAcquisition = "IA Thread";
		/// <summary>
		/// Well known logger name for Data Analysis.
		/// </summary>
		public static readonly string DataAnalysis = "DA";
		/// <summary>
		/// Well known logger name for Reading Group.
		/// </summary>
		public static readonly string ReadingGroup = "RG";
		/// <summary>
		/// Well known logger name for Reference Data Service.
		/// </summary>
		public static readonly string ReferenceDataService = "RDS";
		/// <summary>Well known logger name for Multiplet.</summary>
		public static readonly string Multiplet = "Multiplet";
		/// <summary>
		/// Well known logger name for the Camera Device.
		/// </summary>
		public static readonly string Camera = "Camera";
		/// <summary>
		/// Well known logger name for the Camera status thread.
		/// </summary>
		public static readonly string CameraThread = "Camera Thread";
		/// <summary>
		/// Well known logger name for the iQ2Camera Device.
		/// </summary>
		public static readonly string iQ2Camera = "iQ2Camera";
		/// <summary>
		/// Well known logger name for the iQ2CamFacade Device.
		/// </summary>
		public static readonly string iQ2CamFacade = "iQ2CamFacade";
		/// <summary>
		/// Well known logger name for the SDDCamera Device.
		/// </summary>
		public static readonly string SDDCamera = "SDDCamera";
		/// <summary>
		/// Well known logger name for the SDDCamFacade Device.
		/// </summary>
		public static readonly string SDDCamFacade = "SDDCamFacade";
		/// <summary>
		/// Well known logger name for the OpticalFilters Device.
		/// </summary>
		public static readonly string OpticalFilters = "OpticalFilters";
		/// <summary>
		/// Well known logger name for the OpticalRunStatus.
		/// </summary>
		public static readonly string OpticalRunStatus = "OpticalRun";
		/// <summary>
		/// Well known logger name for the PersistableHeader.
		/// </summary>
		public static readonly string PersistableHeader = "PHeader";
		/// <summary>
		/// Well known logger name for the ManageAcqusition.
		/// </summary>
		public static readonly string ManageAcqusitionRun = "MAcqRun";
		/// <summary>
		/// Well known logger name for the CameraWorkBench.
		/// </summary>
		public static readonly string CameraWorkBench = "CamWB";
		/// <summary>
		/// Well known logger name for the MaskCalibration.
		/// </summary>
		public static readonly string MaskCalibration = "MaskCalib";
		/// <summary>
		/// Well known logger name for the WellFactorCollectionControl.
		/// </summary>
		public static readonly string WellFactorCollectionControl = "WFCollControl";
		/// <summary>Name for the OpticalRunInitiate form.</summary>
		public static readonly string OpticalRunInitiate = "OpticalRunInitiate";
		/// <summary>Name for OpticalRunPrepare form.</summary>
		public static readonly string OpticalRunPrepare = "OpticalRunPrepare";
		/// <summary>Name for OpticalRunMonitor form.</summary>
		public static readonly string OpticalRunMonitor = "OpticalRunMonitor";
		/// <summary>Name for the XmlLoggerConfig.</summary>
		public static readonly string DiagLoggerConfig = "DiagLoggerConfig";
		/// <summary>Name for the BioRadPlateGraphics object.</summary>
		public static readonly string BioRadPlateGraphics = "BioRadPlateGraphics";
		/// <summary>Name for the GenericInstrumentPanelMainForm object.</summary>
		public static readonly string GenericInstrumentPanelMainForm = "GInstrPanelMainForm";
		/// <summary>Name for the DataAcquisitionApp object.</summary>
		public static readonly string DataAcquisitionApp = "DataAcquisitionApp";
		/// <summary>Name for CalibrationRunPrepare form.</summary>
		public static readonly string CalibrationRunPrepare = "CalibrationRunPrepare";
		/// <summary>Name for MainForm form.</summary>
		public static readonly string MainForm = "MainForm";
		/// <summary>Name for the ApplicationStateData object.</summary>
		public static readonly string ApplicationStateData = "AppStateData";
		/// <summary>Name for Applying Alternate Calibration log entries.</summary>
		public static readonly string ApplyAltCalibration = "AltCalibration";
		/// <summary>Name for File (DataFile/Protocol/Platesetup/RunSet) encryption 
		/// and compression utility.</summary>
		public static readonly string FileEncryptCompress = "FileEncryptCompress";
		/// <summary>Name for user preferences form</summary>
		public static readonly string UserPreferences = "UserPreferences";
		/// <summary>Name for user management object.</summary>
		public static readonly string UserManagement = "UserManagement";
        /// <summary>Name for application start up object.</summary>
        public static readonly string ApplicationStartUp = "ApplicationStartUp";
		/// <summary>Name for globber</summary>
		public static readonly string Globber = "Globber";
        /// <summary>Name for notifications</summary>
        public static readonly string Notifications = "Notifications";

		// Fix for Bug 8124
		/// <summary>Name for the machine collection object.</summary>
		public static readonly string MachineCollection = "MachineCollection";

		/// <summary>Name for legacy SE upgrade form</summary>
		public static readonly string SEUpgrade = "SEOldVersionUpgrade";
        #endregion
	}
}
