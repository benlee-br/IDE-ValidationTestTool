using System;
using System.Text;

namespace BioRad.Common.UsbDevice
{
    #region Documentation Tags
    /// <summary>
    /// USB attach/dettach change event arguments.
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
    ///			<item name="vssfile">$Workfile: UsbChangeEventArgs.cs $</item>
    ///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/USB/UsbChangeEventArgs.cs $</item>
    ///			<item name="vssrevision">$Revision: 1 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
    ///			<item name="vssdate">$Date: 2/02/10 1:28p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    [Serializable]
    public class UsbChangeEventArgs : EventArgs
    {
        /// <summary>
        /// USB event type.
        /// </summary>
        public enum UsbEventType
        {
            /// <summary>
            /// USB dettach event.
            /// </summary>
            detach,
            /// <summary>
            /// USB attach event
            /// </summary>
            attach,
            /// <summary>
            /// USB unknown event.
            /// </summary>
            unknown
        }

        #region Member Data
        private UsbEventType m_UsbEventType;
        private string m_DeviceInstanceID;
        #endregion

        #region Accessors
        /// <summary>
        /// Get/set USB event type.
        /// </summary>
        public UsbChangeEventArgs.UsbEventType EventType
        {
            get { return m_UsbEventType; }
        }
        /// <summary>
        /// Get/set USB device instance ID.
        /// </summary>
        public string DeviceInstanceID
        {
            get { return m_DeviceInstanceID; }
        }
        #endregion

        #region Delegates and Events
        #endregion

        #region Constructors and Destructor
        /// <summary>
        /// Initializes a new instance of the UsbDataEventArgs class.
        /// </summary>
        /// <param name="type">Event type (attach, detach).</param>
        /// <param name="deviceInstanceId">Device instance ID</param>
        public UsbChangeEventArgs(UsbChangeEventArgs.UsbEventType type, string deviceInstanceId)
        {
            m_UsbEventType = type;
            m_DeviceInstanceID = deviceInstanceId;
        }
        #endregion
    }
}
