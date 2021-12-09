using System;
using System.IO;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Threading;

using BioRad.Common.Utilities;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>
	/// Appends encrypted log events.
	/// </summary>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Ralph Ansell</item>
	///			<item name="review">Last design/code review:</item>
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
	///			<item name="vssfile">$Workfile: EncryptedAppender.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/EncryptedAppender.cs $</item>
	///			<item name="vssrevision">$Revision: 11 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 7/01/08 2:27p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class EncryptedAppender : ILogAppender
	{
		#region Constants
		#endregion

		#region Member Data
		/// <summary>
		/// This mutes is used to prevent failure opening the log file.
		/// The FileStream constructor is given read/write access to the file, 
		/// and it is opened sharing Read access (that is, requests to 
		/// open the file for writing by this or another process will fail 
		/// until the FileStream object has been closed, but read attempts will succeed). 
		/// </summary>
		private Mutex fileLock;
		/// <summary>
		/// File name (full path).
		/// </summary>
		private string m_Filename = null;
		#endregion

		#region Accessors
		/// <summary>
		/// Get/Set file name for this appender object.
		/// </summary>
		public string Filename
		{
			get
			{
				if (m_Filename == null || m_Filename.Length == 0)
					return null;
				return m_Filename;
			}
		}
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename">Full path of file name.</param>
		public EncryptedAppender(string filename)
		{
			if (filename == null || filename.Length == 0)
				throw new ArgumentNullException();

			DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(filename));
			if (!dir.Exists)
				throw new ApplicationException("Directory does not exist: " + dir.ToString());

			m_Filename = filename;

            string mutexName = m_Filename.GetHashCode().ToString();
            fileLock = new Mutex(false, mutexName);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Creates a shallow copy.
		/// </summary>
		public virtual object Clone()
		{
			EncryptedAppender appender = new EncryptedAppender(Filename);
			return appender;
		}
		/// <summary>
		/// Close appender and release resources.
		/// </summary>
		public virtual void Close()
		{
			fileLock.Close();
		}
        /// <summary>Open an existing appender and truncate so that its size is zero bytes.</summary>
        public virtual void Truncate()
        {
            FileStream fs = null;
            bool releaseFileLock = false;

            try
            {
                // Wait until it is safe to enter.
                if (fileLock.WaitOne(1000 * 60, false))
                {
                    releaseFileLock = true;
                    if (Filename != null)
                    {
                        // Fix for Bug 4159
                        fs = FileUtilities.OpenFile(Filename, FileMode.Truncate,
                            FileAccess.Write, FileShare.None);
                    }
                }
            }
            catch
            {
                //Do nothing
            }
            finally
            {
                if (fs != null)
                {
                    fs.Flush();
                    fs.Close();
                }

                // Release the Mutex.
                if (releaseFileLock)
                    fileLock.ReleaseMutex();
            }
        }
		/// <summary>
		/// Append a log entry appender specific way.
		/// </summary>
		/// <param name="logItem"></param>
		[MethodImplAttribute(MethodImplOptions.Synchronized)]
		public virtual void Append(DiagnosticsLogItem logItem)
		{
			Write(logItem);
		}
		/// <summary>
		/// Opens a binary writer on the supplied filestream.
		/// </summary>
		/// <param name="fileStream">The filestream from which to get a writer.</param>
		/// <returns>A writer.</returns>
		internal static BinaryWriter GetNewBinaryWriter(FileStream fileStream)
		{
			Encoding encoding = new UTF8Encoding();
			BinaryWriter writer = new BinaryWriter(new BufferedStream(fileStream), encoding);
			return writer;
		}
		/// <summary>
		/// Writes DiagnosticsLogItem item to log file.
		/// </summary>
		/// <param name="di">DiagnosticsLogItem item to write.</param>
		/// <remarks>
		/// Set file position to start of ending root element. Write new log entry, then write ending root element.
		/// </remarks>
		[MethodImplAttribute(MethodImplOptions.Synchronized)]
		private void Write(DiagnosticsLogItem di)
		{
			int attemptsToWrite = 0;

			if (Filename == null)
				return;

			while (attemptsToWrite < 5)
			{
                try
                {
                    // Wait until it is safe to enter.
                    if (fileLock.WaitOne(100, false))
                    {
                        try
                        {
                            // Fix for Bug 4159
                            using (FileStream fs = FileUtilities.OpenFile(Filename, FileMode.Append,
                                FileAccess.Write, FileShare.Read))
                            using (BinaryWriter writer = GetNewBinaryWriter(fs))
                            {
                                if (fs.Length == 0)
                                {	// write header.
                                    LoggerXmlTags tag =
                                        new LoggerXmlTags(XmlElementName.BioRadDiagnosticLogFile);
                                    // Convert the start tag into a byte[].
                                    byte[] bytes = Encoding.UTF8.GetBytes(tag.StartTag);
                                    writer.Write(bytes, 0, bytes.Length);
                                    //versions prior to version 1.0 will have empty elements.
                                    writer.Write((Int32)DiagnosticsLogService.c_VersionEncyrptedLogFiles);//encryped log files version
                                    bytes = Encoding.UTF8.GetBytes(tag.EndTag);
                                    writer.Write(bytes, 0, bytes.Length);
                                }

                                // write log item.
                                string str = di.ToXmlString();
                                byte[] encrypted = FileCryptor.GetInstance.Encrypt(str);
                                writer.Write((Int32)encrypted.Length);
                                writer.Write(encrypted, 0, encrypted.Length);
#if DEBUG
                                string s1 = FileCryptor.GetInstance.Decrypt(encrypted);
                                System.Diagnostics.Debug.Assert(string.Compare(s1, str, true) == 0);
#endif
                            }
                            break;
                        }
                        finally
                        {
                            fileLock.ReleaseMutex();
                        }
                    }
                }
                catch (AbandonedMutexException abandonedMutexException)
                {
                    System.Diagnostics.Debug.WriteLine(abandonedMutexException.Message);
                    break;
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    //Do nothing
                }
				finally
				{
					attemptsToWrite++;
				}
			}
		}
		#endregion
	}
}
