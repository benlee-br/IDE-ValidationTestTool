using System;
using System.Collections.Generic;
using System.Text;

namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>Get File Filter (extension) description localized string</summary>
    /// <remarks></remarks>
    /// <classinformation>
    ///		<list type="bullet">
    ///			<item name="authors">Authors: </item>
    ///			<item name="review">Last design/code review:</item>
    ///			<item name="requirementid">Requirement ID # : 
    ///				<see href="">924</see> 
    ///			</item>
    ///			<item name="classdiagram">
    ///				<see href="Reference\FileORImageName">Class Diagram</see> 
    ///			</item>
    ///		</list>
    /// </classinformation>
    /// <archiveinformation>
    ///		<list type="bullet">
    ///			<item name="vssfile">$Workfile: FileExtensionDescription.cs $</item>
    ///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Common/FileExtensionDescription.cs $</item>
    ///			<item name="vssrevision">$Revision: 3 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Blie $</item>
    ///			<item name="vssdate">$Date: 3/16/10 11:46a $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion
    public class FileExtensionDescription
    {
        /// <summary>
        /// Get File Filter (extension) description localized string
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static string FileFilterDescription(ApplicationStateData.Setting setting)
        {
            string localizedValue = string.Empty;
            switch (setting)
            {
                case ApplicationStateData.Setting.platesetupfilefilters:
                    localizedValue = Properties.Resources.platesetupfilefilters;
                    break;
                case ApplicationStateData.Setting.genestudyfilefilters:
                    localizedValue = Properties.Resources.genestudyfilefilters;
                    break;
                case ApplicationStateData.Setting.gxdfilefilters:
                    localizedValue = Properties.Resources.gxdfilefilters;
                    break;
                case ApplicationStateData.Setting.allgxdfilefilters:
                    localizedValue = Properties.Resources.allgxdfilefilters;
                    break;
                case ApplicationStateData.Setting.datasetfilefilters:
                    localizedValue = Properties.Resources.datasetfilefilters;
                    break;
                case ApplicationStateData.Setting.protocolfilefilters:
                    localizedValue = Properties.Resources.protocolfilefilters;
                    break;
                case ApplicationStateData.Setting.Alldatasetfilefilters:
                    localizedValue = Properties.Resources.Alldatasetfilefilters;
                    break;
				case ApplicationStateData.Setting.AllDataAnalysisfilefilters:
					localizedValue = Properties.Resources.AllDataAnalysisfilefilters;
					break;
				case ApplicationStateData.Setting.calibrationfilefilters:
                    localizedValue = Properties.Resources.calibrationfilefilters;
                    break;
                case ApplicationStateData.Setting.RunFileFilters:
                    localizedValue = Properties.Resources.RunFileFilters;
                    break;
                case ApplicationStateData.Setting.opd_datasetfilefilters:
                    localizedValue = Properties.Resources.opd_datasetfilefilters;
                    break;
                case ApplicationStateData.Setting.tad_datasetfilefilters:
                    localizedValue = Properties.Resources.tad_datasetfilefilters;
                    break;
                case ApplicationStateData.Setting.odm_datasetfilefilters:
                    localizedValue = Properties.Resources.odm_datasetfilefilters;
                    break;
                case ApplicationStateData.Setting.rdmlfilefilters:
                    localizedValue = Properties.Resources.rdmlfilefilters;
                    break;
                case ApplicationStateData.Setting.LIMSFileFiler:
                    localizedValue = Properties.Resources.LIMSFileFilter;
                    break;
				case ApplicationStateData.Setting.AllContentFilesFilter:
					localizedValue = Properties.Resources.AllContentRunSetFileFiltersText;
					break;
				/*case ApplicationStateData.Setting.ContentZipFileFilter:
					localizedValue = Properties.Resources.AllContentRunSetFileFilterText;
					break;
				case ApplicationStateData.Setting.ContentCSVFileFilter:
					localizedValue = Properties.Resources.ContentCSVFileFilterText;
					break;*/
                default:
                    break;
            }
            return localizedValue;
        }
    }
}
