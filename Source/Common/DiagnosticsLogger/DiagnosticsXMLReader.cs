using System;
using System.Collections;
using System.IO;
using System.Xml;

namespace BioRad.Common.DiagnosticsLogger
{
	#region Documentation Tags
	/// <summary>Reads the Diagnostics XML file.</summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Shabnam</item>
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
	///			<item name="vssfile">$Workfile: DiagnosticsXMLReader.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/DiagnosticsXMLReader.cs $</item>
	///			<item name="vssrevision">$Revision: 3 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class DiagnosticsXMLReader
	{
		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static DiagnosticsLogItem[] ReadDiagnosticsFile(string fileName)
		{
			//Ralph - what happens if we try to read a huge log file. The LogEnumerator 
			//enumerates over the log items in the file, so there is no need to
			//copy the log items to an array list.
			ArrayList logItems = new ArrayList();
			try
			{
				using ( LogEnumerator logEnumerator = new LogEnumerator(fileName) )
				{
					foreach(DiagnosticsLogItem logItem in logEnumerator)
						logItems.Add(logItem);
				}
			}
			catch(XmlException xmlException)
			{
				Console.WriteLine(xmlException.Message);
			}
			catch(Exception exception)
			{
				Console.WriteLine(exception.Message);
			}
			return (DiagnosticsLogItem[]) logItems.ToArray(typeof(DiagnosticsLogItem));
		}
        #endregion
	}
}
