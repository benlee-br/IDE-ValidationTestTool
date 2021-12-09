using System;

namespace BioRad.Common.BusinessRules
{
	#region Documentation Tags
	/// <summary>
	/// All business rules must implement this interface or derive from 
	/// AbstractBusinessRule.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Lisa von Schlegell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="conformancereview">Conformance review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">1595</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: IBusinessRule.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/BusinessRules/IBusinessRule.cs $</item>
	///			<item name="vssrevision">$Revision: 1 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Lvs $</item>
	///			<item name="vssdate">$Date: 5/25/04 10:12p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public interface IBusinessRule
	{
        #region Accessors
		/// <summary>
		/// Identifier is used to access a business rule from a service.
		/// Usually, identifier is concatenation of an enum type name and string value.
		/// Set implementation should be "set once".
		/// </summary>
		string Identifier { get; set; }
        #endregion

		#region Methods
        #endregion
	}
}
