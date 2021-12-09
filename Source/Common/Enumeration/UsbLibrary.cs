using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Xml;

namespace BioRad.UsbLibrary
{
    /// <summary>
    /// USB stuff.
    /// </summary>
    [CLSCompliant(true)]
    public class Usb
    {
        /// <summary>
        /// Generic definition for returning two values from a method.
        /// </summary>
        /// <typeparam name="T1">First return type.</typeparam>
        /// <typeparam name="T2">Second return type.</typeparam>
        public struct ResultPair<T1, T2>
        {
            private T1 m_T1;
            private T2 m_T2;
            /// <summary>
            /// Get first return type.
            /// </summary>
            public T1 GetT1
            {
                get { return m_T1; }
            }
            /// <summary>
            /// Get second return type.
            /// </summary>
            public T2 GetT2
            {
                get { return m_T2; }
            }
            /// <summary>Initializes a new instance of the ResultPair class.</summary>
            /// <param name="t1">First return type value.</param>
            /// <param name="t2">Second return type value.</param>
            public ResultPair(T1 t1, T2 t2)
            {
                m_T1 = t1;
                m_T2 = t2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Conditional("DEBUG")]
        public static void Assert()
        {
            StackTrace st = new StackTrace(true);
            StackFrame sf = st.GetFrame(1);
            string AssertMessage = string.Format("{0} {1} {2}\n", 
                sf.GetFileName(), sf.GetMethod().Name, sf.GetFileLineNumber());
            Debug.Assert(false, AssertMessage);
            Console.WriteLine(AssertMessage);
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getErrorText">Set to true to get text for error.</param>
        /// <returns>Pair struct where int is win32 error code.</returns>
        public static ResultPair<int, string> GetLastError(bool getErrorText)
        {
            // from header files
            const uint FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
            const uint FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
            const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;

            string text = string.Empty;
            int lastError = Marshal.GetLastWin32Error();

            if (getErrorText)
            {
                IntPtr lpMsgBuf = IntPtr.Zero;

                uint dwChars = FormatMessage(
                    FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
                    IntPtr.Zero,
                    (uint)lastError,
                    0, // Default language
                    ref lpMsgBuf,
                    0,
                    IntPtr.Zero);
                if (dwChars == 0)
                {
                    // Handle the error.
                    lastError = Marshal.GetLastWin32Error();
                }
                else
                {
                    text = Marshal.PtrToStringAnsi(lpMsgBuf);

                    // Free the buffer.
                    lpMsgBuf = LocalFree(lpMsgBuf);
                }
            }

            //Debug.Assert(false, string.Format("GetLastWin32Error = {0} {1}", lastError, text));
            return new Usb.ResultPair<int, string>(lastError, text);
        } 

        /// <summary>
        /// Vendor id string identifier.
        /// </summary>
        public static readonly string c_VidIdentifier = "vid_";
        /// <summary>
        /// Product id string identifier.
        /// </summary>
        public static readonly string c_PidIdentifier = "pid_";

        #region "API"

        // ********************** Constants ************************ 
        const int DIF_PROPERTYCHANGE = 0x00000012;
        const int DICS_FLAG_GLOBAL = 0x00000001;
        const int DICS_PROPCHANGE = 0x00000003;

        const uint GENERIC_WRITE = 0x40000000;
        const uint GENERIC_READ =  0x80000000;

        const uint FILE_SHARE_READ = 0x1;
        const uint FILE_SHARE_WRITE = 0x2;

        const int OPEN_EXISTING = 0x3;
        const int INVALID_HANDLE_VALUE = -1;
        const int FILE_FLAG_OVERLAPPED = 0x40000000;

        const int IOCTL_GET_HCD_DRIVERKEY_NAME = 0x220424;
        const int IOCTL_USB_GET_ROOT_HUB_NAME = 0x220408;
        const int IOCTL_USB_GET_NODE_INFORMATION = 0x220408;    // same as above... strange, eh? 
        const int IOCTL_USB_GET_NODE_CONNECTION_INFORMATION_EX = 0x220448;
        const int IOCTL_USB_GET_DESCRIPTOR_FROM_NODE_CONNECTION = 0x220410;
        const int IOCTL_USB_GET_NODE_CONNECTION_NAME = 0x220414;
        const int IOCTL_USB_GET_NODE_CONNECTION_DRIVERKEY_NAME = 0x220420;

        const int USB_DEVICE_DESCRIPTOR_TYPE = 0x1;
        const int USB_STRING_DESCRIPTOR_TYPE = 0x3;

        const int BUFFER_SIZE = 2048;
        const int MAXIMUM_USB_STRING_LENGTH = 255;

        /// <summary>
        /// The Globally Unique Identifier (GUID) for the "Interface" for a USB Host Controller
        /// </summary>
        const string GUID_DEVINTERFACE_HUBCONTROLLER = "3abf6f2d-71c4-462a-8a92-1e6861e6af27";

        const string REGSTR_KEY_USB = "USB";
        const int DIGCF_PRESENT = 0x2;
        const int DIGCF_ALLCLASSES = 0x4;
        const int DIGCF_DEVICEINTERFACE = 0x10;

        const int SPDRP_HARDWAREID = (0x00000001); // HardwareID (R/W)
        const int SPDRP_DRIVER = 0x9;
        const int SPDRP_DEVICEDESC = 0x0;
        const int REG_SZ = 1;

        // ********************** Enumerations ************************ 
        enum USB_HUB_NODE
        {
            UsbHub,
            UsbMIParent
        }

        enum USB_CONNECTION_STATUS
        {
            NoDeviceConnected,
            DeviceConnected,
            DeviceFailedEnumeration,
            DeviceGeneralFailure,
            DeviceCausedOvercurrent,
            DeviceNotEnoughPower,
            DeviceNotEnoughBandwidth,
            DeviceHubNestedTooDeeply,
            DeviceInLegacyHub
        }

        enum USB_DEVICE_SPEED : byte
        {
            UsbLowSpeed,
            UsbFullSpeed,
            UsbHighSpeed
        }

        // ********************** Stuctures ************************ 
        [StructLayout(LayoutKind.Sequential)]
        struct SP_DEVINFO_DATA
        {
            public int cbSize;
            public Guid ClassGuid;
            public IntPtr DevInst;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public Guid InterfaceClassGuid;
            public int Flags;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public int cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
            public string DevicePath;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct USB_HCD_DRIVERKEY_NAME
        {
            public int ActualLength;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
            public string DriverKeyName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct USB_ROOT_HUB_NAME
        {
            public int ActualLength;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
            public string RootHubName;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct USB_HUB_DESCRIPTOR
        {
            public byte bDescriptorLength;
            public byte bDescriptorType;
            public byte bNumberOfPorts;
            public short wHubCharacteristics;
            public byte bPowerOnToPowerGood;
            public byte bHubControlCurrent;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] bRemoveAndPowerMask;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct USB_HUB_INFORMATION
        {
            public USB_HUB_DESCRIPTOR HubDescriptor;
            public byte HubIsBusPowered;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct USB_NODE_INFORMATION
        {
            public int NodeType;
            public USB_HUB_INFORMATION HubInformation;          // Yeah, I'm assuming we'll just use the first form 
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct USB_NODE_CONNECTION_INFORMATION_EX
        {
            public int ConnectionIndex;
            public USB_DEVICE_DESCRIPTOR DeviceDescriptor;
            public byte CurrentConfigurationValue;
            public byte Speed;
            public byte DeviceIsHub;
            public short DeviceAddress;
            public int NumberOfOpenPipes;
            public int ConnectionStatus;
            //public IntPtr PipeList; 
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct USB_DEVICE_DESCRIPTOR
        {
            public byte bLength;
            public byte bDescriptorType;
            public short bcdUSB;
            public byte bDeviceClass;
            public byte bDeviceSubClass;
            public byte bDeviceProtocol;
            public byte bMaxPacketSize0;
            public short idVendor;
            public short idProduct;
            public short bcdDevice;
            public byte iManufacturer;
            public byte iProduct;
            public byte iSerialNumber;
            public byte bNumConfigurations;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct USB_STRING_DESCRIPTOR
        {
            public byte bLength;
            public byte bDescriptorType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXIMUM_USB_STRING_LENGTH)]
            public string bString;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct USB_SETUP_PACKET
        {
            public byte bmRequest;
            public byte bRequest;
            public short wValue;
            public short wIndex;
            public short wLength;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct USB_DESCRIPTOR_REQUEST
        {
            public int ConnectionIndex;
            public USB_SETUP_PACKET SetupPacket;
            //public byte[] Data; 
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct USB_NODE_CONNECTION_NAME
        {
            public int ConnectionIndex;
            public int ActualLength;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
            public string NodeName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct USB_NODE_CONNECTION_DRIVERKEY_NAME               // Yes, this is the same as the structure above... 
        {
            public int ConnectionIndex;
            public int ActualLength;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
            public string DriverKeyName;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SP_CLASSINSTALL_HEADER
        {
            public int cbSize;
            public int InstallFunction;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SP_PROPCHANGE_PARAMS
        {
            public SP_CLASSINSTALL_HEADER ClassInstallHeader;
            public int StateChange;
            public int Scope;
            public int HwProfile;
            public void Init()
            {
                ClassInstallHeader = new SP_CLASSINSTALL_HEADER();
            }
        }

        // ********************** API Definitions ************************ 
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr LocalFree(IntPtr hlocal);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static extern uint FormatMessage(uint dwFlags, IntPtr lpSource,
           uint dwMessageId, uint dwLanguageId, ref IntPtr lpBuffer,
           uint nSize, IntPtr pArguments);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SetupDiGetClassDevs(               // 1st form using a ClassGUID 
            ref Guid ClassGuid,
            int Enumerator,
            IntPtr hwndParent,
            int Flags
        );
        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]     // 2nd form uses an Enumerator 
        static extern IntPtr SetupDiGetClassDevs(
            int ClassGuid,
            string Enumerator,
            IntPtr hwndParent,
            int Flags
        );

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool SetupDiEnumDeviceInterfaces(
            IntPtr DeviceInfoSet,
            IntPtr DeviceInfoData,
            ref Guid InterfaceClassGuid,
            int MemberIndex,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData
        );

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool SetupDiGetDeviceInterfaceDetail(
            IntPtr DeviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
            ref SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData,
            int DeviceInterfaceDetailDataSize,
            ref int RequiredSize,
            ref SP_DEVINFO_DATA DeviceInfoData
        );

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool SetupDiGetDeviceRegistryProperty(
            IntPtr DeviceInfoSet,
            ref SP_DEVINFO_DATA DeviceInfoData,
            int iProperty,
            ref int PropertyRegDataType,
            IntPtr PropertyBuffer,
            int PropertyBufferSize,
            ref int RequiredSize
        );

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool SetupDiEnumDeviceInfo(
            IntPtr DeviceInfoSet,
            int MemberIndex,
            ref SP_DEVINFO_DATA DeviceInfoData
        );

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiDestroyDeviceInfoList(
            IntPtr DeviceInfoSet
        );

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool SetupDiGetDeviceInstanceId(
            IntPtr DeviceInfoSet,
            ref SP_DEVINFO_DATA DeviceInfoData,
            StringBuilder DeviceInstanceId,
            int DeviceInstanceIdSize,
            out int RequiredSize
        );

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool DeviceIoControl(
            IntPtr hDevice,
            int dwIoControlCode,
            IntPtr lpInBuffer,
            int nInBufferSize,
            IntPtr lpOutBuffer,
            int nOutBufferSize,
            out int lpBytesReturned,
            IntPtr lpOverlapped
        );

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr CreateFile(
           string lpFileName,
           uint dwDesiredAccess,
           uint dwShareMode,
           IntPtr lpSecurityAttributes,
           int dwCreationDisposition,
           int dwFlagsAndAttributes,
           IntPtr hTemplateFile
        );

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("setupapi.dll")]
        static extern bool SetupDiSetClassInstallParams(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, 
            ref SP_CLASSINSTALL_HEADER ClassInstallParams, int ClassInstallParamsSize);

        [DllImport("setupapi.dll")]
        static extern bool SetupDiCallClassInstaller(int InstallFunction, IntPtr DeviceInfoSet, 
            ref SP_DEVINFO_DATA DeviceInfoData);
        #endregion

        #region Delegates and Events
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hub"></param>
        /// <param name="vendorId"></param>
        /// <param name="devices"></param>
        public static void EnumeratePorts(Usb.USBHub hub, Usb.VendorId vendorId, ref List<USBDevice> devices)
        {
            List<Usb.USBPort> ports = hub.GetPorts();
            foreach (Usb.USBPort port in ports)
            {
                if (port.IsHub)
                    EnumeratePorts(port.GetHub(), vendorId, ref devices);

                USBDevice device = port.GetDevice();
                if (device != null)
                {
                    int i = device.DeviceInstanceID.IndexOf
                        (vendorId.ToString(), StringComparison.OrdinalIgnoreCase);
                    if (i >= 0)
                        devices.Add(device);
                }
            }
        }
        /// <summary>
        /// Get list of USB devices for specified vendor id.
        /// </summary>
        /// <param name="vendorId">
        /// vendor ids as follows : vid_4079 or VID_4079. Comaprison is 
        /// case insenitive.
        /// </param>
        /// <returns></returns>
        public static List<USBDevice> GetDevices(Usb.VendorId vendorId)
        {
            List<USBDevice> devices = new List<USBDevice>();
            List<Usb.USBController> hosts = Usb.GetHostControllers();
            int count = hosts.Count;
            foreach (Usb.USBController host in hosts)
            {
                Usb.USBHub hub = host.GetRootHub();
                EnumeratePorts(hub, vendorId, ref devices);
            }
            return devices;
        }
        /// <summary>
        /// Get list of USB devices for specified vendor id.
        /// </summary>
        /// <param name="vendorId">
        /// vendor ids as follows : vid_4079 or VID_4079. Comaprison is 
        /// case insenitive.
        /// </param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static List<USBDevice> GetDevices(Usb.VendorId vendorId, Usb.ProductId productId)
        {
            List<USBDevice> devices = new List<USBDevice>();
            List<USBDevice> list = GetDevices(vendorId);
            foreach (USBDevice d in list)
            {
                int i = d.DeviceInstanceID.IndexOf(productId.ToString(), StringComparison.OrdinalIgnoreCase);
                if (i >= 0)
                    devices.Add(d);
            }
           
            return devices;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<USBDevice> GetAllDevices()
        {
            List<USBDevice> devices = new List<USBDevice>();
            List<Usb.USBController> hosts = Usb.GetHostControllers();
            List<string> instanceIDs = GetAllDevicesPresent();
            foreach (string instanceID in instanceIDs)
            {
                Usb.VendorId vendorId = new VendorId(instanceID);
                if (!string.IsNullOrEmpty(vendorId.ToString()))
                {
                    foreach (Usb.USBController host in hosts)
                    {
                        Usb.USBHub hub = host.GetRootHub();
                        EnumeratePorts(hub, vendorId, ref devices);
                    }
                }
            }
            return devices;
        }
        private static void FindHub(Usb.USBHub hub, string instanceID, ref Usb.USBHub vidHub)
        {
            List<Usb.USBPort> ports = hub.GetPorts();
            foreach (Usb.USBPort port in ports)
            {
                if (port.IsHub)
                    FindHub(port.GetHub(), instanceID, ref vidHub);

                USBDevice device = port.GetDevice();
                if (device != null)
                {
                    int i = device.DeviceInstanceID.IndexOf(instanceID, StringComparison.OrdinalIgnoreCase);
                    if (i >= 0)
                        vidHub = hub;
                }
            }
        }
        /// <summary>
        /// Stops and restarts the specified device instance. Valid only on the local computer.
        /// </summary>
        /// <param name="deviceInstanceId">Device instance ID.</param>
        /// <returns>Error if int of Pair if non zero.</returns>
        public static ResultPair<int, string> RestartDevice(Usb.DeviceInstanceId deviceInstanceId)
        {
            ResultPair<int, string> lastError = new ResultPair<int, string>(0, string.Empty);

            Debug.Assert(!string.IsNullOrEmpty(deviceInstanceId.ToString()));
            if (string.IsNullOrEmpty(deviceInstanceId.ToString()))
                return lastError;

            string enumInstanceId = string.Empty;
            string DevEnum = REGSTR_KEY_USB;

            // Use the "enumerator form" of the SetupDiGetClassDevs API  
            // to generate a list of all USB devices  
            IntPtr h = SetupDiGetClassDevs(0, DevEnum, IntPtr.Zero, DIGCF_PRESENT | DIGCF_ALLCLASSES);
            if (h.ToInt32() != INVALID_HANDLE_VALUE)
            {
                IntPtr ptrBuf = Marshal.AllocHGlobal(BUFFER_SIZE);
                string KeyName = string.Empty;

                bool Success;
                int i = 0;
                do
                {
                    // create a Device Interface Data structure 
                    SP_DEVINFO_DATA da = new SP_DEVINFO_DATA();
                    da.cbSize = Marshal.SizeOf(da);

                    // start the enumeration  
                    Success = SetupDiEnumDeviceInfo(h, i, ref da);
                    if (Success)
                    {
                        int RequiredSize = 0;
                        int RegType = REG_SZ;

                        KeyName = string.Empty;
                        if (SetupDiGetDeviceRegistryProperty(h, ref da, SPDRP_HARDWAREID,
                            ref RegType, ptrBuf, BUFFER_SIZE, ref RequiredSize))
                        {
                            KeyName = Marshal.PtrToStringAuto(ptrBuf);
                        }
                        else
                        {
                            lastError = GetLastError(true);
                        }

                        int nBytes = BUFFER_SIZE;
                        StringBuilder sb = new StringBuilder(nBytes);
                        if (!SetupDiGetDeviceInstanceId(h, ref da, sb, nBytes, out RequiredSize))
                        {
                            lastError = GetLastError(true);
                        }
                        enumInstanceId = sb.ToString();

                        int k = enumInstanceId.IndexOf(deviceInstanceId.ToString(), StringComparison.OrdinalIgnoreCase);
                        if (k >= 0)
                        {
                            SP_PROPCHANGE_PARAMS PropChangeParams;
                            PropChangeParams = new SP_PROPCHANGE_PARAMS();
                            PropChangeParams.Init();
                            PropChangeParams.ClassInstallHeader.cbSize = Marshal.SizeOf(PropChangeParams.ClassInstallHeader);
                            PropChangeParams.ClassInstallHeader.InstallFunction = DIF_PROPERTYCHANGE;
                            PropChangeParams.StateChange = DICS_PROPCHANGE;
                            PropChangeParams.Scope = DICS_FLAG_GLOBAL;
                            PropChangeParams.HwProfile = 0;

                            if (SetupDiSetClassInstallParams(h, ref da,
                                ref PropChangeParams.ClassInstallHeader, Marshal.SizeOf(PropChangeParams)))
                            {
                                if (!SetupDiCallClassInstaller(DIF_PROPERTYCHANGE, h, ref da))
                                {
                                    lastError = GetLastError(true);
                                }
                            }
                            else
                            {
                                lastError = GetLastError(true);
                            }
                            break;
                        }
                    }
                    else
                    {
                        lastError = GetLastError(true);
                    }
                    i++;
                } while (Success);

                Marshal.FreeHGlobal(ptrBuf);
                SetupDiDestroyDeviceInfoList(h);
            }

            return lastError;
        }

        /// <summary>
        /// Get device hub.
        /// </summary>
        /// <param name="instanceID">
        /// Vendor id as follows : vid_4079 or VID_4079. Comaprison is 
        /// case insenitive.
        /// </param>
        /// <returns>null if not found.</returns>
        public static Usb.USBHub GetDeviceHub(string instanceID)
        {
            Usb.USBHub theHub = null;
            List<Usb.USBController> hosts = Usb.GetHostControllers();
            int count = hosts.Count;

            foreach (Usb.USBController host in hosts)
            {
                Usb.USBHub hub = host.GetRootHub();
                FindHub(hub, instanceID, ref theHub);
            }
            return theHub;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInstanceId"></param>
        /// <returns></returns>
        public static string GetDevicePath(string deviceInstanceId)
        {
            //The system-supplied USB hub driver registers instances of GUID_DEVINTERFACE_USB_DEVICE to notify the system and applications 
            //of the presence of USB devices that are attached to a USB hub. 
            Guid GUID_DEVINTERFACE_USB_DEVICE = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED");
            Guid guid = GUID_DEVINTERFACE_USB_DEVICE;
            string path = string.Empty;

            // We start at the "root" of the device tree and look for all 
            // devices that match the interface GUID. 
            IntPtr h = SetupDiGetClassDevs(ref guid, 0, IntPtr.Zero, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
            if (h.ToInt32() != INVALID_HANDLE_VALUE)
            {
                IntPtr ptrBuf = Marshal.AllocHGlobal(BUFFER_SIZE);
                bool success;
                int i = 0;

                string deviceInstanceId1 = deviceInstanceId.Replace('\\', '#');

                do
                {
                    // create a Device Interface Data structure 
                    SP_DEVICE_INTERFACE_DATA dia = new SP_DEVICE_INTERFACE_DATA();
                    dia.cbSize = Marshal.SizeOf(dia);

                    // start the enumeration  
                    success = SetupDiEnumDeviceInterfaces(h, IntPtr.Zero, ref guid, i, ref dia);
                    if (success)
                    {
                        // build a DevInfo Data structure 
                        SP_DEVINFO_DATA da = new SP_DEVINFO_DATA();
                        da.cbSize = Marshal.SizeOf(da);

                        // build a Device Interface Detail Data structure 
                        SP_DEVICE_INTERFACE_DETAIL_DATA didd = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                        didd.cbSize = 4 + Marshal.SystemDefaultCharSize; // trust me :) 

                        // now we can get some more detailed information 
                        int nRequiredSize = 0;
                        int nBytes = BUFFER_SIZE;
                        if (SetupDiGetDeviceInterfaceDetail(h, ref dia, ref didd, nBytes, ref nRequiredSize, ref da))
                        {
                            int k = didd.DevicePath.IndexOf(deviceInstanceId1, StringComparison.OrdinalIgnoreCase);
                            if (k >= 0)
                            {
                                path = didd.DevicePath;
                                break;
                            }
                            else
                            {
                                k = didd.DevicePath.IndexOf(deviceInstanceId, StringComparison.OrdinalIgnoreCase);
                                if (k >= 0)
                                {
                                    path = didd.DevicePath;
                                    break;
                                }
                            }
                        }
                    }
                    i++;
                } while (success);

                Marshal.FreeHGlobal(ptrBuf);
                SetupDiDestroyDeviceInfoList(h);
            }
            return path;
        }
        /// <summary>
        ///  Return a list of USB Host Controllers 
        /// </summary>
        /// <returns></returns>
        static public List<Usb.USBController> GetHostControllers()
        {
            List<USBController> hostList = new List<USBController>();
            Guid hostGUID = new Guid(GUID_DEVINTERFACE_HUBCONTROLLER);

            // We start at the "root" of the device tree and look for all 
            // devices that match the interface GUID of a Hub Controller 
            IntPtr h = SetupDiGetClassDevs(ref hostGUID, 0, IntPtr.Zero, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
            if (h.ToInt32() != INVALID_HANDLE_VALUE)
            {
                IntPtr ptrBuf = Marshal.AllocHGlobal(BUFFER_SIZE);
                bool success;
                int i = 0;
                do
                {
                    USBController host = new USBController();
                    host.ControllerIndex = i;

                    // create a Device Interface Data structure 
                    SP_DEVICE_INTERFACE_DATA dia = new SP_DEVICE_INTERFACE_DATA();
                    dia.cbSize = Marshal.SizeOf(dia);

                    // start the enumeration  
                    success = SetupDiEnumDeviceInterfaces(h, IntPtr.Zero, ref hostGUID, i, ref dia);
                    if (success)
                    {
                        // build a DevInfo Data structure 
                        SP_DEVINFO_DATA da = new SP_DEVINFO_DATA();
                        da.cbSize = Marshal.SizeOf(da);

                        // build a Device Interface Detail Data structure 
                        SP_DEVICE_INTERFACE_DETAIL_DATA didd = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                        didd.cbSize = 4 + Marshal.SystemDefaultCharSize; // trust me :) 

                        // now we can get some more detailed information 
                        int nRequiredSize = 0;
                        int nBytes = BUFFER_SIZE;
                        if (SetupDiGetDeviceInterfaceDetail(h, ref dia, ref didd, nBytes, ref nRequiredSize, ref da))
                        {
                            host.ControllerDevicePath = didd.DevicePath;

                            // get the Device Description and DriverKeyName 
                            int RequiredSize = 0;
                            int RegType = REG_SZ;

                            if (SetupDiGetDeviceRegistryProperty(h, ref da, SPDRP_DEVICEDESC, 
                                ref RegType, ptrBuf, BUFFER_SIZE, ref RequiredSize))
                            {
                                host.ControllerDeviceDesc = Marshal.PtrToStringAuto(ptrBuf);
                            }
                            if (SetupDiGetDeviceRegistryProperty(h, ref da, SPDRP_DRIVER, 
                                ref RegType, ptrBuf, BUFFER_SIZE, ref RequiredSize))
                            {
                                host.ControllerDriverKeyName = Marshal.PtrToStringAuto(ptrBuf);
                            }
                        }
                        hostList.Add(host);
                    }
                    i++;
                } while (success);

                Marshal.FreeHGlobal(ptrBuf);
                SetupDiDestroyDeviceInfoList(h);
            }
            return hostList;
        }

        /// <summary>USB vendor ID</summary>
        public class VendorId
        {
            private string m_Vid;

            /// <summary>
            /// Initializes a new instance of the VendorId class.
            /// </summary>
            /// <param name="vid">any string that conatins a vendor id (VID_NNNN).</param>
            public VendorId(string vid)
            {
                Debug.Assert(!string.IsNullOrEmpty(vid));
                Debug.Assert(vid.IndexOf(Usb.c_VidIdentifier, StringComparison.OrdinalIgnoreCase) >= 0);

                m_Vid = string.Empty;

                int i = vid.IndexOf(Usb.c_VidIdentifier, StringComparison.OrdinalIgnoreCase);
                if (i >= 0)
                    m_Vid = vid.Substring(i, 8);

                Debug.Assert(!string.IsNullOrEmpty(m_Vid));
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Vid;
            }
        }

        /// <summary>USB product ID</summary>
        public struct ProductId
        {
            private string m_Pid;

            /// <summary>
            /// Initializes a new instance of the ProductId class.
            /// </summary>
            /// <param name="pid">any string that conatins a product id (PID_NNNN).</param>
            public ProductId(string pid)
            {
                Debug.Assert(!string.IsNullOrEmpty(pid));
                Debug.Assert(pid.IndexOf(Usb.c_PidIdentifier, StringComparison.OrdinalIgnoreCase) >= 0);

                m_Pid = string.Empty;

                int i = pid.IndexOf(Usb.c_PidIdentifier, StringComparison.OrdinalIgnoreCase);
                if (i >= 0)
                    m_Pid = pid.Substring(i, 8);

                Debug.Assert(!string.IsNullOrEmpty(m_Pid));
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Pid;
            }
        }

        /// <summary>USB device instance ID.</summary>
        /// <remarks>
        /// Device instance ID consists of three parts. 
        /// 1. Vendor ID. 2. Product ID. 3. Instance ID.
        /// </remarks>
        public struct DeviceInstanceId
        {
            private string m_ID;

            /// <summary>
            /// Initializes a new instance of the DeviceInstanceId class.
            /// </summary>
            /// <param name="deviceInstanceId">device instance ID.</param>
            public DeviceInstanceId(string deviceInstanceId)
            {
                Debug.Assert(!string.IsNullOrEmpty(deviceInstanceId));
                Debug.Assert(deviceInstanceId.IndexOf(Usb.c_VidIdentifier, StringComparison.OrdinalIgnoreCase) >= 0);
                Debug.Assert(deviceInstanceId.IndexOf(Usb.c_PidIdentifier, StringComparison.OrdinalIgnoreCase) >= 0);

                m_ID = string.Empty;
                string[] parts = deviceInstanceId.Split('\\');
                Debug.Assert(parts.Length == 3);

                m_ID = deviceInstanceId;
            }
            /// <summary>
            /// Vendor ID
            /// </summary>
            public VendorId VendorId
            {
                get { return new VendorId(m_ID); }
            }
            /// <summary>
            /// Product ID.
            /// </summary>
            public ProductId ProductId
            {
                get { return new ProductId(m_ID); }
            }
            /// <summary>
            /// Instance ID.
            /// </summary>
            public string InstanceId
            {
                get 
                {
                    string instanceId = string.Empty;
                    string[] parts = m_ID.Split('\\');
                    Debug.Assert(parts.Length == 3);
                    if ( parts.Length == 3 )
                        instanceId = parts[2];
                    return instanceId;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_ID;
            }
        }
        /// <summary>
        /// The USB Host Controller Class
        /// </summary>
        public class USBController
        {
            internal int ControllerIndex;
            internal string ControllerDriverKeyName, ControllerDevicePath, ControllerDeviceDesc;

            /// <summary>
            /// 
            /// </summary>
            public USBController()
            {
                ControllerIndex = 0;
                ControllerDevicePath = "";
                ControllerDeviceDesc = "";
                ControllerDriverKeyName = "";
            }

            /// <summary>
            /// Return the index of the instance 
            /// </summary>
            public int Index
            {
                get { return ControllerIndex; }
            }

            /// <summary>
            /// Return the Device Path, such as 
            /// </summary>
            public string DevicePath
            {
                get { return ControllerDevicePath; }
            }

            /// <summary>
            /// The DriverKeyName may be useful as a search key 
            /// </summary>
            public string DriverKeyName
            {
                get { return ControllerDriverKeyName; }
            }

            /// <summary>
            /// Return the Friendly Name, such as "VIA USB Enhanced Host Controller" 
            /// </summary>
            public string Name
            {
                get { return ControllerDeviceDesc; }
            }

            /// <summary>
            /// Return Root Hub for this Controller.
            /// </summary>
            /// <returns></returns>
            public USBHub GetRootHub()
            {
                IntPtr h, h2;
                USBHub Root = new USBHub();
                Root.m_HubIsRootHub = true;
                Root.m_HubDeviceDesc = "Root Hub";

                // Open a handle to the Host Controller 
                h = CreateFile(ControllerDevicePath, GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
                if (h.ToInt32() != INVALID_HANDLE_VALUE)
                {
                    int nBytesReturned;
                    USB_ROOT_HUB_NAME HubName = new USB_ROOT_HUB_NAME();
                    int nBytes = Marshal.SizeOf(HubName);
                    IntPtr ptrHubName = Marshal.AllocHGlobal(nBytes);

                    // get the Hub Name 
                    if (DeviceIoControl(h, IOCTL_USB_GET_ROOT_HUB_NAME, ptrHubName, nBytes, ptrHubName, nBytes, out nBytesReturned, IntPtr.Zero))
                    {
                        HubName = (USB_ROOT_HUB_NAME)Marshal.PtrToStructure(ptrHubName, typeof(USB_ROOT_HUB_NAME));
                        Root.m_HubDevicePath = @"\\.\" + HubName.RootHubName;
                    }

                    // TODO: Get DriverKeyName for Root Hub 

                    // Now let's open the Hub (based upon the HubName we got above) 
                    h2 = CreateFile(Root.m_HubDevicePath, GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
                    if (h2.ToInt32() != INVALID_HANDLE_VALUE)
                    {
                        USB_NODE_INFORMATION NodeInfo = new USB_NODE_INFORMATION();
                        NodeInfo.NodeType = (int)USB_HUB_NODE.UsbHub;
                        nBytes = Marshal.SizeOf(NodeInfo);
                        IntPtr ptrNodeInfo = Marshal.AllocHGlobal(nBytes);
                        Marshal.StructureToPtr(NodeInfo, ptrNodeInfo, true);

                        // get the Hub Information 
                        if (DeviceIoControl(h2, IOCTL_USB_GET_NODE_INFORMATION, ptrNodeInfo, nBytes, ptrNodeInfo, nBytes, out nBytesReturned, IntPtr.Zero))
                        {
                            NodeInfo = (USB_NODE_INFORMATION)Marshal.PtrToStructure(ptrNodeInfo, typeof(USB_NODE_INFORMATION));
                            Root.m_HubIsBusPowered = Convert.ToBoolean(NodeInfo.HubInformation.HubIsBusPowered);
                            Root.m_HubPortCount = NodeInfo.HubInformation.HubDescriptor.bNumberOfPorts;
                        }
                        Marshal.FreeHGlobal(ptrNodeInfo);
                        CloseHandle(h2);
                    }

                    Marshal.FreeHGlobal(ptrHubName);
                    CloseHandle(h);
                }
                return Root;
            }
        }

        /// <summary>USB Hub</summary>
        public class USBHub
        {
            #region Members
            internal int m_HubPortCount;
            internal string m_HubDriverKey, m_HubDevicePath, m_HubDeviceDesc;
            internal string m_HubManufacturer, m_HubProduct, m_HubSerialNumber, m_HubInstanceID;
            internal bool m_HubIsBusPowered, m_HubIsRootHub;
            #endregion

            #region Accessors
            /// <summary>
            /// Port count
            /// </summary>
            public int PortCount
            {
                get { return m_HubPortCount; }
            }

            /// <summary>
            /// Return the Device Path
            /// </summary>
            public string DevicePath
            {
                get { return m_HubDevicePath; }
            }

            /// <summary>
            /// The DriverKey may be useful as a search key
            /// </summary>
            public string DriverKey
            {
                get { return m_HubDriverKey; }
            }

            /// <summary>
            /// return the Friendly Name, such as "VIA USB Enhanced Host Controller" 
            /// </summary>
            public string Name
            {
                get { return m_HubDeviceDesc; }
            }

            /// <summary>
            /// the device path of this device
            /// </summary>
            public string InstanceID
            {
                get { return m_HubInstanceID; }
            }

            /// <summary>
            /// Is this a self-powered hub?
            /// </summary>
            public bool IsBusPowered
            {
                get { return m_HubIsBusPowered; }
            }

            /// <summary>
            /// Is root hub?
            /// </summary>
            public bool IsRootHub
            {
                get { return m_HubIsRootHub; }
            }
            /// <summary>
            /// Manufacturer
            /// </summary>
            public string Manufacturer
            {
                get { return m_HubManufacturer; }
            }
            /// <summary>
            /// Product
            /// </summary>
            public string Product
            {
                get { return m_HubProduct; }
            }
            /// <summary>
            /// Serial number
            /// </summary>
            public string SerialNumber
            {
                get { return m_HubSerialNumber; }
            }
            #endregion

            #region Constructors and Destructor
            /// <summary>
            /// 
            /// </summary>
            public USBHub()
            {
                m_HubPortCount = 0;
                m_HubDevicePath = string.Empty;
                m_HubDeviceDesc = string.Empty;
                m_HubDriverKey = string.Empty;
                m_HubIsBusPowered = false;
                m_HubIsRootHub = false;
                m_HubManufacturer = string.Empty;
                m_HubProduct = string.Empty;
                m_HubSerialNumber = string.Empty;
                m_HubInstanceID = string.Empty;
            }
            #endregion

            #region Methods
            /// <summary>
            /// Return a list of the down stream ports.
            /// </summary>
            /// <returns></returns>
            public List<USBPort> GetPorts()
            {
                List<USBPort> PortList = new List<USBPort>();

                // Open a handle to the Hub device 
                IntPtr h = CreateFile(m_HubDevicePath, GENERIC_WRITE, FILE_SHARE_WRITE, 
                    IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
                if (h.ToInt32() != INVALID_HANDLE_VALUE)
                {
                    int nBytes = Marshal.SizeOf(typeof(USB_NODE_CONNECTION_INFORMATION_EX));
                    IntPtr ptrNodeConnection = Marshal.AllocHGlobal(nBytes);

                    // loop thru all of the ports on the hub 
                    // BTW: Ports are numbered starting at 1 
                    for (int i = 1; i <= m_HubPortCount; i++)
                    {
                        int nBytesReturned;
                        USB_NODE_CONNECTION_INFORMATION_EX NodeConnection = new USB_NODE_CONNECTION_INFORMATION_EX();
                        NodeConnection.ConnectionIndex = i;
                        Marshal.StructureToPtr(NodeConnection, ptrNodeConnection, true);

                        if (DeviceIoControl(h, IOCTL_USB_GET_NODE_CONNECTION_INFORMATION_EX, ptrNodeConnection, nBytes, ptrNodeConnection, nBytes, out nBytesReturned, IntPtr.Zero))
                        {
                            NodeConnection = (USB_NODE_CONNECTION_INFORMATION_EX)Marshal.PtrToStructure(ptrNodeConnection, typeof(USB_NODE_CONNECTION_INFORMATION_EX));

                            // load up the USBPort class 
                            USBPort port = new USBPort();
                            port.m_PortPortNumber = i;
                            port.m_PortHubDevicePath = m_HubDevicePath;
                            USB_CONNECTION_STATUS Status = (USB_CONNECTION_STATUS)NodeConnection.ConnectionStatus;
                            port.m_PortStatus = Status.ToString();
                            USB_DEVICE_SPEED Speed = (USB_DEVICE_SPEED)NodeConnection.Speed;
                            port.m_PortSpeed = Speed.ToString();
                            port.m_PortIsDeviceConnected = (NodeConnection.ConnectionStatus == (int)USB_CONNECTION_STATUS.DeviceConnected);
                            port.m_PortIsHub = Convert.ToBoolean(NodeConnection.DeviceIsHub);
                            port.m_PortDeviceDescriptor = NodeConnection.DeviceDescriptor;

                            // add it to the list 
                            PortList.Add(port);
                        }
                    }
                    Marshal.FreeHGlobal(ptrNodeConnection);
                    CloseHandle(h);
                }
                // convert it into a Collection 
                return PortList;
            }
            #endregion
        }

        /// <summary>
        /// USB Port
        /// </summary>
        public class USBPort
        {
            internal int m_PortPortNumber;
            internal string m_PortStatus, m_PortHubDevicePath, m_PortSpeed;
            internal bool m_PortIsHub, m_PortIsDeviceConnected;
            internal USB_DEVICE_DESCRIPTOR m_PortDeviceDescriptor;

            /// <summary>
            /// 
            /// </summary>
            public USBPort()
            {
                m_PortPortNumber = 0;
                m_PortStatus = "";
                m_PortHubDevicePath = "";
                m_PortSpeed = "";
                m_PortIsHub = false;
                m_PortIsDeviceConnected = false;
            }

            /// <summary>
            /// 
            /// </summary>
            public int PortNumber
            {
                get { return m_PortPortNumber; }
            }

            /// <summary>
            /// return the Device Path of the Hub
            /// </summary>
            public string HubDevicePath
            {
                get { return m_PortHubDevicePath; }
            }

            /// <summary>
            /// the status (see USB_CONNECTION_STATUS above) 
            /// </summary>
            public string Status
            {
                get { return m_PortStatus; }
            }

            /// <summary>
            /// the speed of the connection (see USB_DEVICE_SPEED above) 
            /// </summary>
            public string Speed
            {
                get { return m_PortSpeed; }
            }

            /// <summary>
            /// is this a downstream external hub? 
            /// </summary>
            public bool IsHub
            {
                get { return m_PortIsHub; }
            }

            /// <summary>
            /// 
            /// </summary>
 
            public bool IsDeviceConnected
            {
                get { return m_PortIsDeviceConnected; }
            }

            /// <summary>
            /// return a down stream external hub 
            /// </summary>
            /// <returns>return a down stream external hub </returns>
            public USBDevice GetDevice()
            {
                if (!m_PortIsDeviceConnected)
                {
                    return null;
                }
                USBDevice device = new USBDevice();

                // Copy over some values from the Port class 
                // Ya know, I've given some thought about making Device a derived class... 
                device.PortNumber = m_PortPortNumber;
                device.HubDevicePath = m_PortHubDevicePath;
                device.Descriptor = m_PortDeviceDescriptor;

                // Open a handle to the Hub device 
                IntPtr h = CreateFile(m_PortHubDevicePath, GENERIC_WRITE, FILE_SHARE_WRITE, 
                    IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
                if (h.ToInt32() != INVALID_HANDLE_VALUE)
                {
                    int nBytesReturned;
                    int nBytes = BUFFER_SIZE;
                    // We use this to zero fill a buffer 
                    string NullString = new string((char)0, BUFFER_SIZE / Marshal.SystemDefaultCharSize);

                    // The iManufacturer, iProduct and iSerialNumber entries in the 
                    // Device Descriptor are really just indexes.  So, we have to  
                    // request a String Descriptor to get the values for those strings. 

                    if (m_PortDeviceDescriptor.iManufacturer > 0)
                    {
                        // build a request for string descriptor 
                        USB_DESCRIPTOR_REQUEST Request = new USB_DESCRIPTOR_REQUEST();
                        Request.ConnectionIndex = m_PortPortNumber;
                        Request.SetupPacket.wValue = (short)((USB_STRING_DESCRIPTOR_TYPE << 8) + m_PortDeviceDescriptor.iManufacturer);
                        Request.SetupPacket.wLength = (short)(nBytes - Marshal.SizeOf(Request));
                        Request.SetupPacket.wIndex = 0x409; // Language Code 
                        // Geez, I wish C# had a Marshal.MemSet() method 
                        IntPtr ptrRequest = Marshal.StringToHGlobalAuto(NullString);
                        Marshal.StructureToPtr(Request, ptrRequest, true);

                        // Use an IOCTL call to request the String Descriptor 
                        if (DeviceIoControl(h, IOCTL_USB_GET_DESCRIPTOR_FROM_NODE_CONNECTION,
                            ptrRequest, nBytes, ptrRequest, nBytes, out nBytesReturned, IntPtr.Zero))
                        {
                            // The location of the string descriptor is immediately after 
                            // the Request structure.  Because this location is not "covered" 
                            // by the structure allocation, we're forced to zero out this 
                            // chunk of memory by using the StringToHGlobalAuto() hack above 
                            IntPtr ptrStringDesc = new IntPtr(ptrRequest.ToInt32() + Marshal.SizeOf(Request));
                            USB_STRING_DESCRIPTOR StringDesc = (USB_STRING_DESCRIPTOR)Marshal.PtrToStructure(ptrStringDesc, typeof(USB_STRING_DESCRIPTOR));
                            device.Manufacturer = StringDesc.bString;
                        }
                        Marshal.FreeHGlobal(ptrRequest);
                    }
                    if (m_PortDeviceDescriptor.iProduct > 0)
                    {
                        // build a request for string descriptor 
                        USB_DESCRIPTOR_REQUEST Request = new USB_DESCRIPTOR_REQUEST();
                        Request.ConnectionIndex = m_PortPortNumber;
                        Request.SetupPacket.wValue = (short)((USB_STRING_DESCRIPTOR_TYPE << 8) + m_PortDeviceDescriptor.iProduct);
                        Request.SetupPacket.wLength = (short)(nBytes - Marshal.SizeOf(Request));
                        Request.SetupPacket.wIndex = 0x409; // Language Code 
                        // Geez, I wish C# had a Marshal.MemSet() method 
                        IntPtr ptrRequest = Marshal.StringToHGlobalAuto(NullString);
                        Marshal.StructureToPtr(Request, ptrRequest, true);

                        // Use an IOCTL call to request the String Descriptor 
                        if (DeviceIoControl(h, IOCTL_USB_GET_DESCRIPTOR_FROM_NODE_CONNECTION, 
                            ptrRequest, nBytes, ptrRequest, nBytes, out nBytesReturned, IntPtr.Zero))
                        {
                            // the location of the string descriptor is immediately after the Request structure 
                            IntPtr ptrStringDesc = new IntPtr(ptrRequest.ToInt32() + Marshal.SizeOf(Request));
                            USB_STRING_DESCRIPTOR StringDesc = (USB_STRING_DESCRIPTOR)Marshal.PtrToStructure(ptrStringDesc, typeof(USB_STRING_DESCRIPTOR));
                            device.Product = StringDesc.bString;
                        }
                        Marshal.FreeHGlobal(ptrRequest);
                    }
                    if (m_PortDeviceDescriptor.iSerialNumber > 0)
                    {
                        // build a request for string descriptor 
                        USB_DESCRIPTOR_REQUEST Request = new USB_DESCRIPTOR_REQUEST();
                        Request.ConnectionIndex = m_PortPortNumber;
                        Request.SetupPacket.wValue = (short)((USB_STRING_DESCRIPTOR_TYPE << 8) + m_PortDeviceDescriptor.iSerialNumber);
                        Request.SetupPacket.wLength = (short)(nBytes - Marshal.SizeOf(Request));
                        Request.SetupPacket.wIndex = 0x409; // Language Code 
                        // Geez, I wish C# had a Marshal.MemSet() method 
                        IntPtr ptrRequest = Marshal.StringToHGlobalAuto(NullString);
                        Marshal.StructureToPtr(Request, ptrRequest, true);

                        // Use an IOCTL call to request the String Descriptor 
                        if (DeviceIoControl(h, IOCTL_USB_GET_DESCRIPTOR_FROM_NODE_CONNECTION, 
                            ptrRequest, nBytes, ptrRequest, nBytes, out nBytesReturned, IntPtr.Zero))
                        {
                            // the location of the string descriptor is immediately after the Request structure 
                            IntPtr ptrStringDesc = new IntPtr(ptrRequest.ToInt32() + Marshal.SizeOf(Request));
                            USB_STRING_DESCRIPTOR StringDesc = (USB_STRING_DESCRIPTOR)Marshal.PtrToStructure(ptrStringDesc, typeof(USB_STRING_DESCRIPTOR));
                            device.SerialNumber = StringDesc.bString;
                        }
                        Marshal.FreeHGlobal(ptrRequest);
                    }

                    // Get the Driver Key Name (usefull in locating a device) 
                    USB_NODE_CONNECTION_DRIVERKEY_NAME DriverKey = new USB_NODE_CONNECTION_DRIVERKEY_NAME();
                    DriverKey.ConnectionIndex = m_PortPortNumber;
                    nBytes = Marshal.SizeOf(DriverKey);
                    IntPtr ptrDriverKey = Marshal.AllocHGlobal(nBytes);
                    Marshal.StructureToPtr(DriverKey, ptrDriverKey, true);

                    // Use an IOCTL call to request the Driver Key Name 
                    if (DeviceIoControl(h, IOCTL_USB_GET_NODE_CONNECTION_DRIVERKEY_NAME, 
                        ptrDriverKey, nBytes, ptrDriverKey, nBytes, out nBytesReturned, IntPtr.Zero))
                    {
                        DriverKey = (USB_NODE_CONNECTION_DRIVERKEY_NAME)Marshal.PtrToStructure(ptrDriverKey, typeof(USB_NODE_CONNECTION_DRIVERKEY_NAME));
                        device.DriverKey = DriverKey.DriverKeyName;

                        // use the DriverKeyName to get the Device Description and Instance ID 
                        device.Name = GetDescriptionByKeyName(device.DriverKey);
                        device.DeviceInstanceID = GetInstanceIDByKeyName(device.DriverKey);
                    }
                    Marshal.FreeHGlobal(ptrDriverKey);
                    CloseHandle(h);
                }
                return device;
            }

            /// <summary>
            /// return a down stream external hub 
            /// </summary>
            /// <returns></returns>
            public USBHub GetHub()
            {
                if (!m_PortIsHub)
                {
                    return null;
                }
                USBHub Hub = new USBHub();
                IntPtr h, h2;
                Hub.m_HubIsRootHub = false;
                Hub.m_HubDeviceDesc = "External Hub";

                // Open a handle to the Host Controller 
                h = CreateFile(m_PortHubDevicePath, GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
                if (h.ToInt32() != INVALID_HANDLE_VALUE)
                {
                    // Get the DevicePath for downstream hub 
                    int nBytesReturned;
                    USB_NODE_CONNECTION_NAME NodeName = new USB_NODE_CONNECTION_NAME();
                    NodeName.ConnectionIndex = m_PortPortNumber;
                    int nBytes = Marshal.SizeOf(NodeName);
                    IntPtr ptrNodeName = Marshal.AllocHGlobal(nBytes);
                    Marshal.StructureToPtr(NodeName, ptrNodeName, true);

                    // Use an IOCTL call to request the Node Name 
                    if (DeviceIoControl(h, IOCTL_USB_GET_NODE_CONNECTION_NAME, ptrNodeName, nBytes, ptrNodeName, nBytes, out nBytesReturned, IntPtr.Zero))
                    {
                        NodeName = (USB_NODE_CONNECTION_NAME)Marshal.PtrToStructure(ptrNodeName, typeof(USB_NODE_CONNECTION_NAME));
                        Hub.m_HubDevicePath = @"\\.\" + NodeName.NodeName;
                    }

                    // Now let's open the Hub (based upon the HubName we got above) 
                    h2 = CreateFile(Hub.m_HubDevicePath, GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
                    if (h2.ToInt32() != INVALID_HANDLE_VALUE)
                    {
                        USB_NODE_INFORMATION NodeInfo = new USB_NODE_INFORMATION();
                        NodeInfo.NodeType = (int)USB_HUB_NODE.UsbHub;
                        nBytes = Marshal.SizeOf(NodeInfo);
                        IntPtr ptrNodeInfo = Marshal.AllocHGlobal(nBytes);
                        Marshal.StructureToPtr(NodeInfo, ptrNodeInfo, true);

                        // get the Hub Information 
                        if (DeviceIoControl(h2, IOCTL_USB_GET_NODE_INFORMATION, ptrNodeInfo, nBytes, ptrNodeInfo, nBytes, out nBytesReturned, IntPtr.Zero))
                        {
                            NodeInfo = (USB_NODE_INFORMATION)Marshal.PtrToStructure(ptrNodeInfo, typeof(USB_NODE_INFORMATION));
                            Hub.m_HubIsBusPowered = Convert.ToBoolean(NodeInfo.HubInformation.HubIsBusPowered);
                            Hub.m_HubPortCount = NodeInfo.HubInformation.HubDescriptor.bNumberOfPorts;
                        }
                        Marshal.FreeHGlobal(ptrNodeInfo);
                        CloseHandle(h2);
                    }

                    // Fill in the missing Manufacture, Product, and SerialNumber values 
                    // values by just creating a Device instance and copying the values 
                    USBDevice Device = GetDevice();
                    Hub.m_HubInstanceID = Device.DeviceInstanceID;
                    Hub.m_HubManufacturer = Device.Manufacturer;
                    Hub.m_HubProduct = Device.Product;
                    Hub.m_HubSerialNumber = Device.SerialNumber;
                    Hub.m_HubDriverKey = Device.DriverKey;

                    Marshal.FreeHGlobal(ptrNodeName);
                    CloseHandle(h);
                }
                return Hub;
            }
        }
        /// <summary>
        /// USB device's Description
        /// </summary>
        /// <param name="DriverKeyName"></param>
        /// <returns></returns>
        static string GetDescriptionByKeyName(string DriverKeyName)
        {
            string ans = "";
            string DevEnum = REGSTR_KEY_USB;

            // Use the "enumerator form" of the SetupDiGetClassDevs API  
            // to generate a list of all USB devices 
            IntPtr h = SetupDiGetClassDevs(0, DevEnum, IntPtr.Zero, DIGCF_PRESENT | DIGCF_ALLCLASSES);
            if (h.ToInt32() != INVALID_HANDLE_VALUE)
            {
                IntPtr ptrBuf = Marshal.AllocHGlobal(BUFFER_SIZE);
                string KeyName;

                bool Success;
                int i = 0;
                do
                {
                    // create a Device Interface Data structure 
                    SP_DEVINFO_DATA da = new SP_DEVINFO_DATA();
                    da.cbSize = Marshal.SizeOf(da);

                    // start the enumeration  
                    Success = SetupDiEnumDeviceInfo(h, i, ref da);
                    if (Success)
                    {
                        int RequiredSize = 0;
                        int RegType = REG_SZ;
                        KeyName = "";

                        if (SetupDiGetDeviceRegistryProperty(h, ref da, SPDRP_DRIVER, ref RegType, ptrBuf, BUFFER_SIZE, ref RequiredSize))
                        {
                            KeyName = Marshal.PtrToStringAuto(ptrBuf);
                        }

                        // is it a match? 
                        if (KeyName == DriverKeyName)
                        {
                            if (SetupDiGetDeviceRegistryProperty(h, ref da, SPDRP_DEVICEDESC, ref RegType, ptrBuf, BUFFER_SIZE, ref RequiredSize))
                            {
                                ans = Marshal.PtrToStringAuto(ptrBuf);
                            }
                            break;
                        }
                    }
                    i++;
                } while (Success);

                Marshal.FreeHGlobal(ptrBuf);
                SetupDiDestroyDeviceInfoList(h);
            }
            return ans;
        }
        /// <summary>
        /// Find a USB device's Instance ID. 
        /// </summary>
        /// <param name="DriverKeyName"></param>
        /// <returns></returns>
        private static string GetInstanceIDByKeyName(string DriverKeyName)
        {
            string ans = "";
            string DevEnum = REGSTR_KEY_USB;

            // Use the "enumerator form" of the SetupDiGetClassDevs API  
            // to generate a list of all USB devices  
            IntPtr h = SetupDiGetClassDevs(0, DevEnum, IntPtr.Zero, DIGCF_PRESENT | DIGCF_ALLCLASSES);
            if (h.ToInt32() != INVALID_HANDLE_VALUE)
            {
                IntPtr ptrBuf = Marshal.AllocHGlobal(BUFFER_SIZE);
                string KeyName;

                bool Success;
                int i = 0;
                do
                {
                    // create a Device Interface Data structure 
                    SP_DEVINFO_DATA da = new SP_DEVINFO_DATA();
                    da.cbSize = Marshal.SizeOf(da);

                    // start the enumeration  
                    Success = SetupDiEnumDeviceInfo(h, i, ref da);
                    if (Success)
                    {
                        int RequiredSize = 0;
                        int RegType = REG_SZ;

                        KeyName = "";
                        if (SetupDiGetDeviceRegistryProperty(h, ref da, SPDRP_DRIVER,
                            ref RegType, ptrBuf, BUFFER_SIZE, ref RequiredSize))
                        {
                            KeyName = Marshal.PtrToStringAuto(ptrBuf);
                        }

                        // is it a match? 
                        if (KeyName == DriverKeyName)
                        {
                            int nBytes = BUFFER_SIZE;
                            StringBuilder sb = new StringBuilder(nBytes);
                            SetupDiGetDeviceInstanceId(h, ref da, sb, nBytes, out RequiredSize);
                            ans = sb.ToString();
                            break;
                        }
                    }
                    i++;
                } while (Success);

                Marshal.FreeHGlobal(ptrBuf);
                SetupDiDestroyDeviceInfoList(h);
            }
            return ans;
        }
        /// <summary>
        /// Get list of all USB devices present.
        /// </summary>
        /// <returns>List of instance ID's.</returns>
        public static List<string> GetAllDevicesPresent()
        {
            string ans = "";
            string DevEnum = REGSTR_KEY_USB;
            List<string> list = new List<string>();

            // Use the "enumerator form" of the SetupDiGetClassDevs API  
            // to generate a list of all USB devices  
            IntPtr h = SetupDiGetClassDevs(0, DevEnum, IntPtr.Zero, DIGCF_PRESENT | DIGCF_ALLCLASSES);
            if (h.ToInt32() != INVALID_HANDLE_VALUE)
            {
                IntPtr ptrBuf = Marshal.AllocHGlobal(BUFFER_SIZE);
                string KeyName;

                bool Success;
                int i = 0;
                do
                {
                    // create a Device Interface Data structure 
                    SP_DEVINFO_DATA da = new SP_DEVINFO_DATA();
                    da.cbSize = Marshal.SizeOf(da);

                    // start the enumeration  
                    Success = SetupDiEnumDeviceInfo(h, i, ref da);
                    if (Success)
                    {
                        int RequiredSize = 0;
                        int RegType = REG_SZ;

                        KeyName = "";
                        if (SetupDiGetDeviceRegistryProperty(h, ref da, SPDRP_HARDWAREID,
                            ref RegType, ptrBuf, BUFFER_SIZE, ref RequiredSize))
                        {
                            KeyName = Marshal.PtrToStringAuto(ptrBuf);
                        }

                        int nBytes = BUFFER_SIZE;
                        StringBuilder sb = new StringBuilder(nBytes);
                        SetupDiGetDeviceInstanceId(h, ref da, sb, nBytes, out RequiredSize);
                        ans = sb.ToString();

                        list.Add(ans);

                    }
                    i++;
                } while (Success);

                Marshal.FreeHGlobal(ptrBuf);
                SetupDiDestroyDeviceInfoList(h);
            }
            return list;
        }

        /// <summary>
        /// Usb manager
        /// </summary>
        public partial class UsbManager : IDisposable
        {
            #region Member Data
            private ManagementEventWatcher m_CreationEvents = null;
            private ManagementEventWatcher m_DeletionEvents = null;
            private ManagementScope m_Scope;
            private EventArrivedEventHandler m_CreationHandler;
            private EventArrivedEventHandler m_DeletionHandler;
            #endregion

            #region Constructors and Destructor
            /// <summary>
            /// 
            /// </summary>
            public UsbManager()
            {
                // Bind to local machine
                m_Scope = new ManagementScope("root\\CIMV2");
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="path"></param>
            public UsbManager(string path)
            {
                // Bind to local machine, default
                m_Scope = new ManagementScope(path);
            }
            #endregion

            #region Methods
            /// <summary>
            /// 
            /// </summary>
            /// <param name="handler"></param>
            /// <returns></returns>
            public string SubScribeToCreationEvents(EventArrivedEventHandler handler)
            {
                WqlEventQuery q = new WqlEventQuery();
                q.EventClassName = "__InstanceCreationEvent";
                q.WithinInterval = new TimeSpan(0, 0, 3);
                q.Condition =
                    @"TargetInstance ISA 'Win32_USBControllerDevice' ";
                m_CreationHandler = handler;
                m_CreationEvents = new ManagementEventWatcher(m_Scope, q);
                m_CreationEvents.EventArrived +=
                    new EventArrivedEventHandler(handler);
                m_CreationEvents.Start(); // Start listen for events
                return "Success.";
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="handler"></param>
            /// <returns></returns>
            public string SubScribeToDeletionEvents(EventArrivedEventHandler handler)
            {
                // Bind to local machine
                WqlEventQuery q = new WqlEventQuery();
                q.EventClassName = "__InstanceDeletionEvent";
                q.WithinInterval = new TimeSpan(0, 0, 3);
                q.Condition =
                    @"TargetInstance ISA 'Win32_USBControllerDevice' ";
                m_DeletionHandler = handler;
                m_DeletionEvents = new ManagementEventWatcher(m_Scope, q);
                m_DeletionEvents.EventArrived +=
                    new EventArrivedEventHandler(handler);
                m_DeletionEvents.Start(); // Start listen for events
                return "success.";
            }
            /// <summary>
            /// Unsubcribe USB events.
            /// </summary>
            private void StopListening()
            {
                if (m_CreationEvents != null)
                {
                    m_CreationEvents.EventArrived -=
                        new EventArrivedEventHandler(m_CreationHandler);
                    m_CreationEvents.Stop();
                    m_CreationEvents.Dispose();
                    m_CreationEvents = null;
                }

                if (m_DeletionEvents != null)
                {
                    m_DeletionEvents.EventArrived -=
                        new EventArrivedEventHandler(m_DeletionHandler);
                    m_DeletionEvents.Stop();
                    m_DeletionEvents.Dispose();
                    m_DeletionEvents = null;
                }
            }
            /// <summary>
            /// Releases the resources used by the Diagnostics Log Service.
            /// </summary>
            public void Dispose()
            {
                GC.SuppressFinalize(this);
                Dispose(true);
            }
            /// <summary>
            /// Releases the resources used by the diagnostics logs and optionally releases the managed resources.
            /// </summary>
            /// <param name="disposing"></param>
            private void Dispose(bool disposing)
            {
                if (disposing)
                {
                    StopListening();
                }
            }
            #endregion
        }

        /// <summary>
        /// USB Device
        /// </summary>
        public class USBDevice
        {
            #region Constants
            private const int c_XmlSerializationVersion = 1;
            #endregion

            #region Member Data
            private FileStream m_FileStream;
            private IntPtr m_Handle;
            private int m_DevicePortNumber;
            private string m_DeviceDriverKey;
            private string m_DeviceHubDevicePath;
            private string m_DeviceInstanceID;
            private string m_DeviceName;
            private string m_DeviceManufacturer;
            private string m_DeviceProduct;
            private string m_DeviceSerialNumber;
            private string m_DevicePath;
            private string m_VendorId;
            private string m_ProductId;
            private string m_InstanceId;

            private Usb.USB_DEVICE_DESCRIPTOR m_DeviceDescriptor;
            #endregion

            #region Accessors
            /// <summary>Device path</summary>
            public string DevicePath
            {
                get { return m_DevicePath; }
                internal set { m_DevicePath = value; }
            }
            /// <summary>Vendor ID.</summary>
            public string VendorId
            {
                get { return m_VendorId; }
                internal set { m_VendorId = value; }
            }
            /// <summary>Product ID.</summary>
            public string ProductId
            {
                get { return m_ProductId; }
                internal set { m_ProductId = value; }
            }
            /// <summary>Instance ID. This is not the device instance ID. 
            /// Only the instance ID part of the device instance ID.</summary>
            public string InstanceId
            {
                get { return m_InstanceId; }
                internal set { m_InstanceId = value; }
            }
            /// <summary>
            /// Get descriptor
            /// </summary>
            internal Usb.USB_DEVICE_DESCRIPTOR Descriptor
            {
                get { return m_DeviceDescriptor; }
                set { m_DeviceDescriptor = value; }
            }
            /// <summary>
            /// Port number.
            /// </summary>
            public int PortNumber
            {
                get { return m_DevicePortNumber; }
                internal set { m_DevicePortNumber = value; }
            }
            /// <summary>
            /// Get the Device Path of the Hub (the parent device) 
            /// </summary>
            public string HubDevicePath
            {
                get { return m_DeviceHubDevicePath; }
                internal set { m_DeviceHubDevicePath = value; }
            }
            /// <summary>
            /// Get key
            /// </summary>
            public string DriverKey
            {
                get { return m_DeviceDriverKey; }
                internal set { m_DeviceDriverKey = value; }
            }
            /// <summary>
            /// Get device instance ID
            /// </summary>
            public string DeviceInstanceID
            {
                get { return m_DeviceInstanceID; }
                internal set
                {
                    m_DeviceInstanceID = value;
                    DevicePath = Usb.GetDevicePath(m_DeviceInstanceID);

                    m_VendorId = new VendorId(m_DeviceInstanceID).ToString();
                    m_ProductId = new ProductId(m_DeviceInstanceID).ToString();
                    m_InstanceId = string.Empty;
                    int index = m_DeviceInstanceID.LastIndexOf('\\');
                    if (index >= 0)
                        m_InstanceId = m_DeviceInstanceID.Substring(index + 1);

                }
            }
            /// <summary>
            /// Get name.
            /// </summary>
            public string Name
            {
                get { return m_DeviceName; }
                internal set { m_DeviceName = value; }
            }
            /// <summary>
            /// Get manufacturer
            /// </summary>
            public string Manufacturer
            {
                get { return m_DeviceManufacturer; }
                internal set { m_DeviceManufacturer = value; }
            }
            /// <summary>
            /// Get product
            /// </summary>
            public string Product
            {
                get { return m_DeviceProduct; }
                internal set { m_DeviceProduct = value; }
            }
            /// <summary>
            /// Get serial number
            /// </summary>
            public string SerialNumber
            {
                get { return m_DeviceSerialNumber; }
                internal set { m_DeviceSerialNumber = value; }
            }
            #endregion

            #region Constructors and Destructor
            /// <summary>
            ///  Initializes a new instance of the USBDevice class.
            /// </summary>
            public USBDevice()
            {
                Init();
            }
            #endregion

            #region Methods
            private void Init()
            {
                m_FileStream = null;
                m_Handle = IntPtr.Zero;
                m_DevicePath = string.Empty;
                m_VendorId = string.Empty; 
                m_ProductId = string.Empty; 
                m_InstanceId = string.Empty;
                m_DevicePortNumber = 0;
                m_DeviceHubDevicePath = string.Empty;
                m_DeviceDriverKey = string.Empty;
                m_DeviceManufacturer = string.Empty;
                m_DeviceProduct = "Unknown USB Device";
                m_DeviceSerialNumber = string.Empty;
                m_DeviceName = string.Empty;
                m_DeviceInstanceID = string.Empty;
            }
            /// <summary>
            /// Returns an XML string representing the state of the object.
            /// </summary>
            /// <returns>An XML string representing the state of the object.</returns>
            public string ToXml()
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                settings.ConformanceLevel = ConformanceLevel.Fragment;
                StringBuilder output = new StringBuilder();
                XmlWriter writer = XmlWriter.Create(output, settings);

                writer.WriteStartElement("USBDevice");

                writer.WriteElementString("Version", c_XmlSerializationVersion.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("PortNumber", this.m_DevicePortNumber.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("DriverKey", this.m_DeviceDriverKey.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("HubPath", this.m_DeviceHubDevicePath.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("InstanceID", this.m_DeviceInstanceID.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("Name", this.m_DeviceName.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("Manufacturer", this.m_DeviceManufacturer.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("Product", this.m_DeviceProduct.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("SerialNumber", this.m_DeviceSerialNumber.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("Path", this.m_DevicePath.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("vid", this.m_VendorId.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("pid", this.m_ProductId.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("iid", this.m_InstanceId.ToString(CultureInfo.InvariantCulture));

                writer.WriteEndElement(); // "USBDevice"
                writer.Close();

                return output.ToString();
            }

            /// <summary>
            /// Deserializes from an XML serialization.
            /// </summary>
            /// <param name="xml">the string which contains an appropriate XML fragment.</param>
            /// <returns>true if successful deserialization</returns>
            public bool FromXml(string xml)
            {
                this.Init();

                try
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.ConformanceLevel = ConformanceLevel.Fragment;
                    settings.IgnoreWhitespace = true;
                    XmlReader reader = XmlReader.Create(new System.IO.StringReader(xml),
                       settings);
                    reader.Read();
                    reader.ReadStartElement("USBDevice");
                    string versionString = reader.ReadElementString("Version");
                    int version = System.Convert.ToInt32(versionString, CultureInfo.InvariantCulture);
                    if (version == 1)
                    {
                        this.m_DevicePortNumber = System.Convert.ToInt32(
                          reader.ReadElementString("PortNumber"), CultureInfo.InvariantCulture);

                        this.m_DeviceDriverKey = reader.ReadElementString("DriverKey");
                        this.m_DeviceHubDevicePath = reader.ReadElementString("HubPath");
                        this.m_DeviceInstanceID = reader.ReadElementString("InstanceID");
                        this.m_DeviceName = reader.ReadElementString("Name");
                        this.m_DeviceManufacturer = reader.ReadElementString("Manufacturer");
                        this.m_DeviceProduct = reader.ReadElementString("Product");
                        this.m_DeviceSerialNumber = reader.ReadElementString("SerialNumber");
                        this.m_DevicePath = reader.ReadElementString("Path");
                        this.m_VendorId = reader.ReadElementString("vid");
                        this.m_ProductId = reader.ReadElementString("pid");
                        this.m_InstanceId = reader.ReadElementString("iid");
                    }
                    else
                    {
                        Debug.Assert(false);
                        return false;
                    }
                    reader.ReadEndElement(); // "USBDevice"
                }
                catch (XmlException)
                {
                    Debug.Assert(false);
                    return false;
                }
                catch (FormatException)
                {
                    // One of the conversions failed.
                    Debug.Assert(false);
                    return false;
                }
                catch (OverflowException)
                {
                    // One of the conversions failed.
                    Debug.Assert(false);
                    return false;
                }

                return true;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns>Pair struct where int is win32 error code.</returns>
            private ResultPair<int, string> Open()
            {
                ResultPair<int, string> lastError = new ResultPair<int, string>(0, string.Empty);

                if (m_FileStream != null)
                    m_FileStream.Close();

                m_Handle = Usb.CreateFile(this.DevicePath,
                    Usb.GENERIC_READ | Usb.GENERIC_WRITE,
                    Usb.FILE_SHARE_READ | Usb.FILE_SHARE_WRITE,
                    IntPtr.Zero, Usb.OPEN_EXISTING, Usb.FILE_FLAG_OVERLAPPED, IntPtr.Zero);

                bool success = (m_Handle.ToInt32() == Usb.INVALID_HANDLE_VALUE) ? false : true;

                if (!success)
                {
                    lastError = GetLastError(true);
                }
                else
                {
                    m_FileStream = new FileStream(new SafeFileHandle(m_Handle, false),
                        FileAccess.Read | FileAccess.Write, 64, true);

                    BeginAsyncRead();
                }

                return lastError;
            }
            /// <summary>
            ///
            /// </summary>
            private void Close()
            {
                if (m_FileStream != null)
                    m_FileStream.Close();
                if (m_Handle.ToInt32() != Usb.INVALID_HANDLE_VALUE)
                    Usb.CloseHandle(m_Handle);
                m_Handle = IntPtr.Zero;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="command"></param>
            private string Send(string command)
            {
                string response = string.Empty;
                byte[] bytes = ASCIIEncoding.ASCII.GetBytes(command);
                Write(bytes);
                return response;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            private void Write(byte[] data)
            {
                m_FileStream.Write(data, 0, data.Length);
            }
            /// <summary>
            /// Kicks off an asynchronous read which completes when data is read or when the device
            /// is disconnected. Uses a callback.
            /// </summary>
            private void BeginAsyncRead()
            {
                byte[] data = new byte[64];
                m_FileStream.BeginRead(data, 0, 64, new AsyncCallback(ReadCompleted), data);
            }
            /// <summary>
            /// Callback for above. Care with this as it will be called on the background thread from the async read
            /// </summary>
            /// <param name="iResult">Async result parameter</param>
            private void ReadCompleted(IAsyncResult iResult)
            {
                byte[] buff = (byte[])iResult.AsyncState;	// retrieve the read buffer
                try
                {
                    m_FileStream.EndRead(iResult);	// call end read : this throws any exceptions that happened during the read
                    try
                    {
                    }
                    finally
                    {
                        //BeginAsyncRead();	// when all that is done, kick off another read for the next report
                    }
                }
                catch (Exception ex)	// if we got an IO exception, the device was removed
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return string.Format("Name:'{0}', Product:'{1}', ID:'{2}', Manufacturer:'{3}'",
                    Name, Product, DeviceInstanceID, Manufacturer);
            }
            #endregion
        }
    }
}
