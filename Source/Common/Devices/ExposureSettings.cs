using System;

namespace BioRad.Common.Devices
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
	///			<item name="authors">Authors:</item>
	///			<item name="review">Last design/code review:1/14/04, Pramod Walse</item>
	///			<item name="conformancereview">Conformance review:2/17/04, Pramod Walse</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">1253</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: ExposureSettings.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Devices/ExposureSettings.cs $</item>
	///			<item name="vssrevision">$Revision: 5 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	[Serializable]
	public partial class ExposureSettings : ICloneable
	{
		#region Member Data
		/// <summary>The exposure time in milliseconds.</summary>
		private TimeType m_ExposureTime;
		/// <summary>The default bias value associated 
		/// with that exposure time.</summary>
		private int m_Bias;
		/// <summary>The default gain control level value associated with 
		/// that exposure time.</summary>
		private int m_Gain;
		/// <summary>The normalization factor associated with 
		/// that exposure time.</summary>
		private double m_NormalizationFactor;
		/// <summary>Is it a default. (initial setting)</summary>
		private bool m_IsDefault;
		/// <summary>Should this exposure time be used or not in RTE,
		///  always set to true initially.</summary>
		private bool m_IsValidExposureTime = true;
		#endregion

		#region Accessors
		/// <summary>The exposure time in milliseconds.</summary>
		public TimeType ExposureTime
		{
			get { return this.m_ExposureTime;}
			set { this.m_ExposureTime = value;}
		}
		/// <summary>The bias value associated with that exposure time.</summary>
		public int Bias
		{
			get { return this.m_Bias;}
			set { this.m_Bias = value;}
		}
		/// <summary>The default gain control level value associated with 
		/// that exposure time.</summary>
		public int Gain
		{
			get { return this.m_Gain;}
			set { this.m_Gain = value;}
		}
		/// <summary>The normalization factor associated with 
		/// that exposure time.</summary>
		public double NormalizationFactor
		{
			get { return this.m_NormalizationFactor;}
			set { this.m_NormalizationFactor = value;}
		}
		/// <summary>Is it a default. (initial setting)</summary>
		public bool IsDefault
		{
			get { return this.m_IsDefault;}
			set { this.m_IsDefault= value;}
		}
		/// <summary>
		/// Should this exposure time be used or not in RTE,
		/// always set to true initially.
		/// If true this exposure time is used during image acquisition.
		/// This gets set to false if the plate saturation with the exposure
		/// time is saturatedwellfractionelimination.
		/// </summary>
		public bool IsValidExposureTime
		{
			get { return this.m_IsValidExposureTime;}
			set { this.m_IsValidExposureTime = value;}
		}
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Default Empty Constructor.
		/// </summary>
		public ExposureSettings()
		{
		}
		#endregion

		#region Methods

		/// <summary>Create a clone of the ExposureSettings object.</summary>
		/// <returns>An object of the ExposureSettings type.</returns>
		public object Clone()
		{
			ExposureSettings clonedObject = (ExposureSettings)
				this.MemberwiseClone();

			clonedObject.m_ExposureTime = (TimeType) this.m_ExposureTime.Clone();
			return clonedObject;
		}

		#endregion
	}
}
