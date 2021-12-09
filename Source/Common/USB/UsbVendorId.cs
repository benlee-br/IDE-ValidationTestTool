using System;
using System.Diagnostics;

namespace BioRad.Common.UsbDevice
{
    #region Documentation Tags
    /// <summary>
    /// USB vendor ID
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
    ///			<item name="vssfile">$Workfile: UsbVendorId.cs $</item>
    ///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/USB/UsbVendorId.cs $</item>
    ///			<item name="vssrevision">$Revision: 1 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
    ///			<item name="vssdate">$Date: 2/02/10 1:28p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    public struct UsbVendorId
    {
        #region Constants
        private static readonly string c_Prefix = "vid_";
        #endregion

        #region Member Data
        private string m_Vid;
        #endregion

        //#region Accessors
        //#endregion

        //#region Delegates and Events
        //#endregion

        #region Constructors and Destructor
        /// <summary>
        /// Initializes a new instance of the VendorId class.
        /// </summary>
        /// <param name="vid">Any string that conatins a vendor id (VID_NNNN).</param>
        public UsbVendorId(string vid)
        {
            Debug.Assert(!string.IsNullOrEmpty(vid));

            m_Vid = string.Empty;
            int i = vid.IndexOf(c_Prefix, StringComparison.OrdinalIgnoreCase);
            if (i >= 0)
                m_Vid = vid.Substring(i, 8);
        }
		/// <summary>
		/// Initializes a new instance of the VendorId class.
		/// </summary>
		/// <param name="vid"></param>
		public UsbVendorId(int vid)
		{
			m_Vid = c_Prefix + vid.ToString("X4");
		}
        #endregion

        #region Methods
        /// <summary>
        /// A System.String that represents the current System.Object in current culture.
        /// </summary>
        /// <returns>A System.String that represents the current object.</returns>
        public override string ToString()
        {
            return m_Vid;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;

            int i = string.Compare(this.ToString(), obj.ToString(), true);
            return (i == 0);
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
