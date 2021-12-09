using System;
using System.Text;
using System.Runtime.InteropServices;

namespace BioRad.Win32.Shell
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
    ///			<item name="vssfile">$Workfile: File.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Win32/Shell/File.cs $</item>
    ///			<item name="vssrevision">$Revision: 1 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
    ///			<item name="vssdate">$Date: 7/26/06 2:49p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    public partial class File
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

        /// <summary>Initializes a new instance of the File class.</summary>
        public File() { }

        #endregion

        #region Methods
        #endregion

        #region Event Handlers
        #endregion

        #region DLLImport

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hInstance"></param>
        /// <param name="strFileName"></param>
        /// <param name="uiIconIndex"></param>
        /// <returns></returns>
        [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr ExtractIcon(IntPtr hInstance,
            string strFileName, uint uiIconIndex);

        #endregion
    }
}
