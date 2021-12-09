using System;
using System.Collections.Generic;
using System.Text;

namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>
    /// Enumerated type for the DataStatusListener indicating whether the action has
    /// been aborted or is ready.
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
    ///			<item name="vssfile">$Workfile: Execution_Constants.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ExecutableQueue/Execution_Constants.cs $</item>
    ///			<item name="vssrevision">$Revision: 2 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
    ///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

	public partial class Execution_Constants
    {
        #region Constants
        /// <summary>
        /// Dtermines what happens when a new computation is triggered.
        /// </summary>
        public enum SynchMode : int
        {
            /// <summary>
            /// Unassigned.
            /// </summary>
            Unassigned,
            /// <summary>
            /// Trggering a new computations aborts the current one.
            /// </summary>
            Abortable,
            /// <summary>
            /// Triggering a new computation operates in parallel on another thread.
            /// </summary>
            Parallel,
            /// <summary>
            /// Triggering a new computation gets queued up.
            /// </summary>
            Sequential
        }

        /// <summary>
        /// Status of computation.
        /// </summary>
        public enum CompStatus : int
        {
            /// <summary>
            /// Unassigned.
            /// </summary>
            Unassigned,
            /// <summary>
            /// Computation aborted.
            /// </summary>
            Aborted,
            /// <summary>
            /// Computation pending.
            /// </summary>
            Pending,
            /// <summary>
            /// Computation completed.
            /// </summary>
            Ready
        }

        #endregion
    }
}
