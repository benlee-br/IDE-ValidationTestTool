using System;
using System.Collections.Specialized;
using System.Threading;

namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>
    /// This class provides methods for checking available worker thread 
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
    ///			<item name="vssfile">$Workfile: SystemResourcesManager.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ExecutableQueue/SystemResourcesManager.cs $</item>
    ///			<item name="vssrevision">$Revision: 2 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
    ///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

	public partial class SystemResourcesManager
    {
        #region Constants
        #endregion

        #region Member Data
        /// <summary>
        /// Available memory percent threshold for allowing computation to proceed.
        /// </summary>
        private double m_AvailableMemoryPercentThreshold = .10;
        #endregion

        #region Accessors
        /// <summary>
        /// Gets and Sets percent available memory threshold value.
        /// </summary>
        public double AvailableMemoryPercentThreshold
        {
            get { return m_AvailableMemoryPercentThreshold; }
            set { m_AvailableMemoryPercentThreshold = value; }
        }
        #endregion

        #region Delegates and Events
        #endregion

        #region Constructors and Destructor
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SystemResourcesManager()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="availableMemoryPercentThreshold"></param>
        public SystemResourcesManager(double availableMemoryPercentThreshold)
        {
            m_AvailableMemoryPercentThreshold = availableMemoryPercentThreshold;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Check to see whether there are any threads available in the thread pool and whether there is
        /// adequate memory.
        /// </summary>
        /// <returns>True if there are adequate resources, else false.</returns>
        public bool GetResources(ref bool growThreadPool, ref bool notEnoughMemory)
        {
            int availableThreads = ThreadUtils.GetAvailableThreads;
            growThreadPool = (availableThreads == 0);
 //           notEnoughMemory = !(MemoryResourceUtil.GetAvailableMemoryPercent > m_AvailableMemoryPercentThreshold);
            notEnoughMemory = false;
            if (growThreadPool == true || notEnoughMemory == true)
            {
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Test to see if we have enough memory and threads to proceed.
        /// </summary>
        /// <param name="notEnoughMemoryWaitTime">Wait time for checking for available memory.</param>
        /// <returns></returns>
        public bool CheckResources(int notEnoughMemoryWaitTime)
        {
            bool bSuccess = false;
            bool growThreadPool = false;
            bool notEnoughMemory = false;

            if (GetResources(ref growThreadPool, ref notEnoughMemory) == false)
            {
                if (growThreadPool == true)
                {
                    bSuccess = ThreadUtils.IncreaseWorkerThreads(10, 0);
                    // If cannot get enough thread pool worker thread to run, then bail.
                    if (bSuccess == false)
                        return false;
                }

                // Have enough threads. Now deal swith possible memory issues.
                if (notEnoughMemory == true)
                {
                    int deltaWait = 100; // Milliseconds.
                    int totalWait = 0;
                    // All we can do here is wait for a set period of time up to notEnoughMemoryWaitTime for
                    // the Garbage collector to free up memory.
                    while (totalWait < notEnoughMemoryWaitTime)
                    {
                        Thread.Sleep(deltaWait);
                        // Now recheck for memory. If OK, then proceed, else return false.
                        if (CheckAvailableMemory() == true)
                        {
                            bSuccess = true;
                            break;
                        }

                        totalWait += deltaWait;
                    }
                    if (CheckAvailableMemory() == false)
                        return false;
                }
                bSuccess = true;
            }
            else
                bSuccess = true;
            return bSuccess;
        }

        /// <summary>
        /// Check for sufficient memory.
        /// </summary>
        /// <returns>true if enough memory, else false</returns>
        public bool CheckAvailableMemory()
        {
            return (MemoryResourceUtil.GetAvailableMemoryPercent > m_AvailableMemoryPercentThreshold);
        }
        #endregion

        #region Event Handlers
        #endregion
    }
}
