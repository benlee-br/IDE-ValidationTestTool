using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32.SafeHandles;

using BioRad.Common.DiagnosticsLogger;
namespace BioRad.Common.Enumeration
{
   #region Documentation Tags
   /// <summary>
   /// Class Summary
   /// </summary>
   /// <remarks>
   /// Remarks section for class.
   /// </remarks>
   /// <classinformation>
   ///		<list type="bullet">
   ///			<item name="authors">Authors:</item>
   ///			<item name="review">Last design/code review:</item>
   ///			<item name="conformancereview">Conformance review:</item>
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
   ///			<item name="vssfile">$Workfile: EnumeratorWatcher.cs $</item>
   ///			<item name="vssfilepath">$Archive: /PcrDev/Source/Core/Common/Enumeration/EnumeratorWatcher.cs $</item>
   ///			<item name="vssrevision">$Revision: 22 $</item>
   ///			<item name="vssauthor">Last Check-in by:$Author: Ssarup $</item>
   ///			<item name="vssdate">$Date: 9/15/09 11:46p $</item>
   ///		</list>
   /// </archiveinformation>
   #endregion

   public class NewEnumerationInfoEventArgs : EventArgs
   {
      private NewEnumerationInfoEventArgs() { }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="s"></param>
	  /// <param name="blockType">Identifies source of event MiniOpticon or other</param>
	   public NewEnumerationInfoEventArgs(string s, Blocks blockType)
      {
         m_enumerationInfo = s;
		 m_BlockType = blockType;
      }
      /// <summary>
      /// 
      /// </summary>
      public string m_enumerationInfo;
	   /// <summary>
	   /// 
	   /// </summary>
	   public Blocks m_BlockType;
   }

   /// <summary>
   /// 
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
   public delegate void NewEnumerationInfoEventHandler(object sender, NewEnumerationInfoEventArgs e);

   /// <summary>
   /// Watches a directory for files dropped by the enumerator service, and fires
   /// events containing the new information from those files.
   /// </summary>
   public partial class EnumeratorWatcher : IDisposable
   {
      #region Constants
      #endregion

      #region Member Data
      /// <summary>
      /// the dot net object responsible for watching the directory in which
      /// the enumerator will be dumping messages.
      /// </summary>
       private FileSystemWatcher m_watcher;
	   private string m_Path;
	   private string m_Filename;
	   private Blocks m_BlockType;
      #endregion

      #region Accessors
	  /// <summary>
	  /// 
	  /// </summary>
	  public string WatchedFile
	  {
		  get { return m_Filename; }
	  }
      /// <summary>
      /// 
      /// </summary>
      public string WatchedPath
      {
         get { return m_watcher.Path; }
         set { m_watcher.Path = value; }
      }
      #endregion

      #region Delegates and Events
      /// <summary>
      /// 
      /// </summary>
      public event NewEnumerationInfoEventHandler EnumerationEvent;
      #endregion

      #region Constructors and Destructor
      /// <summary>
      /// Default constructor
      /// </summary>
      [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
      private EnumeratorWatcher()
      {
      }
      /// <summary>
      /// public constructor which takes the full pathname of the directory to watch.
      /// This must be the directory which the service has been told about.
      /// </summary>
      /// <param name="path">the full rooted path of the directory to watch.</param>
	  /// <param name="filename">the name of the file to watch.</param>
	  /// <param name="blockType"></param>
       [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
       public EnumeratorWatcher(string path, string filename, Blocks blockType)
       {
           m_BlockType = blockType;
           m_Filename = Path.GetFileName(filename);
           m_Path = Path.GetFullPath(path);
           if (!Directory.Exists(m_Path))
               Directory.CreateDirectory(m_Path);
           m_watcher = new FileSystemWatcher(m_Path, m_Filename);
		   // Ralph temp fix for windows 8
		   m_watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.LastWrite;// fix for windows 8
           m_watcher.Changed += new FileSystemEventHandler(OnNewEnumerationInfo);
           m_watcher.EnableRaisingEvents = true;
       }
      #endregion

       #region Methods
       /// <summary>
       /// Force a new enumeration. This will not force the server to enumerate.
       /// It will only fire an EnumerationEvent event.
       /// </summary>
       public void NewEnumeration()
       {
		   FireNewEnumerationInfo(Path.Combine(m_watcher.Path, WatchedFile));
       }
       private void FireNewEnumerationInfo(string path)
       {
           if (string.IsNullOrEmpty(path))
               throw new ArgumentNullException("path");

           if (File.Exists(path))
           {
               bool ok = true;
               try
               {
                   SafeFileHandle handle;
                   using (SafeFileAccess w = new SafeFileAccess
                       (path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, out handle))
                   {
                       if (!handle.IsInvalid)
                       {
                           using (FileStream fs = new FileStream(handle, FileAccess.Read))
                           using (StreamReader sr = new StreamReader(fs))
                           {
                               string s = sr.ReadToEnd();
                               ok = true;

                               // Publish the event.
                               ThreadPool.QueueUserWorkItem(
                                    new WaitCallback(FireEventAsync),
                                    new NewEnumerationInfoEventArgs(s, m_BlockType));
                           }
                       }
                   }
               }
               catch (Exception e)
               {
				   // Fix for Bug 10766
				   DiagnosticsLogService.GetService.
						GetDiagnosticsLog("EnumeratorWatcher").Warn(e.Message, e);
				   //DiagnosticsLogService.GetService.
				   //     GetDiagnosticsLog("EnumeratorWatcher").Warn(e.Message);
               }
               finally
               {
                   if (!ok)
                   {
                       string s = string.Format("Error: failed to read file {0}", path);
                       DiagnosticsLogService.GetService.
                            GetDiagnosticsLog("EnumeratorWatcher").SeriousError(s);
                   }
               }
           }
       }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="stateInfo"></param>
       private void FireEventAsync(Object stateInfo)
       {
           if (EnumerationEvent != null)
           {
               NewEnumerationInfoEventArgs args = stateInfo as NewEnumerationInfoEventArgs;
               EnumerationEvent(this, args);
           }
       }
       #endregion

      #region Event Handlers
      /// <summary>
      /// Delegate for file change event, used by the watcher.
      /// </summary>
      /// <param name="source"></param>
      /// <param name="e"></param>
       private void OnNewEnumerationInfo(object source, FileSystemEventArgs e)
       {
		   if (e.Name == WatchedFile)
           {
               FireNewEnumerationInfo(e.FullPath);
           }
       }
      #endregion

      #region IDisposable Members

      /// <summary>
      /// 
      /// </summary>
      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="disposing"></param>
      protected virtual void Dispose(bool disposing)
      {
         if (disposing)
         {
            if (m_watcher != null)
            {
               m_watcher.Dispose();
               m_watcher = null;
            }
         }
      }

      #endregion
   }
}
