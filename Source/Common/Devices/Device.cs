using System;
using System.Collections;
using System.Text;
using BioRad.Common.Services;

namespace BioRad.Common.Devices
{
	#region Documentation Tags
	/// <summary>
	/// This holds information about device. The properties correpond to XML 
	/// configuration file.
	/// </summary>
	/// <remarks>
	/// Generic representation of device.
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
	///			<item name="vssfile">$Workfile: Device.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Devices/Device.cs $</item>
	///			<item name="vssrevision">$Revision: 22 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ssarup $</item>
	///			<item name="vssdate">$Date: 3/20/07 5:21a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	[Serializable]
	public partial class Device : ICloneable
	{
	    #region Member Data
		/// <summary>
		/// Device Id
		/// </summary>
		private string m_DeviceId;
		/// <summary>
		/// Device Key
		/// </summary>
		private string m_DeviceKey;
		/// <summary>
		/// Type of the device.
		/// </summary>
		private DeviceTypes m_DeviceType;
		/// <summary>
		/// Device Emulator flag.
		/// </summary>
		private bool m_IsEmulator;
		/// <summary>
		/// Device default flag. Serves to get list of default devices.
		/// </summary>
		private bool m_IsDefault;
		/// <summary>
		/// Device name.
		/// </summary>
		private string m_Name;
		/// <summary>
		/// Device display name.
		/// </summary>
		private string m_DisplayName;
		/// <summary>
		/// Description of the device.
		/// </summary>
		private string m_Description;
		/// <summary>
		/// Device location
		/// </summary>
		private string m_Location;
		/// <summary>
		/// Device assembly name.
		/// </summary>
		private string m_AssemblyName;
		/// <summary>
		/// Device class name.
		/// </summary>
		private string m_ClassName;
		/// <summary>
		/// Device imterface name.
		/// </summary>
		private string m_InterfaceName;
		/// <summary>
		/// List of other devices supported for the current device.
		/// </summary>
		private string m_SupportedDevices;
		/// <summary>
		/// Information about device configured from Device service.
		/// </summary>
		private object m_DeviceConfigInfo;
		/// <summary>The device's serial number.</summary>
		private string m_SerialNumber;
		/// <summary>The device's firmware version.</summary>
		private string m_FirmwareVersion;
		/// <summary>The device's software version.</summary>
		private string m_SoftwareVersion;
		/// <summary>The device's required firmware version(from configuration).</summary>
		private string m_RequiredFirmwareVersion;
		/// <summary>The model name of the device</summary>
		private string m_Model;
		#endregion

        #region Accessors
		/// <summary>
		/// Gets/Sets device type.
		/// </summary>
		public DeviceTypes DeviceType
		{
			get
			{
				return m_DeviceType;
			}
			set
			{
				m_DeviceType = value;
			}
		}
		/// <summary>
		/// Gets/Sets device ID.
		/// </summary>
		public string DeviceId
		{
			get
			{
				return m_DeviceId;
			}
			set
			{
				m_DeviceId = value;
			}
		}
		/// <summary>
		/// Gets/Sets device Key.
		/// </summary>
		public string DeviceKey
		{
			get
			{
				return m_DeviceKey;
			}
			set
			{
				m_DeviceKey = value;
			}
		}
		/// <summary>
		/// Gets/Sets if device is emulator or not.
		/// </summary>
		public bool IsEmulator
		{
			get
			{
				return m_IsEmulator;
			}
			set
			{
				m_IsEmulator = value;
			}
		}
		/// <summary>
		/// Device default flag. Serves to get list of default devices.
		/// </summary>
		public bool IsDefault
		{
			get
			{
				return m_IsDefault;
			}
			set
			{
				m_IsDefault = value;
			}
		}
		/// <summary>
		/// Gets/Sets device name.
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}
		/// <summary>
		/// Gets/Sets device display name.
		/// </summary>
		public string DisplayName
		{
			get
			{
				return m_DisplayName;
			}
			set
			{
				m_DisplayName = value;
			}
		}
		/// <summary>
		/// Gets/Sets device description.
		/// </summary>
		public string Description
		{
			get
			{
				return m_Description;
			}
			set
			{
				m_Description = value;
			}
		}
		/// <summary>
		/// Gets/Sets device Location.
		/// </summary>
		public string Location
		{
			get
			{
				return m_Location;
			}
			set
			{
				m_Location = value;
			}
		}
		/// <summary>
		/// Gets/Sets device Assembly name.
		/// </summary>
		public string AssemblyName
		{
			get
			{
				return m_AssemblyName;
			}
			set
			{
				m_AssemblyName = value;
			}
		}
		/// <summary>
		/// Gets/Sets device Class name.
		/// </summary>
		public string ClassName
		{
			get
			{
				return m_ClassName;
			}
			set
			{
				m_ClassName = value;
			}
		}
		/// <summary>
		/// Gets/Sets device interface name.
		/// </summary>
		public string InterfaceName
		{
			get
			{
				return m_InterfaceName;
			}
			set
			{
				m_InterfaceName = value;
			}
		}
		/// <summary>
		/// Gets/Sets List of other devices supported for the current device.
		/// </summary>
		public string SupportedDevices
		{
			get
			{
				return m_SupportedDevices;
			}
			set
			{
				m_SupportedDevices = value;
			}
		}
		/// <summary>
		/// Information about device configured from Device service.
		/// </summary>
		public object DeviceConfigInfo
		{
			get
			{
				return m_DeviceConfigInfo;
			}
			set
			{
				m_DeviceConfigInfo = value;
			}
		}
		/// <summary>The device's serial number.</summary>
		public string SerialNumber
		{
			get { return this.m_SerialNumber;}
			set { this.m_SerialNumber = value;}
		}
		/// <summary>The device's firmware version.</summary>
		public string FirmwareVersion
		{
			get { return this.m_FirmwareVersion;}
			set { this.m_FirmwareVersion = value;}
		}
		/// <summary>The device's software version.</summary>
		public string SoftwareVersion
		{
			get { return this.m_SoftwareVersion;}
			set { this.m_SoftwareVersion = value;}
		}
		/// <summary>The device's required firmware version(from configuration).</summary>
		public string RequiredFirmwareVersion
		{
			get { return this.m_RequiredFirmwareVersion;}
			set { this.m_RequiredFirmwareVersion = value;}
		}
		/// <summary>The device's required firmware version(from configuration).</summary>
		public string Model
		{
			get { return this.m_Model;}
			set { this.m_Model = value;}
		}
	    #endregion

        #region Constructors and Destructor
		/// <summary>Empty Constructor</summary>
		public Device()
		{
			//Empty.
		}
        #endregion

		#region Methods
		/// <summary>Create a clone of the Device object.</summary>
		/// <returns>The cloned Device object.</returns>
		public object Clone()
		{
			Device clonedObject = (Device) this.MemberwiseClone();
			
			if((this.m_DeviceConfigInfo != null) &&(m_DeviceConfigInfo is ICloneable))
			{
					clonedObject.DeviceConfigInfo = ((ICloneable) m_DeviceConfigInfo).Clone();
			}
			return clonedObject;
		}

		/// <summary>Returns the member data names and values as a string.</summary>
		/// <returns>A string.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Id - ");
			sb.Append(this.DeviceId.ToString());
			sb.Append(". Name - ");
			sb.Append(this.Name);
			sb.Append(". DisplayName - ");
			sb.Append(this.DisplayName);
			sb.Append(". SerialNumber - ");
			sb.Append(this.SerialNumber);
			sb.Append(". SoftwareVersion - ");
			sb.Append(this.SoftwareVersion);
			sb.Append(". FirmwareVersion - ");
			sb.Append(this.FirmwareVersion);
			sb.Append(". AssemblyName - ");
			sb.Append(this.AssemblyName);
			sb.Append(". ClassName - ");
			sb.Append(this.ClassName);
			sb.Append(". Description - ");
			sb.Append(this.Description);
			sb.Append(". DeviceKey - ");
			sb.Append(this.DeviceKey);
			sb.Append(". InterfaceName - ");
			sb.Append(this.InterfaceName);
			sb.Append(". IsDefault - ");
			sb.Append(this.IsDefault);
			sb.Append(". IsEmulator - ");
			sb.Append(this.IsEmulator);
			sb.Append(". Location - ");
			sb.Append(this.Location);
			sb.Append(". RequiredFirmwareVersion - ");
			sb.Append(this.RequiredFirmwareVersion);
			sb.Append(". SupportedDevices - ");
			sb.Append(this.SupportedDevices);

			return sb.ToString();
		}
		/// <summary>Exports state of object as an XML string.</summary>
		/// <returns>XML string describing state of object data.</returns>
		public string ToXml()
		{
			return string.Empty;
		}
		/// <summary>
		/// Deserializes from an XML serialization.
		/// </summary>
		/// <param name="xml">the string which contains an appropriate XML fragment.</param>
		/// <returns>true if successful deserialization</returns>
		public bool FromXml(string xml)
		{
			return true;
		}
		#endregion
	}
}
