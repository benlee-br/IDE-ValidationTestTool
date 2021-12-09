using System;
using System.Collections;

namespace BioRad.Common.Devices
{
	#region Documentation Tags
	/// <summary>
	/// Typesafe collection of <see cref="Device"/> objects.
	/// </summary>
	/// <remarks>
	/// Based on hash table.
	/// <para>TODO: Move some functionality here from DeviceManager.</para>
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Lisa von Schlegell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="conformancereview">Conformance review:</item>
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
	///			<item name="vssfile">$Workfile: DeviceCollection.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Devices/DeviceCollection.cs $</item>
	///			<item name="vssrevision">$Revision: 8 $</item>
	///			<item name="vssauthor">Last Check-in by: $Author: Pwalse $</item>
	///			<item name="vssdate">$Date: 1/09/07 10:26a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public partial class DeviceCollection : IDisposable, IEnumerable, ICloneable
	{
		#region Contained Classes
		/// <summary>
		/// Enumerates over DeviceCollection.
		/// </summary>
		/// <remarks>Custom enumerator allows enumeration of <see cref="Device"/> objects
		/// rather than the dictionary elements handled by the hashtable enumerator.</remarks>
		private class DeviceCollectionEnumerator : IEnumerator
		{
			#region Member Data
			private IDictionaryEnumerator m_DevicesEnumerator;
			#endregion
	
			#region Accessors
			#region IEnumerator Accessors
			object IEnumerator.Current
			{
				get
				{
					return m_DevicesEnumerator.Value;
				}
			}
			#endregion
			#endregion

			#region Constructors and Destructor
			/// <summary>
			/// Constructor initializes a DeviceCollection enumerator using the
			/// dictionary enumerator of the underlying data store.
			/// </summary>
			/// <param name="enumerator"></param>
			public DeviceCollectionEnumerator ( IDictionaryEnumerator enumerator)
			{
				// This enumerator will defer to the dictionary enumerator
				m_DevicesEnumerator = enumerator;
			}
			#endregion

			#region Methods
			#region IEnumerator Members
			public void Reset()
			{
				m_DevicesEnumerator.Reset();
			}

			public bool MoveNext()
			{
				return m_DevicesEnumerator.MoveNext();
			}
			#endregion
			#endregion
		}
		#endregion

		#region Member Data
		private Hashtable m_Devices = new Hashtable();
		#endregion

		#region Accessors
		/// <summary>
		/// Returns number of devices in the collection.
		/// </summary>
		/// <returns>number of devices in the collection</returns>
		public int Count
		{
			get { return m_Devices.Count; }
		}

		/// <summary>
		/// Indexer to the devices collection keyed by device ID. 
		/// </summary>
		/// <remarks>Device may be overwritten in collection.Set with null value is ignored. 
		/// Returns null if device not found on get.</remarks>
		public Device this[string deviceID]
		{
			get { return (Device) m_Devices[deviceID];}
			set { if (value != null) m_Devices[deviceID] = value; }
		}

		/// <summary>
		/// Indexer to a collection of devices of a specific type.
		/// </summary>
		public DeviceCollection this[DeviceTypes type]
		{
			get
			{
				ArrayList array = new ArrayList();
				foreach (Device device in m_Devices.Values)
				{
					if (device.DeviceType == type)
						array.Add(device);
				}
				return new DeviceCollection(array);
			}
		}
		#endregion

		#region Delegates and Events
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Default constructor creates an empty device collection.
		/// </summary>
		public DeviceCollection()
		{
		}

		/// <summary>
		/// Parameterized constructor creates a device collection containing the
		/// devices in the given array.
		/// </summary>
		/// <remarks>If multiple devices have the same device ID, only one will end up in
		/// the collection.</remarks>
		/// <param name="devices"></param>
		public DeviceCollection(Device [] devices) : this ((IEnumerable) devices)
		{
		}

		/// <summary>
		/// Copy constructor creates a device collection containing the devices in
		/// the given device collection.
		/// </summary>
		/// <param name="devices">collection of devices</param>
		public DeviceCollection(DeviceCollection devices) : this((IEnumerable) devices)
		{
		}

		/// <summary>
		/// Private constructor creates a new device collection from
		/// any enumerable collection of devices.
		/// </summary>
		/// <param name="devices"></param>
		private DeviceCollection(IEnumerable devices)
		{
			foreach (Device device in devices)
			{
				this[device.DeviceId] = device;
			}
		}
		#endregion

		#region Methods
		#region IDisposable Members
		/// <summary>
		/// Dispose allows memory used by hash table to be reclaimed
		/// at the next garbage collection.
		/// </summary>
		public void Dispose()
		{
			if (m_Devices != null) m_Devices.Clear();
			m_Devices = null;
		}
		#endregion

		#region IEnumerable Members
		/// <summary>
		/// Supports enumeration over contained device objects.
		/// </summary>
		/// <returns>Enumerator for a collection of Device objects.</returns>
		public IEnumerator GetEnumerator()
		{
			// Create an enumerator based on an enumerator for the hashtable
			return new DeviceCollectionEnumerator(m_Devices.GetEnumerator());
		}
		#endregion

		#region ICloneable Members
		/// <summary>
		/// Returns a duplicate collection.
		/// </summary>
		/// <remarks>Contained <see cref="Device"/> objects aren't cloned.</remarks>
		/// <returns>Duplicate collection.</returns>
		public object Clone()
		{
			return new DeviceCollection(this);
		}
		#endregion

		/// <summary>
		/// Add device to collection.
		/// </summary>
		/// <remarks>If device already exists in the collection, 
		/// throws <see cref="ArgumentException"/></remarks>
		/// <param name="device">Device to add. If null, ignored.</param>
		public void Add(Device device)
		{
			if (device != null)
			{
				m_Devices.Add(device.DeviceId, device);
			}
		}

		/// <summary>
		/// Check collection for given device.
		/// </summary>
		/// <remarks>Any device object in collection with matching device ID will produce a match.</remarks>
		/// <param name="device"></param>
		/// <returns>true if device is in collection, false if device is null or not in collection</returns>
		public bool Contains(Device device)
		{
			if (device != null)
				return m_Devices.Contains(device.DeviceId);
			else return false;
		}

		/// <summary>
		/// Check collection for device with given device id.
		/// </summary>
		/// <param name="deviceID"></param>
		/// <returns>true if device is in collection, false if device is null or not in collection</returns>
		public bool Contains(string deviceID)
		{
			return m_Devices.Contains(deviceID);
		}

		/// <summary>
		/// Check collection for device of given device type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns>true if any device of given type is in collection, otherwise false.</returns>
		public bool Contains(DeviceTypes type)
		{
			bool found = false;
			foreach (Device device in m_Devices.Values)
			{
				if (device.DeviceType == type)
				{
					found = true;
					break;
				}
			}
			return found;
		}

		/// <summary>
        /// Check collection for device of given device type and simulation mode.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="emulated">True to check for emulated device, false to check for
		/// non-emulated device.</param>
        /// <returns>true if any device of given type and simulation mode is in collection, 
		/// otherwise false.</returns>
		public bool Contains(DeviceTypes type, bool emulated)
		{
			bool found = false;
			foreach (Device device in m_Devices.Values)
			{
				if ((device.DeviceType == type) && (device.IsEmulator == emulated))
				{
					found = true;
					break;
				}
			}
			return found;
		}

		/// <summary>
        /// Get the default device for the given device type and simulation mode.
		/// </summary>
		/// <remarks>Devices can be marked as "default" when configured, implying that
		/// the application will select those devices automatically without user intervention.
		/// <para>If more than one device of a given type is configured as default and
        /// with the same simulation mode, this call will return only one of those devices.
		/// The specific device is undetermined.
		/// </para></remarks>
		/// <param name="type">Selects device type (camera, base unit, etc)</param>
		/// <param name="emulated">True to get an emulated device.</param>
		/// <returns>Specified default device or null if none configured.</returns>
		public Device DefaultDevice(DeviceTypes type, bool emulated)
		{
			Device foundDevice = null;
			foreach (Device device in m_Devices.Values)
			{
				if (device.IsDefault && (device.DeviceType == type) && (device.IsEmulator == emulated))
				{
					foundDevice = device;
					break;
				}
			}
			return foundDevice;
		}

		/// <summary>
		/// Gets a collection of all configured default devices, both emulated and
		/// non-emulated.
		/// </summary>
		/// <remarks>Devices can be marked as "default" when configured, implying that
		/// the application will select those devices automatically without user intervention.</remarks>
		/// <returns>Collection of all configured default devices.</returns>
		public DeviceCollection DefaultDevices()
		{
			ArrayList array = new ArrayList();
			foreach (Device device in m_Devices.Values)
			{
				if (device.IsDefault)
					array.Add(device);
			}
			return new DeviceCollection(array);
		}

		/// <summary>
        /// Gets a collection of all configured default devices for the given simulation
		/// mode.
		/// </summary>
		/// <remarks>Devices can be marked as "default" when configured, implying that
		/// the application will select those devices automatically without user intervention.</remarks>
		/// <param name="emulated">True to get a collection of emulated devices, false
		/// to get a collection of "real" devices.</param>
		/// <returns>Collection of all configured default devices for the given
        /// simulation mode.</returns>
		public DeviceCollection DefaultDevices(bool emulated)
		{
			ArrayList array = new ArrayList();
			foreach (Device device in m_Devices.Values)
			{
				if (device.IsDefault && (device.IsEmulator == emulated))
					array.Add(device);
			}
			return new DeviceCollection(array);
		}

		/// <summary>
		/// Gets a collection of all configured devices for the given device type and 
        /// simulation mode.
		/// </summary>
		/// <remarks>SRS363 4.1 MyiQ 2.0 Hardware Support.</remarks>
		/// <param name="type">Selects device type (camera, base unit, etc)</param>
		/// <param name="emulated">True to get a collection of emulated devices, false
		/// to get a collection of "real" devices.</param>
		/// <returns>Collection of all configured devices for the given device and 
        /// simulation mode.</returns>
		public DeviceCollection AllDevices(DeviceTypes type, bool emulated)
		{
			ArrayList array = new ArrayList();
			foreach (Device device in m_Devices.Values)
			{
				if (device.DeviceType.Equals(type) && (device.IsEmulator.Equals(emulated)))
					array.Add(device);
			}
			return new DeviceCollection(array);
		}

		/// <summary>
		/// Remove device from collection.
		/// </summary>
		/// <param name="device">Device to remove. If null or not in collection,
		/// ignored.</param>
		public void Remove(Device device)
		{
			if ((device != null) && m_Devices.Contains(device.DeviceId))
				m_Devices.Remove(device.DeviceId);
		}

		/// <summary>
		/// Gets device collection as an array.
		/// </summary>
		/// <returns>Array of device objects.</returns>
		public Device[] ToArray()
		{
			ArrayList array = new ArrayList(m_Devices.Values);
			return (Device[]) array.ToArray(typeof(Device));
		}
		#endregion

	}
}
