using System;
using System.IO;
using System.Text;

namespace BioRad.Common.ApplicationExceptions
{
	#region Documentation Tags
	/// <summary>
	/// Static methods for formatting an exception as a string.
	/// </summary>
	/// <remarks>
	/// Copied from Ralph's DiagnosticLogItem.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Ralph Ansell, Lisa von Schlegell</item>
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
	///			<item name="vssfile">$Workfile: ExceptionFormatter.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ApplicationExceptions/ExceptionFormatter.cs $</item>
	///			<item name="vssrevision">$Revision: 5 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class ExceptionFormatter
	{
        #region Member Data
		/// <summary>
		/// Newline string defined for this environment.
		/// </summary>
		private static readonly string NewLine = System.Environment.NewLine;
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Private constructor prevents class instantiation.
		/// </summary>
		private ExceptionFormatter()
		{
		}
		#endregion

        #region Methods
		/// <summary>
		/// Format an exception into a string with appropriate paragraph spacing.
		/// </summary>
		/// <param name="ex">the exception to render</param>
		/// <returns>
		/// the string representation of the exception
		/// </returns>
		/// <remarks>
		/// <para>
		/// Renders the exception type, message, and stack trace. Any nested
		/// exceptions are also rendered.
		/// </para>
		/// </remarks>
		public static string FormatException(Exception ex)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Length = 0;

			if ( ex != null )
			{
                sb.Append(StringUtility.FormatString(Properties.Resources.ExceptionHeading))
					.Append(ex.GetType().FullName)
					.Append(NewLine)
                    .Append(StringUtility.FormatString(Properties.Resources.MessageHeading))
					.Append(ex.Message)
					.Append(NewLine);

				if (ex.Source != null && ex.Source.Length > 0)
				{
                    sb.Append(StringUtility.FormatString(Properties.Resources.SourceHeading))
						.Append(ex.Source).Append(NewLine);
				}
				if (ex.StackTrace != null && ex.StackTrace.Length > 0)
				{
					sb.Append(ex.StackTrace).Append(NewLine);
				}
				if (ex.InnerException != null)
				{
					sb.Append(NewLine)
                        .Append(StringUtility.FormatString(Properties.Resources.NestedExceptionHeading))
						.Append(NewLine)
						.Append(NewLine)
						.Append(FormatException(ex.InnerException))
						.Append(NewLine);
				}
			}
			return sb.ToString();
		}
		#endregion
	}
}
