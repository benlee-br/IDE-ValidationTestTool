using System;
using System.Globalization;

namespace BioRad.Common.Utilities
{
	#region Documentation Tags
	/// <summary>A static class that returns the Calibration QC values based on the instrument type</summary>
	/// <remarks>
	/// 
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Shabnam</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href=""></see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: CalibrationQCUtility.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Utilities/CalibrationQCUtility.cs $</item>
	///			<item name="vssrevision">$Revision: 1 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ssarup $</item>
	///			<item name="vssdate">$Date: 3/18/08 6:24a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public static class CalibrationQCUtility
	{
		#region Methods
		/// <summary>Get the Calibration QC values for a CFX96 instrument.</summary>
		/// <param name="minFluorescenceForLoadedWells">The minimum fluorescence for loaded wells.</param>
		/// <param name="maxFluorescenceForLoadedWells">The maximum fluoroescence for loaded wells.</param>
		/// <param name="minFluorescenceForEmptyWells">The minimum fluorescence for empty wells.</param>
		/// <param name="maxFluorescenceForEmptyWells">The maximum fluoroescence for empty wells.</param>
		/// <param name="minAmbientTemperature">The minimum ambient temperature.</param>
		/// <param name="maxAmbientTemperature">The maximum ambient temperature.</param>
		/// <param name="minShuttleTemperature">The minimum shuttle temperature.</param>
		/// <param name="maxShuttleTemperature">The maximum shuttle temperature.</param>
		/// <param name="primaryChannelBrightnessGreaterThanOtherChannels">Does the fluorescence from the 
		/// primary channel for the fluor have to be greater than the fluorescence from other channels?</param>
		/// <param name="loadedWellsFluorescenceGreaterThanEmptyWells">Does the fluorescence from the loaded wells 
		/// have to be greater than the fluorescence from empty wells?</param>
		/// <param name="loadedWellsPrimaryChannelFluorescenceGreaterThanEmptyWells">The value by which the loaded 
		/// wells have to be brighter than the empty wells.</param>
		public static void GetCFX96CalibrationQCValues
			(ref float minFluorescenceForLoadedWells,
			ref float maxFluorescenceForLoadedWells,
			ref float minFluorescenceForEmptyWells,
			ref float maxFluorescenceForEmptyWells,
			ref int minAmbientTemperature,
			ref int maxAmbientTemperature,
			ref int minShuttleTemperature,
			ref int maxShuttleTemperature,
			ref bool primaryChannelBrightnessGreaterThanOtherChannels,
			ref bool loadedWellsFluorescenceGreaterThanEmptyWells,
			ref float loadedWellsPrimaryChannelFluorescenceGreaterThanEmptyWells)
		{
			ApplicationStateData stateData = ApplicationStateData.GetInstance;

			minFluorescenceForLoadedWells = StringUtility.StringToFloat(
				stateData[ApplicationStateData.Setting.CFX96MinFluorescenceForLoadedWells],
				CultureInfo.InvariantCulture);

			maxFluorescenceForLoadedWells = StringUtility.StringToFloat(
				stateData[ApplicationStateData.Setting.CFX96MaxFluorescenceForLoadedWells],
				CultureInfo.InvariantCulture);

			minFluorescenceForEmptyWells = StringUtility.StringToFloat(
				stateData[ApplicationStateData.Setting.CFX96MinFluorescenceForEmptyWells],
				CultureInfo.InvariantCulture);

			maxFluorescenceForEmptyWells = StringUtility.StringToFloat(
				stateData[ApplicationStateData.Setting.CFX96MaxFluorescenceForEmptyWells],
				CultureInfo.InvariantCulture);

			minAmbientTemperature = int.Parse(stateData[ApplicationStateData.Setting.CFX96MinAmbientTemperature]);
			
			maxAmbientTemperature = int.Parse(stateData[ApplicationStateData.Setting.CFX96MaxAmbientTemperature]);
			
			minShuttleTemperature = int.Parse(stateData[ApplicationStateData.Setting.CFX96MinShuttleTemperature]);
			
			maxShuttleTemperature = int.Parse(stateData[ApplicationStateData.Setting.CFX96MaxShuttleTemperature]);
			
			primaryChannelBrightnessGreaterThanOtherChannels =
				bool.Parse(stateData[ApplicationStateData.Setting.PrimaryChannelBrightnessGreaterThanOtherChannels]);
			
			loadedWellsFluorescenceGreaterThanEmptyWells =
				bool.Parse(stateData[ApplicationStateData.Setting.LoadedWellsFluorescenceGreaterThanEmptyWells]);
			
			loadedWellsPrimaryChannelFluorescenceGreaterThanEmptyWells = StringUtility.StringToFloat(
				stateData[ApplicationStateData.Setting.CFX96LoadedWellsPrimaryChannelFluorescenceGreaterThanEmptyWells],
				CultureInfo.InvariantCulture);

            // TT138 Simulation mode?
            if (System.Configuration.ConfigurationManager.AppSettings["simulation"] != null)
            {
                bool simulation = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["simulation"].ToString());
                if (simulation)
                {
                    minFluorescenceForLoadedWells = float.MinValue;
                    maxFluorescenceForLoadedWells = float.MaxValue;

                    minFluorescenceForEmptyWells = float.MinValue;
                    maxFluorescenceForEmptyWells = float.MaxValue;

                    primaryChannelBrightnessGreaterThanOtherChannels = false;
                    loadedWellsFluorescenceGreaterThanEmptyWells = false;
                    loadedWellsPrimaryChannelFluorescenceGreaterThanEmptyWells = float.MinValue;
                }
            }
        }

		/// <summary>Get the Calibration QC values for a CFX384 instrument.</summary>
		/// <param name="minFluorescenceForLoadedWells">The minimum fluorescence for loaded wells.</param>
		/// <param name="maxFluorescenceForLoadedWells">The maximum fluoroescence for loaded wells.</param>
		/// <param name="minFluorescenceForEmptyWells">The minimum fluorescence for empty wells.</param>
		/// <param name="maxFluorescenceForEmptyWells">The maximum fluoroescence for empty wells.</param>
		/// <param name="minAmbientTemperature">The minimum ambient temperature.</param>
		/// <param name="maxAmbientTemperature">The maximum ambient temperature.</param>
		/// <param name="minShuttleTemperature">The minimum shuttle temperature.</param>
		/// <param name="maxShuttleTemperature">The maximum shuttle temperature.</param>
		/// <param name="primaryChannelBrightnessGreaterThanOtherChannels">Does the fluorescence from the 
		/// primary channel for the fluor have to be greater than the fluorescence from other channels?</param>
		/// <param name="loadedWellsFluorescenceGreaterThanEmptyWells">Does the fluorescence from the loaded wells 
		/// have to be greater than the fluorescence from empty wells?</param>
		/// <param name="loadedWellsPrimaryChannelFluorescenceGreaterThanEmptyWells">The value by which the loaded 
		/// wells have to be brighter than the empty wells.</param>
		public static void GetCFX384CalibrationQCValues
			(ref float minFluorescenceForLoadedWells,
			ref float maxFluorescenceForLoadedWells,
			ref float minFluorescenceForEmptyWells,
			ref float maxFluorescenceForEmptyWells,
			ref int minAmbientTemperature,
			ref int maxAmbientTemperature,
			ref int minShuttleTemperature,
			ref int maxShuttleTemperature,
			ref bool primaryChannelBrightnessGreaterThanOtherChannels,
			ref bool loadedWellsFluorescenceGreaterThanEmptyWells,
			ref float loadedWellsPrimaryChannelFluorescenceGreaterThanEmptyWells)
		{
			ApplicationStateData stateData = ApplicationStateData.GetInstance;

			minFluorescenceForLoadedWells = StringUtility.StringToFloat(
				stateData[ApplicationStateData.Setting.CFX384MinFluorescenceForLoadedWells],
				CultureInfo.InvariantCulture);

			maxFluorescenceForLoadedWells = StringUtility.StringToFloat(
				stateData[ApplicationStateData.Setting.CFX384MaxFluorescenceForLoadedWells],
				CultureInfo.InvariantCulture);

			minFluorescenceForEmptyWells = StringUtility.StringToFloat(
				stateData[ApplicationStateData.Setting.CFX384MinFluorescenceForEmptyWells],
				CultureInfo.InvariantCulture);

			maxFluorescenceForEmptyWells = StringUtility.StringToFloat(
				stateData[ApplicationStateData.Setting.CFX384MaxFluorescenceForEmptyWells],
				CultureInfo.InvariantCulture);

			minAmbientTemperature = int.Parse(stateData[ApplicationStateData.Setting.CFX384MinAmbientTemperature]);

			maxAmbientTemperature = int.Parse(stateData[ApplicationStateData.Setting.CFX384MaxAmbientTemperature]);

			minShuttleTemperature = int.Parse(stateData[ApplicationStateData.Setting.CFX384MinShuttleTemperature]);

			maxShuttleTemperature = int.Parse(stateData[ApplicationStateData.Setting.CFX384MaxShuttleTemperature]);

			primaryChannelBrightnessGreaterThanOtherChannels =
				bool.Parse(stateData[ApplicationStateData.Setting.PrimaryChannelBrightnessGreaterThanOtherChannels]);

			loadedWellsFluorescenceGreaterThanEmptyWells =
				bool.Parse(stateData[ApplicationStateData.Setting.LoadedWellsFluorescenceGreaterThanEmptyWells]);

			loadedWellsPrimaryChannelFluorescenceGreaterThanEmptyWells = StringUtility.StringToFloat(
				stateData[ApplicationStateData.Setting.CFX384LoadedWellsPrimaryChannelFluorescenceGreaterThanEmptyWells],
				CultureInfo.InvariantCulture);

            // TT138 Simulation mode?
            if (System.Configuration.ConfigurationManager.AppSettings["simulation"] != null)
            {
                bool simulation = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["simulation"].ToString());
                if (simulation)
                {
                    minFluorescenceForLoadedWells = float.MinValue;
                    maxFluorescenceForLoadedWells = float.MaxValue;

                    minFluorescenceForEmptyWells = float.MinValue;
                    maxFluorescenceForEmptyWells = float.MaxValue;

                    primaryChannelBrightnessGreaterThanOtherChannels = false;
                    loadedWellsFluorescenceGreaterThanEmptyWells = false;
                    loadedWellsPrimaryChannelFluorescenceGreaterThanEmptyWells = float.MinValue;
                }
            }
        }

		/// <summary>Get the Calibration QC values for a MO instrument.</summary>
		/// <param name="minFluorescenceForLoadedWells">The minimum fluorescence for loaded wells.</param>
		/// <param name="maxFluorescenceForLoadedWells">The maximum fluoroescence for loaded wells.</param>
		/// <param name="minFluorescenceForEmptyWells">The minimum fluorescence for empty wells.</param>
		/// <param name="maxFluorescenceForEmptyWells">The maximum fluoroescence for empty wells.</param>
		/// <param name="minAmbientTemperature">The minimum ambient temperature.</param>
		/// <param name="maxAmbientTemperature">The maximum ambient temperature.</param>
		/// <param name="minShuttleTemperature">The minimum shuttle temperature.</param>
		/// <param name="maxShuttleTemperature">The maximum shuttle temperature.</param>
		/// <param name="primaryChannelBrightnessGreaterThanOtherChannels">Does the fluorescence from the 
		/// primary channel for the fluor have to be greater than the fluorescence from other channels?</param>
		/// <param name="loadedWellsFluorescenceGreaterThanEmptyWells">Does the fluorescence from the loaded wells 
		/// have to be greater than the fluorescence from empty wells?</param>
		/// <param name="loadedWellsPrimaryChannelFluorescenceGreaterThanEmptyWells">The value by which the loaded 
		/// wells have to be brighter than the empty wells.</param>
		public static void GetMOCalibrationQCValues
			(ref float minFluorescenceForLoadedWells,
			ref float maxFluorescenceForLoadedWells,
			ref float minFluorescenceForEmptyWells,
			ref float maxFluorescenceForEmptyWells,
			ref int minAmbientTemperature,
			ref int maxAmbientTemperature,
			ref int minShuttleTemperature,
			ref int maxShuttleTemperature,
			ref bool primaryChannelBrightnessGreaterThanOtherChannels,
			ref bool loadedWellsFluorescenceGreaterThanEmptyWells,
			ref float loadedWellsPrimaryChannelFluorescenceGreaterThanEmptyWells)
		{
			ApplicationStateData stateData = ApplicationStateData.GetInstance;

			minFluorescenceForLoadedWells = StringUtility.StringToFloat(
				stateData[ApplicationStateData.Setting.MOMinFluorescenceForLoadedWells],
				CultureInfo.InvariantCulture);

			maxFluorescenceForLoadedWells = StringUtility.StringToFloat(
				stateData[ApplicationStateData.Setting.MOMaxFluorescenceForLoadedWells],
				CultureInfo.InvariantCulture);

			minFluorescenceForEmptyWells = StringUtility.StringToFloat(
				stateData[ApplicationStateData.Setting.MOMinFluorescenceForEmptyWells],
				CultureInfo.InvariantCulture);

			maxFluorescenceForEmptyWells = StringUtility.StringToFloat(
				stateData[ApplicationStateData.Setting.MOMaxFluorescenceForEmptyWells],
				CultureInfo.InvariantCulture);

			minAmbientTemperature = int.Parse(stateData[ApplicationStateData.Setting.MOMinAmbientTemperature]);

			maxAmbientTemperature = int.Parse(stateData[ApplicationStateData.Setting.MOMaxAmbientTemperature]);

			minShuttleTemperature = int.Parse(stateData[ApplicationStateData.Setting.MOMinShuttleTemperature]);

			maxShuttleTemperature = int.Parse(stateData[ApplicationStateData.Setting.MOMaxShuttleTemperature]);

			primaryChannelBrightnessGreaterThanOtherChannels =
				bool.Parse(stateData[ApplicationStateData.Setting.PrimaryChannelBrightnessGreaterThanOtherChannels]);

			loadedWellsFluorescenceGreaterThanEmptyWells =
				bool.Parse(stateData[ApplicationStateData.Setting.LoadedWellsFluorescenceGreaterThanEmptyWells]);

			loadedWellsPrimaryChannelFluorescenceGreaterThanEmptyWells = StringUtility.StringToFloat(
				stateData[ApplicationStateData.Setting.MOLoadedWellsPrimaryChannelFluorescenceGreaterThanEmptyWells],
				CultureInfo.InvariantCulture);

            // TT138 Simulation mode?
            if (System.Configuration.ConfigurationManager.AppSettings["simulation"] != null)
            {
                bool simulation = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["simulation"].ToString());
                if (simulation)
                {
                    minFluorescenceForLoadedWells = float.MinValue;
                    maxFluorescenceForLoadedWells = float.MaxValue;

                    minFluorescenceForEmptyWells = float.MinValue;
                    maxFluorescenceForEmptyWells = float.MaxValue;

                    primaryChannelBrightnessGreaterThanOtherChannels = false;
                    loadedWellsFluorescenceGreaterThanEmptyWells = false;
                    loadedWellsPrimaryChannelFluorescenceGreaterThanEmptyWells = float.MinValue;
                }
            }
        }
		#endregion
	}
}
