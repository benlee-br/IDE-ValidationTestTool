using System;
using System.Text;
using System.IO;

namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>
    /// Utilities for directory operations.
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
    ///			<item name="vssfile">$Workfile: DirectoryUtils.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/DirectoryUtils.cs $</item>
    ///			<item name="vssrevision">$Revision: 4 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
    ///			<item name="vssdate">$Date: 3/13/07 2:27p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    public partial class DirectoryUtils
    {
        #region Methods
        /// <summary>
        /// Creates all directories and subdirectories as specified by path.
        /// </summary>
        /// <param name="path">The directory path to create.</param>
        /// <returns>A System.IO.DirectoryInfo as specified by path or null.</returns>
        public static System.IO.DirectoryInfo CreateDirectory(string path)
        {
            System.IO.DirectoryInfo di = null;
            di = new DirectoryInfo(path);
            if (!di.Exists)
                di = Directory.CreateDirectory(path);
            return di;
        }
        /// <summary>
        /// Delete all files in specified directory.
        /// </summary>
        /// <param name="di">DirectoryInfo object.</param>
        public static void DeleteAllFiles(System.IO.DirectoryInfo di)
        {
            if (!di.Exists)
                return;
            foreach (FileInfo file in di.GetFiles())
                FileUtilities.DeleteFile(file.FullName);
        }
        /// <summary>
        /// Recursively delete directory and all subdirectories.        
        /// </summary>
        /// <param name="di">Full path of the directory to delete.</param>
        public static void DeleteDirectory(System.IO.DirectoryInfo di)
        {
            if (!di.Exists)
                return;

            DeleteAllFiles(di);

            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                DeleteDirectory(new DirectoryInfo(dir.FullName));
            }

            Directory.Delete(di.FullName);

            return;
        }
        /// <summary>
        /// Copy directory.
        /// </summary>
        /// <param name="source">Source directory path.</param>
        /// <param name="destdir">Destination directory path.</param>
        /// <param name="recursive">True to copy sub directories.</param>
        public static void CopyDirectory(
            DirectoryInfo source, string destdir, bool recursive)
        {
            if (source == null)
                throw new ArgumentNullException("srcdir");
            if (destdir == null)
                throw new ArgumentNullException("destdir");

            string tmppath;

            if (!source.Exists)
                throw new ArgumentException("source dir doesn't exist -> " + source.FullName);

            FileSystemInfo[] infoArray = source.GetFileSystemInfos();

            DirectoryInfo destInfo = new DirectoryInfo(destdir);
            if (!destInfo.Exists)
                Directory.CreateDirectory(destdir);

            foreach (FileSystemInfo item in infoArray)
            {
                if (item is DirectoryInfo)
                {
                    DirectoryInfo dirInfo = item as DirectoryInfo;
                    if (recursive)
                    {
                        tmppath = Path.Combine(destdir, dirInfo.Name);
                        CopyDirectory(dirInfo, tmppath, recursive);
                    }
                }
                else if (item is FileInfo)
                {
                    FileInfo fileInfo = item as FileInfo;
                    tmppath = Path.Combine(destdir, fileInfo.Name);
                    fileInfo.CopyTo(tmppath, false);
                }
            }
        }
        #endregion
    }
}
