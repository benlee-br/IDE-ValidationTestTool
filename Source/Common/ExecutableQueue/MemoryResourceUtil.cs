using System;
using System.Collections.Specialized;
using System.Diagnostics;

namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>
    /// In order to determine whether are adequate system resources to carry out an
    /// action, one must determine proerties about available memory resources.
    /// </summary>
    /// <remarks>
    /// Remarks section for class.
    /// </remarks>
    /// <classinformation>
    ///		<list type="bullet">
    ///			<item name="authors">Authors:</item>
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
    ///			<item name="vssfile">$Workfile: MemoryResourceUtil.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ExecutableQueue/MemoryResourceUtil.cs $</item>
    ///			<item name="vssrevision">$Revision: 2 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
    ///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

	public partial class MemoryResourceUtil
    {
        #region Constants
        #endregion

        #region Member Data 
        #endregion

        #region Accessors
        /// <summary>
        /// Available memory as percent of total working set memory.
        /// </summary>
        public static double GetAvailableMemoryPercent
        {
            get 
            {
                long workingSetmemory = Environment.WorkingSet;
                PerformanceCounter perfCounter = new PerformanceCounter("Memory", "Available Bytes");
                long physicalMemoryLimit = perfCounter.RawValue;
                return ((double)(physicalMemoryLimit - workingSetmemory) / physicalMemoryLimit);
            }
        }
        #endregion

        #region Delegates and Events
        #endregion

        #region Constructors and Destructor
        #endregion

        #region Methods
        #endregion

        #region Event Handlers
        #endregion
    }
}
