using System;

namespace BioRad.Common.Devices
{
	#region Documentation Tags
	/// <summary>
	/// Stores all device types.
	/// </summary>
	/// <remarks>
	/// BaseUnit = 1, Camera = 2, Filter = 3, BarCodeReader = 4.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Pramod Walse</item>
	///			<item name="review">Last design/code review:1/14/04, Pramod Walse</item>
	///			<item name="conformancereview">Conformance review:2/17/04, Pramod Walse</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">1253</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\DeviceManager.wmf">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: DeviceTypes.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/Devices/DeviceTypes.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Shabnam $</item>
	///			<item name="vssdate">$Date: 11/24/04 12:31p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	[Serializable]
	public enum DeviceTypes: int
	{
		/// <summary>Unknown device</summary>
		Unknown,
		/// <summary>
		/// BaseUnit.
		/// </summary>
		BaseUnit = 1,
		/// <summary>
		/// Camera.
		/// </summary>
		Camera = 2,
		/// <summary>
		/// Filter.
		/// </summary>
		Filter = 3,
		/// <summary>
		/// Bar code reader.
		/// </summary>
		BarCodeReader = 4
	}
}
