using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Text;
using System.Diagnostics;
using System.Globalization;

using BioRad.Common;
using BioRad.Common.Services;
using BioRad.Common.Xml;
using BioRad.Common.Utilities;

namespace BioRad.Common.Devices
{
	#region Documentation Tags
	/// <summary>Configuration provider for the DeviceManagerService
	/// that uses an XML file to supply devices.
	/// </summary>
	/// <remarks>
	/// Reads XML config file to load the list of devices.
	/// XML File Name: DeviceManagerConfiguration.xml.
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
	///			<item name="vssfile">$Workfile: XmlDeviceConfig.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Devices/XmlDeviceConfig.cs $</item>
	///			<item name="vssrevision">$Revision: 39 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	[Serializable]
	public partial class XmlDeviceConfig:IConfigurationProvider
	{
		#region Member Data
		/// <summary>
		/// Stores Xml configuration file name for the devices.
		/// </summary>
		private string m_FileName;
		#endregion

		#region Accessors
		/// <summary>
		/// Gets/Sets Xml configuration file name for the devices.
		/// </summary>
		public string FileName
		{
			get{return m_FileName;}
			set{m_FileName = value;}
		}
		#endregion

		#region Delegates and Events
		#endregion

		#region Constructors and Destructor
		/// <summary>Empty Constructor.</summary>
		public XmlDeviceConfig()
		{
			//Empty.
		}
		#endregion

		#region Methods
		/// <summary>
		/// Gets configuration elements from the expected configuration file.
		/// </summary>
		/// <returns>Collection of devices configured from config file.</returns>
		public ICollection GetConfigurationElements()
		{
			ArrayList devices = new ArrayList();
			XmlReader reader = null;
			XmlDocument document = null;
			try
			{
				//Attach full path to file name.
				string fileName = this.FileName;
				FileInfo fileInfo = new FileInfo(fileName);
				if(!fileInfo.Exists)
				{
					StringBuilder sb = new StringBuilder();
					sb.Append(ApplicationPath.DirectoryPath);
					sb.Append(Path.DirectorySeparatorChar);
					sb.Append(fileName);
					fileName = sb.ToString();
				}
				m_FileName =  fileName;
				using (Stream fs = 
						   FileCryptor.GetInstance.DecryptFileContentsToStream(fileName))
				{
					reader = new XmlTextReader(fs);
					document = new XmlDocument();
					document.Load(reader);
					XmlNode docElement = document.DocumentElement;
					XmlNodeList elements = docElement.SelectNodes("/devices/device");
					foreach (XmlNode node in elements)
					{
						Device device = new Device();
						DeviceTypes deviceType = 
							(DeviceTypes) int.Parse(node.Attributes["type"].Value);
						device.DeviceType = deviceType;
						device.DeviceId = node.Attributes["id"].Value;
						device.DeviceKey = node.Attributes["key"].Value;
						device.IsEmulator = bool.Parse(node.Attributes["isemulator"].Value);
						device.IsDefault = bool.Parse(node.Attributes["isdefault"].Value);
						device.Name = node.Attributes["name"].Value;
						device.DisplayName = node.Attributes["displayname"].Value;
						device.Description = node.Attributes["description"].Value;
						device.Location = node.Attributes["location"].Value;
						device.AssemblyName = node.Attributes["assemblyname"].Value;
						device.ClassName = node.Attributes["classname"].Value;
						device.InterfaceName = node.Attributes["interfacename"].Value;
						device.SupportedDevices = node.Attributes["supporteddevices"].Value;
						device.RequiredFirmwareVersion = node.Attributes["requiredfirmwareversion"].Value;

						//Read in parameters into ConfigInfo classes.
						switch (deviceType)
						{
							case DeviceTypes.BaseUnit :
								BaseUnitConfigInfo baseUnitConfigInfo = null;
								if(node.HasChildNodes)
								{
									XmlNode paramsNode = node.SelectSingleNode("parameters");
									if(paramsNode != null)
									{
										baseUnitConfigInfo = new BaseUnitConfigInfo();
										// Localization - convert to invariant culture
										baseUnitConfigInfo.PollingRate = new TimeType(
											StringUtility.StringToDouble
											(paramsNode.Attributes["QueryInstrumentEveryMilliseconds"].Value),
											TimeType.Units.MilliSeconds, double.MinValue, double.MaxValue, 10);
									}
								}
								device.DeviceConfigInfo = baseUnitConfigInfo;
								break;
							case DeviceTypes.Camera :
								CameraConfigInfo cameraConfigInfo = null;
								if(node.HasChildNodes)
								{
									XmlNode paramsNode = node.SelectSingleNode("parameters");
									if(paramsNode != null)
									{
										cameraConfigInfo = PopulateCameraConfigInfo(paramsNode);
									}
								}
								//Assign DeviceConfigInfo 
								device.DeviceConfigInfo = cameraConfigInfo;
								break;
							case DeviceTypes.Filter :
								//Implement when ready.
								break;
							case DeviceTypes.BarCodeReader :
								//Implement when ready.
								break;
						}
						//Add configured device
						devices.Add(device);
					}
				}
			}
			finally
			{
				// always make sure file is closed
				if(reader != null)
				{
					reader.Close();
				}
				document = null;
			}
			return devices;
		}

		/// <summary>
		/// Populates Camera Configuration Info object.
		/// </summary>
		/// <param name="paramsNode">Current Device node</param>
		private CameraConfigInfo PopulateCameraConfigInfo(XmlNode paramsNode)
		{
			CameraConfigInfo cameraConfigInfo = new CameraConfigInfo();
			XmlNode node = null;
			// Single node params
			// Localization - convert to invariant culture
			cameraConfigInfo.DelayTime = new TimeType(
				StringUtility.StringToDouble
				(XMLUtility.ReadAttribute(paramsNode, "delaytime")),
				TimeType.Units.MilliSeconds, double.MinValue, double.MaxValue, 10);
			cameraConfigInfo.IntervalTime = new TimeType(
				StringUtility.StringToDouble
				(XMLUtility.ReadAttribute(paramsNode, "intervaltime")),
				TimeType.Units.MilliSeconds, double.MinValue, double.MaxValue, 10);
			cameraConfigInfo.HorizontalBinning = 
				int.Parse(XMLUtility.ReadAttribute(paramsNode, "horizontalbinning"));
			cameraConfigInfo.VerticalBinning = 
				int.Parse(XMLUtility.ReadAttribute(paramsNode, "verticalbinning"));
			cameraConfigInfo.SaturatedPixelCountThreshold = 
				int.Parse(XMLUtility.ReadAttribute(paramsNode, "saturatedpixelcountthreshold"));
			cameraConfigInfo.WellCountSaturateMargin = 
				int.Parse(XMLUtility.ReadAttribute(paramsNode, "wellcountsaturatemargin"));
			cameraConfigInfo.SaturatedWellFractionBoost = 
				float.Parse(XMLUtility.ReadAttribute(paramsNode, "saturatedwellfractionboost"), CultureInfo.InvariantCulture);
			cameraConfigInfo.SaturatedWellFractionElimination = 
				float.Parse(XMLUtility.ReadAttribute(paramsNode, "saturatedwellfractionelimination"), CultureInfo.InvariantCulture);
			cameraConfigInfo.SaturatedWellFractionThreshold = 
				float.Parse(XMLUtility.ReadAttribute(paramsNode, "saturatedwellfractionthreshold"), CultureInfo.InvariantCulture);
			cameraConfigInfo.TimeOut =
				int.Parse(XMLUtility.ReadAttribute(paramsNode, "timeout"));
			cameraConfigInfo.ImageTransferTime = 
				int.Parse(XMLUtility.ReadAttribute(paramsNode, "imagetransfertime"));
			cameraConfigInfo.RequiredWarmupTime = int.Parse
				(XMLUtility.ReadAttribute(paramsNode, "requiredwarmuptime"));
			cameraConfigInfo.LampWarning = int.Parse
				(XMLUtility.ReadAttribute(paramsNode, "lampwarning"));
			cameraConfigInfo.LampError = int.Parse(paramsNode.Attributes["lamperror"].Value);
			cameraConfigInfo.TransferMode = int.Parse
				(XMLUtility.ReadAttribute(paramsNode, "transfermode"));

			// Bias Levels
			node = paramsNode.SelectSingleNode("biaslevels");
			ArrayList biasLevels = new ArrayList();
			foreach(XmlNode childNode in node.ChildNodes)
			{
				BiasLevel biasLevel = new BiasLevel();
				biasLevel.Bias = int.Parse(childNode.InnerText);
				biasLevels.Add(biasLevel);
			}
			cameraConfigInfo.BiasLevels = (BiasLevel[]) 
				biasLevels.ToArray(typeof(BiasLevel));
			// Gain Levels
			node = paramsNode.SelectSingleNode("gainlevels");
			ArrayList gainLevels = new ArrayList();
			foreach(XmlNode childNode in node.ChildNodes)
			{
				GainLevel gainLevel = new GainLevel();
				gainLevel.Gain = int.Parse(childNode.InnerText);
				gainLevels.Add(gainLevel);
			}
			cameraConfigInfo.GainLevels = (GainLevel[]) 
				gainLevels.ToArray(typeof(GainLevel));
			// Exp Settings.
			node = paramsNode.SelectSingleNode("exposuresettings");
			if(node != null)
			{
				ArrayList exposureSettings = new ArrayList();
				foreach(XmlNode childNode in node.ChildNodes)
				{
					ExposureSettings singleSettings = new ExposureSettings();
					singleSettings.Bias = int.Parse
						(XMLUtility.ReadAttribute(childNode, "bias"));
					// Localization - convert to invariant culture
					singleSettings.ExposureTime = new TimeType(
						StringUtility.StringToDouble
						(XMLUtility.ReadAttribute(childNode, "time")), 
						TimeType.Units.MilliSeconds,
						double.MinValue, double.MaxValue, 10);
					singleSettings.Gain = int.Parse(XMLUtility.ReadAttribute
						(childNode, "gain"));
					singleSettings.IsDefault = bool.Parse
						(XMLUtility.ReadAttribute(childNode, "default"));
					if(singleSettings.IsDefault)
					{
						cameraConfigInfo.ExposureInitialSetting = singleSettings;
					}
					//Always set to true
					singleSettings.IsValidExposureTime = true;
					// Localization - convert to invariant culture
					singleSettings.NormalizationFactor = 
						StringUtility.StringToDouble(XMLUtility.ReadAttribute(childNode, 
						"normalizationfactor"));
					exposureSettings.Add(singleSettings);
				}
				cameraConfigInfo.ExposureAllSettings = (ExposureSettings[]) 
					exposureSettings.ToArray(typeof(ExposureSettings));
			}
			// Algorithms.
			node = paramsNode.SelectSingleNode("algorithms");
			if(node != null)
			{
				ArrayList list = new ArrayList();

				foreach(XmlNode childNode in node.ChildNodes)
				{
					Algorithms a = new Algorithms();

					a.Type = XMLUtility.ReadAttribute(childNode, "type");
					a.IsDefault = bool.Parse
						(XMLUtility.ReadAttribute(childNode, "default"));
					a.Displayname = XMLUtility.ReadAttribute(childNode, "displayname");
					a.Classname = XMLUtility.ReadAttribute(childNode, "classname");
					a.Assemblyname = XMLUtility.ReadAttribute(childNode, "assemblyname");
					a.Interfacename = XMLUtility.ReadAttribute(childNode, "interfacename");
					a.Description = XMLUtility.ReadAttribute(childNode, "description");

					if(string.Compare(a.Type, "exposurecalculator", true).Equals(0))
					{
						a.IsExposureCalculator = true;
					}
					else
					{
						a.IsExposureCalculator = false;
					}

					list.Add(a);
				}

				cameraConfigInfo.AlgorithmsAllSettings = 
					(Algorithms[]) list.ToArray(typeof(Algorithms));

				// supported filter positions
				node = paramsNode.SelectSingleNode("filterpositions");
				cameraConfigInfo.SetFilterPositions(node.InnerText);
			}
			return cameraConfigInfo;
		}
		#endregion
	}
}
