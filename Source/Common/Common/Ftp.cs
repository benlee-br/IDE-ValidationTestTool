using System;
using System.Text;

using Xceed.Compression;
using Xceed.FileSystem;
using Xceed.Zip;
using Xceed.Ftp;

namespace BioRad.Common
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
    ///			<item name="vssfile">$Workfile: Ftp.cs $</item>
    ///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Common/Ftp.cs $</item>
    ///			<item name="vssrevision">$Revision: 1 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
    ///			<item name="vssdate">$Date: 12/03/09 12:19p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    static public class Ftp
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
        /// <summary>Initializes a new instance of the Ftp class.</summary>
        static Ftp()
        {
            Xceed.Ftp.Licenser.LicenseKey = "FTN37-TMWEL-NKTKK-PNBA";
        }

        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostName"></param>
        public static void Connect(string hostName)
        {
           //  http://gxdsw1/GXDSoftwareUpdates/
           //  Physical location of folder is at : 
           //  \\gxdsw1\Development\Websites\GXDSoftwareUpdates

            // We will connect to the following FTP server, and login anonymously.
            // We will then transfer different files in the specified local folder.
            string hostname = "ftp.xceed.com";
            string localFolder = System.IO.Path.GetTempPath() + @"SnippetExplorer\";

            // Always start with an FtpConnection.
            using (FtpConnection connection = new FtpConnection(hostname))
            {
                // Let's access the starting folder on that FTP server.
                AbstractFolder starting = new FtpFolder(connection);

                // We'll be copying files into this local folder:
                AbstractFolder destination = new DiskFolder(localFolder);

                // First, you can copy a single file by getting a reference to that
                // file and calling CopyTo on it:
                AbstractFile sourceFile = starting.GetFile("FileSystem.txt");

                Console.WriteLine("Copying '{0}' in '{1}'",
                  sourceFile.Name,
                  destination.FullName);

                sourceFile.CopyTo(destination, true);

                // Second, you can copy files matching a certain filename pattern,
                // or even more advanced filtering criteria:
                Console.WriteLine("Copying '*.txt' smaller than 50k to '{0}'",
                  destination.FullName);

                starting.CopyFilesTo(
                  destination,
                  false,                         // do not copy files from subfolders
                  true,                          // overwrite existing files
                  "*.txt",                       // copy files matching this name pattern
                  new SizeFilter(0, 51200));  // only copy files smaller than 50kb

                // Finally, you can copy entire folders in a single call (recursively).
                // Let's copy the "images\spot" subfolder.
                AbstractFolder sourceFolder = starting.GetFolder(@"images\spot");

                Console.WriteLine("Copying '{0}' to '{1}'",
                  sourceFolder.FullName,
                  destination.FullName);

                sourceFolder.CopyTo(destination, true);
            }
        }
        #endregion

        #region Event Handlers
        #endregion
    }
}
