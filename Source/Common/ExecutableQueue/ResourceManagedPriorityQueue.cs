using System;

namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>
    /// Priority queue which checks various resources, such as memory and/or worker thread availability
    /// before making a request.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <classinformation>
    ///		<list type="bullet">
    ///			<item name="authors">Authors:JLerner</item>
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
    ///			<item name="vssfile">$Workfile: ResourceManagedPriorityQueue.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ExecutableQueue/ResourceManagedPriorityQueue.cs $</item>
    ///			<item name="vssrevision">$Revision: 2 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
    ///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

	public partial class ResourceManagedPriorityQueue : PriorityQueue
    {
        #region Constants
        #endregion

        #region Member Data
        /// <summary>
        /// If there is not enough memory resources to run, we wait this amount of time
        /// for the Garbage collector to free up memory.
        /// </summary>
        private int m_NotEnoughMemoryWaitTime = 5000;
        /// <summary>
        /// Resource manager for controlling synchronous sequential function calls.
        /// </summary>
        private SystemResourcesManager m_ResourceManager = null;
        #endregion

        #region Accessors
        /// <summary>
        /// Resource manager.
        /// </summary>
        public SystemResourcesManager ResourceManagerObject
        {
            get { return m_ResourceManager; }
            set { m_ResourceManager = value; }
        }
        /// <summary>
        /// Not enough memory wait time.
        /// </summary>
        public int NotEnoughMemoryWaitTime
        {
            get { return m_NotEnoughMemoryWaitTime; }
            set { m_NotEnoughMemoryWaitTime = value; }
        }

        #endregion

        #region Delegates and Events
        #endregion

        #region Constructors and Destructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public ResourceManagedPriorityQueue()
        {
            m_ResourceManager = new SystemResourcesManager();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Resource managed dequeue.
        /// </summary>
        /// <returns>Workitem to be placed on the queue.</returns>
        public WorkItem ResourceManagedDequeue()
        {
            if (m_ResourceManager.CheckResources(m_NotEnoughMemoryWaitTime) == false)
                return null;
            else
                return Dequeue();
        }

        #endregion

        #region Event Handlers
        #endregion
    }
}
