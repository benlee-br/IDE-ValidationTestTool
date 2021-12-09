using System;
using System.Threading;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Class Summary
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
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
	///			<item name="vssfile">$Workfile: ThreadUtils.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ExecutableQueue/ThreadUtils.cs $</item>
	///			<item name="vssrevision">$Revision: 2 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public partial class ThreadUtils
	{
        #region Methods
		/// <summary>
		/// Set maximin number of threadpool threads.
		/// </summary>
        /// <param name="maxnumPoolWorkerThreads">Maximum number of thread pool threads.</param>
		/// <param name="maxnumAsynchIOPoolWorkerThreads">The maximum number of asynchronous I/O threads in the thread pool.</param>
        /// <returns>Returns true if it can grow the thread pool, else false.</returns>
		public static bool SetNumberWorkerThreads(int maxnumPoolWorkerThreads, int maxnumAsynchIOPoolWorkerThreads)
		{
            return System.Threading.ThreadPool.SetMaxThreads(maxnumPoolWorkerThreads, maxnumAsynchIOPoolWorkerThreads);
		}
		/// <summary>
		/// Increase thread pool threads.
		/// </summary>
        /// <param name="numWorker">Additional worker threads.</param>
        /// <param name="numAsynchIO">Additional IO worker threads.</param>
        /// <returns>Returns true if it can grow the thread pool, else false.</returns>
		public static bool IncreaseWorkerThreads(int numWorker, int numAsynchIO)
		{
			int i = GetMaxWorkerThreads;
            return SetNumberWorkerThreads(i + numWorker,i + numAsynchIO);
		}
		/// <summary>
		/// Get maximum number of thread pool threads.
		/// </summary>
		/// <returns>Gets maximum number of thread pool threads.</returns>
		public static int GetMaxWorkerThreads
		{
			get
			{
				int count = 0;
				int completionPortThreads = 0;
				ThreadPool.GetMaxThreads(out count, out completionPortThreads);
				return count;
			}
		}
		/// <summary>
		/// Get minimum number of thread pool threads.
		/// </summary>
		/// <returns>Gets minimum number of thread pool threads.</returns>
		public static int GetMinWorkerThreads
		{
			get
			{
				int count = 0;
				int completionPortThreads = 0;
				ThreadPool.GetMinThreads(out count, out completionPortThreads);
				return count;
			}
		}
		/// <summary>
		/// Currently available thread count.
		/// </summary>
		/// <returns>Currently available thread count.</returns>
		public static int GetAvailableThreads
		{
			get
			{
				int count = 0;
				int completionPortThreads = 0;
				ThreadPool.GetAvailableThreads(out count, out completionPortThreads);
				return count;
			}
		}
        #endregion
	}
}
