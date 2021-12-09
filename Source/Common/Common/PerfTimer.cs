using System;
using System.Collections;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Performance timing.
	/// </summary>
	/// <remarks>
	/// Uses the Win32 high-resolution performance counter.
	/// <example>
	/// How to use: 
	/// <code>
	/// #if DEBUG
	///		PerfTimer timer = new PerfTimer();
	///		timer.Start();
	/// #endif 
	///
	///	// ...your code here...
	///	
	/// #if DEBUG
	///		TimeType t = timer.Stop();
	///		Trace.Write("Elaspe Time: ");
	///		Trace.WriteLine(t.GetAs(TimeType.Units.MilliSeconds).ToString("########.00"));
	///		timer = null;
	/// #endif 
	///	</code>
	/// </example>	
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
	///			<item name="vssfile">$Workfile: PerfTimer.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/PerfTimer.cs $</item>
	///			<item name="vssrevision">$Revision: 9 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 8/29/06 9:22a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class PerfTimer
	{
		#region Nested Classes
		internal class PerfTimerData
		{
			#region Member Data
			/// <summary>
			/// 
			/// </summary>
			public long start = 0;
			/// <summary>
			/// 
			/// </summary>
			public long end = 0;
			/// <summary>
			/// 
			/// </summary>
			public long freq = 0;
			/// <summary>
			/// 
			/// </summary>
			public double elaspeTime = 0;
			#endregion

			#region Constructors and Destructor	
			/// <summary>
			/// 
			/// </summary>
			public PerfTimerData()
			{
			}	
			#endregion
		}
		#endregion

        #region Member Data
		private Stack m_Stack = null;
        #endregion

		#region Accessors
		/// <summary>
		/// 
		/// </summary>
		public int Count
		{
			get{return m_Stack.Count;}
		}
		#endregion

        #region Methods
		/// <summary>
		/// Call just before your timing loop. 
		/// </summary>
		public bool Start()
		{
			if ( m_Stack == null )
				m_Stack = new Stack();

			PerfTimerData p = new PerfTimerData();

			bool b = BioRad.Win32.Kernel.Process.QueryPerformanceCounter(ref p.start);
			if ( !b )
				throw new ApplicationException("QueryPerformanceCounter failed.");

			m_Stack.Push( p );

			return b;
		}
		/// <summary>
		/// Call just after your timing loop and after calling Start.
		/// </summary>
		public TimeType Stop()
		{
			if ( m_Stack.Count == 0 )
				throw new ApplicationException("Start/Stop sequence mismatch.");

			PerfTimerData p = (PerfTimerData)m_Stack.Pop();

			bool b = BioRad.Win32.Kernel.Process.QueryPerformanceCounter(ref p.end);
			if ( !b )
				throw new ApplicationException("QueryPerformanceCounter failed.");

			b = BioRad.Win32.Kernel.Process.QueryPerformanceFrequency(ref p.freq);
			if ( !b )
				throw new ApplicationException("Installed hardware does not supports a high-resolution performance counter.");

			return new TimeType(( p.end - p.start) * 1000.0 / p.freq, TimeType.Units.MilliSeconds);
		}
        /// <summary>
        /// Call just after your timing loop and after calling Start.
        /// </summary>
        public double Stop(bool usedForOverloadingMethodSignature)
        {
            if (m_Stack.Count == 0)
                throw new ApplicationException("Start/Stop sequence mismatch.");

            PerfTimerData p = (PerfTimerData)m_Stack.Pop();

            bool b = BioRad.Win32.Kernel.Process.QueryPerformanceCounter(ref p.end);
            if (!b)
                throw new ApplicationException("QueryPerformanceCounter failed.");

            b = BioRad.Win32.Kernel.Process.QueryPerformanceFrequency(ref p.freq);
            if (!b)
                throw new ApplicationException("Installed hardware does not supports a high-resolution performance counter.");

            return (p.end - p.start) * 1000.0 / p.freq;
        }
        #endregion
	}
}
