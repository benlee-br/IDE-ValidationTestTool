using System;
using BioRad.Common.DiagnosticsLogger;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Range error exception. 
	/// </summary>
	/// <remarks>
	/// Use this class for throwing a range error exception when an out of range error is detected.
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
	///			<item name="vssfile">$Workfile: RangeErrorException.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/ApplicationExceptions/RangeErrorException.cs $</item>
	///			<item name="vssrevision">$Revision: 3 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 10/13/03 1:01p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public class RangeErrorException
	{
		#region Constructors and Destructor
		/// <summary>
		/// Initializes a new instance of the RangeErrorException class.
		/// </summary>
		public RangeErrorException()
		{
		}
		#endregion

		#region Accessors
		/// <summary>
		/// Get range error exception. 
		/// </summary>
		/// <remarks>
		/// Creates new application exception for general range error.
		/// </remarks>
		public ApplicationException Range
		{
			get
			{
				//todo: language translation for strings.
				string description = "range error";
				ApplicationException ex = new ApplicationException(description);
				return ex;
			}
		}
		/// <summary>
		/// Get lower bound range error exception. 
		/// </summary>
		/// <remarks>
		/// Creates new application exception for lower bound range error.
		/// </remarks>		
		public ApplicationException LowerBound
		{
			get
			{
				//todo: language translation for strings.
				string description = "lower bound range error";
				ApplicationException ex = new ApplicationException(description);
				return ex;
			}
		}
		/// <summary>
		/// Get upper bound range error exception. 
		/// </summary>
		/// <remarks>
		/// Creates new application exception for upper bound range error.
		/// </remarks>
		public ApplicationException UpperBound
		{
			get
			{
				//todo: language translation for strings.
				string description = "upper bound range error";
				ApplicationException ex = new ApplicationException(description);
				return ex;
			}
		}
		#endregion
	}
}
