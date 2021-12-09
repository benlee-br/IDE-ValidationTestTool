using System;
using System.Collections;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Represents a first-in, first-out collection of objects by priority. 
	/// Type to efficiently support finding the item with the highest 
	/// priority across a series of operations.	
	/// </summary>
	/// <remarks>Thread-safe, members use a locking mechanism.</remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Ralph Ansell</item>
	///			<item name="review">Last design/code review:</item>
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
	///			<item name="vssfile">$Workfile: PriorityQueue.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Collections/PriorityQueue.cs $</item>
	///			<item name="vssrevision">$Revision: 5 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Jlerner $</item>
	///			<item name="vssdate">$Date: 1/16/07 1:57p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public class PriorityQueue : IDisposable
	{
		#region Queue Priorities
		/// <summary>
		/// List of priorities
		/// </summary>
		public enum Priority
		{
			/// <summary>
			/// Scheduled before work items with any other priority.  
			/// </summary>
			Highest = 0,
			/// <summary>
			/// Scheduled after work items with Highest priority and before those with Normal priority.  
			/// </summary>
			AboveNormal,
			/// <summary>
			/// Scheduled after work items with AboveNormal priority and before those with 
			/// BelowNormal priority. Work items have Normal priority by default.  
			/// </summary>
			Normal,
			/// <summary>
			/// Scheduled after work items with Normal priority and before those with Lowest priority.  
			/// </summary>
			BelowNormal,
			/// <summary>
			/// Scheduled after work items with any other priority.  
			/// </summary>
			Lowest
		}
		#endregion

        #region Member Data
		/// <summary>
		/// 
		/// </summary>
		private bool disposed = false;
		/// <summary>
		/// Priority queues. There is one queue for each type of priority.
		/// </summary>
		private Hashtable m_Queues;
		/// <summary>
		/// The total number of work items within the queues 
		/// </summary>
		private int m_WorkItemsCount;
        #endregion

        #region Accessors
		/// <summary>
		/// Get total number of work items queued. 
		/// </summary>
		public virtual int Count
		{
			get
			{
				int count = 0;
				lock (m_Queues.SyncRoot)
				{
					count = m_WorkItemsCount;
				}
				return count;
			}
		}
        #endregion

        #region Constructors and Destructor
		/// <summary>
		/// Initializes a new instance of the PriorityQueue class.
		/// </summary>
		public PriorityQueue()
		{
			m_WorkItemsCount = 0;
			m_Queues = new Hashtable();

			foreach ( PriorityQueue.Priority v in Enum.GetValues(typeof(PriorityQueue.Priority)) )
				m_Queues.Add(v, new Queue() );
		}
        #endregion

        #region Methods
		/// <summary>
		/// Explicitly releases all resources used by this object.
		/// </summary>
		public virtual void Dispose()
		{
			if ( !disposed )
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
		/// Explicitly releases all resources used by this object.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			lock(this)
			{
				if (disposing)
				{
             
				}

				disposed = true;
			}
		}

		/// <summary>
		/// Sync object
		/// </summary>
		public object SyncRoot
		{
			get { return m_Queues.SyncRoot; }
		}

		/// <summary>
		/// Adds an work item to the end of the queue.
		/// </summary>
		/// <param name="workItem">The work item to add to the queue.</param>
		public virtual void Enqueue(WorkItem workItem)
		{
			if ( workItem == null )
				throw new ArgumentNullException("workItem");

			lock (m_Queues.SyncRoot)
			{
				(m_Queues[workItem.Priority] as Queue).Enqueue(workItem);
				m_WorkItemsCount++;
			}
		}
        /// <summary>
        /// Returns the work item at the beginning of the queue without removing it.
        /// </summary>
        /// <returns>The work item at the beginning of the queue or null.</returns>
        public virtual WorkItem Peek()
        {
            WorkItem workItem = null;

            lock (m_Queues.SyncRoot)
            {
                // check queues highest to lowest.
                Queue queue;

                foreach (PriorityQueue.Priority v in Enum.GetValues(typeof(PriorityQueue.Priority)))
                {
                    queue = m_Queues[v] as Queue;
                    if (queue.Count > 0)
                    {
                        workItem = queue.Peek() as WorkItem;
                        break;
                    }
                }
            }

            return workItem;
        }
		/// <summary>
		/// Removes and returns the work item at the beginning of the queue.
		/// </summary>
		/// <returns>The work item that is removed from the beginning of the queue or null.</returns>
		public virtual WorkItem Dequeue()
		{
			WorkItem workItem = null;

			lock (m_Queues.SyncRoot)
			{
				// check queues highest to lowest.
				Queue queue;
			
				foreach ( PriorityQueue.Priority v in Enum.GetValues(typeof(PriorityQueue.Priority)) )
				{
					queue = m_Queues[v] as Queue;
					if ( queue.Count > 0 )
					{
						workItem = queue.Dequeue() as WorkItem;
						m_WorkItemsCount--;
						break;
					}
				}
			}

			return workItem;
		}
		/// <summary>
		/// Removes all <see cref="WorkItem"/> work items from the queue.
		/// </summary>
		public virtual void Clear()
		{
			lock (m_Queues.SyncRoot)
			{
				Queue queue;
				foreach ( PriorityQueue.Priority v in Enum.GetValues(typeof(PriorityQueue.Priority)) )
				{
					queue = m_Queues[v] as Queue;
					queue.Clear();
				}
                m_WorkItemsCount = 0;
			}
		}

		/// <summary>
		/// Gets an enumerator for the queue object having the given priority filter.
		/// </summary>
		/// <param name="priority">Priority filter.</param>
		/// <returns>Enumerator interface on Queue object having given priority.</returns>
		public IEnumerator GetEnumerator(PriorityQueue.Priority priority)
		{
			Queue filteredQueue = m_Queues[priority] as Queue;
			return filteredQueue.GetEnumerator();
		}
        #endregion
	}
}
