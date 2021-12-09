using System;
using System.Runtime.InteropServices;

namespace BioRad.Win32.Kernel
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
	///			<item name="vssfile">$Workfile: PowerManagement.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Win32/Kernel/PowerManagement.cs $</item>
	///			<item name="vssrevision">$Revision: 5 $</item>
	///			<item name="vssauthor">Last Check-in by: $Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public partial class PowerManagement
	{
		#region Constants

		/// <summary>
		/// Thread's execution requirements.
		/// </summary>
        public enum Flags : uint
		{
			/// <summary>
			/// Informs the system that the state being set should remain in effect until the next call that uses ES_CONTINUOUS and one of the other state flags is cleared.
			/// </summary>
			Continuous=0x80000000,
			/// <summary>
			/// Forces the display to be on by resetting the display idle timer.
			/// </summary>
			DisplayReq=0x00000002,
			/// <summary>
			/// Forces the system to be in the working state by resetting the system idle timer.
			/// </summary>
			SystemReq=0x00000001
		}

        #endregion

        #region Methods

		/// <summary>
		/// The SetThreadExecutionState function enables applications to inform the system that it is in use, thereby preventing the system from entering the sleeping power state or turning off the display while the application is running.
		/// </summary>
		/// <param name="flags">Thread's execution requirements.</param>
		/// <returns>
		/// If the function succeeds, the return value is the previous thread execution state.
		/// If the function fails, the return value is NULL.
		/// </returns>
		public static Flags SetThreadExecutionState(Flags flags)
		{
			Flags ret=0;

            if (0 == (ret = (Flags)SetThreadExecutionState((uint)flags)))
				BioRad.Win32.Kernel.APIBuffer.ThrowGetLastError();
			//else
				return ret;
		}

        #endregion

		#region Unmanaged API

		[DllImport("kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        private static extern uint SetThreadExecutionState(uint flags);

		#endregion	
	}
}
