using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Text;
using BioRad.Common.ApplicationExceptions;
using BioRad.Common.DiagnosticsLogger;
using BioRad.Common.Services;

namespace BioRad.Common.Devices
{
	#region Documentation Tags
	/// <summary>
	/// <para>
	/// This class is an implementation of IService from service provider.
	/// The Device manager service provides list of current devices supported in addition to 
	/// instantiating objects to create the instance of a current device or device emulator.
	/// </para>
	/// <para>
	/// This service does some special caching to support its business methods. Items 
	/// will be usually be stored in more than one internal structure (such as a hash
	/// table) so that they can be available upon request, for different requests.
	/// </para>
	/// </summary>
	/// <remarks>
	/// Since this class is internal, it is wrapped by 
	/// <see cref="DeviceManager">DeviceManager</see> class. 
	/// All public Methods are available from 
	/// <see cref="DeviceManager">DeviceManager</see> class. The usage is 
	/// defined in <see cref="DeviceManager">DeviceManager</see> class.
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
	///			<item name="vssfile">$Workfile: DeviceManagerService.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Devices/DeviceManagerService.cs $</item>
	///			<item name="vssrevision">$Revision: 27 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class DeviceManagerService : AbstractService
	{
		#region Constants
        #endregion

        #region Member Data
		/// <summary>
		/// ArrayList of all current devices configured for the service.
		/// </summary>
		private ArrayList m_Devices;
		/// <summary>
		/// Hashtable used to group devices type for faster
		/// retrieval by type.
		/// </summary>
		private Hashtable m_DevicesByDeviceType;
        #endregion

        #region Accessors
        #endregion

		#region Delegates and Events
		#endregion

        #region Constructors and Destructor
		/// <summary>Empty Constructor.</summary>
		public DeviceManagerService()
		{
			//Empty.
		}
        #endregion

        #region Methods
		/// <summary>
		/// Loads configuration elements for the service into whatever mechanisms
		/// the service chooses to use for caching those elements.
		/// For this service, the configuration elements supplied should be an array
		/// of devices.
		/// </summary>
		/// <param name="configurationElements"></param>
		public override void Load(ICollection configurationElements)
		{
			// initialize cached collections
			m_Devices = new ArrayList();
			m_DevicesByDeviceType = new Hashtable();
			// iterate through config elements, treating each as a device
			foreach (object item in configurationElements)
			{
				if (!(item is Device))
				{
					throw new ServiceLoadException(this,
                        StringUtility.FormatString(Properties.Resources.ConfigElementNotADevice));
				}
				// cast and store in cached collections
				Device deviceItem = (Device) item;
				m_Devices.Add(deviceItem );
				StoreDeviceByDeviceType(deviceItem);
			}
		}

		/// <summary>
		/// Private method that properly handles grouping devices by type in
		/// internal hashtable.
		/// </summary>
		/// <param name="item">Device item.</param>
		private void StoreDeviceByDeviceType(Device item)
		{
			ArrayList itemsByType = (ArrayList) m_DevicesByDeviceType[item.DeviceType];
			// if there is no list of this device by type, create it and add
			// it to the hashtable.
			if (itemsByType == null)
			{
				itemsByType = new ArrayList();
				m_DevicesByDeviceType.Add(item.DeviceType, itemsByType);
			}
			itemsByType.Add(item);
		}
		/// <summary>
		/// Business method that returns an array of available devices
		/// by device type. Uses the hashtable of devices by type.
		/// </summary>
		/// <param name="deviceType">The type of device to filter on.</param>
		/// <returns>An array of all current devices of the specified type.</returns>
		public Device[] GetDevicesByDeviceType(string deviceType)
		{
			if(m_DevicesByDeviceType.Count.Equals(0))
			{
				throw new LoggableApplicationException(this, DiagnosticSeverity.Serious,
					DiagnosticTag.IS,
                    StringUtility.FormatString(Properties.Resources.DevicesNotConfigured));
			}
			ArrayList itemsByType = (ArrayList) m_DevicesByDeviceType[deviceType];
			if (itemsByType == null)
			{
				throw new LoggableApplicationException(this, DiagnosticSeverity.Serious,
					DiagnosticTag.IS,
                    StringUtility.FormatString(Properties.Resources.DeviceWithTypeNotFound));
			}
			else
			{
				return (Device[]) itemsByType.ToArray(typeof(Device));
			}
		}
		/// <summary>
		/// Business method that returns an array of all devices.
		/// </summary>
		/// <returns>An array of all current devices of the specified type.</returns>
		public Device[] GetAllDevices()
		{
			if(m_Devices.Count.Equals(0))
			{
				throw new LoggableApplicationException(this, DiagnosticSeverity.Serious,
					DiagnosticTag.IS,
                    StringUtility.FormatString(Properties.Resources.DevicesNotConfigured));
			}
			return (Device[]) m_Devices.ToArray(typeof(Device));
		}
		/// <summary>
		/// Get list of default devices(includes both emulated and non-emulated).
		/// </summary>
		/// <returns>Array of Device objects.</returns>
		public Device[] GetDefaultDevices()
		{
			if (m_Devices == null)
			{
				throw new LoggableApplicationException(this, DiagnosticSeverity.Serious,
					DiagnosticTag.IS,
                    StringUtility.FormatString(Properties.Resources.DevicesNotConfigured));
			}
			if(m_Devices.Count.Equals(0))
			{
				throw new LoggableApplicationException(this, DiagnosticSeverity.Serious,
					DiagnosticTag.IS,
                    StringUtility.FormatString(Properties.Resources.DevicesNotConfigured));
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
			if (m_Devices == null)
			{
				throw new LoggableApplicationException(this, DiagnosticSeverity.Serious,
					DiagnosticTag.IS,
                    StringUtility.FormatString(Properties.Resources.DevicesNotConfigured));
			}
			if(m_Devices.Count.Equals(0))
			{
				throw new LoggableApplicationException(this, DiagnosticSeverity.Serious,
					DiagnosticTag.IS,
                    StringUtility.FormatString(Properties.Resources.DevicesNotConfigured));
			}
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
		/// Gets device from list by its Id.
		/// </summary>
		/// <param name="deviceId">Device Id</param>
		/// <returns>Device object</returns>
		private Device GetDeviceById(string deviceId)
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
