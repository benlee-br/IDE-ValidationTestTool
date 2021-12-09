using System;
using System.Collections.Generic;
using System.Data;

using BioRad.Common.DiagnosticsLogger;

namespace BioRad.Common
{
	/// <summary>This class contains the settings for a single camera exposure.</summary>
	public class CameraSettings : KeyValueIO
	{
		#region Constants
		private enum Parameters
		{
			LedIntensity,
			LedNumber,
			ChannelColor,
			ExposureTime,
			RedGain,
			BlueGain,
			GreenGain1,
			GreenGain2,
			BinFactor,
			FrameWidth,
			FrameHeight,
			PixelBytes,
			PixelBits,
			FilterPosition,
			FanState,
			FanDutyCycle,
			LedTemperature,
			SaturatedPixelCountThreshold,
			Dwell
		}
		#endregion

		//#region Member Data
		//#endregion

		#region Accessors
		/// <summary>Frame width</summary>
		public int FrameWidth
		{
			get { return int.Parse(this["FrameWidth"]); }
			set { this.Add("FrameWidth", value.ToString()); }
		}
		/// <summary>Frame height</summary>
		public int FrameHeight
		{
			get { return int.Parse(this["FrameHeight"]); }
			set { this.Add("FrameHeight", value.ToString()); }
		}
		/// <summary></summary>
		public int PixelBytes
		{
			get { return int.Parse(this["PixelBytes"]); }
			set { this.Add("PixelBytes", value.ToString()); }
		}
		/// <summary></summary>
		public int PixelBits
		{
			get { return int.Parse(this["PixelBits"]); }
			set { this.Add("PixelBits", value.ToString()); }
		}
		/// <summary></summary>
		public int BinFactor
		{
			get { return int.Parse(this["BinFactor"]); }
			set { this.Add("BinFactor", value.ToString()); }
		}
		/// <summary></summary>
		public double BlueGain
		{
			get { return double.Parse(this["BlueGain"]); }
			set { this.Add("BlueGain", value.ToString()); }
		}
		/// <summary></summary>
		public double GreenGain1
		{
			get { return double.Parse(this["GreenGain1"]); }
			set { this.Add("GreenGain1", value.ToString()); }
		}
		/// <summary></summary>
		public double GreenGain2
		{
			get { return double.Parse(this["GreenGain2"]); }
			set { this.Add("GreenGain2", value.ToString()); }
		}
		/// <summary></summary>
		public double RedGain
		{
			get { return double.Parse(this["RedGain"]); }
			set { this.Add("RedGain", value.ToString()); }
		}
		/// <summary></summary>
		public int ExposureTime
		{
			get { return int.Parse(this["ExposureTime"]); }
			set { this.Add("ExposureTime", value.ToString()); }
		}
		/// <summary></summary>
		public int LedNumber
		{
			get { return int.Parse(this["LedNumber"]); }
			set { this.Add("LedNumber", value.ToString()); }
		}
		/// <summary></summary>
		public int LedIntensity
		{
			get { return int.Parse(this["LedIntensity"]); }
			set { this.Add("LedIntensity", value.ToString()); }
		}
		/// <summary></summary>
		public int ChannelColor
		{
			get { return int.Parse(this["ChannelColor"]); }
			set { this.Add("ChannelColor", value.ToString()); }
		}
		/// <summary></summary>
		public int LedTemperature
		{
			get { return int.Parse(this["LedTemperature"]); }
			set { this.Add("LedTemperature", value.ToString()); }
		}
		/// <summary>One based filter position.</summary>
		public int FilterPosition
		{
			get { return int.Parse(this["FilterPosition"]); }
			set { this.Add("FilterPosition", value.ToString()); }
		}
		/// <summary></summary>
		public int FanState
		{
			get { return int.Parse(this["FanState"]); }
			set { this.Add("FanState", value.ToString()); }
		}
		/// <summary></summary>
		public int FanDutyCycle
		{
			get { return int.Parse(this["FanDutyCycle"]); }
			set { this.Add("FanDutyCycle", value.ToString()); }
		}
		/// <summary></summary>
		public int SaturationThreshold
		{
			get { return int.Parse(this["SaturationThreshold"]); }
			set { this.Add("SaturationThreshold", value.ToString()); }
		}
		/// <summary></summary>
		public int Dwell
		{
			get { return int.Parse(this["Dwell"]); }
			set { this.Add("Dwell", value.ToString()); }
		}
		#endregion

		//#region Delegates and Events
		//#endregion

		#region Constructors
		/// <summary>Initializes a new instance of the CameraSettings class.</summary>
		public CameraSettings()
			: base()
		{
			Init();
		}
		/// <summary>Initializes a new instance of the CameraSettings class from a file.</summary>
		/// <param name="path"></param>
		public CameraSettings(string path)
			: base(path)
		{
		}
		private void Init()
		{
			this.FrameWidth = 0;
			this.FrameHeight = 0;
			this.BinFactor = 0;
			this.BlueGain = 0;
			this.ChannelColor = 0;
			this.Dwell = 0;
			this.ExposureTime = 0;
			this.FanDutyCycle = 0;
			this.FanState = 0;
			this.FilterPosition = 0;
			this.GreenGain1 = 0;
			this.GreenGain2 = 0;
			this.LedIntensity = 0;
			this.LedNumber = 0;
			this.LedTemperature = 0;
			this.PixelBits = 0;
			this.PixelBytes = 0;
			this.RedGain = 0;
			this.SaturationThreshold = 0;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Short string representation of this class.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("POS{0}LED({1},{2},{3})R{4}B{5}G1R{6}G2R{7}RGB{8}ET{9}BIN{10}FAN({11},{12}),SAT{13},DWL{14}",
				FilterPosition, LedNumber, LedIntensity, LedTemperature, RedGain, BlueGain, GreenGain1, GreenGain2,
				ChannelColor, ExposureTime, BinFactor, FanState, FanDutyCycle, SaturationThreshold, Dwell);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public new object Clone()
		{
			CameraSettings cs = this.MemberwiseClone() as CameraSettings;
			cs.m_DataSet = base.m_DataSet.Copy();
			return cs;
		}
		#endregion

		//#region Event Handlers
		//#endregion
	}
}
