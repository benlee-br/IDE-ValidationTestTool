using System;
using System.Collections;
using System.Text;

namespace BioRad.Common.Devices
{
	#region Documentation Tags
	/// <summary>This class stores base unit specific configuration information. 
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Pramod Walse, Ralph Ansell</item>
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
	///			<item name="vssfile">$Workfile: BaseUnitConfigInfo.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Devices/BaseUnitConfigInfo.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	[Serializable]
	public partial class BaseUnitConfigInfo : ICloneable
	{
		#region Member Data
		/// <summary>
		/// Polling rate of the base unit for thermal program status information.
		/// </summary>
		private TimeType m_PollingRate = null;
		#endregion

		#region Accessors
		/// <summary>
		/// Get/Set polling rate in milliseconds.
		/// </summary>
		public TimeType PollingRate
		{
			get 
			{ 
				return this.m_PollingRate;
			}
			set 
			{ 
				this.m_PollingRate = value;
			}
		}
		#endregion

        #region Constructors and Destructor
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public BaseUnitConfigInfo()
		{
		}
		#endregion

		#region Methods
		/// <summary>Create a clone of the BaseUnitConfigInfo.cs object.</summary>
		/// <returns>An object of the BaseUnitConfigInfo.cs type.</returns>
		public object Clone()
		{
			BaseUnitConfigInfo clonedDeviceInfo = (BaseUnitConfigInfo) this.MemberwiseClone();
			return clonedDeviceInfo;
		}
		#endregion
	}
}
