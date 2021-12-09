using System;
using System.Management;
using System.Collections.Generic;
using System.Linq;
namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>Uses WMI to query the system for hardware and software information. Returns the information
	/// as string that are always in English.</summary>
	/// <remarks>This class is for diagnostics purposes and should not be localized.</remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Shabnam</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href=""></see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: SystemHardwareSoftwareInformation.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/SystemHardwareSoftwareInformation.cs $</item>
	///			<item name="vssrevision">$Revision: 1 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ssarup $</item>
	///			<item name="vssdate">$Date: 12/03/07 3:33a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class SystemHardwareSoftwareInformation
	{
		#region Constants
		private const string c_InfoFormatString = "{0} : {1}";

		/// <summary>The type of information to retrieve.</summary>
		public enum InfoType : int
		{
			/// <summary>Unassigned value.</summary>
			Unassigned,
			/// <summary>Computer System</summary>
			ComputerSystem,
			/// <summary>Computer System Product.</summary>
			ComputerSystemProduct,
			/// <summary>Operating System</summary>
			OperatingSystem,
			/// <summary>The Page File settings.</summary>
			PageFileSetting,
			/// <summary>The page file usage.</summary>
			PageFileUsage,
			/// <summary>Physical Memory </summary>
			PhysicalMemory,
			/// <summary>Processor</summary>
			Processor,
			/// <summary>All available information.</summary>
			All
		}
        #endregion

        #region Methods
        /// <summary>
        /// Get list of connected Opus devices.
        /// </summary>
        /// <returns></returns>
        public static List<PropertyDataCollection> GetOpusDevices()
        {
            List<PropertyDataCollection> list = new List<PropertyDataCollection>();

            // Ports (COM & LPT ports)-class (guid: 4d36e978-e325-11ce-bfc1-08002be10318). 
            // Get all serial (COM)-ports you can see in the devicemanager
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\cimv2",
                "SELECT * FROM Win32_PnPEntity WHERE ClassGuid=\"{4d36e978-e325-11ce-bfc1-08002be10318}\""))
            {
                ManagementObjectCollection moc = searcher.Get();
                foreach (ManagementObject mo in moc)
                {
                    //System.Diagnostics.Debug.WriteLine("-----------------------------");
                    PropertyDataCollection properties = mo.Properties;

                    string deviceID = properties["DeviceID"].Value.ToString();
                    if (deviceID.IndexOf("BIO-RAD_CFX_OPUS") > 0)
                        list.Add(properties);

                    foreach (PropertyData devProperty in properties)
                    {
                        if (devProperty.Type == CimType.DateTime)
                        {
                            if (devProperty.Value != null)
                            {
                            }
                        }
                        else
                        {
                            //System.Diagnostics.Debug.WriteLine(" {0}----->{1}", devProperty.Name, devProperty.Value);
                        }
                    }
                }
            }

            return list;
        }
       
        /// <summary>Gets information about the computer system, operating system, physical memory 
        /// and processor.</summary>
        /// <returns>A collection of string containing the System information.</returns>
        public static List<string> GetInformation()
		{
			List<string> infoList = new List<string>();
			infoList.AddRange(GetComputerSystemInfo());
			infoList.Add(string.Empty);
			infoList.AddRange(GetComputerSystemProductInfo());
			infoList.Add(string.Empty);
			infoList.AddRange(GetOperatingSystemInfo());
			infoList.Add(string.Empty);
			infoList.AddRange(GetPageFileSettingInfo());
			infoList.Add(string.Empty);
			infoList.AddRange(GetPageFileUsageInfo());
			infoList.Add(string.Empty);
			infoList.AddRange(GetPhysicalMemoryInfo());
			infoList.Add(string.Empty);
			infoList.AddRange(GetProcessorInfo());
			infoList.Add(string.Empty);
			
			return infoList;
		}

		/// <summary>Gets information based on the InfoType.</summary>
		/// <param name="infoType">The type of information to get.</param>
		/// <returns>A collection of string containing the requested system information.</returns>
		public static List<string> GetInformation(InfoType infoType)
		{
			switch(infoType)
			{
				case InfoType.All:
					return GetInformation();
				case InfoType.ComputerSystem:
					return GetComputerSystemInfo();
				case InfoType.ComputerSystemProduct:
					return GetComputerSystemProductInfo();
				case InfoType.OperatingSystem:
					return GetOperatingSystemInfo();
				case InfoType.PageFileSetting:
					return GetPageFileSettingInfo();
				case InfoType.PageFileUsage:
					return GetPageFileUsageInfo();
				case InfoType.PhysicalMemory:
					return GetPhysicalMemoryInfo();
				case InfoType.Processor:
					return GetProcessorInfo();
				default:
					return null;
			}
		}

		/// <summary>Get information about the Operating System.</summary>
		/// <returns>A collection of string containing the Operating System information.</returns>
		private static List<string> GetOperatingSystemInfo()
		{
			List<string> infoList = new List<string>();
			infoList.Add("Operating System Information");
			infoList.Add(string.Empty);

			ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
			foreach (ManagementObject managementObject in objectSearcher.Get())
			{
				infoList.Add("Name - " + managementObject.ToString());
				infoList.Add(string.Empty);
				PropertyDataCollection collection = managementObject.Properties;
				PropertyData[] propertyDataCollection = new PropertyData[collection.Count];
				collection.CopyTo(propertyDataCollection, 0);
				foreach (PropertyData propertyData in propertyDataCollection)
				{
					infoList.Add(string.Format(c_InfoFormatString,
						propertyData.Name,
						(propertyData.Value == null) ? string.Empty : propertyData.Value.ToString()));
				}
			}

			return infoList;
		}
		/// <summary>Get information about the Physical Memory.</summary>
		/// <returns>A collection of string containing the Phystical Memory information.</returns>
		private static List<string> GetPhysicalMemoryInfo()
		{
			List<string> infoList = new List<string>();
			infoList.Add("Physical Memory Information");
			
			ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher("select * from Win32_PhysicalMemory");
			foreach (ManagementObject managementObject in objectSearcher.Get())
			{
				infoList.Add(string.Empty);
				infoList.Add("Name - " + managementObject.ToString());
				infoList.Add(string.Empty);
				PropertyDataCollection collection = managementObject.Properties;
				PropertyData[] propertyDataCollection = new PropertyData[collection.Count];
				collection.CopyTo(propertyDataCollection, 0);
				foreach (PropertyData propertyData in propertyDataCollection)
				{
					infoList.Add(string.Format(c_InfoFormatString,
						propertyData.Name,
						(propertyData.Value == null) ? string.Empty : propertyData.Value.ToString()));
				}
			}

			return infoList;
		}
		/// <summary>Get information about the Processor.</summary>
		/// <returns>A collection of string containing the Processor information.</returns>
		private static List<string> GetProcessorInfo()
		{
			List<string> infoList = new List<string>();
			infoList.Add("Processor Information");
			
			ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher("select * from Win32_Processor");
			foreach (ManagementObject managementObject in objectSearcher.Get())
			{
				infoList.Add(string.Empty); 
				infoList.Add("Name - " + managementObject.ToString());
				infoList.Add(string.Empty);
				PropertyDataCollection collection = managementObject.Properties;
				PropertyData[] propertyDataCollection = new PropertyData[collection.Count];
				collection.CopyTo(propertyDataCollection, 0);
				foreach (PropertyData propertyData in propertyDataCollection)
				{
					infoList.Add(string.Format(c_InfoFormatString,
						propertyData.Name,
						(propertyData.Value == null) ? string.Empty : propertyData.Value.ToString()));
				}
			}

			return infoList;
		}
		/// <summary>Get information about the Computer System.</summary>
		/// <returns>A collection of string containing the Computer System information.</returns>
		private static List<string> GetComputerSystemInfo()
		{
			List<string> infoList = new List<string>();
			infoList.Add("Computer System Information");
			
			ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
			foreach (ManagementObject managementObject in objectSearcher.Get())
			{
				infoList.Add(string.Empty);
				infoList.Add("Name - " + managementObject.ToString());
				infoList.Add(string.Empty);
				PropertyDataCollection collection = managementObject.Properties;
				PropertyData[] propertyDataCollection = new PropertyData[collection.Count];
				collection.CopyTo(propertyDataCollection, 0);
				foreach (PropertyData propertyData in propertyDataCollection)
				{
					infoList.Add(string.Format(c_InfoFormatString,
						propertyData.Name,
						(propertyData.Value == null) ? string.Empty : propertyData.Value.ToString()));
				}
			}

			return infoList;
		}
		/// <summary>Get information about the Computer System Product.</summary>
		/// <returns>A collection of string containing the Computer System Product information.</returns>
		private static List<string> GetComputerSystemProductInfo()
		{
			List<string> infoList = new List<string>();
			infoList.Add("Computer System Product Information");

			ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher("select * from Win32_ComputerSystemProduct");
			foreach (ManagementObject managementObject in objectSearcher.Get())
			{
				infoList.Add(string.Empty);
				infoList.Add("Name - " + managementObject.ToString());
				infoList.Add(string.Empty);
				PropertyDataCollection collection = managementObject.Properties;
				PropertyData[] propertyDataCollection = new PropertyData[collection.Count];
				collection.CopyTo(propertyDataCollection, 0);
				foreach (PropertyData propertyData in propertyDataCollection)
				{
					infoList.Add(string.Format(c_InfoFormatString,
						propertyData.Name,
						(propertyData.Value == null) ? string.Empty : propertyData.Value.ToString()));
				}
			}

			return infoList;
		}
		/// <summary>Get information about the Page File Setting.</summary>
		/// <returns>A collection of string containing the Page File Setting information.</returns>
		private static List<string> GetPageFileSettingInfo()
		{
			List<string> infoList = new List<string>();
			infoList.Add("Page File Setting Information");

			ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher("select * from Win32_PageFileSetting");
			foreach (ManagementObject managementObject in objectSearcher.Get())
			{
				infoList.Add(string.Empty);
				infoList.Add("Name - " + managementObject.ToString());
				infoList.Add(string.Empty);
				PropertyDataCollection collection = managementObject.Properties;
				PropertyData[] propertyDataCollection = new PropertyData[collection.Count];
				collection.CopyTo(propertyDataCollection, 0);
				foreach (PropertyData propertyData in propertyDataCollection)
				{
					infoList.Add(string.Format(c_InfoFormatString,
						propertyData.Name,
						(propertyData.Value == null) ? string.Empty : propertyData.Value.ToString()));
				}
			}

			return infoList;
		}
		/// <summary>Get information about the Page File Usage.</summary>
		/// <returns>A collection of string containing the Page File Usage information.</returns>
		private static List<string> GetPageFileUsageInfo()
		{
			List<string> infoList = new List<string>();
			infoList.Add("Page File Usage Information");

			ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher("select * from Win32_PageFileUsage");
			foreach (ManagementObject managementObject in objectSearcher.Get())
			{
				infoList.Add(string.Empty);
				infoList.Add("Name - " + managementObject.ToString());
				infoList.Add(string.Empty);
				PropertyDataCollection collection = managementObject.Properties;
				PropertyData[] propertyDataCollection = new PropertyData[collection.Count];
				collection.CopyTo(propertyDataCollection, 0);
				foreach (PropertyData propertyData in propertyDataCollection)
				{
					infoList.Add(string.Format(c_InfoFormatString,
						propertyData.Name,
						(propertyData.Value == null) ? string.Empty : propertyData.Value.ToString()));
				}
			}

			return infoList;
		}
		#endregion
	}
}
