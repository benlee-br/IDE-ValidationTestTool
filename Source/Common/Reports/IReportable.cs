using System;

namespace BioRad.Reports
{
	#region Documentation Tags
	/// <summary>
	/// UI forms and other types providing reporting facilities should implement this interface.
	/// </summary>
	/// <remarks>
	/// Implementers can be used to provide report data to types implementing IReporter,
	/// such as ReportViewer.
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
	///			<item name="vssfile">$Workfile: IReportable.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/Reports/IReportable.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Staran $</item>
	///			<item name="vssdate">$Date: 1/23/06 2:13p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public interface IReportable
	{
        #region Accessors
		/// <summary>
		/// Returns a list of all report tags populated by implementing type.
		/// </summary>
		string [] ReportTags { get; }
		/// <summary>
		/// Returns the enumeration type defining report tags directly populated
		/// by implementing type.
		/// </summary>
		Type ReportTagType { get; }
		/// <summary>
		/// Returns the name (form ID) of the report viewer to use. If null or empty,
		/// a default report viewer is requested.
		/// </summary>
		string ReportViewer { get; }
		#endregion

		#region Methods
		/// <summary>
		/// Return report data to be used by a reporter.
		/// </summary>
		/// <param name="caption">Report caption, displayed to the user. </param>
		/// <param name="initialReport">First report to be selected from report list.</param>
		/// <param name="reports">All reports available for report data.</param>
		/// <param name="reportData">Collection of name-value pairs where name identifies the 
		/// data to be reported and value is the data to be reported.</param>
		/// <param name="folderToSave">RTF and PDF files will be saved there unless value is null 
		/// (in that case use default folder)</param>
		void GetReportData ( out string caption, 
			out object initialReport, 
			out object [] reports, 
			out ReportedCollection reportData,out string folderToSave);
		/// <summary>
		/// Add report data to a report collection.
		/// </summary>
		/// <remarks>This method will be used when collecting report data from
		/// multiple sources.</remarks>
		/// <param name="reportData">Collection of name-value pairs where name identifies the 
		/// data to be reported and value is the data to be reported.</param>
		void AddReportData(ref ReportedCollection reportData);
        #endregion
	}
}
