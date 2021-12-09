using System;
using System.Text;
using System.Drawing;

using BioRad.Win32.User;

namespace BioRad.Win32.Kernel
{
    #region Documentation Tags
    /// <summary>
	/// 
    /// </summary>
    /// <remarks>
	/// //TODO: PW Review: Process is part of .Net framework, do not use such names.
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
    ///			<item name="vssfile">$Workfile: Process.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Win32/Kernel/Process.cs $</item>
    ///			<item name="vssrevision">$Revision: 2 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Pwalse $</item>
    ///			<item name="vssdate">$Date: 9/14/06 1:42p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    public partial class Process
    {
        #region Constants
        #endregion

        #region Member Data
        #endregion

        #region Accessors
        #endregion

        #region Delegates and Events
        #endregion

        #region Constructors and Destructor

        /// <summary>Initializes a new instance of the Process class.</summary>
        public Process() { }

        #endregion

        #region Methods

        /// <summary>
        /// generate desktop screen snap shot imgae
        /// </summary>
        /// <returns></returns>
        public static System.Drawing.Bitmap GetDesktopImage()
        {
            //In size variable we shall keep the size of the screen.
            SIZE size;

            //Variable to keep the handle to bitmap.
            IntPtr hBitmap;

            //Here we get the handle to the desktop device context.
            IntPtr hDC = Window.GetDC(Window.GetDesktopWindow());

            //Here we make a compatible device context in memory for screen device context.
            IntPtr hMemDC = BioRad.Win32.GDI.Bitmap.CreateCompatibleDC(hDC);

            //We pass SM_CXSCREEN constant to GetSystemMetrics to get the X coordinates of screen.
            size.cx = Window.GetSystemMetrics(Window.SM_CXSCREEN);

            //We pass SM_CYSCREEN constant to GetSystemMetrics to get the Y coordinates of screen.
            size.cy = Window.GetSystemMetrics(Window.SM_CYSCREEN);

            //We create a compatible bitmap of screen size using screen device context.
            hBitmap = BioRad.Win32.GDI.Bitmap.CreateCompatibleBitmap(hDC, size.cx, size.cy);
            //int a = BioRad.Win32.GDI.Bitmap.IntPtr hdc,();
            //As hBitmap is IntPtr we can not check it against null. For this purspose IntPtr.Zero is used.
            if (hBitmap != IntPtr.Zero)
            {
                //Here we select the compatible bitmap in memeory device context and keeps the refrence to Old bitmap.
                IntPtr hOld = (IntPtr)BioRad.Win32.GDI.Bitmap.SelectObject(hMemDC, hBitmap);
                //We copy the Bitmap to the memory device context.
                BioRad.Win32.GDI.Bitmap.BitBlt(hMemDC, 0, 0, size.cx, size.cy, hDC, 0, 0, BioRad.Win32.GDI.Bitmap.SRCCOPY);
                //We select the old bitmap back to the memory device context.
                BioRad.Win32.GDI.Bitmap.SelectObject(hMemDC, hOld);
                //We delete the memory device context.
                BioRad.Win32.GDI.Bitmap.DeleteDC(hMemDC);
                //We release the screen device context.
                Window.ReleaseDC(Window.GetDesktopWindow(), hDC);
                //Image is created by Image bitmap handle and stored in local variable.
                System.Drawing.Bitmap bmp = System.Drawing.Image.FromHbitmap(hBitmap);
                //Release the memory to avoid memory leaks.
                BioRad.Win32.GDI.Bitmap.DeleteObject(hBitmap);
                //This statement runs the garbage collector manually.
                GC.Collect();
                //Return the bitmap 
                return bmp;
            }

            //If hBitmap is null retunrn null.
            return null;
        }
        /// <summary>
        /// generate active window screen snap shot imgae
        /// </summary>
        /// <returns></returns>
        public static System.Drawing.Bitmap GetWindowImage(IntPtr handle, int width, int height)
        {
            //In size variable we shall keep the size of the screen.
            SIZE size;

            //Variable to keep the handle to bitmap.
            IntPtr hBitmap;

            //Here we get the handle to the desktop device context.
            IntPtr hDC = Window.GetDC(handle);

            //Here we make a compatible device context in memory for screen device context.
            IntPtr hMemDC = BioRad.Win32.GDI.Bitmap.CreateCompatibleDC(hDC);

            //We pass SM_CXSCREEN constant to GetSystemMetrics to get the X coordinates of screen.
            size.cx = width;

            //We pass SM_CYSCREEN constant to GetSystemMetrics to get the Y coordinates of screen.
            size.cy = height;

            //We create a compatible bitmap of screen size using screen device context.
            hBitmap = BioRad.Win32.GDI.Bitmap.CreateCompatibleBitmap(hDC, size.cx, size.cy);
            //int a = BioRad.Win32.GDI.Bitmap.IntPtr hdc,();
            //As hBitmap is IntPtr we can not check it against null. For this purspose IntPtr.Zero is used.
            if (hBitmap != IntPtr.Zero)
            {
                //Here we select the compatible bitmap in memeory device context and keeps the refrence to Old bitmap.
                IntPtr hOld = (IntPtr)BioRad.Win32.GDI.Bitmap.SelectObject(hMemDC, hBitmap);
                //We copy the Bitmap to the memory device context.
                BioRad.Win32.GDI.Bitmap.BitBlt(hMemDC, 0, 0, size.cx, size.cy, hDC, 0, 0, BioRad.Win32.GDI.Bitmap.SRCCOPY);
                //We select the old bitmap back to the memory device context.
                BioRad.Win32.GDI.Bitmap.SelectObject(hMemDC, hOld);
                //We delete the memory device context.
                BioRad.Win32.GDI.Bitmap.DeleteDC(hMemDC);
                //We release the screen device context.
                Window.ReleaseDC(handle, hDC);
                //Image is created by Image bitmap handle and stored in local variable.
                System.Drawing.Bitmap bmp = System.Drawing.Image.FromHbitmap(hBitmap);
                //Release the memory to avoid memory leaks.
                BioRad.Win32.GDI.Bitmap.DeleteObject(hBitmap);
                //This statement runs the garbage collector manually.
                GC.Collect();
                //Return the bitmap 
                return bmp;
            }

            //If hBitmap is null retunrn null.
            return null;
        }
        /// <summary>
        /// generate active window screen snap shot imgae
        /// </summary>
        /// <returns></returns>
        public static System.Drawing.Bitmap GetActiveWindowImage()
        {
            //In size variable we shall keep the size of the screen.
            SIZE size;

            //Variable to keep the handle to bitmap.
            IntPtr hBitmap;

            //Here we get the handle to the desktop device context.
            IntPtr hDC = Window.GetDC(Window.GetActiveWindow());

            //Here we make a compatible device context in memory for screen device context.
            IntPtr hMemDC = BioRad.Win32.GDI.Bitmap.CreateCompatibleDC(hDC);

            //We pass SM_CXSCREEN constant to GetSystemMetrics to get the X coordinates of screen.
            size.cx = Window.GetSystemMetrics(Window.SM_CXSCREEN);

            //We pass SM_CYSCREEN constant to GetSystemMetrics to get the Y coordinates of screen.
            size.cy = Window.GetSystemMetrics(Window.SM_CYSCREEN);

            //We create a compatible bitmap of screen size using screen device context.
            hBitmap = BioRad.Win32.GDI.Bitmap.CreateCompatibleBitmap(hDC, size.cx, size.cy);
            //int a = BioRad.Win32.GDI.Bitmap.IntPtr hdc,();
            //As hBitmap is IntPtr we can not check it against null. For this purspose IntPtr.Zero is used.
            if (hBitmap != IntPtr.Zero)
            {
                //Here we select the compatible bitmap in memeory device context and keeps the refrence to Old bitmap.
                IntPtr hOld = (IntPtr)BioRad.Win32.GDI.Bitmap.SelectObject(hMemDC, hBitmap);
                //We copy the Bitmap to the memory device context.
                BioRad.Win32.GDI.Bitmap.BitBlt(hMemDC, 0, 0, size.cx, size.cy, hDC, 0, 0, BioRad.Win32.GDI.Bitmap.SRCCOPY);
                //We select the old bitmap back to the memory device context.
                BioRad.Win32.GDI.Bitmap.SelectObject(hMemDC, hOld);
                //We delete the memory device context.
                BioRad.Win32.GDI.Bitmap.DeleteDC(hMemDC);
                //We release the screen device context.
                Window.ReleaseDC(Window.GetDesktopWindow(), hDC);
                //Image is created by Image bitmap handle and stored in local variable.
                System.Drawing.Bitmap bmp = System.Drawing.Image.FromHbitmap(hBitmap);
                //Release the memory to avoid memory leaks.
                BioRad.Win32.GDI.Bitmap.DeleteObject(hBitmap);
                //This statement runs the garbage collector manually.
                GC.Collect();
                //Return the bitmap 
                return bmp;
            }

            //If hBitmap is null retunrn null.
            return null;
        }

        #endregion

        #region Event Handlers
        #endregion
 
        #region ImportDll EntryPoints

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern bool GetProcessWorkingSetSize(IntPtr proc, out int min, out int max);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);
        /// <summary>
        /// Win32 high-resolution performance counter.
        /// </summary>
        /// <remarks>
        /// Call QueryPerformanceCounter just before and just after your timing loop, 
        /// subtract counts, multiply by 1.0e9, divide by frequency, 
        /// divide by number of iterations, and that's your approximate time per 
        /// iteration in ns.
        /// </remarks>
        /// <param name="lpPerformanceCount"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern bool QueryPerformanceCounter(ref long lpPerformanceCount);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpFrequency"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern bool QueryPerformanceFrequency(ref long lpFrequency);
        #endregion
    }

    /// <summary>
    /// This structure shall be used to keep the size of the screen.
    /// </summary>
    public struct SIZE
    {
        /// <summary>
        /// Width 
        /// </summary>
        public int cx;
        /// <summary>
        /// Height
        /// </summary>
        public int cy;
    }
}
