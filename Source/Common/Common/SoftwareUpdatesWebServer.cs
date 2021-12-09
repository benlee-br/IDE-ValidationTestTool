using System;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Configuration;

namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>
    /// File transfer to/from software updates web server.
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
    ///			<item name="vssfile">$Workfile: SoftwareUpdatesWebServer.cs $</item>
    ///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Common/SoftwareUpdatesWebServer.cs $</item>
    ///			<item name="vssrevision">$Revision: 6 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
    ///			<item name="vssdate">$Date: 1/11/10 1:15p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    public class SoftwareUpdatesWebServer
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
        #endregion

        #region Methods
        /// <summary>
        /// Down load a file from a web server.
        /// </summary>
        /// <param name="url">Web server address with file name.</param>
        /// <param name="dest">Path where to copy file.</param>
        /// <returns></returns>
        public static bool DownloadFile(string url, string dest)
        {
            bool ok = true;
            try
            {
                using (System.Net.WebClient wcDownload = new System.Net.WebClient())
                {
                    //"http://gxdsw1/GXDSoftwareUpdates/C1000/"
                    int index = url.LastIndexOf('/');
                    string name = url.Substring(0, index);
                    wcDownload.BaseAddress = name;
                    wcDownload.UseDefaultCredentials = true;

                    // Create a request to the file we are downloading
                    System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                    webRequest.ContentType = "application/octet-stream";

                    // Set default authentication for retrieving the file
                    webRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
                    // Retrieve the response from the server
                    System.Net.HttpWebResponse webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse();
                    // Ask the server for the file size and store it
                    //Int64 fileSize = webResponse.ContentLength;

                    // Open the URL for download 
                    using (Stream reader = wcDownload.OpenRead(url))
                    using (Stream writer = new FileStream(dest, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        // It will store the current number of bytes we retrieved from the server
                        int bytesSize = 0;
                        int totalBytesRead = 0;
                        byte[] downBuffer = new byte[2048];

                        while ((bytesSize = reader.Read(downBuffer, 0, downBuffer.Length)) > 0)
                        {
                            totalBytesRead += bytesSize;
                            writer.Write(downBuffer, 0, bytesSize);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                string message = ex.Message;
                ok = false;
            }

            return ok;
        }
        /// <summary>
        /// ************************ not working
        /// </summary>
        /// <param name="url"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        private static bool UploadFile(string url, string src)
        {
            Debug.Assert(false, "under construction");
            bool ok = true;
            try
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    wc.BaseAddress = Path.GetDirectoryName(url); 

                    url = wc.BaseAddress + "upgrade_versions_Copy.txt";
                    wc.UseDefaultCredentials = true;

                    // Create a request to the file we are downloading
                    System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                    // Set default authentication for retrieving the file
                    webRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
                    webRequest.Method = "POST";

                    using (Stream requestStream = webRequest.GetRequestStream())
                    using (Stream reader = new FileStream(src, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        // It will store the current number of bytes we retrieved from the server
                        int bytesSize = 0;
                        byte[] downBuffer = new byte[2048];

                        while ((bytesSize = reader.Read(downBuffer, 0, downBuffer.Length)) > 0)
                        {
                            requestStream.Write(downBuffer, 0, bytesSize);
                        }
                    }

                    wc.UploadFile(url, src);
                }
            }
            catch
            {
                ok = false;
            }
            return ok;
        }
        #endregion

        #region Event Handlers
        #endregion
    }
}
