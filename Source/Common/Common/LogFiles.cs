using System;
using System.IO;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Log files.
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
	///			<item name="vssfile">$Workfile: LogFiles.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/LogFiles.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Pwalse $</item>
	///			<item name="vssdate">$Date: 9/15/06 5:38p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class LogFiles
	{
		#region Constants
		/// <summary>
		/// Log file extension.
		/// </summary>
		private const string c_LogFileExtension = @".xml";
        #endregion

        #region Member Data
        #endregion

        #region Accessors
        #endregion

        #region Constructors and Destructor
        #endregion

        #region Methods
		/// <summary>
		/// Manages history log file sizes, backs them up as needed.
		/// </summary>
		/// <param name="file"></param>
		/// <param name="maxFileSize"></param>
		public static void ManageHistoryLogFileSize(string file, long maxFileSize)
		{
			try
			{
				//Set File
				string fileName = string.Concat(file, c_LogFileExtension);
				string historyLogFileNameUI = Path.Combine(ApplicationPath.CommonApplicationDataPath(), 
					Path.Combine("Logs", fileName));
				FileInfo fileInfo = new FileInfo(historyLogFileNameUI);
				if (fileInfo.Exists == true) 
				{
					if(fileInfo.Length > maxFileSize)//maxFileSize
					{
						//Rename and backup file.
						fileName = string.Concat(Path.GetFileNameWithoutExtension(historyLogFileNameUI), 
							" ", DateTime.Now.ToString("yyyy-MM-dd hhmm"),
							Path.GetExtension(historyLogFileNameUI));
						string backupDirectory = Path.Combine(ApplicationPath.CommonApplicationDataPath(),
							"Logs\\Backup");
						DirectoryInfo directoryInfo = new DirectoryInfo(backupDirectory);
						if(!directoryInfo.Exists)
						{
							directoryInfo.Create();
						}
						directoryInfo = null;
						fileInfo.MoveTo(Path.Combine(backupDirectory, fileName));
					}
				}
				fileInfo = null;
			}
			catch
			{
				//Ignore these exceptions.
			}
		}
        #endregion
	}
}
