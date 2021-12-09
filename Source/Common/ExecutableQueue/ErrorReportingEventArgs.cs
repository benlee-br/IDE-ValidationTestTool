using System;
using System.Text;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// This class packages an error reporting exception.
	/// </summary>
	/// <remarks>
	/// 
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href=""></see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: ErrorReportingEventArgs.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ExecutableQueue/ErrorReportingEventArgs.cs $</item>
	///			<item name="vssrevision">$Revision: 3 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Jlerner $</item>
	///			<item name="vssdate">$Date: 6/12/07 2:19p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class ErrorReportingEventArgs : EventArgs
	{
		//#region Constants
		//#endregion

		#region Member Data
		/// <summary>
		/// Error exception.
		/// </summary>
		private Exception m_ErrorException;
		/// <summary>
		/// Name, if any, of Object generating and reporting the exception.
		/// </summary>
		private string m_SenderName;
		/// <summary>
		/// Error level.
		/// </summary>
		private ExecutableObject.ErrorLevel m_ErrorLevel = ExecutableObject.ErrorLevel.Unassigned;
		/// <summary>
		/// Localized error string.
		/// </summary>
		private string m_ErrorMessageLocalized = string.Empty;
		/// <summary>
		/// Invariant culture error string.
		/// </summary>
		private string m_ErrorMessageInvariantCulture = string.Empty;

		#endregion

		#region Accessors
		/// <summary>
		/// Localized error message.
		/// </summary>
		public string ErrorMessageLocalized
		{
			get { return m_ErrorMessageLocalized; }
			set { m_ErrorMessageLocalized = value; }
		}
		/// <summary>
		///  Invariant culture erro rmessage.
		/// </summary>
		public string ErrorMessageInvariantCulture
		{
			get { return m_ErrorMessageInvariantCulture; }
			set { m_ErrorMessageInvariantCulture = value; }
		}
		/// <summary>
		/// Error Level.
		/// </summary>
		public ExecutableObject.ErrorLevel ErrorLevel
		{
			get { return m_ErrorLevel; }
			set { m_ErrorLevel = value; }
		}
		/// <summary>
		/// Exception object.
		/// </summary>
		public Exception ErrorException
		{
			get { return m_ErrorException; }
			set { m_ErrorException = value; }
		}

		/// <summary>
		/// Name, if any, of Object generating and reporting the exception.
		/// </summary>
		public String SenderName
		{
			get { return m_SenderName; }
			set { m_SenderName = value; }
		}
		#endregion

		//#region Delegates and Events
		//#endregion

		#region Constructors and Destructor

		/// <summary>Initializes a new instance of the ErrorReportingEventArgs class.</summary>
		public ErrorReportingEventArgs() { }

		#endregion

		//#region Methods
		//#endregion

		//#region Event Handlers
		//#endregion
	}
}
