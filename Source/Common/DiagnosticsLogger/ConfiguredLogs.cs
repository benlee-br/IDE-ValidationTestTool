using System;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>
	/// This class contains information about a Diagnostic log configuration file.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Ralph Ansell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">Replace this text with ID</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\DiagLogger.wmf">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: ConfiguredLogs.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/ConfiguredLogs.cs $</item>
	///			<item name="vssrevision">$Revision: 11 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class ConfiguredLog
	{
        #region Member Data
		/// <summary>
		/// String (log name) used to create a relationship between a source of events and a log.
		/// </summary>
		private string m_Name = "";
		/// <summary>
		/// String to display to users that identifies the log.
		/// </summary>
		private string m_DisplayName = "";
		/// <summary>
		/// Full path to where the log file exists.
		/// </summary>
		private string m_File = "";
		/// <summary>
		/// Logger level
		/// </summary>
		private DiagnosticLevel m_Level = DiagnosticLevel.ALL;
		/// <summary>
		/// Level assigned to all loggers regardless of the level specified by the specfic logger.
		/// </summary>
		private DiagnosticLevel m_LevelAllLogs = DiagnosticLevel.ALL;
		/// <summary>
		/// 
		/// </summary>
		private string m_Override = "";
        #endregion

        #region Accessors
		/// <summary>
		/// Gets/Sets override flag for changing logging level for all loggers.
		/// </summary>
		public string Override
		{
			get
			{
				return m_Override;
			}
			set
			{
				m_Override = value;
			}
		}
		/// <summary>
		/// Gets/Sets logger level.
		/// </summary>
		public DiagnosticLevel LevelAllLogs
		{
			get
			{
				return m_LevelAllLogs;
			}
			set
			{
				m_LevelAllLogs = value;
			}
		}
		/// <summary>
		/// Gets/Sets logger level.
		/// </summary>
		public DiagnosticLevel Level
		{
			get
			{
				return m_Level;
			}
			set
			{
				m_Level = value;
			}
		}
		/// <summary>
		/// Gets/Sets file path of the log file.
		/// </summary>
		public string File
		{
			get
			{
				return m_File;
			}
			set
			{
				m_File = value;
			}
		}
		/// <summary>
		/// Gets/Sets log name.
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}
		/// <summary>
		/// Gets/Sets display name for the log file.
		/// </summary>
		public string DisplayName
		{
			get
			{
				return m_DisplayName;
			}
			set
			{
				m_DisplayName = value;
			}
		}
        #endregion
	}
}
