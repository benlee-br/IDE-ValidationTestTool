using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Globalization;

using BioRad.Common.DiagnosticsLogger;

namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>
    /// Synchronize access to a file that is read and written to by multiple processes.
    /// </summary>
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
    ///			<item name="vssfile">$Workfile: SyncFileAccess.cs $</item>
    ///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Common/SyncFileAccess.cs $</item>
    ///			<item name="vssrevision">$Revision: 14 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Staran $</item>
    ///			<item name="vssdate">$Date: 7/09/10 2:26p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    public class SafeFileAccess : IDisposable
    {
        //todo:Ralph rename file.
        #region Constants
        [DllImport("Kernel32", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateFileW", ExactSpelling = true)]
        private static extern SafeFileHandle CreateFile(String pFileName, FileAccess dwDesiredAccess,
           FileShare dwShareMode, IntPtr pSecurityAttributes, FileMode dwCreationDisposition, FileOptions dwFlagsAndAttributes,
           IntPtr hTemplateFile);
        #endregion

        #region Member Data
        private SafeFileHandle m_SafeFileHandle;
        #endregion

        //#region Accessors
        //#endregion

        //#region Delegates and Events
        //#endregion

        #region Constructors and Destructor
        private SafeFileAccess()
        {
        }
        /// <summary>
        /// Synchronize access to a file that is read and written to by multiple threads/processes.
        /// </summary>
        /// <remarks>
        /// Trys to gain access to file. Retrys 10 times with 50ms delay.
        /// <example>
        /// <code>
        /// SafeFileHandle handle;
        /// using (SafeFileAccess w = new SafeFileAccess
        ///     (path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, out handle))
        /// {
        ///     if (!handle.IsInvalid)
        ///     {
        ///         using (FileStream fs = new FileStream(handle, FileAccess.Read))
        ///         {
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>
        /// </remarks>
        /// <param name="path">The file to access.</param>
        /// <param name="mode">
        /// A System.IO.FileMode value that specifies whether a file is created if one
        /// does not exist, and determines whether the contents of existing files are
        /// retained or overwritten.
        /// </param>
        /// <param name="access">
        /// A System.IO.FileAccess value that specifies the operations that can be performed
        /// on the file.
        /// </param>
        /// <param name="share">
        /// A System.IO.FileShare value specifying the type of access other threads have
        /// to the file.
        /// </param>
        /// <param name="handle">Wrapper class for a file handle.</param>
        public SafeFileAccess(
            string path, FileMode mode, FileAccess access, FileShare share, out SafeFileHandle handle)
        {
            int maxRetry = 10;
            int sleep = 100;

            try
            {
                do
                {
                    m_SafeFileHandle = CreateFile(path,
                        access, share, IntPtr.Zero, mode, FileOptions.None, IntPtr.Zero);

					// 05/11/2010 VN: Possible fix for Defect 11241: 
					// Check for handle not null
                    if (m_SafeFileHandle != null && !m_SafeFileHandle.IsInvalid)
                        break;

                    Thread.Sleep(sleep);
                } while (maxRetry-- > 0);
            }
            catch (Exception ex) //Logged
            {
				// Fix for Bug 10766
				DiagnosticsLogService.GetService.GetDiagnosticsLog
					("SafeFileAccess").SeriousError(ex.Message, ex);

                System.Diagnostics.Debug.Assert(false, ex.Message);
            }
            finally
            {
                handle = m_SafeFileHandle;

				// 05/11/2010 VN: Possible fix for Defect 11241: 
				// Check for handle not null
				if (handle != null && handle.IsInvalid)
                {
                    DiagnosticsLogService.GetService.GetDiagnosticsLog("SafeFileAccess").Debug
                        (string.Format(CultureInfo.InvariantCulture, 
                        "Failed to gain access to file {0}", path));
                }

            }
        }
        /// <summary></summary>
        ~SafeFileAccess()
        {
            Dispose(false);
        }

        #endregion

        #region Methods
        /// <summary>Dispose method.</summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_SafeFileHandle != null)
                    m_SafeFileHandle.Close();
            }
        }
        /// <summary>Dispose method.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        //#region Event Handlers
        //#endregion
    }

	/// <summary>Synchronize access to file using named mutex.</summary>
	public class LockFileAccess : IDisposable//bug 1137
	{
		// Example usage:
		//
		//try
		//{
		//    using (LockFileAccess sio = new LockFileAccess(path, 1000))
		//    using (FileStream fs = File.Open(path, FileMode.Append, FileAccess.Write, FileShare.None))
		//    using (StreamWriter sw = new StreamWriter(fs))
		//    {
		//        string msg = string.Format("{0} {1}",
		//            DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss.ffff tt", System.Globalization.CultureInfo.InvariantCulture),
		//            line);
		//        sw.WriteLine(msg);
		//        sw.Flush();
		//    }
		//}
		//catch (Exception ex)
		//{
		//    Debug.Assert(false, ex.Message);
		//}
		//
		//using (LockFileAccess sio = new LockFileAccess(path, 1000))
		//{
		//    File.Delete(path);
		//}

		//#region Constants
		//#endregion

		#region Member Data
		private string m_Path;
		private Mutex m_Mutex;
		private bool m_ReleaseMutex;
		#endregion

		//#region Accessors
		//#endregion

		//#region Delegates and Events
		//#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the LockFile class.
		/// </summary>
		/// <param name="path">Full path of file.</param>
		/// <param name="waitTimeInMs">How long to wait for access.</param>
		public LockFileAccess(string path, int waitTimeInMs)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException("path");
			if (waitTimeInMs < 0)
				throw new ArgumentNullException("waitTimeInMs");

			m_ReleaseMutex = false;
			m_Path = path;
			m_Mutex = new Mutex(false, GetMutexName(path));

			if (m_Mutex.WaitOne(waitTimeInMs, false))
				m_ReleaseMutex = true;
			else
				throw new ApplicationException("mutex timeout");
		}
		#endregion

		#region Methods
		/// <summary>
		/// Create mutex name from full file path.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns>Mutex name</returns>
		private string GetMutexName(string fileName)
		{
			string canonicalName = Path.GetFullPath(fileName).ToLower();
			canonicalName = canonicalName.Replace('\\', '_');
			canonicalName = canonicalName.Replace('/', '_');
			canonicalName = canonicalName.Replace(':', '_');
			return "lockfile-mutex-" + canonicalName;
		}
		/// <summary>
		/// Release mutex.
		/// </summary>
		public void Dispose()
		{
			// Release the Mutex.
			if (m_ReleaseMutex)
				m_Mutex.ReleaseMutex();
		}
		#endregion

		//#region Event Handlers
		//#endregion
	}
}
