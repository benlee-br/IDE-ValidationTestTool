using System;
using System.Diagnostics;
using BioRad.Common.DiagnosticsLogger;

namespace BioRad.Common.Utilities
{
	#region Documentation Tags
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// 
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href=""></see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile:  $</item>
	///			<item name="vssfilepath">$Archive: $</item>
	///			<item name="vssrevision">$Revision: $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: $</item>
	///			<item name="vssdate">$Date: $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public static class Util
	{
		#region Methods
		/// <summary>Gets the process that corresponds to the passed in id.</summary>
		/// <param name="id">Unique identifier for the associated process.</param>
		/// <param name="processName">The friendly name of the process.</param>
		/// <returns>Process object if process is running, else null.</returns>
		public static Process GetProcessNoMemLeak(int id, string processName)
		{
			// WI 190 - changed the way this method works based on the recommendation in 
			// http://msdn.microsoft.com/en-us/library/z3w4xdc9(v=VS.90).aspx
			// using GetProcessesByName causes a memory leak because objects are created for all
			// the processes but since we cannot get a reference to them we cannot call the dispose on them
			// so instead GetProcesses is used and the one needed is returned and the rest are disposed
			// a sample app did not show any time performance difference between the 2 methods
			// but the change will need to be throughly tested because it is a change in the way 
			// we get the process object
			Process process = null;
			if (id > 0)
			{
				try
				{
					// make sure the id is the server process name.
					Process[] processes = Process.GetProcesses();
					foreach (Process p in processes)
					{
						try
						{
							if (p.Id == id)
							{
								if (!p.HasExited)
									process = p;
								// we cannot break at this point because we need to 
								// dispose all the process objects
							}
						}
						finally
						{
							p.Close();
							p.Dispose();
						}
					}
				}
				catch (Exception ex)
				{
					DiagnosticsLogService.GetService.GetDiagnosticsLog("ProcessUtility").Exception(ex);
					process = null;
				}
			}
			return process;
		}
		#endregion
	}
}
