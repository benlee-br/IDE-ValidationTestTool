using System;
using System.Collections;
using System.Reflection;
using System.Text;

using BioRad.Common;
using BioRad.Common.Reflection;

namespace BioRad.Common.Devices
{
	#region Documentation Tags
	/// <summary>
	/// DeviceManager class provides a single point of access for the instantiation of device objects.
	/// Device objects are objects that support a specific hardware device. 
	/// </summary>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Pramod Walse, Ralph Ansell</item>
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
	///			<item name="vssfile">$Workfile: DeviceManager.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Devices/DeviceManager.cs $</item>
	///			<item name="vssrevision">$Revision: 35 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class DeviceManager
	{

		#region Member Data
		/// <summary>
		/// List of Device objects from service.
		/// </summary>
		private ArrayList m_Devices = null;
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Initializes a new instance of the DeviceManager class with list of devices.
		/// </summary>
		public DeviceManager(Device[] devices)
		{
			if ( devices != null )
			{
				m_Devices = new ArrayList();
				foreach(Device device in devices)
				{
					m_Devices.Add(device);
				}
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Determines if specified device ID is a configured.
		/// </summary>
		/// <param name="deviceId">Device ID of device.</param>
		/// <returns>True if device is configured.</returns>
		private bool IsConfiguredDevice(string deviceId)
		{
			Device currentDevice = GetDeviceById(deviceId);
			if(currentDevice == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		/// <summary>
		/// Get list of all configured devices.
		/// </summary>
		/// <returns>Array of Device objects.</returns>
		public Device[] GetAllDevices()
		{
			if (m_Devices == null)
			{
				return null;
			}
			else
			{
				return (Device[]) m_Devices.ToArray(typeof(Device));
			}
		}
		/// <summary>
		/// Get list of default devices(includes both emulated and non-emulated).
		/// </summary>
		/// <returns>Array of Device objects.</returns>
		public Device[] GetDefaultDevices()
		{
			if (m_Devices == null)
			{
				return null;
			}
			else
			{
				ArrayList returnDevices = new ArrayList();
				Device[] devices = (Device[]) m_Devices.ToArray(typeof(Device));
				foreach(Device device in devices)
				{
					if(device.IsDefault)
					{
						returnDevices.Add(device);
					}
				}
				return (Device[]) returnDevices.ToArray(typeof(Device));
			}
		}
		/// <summary>
		/// Get list of all supported devices for current device.
		/// </summary>
		/// <param name="deviceId">Current Device ID</param>
		/// <returns>Array of Device objects.</returns>
		public Device[] GetSupportedDevicesByDevice(string deviceId)
		{
			Device currentDevice = null;
			ArrayList deviceInfoList = new ArrayList();
			
			Device[] devices = (Device[]) m_Devices.ToArray(typeof(Device));
			currentDevice = GetDeviceById(deviceId);

			if(currentDevice != null)
			{
				string supportedDevices = currentDevice.SupportedDevices;
				if(!supportedDevices.Equals(""))
				{	
					string seperator = ",";
					char [] delimiter = seperator.ToCharArray();
					string[] supportedDevicesList = supportedDevices.Split(delimiter);
					for(int i=0;i<supportedDevicesList.Length; i++)
					{
						string id = supportedDevicesList[i].ToString();
						foreach(Device device in devices)
						{
							if(string.Compare(device.DeviceId, id, true).Equals(0))
							{
								deviceInfoList.Add(device);
								break;
							}
						}
					}
				}
			}
			return (Device[]) deviceInfoList.ToArray(typeof(Device));
		}
		/// <summary>
		/// Get an object which represents the device specified by the argument. 
		/// </summary>
		/// <example>
		/// It is recommended that you check the object returned for 
		/// support of the interface desired as follows:
		/// <code>
		/// object obj = DeviceManager.Instance().GetDeviceObject("2001");
		///	if ( obj != null )
		///	{
		///		Debug.Assert(obj is IMyInterface);
		///		if ( obj is IMyInterface )
		///		{
		///			IMyInterface = (IMyInterface)obj;
		///		}
		///	}
		///	</code>
		/// </example>	
		/// <remarks>
		/// Instantiate a object for a specific device ID. 
		/// </remarks>
		/// <param name="deviceId">Device ID of device to instantiate.</param>
		/// <returns>
		/// Instantiated device object or null if device ID not configured.
		/// </returns>
		// todo: make thread safe???
		public object GetDeviceObject(string deviceId)
		{
			object theDeviceObject = null;

			if ( deviceId != null )
			{
				Device device = GetDeviceById(deviceId);
				if ( device != null )
				{
					ReflectionUtil ru = new ReflectionUtil();
					try
					{
						theDeviceObject = ru.InstantiateObject(device.ClassName, device.AssemblyName);
					}
					catch(Exception ex)
					{
						StringBuilder sb = new StringBuilder();
						sb.Append("Device object could not be created for assembly = ");
						sb.Append(device.AssemblyName);
						sb.Append(" and class = ");
						sb.Append(device.ClassName);
						if ( ex.InnerException != null )
						{
							sb.Append("\n\n");
							sb.Append(ex.InnerException.Message);
						}
						sb.Append("\n\n");
						sb.Append(ex.StackTrace);
						throw new ApplicationException(sb.ToString());	
					}
				}
			}

			return theDeviceObject;
		}
		/// <summary>
		/// Get an object which represents the device specified by the argument. 
		/// </summary>
		/// <param name="device">Device object to instantiate.</param>
		/// <returns>Instantiated device object or null if device not configured.</returns>
		static public object GetDeviceObject(Device device)
		{
			object theDeviceObject = null;

			if ( device != null )
			{
				if ( device != null )
				{
					ReflectionUtil ru = new ReflectionUtil();
					try
					{
						theDeviceObject = ru.InstantiateObject(device.ClassName, device.AssemblyName);
					}
					catch(Exception ex)
					{
						StringBuilder sb = new StringBuilder();
						sb.Append("Device object could not be created for assembly = ");
						sb.Append(device.AssemblyName);
						sb.Append(" and class = ");
						sb.Append(device.ClassName);
						if ( ex.InnerException != null )
						{
							sb.Append("\n\n");
							sb.Append(ex.InnerException.Message);
						}
						sb.Append("\n\n");
						sb.Append(ex.StackTrace);
						throw new ApplicationException(sb.ToString());	
					}
				}
			}

			return theDeviceObject;
		}
		/// <summary>
		/// Gets device from list by its Id.
		/// </summary>
		/// <param name="deviceId">Device Id</param>
		/// <returns>Device object</returns>
		public Device GetDeviceById(string deviceId)
		{
			Device[] devices = (Device[]) m_Devices.ToArray(typeof(Device));
			foreach(Device device in devices)
			{
				if(string.Compare(device.DeviceId, deviceId, true).Equals(0))
				{
					return device;
				}
			}
			return null;
		}
		#endregion
	}
}
