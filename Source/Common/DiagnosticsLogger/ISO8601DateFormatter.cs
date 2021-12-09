using System;
using System.Text;
using System.Globalization;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>
	/// Formats the <see cref="DateTime"/> specified as a ISO8601 formatted date string: 'YYYY-MM-DDTHH:MM:SS,SSS'.
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
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
	///			<item name="vssfile">$Workfile: ISO8601DateFormatter.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/DiagnosticsLogger/ISO8601DateFormatter.cs $</item>
	///			<item name="vssrevision">$Revision: 6 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 1/30/06 10:01a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public class ISO8601DateTime
	{
		#region Methods
		/// <summary>
		/// Converts a ISO 8601 format date time string to a local DateTime object.
		/// </summary>
		/// <param name="iSO8601DateTime"></param>
		/// <returns>DateTime object</returns>
		public static DateTime ToLocal(string iSO8601DateTime)
		{
			// defect 3957 - Ralph - 01/30/2006 
			// Log Timestamps not accounting for timezone
            return W3CDateTime.ToLocal(iSO8601DateTime, CultureInfo.InvariantCulture);
		}
		/// <summary>
		/// Gets ISO 8601 UTC date and time.
		/// </summary>
		/// <param name="dateTime">The date to format.</param>
        /// <returns>Date time as string formatted as ISO 8601 format</returns>
		public static string ToString(DateTime dateTime)
		{
			// defect 3957 - Ralph - 01/30/2006 
			// Log Timestamps not accounting for timezone
            return W3CDateTime.ToUtc(dateTime, CultureInfo.InvariantCulture);
		}
		/// <summary>
		/// Get difference between two date time strings.
		/// </summary>
		/// <param name="iSO8601DateTime1">string format 'YYYY-MM-DDTHH:MM:SS,SSS' or 'YYYY-MM-DDTHH:MM:SS'</param>
		/// <param name="iSO8601DateTime2">string format 'YYYY-MM-DDTHH:MM:SS,SSS' or 'YYYY-MM-DDTHH:MM:SS'</param>
		/// <returns>TimeSpan object that represents the difference between the two date times.</returns>
		public static TimeSpan Diff(string iSO8601DateTime1, string iSO8601DateTime2)
		{
			// defect 3957 - Ralph - 01/30/2006 
			// Log Timestamps not accounting for timezone
            return W3CDateTime.Diff(iSO8601DateTime1, iSO8601DateTime2, CultureInfo.InvariantCulture);
		}
		#endregion
	}
}
