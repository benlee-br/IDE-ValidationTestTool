using System;
using System.IO;

namespace BioRad.Reports
{
	#region Documentation Tags
	/// <summary>
	/// UI forms and other types providing reporting facilities should implement this interface.
	/// </summary>
	/// <remarks>
	/// Implemented by ReportViewer.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Lisa von Schlegell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="conformancereview">Conformance review: LvS 9/14/04</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">679</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: IReporter.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/Reports/IReporter.cs $</item>
	///			<item name="vssrevision">$Revision: 6 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Staran $</item>
	///			<item name="vssdate">$Date: 1/23/06 2:13p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public interface IReporter
	{
        #region Accessors
		/// <summary>
		/// Reporter caption, displayed to the user. 
		/// </summary>
		string Caption { set;}
		/// <summary>
		/// Specify Optional folder to save reports. If null, will use the default config file report folder
		/// </summary>
		string FolderToSave { set;}
		/// <summary>
		/// First report to be selected by reporter from report list.
		/// </summary>
		object InitialReport { set; }
		/// <summary>
		/// All reports available to the reporter.
		/// </summary>
		object[] Reports { set;}
		/// <summary>
		/// Collection of name-value pairs where name identifies the location within
		/// a report template and the value is the data to be inserted by the reporter
		/// into a report.
		/// </summary>
		/// <remarks>If no match is found for a given name within a report template,
		/// the data is not reported.</remarks>
		ReportedCollection ReportData { set;}
        #endregion

		#region Methods
		/// <summary>
		/// Clears settings including report data and templates.
		/// </summary>
		void Clear();

		/// <summary>
		/// Loads and prints a report under control of a print dialog.
		/// </summary>
		/// <remarks>Expects ReportData collection to have been set previously.</remarks>
		/// <param name="report">Report template to populate and print.</param>
		void QuickPrint(object report);
		#endregion
	}
}
