using System;
using System.Text;

namespace BioRad.Common.DiagnosticsLogger
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
	///			<item name="vssfile">$Workfile: ConsoleFormat.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/ConsoleFormat.cs $</item>
	///			<item name="vssrevision">$Revision: 3 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public partial class ConsoleFormat : IFormat
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
		/// 
		/// </summary>
		/// <param name="logItem"></param>
		/// <returns></returns>
		public string Format(DiagnosticsLogItem logItem)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("Time: ");
			sb.Append(logItem.TimeStamp);
			sb.Append("\n");

			sb.Append("Severity: ");
			sb.Append(logItem.SeverityName);
			sb.Append("\n");

			sb.Append("Tag: ");
			sb.Append(logItem.TagName);
			sb.Append("\n");

			sb.Append("Message: ");
			sb.Append(logItem.Message);
			sb.Append("\n");

			return sb.ToString();
		}
        #endregion

		#region Event Handlers
		#endregion
	}
}
