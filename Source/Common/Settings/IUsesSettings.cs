using System;

namespace BioRad.Common.Settings
{
	#region Documentation Tags
	/// <summary>
	/// Types which require configuration with a settings service may implement
	/// this interface.
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
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
	///			<item name="vssfile">$Workfile: IUsesSettings.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/Settings/IUsesSettings.cs $</item>
	///			<item name="vssrevision">$Revision: 1 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Lvs $</item>
	///			<item name="vssdate">$Date: 5/25/04 10:13p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public interface IUsesSettings
	{
        #region Accessors
		/// <summary>
		/// Configures settings service.
		/// </summary>
		ISettingsService SettingsService { set; }
        #endregion
	}
}
