using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Reflection;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using System.Diagnostics;

using BioRad.Common.Utilities;

namespace BioRad.Common.DiagnosticsLogger
{
	/// <summary>
	/// XmlTextReader extension.
	/// </summary>
	public class LogXmlTextReader : XmlTextReader
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		public LogXmlTextReader(Stream input)
			:// Ralph 11.16.2005
			base(input, XmlNodeType.Element,
			new XmlParserContext(null, null, string.Empty, XmlSpace.None))
		{
			this.WhitespaceHandling = WhitespaceHandling.None;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		public LogXmlTextReader(StringReader input)
			:
			base(input)
		{
			this.WhitespaceHandling = WhitespaceHandling.None;
		}
		/// <summary>
		/// Do not want to close the file stream object.
		/// </summary>
		public override void Close()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		public override ReadState ReadState
		{
			get
			{
				return ReadState.Initial;
			}
		}
	}

	#region Documentation Tags
	/// <summary>
	/// Enumerator that can iterate through collection of log entries in a log file.
	/// </summary>
	/// <remarks>
	/// Represents a enumerator that provides fast, non-cached, forward-only access to log entries
	/// in log file.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Ralph Ansell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">626</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\DiagLogger.wmf">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: LogEnumerator.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/DiagnosticsLogger/LogEnumerator.cs $</item>
	///			<item name="vssrevision">$Revision: 12 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 7/01/08 2:27p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	[Serializable]
	public partial class LogEnumerator : IEnumerator, IEnumerable, IDisposable
	{
		#region Member Data
		private const int c_ReadLength = 2000;
		#endregion

		#region Member Data
		/// <summary>
		/// Private Object used purely for synchronization.
		/// </summary>
		private Object objLock = new Object();
		/// <summary>
		/// 
		/// </summary>
		private FileStream m_fs = null;
		/// <summary>
		/// List contains file position of each log entry.
		/// </summary>
		private ArrayList m_LogEntryPos;
		/// <summary>
		/// Log File path.
		/// </summary>
		private long m_CurrentIndex = -1;
		/// <summary>
		/// 
		/// </summary>
		private int m_Count = -1;
		/// <summary>
		/// 
		/// </summary>
		private int m_Version;
		/// <summary>
		/// 
		/// </summary>
		private long m_StartPositionEncryptedData;
		/// <summary>
		/// 
		/// </summary>
		private string m_Filename;
		/// <summary>
		/// 
		/// </summary>
		private BinaryReader m_BinaryReader;
		/// <summary>
		/// 
		/// </summary>
		private bool m_BioRadLogFile;
		/// <summary>
		/// 
		/// </summary>
		private LogXmlTextReader m_Reader = null;
		#endregion

		#region Accessors
		/// <summary>
		/// Gets the current log entry, DiagnosticsLogItem object, in the collection.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">
		/// Throws invalid operation excetion.
		/// </exception>
		/// <remarks>
		/// <para>
		/// First call to Current opens log file and reads first log entry. Subsequent calls to 
		/// Current reads next log entry in log file.
		/// </para>
		/// <para>
		/// After an enumerator is created or after a method Reset, MoveNext must be called to advance the 
		/// enumerator to the first element of the collection before reading the value of method Current; 
		/// otherwise, method Current is undefined.
		/// </para>
		/// <para>
		/// Method Current does not move the position of the enumerator and consecutive calls to method 
		/// Current return the same object until either method MoveNext or Reset is called.
		/// </para>
		/// </remarks>
		public object Current
		{
			get
			{
				if (m_CurrentIndex == -1)
					return null;
				return this[m_CurrentIndex];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Version
		{
			get { return m_Version; }
		}
		/// <summary>
		/// 
		/// </summary>
		public bool IsBioRadDiagnosticLogFile
		{
			get { return m_BioRadLogFile; }
		}
		/// <summary>
		/// Get number of log entries.
		/// </summary>
		public int Count
		{
			[MethodImplAttribute(MethodImplOptions.Synchronized)]
			get
			{
				if (m_Count != -1)
					return m_Count;

				m_Count = 0;
				m_LogEntryPos.Clear();

				if (m_Version == DiagnosticsLogService.c_VersionEncyrptedLogFiles)// encrypted data version
				{
					try
					{
						m_BinaryReader.BaseStream.Position =
							m_StartPositionEncryptedData;

						Int32 len;
						while (m_BinaryReader.BaseStream.Position < m_BinaryReader.BaseStream.Length)
						{
							// save file position for each log entry.
							m_LogEntryPos.Add(m_BinaryReader.BaseStream.Position);
							len = m_BinaryReader.ReadInt32();
							if (len > 0 && len < m_BinaryReader.BaseStream.Length)
							{
								m_BinaryReader.ReadBytes(len);
								m_Count++;
							}
						}
					}
					catch
					{
						//iqnore exceptions
					}
					finally
					{
						// Reset file pointer.
						m_BinaryReader.BaseStream.Position =
							m_StartPositionEncryptedData;
					}
				}
				else // previous older versions used XML text reader.
				{
					long curpos = m_fs.Position;
					m_fs.Position = 0;

					LogXmlTextReader xmlTextReader = new LogXmlTextReader(m_fs);
					try
					{
						while (!xmlTextReader.EOF)
						{
							xmlTextReader.Read();
							switch (xmlTextReader.NodeType)
							{
								case XmlNodeType.Element:
									if (xmlTextReader.Name.Equals(XmlElementName.Log.ToString()))
									{
										// save file position for each log entry.
										m_Count++;
									}
									break;
							}
						}
					}
					catch
					{
						// Defect 4078 - Ralph 03/03/2006
						//iqnore exceptions
					}
					finally
					{
						if (xmlTextReader != null)
							xmlTextReader.Close();

						m_fs.Position = curpos;
					}
				}

				return m_Count;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private DiagnosticsLogItem GetDiagnosticsLogItem(LogXmlTextReader reader)
		{
			if (reader == null)
				throw new ArgumentNullException();

			DiagnosticsLogItem di = null;
			StringCollection elementNames = new StringCollection();
			elementNames.AddRange(Enum.GetNames(typeof(XmlElementName)));
			string[] elementValue = new string[elementNames.Count];

			int i = -1;
			bool done = false;
			try
			{
				while (!reader.EOF && !done)
				{
					reader.Read();

					switch (reader.NodeType)
					{
						case XmlNodeType.Element:
							if (reader.Name.Equals(XmlElementName.BioRadDiagnosticLogFile.ToString()))
								break;
							i = elementNames.IndexOf(reader.Name.ToString());
							break;
						case XmlNodeType.Text:
							if (i >= 0)
							{
								Debug.Assert(reader.Value != null);
								elementValue[i] = reader.Value;
							}
							break;
						case XmlNodeType.EndElement:
							if (reader.Name.Equals(XmlElementName.BioRadDiagnosticLogFile.ToString()))
								break;
							if (reader.Name.Equals(XmlElementName.Log.ToString()))
								done = true;
							break;
					}
				}
			}
			catch
			{
				// Defect 4078 - Ralph 03/03/2006
				//iqnore exceptions
			}
			finally
			{
				di = new DiagnosticsLogItem(
					elementValue[(int)XmlElementName.LgNm],
					elementValue[(int)XmlElementName.TS],
					elementValue[(int)XmlElementName.ANm],
					elementValue[(int)XmlElementName.Sev],
					elementValue[(int)XmlElementName.Tag],
					elementValue[(int)XmlElementName.Msg],
					elementValue[(int)XmlElementName.MsgNm],
					elementValue[(int)XmlElementName.Stack],
					elementValue[(int)XmlElementName.Level],
					elementValue[(int)XmlElementName.Data]
					);
			}

			return di;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xmlText"></param>
		/// <returns></returns>
		private DiagnosticsLogItem GetDiagnosticsLogItem(string xmlText)
		{
			if (xmlText == null || xmlText.Length == 0)
				throw new ArgumentNullException();

			DiagnosticsLogItem di = null;

			LoggerXmlTags tag =
				new LoggerXmlTags(XmlElementName.Log);
			int i = xmlText.IndexOf(tag.StartTag);
			int j = xmlText.IndexOf(tag.EndTag);
			bool isOK = (i != -1 && j != -1) ? true : false;
			Debug.Assert(isOK);
			if (!isOK)
				return null;

			using (StringReader strReader = new StringReader(xmlText))
			{
				LogXmlTextReader reader = new LogXmlTextReader(strReader);
				di = GetDiagnosticsLogItem(reader);
			}

			return di;
		}
		/// <summary>
		/// Indexer for direct access to DiagnosticsLogItem object.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">
		/// Throws invalid operation excetion.
		/// </exception>
		/// <remarks>
		/// Returns the DiagnosticsLogItem object for the argument index.
		/// </remarks>
		private DiagnosticsLogItem this[long index]
		{
			[MethodImplAttribute(MethodImplOptions.Synchronized)]
			get
			{
				if (index < 0 || index >= Count)
					throw new ArgumentException("Index out of range error");

				DiagnosticsLogItem di = null;
				StringCollection elementNames = new StringCollection();
				elementNames.AddRange(Enum.GetNames(typeof(XmlElementName)));
				string[] elementValue = new string[elementNames.Count];

				if (m_Version == DiagnosticsLogService.c_VersionEncyrptedLogFiles)// encrypted data version
				{
					long curpos = m_BinaryReader.BaseStream.Position;//save current file position

					try
					{
						m_BinaryReader.BaseStream.Position =
							(long)m_LogEntryPos[(int)index];

						Int32 len = m_BinaryReader.ReadInt32();
						if (len > 0 && len < m_BinaryReader.BaseStream.Length)
						{
							byte[] encrypted = new byte[len];
							m_BinaryReader.Read(encrypted, 0, encrypted.Length);
							string stringFromUnicode = FileCryptor.GetInstance.Decrypt(encrypted, Encoding.Unicode);
							string s;
							if (stringFromUnicode.IndexOf(XmlElementName.Log.ToString()) >= 0)
							{
								s = stringFromUnicode;
							}
							else
							{
								string stringFromAscii = FileCryptor.GetInstance.Decrypt(encrypted, Encoding.ASCII);
								s = stringFromAscii;
							}
							di = GetDiagnosticsLogItem(s);
						}
					}
					finally
					{
						m_BinaryReader.BaseStream.Position = curpos;
					}
				}
				else // versions that use XML text reader.
				{
					di = GetDiagnosticsLogItem(m_Reader);
				}

				return di;
			}
		}
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Initializes a new instance of the LogEnumerator class.
		/// </summary>
		/// <param name="fileName">Full path of log file.</param>
		public LogEnumerator(string fileName)
		{
			m_LogEntryPos = new ArrayList(0);

			m_BioRadLogFile = false;
			if (!IsBioRadLogFile(fileName))
				throw new ApplicationException("Selected file is not a Bio-Rad diagnostic log file.");
			else
				m_BioRadLogFile = true;

			m_Filename = fileName;
			// Fix for Bug 4159
			m_fs = FileUtilities.OpenFile(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

			m_BinaryReader = GetNewBinaryReader(m_fs);

			m_Version = 0;
			m_StartPositionEncryptedData = 0;

			Init();
		}
		#endregion

		#region Methods
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
			lock (this)
			{
				if (disposing)
				{
					if (m_BinaryReader != null)
						m_BinaryReader.Close();
					m_BinaryReader = null;
				}
			}
		}
		/// <summary>
		/// Verifys that existing file is a BioRad log file.
		/// </summary>
		/// <param name="filename">Full file path to log file.</param>
		/// <returns>true if argument is a Bio-Rad diagnostic log file.</returns>
		public static bool IsBioRadLogFile(string filename)
		{
			if (filename == null)
				throw new ArgumentException();

			bool isBioRadLogFile = false;
			
			try
			{
				if (File.Exists(filename))// Prevent exception if file does not exists.
				{
					LoggerXmlTags tag =
						new LoggerXmlTags(XmlElementName.BioRadDiagnosticLogFile);
					// Fix for Bug 4159
					using (FileStream fs =
						FileUtilities.OpenFile(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
					//using ( FileStream fs =
					//    new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
					using (BinaryReader reader = GetNewBinaryReader(fs))
					{
						byte[] bytes = new byte[c_ReadLength];
						int count = reader.Read(bytes, 0, bytes.Length);
						string s = Encoding.UTF8.GetString(bytes);
						int i = s.IndexOf(tag.StartTag);
						isBioRadLogFile = (i != -1) ? true : false;
					}
				}
			}
			catch
			{
				// Defect 4078 - Ralph 03/03/2006
				//iqnore exceptions
			}
		
			return isBioRadLogFile;
		}
		/// <summary>
		/// 
		/// </summary>
		private void Init()
		{
			// default version.
			m_Version = DiagnosticsLogService.c_VersionXmlTextReaderLogFiles;

			try
			{
				// determine log file version.
				LoggerXmlTags tag =
					new LoggerXmlTags(XmlElementName.BioRadDiagnosticLogFile);

				Debug.Assert(m_BinaryReader != null);
				Debug.Assert(m_BinaryReader.BaseStream != null);

				m_BinaryReader.BaseStream.Position = 0;

				byte[] bytes = new byte[tag.StartTag.Length];
				int count = m_BinaryReader.Read(bytes, 0, bytes.Length);
				string s = Encoding.UTF8.GetString(bytes);
				int i = s.IndexOf(tag.StartTag);
				if (i == 0)
					m_Version = m_BinaryReader.ReadInt32();
			}
			catch
			{
				m_BioRadLogFile = false;
			}
			finally
			{
				m_BinaryReader.BaseStream.Position = 0;

				if (m_Version == DiagnosticsLogService.c_VersionEncyrptedLogFiles)
				{
					// get start position in file of encrpted data.
					m_StartPositionEncryptedData =
						PositionFilePointerToStartOfEncryptedData(m_BinaryReader);
				}

				Reset();
			}
		}
		/// <summary>
		/// Gets a binary reader using the default encoding on the supplied stream.
		/// </summary>
		/// <param name="fileStream">The filestream from which to get a reader.</param>
		/// <returns>A reader.</returns>
		internal static BinaryReader GetNewBinaryReader(FileStream fileStream)
		{
			Encoding encoding = new UTF8Encoding();
			BinaryReader reader = new BinaryReader(fileStream, encoding);
			return reader;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="binaryReader"></param>
		/// <returns></returns>
		private long PositionFilePointerToStartOfEncryptedData(BinaryReader binaryReader)
		{
			long pos = 0;
			if (m_Version == DiagnosticsLogService.c_VersionEncyrptedLogFiles)
			{
				LoggerXmlTags tag =
					new LoggerXmlTags(XmlElementName.BioRadDiagnosticLogFile);

				binaryReader.BaseStream.Position = 0;

				byte[] bytes = new byte[tag.StartTag.Length];
				int count = m_BinaryReader.Read(bytes, 0, bytes.Length);
				Debug.Assert(count == tag.StartTag.Length);
				count = m_BinaryReader.ReadInt32();
				bytes = new byte[tag.EndTag.Length];
				count = m_BinaryReader.Read(bytes, 0, bytes.Length);
				Debug.Assert(count == tag.EndTag.Length);

				pos = binaryReader.BaseStream.Position;
			}
			return pos;
		}
		/// <summary>
		/// Returns an enumerator that can iterate through collection of DiagnosticsLogItem objects.
		/// </summary>
		/// <returns>
		/// A DiagnosticsLogItem enumerator for the DiagnosticsLogItem Collection.
		/// </returns>
		/// <remarks>
		/// <para>
		/// Enumerators are intended to be used only to read data in the collection. 
		/// </para>
		/// <para>
		/// Enumerators cannot be used to modify the underlying collection.
		/// </para>
		/// <para>
		/// The enumerator does not have exclusive access to the collection.
		/// </para>
		/// <para>
		/// Initially, the enumerator is positioned before the first element in the collection. 
		/// Method Reset also brings the enumerator back to this position. Therefore, after an enumerator is created or 
		/// after a method Reset, method MoveNext must be called to advance the enumerator to the first element of the collection 
		/// before reading the value of method Current.
		/// </para>para>
		/// <para>
		/// The enumerator is in an invalid state if it is positioned before the first element in the 
		/// collection or after the last element in the collection. 
		/// Whenever the enumerator is in an invalid state, calling Current throws an exception.
		/// </para>para>
		/// <para>
		/// Method Current returns the same object until either method MoveNext or method Reset is called.
		/// </para>
		/// </remarks>
		[MethodImplAttribute(MethodImplOptions.Synchronized)]
		public IEnumerator GetEnumerator()
		{
			Reset();
			return (IEnumerator)this;
		}
		/// <summary>
		/// Advances the enumerator to the next log entry, DiagnosticsLogItem object, of the collection.
		/// </summary>
		/// <returns>
		/// true if the enumerator was successfully advanced to the next log entry; 
		/// false if the enumerator has passed the end of the collection.
		/// </returns>
		/// <remarks>
		/// Log entries can be added to a log file while being enumerated. 
		/// The number of log entries are determined when GetEnumerator method is called. 
		/// This means that log entries written to the log file after a call to GetEnumerator method 
		/// will not be accessible for this instantiated object unless GetEnumerator method is called again.
		/// </remarks>
		public bool MoveNext()
		{
			long l = m_CurrentIndex;
			if (++l < Count)
			{
				m_CurrentIndex++;
				return true;
			}
			return false;
		}
		/// <summary>
		/// Sets the enumerator to its initial position, which is before the first 
		/// object in the collection.
		/// </summary>
		public void Reset()
		{
			if (m_Version == DiagnosticsLogService.c_VersionEncyrptedLogFiles)
			{
				m_BinaryReader.BaseStream.Position =
					m_StartPositionEncryptedData;
			}
			else
			{
				m_fs.Position = 0;

				if (m_Reader != null)
				{
					m_Reader.Close();
					m_Reader = null;
				}
				m_Reader = new LogXmlTextReader(m_fs);
			}

			// initialize count and list of file positions.
			m_Count = Count;

			m_CurrentIndex = -1;
		}
		#endregion
	}
}
