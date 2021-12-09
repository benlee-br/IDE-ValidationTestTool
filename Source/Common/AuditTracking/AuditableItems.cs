using System;

namespace BioRad.Common.AuditTracking
{
    #region Documentation Tags
    /// <summary></summary>
    /// <remarks></remarks>
    /// <classinformation>
    ///		<list type="bullet">
    ///			<item name="authors">Authors:Tom Houser</item>
    ///			<item name="review">Last design/code review:</item>
    ///			<item name="requirementid">Requirement ID # : 
    ///				<see href="">????</see> 
    ///			</item>
    ///		</list>
    /// </classinformation>
    #endregion

    public partial class AuditableItems
	{
		#region Constants
		#region Enums
		/// <summary>The auditable tags for the objects in the data file.</summary>
		public enum Auditable : int
		{
			/// <summary>Unassigned value.</summary>
			Unassigned,
			/// <summary>The active experiment number.</summary>	
			ActiveExpNum,
			/// <summary>The active RME file name.</summary>
			RMEFileName,
			/// <summary>This is replacing RMEFileName (which has been left to handle
			/// data files that have been created in v0500 prior to this change).
			/// </summary>
			RMEActiveInfo,
			/// <summary>The active RME type.</summary>
			RMETypeActive,
			/// <summary>The active Well Factor file name.</summary>
			WFFileName,
			/// <summary>This is replacing WFFileName (which has been left to handle
			/// data files that have been created in v0500 prior to this change).
			/// </summary>
			WFActiveInfo,
			/// <summary>The active Well Factor type.</summary>
			WFTypeActive,
			/// <summary>The active wells.</summary>
			ActiveWells,
			/// <summary>This is replacing BFFileName (which has been left to handle
			/// data files that have been created in v0500 prior to this change).
			/// </summary>
			BFActiveInfo,
			/// <summary>The active Background Readings file name.</summary>
			BFFileName,
			/// <summary>The active Background Readings.</summary>
			BFTypeActive,
			/// <summary>The display wells collection.</summary>
			DisplayWells,
			/// <summary>The active fluors for PCR Quant.</summary>
			PCRActiveFluors,
			/// <summary>The active stage for PCR Quant.</summary>
			PCRActiveStageNum,
			/// <summary>The active step for PCR Quant.</summary>
			PCRActiveStepNum,
			/// <summary>The active WellGroup for PCR Quant.</summary>
			PCRActiveWellGroup,
			/// <summary>The active fluor for MC.</summary>
			MCActiveFluor,
			/// <summary>The active stage for MC.</summary>
			MCActiveStageNum,
			/// <summary>The active step for MC.</summary>
			MCActiveStepNum,
			/// <summary>The ThresholdForNegativePeak for MC.</summary>
			MCThresholdForNegativePeakAutoFind,
			/// <summary>The ThresholdForPeakAutoFind for MC.</summary>
			MCThresholdForPeakAutoFind,
			/// <summary>The IncludeNegativePeaks flag for MC.</summary>
			MCIncludeNegativePeaks,
			/// <summary>The IncludePositivePeaks flag for MC.</summary>
			MCIncludePositivePeaks,
			/// <summary>The MaxPeakWidth for MC.</summary>
			MCMaxPeakWidth,
			/// <summary>The MinPeakWidth for MC.</summary>
			MCMinPeakWidth,
			/// <summary>The active fluor for EP.</summary>
			EPActiveFluor,
			/// <summary>The AD X axis fluor.</summary>
			ADFluorIdXAxis,
			/// <summary>The AD Y axis fluor.</summary>
			ADFluorIdYAxis,
			/// <summary>The AD automatic call/manual call setting.</summary>
			ADAutomaticCall,
			/// <summary>The AD well call data.</summary>
			ADCall,
			/// <summary>The AD display mode.</summary>
			ADDisplayMode,
			/// <summary>The AD repeat number.</summary>
			ADRepeatNumber,
			/// <summary>The AD normalize data.</summary>
			ADNormalize,
			/// <summary>The AD horizontal threshold mode (automatic/manual).</summary>
			ADHThresholdMode,
			/// <summary>The AD horizontal threshold value.</summary>
			ADHThreshold,
			/// <summary>The AD vertical threshold mode (automatic/manual).</summary>
			ADVThresholdMode,
			/// <summary>The AD vertical threshold value.</summary>
			ADVThreshold,
			/// <summary>The EP call method.</summary>
			EPCallMethod,
			/// <summary>The EP number of ranks to use.</summary>
			EPNumRanks,
			/// <summary>The EP number of repeats to average.</summary>
			EPNumRepeats,
			/// <summary>EP sort data by call.</summary>
			EPSortDataByCall,
			/// <summary>The EP tolerance calculation method.</summary>
			EPTolMethod,
			/// <summary>The EP tolerance value.</summary>
			EPTolValue,
			/// <summary>The EP control definition data.</summary>
			EPWells,
			/// <summary>The analysis mode (fluorophore vs target).</summary>
			PCRAnalysisMode,
			/// <summary>The PCR baseline mode.</summary>
			PCRBaseLine,
			/// <summary>The PCR digital filter.</summary>
			PCRDigitalFilter,
			/// <summary>The PCR display mode.</summary>
			PCRDisplayMode,
			/// <summary>The PCR baseline method.</summary>
			PCRBaseLineMethod,
			/// <summary>The PCR data window (beginning/end and value).</summary>
			PCRDataWindow,
			/// <summary>The PCR digital filter enabled.</summary>
			PCRDigitalFilterGlobalEnabled,
			/// <summary>The PCR threshold value.</summary>
			PCRThresholdValue,
			/// <summary>PCR auto/manual threshold.</summary>
			PCRAutoThreshold,
			/// <summary>PCR auto/manual baseline.</summary>
			PCRAutoBaseline,
			/// <summary>The PCR baseline cyles (begin/end) values.</summary>
			PCRBaselineValue,
			/// <summary>Edits to the plate description</summary>
			PlateDescription,
			/// <summary>Edits to the plate file name</summary>
			PlateSetupImport,
			/// <summary>Edits to the plate type</summary>
			PlateType,
			/// <summary>Edits to the assay name</summary>
			PlateAssayName,
			/// <summary>Edits to the experiment name</summary>
			PlateExperimentName,
			/// <summary>Edits to the fluor units</summary>
			PlateFluorUnits,
			/// <summary>Edits to the wells in the plate defination</summary>
			PlateWellsEdit,
			/// <summary>Edits to the fluos(dye layers) in the plate defination</summary>
			PlateFluorsChange,
			/// <summary>Edits to the well group(s) in the plate defination</summary>
			PlateWellGroupChange,
			/// <summary>The MC digital filter.</summary>
			MCDigitalFilter,
			/// <summary>The MC global filter enabled.</summary>
			MCDigitalFilterGlobalEnabled,
			/// <summary>The MC peak override data.</summary>
			MCPeakOverride,
			/// <summary>The GE active fluor.</summary>
			GEActiveFluor,
			/// <summary>The GE Analysis Mode (normalized/relative qty).</summary>
			GEAnalysisMode,
			/// <summary>The GE data set.</summary>
			GEDataSet,
			/// <summary>The GE Data set Efficiency value.</summary>
			GEDataSetEff,
			/// <summary>The GE global control condition setting.</summary>
			GEGlobalControl,
			/// <summary>The GE corrected standard deviation applied setting</summary>
			GeCorrStdDev,
			/// <summary>The GE Graph expression.</summary>
			GEGraphExp,
			/// <summary>The GE scaling option.</summary>
			GEScaling,
			/// <summary>The GE graph error setting.</summary>
			GEGraphError,
            /// <summary>The GE graph error setting.</summary>
            GEGraphErrorGroup,
            /// <summary>The GE graph error value.</summary>
            GEGraphErrorValue,
			/// <summary>The GE condition object</summary>
			GECondition,
			/// <summary>The GE condition full name.</summary>
			GEConditionFullName,
			/// <summary>Is the control a reference?</summary>
			GEConditionIsRef,
			/// <summary>The GE replicate information.</summary>
			GEReplicate,
			/// <summary>The GE replicate full name.</summary>
			GEReplicateFullName,
			/// <summary>Is the control a reference?</summary>
			GEReplicateIsRef,
			/// <summary>The GE Replicate auto efficiency.</summary>
			GEReplicateAutoEff,
			/// <summary>The GE Replicate efficiency value.</summary>
			GEReplicateEff,
			/// <summary>GE sample data</summary>
			GESampleData,
			/// <summary>GE target</summary>
			GETarget,
			/// <summary>GE Sample Name Grouping option</summary>
			GESampleNameGroupingOption,
			/// <summary>The GE Bar chart x axis.</summary>
			GEBarGraphXAxisDisplay,
            /// <summary>The GE Bar chart Y axis.</summary>
			GEBarGraphYAxisDisplay,//US1157 
            /// <summary>The MutiGE data files set.</summary>
            MGEDataFiles,
			/// <summary>The description in the PersistableHeader (Notes textbox)</summary>
			Description,
			/// <summary>The data file was created.</summary>
			FileCreated,
			/// <summary>The fully qualified name.</summary>
			FullyQualifiedName,
			/// <summary>Run Time audit information.</summary>
			/// <remarks>Run-time protocol editing and run completes are tracked with
			/// this value.</remarks>
			RunTime,
			/// <summary>A change in the status of the run time protocol.</summary>
			RunTimeProtocol,
			/// <summary>Run completed</summary>
			RunComplete,
			/// <summary>Run started.</summary>
			RunStart,
			/// <summary>EDO Updated with DWF.</summary>
			WFChange,
			/// <summary>The Protocol run definition has been changed for a fast scan run.</summary>
			RunDefinitionChanged,
			/// <summary>The sample volume has been changed.</summary>
			ProtocolVolumeChanged,
			/// <summary>The lid temperature has been changed.</summary>
			ProtocolLidTemperatureChanged,
			/// <summary>The step temperature for the End Point protocol has been changed.</summary>
			EndPointProtocolStepTemperatureChanged,
			/// <summary>Interpolated Plate reads have been added</summary>
			InterpolatedPlateReads,
			/// <summary>Audit Trail date Control</summary>
			AuditTrailControl,
			/// <summary>EDO2 Run ID</summary>
			RunID,
			/// <summary>Analysis Parameter</summary>
			AnalysisParameter,
			/// <summary>Dye Calibration Data</summary>
			DyeCalibrationData,
			/// <summary>Ct Detection Algorithm</summary>
			CtAlgorithm,
			/// <summary>Plate replaced during a run or in the data file.</summary>
			PlateReplace,
			/// <summary>Plate edited during a run or in the data file.</summary>
			PlateEdit,
			/// <summary>The begin and end cycles to analyze.</summary>
			CyclesToAnalyze,
			/// <summary>Drift correction applied or not.</summary>
			DriftCorrection,
			/// <summary>Cluster by target/sample</summary>
			ClustergramClusterBy,
			/// <summary>Clustergram Split out replicates.</summary>
			ClustergramSplitOutReplicates,
			/// <summary>Scatter plot control sample name</summary>
			ControlSample,
			/// <summary>Scatter plot experimental sample name.</summary>
			ScatterPlotExperimentalSample,
			/// <summary>Scatter plot regulation threshold value.</summary>
			ScatterPlotRegulationThreshold,
			/// <summary>Volcano plot experimental sample name.</summary>
			VolcanoPlotExperimentalSample,
			/// <summary>Volcano plot regulation threshold value.</summary>
			VolcanoPlotRegulationThreshold,
			/// <summary>Volcano plot p-value.</summary>
			VolcanoPlotPValue,
            /// <summary>Bar plot p-value.</summary>
			BarPlotPValue,//TT440
            /// <summary>ANOVA p-value threshold.</summary>
			AnovaPValueThreshold,//TT440
            /// <summary>Heat map experimental sample name.</summary>
            HeatMapExperimentalSample,			
			/// <summary>Heat Map Split out replicates.</summary>
			HeatMapSplitOutReplicates,
			/// <summary>The file selected for display in the heat map (gene study only).</summary>
			HeatMapSelectedFile,
            /// <summary>Edits to the standard units</summary>
            StandardUnits
		}
		#endregion
		#endregion

		#region Methods
		/// <summary></summary>
		/// <param name="auditableItem"></param>
		/// <param name="containsEmptyValues">If true (either the new or old value is an
		/// empty string) returns the localized string with '(*)' added to the end of it.</param>
		/// <returns></returns>
		public static string GetItemLocalizedName( Auditable auditableItem, bool containsEmptyValues )
		{
			try
			{
                //AuditableItemsStrResNames.Names name = (AuditableItemsStrResNames.Names)(Enum.Parse( typeof( AuditableItemsStrResNames.Names ), 
                //    auditableItem.ToString() ));
                string name = Properties.Resources.ResourceManager.GetString
                    (auditableItem.ToString());

				// Fix for Bug 4144 - if the old or new value for this change is an
				// empty string - the name will have a (*) added to the end
				if(containsEmptyValues)
				{
                    return StringUtility.FormatString(Properties.Resources.ChangeDetailsWithMissingInfo_1, name );
				}
				else
				{
					return StringUtility.FormatString(name );
				}
			}
			catch( ArgumentException )
			{
				return "Object not found";
			}
		}
		#endregion
	}
}
