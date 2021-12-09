using System;
using System.Text;

using BioRad.Common.DiagnosticsLogger;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Codes for use in class <see cref="Results"/> to identify a specific return results.
	/// </summary>
	/// <remarks>
	/// This class was written to be used as the return type for interface methods. 
	/// Allows interface method to return a success/failed status along with an message 
	/// describing why interface method failed.
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
	///			<item name="vssfile">$Workfile: Results.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Common/Results.cs $</item>
	///			<item name="vssrevision">$Revision: 49 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Vnguyen $</item>
	///			<item name="vssdate">$Date: 1/26/11 2:21p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public enum ResultCode : int
	{
		/// <summary>
		/// 
		/// </summary>
		success,
		/// <summary>
		/// Undefined code.
		/// </summary>
		undefined,
		/// <summary>
		/// Local protocol running on thermal cycler device.
		/// </summary>
		LocalProtocolRunning,
		/// <summary>
		/// One or more argument(s) to a method where found to be bad.
		/// </summary>
		BadArgument,
		/// <summary>
		/// 
		/// </summary>
		NullArgument,
		/// <summary>
		/// 
		/// </summary>
		Exception,
		/// <summary>
		/// 
		/// </summary>
		StopRun,
		/// <summary>
		/// 
		/// </summary>
		NullThermalCycler,
		/// <summary>
		/// 
		/// </summary>
		NullCamera,
		/// <summary>
		/// 
		/// </summary>
		InsufficientDwellTimeLeft,
		/// <summary>
		/// 
		/// </summary>
		NotInImageAcquisitionMode,
		/// <summary>
		/// 
		/// </summary>
		EDOWriteError,
		/// <summary>
		/// 
		/// </summary>
		InvalidProtocol,
		/// <summary>
		/// 
		/// </summary>
		InvalidPlate,
		/// <summary>
		/// 
		/// </summary>
		RunInProgress,
		/// <summary>
		/// 
		/// </summary>
		NotConnectedThermalCycler,
		/// <summary>
		/// 
		/// </summary>
		BadConfiguration,
		/// <summary>Camera not connected.</summary>
		NotConnectedCamera,
		/// <summary>Timed out waiting for communication from the camera.</summary>
		CameraTimeOut,
		/// <summary> the case is/was open, so it now the sensor can be can be reset</summary>
		CameraCaseOpenDetected,
		/// <summary>THe camera optical liid is open</summary>
		CameraOpticalLidOpen,
		/// <summary>Camera image is over saturated. Data acquisition cannot continue.</summary>
		OverSaturatedImage,
		/// <summary>Invalid firmware version.</summary>
		InvalidFirmwareVersion,
		/// <summary>The multiplet is being explicitely closed.</summary>
		MultipletExplicitelyClosed,
		/// <summary>Camera Presetting failed.</summary>
		CameraPreSettingFailed,
		/// <summary>Camera Presetting succeeded.</summary>
		CameraPreSettingSucceeded,
		/// <summary>
		/// Result code for invalid calling method in EndPoint
		/// </summary>
		EndPointInvalidMethod,
		/// <summary>
		/// Result code for outliers controls in EndPoint
		/// </summary>
		EndPointOutliers,
		/// <summary>The RFUs for the positive and negative controls
		/// overlap in EndPoint.</summary>
		EndPointPosNegCntOverlap,
		/// <summary>The RFU range is invalidin EndPoint.</summary>
		EndPointInvalidRFURange,
		/// <summary>The Positive and Negative limits overlap in EndPoint.</summary>
		EndPointPosNegLimitsOverlap,

		// Run Parameters - Denali3
		/// <summary>The Protocol is Real Time Protocol and the instrument is non-Real Time.</summary>
		RunningRTProtocolOnNonRTInstrument,
		/// <summary>The MeltCurveStep definition is invalid for a MiniOpticon.</summary>
		InvalidMCStepForMiniOpticon,
		/// <summary>The Protocol is non-Real Time and the instrument is Real Time.</summary>
		RunningNonRTProtocolOnRTInstrument,
		/// <summary>The Protocol is non-Real Time and the instrument is a MiniOpticon.</summary>
		RunningNonRTProtocolOnMiniOpticon,
		/// <summary>The Protocol has a Gradient Step and the instrument has 384 wells.</summary>
		RunningGradientOn384WellsInstrument,
		/// <summary>Empty Plate.</summary>
		EmptyPlate,
		/// <summary>Well count mismatch between the plate and baseunit.</summary>
		WellCountMismatch,
		/// <summary>The number of cycles for the Plate Read is less than the 
		/// recommended value.</summary>
		CyclesLessThanRecommentedForPlateRead,
		// User rights
		/// <summary>The user does not the permission to perform the 
		/// operation.</summary>
		UserDoesNotHavePermissionForOperation,
		/// <summary>No protocol has been selected for the run.</summary>
		NoProtocolSelectedForRun,
		/// <summary>No plate has been selected for the run.</summary>
		NoPlateSelectedForRun,
		/// <summary>An error has occurred.</summary>
		Error,
		/// <summary>Sample Volume has been changed by the system.</summary>
		SampleVolumeChanged,

		// Fix for Bug 7099
		/// <summary>The ramp rate is invalid for the instrument.</summary>
		RampRateInvalid,
		/// <summary>Start at instrument option is not valid for MO.</summary>
		StartAtInstrumentNotAvailableForMiniOpticons,
		/// <summary>The instrument does not have all the needed calibration
		/// files.</summary>
		MissingCalibrationFiles,
		/// <summary>
		/// Known AMD processormachine caused duplicate plate read error
		/// </summary>
		DuplicatePlateReads,
		// Fix for Bug 9890
		/// <summary>The GradientStep definition is invalid for a MiniOpticon.</summary>
		InvalidGradientStepForMiniOpticon,
		/// <summary>
		/// CFX 2 plate invalid
		/// </summary>
		InvalidPlateForCFX2,
		// Bug 11530 
		/// <summary>Melt Curve is not allowed.</summary>
		MCNotAllowed
	}

	#region Documentation Tags
	/// <summary>
	/// Results class maintains an internal success/failed state. 
	/// Also contains string member for returning additional information.
	/// </summary>
	/// <remarks>
	/// This class was written to be used as the return type for interface methods. 
	/// Allows interface method to return a success/failed status along with an message 
	/// describing why interface method failed.
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
	///			<item name="vssfile">$Workfile: Results.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Common/Results.cs $</item>
	///			<item name="vssrevision">$Revision: 49 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Vnguyen $</item>
	///			<item name="vssdate">$Date: 1/26/11 2:21p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	[Serializable]
	public partial class Results
	{
		#region Member Data
		/// <summary>
		/// Diagnostics log item which contains additional information results.
		/// For example, called method failed because of an exception. DiagnosticsLogItem object
		/// will contain the exception object.
		/// </summary>
		DiagnosticsLogItem m_DiagnosticsLogItem = null;
		/// <summary>
		/// Internal machine representation for success/fail state.
		/// </summary>
		private bool m_Result = false;
		/// <summary>
		/// Message buffer for returning additional information on reason method failed.
		/// </summary>
		private StringBuilder m_Message = new StringBuilder();
		/// <summary>
		/// Additional information describing the failure. Use only if IsFailed is true.
		/// </summary>
		private ResultCode m_ResultCode = ResultCode.undefined;
		private long m_ErrorCode = 0;
		private object m_ResultObject = null;
		private string m_InvariantCultureText = string.Empty;
		private string m_CurrentCultureText = string.Empty;
		private DateTime m_DateTime;
		private string m_WarningMessage = string.Empty;
		private Exception m_Exception = null;
		#endregion

		#region Accessors
		/// <summary>Get time error was detected.</summary>
		public DateTime DateTime
		{
			get { return m_DateTime; }
		}
		/// <summary>
		/// Get/Set error code. Zero = success.
		/// </summary>
		/// <remarks>
		/// This is here for egacy reasons. 
		/// Use the ErrorCode method.
		/// </remarks>
		public ResultCode ResultCode
		{
			get { return m_ResultCode; }
			set
			{
				m_ResultCode = value;
				m_ErrorCode = (long)m_ResultCode;
			}
		}
		/// <summary>
		/// Get/Set error code. Zero = success.
		/// </summary>
		public long ErrorCode
		{
			get { return m_ErrorCode; }
			set { m_ErrorCode = value; }
		}
		/// <summary>
		/// Get/Set DiagnosticsLogItem. Can be null.
		/// </summary>
		public DiagnosticsLogItem DiagnosticsLogItem
		{
			get { return m_DiagnosticsLogItem; }
			set { m_DiagnosticsLogItem = value; }
		}
		/// <summary>
		/// Get/Set message text. 
		/// Can be invariant or current culture text.
		/// Up to the caller to decide. Use the invariant and current
		/// accessor to set text based on the culture.
		/// </summary>
		/// <remarks>
		/// This is here for egacy reasons. 
		/// Use the InvariantCultureText and CurrentCultureText methods.
		/// </remarks>
		public string Message
		{
			get
			{
				return m_Message.ToString();
			}
			set
			{
				m_DateTime = DateTime.Now;
				m_Message.Remove(0, m_Message.Length);
				m_Message.Append(value);
			}
		}
		/// <summary>
		/// Get/set invariant culture text.
		/// </summary>
		public string InvariantCultureText
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_InvariantCultureText))
					return Message;

				return this.m_InvariantCultureText;
			}
			set
			{
				m_DateTime = DateTime.Now;
				this.m_InvariantCultureText = value;
			}
		}
		/// <summary>
		/// Get/set current culture text.
		/// </summary>
		public string CurrentCultureText
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_CurrentCultureText))
					return Message;

				return this.m_CurrentCultureText;
			}
			set
			{
				m_DateTime = DateTime.Now;
				this.m_CurrentCultureText = value;
			}
		}
		/// <summary>
		/// Represents a mutable string of characters.
		/// </summary>
		public StringBuilder MessageBuilder
		{
			get { return m_Message; }
		}

		/// <summary>
		/// Get/Set return object.
		/// </summary>
		/// <remarks>
		/// if you need to return any object whether success or fail
		/// </remarks>
		public object ResultsObject
		{
			get { return m_ResultObject; }
			set { m_ResultObject = value; }
		}
		/// <summary>
		/// Warning message, if any
		/// </summary>
		public string WarningMessage
		{
			get { return m_WarningMessage; }
			set { m_WarningMessage = value; }
		}
		/// <summary>
		/// The exception, if any, associated with this Results object.
		/// </summary>
		public Exception Exception
		{
			get { return m_Exception; }
			set { m_Exception = value; }
		}
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Initializes a new instance of the Results class. Internal state is initiliaze to failed state.
		/// </summary>
		public Results()
		{
		}
		/// <summary>
		/// Constructs a new instance with a certain success state.
		/// </summary>
		/// <param name="success">the success state to inialize the object to.  True for success, 
		/// false for fail.</param>
		public Results(bool success)
		{
			if (success)
				SetSuccess();
			else
				SetFailed();
		}
		#endregion

		#region Methods
		/// <summary>
		/// Test state of object for success.
		/// </summary>
		/// <returns>Returns true if state is set to success. False otherwise</returns>
		public bool IsSuccess()
		{
			return this.m_Result;
		}
		/// <summary>
		/// Test state of object for fail.
		/// </summary>
		/// <returns>Returns true if state is set to failed. False otherwise</returns>
		public bool IsFailed()
		{
			return !this.m_Result;
		}
		/// <summary>
		/// Set the state of object to success and set the result code to success.
		/// </summary>
		public void SetSuccess()
		{
			this.m_Result = true;
			this.ResultCode = ResultCode.success;
		}
		/// <summary>
		/// Set the state of object to failed.
		/// </summary>
		public void SetFailed()
		{
			this.m_Result = false;
			this.ResultCode = ResultCode.undefined;
		}
		/// <summary>
		/// Set to fail and initialize string with message passed as argument
		/// </summary>
		/// <param name="message"></param>
		public void SetFailed(string message)
		{
			this.SetFailed();
			this.Message = message;
		}
		/// <summary>
		/// Set to fail and initialize string with message passed as argument
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="resultCode">The result code.</param>
		public void SetFailed(string message, ResultCode resultCode)
		{
			this.SetFailed();
			this.Message = message;
			this.m_ResultCode = resultCode;
		}
		/// <summary>
		/// Set to fail and initialize string with message passed as argument
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="resultCode">The result code.</param>
		/// <param name="exception">The exception object.</param>
		public void SetFailed(string message, ResultCode resultCode, Exception exception)
		{
			this.SetFailed();
			this.Message = message;
			this.m_ResultCode = resultCode;
			if (exception != null)
				this.m_ResultObject = exception;
		}
		/// <summary>
		/// Append messageToAppend to current message
		/// </summary>
		/// <param name="messageToAppend"></param>
		public void Append(string messageToAppend)
		{
			this.m_Message.Append(messageToAppend);
		}
		/// <summary>
		/// Serious error or just a warning
		/// </summary>
		/// <returns>Returns true if error is serious. False otherwise</returns>
		public bool IsSeriousError()
		{
			bool isError = true;
			switch (m_ErrorCode)
			{
				default:
					isError = true;
					break;

				// This is a warning and user can select to proceed with the run
				// 01/13/10 VN: Defect 11080: Ignore this warning since UI allows user to continue the run or not
				// Ignored only when running in console mode
				case (int)ResultCode.CyclesLessThanRecommentedForPlateRead:
				case (int)ResultCode.RunningNonRTProtocolOnRTInstrument:
				{
					if(ApplicationStateData.GetInstance.IsConsoleMode)
						isError = false;
				}
				break;				
			}
			return isError;
		}
		#endregion
	}
}
