using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Reflection;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Threading;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>
	/// The XMLAppender class writes information about important software or hardware events to a log file.	
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
	///			<item name="vssfile">$Workfile: XMLAppender.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/XMLAppender.cs $</item>
	///			<item name="vssrevision">$Revision: 39 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 7/01/08 2:27p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public partial class XMLAppender : ILogAppender
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
				if ( m_Filename == null || m_Filename.Length == 0 )
					return null;
				return m_Filename;
			}
		}
        #endregion

        #region Constructors and Destructor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName">Full path of file name.</param>
		public XMLAppender(string fileName)
		{
			DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(fileName));
			if ( !dir.Exists )
			{
				throw new ApplicationException("Directory does not exist: " + dir.ToString());
			}
			
			m_Filename = fileName;
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
			XMLAppender appender = new XMLAppender(Filename);
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
		/// Writes DiagnosticsLogItem item to log file.
		/// </summary>
		/// <param name="di">DiagnosticsLogItem item to write.</param>
		/// <remarks>
		/// Set file position to start of ending root element. Write new log entry, then write ending root element.
		/// </remarks>
		[MethodImplAttribute(MethodImplOptions.Synchronized)]
		private void Write(DiagnosticsLogItem di)
		{
			LoggerXmlTags tag = new LoggerXmlTags(XmlElementName.BioRadDiagnosticLogFile);
			XmlTextWriter writer = null;
			FileStream fs = null;
			bool releaseFileLock = false;

			try
			{
				// Wait until it is safe to enter.
                if (fileLock.WaitOne(1000 * 60, false))
				{
					releaseFileLock = true;
					if ( Filename != null )
					{
						// Fix for Bug 4159
                        fs = FileUtilities.OpenFile(Filename, FileMode.Append,
                            FileAccess.Write, FileShare.Read);
						writer = new XmlTextWriter(fs, Encoding.UTF8);
						fs.Seek(fs.Length, SeekOrigin.Begin);

						if ( writer != null )
						{
							writer.Formatting = Formatting.Indented;
							writer.Namespaces = false;

							if ( fs.Length == 0 )
							{	// New file, write header.
								//writer.WriteStartDocument(true);
								writer.WriteRaw(tag.StartTag);
								//todo: Ralph maybe add some header stuff.
								writer.WriteRaw(tag.EndTag);
								writer.Flush();
							}

							// write log item.
							string str = di.ToXmlString();
							writer.WriteRaw(str);
							writer.Flush();
						}
					}
				}
			}
			catch
			{
				//Do nothing
				//TODO: Q these items in future to get them looged.
			}
			finally
			{
				if ( writer != null )
				{
					writer.Flush();
					writer.Close();
				}

				// Release the Mutex.
				if ( releaseFileLock )
					fileLock.ReleaseMutex();
			}
		}
        #endregion
	}
}
