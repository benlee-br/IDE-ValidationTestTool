using System;
using System.Collections.Specialized;

namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>
    /// Event arguments describing how much of a computation has been completed.
    /// This is used for handling asynchronous events fired by acomputation to the
    /// parent form, which typically contains a status control.
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
    ///			<item name="vssfile">$Workfile: ProgressStatusEventArgs.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ExecutableQueue/ProgressStatusEventArgs.cs $</item>
    ///			<item name="vssrevision">$Revision: 2 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
    ///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

	public partial class ProgressStatusEventArgs : EventArgs
    {
        #region Constants
        #endregion

        #region Member Data
        /// <summary>
        /// Current count of progress.
        /// </summary>
        private int m_CurrentCount;
        /// <summary>
        /// Current maximum progress count.
        /// </summary>
        private int m_MaxCount;
        /// <summary>
        /// Text describing what computation is occurring.
        /// </summary>
        private string m_TextDescription = string.Empty;
        #endregion

        #region Accessors
        /// <summary>
        /// Gets current progress count.
        /// </summary>
        public int CurrentCount
        {
            get { return m_CurrentCount; }
        }
        /// <summary>
        /// Gets current maximum progress count.
        /// </summary>
        public int MaxCount
        {
            get { return m_MaxCount; }
        }
        /// <summary>
        /// Description of current computation.
        /// </summary>
        public string Description
        {
            get { return m_TextDescription.Clone() as string; }
        }
        #endregion

        #region Delegates and Events
        #endregion

        #region Constructors and Destructor
        /// <summary>
        /// Eventargs object
        /// </summary>
        /// <param name="currentCount"></param>
        /// <param name="maxCount"></param>
        /// <param name="description"></param>
        public ProgressStatusEventArgs(int currentCount, int maxCount, string description)
        {
            m_CurrentCount = currentCount;
            m_MaxCount = maxCount;
            m_TextDescription = description;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Maps a current task currentCount out a maximum of maxCount, whatever its value, to a
        /// value which is relative to a fixed max for the control. For example, the maxCount for a task
        /// may be 64, but the control max count is set to 10, so that 32 maps to 5.
        /// </summary>
        /// <param name="currentCount">Current index of tasks.</param>
        /// <param name="maxCount">Maximum count of tasks.</param>
        /// <param name="maxControlCount">Maximum count set for control.</param>
        /// <returns>Progress count scaled to the maxControlCount value</returns>
        static public int MapCurrentCountToProgressCount(int currentCount, int maxCount, int maxControlCount)
        {
            return (int)((double)(currentCount * maxControlCount) / (double)maxCount);
        }

        #endregion

        #region Event Handlers
        #endregion
    }
}
