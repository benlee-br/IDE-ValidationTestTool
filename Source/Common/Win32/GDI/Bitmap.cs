using System;
using System.Text;
using System.Runtime.InteropServices;

namespace BioRad.Win32.GDI
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
    ///			<item name="vssfile">$Workfile: Bitmap.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Win32/GDI/Bitmap.cs $</item>
    ///			<item name="vssrevision">$Revision: 1 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
    ///			<item name="vssdate">$Date: 7/26/06 2:51p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    public partial class Bitmap
    {
        #region Constants

        /// <summary>
        /// ScreenCopy Enum to match win32 definition
        /// </summary>
        public const int SRCCOPY = 13369376;
        
        #endregion

        #region Member Data
        #endregion

        #region Accessors
        #endregion

        #region Delegates and Events
        #endregion

        #region Constructors and Destructor

        /// <summary>Initializes a new instance of the Bitmap class.</summary>
        public Bitmap() { }

        #endregion

        #region Methods
        #endregion

        #region Event Handlers
        #endregion

        #region DLLImport

        /// <summary>
        /// Windows GDI32 wrapper for deleteDC 
        /// </summary>
        /// <param name="hDc"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern IntPtr DeleteDC(IntPtr hDc);

        /// <summary>
        /// Windows GDI32 wrapper for DeleteObject
        /// </summary>
        /// <param name="hDc"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern IntPtr DeleteObject(IntPtr hDc);

        /// <summary>
        /// Windows GDI32 wrapper for BitBlt
        /// </summary>
        /// <param name="hdcDest"></param>
        /// <param name="xDest"></param>
        /// <param name="yDest"></param>
        /// <param name="wDest"></param>
        /// <param name="hDest"></param>
        /// <param name="hdcSource"></param>
        /// <param name="xSrc"></param>
        /// <param name="ySrc"></param>
        /// <param name="RasterOp"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
        public static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int wDest,
            int hDest, IntPtr hdcSource, int xSrc, int ySrc, int RasterOp);

        /// <summary>
        /// Windows GDI32 wrapper for CreateCompatibleBitmap
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth,
            int nHeight);

        /// <summary>
        /// Windows GDI32 wrapper for CreateCompatibleDC
        /// </summary>
        /// <param name="hdc"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        /// <summary>
        /// Windows GDI32 wrapper for SelectObject
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="bmp"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);
    
        #endregion
    }
}
