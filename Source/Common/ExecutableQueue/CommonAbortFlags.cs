using System;
using System.Collections.Specialized;
//using System.EnterpriseServices;

namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>
    /// Shared flags between threads, allowing one to cleanly abort a long operation.
    /// </summary>
    /// <remarks>
    /// This may require resolution regarding variables shared between threads.
    /// </remarks>
    /// <classinformation>
    ///		<list type="bullet">
    ///			<item name="authors">Authors: JLerner</item>
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
    ///			<item name="vssfile">$Workfile: CommonAbortFlags.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ExecutableQueue/CommonAbortFlags.cs $</item>
    ///			<item name="vssrevision">$Revision: 1 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Jlerner $</item>
    ///			<item name="vssdate">$Date: 7/28/06 6:09p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

//    [Synchronization]
    public class CommonAbortFlags
    {
        #region Constants
        #endregion

        #region Member data
        /// <summary>
        /// Abort flag.
        /// </summary>
        private volatile bool m_Aborting = false;
        /// <summary>
        /// Abort flag.
        /// </summary>
        private volatile bool m_Aborted = false;

        /// <summary>
        /// Locking object for threadsafe access.
        /// </summary>
        private object m_SyncLock = new object();
        #endregion

        #region Constructors and Destructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public CommonAbortFlags()
        {
        }
        #endregion

        #region Accessors
        /// <summary>
        /// Gets current abort flag value.
        /// </summary>
        public bool AbortingFlag
        {
            get 
            {
                lock (m_SyncLock)
                {
                    return m_Aborting;
                }
            }
           set 
            {
                lock (m_SyncLock)
                {
                    m_Aborting=value;
                }
            }

        }
        /// <summary>
        /// Gets current abort flag value.
        /// </summary>
        public bool AbortedFlag
        {
            get
            {
                lock (m_SyncLock)
                {
                    return m_Aborted;
                }
            }
            set 
            {
                lock (m_SyncLock)
                {
                    m_Aborted=value;
                }
            }
        }
        #endregion

        #region Delegates and Events
        #endregion

        #region Methods
        #endregion

        #region Event Handlers
        #endregion
    }
}
