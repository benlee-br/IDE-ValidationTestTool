using System;
using System.Collections.Specialized;
using System.Collections;
using System.Diagnostics;

namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>
    /// Collection of listeners requested by a client. This seems to be especially critical for
    /// a high-speed action such as dragging a threshold in PCR where there will be rapidly queued
    /// evnts, since this provides a colleciotn for a unique sink for each rapidly queued action.
    /// </summary>
    /// <remarks>
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
    ///			<item name="vssfile">$Workfile: DataStatusListeners.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ExecutableQueue/DataStatusListeners.cs $</item>
    ///			<item name="vssrevision">$Revision: 2 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
    ///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

	public partial class DataStatusListeners : IDisposable
    {
        #region Constants
        /// <summary>
        /// Determines whether each Listener triggers a callback, or whether
        /// the whole group triggers one callback.
        /// </summary>
        public enum SignalMode : int 
        {
            /// <summary>
            /// Each Listener triggers a callback.
            /// </summary>
            PerListener,
            /// <summary>
            /// Only the completion of the whole grouip triggers a callback.
            /// </summary>
            PerGroup

        }
        #endregion

        #region Member Data
        /// <summary>
        /// Flag indicating whether the local data has been disposed or not.
        /// </summary>
        private bool m_Disposed = false;
        /// <summary>
        /// Collection of DataStatusListeners.
        /// </summary>
        private Queue m_DataStatusListeners;
        /// <summary>
        /// Collection of key-based listners.
        /// </summary>
        private Hashtable m_Listeners = null;
        /// <summary>
        /// Synchronization object for accessors.
        /// </summary>
        private object m_SynchLock = new object();
        #endregion

        #region Accessors

        #endregion

        #region Delegates and Events
        #endregion

        #region Constructors and Destructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public DataStatusListeners()
        {
            m_DataStatusListeners = new Queue();
            m_Listeners = new Hashtable();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add a listener to collection.
        /// </summary>
        /// <param name="dataStatusListener">Listener object.</param>
        public void Enqueue(DataStatusListener dataStatusListener)
        {
            lock (m_SynchLock)
            {
                m_DataStatusListeners.Enqueue(dataStatusListener);
            }
        }

        /// <summary>
        /// Returns true if collections contains key.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <returns>True if contains key, else false.</returns>
        public bool Contains(object key)
        {
            return m_Listeners.ContainsKey(key);
        }

        /// <summary>
        /// Add DataStatusListener object to collection.
        /// </summary>
        /// <param name="key">Action ID ot GUID identifying this DSL.</param>
        /// <param name="dataStatusListener">Listener object.</param>
        public void Add(object key, DataStatusListener dataStatusListener)
        {
            lock (m_SynchLock)
            {
                m_Listeners.Add(key, dataStatusListener);
            }
        }

        /// <summary>
        /// Return of count.
        /// </summary>
        /// <returns>Count of listners.</returns>
        public int Count()
        {
            int count = 0;
            lock(m_SynchLock)
            {
              //  count = m_DataStatusListeners.Count;
                count = m_Listeners.Count;
            }

            return count;
        }

        /// <summary>
        /// Add a listener to collection.
        /// </summary>
        public DataStatusListener Dequeue()
        {
            DataStatusListener dsl = null;
            lock (m_SynchLock)
            {
                dsl = m_DataStatusListeners.Dequeue() as DataStatusListener;
            }
            return dsl;
        }

        /// <summary>
        /// Remove the DataStatusListener object based upon its compID.
        /// </summary>
        /// <param name="key"></param>
        public void Remove(object key)
        {
            lock (m_SynchLock)
            {
                if(m_Listeners.Contains(key) == true)
                {
                    DataStatusListener dsl = m_Listeners[key] as DataStatusListener;
                    m_Listeners.Remove(key);
                    dsl = null;
                }
            }
        }
        /// <summary>
        /// Clear all collections.
        /// </summary>
        public void Clear()
        {
            m_DataStatusListeners.Clear();
            m_Listeners.Clear();
        }

        /// <summary>
        /// Explicitly releases all resources used by this object.
        /// </summary>
        public virtual void Dispose()
        {
            if (!m_Disposed)
            {
                // Call the overridden Dispose method that contains common cleanup code
                // Pass true to indicate that it is called from Dispose
                Dispose(true);
                // Prevent subsequent finalization of this object. This is not needed 
                // because managed and unmanaged resources have been explicitly released
                GC.SuppressFinalize(this);
            }
        }
        /// <summary>
        /// Releases the unmanaged resources used by the object and optionally 
        /// releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; 
        /// false to release only unmanaged resources. 
        /// </param>
        /// <remarks>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the 
        /// runtime from inside the finalizer and you should not reference 
        /// other objects. Only unmanaged resources can be disposed.
        /// </remarks>
        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (m_DataStatusListeners != null)
                {
                    m_DataStatusListeners.Clear();
                    m_DataStatusListeners = null;
                }
                if (m_Listeners != null)
                {
                    m_Listeners.Clear();
                    m_Listeners = null;
                }
                m_Disposed = true;
            }
        }

        #endregion

        #region Event Handlers
        #endregion
    }
}
