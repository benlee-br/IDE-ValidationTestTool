using System;
using System.Collections.Specialized;
using System.Threading;
using System.Diagnostics;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Allows one to execute priority sorted, queued, asynchronous, computation that
	/// is guaranteed to be sequential, and sychronous within the asynchronous thread context.
	/// Therefore, subsequent computations cannot interfere with eachother.
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
	///			<item name="vssfile">$Workfile: ExecutablePriorityQueue.cs $</item>
	///			<item name="vssfilepath">$Archive: /CFX_15/Source/Core/Common/ExecutableQueue/ExecutablePriorityQueue.cs $</item>
	///			<item name="vssrevision">$Revision: 10 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Blie $</item>
	///			<item name="vssdate">$Date: 09/01/20 16:59 $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class ExecutablePriorityQueue : ResourceManagedPriorityQueue
	{
		#region Internal Classes
		/// <summary>
		/// Shared reference to a thread-safe abort flags.
		/// </summary>
		public class CommonAbortFlags
		{
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
						m_Aborting = value;
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
						m_Aborted = value;
					}
				}
			}
			#endregion
		}
		#endregion

		#region Constants
		#endregion

		#region Member Data
		/// <summary>
		/// Shared object used for aborting a running, asynchronous thread.
		/// </summary>
		private CommonAbortFlags m_CommonAbortFlag;

		/// <summary>
		/// Asynchronous delegate to process queue asynchronously.
		/// </summary>
		private AbortableAsyncRunDelegate m_ThreadPoolDelegate;

		/// <summary>
		/// Lock used to test thread count and start a new thread or block a 
		/// new threadstart if there is already a thread running.
		/// </summary>
		private object m_ThreadCountLock = new object();
		/// <summary>
		/// We can only allow one thread to be running and processing the queue at once,
		/// so we keep a count. If anyone tries to start a second one, it simply won't allow this.
		/// </summary>
		private int m_ProcessQueueThreadCount = 0;

		/// <summary>
		/// If a computation hangs, then we must have a protected means to bail out.
		/// Set default o 100 milliseconds.
		/// </summary>
		private int m_WatchDogTimeOut = -1;
		/// <summary>
		/// Sycnhronization locking object.
		/// </summary>
		private object m_SynchLock = new object();
		/*
				 /// <summary>
				 /// When operating in queued scenario mode, if the user knows how many  
				 /// readers will be reading the data, then one can allow the next queued scenario change to block until
				 /// this reference count becomes zero. The readers can decrement this reference count.
				 /// Then the wait releases allowing the next round of readers to execute. This guarantees 
				 /// that the reader has completed accessing the data before the flags can be reset by a 
				 /// scenario change under them. If someone waits too long, then this tiems out and allows the next scenario
				 /// change to occur.
				 /// </summary>
				 private int m_MultipleReaderReferenceCount = 0;

				 /// <summary>
				 /// 
				 /// </summary>
				 private AutoResetEvent m_MultipleReaderReferenceCountBlock = new AutoResetEvent();
		*/
		#endregion

		#region Accessors
		/// <summary>
		/// Timeout for queued computations.
		/// </summary>
		public int WatchDogTimeOut
		{
			get { return m_WatchDogTimeOut; }
			set { m_WatchDogTimeOut = value; }
		}
		/// <summary>
		/// Get reference to thread pool delegate.
		/// </summary>
		public AbortableAsyncRunDelegate AsynchProcessQueueDelegate
		{
			get
			{
				lock (m_SynchLock)
				{
					return m_ThreadPoolDelegate;
				}
			}
		}

		/// <summary>
		/// Gets reference to CommonAbortFlag object.
		/// </summary>
		public CommonAbortFlags AbortFlagObject
		{
			get
			{
				lock (m_SynchLock)
				{
					return m_CommonAbortFlag;
				}
			}
		}

		/// <summary>
		/// Returns true if the ProcessQueue is running.
		/// </summary>
		public bool ProcessingQueue
		{
			get
			{
				lock (m_ThreadCountLock)
				{
					return m_ProcessQueueThreadCount == 1;
				}
			}
			set
			{
				lock (m_ThreadCountLock)
				{
					if (value == true)
						m_ProcessQueueThreadCount = 1;
					else
						m_ProcessQueueThreadCount = 0;
				}
			}
		}

		/// <summary>
		/// Delegate to process queue asynchronously on separate thread.
		/// </summary>
		public AbortableAsyncRunDelegate ThreadPoolDelegate
		{
			get
			{
				lock (m_SynchLock)
				{
					return m_ThreadPoolDelegate;
				}
			}
			set
			{
				lock (m_SynchLock)
				{
					m_ThreadPoolDelegate = value;
				}
			}
		}
		#endregion

		#region Delegates and Events
		/// <summary>
		/// Delegate for asynchronous calls.
		/// </summary>
		public delegate void AsynchRunDelegate();

		/// <summary>
		/// Delegate for asynchronous thread calls.
		/// </summary>
		/// <param name="abortFlagObject">Shared abort flag object.</param>
		public delegate void AbortableAsyncRunDelegate(CommonAbortFlags abortFlagObject);
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Constructor.
		/// </summary>
		public ExecutablePriorityQueue()
		{
			m_CommonAbortFlag = new CommonAbortFlags();
		}
		#endregion

		#region Methods

		/// <summary>
		/// Sets abort flag value.
		/// </summary>
		/// <param name="abortingFlagValue">Aborting flag value.</param>
		public void SetAbortingFlag(bool abortingFlagValue)
		{
			m_CommonAbortFlag.AbortingFlag = abortingFlagValue;
		}

		/// <summary>
		/// Sets abort flag value.
		/// </summary>
		/// <param name="abortedFlagValue">Aborted flag value.</param>
		public void SetAbortedFlag(bool abortedFlagValue)
		{
			m_CommonAbortFlag.AbortedFlag = abortedFlagValue;
		}

		/// <summary>
		/// Reset abort flags to false.
		/// </summary>
		public void ResetAbortFlags()
		{
			lock (this)
			{
				m_CommonAbortFlag.AbortingFlag = false;
				m_CommonAbortFlag.AbortedFlag = false;
			}
		}
		/// <summary>
		/// Default ProcessQueue method.
		/// </summary>
		private void ProcessQueue()
		{
			CommonAbortFlags abortFlag = new CommonAbortFlags();
			abortFlag.AbortedFlag = false;
			abortFlag.AbortingFlag = false;
			ProcessQueue(abortFlag);
		}

		/// <summary>
		/// Execute a delegate synchronously, but only after checking for memory
		/// and threadpool resources. Since we might not be able to get a thread from the thread pool,
		/// we might need to grow the thread pool before proceeding.
		/// At this point, calling BeginInvoke will try to grab a worker thread from the threadpool
		/// as well as memory for executing the computation. We will handle various exceptions by
		/// waiting for resources to be available, and then try again.
		/// </summary>
		/// <param name="asynchRunDelegate">Delegate to be executed.</param>
		/// <returns>If the threadpool can be grown and/or waiting will allow enough memory to be freed
		/// up, the this returns true, else false.</returns>
		private bool SynchronousResourceManagedExecute(AsynchRunDelegate asynchRunDelegate)
		{
			//bool bSuccess = false;
			//if (this.ResourceManagerObject.CheckResources(this.NotEnoughMemoryWaitTime) == false)
			//   return false;
			//bSuccess = true;

			// If we get here, we have threads and we have memory.
			//IAsyncResult ar = asynchRunDelegate.BeginInvoke(null, null);
			//ar.AsyncWaitHandle.WaitOne(m_WatchDogTimeOut, false);
			//if (ar.IsCompleted)
			//{
			//   bSuccess = true;
			//}
			//else
			//{
			//   bSuccess = false;
			//}
			//// Clean up resources.
			//if (ar != null)
			//   asynchRunDelegate.EndInvoke(ar);

			//return bSuccess;

			asynchRunDelegate.Invoke();
			return true;
		}

		/// <summary>
		/// Abortable ProcessQueue method.
		/// </summary>
		private void ProcessQueue(CommonAbortFlags abortFlag)
		{
			//bool done = true;
			try
			{
				if (abortFlag.AbortingFlag == true || abortFlag.AbortedFlag == true)
					return;
				do
				{
					// Language CurrentCulture setting
                    Thread.CurrentThread.CurrentUICulture = ApplicationStateData.GetInstance.AppCurrentUICulture;


					WorkItem workItem = null;
					ExecutableObject executableObject = null;
					ResourceManagedPriorityQueue resourceManagedPriorityQueue = this as ResourceManagedPriorityQueue;
					if (resourceManagedPriorityQueue.Count > 0)
					{
						//                            workItem = priorityQueue.Dequeue() as WorkItem;
						object dequeuedItem = resourceManagedPriorityQueue.ResourceManagedDequeue();
						Debug.Assert(dequeuedItem is WorkItem);
						workItem = dequeuedItem as WorkItem;
						if (workItem == null)
							throw new ApplicationException("Null workItem error.");
						executableObject = workItem.Item as ExecutableObject;
					}

					if (executableObject != null)
					{
						AsynchRunDelegate asynchRunDelegate = new AsynchRunDelegate(executableObject.Execute);
						workItem.StartedTime = DateTime.Now;
						bool bSuccess = SynchronousResourceManagedExecute(asynchRunDelegate);
						workItem.CompletedTime = DateTime.Now;
						TimeSpan time = workItem.ProcessingTime;
						if (bSuccess == false)
							throw new ApplicationException("Insufficienct memory or thread resources.");
					}

					// Stop processing queue if aborting. Don't clear anything in queue, since they
					// may be valid. We do this if we are aborting, we have new entries while shutting down,
					// and we must reset the dirty flags before we process the new queued events.
					if (abortFlag.AbortingFlag == true)
					{
						lock (this.m_ThreadCountLock)
						{
							this.m_ProcessQueueThreadCount = 0;
						}
						// Now we've finished aborting.
						//abortFlag.AbortingFlag = false;
						abortFlag.AbortedFlag = true;
						return;
					}
					// more messages to process?
					//lock (resourceManagedPriorityQueue.SyncRoot)
					//{
					//   done = (resourceManagedPriorityQueue.Count == 0);
					//   lock (this.m_ThreadCountLock)
					//   {
					//      this.m_ProcessQueueThreadCount = 0;
					//   }

					//}
					if (resourceManagedPriorityQueue.Count == 0)
						Thread.Sleep(100);
				} while (true);
			}
			catch (Exception ex)
			{
				//todo: log error
				Console.WriteLine(ex.Message);
				Debug.Assert(false); // added 1.4.2007 by TLH.
			}
			finally
			{
			}
		}

		/// <summary>
		/// Post an executable object to the priority queue asynchronously.
		/// Returns without waiting for the message to be processed. 
		/// </summary>
		/// <remarks>
		/// You must instantiate a new message each time you wish to send a message. 
		/// The SendMessageAsync method returns immediately before a message is 
		/// processed. SendMessageAsync method will return false if it detects a 
		/// reference to a object in the queue that is the same as the argument object.
		/// </remarks>
		/// <param name="executableObject">Object to be queued.</param>
		/// <param name="priority">Priority of execution.</param>
		/// <returns>True if message will be processed.</returns>
		public bool PostAsyncSequentialExecutionToQueue(ExecutableObject executableObject, Priority priority)
		{
			bool status = false;
			PriorityQueue priorityQueue = this as PriorityQueue;

			if (executableObject == null)
			{
				System.Diagnostics.Debug.Assert(false, "null argument");//todo: localization
				return status;
			}

			//            lock (priorityQueue.SyncRoot) 
			//			{
			// We completely lock out the queue while aborting. This is not really necessary,
			// but is much safer than allowing more workitens to queue while aborting.
			//                if (m_CommonAbortFlag.AbortedFlag == true || m_CommonAbortFlag.AbortingFlag == true)
			//                    return false;
			priorityQueue.Enqueue(new WorkItem(executableObject, priority));

			status = true;
			//           }

			//TODO add aborting capability to processing the queue.
			AsynchRunQueue(this.m_CommonAbortFlag);

			return status;
		}

		/// <summary>
		/// Process the queue.
		/// </summary>
		public void AsynchRunQueue(CommonAbortFlags commonAbortFlag)
		{
			PriorityQueue priorityQueue = this as PriorityQueue;

			//            lock (priorityQueue.SyncRoot)
			//            {
			// If the message count is one then start the message processing thread, 
			// but only if it is not already currently being processed.
			if (commonAbortFlag.AbortingFlag == false && priorityQueue.Count >= 1 && !ProcessingQueue/*m_ProcessQueueThreadCount == 0*/)
			{

				lock (this.m_ThreadCountLock)
				{
					m_ThreadPoolDelegate = new AbortableAsyncRunDelegate(ProcessQueue);
					AsyncCallback cb = new AsyncCallback(CheckAbortedResumeProcessingQueue);
					m_ThreadPoolDelegate.BeginInvoke(m_CommonAbortFlag, cb, m_CommonAbortFlag); // Runs on a worker thread from the pool.
					this.m_ProcessQueueThreadCount = 1;
				}
			}
			//            }
		}

		/// <summary>
		/// Callback stub.
		/// </summary>
		/// <param name="ar">AsyncResult object.</param>
		public virtual void CheckAbortedResumeProcessingQueue(IAsyncResult ar) { }
		#endregion

		#region Event Handlers
		#endregion
	}
}
