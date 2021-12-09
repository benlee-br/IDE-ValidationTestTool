using System;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>This class contains information about the assembly and firmware versions that were
	/// used for the PCR run.</summary>
	/// <remarks>This information is mainly for diagnostic purposes.</remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Shabnam</item>
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
	///			<item name="vssfile">$Workfile: ApplicationInfo.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/HardwareSoftwareInfo/ApplicationInfo.cs $</item>
	///			<item name="vssrevision">$Revision: 8 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	[Serializable]
	public partial class ApplicationInformation
	{
		#region Constants
        #endregion

        #region Member Data
		/// <summary>The software version used.</summary>
		private string m_AssemblyVersion;
		/// <summary>The firmware version used for the thermal base unit.</summary>
		private string m_ThermalBaseUnitFirmwareVersion;
		/// <summary>The software version used for the thermal base unit.</summary>
		private string m_ThermalBaseUnitSoftwareVersion;
		/// <summary>The firmware version used for the camera.</summary>
		private string m_CameraFirmwareVersion;
		/// <summary>The software version used for the camera.</summary>
		private string m_CameraSoftwareVersion;
        #endregion

        #region Accessors
		/// <summary>Gets / Sets the software version used.</summary>
		/// <remarks>This will probably be replaced by the different assembly versions.
		/// </remarks>
		public string AssemblyVersion
		{
			get { return this.m_AssemblyVersion;}
			set { this.m_AssemblyVersion = value;}
		}
		/// <summary>Gets / Sets the firmware version used for the thermal base unit.</summary>
		public string ThermalBaseUnitFirmwareVersion
		{
			get { return this.m_ThermalBaseUnitFirmwareVersion;}
			set { this.m_ThermalBaseUnitFirmwareVersion = value;}
		}
		/// <summary>Gets / Sets the software version used for the thermal base unit.</summary>
		public string ThermalBaseUnitSoftwareVersion
		{
			get { return this.m_ThermalBaseUnitSoftwareVersion;}
			set { this.m_ThermalBaseUnitSoftwareVersion = value;}
		}
		/// <summary>Gets / Sets the firmware version used for the camera.</summary>
		public string CameraFirmwareVersion
		{
			get { return this.m_CameraFirmwareVersion;}
			set { this.m_CameraFirmwareVersion = value;}
		}
		/// <summary>Gets / Sets the software version used for the camera.</summary>
		public string CameraSoftwareVersion
		{
			get { return this.m_CameraSoftwareVersion;}
			set { this.m_CameraSoftwareVersion = value;}
		}
        #endregion

		#region Delegates and Events
		#endregion

        #region Constructors and Destructor
		/// <summary>Constructs a new ApplicationInformation instance.</summary>
		/// <remarks>Default constructor.</remarks>
		public ApplicationInformation()
		{
		}
		/// <summary>Constructs a new ApplicationInformation instance.</summary>
		/// <remarks>Sets the member data to the passed in parameters.</remarks>
		/// <param name="assemblyVersion">The assembly version.</param>
		/// <param name="baseUnitFirmwareVersion">The thermal base unit's firmware version.</param>
		/// <param name="cameraFirmwareVersion">The camera's firmware version.</param>
		public ApplicationInformation(string assemblyVersion, string baseUnitFirmwareVersion,
			string cameraFirmwareVersion)
		{
			this.m_AssemblyVersion = assemblyVersion;
			this.m_ThermalBaseUnitFirmwareVersion = baseUnitFirmwareVersion;
			this.m_CameraFirmwareVersion = cameraFirmwareVersion;
		}
        #endregion

        #region Methods
        #endregion

		#region Event Handlers
		#endregion
	}
}
