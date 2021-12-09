using System;
using System.Collections;
using System.Text;

namespace BioRad.Common.Devices
{
	#region Documentation Tags
	/// <summary>This class stores camera specific configuration information. 
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Pramod Walse</item>
	///			<item name="review">Last design/code review:1/14/04, Pramod Walse</item>
	///			<item name="conformancereview">Conformance review:2/17/04, Pramod Walse</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">1253</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\DeviceManager.wmf">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: CameraConfigInfo.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Devices/CameraConfigInfo.cs $</item>
	///			<item name="vssrevision">$Revision: 15 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	[Serializable]
	public partial class CameraConfigInfo : ICloneable
	{
		#region Constants
		/// <summary>
		/// Default time out for camera.
		/// </summary>
		public static readonly int s_DefaultTimeOut = 10000;
		#endregion

        #region Member Data
		/// <summary>Interval time frame between exposures for continuous 
		/// exposure sequence.</summary>
		private TimeType m_IntervalTime;
		/// <summary>Delay time frame between exposures for continuous exposure sequence.
		/// </summary>
		private TimeType m_DelayTime;
		/// <summary>Vertical binning factor.</summary>
		private int m_Vbinning;
		/// <summary>Horizontal binning factor.</summary>
		private int m_Hbinning;
		/// <summary>Number of pixels per well at or above which a significant saturation
		/// condition is declared for the well.  A significant saturation condition
		/// for a specified fraction of all wells typically triggers a counter response
		/// normally a decrease in the camera exposure time.</summary>
		private int m_SaturatedPixelCountThreshold;
		/// <summary>Trigger fraction of wells whose saturated pixel count is GE
		/// SaturatedPixelCountThreshold.  These two threshold values act together to 
		/// define when a saturation condition exists on the plate and to trigger a 
		/// response to that condition.</summary>
		private float m_SaturatedWellFractionThreshold;
		/// <summary>When the fraction of loaded wells found to be saturated is LE this 
		/// value, the next longer exposure time will be added to the current exposure
		/// multiplet.  This is the means of assuring the best signal/noise ratio for the
		/// data set.</summary>
		private float m_SaturatedWellFractionBoost;
		/// <summary>When the fraction of loaded wells found to be saturated is GE this 
		/// value, the corresponding exposure time will be permanently eliminated from the
		/// exposure multiplet for the current filter pair.</summary>
		private float m_SaturatedWellFractionElimination;
		/// <summary>Margin value used to calculate the Saturation level in mask overlay.
		/// </summary>
		private int m_WellCountSaturateMargin;
		/// <summary>An array of the allowed Exposure Times and their associated
		/// bias values.</summary>
		/// <remarks>The unit for the Exposure times is milliseconds.The first dimension of the
		/// Array stores the Exposure time and the second is the associated bias value.</remarks>
		private ExposureSettings[] m_ExposureSettings;
		/// <summary>
		/// 
		/// </summary>
		private Algorithms[] m_Algorithms;
		/// <summary>The initial (default) exposure time and bias value.</summary>
		/// <remarks>The unit for the Exposure times is milliseconds.The first dimension of the
		/// Array stores the Exposure time and the second is the associated bias value.</remarks>
		private ExposureSettings m_ExposureInitialSetting;
		/// <summary>
		/// Sequenced objects of gain control levels.
		/// Automatic Gain Control level, adjustment AGC value, if supported.
		/// A camera sensor may have AGC, and if it does, it may or may not allow 
		/// turning it off.
		/// </summary>
		private GainLevel[] m_GainLevels;
		/// <summary>
		/// Sequenced objects of bias levels.
		/// </summary>
		private BiasLevel[] m_BiasLevels;
		/// <summary>The number of milliseconds to wait for the camera data
		/// reading event before timeout during data acquisition.
		/// </summary>
		private int m_TimeOut;
		/// <summary>The estimated time it takes to take an exposure and transfer the 
		/// image from the Camera to ImageAcquisition.</summary>
		private int m_ImageTransferTime;
		/// <summary>Required warmup time in minutes(from configuration).</summary>
		private int m_RequiredWarmupTime;
		/// <summary>If the camera's lamp exceeds this time in hours, a warning needs to be displayed(from configuration).</summary>
		private int m_LampWarning;
		/// <summary>If the camera's lamp exceeds this time in hours, it needs to be changed(from configuration).</summary>
		private int m_LampError;
		/// <summary>0=default (don't run extended selftest),1=run extended test, 2=force to standard mode,3=force to pipelining mode</summary>
		private int m_TransferMode;
		/// <summary>The supported filter positions.</summary>
		private int[] m_FilterPositions;
        #endregion

        #region Accessors
		/// <summary>Interval time frame between exposures for continuous 
		/// exposure sequence.</summary>
		public TimeType IntervalTime
		{
			get { return this.m_IntervalTime;}
			set { this.m_IntervalTime = value;}
		}
		/// <summary>Delay time frame between exposures for continuous exposure sequence.
		/// </summary>
		public TimeType DelayTime
		{
			get { return this.m_DelayTime;}
			set { this.m_DelayTime = value;}
		}
		/// <summary>Vertical binning factor.</summary>
		public int VerticalBinning
		{
			get { return this.m_Vbinning;}
			set { this.m_Vbinning = value;}
		}
		/// <summary>Horizontal binning factor.</summary>
		public int HorizontalBinning
		{
			get { return this.m_Hbinning;}
			set { this.m_Hbinning = value;}
		}
		/// <summary>
		/// Sequenced objects  of gain control levels.
		/// Automatic Gain Control level, adjustment AGC value, if supported.
		/// A camera sensor may have AGC, and if it does, it may or may not allow 
		/// turning it off</summary>
		public GainLevel[] GainLevels		
		{
			get { return (this.m_GainLevels == null) 
					  ? new GainLevel[0] : this.m_GainLevels;}
			set { this.m_GainLevels = value;}
		}
		/// <summary>
		/// Sequenced objects of bias levels.
		/// </summary>
		public BiasLevel[] BiasLevels		
		{
			get { return (this.m_BiasLevels == null) ?
					  new BiasLevel[0] : this.m_BiasLevels;}
			set { this.m_BiasLevels = value;}
		}
		/// <summary>Number of pixels per well at or above which a significant saturation
		/// condition is declared for the well.  A significant saturation condition
		/// for a specified fraction of all wells typically triggers a counter response
		/// normally a decrease in the camera exposure time.</summary>
		public int SaturatedPixelCountThreshold
		{
			get { return this.m_SaturatedPixelCountThreshold;}
			set { this.m_SaturatedPixelCountThreshold = value;}
		}
		/// <summary>Trigger fraction of wells whose saturated pixel count is >=
		/// SaturatedPixelCountThreshold.  These two threshold values act together to 
		/// define when a saturation condition exists on the plate and to trigger a 
		/// response to that condition.</summary>
		public float SaturatedWellFractionThreshold
		{
			get { return this.m_SaturatedWellFractionThreshold;}
			set { this.m_SaturatedWellFractionThreshold = value;}
		}
		/// <summary>When the fraction of loaded wells found to be saturated is GE this 
		/// value, the next longer exposure time will be added to the current exposure
		/// multiplet.  This is the means of assuring the best signal/noise ratio for the
		/// data set.</summary>
		public float SaturatedWellFractionBoost
		{
			get { return this.m_SaturatedWellFractionBoost;}
			set { this.m_SaturatedWellFractionBoost = value;}
		}
		/// <summary>When the fraction of loaded wells found to be saturated is >= this 
		/// value, the corresponding exposure time will be permanently eliminated from the
		/// exposure multiplet for the current filter pair.</summary>
		public float SaturatedWellFractionElimination
		{
			get { return this.m_SaturatedWellFractionElimination;}
			set { m_SaturatedWellFractionElimination = value;}
		}
		/// <summary>Margin value used to calculate the Saturation level in mask overlay.</summary>
		public int WellCountSaturateMargin
		{
			get { return this.m_WellCountSaturateMargin;}
			set { this.m_WellCountSaturateMargin = value;}
		}
		/// <summary>An array of the allowed Exposure Times and their associated
		/// bias values.</summary>
		public ExposureSettings[] ExposureAllSettings
		{
			get { return (this.m_ExposureSettings == null) ?
					  new ExposureSettings[0] : this.m_ExposureSettings;}
			set { this.m_ExposureSettings = value;}
		}
		/// <summary>An array of the allowed algorithms.</summary>
		public Algorithms[] AlgorithmsAllSettings
		{
			get { return (this.m_Algorithms == null) ?
					  new Algorithms[0] : this.m_Algorithms;}
			set { this.m_Algorithms = value;}
		}
		/// <summary>Default exposure calculator.</summary>
		public string DefaultExposureCalculatorName
		{
			get 
			{
				foreach(Algorithms a in AlgorithmsAllSettings)
				{
					if ( a.IsDefault )
						return a.Displayname;
				}
				return "***";
			}
		}
		/// <summary>The intial (default) exposure time and its associated bias value.</summary>
		/// <remarks>The unit for the Exposure times is milliseconds.The first dimension of the
		/// Array stores the Exposure time and the second is the associated bias value.
		/// Eg. this.m_ExposureInitialSetting[0,0] is the exposure time and 
		/// this.m_ExposureInitialSetting[0,1] is the bias value to use with that exposure time.</remarks>
		public ExposureSettings ExposureInitialSetting
		{
			get { return this.m_ExposureInitialSetting;}
			set { this.m_ExposureInitialSetting = value;}
		}
		/// <summary>Gets the default exposuretime value.</summary>
		public TimeType DefaultExposureTime
		{
			get { return this.m_ExposureInitialSetting.ExposureTime;}
		}
		/// <summary>Gets the longest possible ExposureTime.</summary>
		public TimeType LongestExposureTime
		{
			get 
			{
				TimeType exposureTime = new TimeType();
				foreach(ExposureSettings exposureSettings in this.m_ExposureSettings)
				{
					if(exposureSettings.ExposureTime > exposureTime)
					{
						exposureTime = exposureSettings.ExposureTime;
					}
				}
			return exposureTime;
			}
		}
		/// <summary>The number of milliseconds to wait for the camera data
		/// reading event before timeout during data acquisition.
		/// </summary>
		public int TimeOut
		{
			get { return this.m_TimeOut;}
			set { this.m_TimeOut = value;}
		}
		/// <summary>The estimated time it takes to take an exposure and transfer the 
		/// image from the Camera to ImageAcquisition.</summary>
		public int ImageTransferTime
		{
			get { return this.m_ImageTransferTime;}
			set { this.m_ImageTransferTime = value;}
		}
		/// <summary>The device's required warmup time in minutes(from configuration).</summary>
		public int RequiredWarmupTime
		{
			get { return this.m_RequiredWarmupTime;}
			set { this.m_RequiredWarmupTime = value;}
		}
		/// <summary>If device's lamp exceeds this time in hours, a warning needs to be displayed.</summary>
		public int LampWarning
		{
			get { return this.m_LampWarning;}
			set { this.m_LampWarning = value;}
		}
		/// <summary>If device's lamp exceeds this time in hours, an error needs to be displayed.</summary>
		public int LampError
		{
			get { return this.m_LampError;}
			set { this.m_LampError = value;}
		}		

		/// <summary>Get or set the transfer mode for the camera </summary>
		public int TransferMode
		{
			get { return this.m_TransferMode;}
			set { this.m_TransferMode =value;}
		}
		/// <summary>Filter Positions </summary>
		/// <remarks>ReadOnly property.</remarks>
		public int[] FilterPositions
		{
			get { return this.m_FilterPositions;}
		}
		#endregion

        #region Constructors and Destructor
		/// <summary>
		/// Default Empty Constructor.
		/// </summary>
		public CameraConfigInfo()
		{
		}
        #endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="exposureTime"></param>
		/// <returns></returns>
		public int GetExposureTimeIndex(TimeType exposureTime)
		{
			int index = -1;
			for( int i = 0; index < m_ExposureSettings.Length; i++ )
			{
				if ( m_ExposureSettings[i].ExposureTime.Time == exposureTime.Time )
				{
					index = i;
					break;
				}
			}
			if ( index == -1 )
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("Index for exposure time ");
				sb.Append(exposureTime);
				sb.Append(", not found.");
				throw new ApplicationException(sb.ToString());
			}
			return index;
		}
		/// <summary>Create a clone of the CameraConfigInfo object.</summary>
		/// <remarks>This method creates a deep clone of the CameraConfigInfo object by creating 
		/// a clone of the contained ExposureSettings[]. </remarks>
		/// <returns>An object of the CameraConfigInfo type.</returns>
		public object Clone()
		{
			CameraConfigInfo clonedCameraConfigInfo = (CameraConfigInfo) 
				this.MemberwiseClone();

			if(clonedCameraConfigInfo.m_IntervalTime != null)
				clonedCameraConfigInfo.m_IntervalTime = (TimeType)
					this.m_IntervalTime.Clone();
			if(clonedCameraConfigInfo.m_DelayTime != null)
				clonedCameraConfigInfo.m_DelayTime = (TimeType)
					this.m_DelayTime.Clone();

			//clone the ExposureSettings in this stage
			ArrayList exposureSettings = new ArrayList();
			foreach(ExposureSettings exposureSetting in this.ExposureAllSettings)
			{
				ExposureSettings clonedExposureSetting = 
					(ExposureSettings) exposureSetting.Clone();
				exposureSettings.Add(clonedExposureSetting);
			}
			clonedCameraConfigInfo.ExposureAllSettings = 
				(ExposureSettings[]) exposureSettings.ToArray(typeof(ExposureSettings));

			ArrayList algorithmsArray = new ArrayList();
			foreach(Algorithms algor in this.AlgorithmsAllSettings)
			{
				algorithmsArray.Add((Algorithms)algor.Clone());
			}
			clonedCameraConfigInfo.AlgorithmsAllSettings = 
				(Algorithms[]) algorithmsArray.ToArray(typeof(Algorithms));
			
			//Clone initial setting
			if(this.ExposureInitialSetting != null)
			{
				ExposureSettings clonedExposureInitialSetting = 
					(ExposureSettings) this.ExposureInitialSetting.Clone();
				clonedCameraConfigInfo.ExposureInitialSetting = clonedExposureInitialSetting;
			}

			//clone Bias Levels
			ArrayList biasLevels = new ArrayList();
			foreach(BiasLevel biasLevel in this.BiasLevels)
			{
				BiasLevel clonedBiasLevel = 
					(BiasLevel) biasLevel.Clone();
				biasLevels.Add(clonedBiasLevel);
			}
			clonedCameraConfigInfo.BiasLevels = 
				(BiasLevel[]) biasLevels.ToArray(typeof(BiasLevel));

			//clone Gain Levels
			ArrayList gainLevels = new ArrayList();
			foreach(GainLevel gainLevel in this.GainLevels)
			{
				GainLevel clonedGainLevel = 
					(GainLevel) gainLevel.Clone();
				gainLevels.Add(clonedGainLevel);
			}
			clonedCameraConfigInfo.GainLevels = 
				(GainLevel[]) gainLevels.ToArray(typeof(GainLevel));


			return clonedCameraConfigInfo;
		}

		/// <summary>Creates the array of supported filter positions.</summary>
		/// <param name="filterPositions">A comma separated string containing the 
		/// filter positions.</param>
		public void SetFilterPositions(string filterPositions)
		{
			if((filterPositions == null) || (filterPositions.Length.Equals(0)))
				this.m_FilterPositions = new int[0];

			// split the string based on ','
			string[] positions = filterPositions.Split(',');
			this.m_FilterPositions = new int[positions.Length];
			int index = 0;
			foreach(string position in positions)
			{
				this.m_FilterPositions[index] = int.Parse(position);
				index++;
			}
		}
		#endregion
	}
}
