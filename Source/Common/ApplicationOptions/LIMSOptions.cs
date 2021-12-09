using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

using BioRad.Common.Xml;
using BioRad.Common.Utilities;
using System.Xml.Linq;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// 
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:</item>
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
	///			<item name="vssfile">$Workfile: LIMSOptions.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/ApplicationOptions/LIMSOptions.cs $</item>
	///			<item name="vssrevision">$Revision: 9 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Blie $</item>
	///			<item name="vssdate">$Date: 9/23/10 10:45a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public class LIMSOptions : BioRadXmlSerializableBase
	{
		#region Constants
		const string c_Completed = "Completed";
		#endregion

		#region Member Data
		/// <summary>
		/// AutoMoveCompleteFiles
		/// </summary>
		private bool m_AutoExportCompleteFiles = false;
		/// <summary>Default plate run file directory Path.</summary>
		private string m_PlateRunFilePath = ApplicationPath.LIMSCommonApplicationDataPath();
		/// <summary>Default plate run file linked protocol directory path.</summary>
		private string m_ProtocolFilePath = ApplicationPath.LIMSProtocolCommonApplicationDataPath();
		/// <summary>Move completed plate run file to default sub folder</summary> 
		private string m_CompletedFilesPath = ApplicationPath.LIMSCommonApplicationDataPath();
		/// <summary>Custom data Export folder.</summary>
		private string m_CustomDataExportFilesPath = ApplicationPath.LIMSCommonApplicationDataPath();
		#endregion

		#region Accessors
		/// <summary>
		/// Default plate run file directory Path.
		/// </summary>
		public string PlateRunFilePath
		{
			get { return m_PlateRunFilePath; }
			set { m_PlateRunFilePath = value; }
		}
		/// <summary>
		/// Default plate run file linked protocol directory path.
		/// </summary>
		public string ProtocolFilePath
		{
			get { return m_ProtocolFilePath; }
			set { m_ProtocolFilePath = value; }
		}
		/// <summary>Move completed plate run file to default sub folder, 
		/// if m_AutoMoveCompleteFiles set to TRUE.</summary>
		public string CompletedFilesPath
		{
			get { return m_CompletedFilesPath; }
			set { m_CompletedFilesPath = value; }
		}
		/// <summary>
		/// Default plate run file directory Path.
		/// </summary>
		public bool AutoExportCustomData
		{
			get { return m_AutoExportCompleteFiles; }
			set { m_AutoExportCompleteFiles = value; }
		}
		/// <summary>
		/// Custom data Export folder.
		/// </summary>
		public string CustomeExportDataFilePath
		{
			get { return m_CustomDataExportFilesPath; }
			set { m_CustomDataExportFilesPath = value; }
		}
		#endregion

		//#region Delegates and Events
		//#endregion

		#region Constructors and Destructor

		/// <summary>Public parameterless construction (Default).</summary>
		public LIMSOptions() { }

		/// <summary>
		/// FromXml deserializer.
		/// </summary>
		/// <param name="fromXml"></param>
		public LIMSOptions(string fromXml)
		{
			FromXml(fromXml);
		}

		#endregion

		#region Methods
		/// <summary>Value based (rather than reference based) equality test.  True if value equals, false if not.</summary>
		/// <param name="that">object to compare to.</param>
		/// <returns>true if value equals, false if not</returns>
		public bool ValueEquals(LIMSOptions that)
		{
			return this.ToXml().Equals(that.ToXml());
		}

		#region Xml Serialization
		// Serialization version.
		private const int c_SerializationVersion = 1;

		// Serialization Ids
		private const string c_SerializationId = "LIMSOptions";
		private const string c_SerializationVersionId = "SerVersion";
		private const string c_PlateRunFilePathId = "PlateRunFilePath";
		private const string c_ProtocolFilePathId = "ProtocolFilePath";
		private const string c_AutoMoveCompleteFilesId = "AutoMoveCompleteFiles";
		private const string c_CustomDataExportFilePathId = "CustomDataExportFilePath";
		private const string c_DataCompleteFilePathId = "DataRunCompleteFilePath";

		/// <summary>Deserialization constructor.</summary>
		public LIMSOptions(XElement element)
		{
			FromXElement(element);
		}
		/// <summary>Gets the serialization id of the most derived class</summary>
		/// <returns>The serialization id of the most derived class</returns>
		protected override string GetSerializationId()
		{
			return c_SerializationId;
		}
		/// <summary>Fill a table in with sufficient information to represent the state of the object.</summary>
		/// <returns>a state table which will be populated by this method.  This state
		/// table will be of the most recent version.</returns>
		protected override SerializationState ToSerializationState()
		{
			SerializationState state = new SerializationState();
			state.AddInt(c_SerializationVersionId, c_SerializationVersion);

			state.AddString(c_PlateRunFilePathId, m_PlateRunFilePath);
			state.AddString(c_ProtocolFilePathId, m_ProtocolFilePath);
			state.AddBool(c_AutoMoveCompleteFilesId, m_AutoExportCompleteFiles);
			state.AddString(c_CustomDataExportFilePathId, m_CustomDataExportFilesPath);
			state.AddString(c_DataCompleteFilePathId, m_CompletedFilesPath);

			return state;
		}
		/// <summary>populate this instance from a SerializationState.</summary>
		/// <param name="state">state of the object</param>
		protected override void FromSerializationState(SerializationState state)
		{
			UpgradeSerializationState(state);

			// Validate that the version is the most recent, as guaranteed by UpgradeSerializationState().
			int version = state.GetInt(c_SerializationVersionId);
			if (version != c_SerializationVersion)
				throw new XmlSerializationException("State dictionary is not of the current version.");

			// Populate this object's state from the state dictionary
			m_PlateRunFilePath = state.GetString(c_PlateRunFilePathId);
			m_ProtocolFilePath = state.GetString(c_ProtocolFilePathId);
			if (state.ContainsKey(c_CustomDataExportFilePathId))
				m_CustomDataExportFilesPath = state.GetString(c_CustomDataExportFilePathId);
			if (state.ContainsKey(c_DataCompleteFilePathId))
				m_CompletedFilesPath = state.GetString(c_DataCompleteFilePathId);

			m_AutoExportCompleteFiles = state.GetBool(c_AutoMoveCompleteFilesId);
		}
		/// <summary>Upgrades a SerializationState of any version, to the current version.</summary>
		/// <param name="state">persisted state to upgrade.</param>
		private void UpgradeSerializationState(SerializationState state)
		{
			int version = state.GetInt(c_SerializationVersionId);
			if (version < 1 || version > c_SerializationVersion)
			{
				// This is an unknown version.
				throw new XmlSerializationException(string.Format(CultureInfo.CurrentCulture,
						"Unknown serialized version", version));
			}

			//// Example:
			//if (version == 1)
			//{
			//   // Upgrade version 1 SerializationState to version 2

			//   // We are now consistent with version 2 state dictionaries.
			//   int newVersion = 2;
			//   state.AddInt(c_SerializationVersionId, newVersion);
			//   version = newVersion;
			//}

			Debug.Assert(version == c_SerializationVersion);
		}
		#endregion Xml Serialization
		#endregion

		//#region Event Handlers
		//#endregion
	}

	/// <summary></summary>
	public class AutomaticGridExportOptions : BioRadXmlSerializableBase
	{
		#region Constants
		/// <summary>Formats for auto export. These are persisted as integers, so the order cannot be changed.</summary>
		public enum AutoExportFormat
		{
			/// <summary></summary>
			Csv,
			/// <summary></summary>
			Text,
			/// <summary></summary>
			Excel2007,
			/// <summary></summary>
			Excel2003,
			/// <summary></summary>
			Xml
		}
		#endregion

		#region Members
		private bool m_DoAutomaticExport = false;
		private AutoExportFormat m_AutoExportFormat = AutoExportFormat.Excel2007;
		private bool m_UseDatafileFolder = true;
		private string m_CustomExportFolder = string.Empty;
		#endregion

		#region Accessors
		/// <summary></summary>
		public bool DoAutomaticExport
		{
			get { return m_DoAutomaticExport; }
			set { m_DoAutomaticExport = value; }
		}
		/// <summary></summary>
		public AutoExportFormat ExportFormat
		{
			get { return m_AutoExportFormat; }
			set { m_AutoExportFormat = value; }
		}
		/// <summary></summary>
		public bool UseDatafileFolder
		{
			get { return m_UseDatafileFolder; }
			set { m_UseDatafileFolder = value; }
		}
		/// <summary></summary>
		public string CustomExportFolder
		{
			get { return m_CustomExportFolder; }
			set { m_CustomExportFolder = value; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public AutomaticGridExportOptions()
		{
		}
		#endregion

		#region Xml Serialization
		// Serialization version.
		private const int c_SerializationVersion = 1;

		// Serialization Ids
		private const string c_SerializationId = "AutomaticExportOptions";
		private const string c_SerializationVersionId = "SerVersion";

		/// <summary>Deserialization constructor.</summary>
		public AutomaticGridExportOptions(XElement element)
		{
			FromXElement(element);
		}
		/// <summary>Gets the serialization id of the most derived class</summary>
		/// <returns>The serialization id of the most derived class</returns>
		protected override string GetSerializationId()
		{
			return c_SerializationId;
		}
		/// <summary>Fill a table in with sufficient information to represent the state of the object.</summary>
		/// <returns>a state table which will be populated by this method.  This state
		/// table will be of the most recent version.</returns>
		protected override SerializationState ToSerializationState()
		{
			SerializationState state = new SerializationState();
			state.AddInt(c_SerializationVersionId, c_SerializationVersion);

			state.AddBool("DoExport", m_DoAutomaticExport);
			state.AddInt("Format", (int)m_AutoExportFormat);
			state.AddBool("UseDatafileFolder", m_UseDatafileFolder);
			state.AddString("CustomFolder", m_CustomExportFolder);

			return state;
		}
		/// <summary>populate this instance from a SerializationState.</summary>
		/// <param name="state">state of the object</param>
		protected override void FromSerializationState(SerializationState state)
		{
			UpgradeSerializationState(state);

			// Validate that the version is the most recent, as guaranteed by UpgradeSerializationState().
			int version = state.GetInt(c_SerializationVersionId);
			if (version != c_SerializationVersion)
				throw new XmlSerializationException("State dictionary is not of the current version.");

			m_DoAutomaticExport = state.GetBool("DoExport");
			m_AutoExportFormat = (AutoExportFormat)(state.GetInt("Format"));
			m_UseDatafileFolder = state.GetBool("UseDatafileFolder");
			m_CustomExportFolder = state.GetString("CustomFolder");
		}
		/// <summary>Upgrades a SerializationState of any version, to the current version.</summary>
		/// <param name="state">persisted state to upgrade.</param>
		private void UpgradeSerializationState(SerializationState state)
		{
			int version = state.GetInt(c_SerializationVersionId);
			if (version < 1 || version > c_SerializationVersion)
			{
				// This is an unknown version.
				throw new XmlSerializationException(string.Format(CultureInfo.CurrentCulture,
						"Unknown serialized version", version));
			}

			//// Example:
			//if (version == 1)
			//{
			//   // Upgrade version 1 SerializationState to version 2

			//   // We are now consistent with version 2 state dictionaries.
			//   int newVersion = 2;
			//   state.AddInt(c_SerializationVersionId, newVersion);
			//   version = newVersion;
			//}

			Debug.Assert(version == c_SerializationVersion);
		}
		#endregion Xml Serialization
	}
	/// <summary></summary>
	public class RDMLExportOptions : BioRadXmlSerializableBase
	{
		#region Constants
		/// <summary>Formats for auto export. These are persisted as integers, so the order cannot be changed.</summary>
		public enum RDMLExportVersion
		{
			/// <summary></summary>
			RDML11 = 0,
			/// <summary></summary>
			RDML10,
		}
		#endregion

		#region Members
		private bool m_DoAutomaticExport = false;
		private RDMLExportVersion m_AutoExportVersion = RDMLExportVersion.RDML11;
		#endregion

		#region Accessors
		/// <summary></summary>
		public bool DoAutomaticExport
		{
			get { return m_DoAutomaticExport; }
			set { m_DoAutomaticExport = value; }
		}
		/// <summary></summary>
		public RDMLExportVersion ExportVersion
		{
			get { return m_AutoExportVersion; }
			set { m_AutoExportVersion = value; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public RDMLExportOptions()
		{
		}
		#endregion

		#region Xml Serialization
		// Serialization version.
		private const int c_SerializationVersion = 1;

		// Serialization Ids
		private const string c_SerializationId = "RDMLExportOptions";
		private const string c_SerializationVersionId = "SerVersion";

		/// <summary>Deserialization constructor.</summary>
		public RDMLExportOptions(XElement element)
		{
			FromXElement(element);
		}
		/// <summary>Gets the serialization id of the most derived class</summary>
		/// <returns>The serialization id of the most derived class</returns>
		protected override string GetSerializationId()
		{
			return c_SerializationId;
		}
		/// <summary>Fill a table in with sufficient information to represent the state of the object.</summary>
		/// <returns>a state table which will be populated by this method.  This state
		/// table will be of the most recent version.</returns>
		protected override SerializationState ToSerializationState()
		{
			SerializationState state = new SerializationState();
			state.AddInt(c_SerializationVersionId, c_SerializationVersion);

			state.AddBool("DoExport", m_DoAutomaticExport);
			state.AddInt("Version", (int)m_AutoExportVersion);
			return state;
		}
		/// <summary>populate this instance from a SerializationState.</summary>
		/// <param name="state">state of the object</param>
		protected override void FromSerializationState(SerializationState state)
		{
			UpgradeSerializationState(state);

			// Validate that the version is the most recent, as guaranteed by UpgradeSerializationState().
			int version = state.GetInt(c_SerializationVersionId);
			if (version != c_SerializationVersion)
				throw new XmlSerializationException("State dictionary is not of the current version.");

			m_DoAutomaticExport = state.GetBool("DoExport");
			if (state.GetInt("Version") > 1)
				m_AutoExportVersion = RDMLExportVersion.RDML11;
			else
				m_AutoExportVersion = (RDMLExportVersion)(state.GetInt("Version"));
		}
		/// <summary>Upgrades a SerializationState of any version, to the current version.</summary>
		/// <param name="state">persisted state to upgrade.</param>
		private void UpgradeSerializationState(SerializationState state)
		{
			int version = state.GetInt(c_SerializationVersionId);
			if (version < 1 || version > c_SerializationVersion)
			{
				// This is an unknown version.
				throw new XmlSerializationException(string.Format(CultureInfo.CurrentCulture,
						  "Unknown serialized version", version));
			}
			Debug.Assert(version == c_SerializationVersion);
		}
		#endregion Xml Serialization
	}
	/// <summary>
	/// Static wrapper class for loading and saving the collection of standard curve infos for FSD.
	/// </summary>
	public sealed class PersistedStandardCurveInfosForFsd
	{
		/// <summary>
		/// Retrieve the persisted StandardCurveInfoForFsdCollection
		/// </summary>
		/// <returns></returns>
		public static StandardCurveInfoForFsdCollection GetPersistedStandardCurveInfosForFsd()
		{
			try
			{
				ApplicationOptions ao = PersistedApplicationOptions.GetInstance;
				if (ao.StandardCurveInfoCollectionForFsd == null)
					return new StandardCurveInfoForFsdCollection();
				else
					return ao.StandardCurveInfoCollectionForFsd as StandardCurveInfoForFsdCollection;
			}
			catch
			{
				Debug.Assert(false, "Exception thrown trying to StandardCurveInfoForFsdCollection.");
				return new StandardCurveInfoForFsdCollection();
			}
		}

		/// <summary>
		/// Store the persisted StandardCurveInfoForFsdCollection.
		/// </summary>
		/// <param name="infos"></param>
		public static void PersistStandardCurveInfoForFsdCollection(StandardCurveInfoForFsdCollection infos)
		{
			ApplicationOptions ao = PersistedApplicationOptions.GetInstance;
			ao.StandardCurveInfoCollectionForFsd = infos;
			PersistedApplicationOptions.PersistApplicationOptions(ao);
		}
	}
	/// <summary>
	/// Collection of persisted standard curve infos for FSD
	/// </summary>
	public class StandardCurveInfoForFsdCollection : BioRadXmlSerializableBase
	{
		#region Members
		private List<StandardCurveInfoForFsd> m_StdCurveInfos = new List<StandardCurveInfoForFsd>();
		#endregion

		#region Accessors
		/// <summary>
		/// All standard curve infos
		/// </summary>
		public List<StandardCurveInfoForFsd> StdCurveInfos
		{
			get { return m_StdCurveInfos; }
			set { m_StdCurveInfos = value; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor.
		/// </summary>
		public StandardCurveInfoForFsdCollection()
		{
		}
		#endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="stdCurveInfo"></param>
		public void AddStandardCurveInfo(StandardCurveInfoForFsd stdCurveInfo)
		{
			if (stdCurveInfo == null)
			{
				Debug.Assert(false);
				return;
			}
			StandardCurveInfoForFsd currentInfo = TryGetStandardCurveInfo(stdCurveInfo.TestName, stdCurveInfo.PcrKitLot, stdCurveInfo.StdKitLot);
			if (currentInfo != null)
				m_StdCurveInfos.Remove(currentInfo);
			m_StdCurveInfos.Add(stdCurveInfo);
		}
		/// <summary>
		/// Returns the re-usable std curve info, if it exists, for the given lot numbers.  Otherwise null.
		/// </summary>
		/// <param name="testName"></param>
		/// <param name="pcrKitNumber"></param>
		/// <param name="stdKitNumber"></param>
		/// <returns>the re-usable std curve info, if it exists, for the given lot numbers.  Otherwise null.</returns>
		public StandardCurveInfoForFsd TryGetStandardCurveInfo(string testName, string pcrKitNumber, string stdKitNumber)
		{
			foreach (StandardCurveInfoForFsd info in m_StdCurveInfos)
				if (info.TestName.ToLowerInvariant() == testName.ToLowerInvariant() &&
					info.PcrKitLot.ToLowerInvariant() == pcrKitNumber.ToLowerInvariant() &&
					info.StdKitLot.ToLowerInvariant() == stdKitNumber.ToLowerInvariant())
				{
					return info;
				}
			return null;
		}
		#endregion

		#region Xml Serialization
		// Serialization version.
		private const int c_SerializationVersion = 1;

		// Serialization Ids
		private const string c_SerializationId = "StandardCurvePersistedInfoForFsdCollection";
		private const string c_SerializationVersionId = "SerVersion";

		/// <summary>Deserialization constructor.</summary>
		public StandardCurveInfoForFsdCollection(XElement element)
		{
			FromXElement(element);
		}
		/// <summary>Gets the serialization id of the most derived class</summary>
		/// <returns>The serialization id of the most derived class</returns>
		protected override string GetSerializationId()
		{
			return c_SerializationId;
		}
		/// <summary>Fill a table in with sufficient information to represent the state of the object.</summary>
		/// <returns>a state table which will be populated by this method.  This state
		/// table will be of the most recent version.</returns>
		protected override SerializationState ToSerializationState()
		{
			SerializationState state = new SerializationState();
			state.AddInt(c_SerializationVersionId, c_SerializationVersion);

			if (m_StdCurveInfos != null && m_StdCurveInfos.Count > 0)
			{
				state.AddIBioRadXmlSerializables("infos", m_StdCurveInfos.ConvertAll<IBioRadXmlSerializable>(
					delegate(StandardCurveInfoForFsd a) 
					{ 
						return a as IBioRadXmlSerializable; 
					}));
			}

			return state;
		}
		/// <summary>populate this instance from a SerializationState.</summary>
		/// <param name="state">state of the object</param>
		protected override void FromSerializationState(SerializationState state)
		{
			UpgradeSerializationState(state);

			// Validate that the version is the most recent, as guaranteed by UpgradeSerializationState().
			int version = state.GetInt(c_SerializationVersionId);
			if (version != c_SerializationVersion)
				throw new XmlSerializationException("State dictionary is not of the current version.");

			m_StdCurveInfos = new List<StandardCurveInfoForFsd>();
			if (state.ContainsKey("infos"))
			{
				List<IBioRadXmlSerializable> infos = state.GetIBioRadXmlSerializables("infos", typeof(StandardCurveInfoForFsd));
				m_StdCurveInfos = infos.ConvertAll<StandardCurveInfoForFsd>(
					delegate(IBioRadXmlSerializable a)
					{
						return a as StandardCurveInfoForFsd;
					});
			}
		}
		/// <summary>Upgrades a SerializationState of any version, to the current version.</summary>
		/// <param name="state">persisted state to upgrade.</param>
		private void UpgradeSerializationState(SerializationState state)
		{
			int version = state.GetInt(c_SerializationVersionId);
			if (version < 1 || version > c_SerializationVersion)
			{
				// This is an unknown version.
				throw new XmlSerializationException(string.Format(CultureInfo.CurrentCulture,
						"Unknown serialized version", version));
			}

			//// Example:
			//if (version == 1)
			//{
			//   // Upgrade version 1 SerializationState to version 2

			//   // We are now consistent with version 2 state dictionaries.
			//   int newVersion = 2;
			//   state.AddInt(c_SerializationVersionId, newVersion);
			//   version = newVersion;
			//}

			Debug.Assert(version == c_SerializationVersion);
		}
		#endregion Xml Serialization
	}
	/// <summary>
	/// Standard curve info and other related analyzed results for FSD.
	/// </summary>
	public class StandardCurveInfoForFsd : BioRadXmlSerializableBase
	{
		#region Members
		private string m_TestName = "";
		private string m_PcrKitLot = "";
		private string m_StdKitLot = "";
		private double m_Slope = 0;
		private double m_HighestQuantity = 0;
		private double m_LowestQuantity = 0;
		private double m_LowestStandardBoundingCt = 0;
		private double m_HighestStandardBoundingCt = 0;
		private double m_InternalControlStdDev = 0;
		private double m_HighestStandardBoundingInternalControlCt = 0;
		private double m_MeanICCtOfStandards = 0;
		private double m_YIntercept = 0;
		#endregion

		#region Accessors
		/// <summary>Name of test, such as Legio Pneumo, etc.</summary>
		public string TestName
		{
			get { return m_TestName; }
			set { m_TestName = value; }
		}
		/// <summary>Pcr Kit lot number this info pertains to.</summary>
		public string PcrKitLot
		{
			get { return m_PcrKitLot; }
			set { m_PcrKitLot = value; }
		}
		/// <summary>Std Kit lot number this info pertains to.</summary>
		public string StdKitLot
		{
			get { return m_StdKitLot; }
			set { m_StdKitLot = value; }
		}
		/// <summary></summary>
		public double Slope
		{
			get { return m_Slope; }
			set { m_Slope = value; }
		}
		/// <summary></summary>
		public double YIntercept
		{
			get { return m_YIntercept; }
			set { m_YIntercept = value; }
		}
		/// <summary></summary>
		public double HighestQuantity
		{
			get { return m_HighestQuantity; }
			set { m_HighestQuantity = value; }
		}
		/// <summary></summary>
		public double LowestQuantity
		{
			get { return m_LowestQuantity; }
			set { m_LowestQuantity = value; }
		}
		/// <summary></summary>
		public double LowestStandardBoundingCt
		{
			get { return m_LowestStandardBoundingCt; }
			set { m_LowestStandardBoundingCt = value; }
		}
		/// <summary></summary>
		public double HighestStandardBoundingCt
		{
			get { return m_HighestStandardBoundingCt; }
			set { m_HighestStandardBoundingCt = value; }
		}
		/// <summary></summary>
		public double InternalControlStdDev
		{
			get { return m_InternalControlStdDev; }
			set { m_InternalControlStdDev = value; }
		}
		/// <summary></summary>
		public double HighestStandardBoundingInternalControlCt
		{
			get { return m_HighestStandardBoundingInternalControlCt; }
			set { m_HighestStandardBoundingInternalControlCt = value; }
		}
		/// <summary></summary>
		public double MeanICCtOfStandards
		{
			get { return m_MeanICCtOfStandards; }
			set { m_MeanICCtOfStandards = value; }
		}
		#endregion

		#region Constructors
		/// <summary>Default constructor.</summary>
		public StandardCurveInfoForFsd()
		{
		}
		#endregion

		#region Xml Serialization
		// Serialization version.
		private const int c_SerializationVersion = 1;

		// Serialization Ids
		private const string c_SerializationId = "StandardCurveInfoForFsd";
		private const string c_SerializationVersionId = "SerVersion";

		/// <summary>Deserialization constructor.</summary>
		public StandardCurveInfoForFsd(XElement element)
		{
			FromXElement(element);
		}
		/// <summary>Gets the serialization id of the most derived class</summary>
		/// <returns>The serialization id of the most derived class</returns>
		protected override string GetSerializationId()
		{
			return c_SerializationId;
		}
		/// <summary>Fill a table in with sufficient information to represent the state of the object.</summary>
		/// <returns>a state table which will be populated by this method.  This state
		/// table will be of the most recent version.</returns>
		protected override SerializationState ToSerializationState()
		{
			SerializationState state = new SerializationState();
			state.AddInt(c_SerializationVersionId, c_SerializationVersion);

			state.AddString("testName", m_TestName);
			state.AddString("pcrKit", m_PcrKitLot);
			state.AddString("stdKit", m_StdKitLot);
			state.AddDouble("slope", m_Slope);
			state.AddDouble("YIntercept", m_YIntercept);
			state.AddDouble("highQuant", m_HighestQuantity);
			state.AddDouble("lowQuant", m_LowestQuantity);
			state.AddDouble("icStdDev", m_InternalControlStdDev);
			state.AddDouble("lowStandardBoundingCt", m_LowestStandardBoundingCt);
			state.AddDouble("highStandardBoundingCt", m_HighestStandardBoundingCt);
			state.AddDouble("meanICCtOfStds", m_MeanICCtOfStandards);
			state.AddDouble("highStdBoundingICCt", m_HighestStandardBoundingInternalControlCt);

			return state;
		}
		/// <summary>populate this instance from a SerializationState.</summary>
		/// <param name="state">state of the object</param>
		protected override void FromSerializationState(SerializationState state)
		{
			UpgradeSerializationState(state);

			// Validate that the version is the most recent, as guaranteed by UpgradeSerializationState().
			int version = state.GetInt(c_SerializationVersionId);
			if (version != c_SerializationVersion)
				throw new XmlSerializationException("State dictionary is not of the current version.");

			m_TestName = state.GetString("testName");
			m_PcrKitLot = state.GetString("pcrKit");
			m_StdKitLot = state.GetString("stdKit");
			m_Slope = state.GetDouble("slope");
			m_YIntercept = state.GetDouble("YIntercept");
			m_HighestQuantity = state.GetDouble("highQuant");
			m_LowestQuantity = state.GetDouble("lowQuant");
			m_InternalControlStdDev = state.GetDouble("icStdDev");
			m_LowestStandardBoundingCt = state.GetDouble("lowStandardBoundingCt");
			m_HighestStandardBoundingCt = state.GetDouble("highStandardBoundingCt");
			m_MeanICCtOfStandards = state.GetDouble("meanICCtOfStds");
			m_HighestStandardBoundingInternalControlCt = state.GetDouble("highStdBoundingICCt");
		}
		/// <summary>Upgrades a SerializationState of any version, to the current version.</summary>
		/// <param name="state">persisted state to upgrade.</param>
		private void UpgradeSerializationState(SerializationState state)
		{
			int version = state.GetInt(c_SerializationVersionId);
			if (version < 1 || version > c_SerializationVersion)
			{
				// This is an unknown version.
				throw new XmlSerializationException(string.Format(CultureInfo.CurrentCulture,
						"Unknown serialized version", version));
			}

			if (state.ContainsKey("lowStandardBoundingCt") == false)
			{
				state.AddDouble("lowStandardBoundingCt", 0);
				state.AddDouble("highStandardBoundingCt", 0);
				state.AddDouble("meanICCtOfStds", 0);
				state.AddDouble("highStdBoundingICCt", 0);
			}

			if (state.ContainsKey("YIntercept") == false)
			{
				state.AddDouble("YIntercept", 0);
			}

			if (state.ContainsKey("testName") == false)
			{
				state.AddString("testName", "");
			}

			//// Example:
			//if (version == 1)
			//{
			//   // Upgrade version 1 SerializationState to version 2

			//   // We are now consistent with version 2 state dictionaries.
			//   int newVersion = 2;
			//   state.AddInt(c_SerializationVersionId, newVersion);
			//   version = newVersion;
			//}

			Debug.Assert(version == c_SerializationVersion);
		}
		#endregion Xml Serialization
	}
}
