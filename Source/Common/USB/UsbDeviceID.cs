using System;
using System.Diagnostics;

namespace BioRad.Common.UsbDevice
{
    #region Documentation Tags
    /// <summary>
    /// USB device instance ID
    /// </summary>
    /// <classinformation>
    ///		<list type="bullet">
    ///			<item name="authors">Authors:Ralph Ansell</item>
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
    ///			<item name="vssfile">$Workfile: UsbDeviceID.cs $</item>
    ///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/USB/UsbDeviceID.cs $</item>
    ///			<item name="vssrevision">$Revision: 1 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
    ///			<item name="vssdate">$Date: 2/02/10 1:28p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    public struct UsbDeviceId
    {
        #region Constants
        #endregion

        #region Member Data
        private UsbVendorId m_Vid;
        private UsbProductId m_Pid;
        private string m_SerialNumber;
        #endregion

        #region Accessors
        /// <summary>
        /// Get USB vendor ID.
        /// </summary>
        public UsbVendorId VendorId
        {
            get
            {
                return m_Vid;
            }
        }
        /// <summary>
        /// Get USB product ID.
        /// </summary>
        public UsbProductId ProductId
        {
            get
            {
                return m_Pid;
            }
        }
        /// <summary>
        /// Get USB serial number.
        /// </summary>
        public string SerialNumber
        {
            get
            {
                return m_SerialNumber;
            }
        }
        #endregion

        //#region Delegates and Events
        //#endregion

        #region Constructors and Destructor
        /// <summary>
        /// Initializes a new instance of the UsbDeviceId class.
        /// </summary>
        /// <param name="deviceID">Any string that conatins a device instance id (VID_NNNN\PID_NNNN\nn.nn.nn.nn).</param>
        public UsbDeviceId(string deviceID)
        {
            Debug.Assert(!string.IsNullOrEmpty(deviceID));

            m_SerialNumber = string.Empty;

            m_Vid = new UsbVendorId(deviceID);
            m_Pid = new UsbProductId(deviceID);
            if (!string.IsNullOrEmpty(m_Vid.ToString()) && !string.IsNullOrEmpty(m_Pid.ToString()))
            {
                int i = deviceID.IndexOf("pid_", StringComparison.OrdinalIgnoreCase);
                if (i >= 0)
                    deviceID = deviceID.Substring(i + 8);

                while (deviceID.IndexOf("\\") >= 0)
                    deviceID = deviceID.Replace("\\", "");
                while (deviceID.IndexOf("\"") >= 0)
                    deviceID = deviceID.Replace("\"", "");
                deviceID = deviceID.Trim();
                deviceID = deviceID.TrimEnd('.');
                deviceID = deviceID.Trim();

                m_SerialNumber = deviceID;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// A System.String that represents the current System.Object in current culture.
        /// </summary>
        /// <returns>A System.String that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0}\\{1}\\{2}", VendorId, ProductId, SerialNumber);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            bool equal = false;
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;

            UsbDeviceId id = (UsbDeviceId)obj;
            if (this.VendorId.Equals(id.VendorId))
            {
                if (this.ProductId.Equals(id.ProductId))
                {
                    if (string.Compare(this.SerialNumber, id.SerialNumber, true) == 0)
                        equal = true;
                }
            }
            return equal;
        }
        /// <summary>
        /// Overrides the GetHashCode method.
        /// Uses the builtin types hash code.
        /// </summary>
        /// <returns>A hash value for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        //#region Event Handlers
        //#endregion
    }
}
