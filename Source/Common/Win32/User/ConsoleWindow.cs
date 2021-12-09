using System;
using System.Text;
using System.Runtime.InteropServices;

namespace BioRad.Win32.User
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
	///			<item name="authors">Authors:Ralph Ansell</item>
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
	///			<item name="vssfile">$Workfile: Console.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Win32/User/Console.cs $</item>
	///			<item name="vssrevision">$Revision: 2 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 10/12/06 6:00a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class ConsoleWindow
	{
		#region Constants
		#region ShowWindowSytles
		/// <summary>
		/// States to be used with ShowWindow API call.
		/// </summary>
		public enum ShowWindowStyles : short
		{
			/// <summary>Hides the window and activates another window.</summary>
			SW_HIDE = 0,
			/// <summary>Activates and displays the window.</summary>
			SW_SHOWNORMAL = 1,
			/// <summary>Activates the window and displays it as a minimized window.</summary>
			SW_SHOWMINIMIZED = 2,
			/// <summary>Activates the window and displays it as a maximized window.</summary>
			SW_SHOWMAXIMIZED = 3,
			/// <summary>Maximizes the specified window.</summary>
			SW_MAXIMIZE = 3,
			/// <summary>Displays a window in its most recent size and position. Does not activate window.</summary>
			SW_SHOWNOACTIVATE = 4,
			/// <summary>Activates the window and displays it in its current size and position.</summary>
			SW_SHOW = 5,
			/// <summary>Minimizes the specified window.</summary>
			SW_MINIMIZE = 6,
			/// <summary>Displays the window as a minimized window. Window is not activated.</summary>
			SW_SHOWMINNOACTIVE = 7,
			/// <summary>Displays window in its current size and position, except window is not activated.</summary>
			SW_SHOWNA = 8,
			/// <summary>
			/// Activates and displays the window. IF the window is minimized or maximized, the system
			/// restores it to its original size and position. Use when restoring a minimized window.
			/// </summary>
			SW_RESTORE = 9,
			/// <summary>
			/// Sets the show state based on the SW_ value specified in the STARTUPINFO structure
			/// passed to the CreateProcess function which started the application.
			/// </summary>
			SW_SHOWDEFAULT = 10,
			/// <summary>
			/// Windows 2000/XP: Minimizes a window even if the thread that owns it is hung. Should be used
			/// only when minimizing windows from a different thread.
			/// </summary>
			SW_FORCEMINIMIZE = 11
		}
		#endregion
		#endregion

		#region Member Data
		[DllImport("kernel32")]
		static extern IntPtr GetConsoleWindow();
		[DllImport("user32")]
		static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
		[DllImport("user32")]
		static extern bool IsWindowVisible(IntPtr hwnd);
		#endregion

		#region Accessors
		/// <summary>
		/// Specifies whether the console window should be visible or hidden
		/// </summary>
		public static bool Visible
		{
			get
			{
				IntPtr hwnd = GetConsoleWindow();
				return hwnd != IntPtr.Zero && IsWindowVisible(hwnd);
			}
			set
			{
				IntPtr hwnd = GetConsoleWindow();
				if (hwnd != IntPtr.Zero)
					ShowWindow(hwnd, value ? (int)ShowWindowStyles.SW_SHOW : (int)ShowWindowStyles.SW_HIDE);
			}
		}
		#endregion

		#region Delegates and Events
		#endregion

		#region Constructors and Destructor

		/// <summary>Initializes a new instance of the Console class.</summary>
		public ConsoleWindow() { }

		#endregion

		#region Methods
		#endregion

		#region Event Handlers
		#endregion
	}
}
