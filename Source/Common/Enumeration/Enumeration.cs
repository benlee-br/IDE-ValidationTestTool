using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using BioRad.Common.Utilities;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BioRad.Common.Enumeration
{
   #region Documentation Tags
   /// <summary>
   /// Class Summary
   /// </summary>
   /// <remarks>
   /// Remarks section for class.
   /// </remarks>
   /// <classinformation>
   ///		<list type="bullet">
   ///			<item name="authors">Authors:</item>
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
   ///			<item name="vssfile">$Workfile: Enumeration.cs $</item>
   ///			<item name="vssfilepath">$Archive: /CFX_15/Source/Core/Common/Enumeration/Enumeration.cs $</item>
   ///			<item name="vssrevision">$Revision: 35 $</item>
   ///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
   ///			<item name="vssdate">$Date: 1/07/09 9:36a $</item>
   ///		</list>
   /// </archiveinformation>
   #endregion

	public partial class EnumerationUtils
    {
		/// <summary>
		/// 
		/// </summary>
		public enum InstrumentTypes
		{
			/// <summary>All MiniOpticon instruments</summary>
			MiniOpticon,
			/// <summary>All CFX instruments</summary>
			CFX,//Sierra,
			/// <summary>Locust</summary>
			Locust,
            /// <summary>
            /// CFX 3G instrument
            /// </summary>
            CFX3G
		}
        #region Constructors and Destructor
        private EnumerationUtils() { }
        #endregion

        #region Methods
		/// <summary>
		/// Get list of file paths for files that contain data of emulated connected instruments.
		/// </summary>
		/// <returns>List of full file paths.</returns>
		public static List<KeyValuePair<Blocks, string>> ConnectedInstrumentsEmulationFiles()
		{
			List<KeyValuePair<Blocks, string>> list = new List<KeyValuePair<Blocks, string>>();
			foreach (InstrumentTypes type in Enum.GetValues(typeof(InstrumentTypes)))
				list.Add(new KeyValuePair<Blocks, string>());

			string path = Path.Combine(ApplicationPath.SimulatedConnectedInstrumentsFolderPath(),
					ApplicationPath.FlagshipConnectedInstrumentsFilename());
			list[(int)InstrumentTypes.CFX] = new KeyValuePair<Blocks, string>(Blocks.FlagshipNonRT, path);

			path = Path.Combine(ApplicationPath.SimulatedConnectedInstrumentsFolderPath(),
					 ApplicationPath.LocustConnectedInstrumentsFilename());
			list[(int)InstrumentTypes.Locust] = new KeyValuePair<Blocks, string>(Blocks.Locust, path);

            path = Path.Combine(ApplicationPath.SimulatedConnectedInstrumentsFolderPath(),
                     ApplicationPath.CFX3GConnectedInstrumentsFilename());
            list[(int)InstrumentTypes.CFX3G] = new KeyValuePair<Blocks, string>(Blocks.CFX3G, path);

            path = Path.Combine(ApplicationPath.SimulatedConnectedInstrumentsFolderPath(),
					ApplicationPath.MiniOpticonConnectedInstrumentsFilename());
			list[(int)InstrumentTypes.MiniOpticon] = new KeyValuePair<Blocks, string>(Blocks.MiniOpticon, path);
				
			return list;
		}
        /// <summary>
        /// Determines whether a block type is enumerable by the legacy MJ enumerator.
        /// </summary>
        /// <param name="block">The block in question.</param>
        /// <returns>true if the block is a type that is enumerable by the legacy MJ
        /// enumeration DLL.</returns>
        public static bool IsBlockMJEnumerable(Blocks block)
        {
            return (block == Blocks.Chromo4 ||
               block == Blocks.MiniOpticon ||
               block == Blocks.NonRTLegacyMJ ||
               block == Blocks.BusyBlockMJ);
        }
        /// <summary>
        /// Returns the full path to the folder that contains files with data for connected instruments.
        /// </summary>
        /// <returns>path to enumerator service directory.</returns>
        public static string GetEnumeratedConnectedInstrumentsFolderName()
        {
            string folder = ApplicationStateData.GetInstance[
              ApplicationStateData.Setting.EnumeratedConnectedInstrumentsFolderName].ToString();
            return Path.Combine(
                BioRad.Common.ApplicationPath.CommonApplicationDataPath(),
                folder);
        }
        /// <summary>
        /// Determine if specified block is a real-time block.
        /// </summary>
        /// <param name="block"></param>
        /// <returns>True if real-time block.</returns>
        public static bool IsBlockRealTime(Blocks block)
        {
            return (block == Blocks.Chromo4 ||
               block == Blocks.MiniOpticon ||
               block == Blocks.IQ5 ||
               block == Blocks.FlagshipRT ||
			   block == Blocks.Locust ||
               block == Blocks.CFX3G);
        }
		/// <summary>
		/// Determine if specified block is a flagship block.
		/// </summary>
		/// <param name="block"></param>
		/// <returns>True if flagship block.</returns>
		public static bool IsBlockFlagship(Blocks block)// MSTR-238 removed CFX3G
		{
            return (block == Blocks.Satellite ||
               block == Blocks.FlagshipNonRT ||
               block == Blocks.FlagshipRT);
        }
        #endregion
    }

   /// <summary>
   /// Strings that are returned by the MJ enumerator to identify devices.
   /// Do not assume these are the complete strings returned by the enumerator.  Rather,
   /// determine whether these are substrings of the strings returned by the enumerator.
   /// These strings are not subject to localization.
   /// </summary>
   internal struct EnumeratorIdStrings
   {
      // Device Classes
      public const string RTBlocks = "MJR Continuous Fluorescence Detectors";
      public const string Cyclers = "MJR Thermal Cyclers";

      // Blocks
      public const string Chromo4 = "Chromo4";
      public const string MiniOpticon = "MiniOpticon";
      //public const string Sierra = "TODO";

      // Cyclers
      public const string DNAEngine = "PTC-200";
      public const string MiniCycler = "PTC-1148";
      //public const string Dyad = "TODO";
      //public const string DyadDisciple = "TODO";
      //public const string Tetrad = "TODO";
      //public const string Tetrad2 = "TODO";
      //public const string Everett = "TODO";
      //public const string Satellite = "TODO";

      // Busy instruments
      public const string BusyBlock = "Device Busy";
   }

   #region Enums
   /// <summary>
   /// Types of blocks capable of being enumerated.
   /// </summary>
   public enum Blocks
   {
      /// <summary>Chromo4 real-time.</summary>
      Chromo4,
      /// <summary>MiniOpticon real-time.</summary>
      MiniOpticon,
      /// <summary>Real-time flagship.</summary>
	  FlagshipRT,//Sierra,
	  /// <summary>Conventional flagship.</summary>
	  FlagshipNonRT,
	  /// <summary></summary>
	  Satellite,
      /// <summary>legacy MJ non-real-time alphas such as 96 well alphas and
      /// dual block 48s and 384s etc.</summary>
      NonRTLegacyMJ,
      /// <summary>BioRad IQ5 real-time.</summary>
      IQ5,
      /// <summary>Denotes the lack of a Block, used with cycler bays that do not
      /// contain a module.</summary>
      None,
      /// <summary>Busy ch4 or mini - can't tell which without opening the port
      /// for communication.</summary>
      BusyBlockMJ,
      /// <summary>uninitialized.</summary>
      Uninitialized,
	  /// <summary>
	  /// Locust block
	  /// </summary>
	  Locust,
      /// <summary>
      /// CFX3G Instrument
      /// </summary>
      CFX3G
   }

   /// <summary>
   /// Block type filter capable of being enumerated.
   /// </summary>
   public enum BlockTypeFilters
   {
       /// <summary>
       /// All Blocks
       /// </summary>
       All,
       /// <summary>
       /// All Flagships
       /// </summary>
       Flagships,
       /// <summary>
       /// All Available Blocks
       /// </summary>
       Available,
       /// <summary>
       /// All Running Blocks
       /// </summary>
       Running,
       /// <summary>
       /// My running blocks
       /// </summary>
       MyRunning
   }

   /// <summary>
   /// types of cycler bases capable of being enumerated.
   /// </summary>
   public enum CyclerBases
   {
      /// <summary>MJ DNA Engine, aka ptc-200. Connects by RS232.</summary>
      DnaEngine,
      /// <summary>MJ Mini cycler, aka ptc-1148. This is also the base used
      /// by MiniOpticons.  Connects by USB.</summary>
      MiniCycler,
      /// <summary>MJ Dyad base, 2 bays. Connects by RS232.</summary>
      Dyad,
      /// <summary>MJ Dyad Disciple base, 2 bays, connects by USB.</summary>
      DyadDisciple,
      /// <summary>MJ Tetrad base, 4 bays. Connects by RS232.</summary>
      Tetrad,
      /// <summary>MJ Tetrad2 base, 4 bays. Connects by RS232.</summary>
      Tetrad2,
      /// <summary>Bio-Rad's Everett flagship cycler base. Connects by USB.</summary>
      Everett,
      /// <summary>Bio-Rad's Satellite cycler base. Connects by USB.</summary>
      Satellite,
      /// <summary>Denotes the lack of a base, used with detected blocks for 
      /// which a connected base cannot be found.</summary>
      None,
      /// <summary>uninitialized</summary>
      Uninitialized,
	  /// <summary>
	  /// 
	  /// </summary>
	  Locust,
      /// <summary>
      /// 
      /// </summary>
      CFX3G
   }
   #endregion

   /// <summary>
   /// These are the keys for use with the dictionary of connection parameters.
   /// </summary>
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
   public struct ConnectionTags
   {
      /// <summary>
      /// 
      /// </summary>
      public static readonly string MJConnectionParameter = "medium";
      /// <summary>
      /// 
      /// </summary>
      public static readonly string MJDeviceInstanceNum = "deviceNum";
      /// <summary>
      /// 
      /// </summary>
      public static readonly string MJDeviceInstanceNum2 = "deviceNum2";
      /// <summary>
      /// 
      /// </summary>
      public static readonly string MJUnitOpts = "unitOpts";
   }

   /// <summary>
   /// Connected instrument
   /// </summary>
   public class ConnectedInstrument : IComparer<ConnectedInstrument>
   {
      #region Member Data
      private string m_DevicePath;
      private string m_DeviceInstanceId;
      private Blocks m_block;
      private CyclerBases m_cyclerBase;
      private string m_blockDisplayId;
      private string m_bayDisplayId;
      private int m_bayNumber;
      private int m_blockNumber;
      private int m_blockCount;
      private Dictionary<string, string> m_blockConnectionParameters
         = new Dictionary<string, string>();
      private Dictionary<string, string> m_cyclerConnectionParameters
         = new Dictionary<string, string>();
      private const int xmlSerializationVersion = 1;
      #endregion

      #region Accessors
      /// <summary>
      /// A device instance ID is a system-supplied device identification string that 
      /// uniquely identifies a device in the system. 
      /// The PnP manager assigns a device instance ID to 
      /// each device node in a system's device tree.
      /// </summary>
      public string DeviceInstanceId
      {
          get { return m_DeviceInstanceId; }
          set { m_DeviceInstanceId = value; }
      }
      /// <summary>A hardware ID is a vendor-defined identification string that 
      /// Setup uses to match a device to an INF file.</summary>
       public string DevicePath
       {
           get { return m_DevicePath; }
           set { m_DevicePath = value; }
       }
	  /// <summary>
	  /// Make displayable name.
	  /// </summary>
	  /// <param name="serialNumber">Serial number or nick name.</param>
	  /// <param name="blockLetter">Block letter "A" or "B"</param>
      /// <param name="isDualBlock">dual block</param>
	  /// <returns></returns>
	  public static string MakeDisplayableName(
          string serialNumber, 
          char blockLetter,
          bool isDualBlock)
	  {
		  if (string.IsNullOrEmpty(serialNumber))
			  throw new ArgumentNullException("serialNumber");

          if (isDualBlock)
          {
              // name is combintaion of serial number and block number.
              return string.Format("{0}.{1}",
                  serialNumber.Length > 0 ? serialNumber : "***", blockLetter);
          }
          else
          {
              // name is just the serial number.
              return serialNumber;
          }
	  }
      /// <summary>type of block, possibly on a multi-block module.</summary>
      public Blocks Block
      {
         get { return m_block; }
         set { m_block = value; }
      }
      /// <summary>Type of cycler, possibly a multi-bay one.</summary>
      public CyclerBases CyclerBase
      {
         get { return m_cyclerBase; }
         set { m_cyclerBase = value; }
      }
      /// <summary>UI identifier for the block (e.g. "Snoopy - block 2")</summary>
      public string BlockDisplayId
      {
         get { return m_blockDisplayId; }
         set { m_blockDisplayId = value; }
      }
      /// <summary>UI identifier for the bay (e.g. "Woodstock - bay 3")</summary>
      public string BayDisplayId
      {
         get { return m_bayDisplayId; }
         set { m_bayDisplayId = value; }
      }
      /// <summary>zero-based bay index on the base</summary>
      public int BayNumber
      {
         get { return m_bayNumber; }
         set { m_bayNumber = value; }
      }
      /// <summary>zero-based block index on the module</summary>
      public int BlockNumber
      {
         get { return m_blockNumber; }
         set { m_blockNumber = value; }
      }
      /// <summary>any necessary information to allow the application to connect to this
      /// block.  This object can be interpreted in context by other code
      /// depending on what sort of instrument the rest of the structure represents.
      /// </summary>
      public Dictionary<string, string> BlockConnectionParameters
      {
         get { return m_blockConnectionParameters; }
         set { m_blockConnectionParameters = value; }
      }
      /// <summary>any necessary information to allow the application to connect to this
      /// cycler.  This object can be interpreted in context by other code
      /// depending on what sort of instrument the rest of the structure represents.
      /// </summary>
      public Dictionary<string, string> CyclerConnectionParameters
      {
         get { return m_cyclerConnectionParameters; }
         set { m_cyclerConnectionParameters = value; }
      }
      /// <summary>
      /// Get number of blocks.
      /// </summary>
       public int BlockCount
       {
           get{return m_blockCount;}
           set { m_blockCount = value; }
       }
       /// <summary>
       /// Determine if block is a dual block.
       /// </summary>
       public bool IsDualBlock
       {
           get
           {
               return (BlockCount > 1) ? true: false;
           }
       }
      #endregion

      #region Constructors and Destructor
      /// <summary>
      /// Creates an uninitialized object.
      /// </summary>
      public ConnectedInstrument()
      {
         PreInitialize();
      }

      /// <summary>Creates and initializes an object</summary>
      /// <param name="block">block</param>
      /// <param name="cyclerBase">cyclerBase</param>
      /// <param name="blockDisplayId">blockDisplayID</param>
      /// <param name="bayDisplayId">bayDisplayID</param>
      /// <param name="bayNumber">bayNumber</param>
      /// <param name="blockNumber">blockNumber</param>
      /// <param name="blockConnectionParameters">blockConnectionParameters</param>
      /// <param name="cyclerConnectionParameters">cyclerConnectionParameters</param>
      public ConnectedInstrument(
         Blocks block,
         CyclerBases cyclerBase,
         string blockDisplayId,
         string bayDisplayId,
         int bayNumber,
         int blockNumber,
         Dictionary<string, string> blockConnectionParameters,
         Dictionary<string, string> cyclerConnectionParameters)
      {
         PreInitialize();
         this.Block = block;
         this.CyclerBase = cyclerBase;
         this.BlockDisplayId = blockDisplayId;
         this.BayDisplayId = bayDisplayId;
         this.BayNumber = bayNumber;
         this.BlockNumber = blockNumber;
         this.BlockConnectionParameters = blockConnectionParameters;
         this.CyclerConnectionParameters = cyclerConnectionParameters;
      }
      #endregion

      #region Methods
      /// <summary>
      /// Returns a human readable string representing the state of the object.
      /// </summary>
      /// <returns>a human readable string representing the state of the
      /// object.</returns>
      public override string ToString()
      {
         System.Text.StringBuilder ret = new StringBuilder();
         FieldInfo[] fieldInfos = this.GetType().GetFields(
            BindingFlags.NonPublic | BindingFlags.Instance);
         foreach (FieldInfo fieldInfo in fieldInfos)
         {
            ret.Append(fieldInfo.Name);
            ret.Append(": ");
            ret.Append(fieldInfo.GetValue(this).ToString());
            ret.Append("\r\n");
         }
         return ret.ToString();
      }

      /// <summary>
      /// Initializes this object with data from the MJ enumerator.
      /// </summary>
      /// <param name="enumeratorData">The "value" from one of the key/value 
      /// pairs created by the MJ enumerator, which should denote a block.</param>
      /// <returns>Returns true if the parameter denotes a block and the object 
      /// was fully initialized, false if the object is not fully initialized, 
      /// perhaps due to the parameter denoting something other than a block, 
      /// such as the MJ logger process or a NiDaq card or a cycler base.</returns>
      public bool InitializeFromEnumeratorData(string enumeratorData)
      {
         const int len1 = 10;
         const int len2 = 19;
         PreInitialize();

         // Split the enumerator data string into its component parts.
         string[] splitData = enumeratorData.Split(';');

         if (splitData.Length != len1 && splitData.Length != len2)
         {
            Debug.Assert(false);
            return false;
         }

         this.DevicePath = string.Empty;
         for (bool isAssociatedDevice = false; ; isAssociatedDevice = true)
         {
            int offset = (isAssociatedDevice ? 9 : 0);
            this.DevicePath = splitData[splitData.Length - 1];
            int deviceInstanceNum = Convert.ToInt32(splitData[0 + offset],
               CultureInfo.InvariantCulture);
            int deviceInstanceNum2 = Convert.ToInt32(splitData[1 + offset],
               CultureInfo.InvariantCulture);
            int communicationsMedium = Convert.ToInt32(splitData[2 + offset],
               CultureInfo.InvariantCulture);
            string deviceName = splitData[3 + offset];
            string deviceType = splitData[4 + offset];
            string serialNumber = splitData[5 + offset];
            //bool busy = (splitData[6 + offset] == "B");
            bool associated = (splitData[8 + offset] == "A");

            if (isAssociatedDevice == false)
            { // this is the device defined by the first 9 values in splitData.
               if (deviceType.IndexOf(EnumeratorIdStrings.RTBlocks) >= 0)
               {
                  if (deviceName.IndexOf(EnumeratorIdStrings.MiniOpticon) >= 0)
                     Block = Blocks.MiniOpticon;
                  else if (deviceName.IndexOf(EnumeratorIdStrings.Chromo4) >= 0)
                     Block = Blocks.Chromo4;
                  else if (deviceName.IndexOf(EnumeratorIdStrings.BusyBlock) >= 0)
                     Block = Blocks.BusyBlockMJ;
                  else
                     Debug.Assert(false); // New machine type?
                  BlockDisplayId = serialNumber;
                  BlockNumber = deviceInstanceNum2;
                  BlockConnectionParameters.Clear();
                  switch (Block)
                  {
                     case Blocks.MiniOpticon:
                        BlockConnectionParameters.Add(ConnectionTags.MJConnectionParameter,
                           4.ToString(CultureInfo.InvariantCulture));
                        break;
                     case Blocks.Chromo4:
                        BlockConnectionParameters.Add(ConnectionTags.MJConnectionParameter,
                           4.ToString(CultureInfo.InvariantCulture));
                        break;
                     case Blocks.BusyBlockMJ:
                        break;
                     default:
                        Debug.Assert(false);
                        break;
                  }
                  BlockConnectionParameters.Add(ConnectionTags.MJDeviceInstanceNum,
                     deviceInstanceNum.ToString(CultureInfo.InvariantCulture));
                  BlockConnectionParameters.Add(ConnectionTags.MJDeviceInstanceNum2,
                     deviceInstanceNum2.ToString(CultureInfo.InvariantCulture));
                  if (Block == Blocks.MiniOpticon && associated)
                  {
                      if (splitData.Length != len2)
                        Debug.Assert(false);
                     else
                     {
                        StringBuilder unitOpts = new StringBuilder();
                        unitOpts.Append("asn:").Append(serialNumber).Append("\n");
                        unitOpts.Append("cycler.conn:usb").Append(deviceInstanceNum.
                           ToString(CultureInfo.InvariantCulture)).Append("\n");
                        unitOpts.Append("instr.conn:usb").Append(splitData[9]).
                           Append("\n");
                        unitOpts.Append("msn:0\n");
                        unitOpts.Append("scanstring:").Append(enumeratorData).Append(
                           "\n");
                        BlockConnectionParameters.Add(ConnectionTags.MJUnitOpts,
                           unitOpts.ToString());
                     }
                  }

                  if (associated == false)
                  {
                     CyclerBase = CyclerBases.None;
                     BayNumber = 0;
                     BayDisplayId = "None";
                     CyclerConnectionParameters.Clear();
                     break;
                  }
               }
               else // Not a block
                  break;
            }
            else
            { // this is the device defined by the last 9 values in splitData.
               if (deviceType.IndexOf(EnumeratorIdStrings.Cyclers) == -1)
               {
                  Debug.Assert(false);
                  break;
               }
               if (deviceName.IndexOf(EnumeratorIdStrings.DNAEngine) >= 0)
                  CyclerBase = CyclerBases.DnaEngine;
               else if (deviceName.IndexOf(EnumeratorIdStrings.MiniCycler) >= 0)
                  CyclerBase = CyclerBases.MiniCycler;
               else
               {
                  Debug.Assert(false);
                  CyclerBase = CyclerBases.None;
               }
               BayNumber = deviceInstanceNum2;
               BayDisplayId = serialNumber;
               CyclerConnectionParameters.Clear();
               switch (CyclerBase)
               {
                  case CyclerBases.MiniCycler:
                     CyclerConnectionParameters.Add(ConnectionTags.MJConnectionParameter,
                        3.ToString(CultureInfo.InvariantCulture));
                     break;
                  case CyclerBases.DnaEngine:
                     CyclerConnectionParameters.Add(ConnectionTags.MJConnectionParameter,
                        0.ToString(CultureInfo.InvariantCulture));
                     break;
                  default:
                     Debug.Assert(false);
                     break;
               }
               CyclerConnectionParameters.Add(ConnectionTags.MJDeviceInstanceNum,
                  deviceInstanceNum.ToString(CultureInfo.InvariantCulture));
               CyclerConnectionParameters.Add(ConnectionTags.MJDeviceInstanceNum2,
                  deviceInstanceNum2.ToString(CultureInfo.InvariantCulture));
               break;
            }
         }

         if (IsInitialized())
            return true;
         return false;
      }
		/// <summary>
      /// Fills in the connection parameters necessary for simulation.
		/// </summary>
       public void EmulateConnection()
       {
           BlockConnectionParameters.Remove(ConnectionTags.MJUnitOpts);
           BlockConnectionParameters.Add(ConnectionTags.MJUnitOpts,
               "emulator:true");
       }
      /// <summary>
      /// Sets all internal variables to uninitialized state.
      /// </summary>
      private void PreInitialize()
      {
         DeviceInstanceId = string.Empty;
         DevicePath = string.Empty;
         Block = Blocks.Uninitialized;
         CyclerBase = CyclerBases.Uninitialized;
         BlockDisplayId = "__uninitialized";
         BayDisplayId = "__uninitialized";
         BayNumber = -1;
         BlockNumber = -1;
         BlockConnectionParameters.Clear();
         CyclerConnectionParameters.Clear();
      }

      /// <summary>
      /// Checks whether the object has been initialized.
      /// </summary>
      /// <returns>True if the object is in an initialized state.</returns>
      public bool IsInitialized()
      {
         return (Block != Blocks.Uninitialized &&
            CyclerBase != CyclerBases.Uninitialized);
      }

      /// <summary>
      /// Implementation of the IComparable interface.  Compares two of these
      /// objects, for use with sorting.
      /// </summary>
      /// <param name="x">first operand</param>
      /// <param name="y">second operand</param>
      /// <returns>negative if x not equal to y, 0 if x equals y.
      /// </returns>
      public int Compare(ConnectedInstrument x, ConnectedInstrument y)
      {
          if (x == null)
              throw new ArgumentNullException("x");
          if (y == null)
              throw new ArgumentNullException("y");
          if (x.Block == y.Block && x.BlockNumber == y.BlockNumber)
              return string.Compare(x.BlockDisplayId, y.BlockDisplayId, true);
          else
              return -1;
      }
	  /// <summary>
	  /// Remove invalid XML characters.
	  /// </summary>
	  /// <param name="text"></param>
	  /// <returns></returns>
	  public static string RemoveInvalidXmlChars(string text)// TFS bug 1210
	  {
		  if (string.IsNullOrEmpty(text))
			  return text;
		  var validXmlChars = text.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
		  return new string(validXmlChars);
	  }
	  /// <summary>
	  /// Is valid XML string?
	  /// </summary>
	  /// <param name="text"></param>
	  /// <returns></returns>
	  public static bool IsValidXmlString(string text)// TFS bug 1210
	  {
		  try
		  {
			  if (!string.IsNullOrEmpty(text))
				  XmlConvert.VerifyXmlChars(text);
			  return true;
		  }
		  catch
		  {
			  return false;
		  }
	  }
      /// <summary>
      /// Returns an XML string representing the state of the object.
      /// </summary>
      /// <returns>An XML string representing the state of the object.</returns>
	  public string ToXml()// TFS bug 1210
      {
         XmlWriterSettings settings = new XmlWriterSettings();
         settings.Indent = true;
         settings.OmitXmlDeclaration = true;
         settings.ConformanceLevel = ConformanceLevel.Fragment;
         StringBuilder output = new StringBuilder();
         XmlWriter writer = XmlWriter.Create(output, settings);

         writer.WriteStartElement("ConnectedInstrument");
		 writer.WriteElementString("Version", RemoveInvalidXmlChars(xmlSerializationVersion.ToString(CultureInfo.InvariantCulture)));
         writer.WriteElementString("m_block", RemoveInvalidXmlChars(m_block.ToString()));
         writer.WriteElementString("m_cyclerBase", RemoveInvalidXmlChars(m_cyclerBase.ToString()));
         writer.WriteElementString("m_blockDisplayId", RemoveInvalidXmlChars(m_blockDisplayId.ToString(CultureInfo.InvariantCulture)));
         writer.WriteElementString("m_bayDisplayId", RemoveInvalidXmlChars(m_bayDisplayId.ToString(CultureInfo.InvariantCulture)));
         writer.WriteElementString("m_bayNumber", RemoveInvalidXmlChars(m_bayNumber.ToString(CultureInfo.InvariantCulture)));
         writer.WriteElementString("m_blockNumber", RemoveInvalidXmlChars(m_blockNumber.ToString(CultureInfo.InvariantCulture)));
         writer.WriteElementString("m_blockCount", RemoveInvalidXmlChars(m_blockCount.ToString(CultureInfo.InvariantCulture)));
         writer.WriteElementString("m_DeviceInstanceId", RemoveInvalidXmlChars(m_DeviceInstanceId.ToString(CultureInfo.InvariantCulture)));
         writer.WriteElementString("m_DevicePath", RemoveInvalidXmlChars(m_DevicePath.ToString(CultureInfo.InvariantCulture)));
         writer.WriteStartElement("m_blockConnectionParameters");
         int i = 0;
         foreach (KeyValuePair<string, string> kvp in m_blockConnectionParameters)
         {
            writer.WriteStartElement("KeyValuePair." + RemoveInvalidXmlChars(i.ToString(CultureInfo.InvariantCulture)));
            writer.WriteElementString("key", RemoveInvalidXmlChars(kvp.Key));
            writer.WriteElementString("value", RemoveInvalidXmlChars(kvp.Value));
            writer.WriteEndElement(); // "KeyValuePair"
            ++i;
         }
         writer.WriteEndElement(); // "m_blockConnectionParameters"
         writer.WriteStartElement("m_cyclerConnectionParameters");
         i = 0;
         foreach (KeyValuePair<string, string> kvp in m_cyclerConnectionParameters)
         {
            writer.WriteStartElement("KeyValuePair." + RemoveInvalidXmlChars(i.ToString(CultureInfo.InvariantCulture)));
            writer.WriteElementString("key", RemoveInvalidXmlChars(kvp.Key));
            writer.WriteElementString("value", RemoveInvalidXmlChars(kvp.Value));
            writer.WriteEndElement(); // "KeyValuePair"
            ++i;
         }
         writer.WriteEndElement(); // "m_cyclerConnectionParameters"
         writer.WriteEndElement(); // "ConnectedInstrument"

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
         this.PreInitialize();

         try
         {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            XmlReader reader = XmlReader.Create(new System.IO.StringReader(xml),
               settings);
            reader.Read();
            reader.ReadStartElement("ConnectedInstrument");
            string versionString = reader.ReadElementString("Version");
            int version = System.Convert.ToInt32(versionString,
               CultureInfo.InvariantCulture);
            if (version == 1)
            {
               string s = reader.ReadElementString("m_block");
               Block = (Blocks)(System.Enum.Parse(typeof(Blocks), s));
               s = reader.ReadElementString("m_cyclerBase");
               CyclerBase = (CyclerBases)(System.Enum.Parse(typeof(CyclerBases), s));
               BlockDisplayId = reader.ReadElementString("m_blockDisplayId");
               BayDisplayId = reader.ReadElementString("m_bayDisplayId");
               BayNumber = System.Convert.ToInt32(
                  reader.ReadElementString("m_bayNumber"), CultureInfo.InvariantCulture);
               BlockNumber = System.Convert.ToInt32(
                  reader.ReadElementString("m_blockNumber"), CultureInfo.InvariantCulture);
               BlockCount = System.Convert.ToInt32(
                 reader.ReadElementString("m_blockCount"), CultureInfo.InvariantCulture);
               DeviceInstanceId = reader.ReadElementString("m_DeviceInstanceId");
               DevicePath = reader.ReadElementString("m_DevicePath");

               if (reader.IsEmptyElement)
               {
                  reader.ReadStartElement("m_blockConnectionParameters");
               }
               else
               {
                  reader.ReadStartElement("m_blockConnectionParameters");
                  int i = 0;
                  while (reader.Name == "KeyValuePair." + i.ToString(
                     CultureInfo.InvariantCulture))
                  {
                     reader.ReadStartElement("KeyValuePair." + i.ToString(
                        CultureInfo.InvariantCulture));
                     string key = reader.ReadElementString("key");
                     string value = reader.ReadElementString("value");
                     BlockConnectionParameters.Add(key, value);
                     reader.ReadEndElement(); // "KeyValuePair.i"
                     ++i;
                  }
                  reader.ReadEndElement(); // "m_blockConnectionParameters"
               }
               if (reader.IsEmptyElement)
                  reader.ReadStartElement("m_cyclerConnectionParameters");
               else
               {
                  reader.ReadStartElement("m_cyclerConnectionParameters");
                  int i = 0;
                  while (reader.Name == "KeyValuePair." + i.ToString(
                     CultureInfo.InvariantCulture))
                  {
                     reader.ReadStartElement("KeyValuePair." + i.ToString(
                        CultureInfo.InvariantCulture));
                     string key = reader.ReadElementString("key");
                     string value = reader.ReadElementString("value");
                     CyclerConnectionParameters.Add(key, value);
                     reader.ReadEndElement(); // "KeyValuePair.i"
                     ++i;
                  }
                  reader.ReadEndElement(); // "m_cyclerConnectionParameters"
               }
            }
            else
            {
               Debug.Assert(false);
               // todo: error handling.
               return false;
            }
            reader.ReadEndElement(); // "ConnectedInstrument"
         }
         catch (XmlException)
         {
            Debug.Assert(false);
            // todo: error handling.
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
      #endregion
   }

   /// <summary>
   /// A class to contain and access a collection of connected instruments.
   /// </summary>
   public class ConnectedInstrumentsCollection : System.Collections.Generic.IEnumerable<ConnectedInstrument>
   {
      #region Member Data
      /// <summary>The ConnectedInstrument collection</summary>
      private List<ConnectedInstrument> m_instruments;
      private object objlock = new Object();
      /// <summary>Serialization version number</summary>
      private const int xmlSerializationVersion = 1;
      #endregion

      #region Accessors
		/// <summary>
		/// Readonly property that returns the number of connected instruments
		/// in the collection.
		/// </summary>
		public int Count
		{
			get 
            {
                int count = 0;
                lock (objlock)
                {
                    count = m_instruments.Count;
                }
                return count;
            }
		}
      #endregion

      #region Constructors and Destructor
      /// <summary>
      /// creates an empty collection of instruments.
      /// </summary>
      public ConnectedInstrumentsCollection()
      {
          m_instruments = new List<ConnectedInstrument>();
          m_instruments.Clear();
      }
      #endregion

      #region Methods
		/// <summary>
		/// Add a connected instrument to the collection.
		/// </summary>
		/// <param name="a">The instrument to add.</param>
		public void AddInstrument(ConnectedInstrument a)
		{
            lock (objlock)
            {
                m_instruments.Add(a);
            }
		}
        /// <summary>
        /// Find instrument.
        /// </summary>
        /// <param name="path">Device path.</param>
        /// <returns>The first element that matches the conditions defined by the specified predicate,
        /// if found; otherwise, the default value for type.</returns>
        public ConnectedInstrument Find(string path)
        {
            ConnectedInstrument desiredObject = null;
            lock (objlock)
            {
                foreach (ConnectedInstrument ci in m_instruments)
                {
                    int i = CultureInfo.CurrentUICulture.CompareInfo.IndexOf
                               (ci.DevicePath, path, CompareOptions.IgnoreCase);
                    if (i >= 0)
                    {
                        desiredObject = ci;
                        break;

                    }
                }
            }
            return desiredObject;
        }
		/// <summary>
		/// Fill in the connection parameters of each connected instrument with emulated 
		/// connection data.
		/// </summary>
		public void EmulateConnections()
		{
            lock (objlock)
            {
                foreach (ConnectedInstrument i in m_instruments)
                    i.EmulateConnection();
            }
		}
      /// <summary>
      /// Clear the collection.
      /// </summary>
      public void Clear()
      {
          lock (objlock)
          {
              m_instruments.Clear();
          }
      }
      /// <summary>
      /// Searches for the specified object and returns the zero-based index of the
      /// first occurrence within the entire System.Collections.Generic.List.
      /// </summary>
      /// <param name="item">The object to locate in the System.Collections.Generic.List. The value
      /// can be null for reference types.</param>
      /// <returns>The zero-based index of the first occurrence of item within the entire System.Collections.Generic.List,
      ///    if found; otherwise, �1.</returns>
      public int IndexOf(ConnectedInstrument item)
      {
          int index = -1;
          lock (objlock)
          {
              index = m_instruments.IndexOf(item);
          }
          return index;
      }
      /// <summary>
      ///  Removes the first occurrence of a specific object from the System.Collections.Generic.List.
      /// </summary>
      /// <param name="item">The object to remove from the System.Collections.Generic.List.</param>
      /// <returns>true if item is successfully removed; otherwise, false.</returns>
      public bool Remove(ConnectedInstrument item)
      {
          bool results = false;
          lock (objlock)
          {
              results = m_instruments.Remove(item);
          }
          return results;
      }
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public bool HasUnmatchedInstruments()
      {
          lock (objlock)
          {
              foreach (ConnectedInstrument inst in m_instruments)
              {
                  if (inst.CyclerBase == CyclerBases.None)
                      return true;
                  if (inst.Block == Blocks.None)
                      return true;
              }
          }
         return false;
      }

      /// <summary>
      /// Refreshes the collection of connected instruments with an up-to-date
      /// list of connected MJ instruments.</summary>
      /// <param name="input">The string created by the MJ Enumerator which 
      /// contains information about all known connected MJ instruments and 
      /// related items such as the logger process.</param>
      public void MergeMJString(string input)
      {
          lock (objlock)
          {
              // Get a dictionary from the serialized COptions structure.
              Dictionary<string, string> dictionary =
                 MJUtilities.CreateDictionaryFromCOptions(input);

              // Remove any MJ blocks currently in our instrument collection, to prepare
              //  the collection to receive a fresh set of connected MJ blocks.
              // Note that foreach should not be used for a loop that modifies the 
              //  collection, so I'm using a plain for.
              for (int i = 0; i < m_instruments.Count; ++i)
                  if (EnumerationUtils.IsBlockMJEnumerable(m_instruments[i].Block))
                  {
                      m_instruments.RemoveAt(i);
                      --i;
                  }

              // Now loop through the entries to find any blocks, and add them to our
              //  connected instrument collection.
              foreach (KeyValuePair<string, string> entry in dictionary)
              {
                  ConnectedInstrument instrument = new ConnectedInstrument();
                  if (instrument.InitializeFromEnumeratorData(entry.Value))
                      m_instruments.Add(instrument);
              }

              // Sort the list using our custom IComparer interface function.
              m_instruments.Sort(new ConnectedInstrument());
          }
      }
      /// <summary>
      /// Returns a string which represents the state of the object in a human readable
      /// format.
      /// </summary>
      /// <returns>A string representing the state of the object</returns>
      public override string ToString()
      {
          string ret = string.Empty;
          lock (objlock)
          {
              ret = "Instruments: " + m_instruments.Count + "\r\n\r\n";
              foreach (ConnectedInstrument instrument in m_instruments)
              {
                  ret += instrument.ToString();
                  ret += "\r\n\r\n";
              }
          }
         return ret;
      }
      /// <summary>
      /// Deserializes from an XML serialization.
      /// </summary>
      /// <param name="xml">the string which contains an appropriate XML fragment.</param>
      /// <returns>true if successful deserialization</returns>
      public bool FromXml(string xml)
      {
          lock (objlock)
          {
              m_instruments.Clear();
              try
              {
                  XmlReaderSettings settings = new XmlReaderSettings();
                  settings.ConformanceLevel = ConformanceLevel.Fragment;
                  settings.IgnoreWhitespace = true;
                  XmlReader reader = XmlReader.Create(new System.IO.StringReader(xml), settings);
                  reader.Read();
                  reader.ReadStartElement("ConnectedInstruments");
                  string versionString = reader.ReadElementString("Version");
                  int version = System.Convert.ToInt32(versionString,
                     CultureInfo.InvariantCulture);
                  if (version == 1)
                  {
                      if (reader.IsEmptyElement)
                      {
                          reader.ReadStartElement("instruments");
                      }
                      else
                      {
                          reader.ReadStartElement("instruments");
                          int i = 0;
                          while (reader.Name == "i." + i.ToString(
                             CultureInfo.InvariantCulture))
                          {
                              string instrumentXml = reader.ReadInnerXml();
                              ConnectedInstrument newInstrument = new ConnectedInstrument();
                              if (newInstrument.FromXml(instrumentXml))
                                  m_instruments.Add(newInstrument);
                              else
                                  throw new XmlException("Failed to unserialize internal object");

                              ++i;
                          }
                          reader.ReadEndElement(); // "instruments"
                      }
                  }
                  else
                  {
                      Debug.Assert(false);
                      // todo: error handling.
                      return false;
                  }
                  reader.ReadEndElement(); // "ConnectedInstruments"
              }
              catch (XmlException)
              {
                  Debug.Assert(false);
                  // todo: error handling.
                  return false;
              }
          }

         return true;
      }
      #endregion

      #region Interfaces
      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      {
         return m_instruments.GetEnumerator();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public System.Collections.Generic.IEnumerator<ConnectedInstrument> GetEnumerator()
      {
         return m_instruments.GetEnumerator();
      }
      #endregion
   }
}
