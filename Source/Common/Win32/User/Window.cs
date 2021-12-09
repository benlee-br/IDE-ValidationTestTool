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
    ///			<item name="vssfile">$Workfile: Window.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Win32/User/Window.cs $</item>
    ///			<item name="vssrevision">$Revision: 2 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Pwalse $</item>
    ///			<item name="vssdate">$Date: 5/04/07 4:19p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    public partial class Window
    {
        #region Constants

		/// <summary>
		/// SM_CXSCREEN Enum to match win32 definition
		/// </summary>
		public  const int SM_CXSCREEN=0;
		/// <summary>
		/// SM_CYSCREEN Enum to match win32 definition
		/// </summary>
		public  const int SM_CYSCREEN=1;

		#endregion		
		
        #region Constructors and Destructor
		#endregion

        #region Constants
        #endregion

        #region Member Data
        #endregion

        #region Accessors
        #endregion

        #region Delegates and Events
        #endregion

        #region Methods
        #endregion

        #region Event Handlers
        #endregion
 
        #region ImportDll EntryPoints

		/// <summary>
		/// Windows GDI32 wrapper for GetDesktopWindow
		/// </summary>
		/// <returns></returns>
		[DllImport("user32.dll", EntryPoint="GetDesktopWindow")]
		public static extern IntPtr GetDesktopWindow();

		/// <summary>
		/// Windows GDI32 wrapper for GetActiveWindow
		/// </summary>
		/// <returns></returns>
		[DllImport("user32.dll", EntryPoint="GetActiveWindow")]
		public static extern IntPtr GetActiveWindow();

		/// <summary>
		/// Windows GDI32 wrapper for GetSystemMetrics
		/// </summary>
		/// <param name="ptr"></param>
		/// <returns></returns>
		[DllImport("user32.dll",EntryPoint="GetDC")]
		public static extern IntPtr GetDC(IntPtr ptr);

		/// <summary>
		/// Windows GDI32 wrapper for GetSystemMetrics
		/// </summary>
		/// <param name="abc"></param>
		/// <returns></returns>
		[DllImport("user32.dll",EntryPoint="GetSystemMetrics")]
		public static extern int GetSystemMetrics(int abc);

		/// <summary>
		/// Windows GDI32 wrapper for 
		/// </summary>
		/// <param name="ptr"></param>
		/// <returns></returns>
		[DllImport("user32.dll",EntryPoint="GetWindowDC")]
		public static extern IntPtr GetWindowDC(Int32 ptr);

		/// <summary>
		/// Windows GDI32 wrapper for ReleaseDC
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="hDc"></param>
		/// <returns></returns>
		[DllImport("user32.dll",EntryPoint="ReleaseDC")]
		public static extern IntPtr ReleaseDC(IntPtr hWnd,IntPtr hDc);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hWnd"></param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="nCmdShow"></param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hWnd"></param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern bool IsIconic(IntPtr hWnd);
		#endregion
    }
}
