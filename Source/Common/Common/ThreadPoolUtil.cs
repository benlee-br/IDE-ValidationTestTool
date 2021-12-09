using System;
using System.Collections;
using System.Threading;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// </summary>
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
	///			<item name="vssfile">$Workfile: ThreadPoolUtil.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/Common/ThreadPoolUtil.cs $</item>
	///			<item name="vssrevision">$Revision: 2 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 5/08/06 12:29p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public sealed class ThreadPoolUtil
	{
		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="num"></param>
		public static void SetNumberWorkerThreads(int num)
		{
			BioRad.Common.ThreadManager threadManager =
				new BioRad.Common.ThreadManager();

			// QueryInterface for the ICorThreadPool interface:
			BioRad.Common.ICorThreadpool  ct = 
				(BioRad.Common.ICorThreadpool)threadManager;

			uint maxWorkerThreads = 0;
			uint maxIOThreads = 0;
			ct.GetMaxThreads(out maxWorkerThreads, out maxIOThreads);
			ct.SetMaxThreads((uint)num, maxIOThreads);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="num"></param>
		public static void IncreaseWorkerThreads(int num)
		{
			SetNumberWorkerThreads(GetMaxWorkerThreads+num);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
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
		/// 
		/// </summary>
		/// <returns></returns>
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
		/// 
		/// </summary>
		/// <returns></returns>
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
