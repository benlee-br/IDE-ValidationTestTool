using System;

namespace BioRad.Common
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
	///			<item name="vssfile">$Workfile: IDirty.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/IDirty.cs $</item>
	///			<item name="vssrevision">$Revision: 1 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 8/07/03 3:13p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public interface IDirty
	{
        #region Accessors
		/// <summary>
		/// 
		/// </summary>
		bool IsDirty
		{
			get;
			set;
		}
        #endregion
	}
}
