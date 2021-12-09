using System;
using BioRad.Common;
using BioRad.Common.DiagnosticsLogger;

namespace BioRad.Common.Devices
{
	#region Documentation Tags
	/// <summary>
	/// Base Interface of all devices.
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Ralph Ansell</item>
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
	///			<item name="vssfile">$Workfile: IDevice.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/Devices/IDevice.cs $</item>
	///			<item name="vssrevision">$Revision: 18 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Mchilcott $</item>
	///			<item name="vssdate">$Date: 3/24/05 3:21p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public interface IDevice
	{
		#region Accessors
		/// <summary>Set the DiagnosticsLog object for this device.</summary>
		DiagnosticsLog SetDiagnosticsLog{set;}
		#endregion

		#region Methods
		/// <summary>
		/// Attempt to connect to the device
		/// </summary>
		/// <param name="communicationSettings"></param>
		/// <returns><see cref="Results"/>object indicating whether command was successful or failed</returns>
		Results Connect(string communicationSettings);		

		/// <summary>
		/// Check whether a connection with device has been established.
		/// </summary>
		/// <returns>True if was able to establish communication with device.</returns>
		bool Connected();

		/// <summary>
		/// Disconnect from the device
		/// </summary>
		/// <returns><see cref="Results"/>object indicating whether command was successful or failed</returns>
		Results Disconnect();
		
		/// <summary>
		/// Check whether device is an emulator or not
		/// </summary>
		/// <returns>True if device is an emulator</returns>
		bool Emulated();

		/// <summary>
		/// Get the firmware version of the device
		/// </summary>
		/// <param name="firmwareVersion">Firmware version</param>
		/// <returns><see cref="Results"/>object indicating whether command was successful or failed.</returns>
		Results GetFirmwareVersion(ref string firmwareVersion);
		
		/// <summary>
		/// Get model
		/// </summary>
		/// <param name="modelName"></param>
		/// <returns><see cref="Results"/>object indicating whether command was successful or failed</returns>
		Results GetModel(ref string modelName);

		/// <summary>
		/// Returns the product name for the device.
		/// </summary>
		/// <param name="productName">Product name</param>
		/// <returns><see cref="Results"/>object indicating whether command was successful or failed.</returns>
		Results GetProductName(ref string productName);
		
		/// <summary>
		/// Return the serial number of the device.
		/// </summary>
		/// <param name="serialNumber">Serial number</param>
		/// <returns><see cref="Results"/>object indicating whether command was successful or failed.</returns>
		Results GetSerialNumber(ref string serialNumber);
		
		/// <summary>
		/// Returns the software version of the device.
		/// </summary>
		/// <param name="softwareVersion"></param>
		/// <returns><see cref="Results"/>object indicating whether command was successful or failed.</returns>
		Results GetSoftwareVersion(ref string softwareVersion);
		
		/// <summary>
		/// Resetting a device places the device back into an initial start-up condition.
		/// </summary>
		/// <returns><see cref="Results"/>object indicating whether command was successful or failed</returns>
		Results Reset();

		/// <summary>
		/// Request device to perform a self-test to verify readiness. 
		/// </summary>
		/// <returns><see cref="Results"/>object indicating whether command was successful or failed.</returns>
		Results SelfTest();

		/// <summary>
		/// Set model (Use with care)
		/// </summary>
		/// <param name="modelName"></param>
		/// <returns><see cref="Results"/>object indicating whether command was successful or failed</returns>
		Results SetModel(string modelName);

		/// <summary>
		/// Set the serial number of the device (Use with care)
		/// </summary>
		/// <param name="serialNumber">Serial number</param>
		/// <returns><see cref="Results"/>object indicating whether command was successful or failed.</returns>
		Results SetSerialNumber(string serialNumber);
		/// <summary>
		/// Check that the serial number passed as argument is valid. Does not actually query from the device.
		/// </summary>
		/// <param name="serialnumber">Serial number to verify</param>
		/// <returns><see cref="Results"/>object indicating whether command was successful or failed</returns>
		Results VerifySerialNumber(string serialnumber);
		/// <summary>
		/// Gets/Sets device ID as defined in device manager.
		/// </summary>
		string DeviceId{get;set;}

		#endregion
	}
}
