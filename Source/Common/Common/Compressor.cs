// Comment out one of the two below lines to switch between singleton and non-singleton behavior.  If singleton is used, the 
//	 DeCompress and Compress classes will need to be made thread safe.
//#define UseSingleton
#undef UseSingleton

using System;
using System.IO;

using Xceed.FileSystem;
using Xceed.Zip;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Provides capability for encrypted decompression from a 
	/// file into a file. THis engine takes a single source file to a zip file,
	/// and extracts a single file out of the zip file.
	/// To use this engine, there are two redistributables that must be installed
	/// on the machine: dzncore.dll, in System 32, and dznet.dll. This latter file
	/// must be added as a reference to the project.
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: JLerner</item>
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
	///			<item name="vssfile">$Workfile: Compressor.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Common/Compressor.cs $</item>
	///			<item name="vssrevision">$Revision: 21 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Staran $</item>
	///			<item name="vssdate">$Date: 7/21/10 11:35a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class DeCompress	// UNZIP
	{
		#region TODO
		// 1) There appears to be a bug or poorly documented issue in the case of mem to mem
		// (de)compression calls. First call to the compress decompress leaves the object in 
		// a state such that subsequent calls fail. I work around this by recreating a new
		// object with each call. This works, but is very costly. Needs to be investigated,
		// so use the mem to mem feature in a limited way. THis problem does not appear to
		// occur with file to file.
		#endregion

		#region Constants
		/// <summary>
		/// TODO: need to store hidden
		/// </summary>
		private const string c_EncryptPWD =
			"SecureCompressDecompressKeyiQ5V4Files!!##$$";
		#endregion

		#region Member Data
#if UseSingleton
		/// <summary>
		/// Static private instance, used to implement singleton pattern.
		/// </summary>
		private static DeCompress m_DeCompress = null;
#endif
		/// <summary>
		/// Engine for unzipping a file.
		/// </summary>
		private CDUnZipSNET					_UnZipEngine = null;
		/// <summary>
		/// Error string.
		/// </summary>
		private string						_ErrorString;
		/// <summary>
		/// Error code.
		/// </summary>
		private CDUnZipSNET.UNZIPRESPONSE	_ErrorCode;
		/// <summary>
		/// Binary reader for unzip stream operations.
		/// </summary>
		BinaryReader objBinaryReader;
		/// <summary>
		/// Binary writer for unzip stream operations.
		/// </summary>
		BinaryWriter objBinaryWriter; 
		/// <summary>
		/// Input buffer memory stream for memory to memory compression.
		/// </summary>
		MemoryStream _InMS = null;
		// Allocate output memory stream.
		/// <summary>
		/// Output buffer memory stream for memory to memory compression.
		/// </summary>
		MemoryStream _OutMS = null;
		#endregion

		#region Accessors
		/// <summary>
		/// Gets error string.
		/// </summary>
		public string ErrorString
		{
			get{return _ErrorString;}
		}
		#endregion

		#region Delegates and Events
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Constructor for decompression engine.
		/// </summary>
		public DeCompress()
		{
			_UnZipEngine = new CDUnZipSNET();
		}
		#endregion

		#region Methods
		/// <summary>
		/// Singleton instance provider for compress.
		/// </summary>
		/// <returns>Class singleton instance</returns>
		public static DeCompress GetInstance()
		{
#if UseSingleton
			if (m_DeCompress == null)
			{
			   m_DeCompress = new DeCompress();
			}
			return m_DeCompress;
#else
			return new DeCompress();
#endif
		}
		internal (bool, string[]) ExtractFileListFromZipFile(string zipFilePath, string outputFolderPath)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Exrtract all files from a zip file
		/// </summary>
		/// <param name="zipFilePath">zip file</param>
		/// <param name="outputFolderPath">target folder, which need not exist.</param>
		/// <returns>true on success, false on failure.</returns>
		public bool ExtractAllFromZipFile(string zipFilePath, string outputFolderPath)
		{
			bool bRet = false;
			if (zipFilePath == null || outputFolderPath == null)
				throw new ArgumentNullException();

			// All operations share these flags.
			SetCommonDecompressionFlags();

			// Decryption Setup
			_UnZipEngine.DecryptFlag = true;
			if (_UnZipEngine.DecryptFlag == true)
				_UnZipEngine.DecryptCode = c_EncryptPWD;

			// The name of the zip file
			_UnZipEngine.ZIPFile = "\"" + zipFilePath + "\"";
			_UnZipEngine.Filespec = "*.*";
			_UnZipEngine.Destination = "\"" + outputFolderPath + "\"";
			_UnZipEngine.RecurseFlag = true;

			// Title to appear on the external progress dialog
			_UnZipEngine.ExtProgTitle = "";

			// Execute
			_UnZipEngine.ActionDZ = CDUnZipSNET.DUZACTION.UNZIP_EXTRACT;

			_ErrorString = _UnZipEngine.ErrorCode.ToString();
			_ErrorCode = _UnZipEngine.ErrorCode;
			bRet = (_ErrorCode == CDUnZipSNET.UNZIPRESPONSE.UE_OK) ? true : false;

			return bRet;
		}
		/// <summary>
		/// Decompress a memory Stream. Note that while this uses the
		/// winzip compression algorithm, there is no reference to the winzip file format.
		/// This just takes bytes to bytes. If you write this to a file it will not be in winzip
		/// format(unlike file to file method), but just a binary file. There appears to be
		/// a bug. Running the compression engine leaves it in a state where you cannot reuse it,
		/// so we have to create a new one. This is very costly and needs investigation. Doesn't
		/// happen for file to file.
		/// </summary>
		/// <param name="outputMemoryStream">Output memory stream</param>
		/// <param name="inputMemoryStream">Input memory Stream</param>
		/// <returns>true if successful, else false</returns>
		public bool DecompressMemoryStream(MemoryStream outputMemoryStream, 
			MemoryStream inputMemoryStream)
		{
			bool bRet = false;
			if(outputMemoryStream == null || inputMemoryStream == null || c_EncryptPWD == null)
				throw new ArgumentNullException();

			// All operations share these flags.
			SetCommonDecompressionFlags();

			_InMS = inputMemoryStream;
			_OutMS = outputMemoryStream;

			// Encryption Setup
			_UnZipEngine.DecryptFlag = true;
			if(_UnZipEngine.DecryptFlag == true)
				_UnZipEngine.DecryptCode = c_EncryptPWD;

			_UnZipEngine.UnZipMemToMemCallback += new CDUnZipSNET.OnUnZipMemToMemCallback(this.UnZipMemToMemCallback_event);
			//Cause DynaZip .NET to perform the mem to mem uncompression operation
			_UnZipEngine.ActionDZ = CDUnZipSNET.DUZACTION.UNZIP_MEMTOMEM;

			_ErrorString = _UnZipEngine.ErrorCode.ToString();
			_ErrorCode = _UnZipEngine.ErrorCode;
			bRet = (_ErrorCode == CDUnZipSNET.UNZIPRESPONSE.UE_OK) ? true : false;

			// There appears to be a bug in mem to mem that leaves the object in a state that blocks reuse
			// of the object, so we must create a new one. Very costly.
			_UnZipEngine = null;
			_UnZipEngine = new CDUnZipSNET();

			return bRet;
		}

        /// <summary>
        /// Callback function for buffered decompression.
        /// </summary>
        /// <param name="lAction"></param>
        /// <param name="lpMemBuf"></param>
        /// <param name="pdwSize"></param>
        /// <param name="dwTotalReadL"></param>
        /// <param name="dwTotalReadH"></param>
        /// <param name="dwTotalWrittenL"></param>
        /// <param name="dwTotalWrittenH"></param>
        /// <param name="plRet"></param>
        private void UnZipMemToMemCallback_event(CDUnZipSNET.MEMTOMEMACTION lAction, ref byte[] lpMemBuf, ref uint pdwSize, uint dwTotalReadL, uint dwTotalReadH, uint dwTotalWrittenL, uint dwTotalWrittenH, ref CDUnZipSNET.MEMTOMEMRESPONSE plRet)
		{
			int bytesToRead;
			switch(lAction)
			{
				case CDUnZipSNET.MEMTOMEMACTION.MEM_READ_DATA:
					//DynaZip is requesting some input data to be uncompressed
					//If this is the first time DynaZip is requesting the input data then
					//initialize the input stream and seek to the beginning of the stream
					//Seeking in a stream requires that the stream be seekable.
					//If your stream can not be seeked then your initialization will be
					//different
					if((dwTotalReadL == 0) && (dwTotalReadH == 0))
					{
						//This is the first time DynaZip is requesting input data
						//Seek to the beginning of the memory stream
						_InMS.Seek(0, System.IO.SeekOrigin.Begin);
						//Create a binary reader object attached to this stream
						//we use it to get the actual data out of the stream into the lpMemBuf array
						objBinaryReader = new System.IO.BinaryReader(_InMS);
					}
					try
					{
						//Figure out how many data bytes we can read this time.
						//Never read more bytes that DynaZip is requesting.
						//the pdwSize parameter indicates the maximum number of bytes
						//DynaZip will safely accept.
						bytesToRead = (int)(objBinaryReader.BaseStream.Length - dwTotalReadL);
						//If the stream still has more bytes that DynaZip can accept, then return the 
						//maximum number requested by DynaZip, otherwise just read the remaining bytes
						//from the memory stream and set the pdwSize parameter to the number of bytes
						//actually read.
						if(bytesToRead > pdwSize)
						{
							//Read a full buffer
							bytesToRead = (int)pdwSize;
							// there will be more data to read from the stream
							plRet = CDUnZipSNET.MEMTOMEMRESPONSE.MEM_CONTINUE;
						}
						else
						{
							//Stream has no more data
							plRet = CDUnZipSNET.MEMTOMEMRESPONSE.MEM_DONE;
						}
						pdwSize = (uint)bytesToRead;
						if(bytesToRead > 0)
						{
							objBinaryReader.Read(lpMemBuf, 0, bytesToRead);
						}
					}
					catch //Not reported to user
					{
						//This is a severe error
						plRet = CDUnZipSNET.MEMTOMEMRESPONSE.MEM_ERROR;
					}
					break;

				case CDUnZipSNET.MEMTOMEMACTION.MEM_WRITE_DATA:
					//DynaZip is providing the uncompressed data a block at a time
					//If this is the first time DynaZip is providing output data for this
					//uncompression operation, then we initialize the output stream
					if((dwTotalWrittenL == 0) && (dwTotalWrittenH == 0))
					{
						//we need to access the output stream with a binary writer interface
						objBinaryWriter = new System.IO.BinaryWriter(_OutMS);
					}
					try
					{
						//Write the data that DynaZip is providing.
						objBinaryWriter.Write(lpMemBuf, 0, (int)pdwSize);
						//All is good, so continue to receive more until done
						plRet = CDUnZipSNET.MEMTOMEMRESPONSE.MEM_CONTINUE;
					}
					catch //Not reported to user
					{
						//This is a severe error
						plRet = CDUnZipSNET.MEMTOMEMRESPONSE.MEM_ERROR;
					}
					break;

				default:
					//This is a severe error
					plRet = CDUnZipSNET.MEMTOMEMRESPONSE.MEM_ERROR;
					break;
			}
		}

		/// <summary>
		/// Do decompression. We require that there be exactly 1 data file in the zip file.
		/// We determine the name of that file, so you don't have to know this in advance.
		/// </summary>
		/// <param name="fullPathZipFileName">Full path name of zip file to decompress.
		/// Example: ""E:\Test Data\2ColorFamTexRed.zip"</param>
		/// <param name="fullPathDest">Full path to the destination directory for the contained file.
		/// Example: "E:\Test Data"</param>
		/// <returns>true if successfully decompresses the file, else false.</returns>
		public bool DecompressFile(string fullPathZipFileName, string fullPathDest)
		{
			bool bRet = false;
			if(fullPathZipFileName == null || fullPathDest == null || c_EncryptPWD == null)
				throw new ArgumentNullException();

			// All operations share these flags.
			SetCommonDecompressionFlags();

			// Decryption Setup
			_UnZipEngine.DecryptFlag = true;
			if(_UnZipEngine.DecryptFlag == true)
				_UnZipEngine.DecryptCode = c_EncryptPWD;

			// The name of the zip file
			_UnZipEngine.ZIPFile = "\"" + fullPathZipFileName + "\"";

			// Extract the file name of the source file from the zip file. This is required to 
			// unzip the file and we really don't know a priori what it is.
			// FileSpecs to match for unzip by named method. We require that the
			// contained source file be of the same name as the zip file, with no path.
			string fileName = ExtractSourceFileName(fullPathZipFileName);
			if(!string.IsNullOrEmpty(fileName))
			{
				_UnZipEngine.Filespec = "\"" + fileName + "\"";
				if(_UnZipEngine.Filespec == null) return bRet;

				// DynaZIP .NET String Properties
				// Set Folder to UnZIP into
				_UnZipEngine.Destination = "\"" + fullPathDest + "\"";

				// Title to appear on the external progress dialog
				_UnZipEngine.ExtProgTitle = "";

				// Execute.
				// The list of files in the zip files is 0-based. We get the first,
				// and only file.
				// Return Count and Return String initialization
				_UnZipEngine.ReturnCount = 1;
				_UnZipEngine.ReturnString = "";
				_UnZipEngine.UnZIPIndex = 0;
				_UnZipEngine.ActionDZ = CDUnZipSNET.DUZACTION.UNZIP_EXTRACT;
				_ErrorString = _UnZipEngine.ErrorCode.ToString();
				_ErrorCode = _UnZipEngine.ErrorCode;
				bRet = (_ErrorCode == CDUnZipSNET.UNZIPRESPONSE.UE_OK) ? true : false;
			}
			else
			{
				_ErrorString = "CDUnZipSNet Engine: Must be only 1 file in zip file.";		
			}

			return bRet;
		}

		/// <summary>
		/// We will assume that there is only a single file in the zip file, 
		/// so we take this first file name out.
		/// </summary>
		/// <param name="fullPathZipFileName">Full path to zip file containing compressed file.</param>
		/// <returns></returns>
		public string ExtractSourceFileName(string fullPathZipFileName)
		{
			// All operations share these flags.
			SetCommonDecompressionFlags();

			// The name of the zip file
			_UnZipEngine.ZIPFile = "\"" + fullPathZipFileName + "\"";

			// Get count of items in zip file as a check. It should be one.
			_UnZipEngine.ActionDZ = CDUnZipSNET.DUZACTION.UNZIP_COUNTALLZIPMEMBERS;
			int fileCount = _UnZipEngine.ReturnCount;
			if(fileCount != 1) 
            {
                return string.Empty;
            }
			else
			{
				// Get name of first file in zip file.
				_UnZipEngine.ActionDZ = CDUnZipSNET.DUZACTION.UNZIP_GETNEXTZIPINFO;

				string fullPathString = _UnZipEngine.zi_FileName;
				return Path.GetFileName(fullPathString);
			}
		}

		/// <summary>
		/// Sets flags common to all operations.
		/// </summary>
		private void SetCommonDecompressionFlags()
		{
			// Always set to false for DynaZIP .NET
			_UnZipEngine.UseEncodedBinary = false;

			// Always set to zero for DynaZIP .NET
			_UnZipEngine.WaitSeconds = 0;

			// Always set to true for DynaZIP .NET
			_UnZipEngine.BackgroundProcessFlag = true;

			// UnZip String and memory related properties
			_UnZipEngine.UnZIPString = "";
			_UnZipEngine.UnZIPStringOffset = 0;
			_UnZipEngine.UnZIPStringSize = 0;

			// Make a reference to a single byte to avoid null access
			_UnZipEngine.MemoryBlock = new Byte[1];
			_UnZipEngine.StartingOffset = 0;
			_UnZipEngine.MemoryBlockSize = 0;

			// If true = Overwrite non-read only files without asking
			_UnZipEngine.OverwriteFlag = true;
			// true = only test without extracting files
			_UnZipEngine.TestFlag = false;
			// true = don't display warning messages, other messages still appear
			_UnZipEngine.QuietFlag = true;
			// true = don't display any messages
			_UnZipEngine.AllQuiet = true;
			// true = proceed to unzip by traversing into folders
			_UnZipEngine.RecurseFlag = false;
			// true = do not extract any directory items.
			// a directory item has a "d" attribute
			_UnZipEngine.NoDirectoryItemsFlag = true;
			// true = remove all path prefixes when extracting 
			// items that may include path parts
			_UnZipEngine.NoDirectoryNamesFlag = true;
			// true = while extracting items translate Line Feed characters
			// into Carriage Return Line Feed character pairs.
			// only use this option when extracting pure text data
			_UnZipEngine.ConvertLFToCRLFFlag = false;
			// true = create a diagnostic log file in the winnt folder.
			// this option is useful for determining the cause of 
			// invalid parameter problems  
			_UnZipEngine.DiagnosticFlag = false;
			// true = issue a rename callback event for each item
			// as it is extracted from the zip file.
			_UnZipEngine.RenameCallbackFlag = false;

			// UnZipSubOptions to be set
			_UnZipEngine.UnzipSubOptions = CDUnZipSNET.UNZIPSUBOPTION.USO_NONE;
			// set this option to allow the overwriting of ReadOnly Files.
			// this option only has meaning when the OverwriteFlag property
			// is set to true
			//duzn.UnzipSubOptions |= CDUnZipSNET.UNZIPSUBOPTION.USO_OVERWRITE_RO;

			// allow the MinorCancel event to respond to cancel requests
			//	duzn.UnzipSubOptions |= CDUnZipSNET.UNZIPSUBOPTION.USO_MINORCANCEL;
			// display an external progress window via DynaZIP .NET
			//	duzn.UnzipSubOptions |= CDUnZipSNET.UNZIPSUBOPTION.USO_EXTERNALPROG;
			// Allow the user to cancel an extraction via the external progress display
			// duzn.UnzipSubOptions |= CDUnZipSNET.UNZIPSUBOPTION.USO_EXTERNALPROGCANCEL;
			// Don't extract any items in the zip file that have long file names
			// duzn.UnzipSubOptions |= CDUnZipSNET.UNZIPSUBOPTION.USO_IGNORELONGNAMES;
			
			// perform a simple mangling of file names to become Old DOS 8.3 file names
			//	duzn.UnzipSubOptions |= CDUnZipSNET.UNZIPSUBOPTION.USO_MANGLELONGNAMES;
			
			// if an extraction fails, then the partial file that was extracted will
			// left on the system
			//	duzn.UnzipSubOptions |= CDUnZipSNET.UNZIPSUBOPTION.USO_LEAVEPARTIAL;
			
			// CAUTION!!!  Only use this option if absolutely necessary
			// It forces all drives to behave as if they are removable types
			//	duzn.UnzipSubOptions |= CDUnZipSNET.UNZIPSUBOPTION.USO_FORCEREMOVABLE;

			// Keep a log of the files as they are extracted
			// duzn.UnzipSubOptions |= CDUnZipSNET.UNZIPSUBOPTION.USO_LOGZIPRESULTS; 
			
			// Don't rely on Volume Labels when unzipping from Multi-Volume Files
			//	duzn.UnzipSubOptions |= CDUnZipSNET.UNZIPSUBOPTION.USO_IGNOREVOLLABELS; 
			
			// Force pure synchronous activity without releasing control to OS
			//	duzn.UnzipSubOptions |= CDUnZipSNET.UNZIPSUBOPTION.USO_INHIBITPUMP; 
			
			// Process Multi-Volume Files from the hard Drive
			//	duzn.UnzipSubOptions |= CDUnZipSNET.UNZIPSUBOPTION.USO_MVFROMHD; 
			
			// Process ANSI Names for items instead of OEM Names
			//	duzn.UnzipSubOptions |= CDUnZipSNET.UNZIPSUBOPTION.USO_ANSI_NAMES; 

			// duzn.UnzipSubOptions2 = CDUnZipSNET.UNZIPSUBOPTION2.USO2_NONE;
			// If user wants to use Extended Renaming
			//	duzn.UnzipSubOptions2 |= CDUnZipSNET.UNZIPSUBOPTION2.USO2_EXTRENAME;
			
			// If user wants to extract the items in the ordere determined by the FileSpecs
			//	duzn.UnzipSubOptions2 |= CDUnZipSNET.UNZIPSUBOPTION2.USO2_ITEMLIST_ISORDERED;
			
			// If user wants to interpret the FileSpecs as a list of Indices
			//	duzn.UnzipSubOptions2 |= CDUnZipSNET.UNZIPSUBOPTION2.USO2_ITEMLIST_ISINDEXLIST;		

			// Preset the state of the Zip Info properties
			_UnZipEngine.zi_attr = 0;
			_UnZipEngine.zi_cMethod = 0;
			_UnZipEngine.zi_cOption = 0;
			_UnZipEngine.zi_cPathType = 0;
			_UnZipEngine.zi_crc_32 = 0;
			_UnZipEngine.zi_cSize = 0;
			_UnZipEngine.zi_DateTime = "";
			_UnZipEngine.zi_FileName = "";
			unchecked
			{
				// Force the -1 Index setting into an unsigned int value
				_UnZipEngine.zi_Index = -1;
			}
			_UnZipEngine.zi_oSize = 0;
			_UnZipEngine.zi_resv1 = 0;
			_UnZipEngine.zi_ExtVersion = 0;

			// setup DynaZIP .NET Event Handlers
			/*
			_UnZipEngine.MajorStatusFlag = MajorStatus.Checked;
			_UnZipEngine.MinorStatusFlag = MinorStatus.Checked;
			_UnZipEngine.MessageCallbackFlag = true;
			_UnZipEngine.UnZipMajorStatus += new CDUnZipSNET.OnUnZipMajorStatus(this.UnZipMajorStatus_event);
			_UnZipEngine.UnZipMinorStatus += new CDUnZipSNET.OnUnZipMinorStatus(this.UnZipMinorStatus_event);
			_UnZipEngine.UnZipRenameCallback += new CDUnZipSNET.OnUnZipRenameCallback(this.UnZipRenameCallback_event);
			_UnZipEngine.UnZipMessageCallback += new CDUnZipSNET.OnUnZipMessageCallback(this.UnZipMessageCallback_event);
			_UnZipEngine.UnZipRenameCallbackEX += new CDUnZipSNET.OnUnZipRenameCallbackEX(this.UnZipRenameCallbackEX_event);
			*/
		}
		#endregion

		#region Event Handlers
		#endregion
	}


	#region Documentation Tags
	/// <summary>
	/// Provides capability for encrypted compression from a 
	/// file into a file. THis engine takes a single source file to a zip file,
	/// and extracts a single file out of the zip file.
	/// To use this engine, there are two redistributables that must be installed
	/// on the machine: dzncore.dll, in System 32, and dznet.dll. This latter file
	/// must be added as a reference to the project.
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: JLerner</item>
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
	///			<item name="vssfile">$Workfile: Compressor.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Common/Compressor.cs $</item>
	///			<item name="vssrevision">$Revision: 21 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Staran $</item>
	///			<item name="vssdate">$Date: 7/21/10 11:35a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public class Compress	// ZIP
	{
		#region Constants
		private const string c_EncryptPWD =
			"SecureCompressDecompressKeyiQ5V4Files!!##$$";
		#endregion

		#region Member Data
#if UseSingleton
		/// <summary>
		/// Static private instance, used to implement singleton pattern.
		/// </summary>
		private static Compress m_Compress = null;
#endif
		/// <summary>
		/// Compression engine.
		/// </summary>
		private CDZipSNET				_ZipEngine = null;
		/// <summary>
		/// Error string.
		/// </summary>
		private string					_ErrorString = null;
		/// <summary>
		/// Error code.
		/// </summary>
		private CDZipSNET.ZIPRESPONSE	_ErrorCode;
		/// <summary>
		/// Binary reader for unzip stream operations.
		/// </summary>
		BinaryReader objBinaryReader;
		/// <summary>
		/// Binary writer for unzip stream operations.
		/// </summary>
		BinaryWriter objBinaryWriter; 
		/// <summary>
		/// Input memory stream.
		/// </summary>
		MemoryStream _InMS = null;
		/// <summary>
		/// Output memory stream.
		/// </summary>
		MemoryStream _OutMS = null;
		#endregion

		#region Accessors
		/// <summary>
		/// Gets error string.
		/// </summary>
		public string ErrorString
		{
			get{return _ErrorString;}
		}
		#endregion

		#region Delegates and Events
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Constructor for compression class.
		/// </summary>
		public Compress()
		{
			_ZipEngine = new CDZipSNET();
		}
		#endregion

		#region Methods
		/// <summary>
		/// Singleton instance provider for compress.
		/// </summary>
		/// <returns>Class singleton instance</returns>
		public static Compress GetInstance()
		{
#if UseSingleton
			if (m_Compress == null)
			{
				m_Compress = new Compress();
			}
			return m_Compress;
#else
			return new Compress();
#endif
		}
		/// <summary>
		/// Compress a memory stream to another memory stream. Note that while this uses the
		/// winzip compression algorithm, there is no reference to the winzip file format.
		/// This just takes bytes to bytes. If you write this to a file it will not be in winzip
		/// format(unlike file to file method), but just a binary file. There appears to be
		/// a bug. Running the compression engine leaves it in a state where you cannot reuse it,
		/// so we have to create a new one. This is very costly and needs investigation. Doesn't
		/// happen for file to file.
		/// </summary>
		/// <param name="outputMemoryStream"></param>
		/// <param name="inputMemoryStream"></param>
		/// <returns></returns>
		public bool CompressMemoryStream(MemoryStream outputMemoryStream, 
			MemoryStream inputMemoryStream)
		{
			bool bRet = false;
			if(outputMemoryStream == null || inputMemoryStream == null || c_EncryptPWD == null)
				throw new ArgumentNullException();

			// All operations share these flags.
			SetCommonCompressionFlags(); 
			_InMS = inputMemoryStream;
			_OutMS = outputMemoryStream;

			// Encryption Setup
			_ZipEngine.EncryptFlag = true;
			if(_ZipEngine.EncryptFlag == true)
				_ZipEngine.EncryptCode = c_EncryptPWD;

			//Cause DynaZip .NET to perform the mem to mem compression operation
			_ZipEngine.ZipMemToMemCallback += new CDZipSNET.OnZipMemToMemCallback(this.ZipMemToMemCallback_event);
			_ZipEngine.ActionDZ = CDZipSNET.DZACTION.ZIP_MEMTOMEM;

			_ErrorString = _ZipEngine.ErrorCode.ToString();
			_ErrorCode = _ZipEngine.ErrorCode;
			bRet = (_ErrorCode == CDZipSNET.ZIPRESPONSE.ZE_OK) ? true : false;

			// There appears to be a bug in mem to mem that leaves the object in a state that blocks reuse
			// of the object, so we must create a new one. Very costly.
			_ZipEngine = null;
			_ZipEngine = new CDZipSNET();

			return bRet;
		}

		/// <summary>
		/// Callback method for buffered memory based compression.
		/// </summary>
		/// <param name="lAction"></param>
		/// <param name="lpMemBuf"></param>
		/// <param name="pdwSize"></param>
		/// <param name="dwTotalReadL"></param>
		/// <param name="dwTotalReadH"></param>
		/// <param name="dwTotalWrittenL"></param>
		/// <param name="dwTotalWrittenH"></param>
		/// <param name="plRet"></param>
		private void ZipMemToMemCallback_event(CDZipSNET.MEMTOMEMACTION lAction, ref byte[] lpMemBuf, ref uint pdwSize, uint dwTotalReadL, uint dwTotalReadH, uint dwTotalWrittenL, uint dwTotalWrittenH, ref CDZipSNET.MEMTOMEMRESPONSE plRet)
		{
			int bytesToRead;
			switch(lAction)
			{
				case CDZipSNET.MEMTOMEMACTION.MEM_READ_DATA:
					//DynaZip is requesting some input data to be uncompressed
					//If this is the first time DynaZip is requesting the input data then
					//initialize the input stream and seek to the beginning of the stream
					//Seeking in a stream requires that the stream be seekable.
					//If your stream can not be seeked then your initialization will be
					//different
					if((dwTotalReadL == 0) && (dwTotalReadH == 0))
					{
						//This is the first time DynaZip is requesting input data
						//Seek to the beginning of the memory stream
						_InMS.Seek(0, System.IO.SeekOrigin.Begin);
						
						//Create a binary reader object attached to this stream
						//we use it to get the actual data out of the stream into the lpMemBuf array
						objBinaryReader = new System.IO.BinaryReader(_InMS);
					}
					try
					{
						//Figure out how many data bytes we can read this time.
						//Never read more bytes that DynaZip is requesting.
						//the pdwSize parameter indicates the maximum number of bytes
						//DynaZip will safely accept.
						bytesToRead = (int)(objBinaryReader.BaseStream.Length - dwTotalReadL);
						//If the stream still has more bytes that DynaZip can accept, then return the 
						//maximum number requested by DynaZip, otherwise just read the remaining bytes
						//from the memory stream and set the pdwSize parameter to the number of bytes
						//actually read.
						if(bytesToRead > pdwSize)
						{
							//Read a full buffer
							bytesToRead = (int)pdwSize;
							// there will be more data to read from the stream
							plRet = CDZipSNET.MEMTOMEMRESPONSE.MEM_CONTINUE;
						}
						else
						{
							//Stream has no more data
							plRet = CDZipSNET.MEMTOMEMRESPONSE.MEM_DONE;
						}
						pdwSize = (uint)bytesToRead;
						if(bytesToRead > 0)
						{
							objBinaryReader.Read(lpMemBuf, 0, bytesToRead);
						}
					}
					catch  //Not reported to user
					{
						//This is a severe error
						plRet = CDZipSNET.MEMTOMEMRESPONSE.MEM_ERROR;
					}
					break;

				case CDZipSNET.MEMTOMEMACTION.MEM_WRITE_DATA:
					//DynaZip is providing the uncompressed data a block at a time
					//If this is the first time DynaZip is providing output data for this
					//uncompression operation, then we initialize the output stream
					if((dwTotalWrittenL == 0) && (dwTotalWrittenH == 0))
					{
						//we need to access the output stream with a binary writer interface
						objBinaryWriter = new System.IO.BinaryWriter(_OutMS);
					}
					try
					{
						//Write the data that DynaZip is providing.
						objBinaryWriter.Write(lpMemBuf, 0, (int)pdwSize);
						//All is good, so continue to receive more until done
						plRet = CDZipSNET.MEMTOMEMRESPONSE.MEM_CONTINUE;
					}
					catch  //Not reported to user
					{
						//This is a severe error
						plRet = CDZipSNET.MEMTOMEMRESPONSE.MEM_ERROR;
					}
					break;

				default:
					//This is a severe error
					plRet = CDZipSNET.MEMTOMEMRESPONSE.MEM_ERROR;
					break;
			}
		}

		/// <summary>
		/// Compresses a file. We allow for exactly one file to be compressed into a zip file.
		/// File format is Winzip.
		/// </summary>
		/// <param name="fullPathZipFileName">Full path for target zip file to be created.
		/// Example: "E:\Test Data\2ColorFamTexRed.zip"</param>
		/// <param name="fullPathSourceFile">Full path for single source file to be compressed.
		/// Example: "E:\Test Data\2ColorFamTexRed.opd"</param>
		/// <returns>true if compression occurs, else false.</returns>
		public bool CompressFile(string fullPathZipFileName, string fullPathSourceFile)
		{
			bool bRet = false;
			if(fullPathZipFileName == null || fullPathSourceFile == null || c_EncryptPWD == null)
				throw new ArgumentNullException();
			// All operations share these flags.
			SetCommonCompressionFlags();

			// Encryption Setup
			_ZipEngine.EncryptFlag = true;
			if(_ZipEngine.EncryptFlag == true)
				_ZipEngine.EncryptCode = c_EncryptPWD;

			// The name of the zip file which will be created or added to.
			_ZipEngine.ZIPFile = "\"" + fullPathZipFileName + "\"";

			// Actually, we assume that there will be only one file compressed.
			// A list of SPACE delimited file specifications that determine
			// which files will be zipped.  Each filespec must be described
			// as a fully qualified drive with pathname.  The filename and
			// any extension can contain the ? or * wildcard chareacters.
			// UNC paths are also supported.
			// IMPORTANT
			// If a filespec in the ItemList describes a long file name which contains
			// one or more SPACE characters in its folder or file name parts, then that
			// filespec must be enclosed within double quotes.
			//
			// for example:
			// c:\my folder of files\my documents.*
			// should be programatically entered into the ItemList as
			// "\"" + "c:\my folder of files\my documents.*" + "\""

			// Viet:todo: In Visual Studio only: The zip call below causes unknown handling when running 
			// simulation with at least one simulated instrument 
			// Symtom: Experiment Setup Form -> Create new protocol/plate or edit existing protocol/plate
			// Problem: Can not close the forms and app can not receive any mouse click event afterward
			// App works OK with released version or when app runs outside Visual Studio

			//PW 5/1/07: I think it works now!
//#if DEBUG
			//bRet = true;
//#else			
			_ZipEngine.ItemList = "\"" + fullPathSourceFile + "\"";
		
			_ZipEngine.ActionDZ = CDZipSNET.DZACTION.ZIP_ADD;	// this line causes unknown handling

			_ErrorString = _ZipEngine.ErrorCode.ToString();
			_ErrorCode = _ZipEngine.ErrorCode;
			bRet = (_ErrorCode == CDZipSNET.ZIPRESPONSE.ZE_OK) ? true : false;
//#endif					
			return bRet;
		}
		/// <summary>
		/// Creates zip file from provided files.
		/// </summary>
		/// <param name="fullPathFileNames">File names with full path to be zipped</param>
		/// <param name="fullPathZipFileName">Zip file name to be created</param>
		/// <param name="allowRecursionMode">Set to false in normal mode. 
		/// If set to true, all files in the specified folder will be zipped recursively and 
		/// their relative path stored as well in the zip file</param>
		/// <returns>true if compression occurs, else false.</returns>		 
		public bool CreateZipFile(string[] fullPathFileNames, string fullPathZipFileName,
			bool allowRecursionMode)
		{
			bool bRet = false;
			if (fullPathZipFileName == null || fullPathFileNames == null)
				throw new ArgumentNullException();
			if (File.Exists(fullPathZipFileName))
				File.Delete(fullPathZipFileName);
			// All operations share these flags.
			SetCommonCompressionFlags();

			//Override needed flags when recursive mode is specified
			if (allowRecursionMode)
			{
				_ZipEngine.NoDirectoryEntriesFlag = false;
				_ZipEngine.NoDirectoryNamesFlag = false;
				_ZipEngine.RecurseFlag = true;
				_ZipEngine.ZipSubOptions |= CDZipSNET.ZIPSUBOPTION.ZSO_RELATIVEPATHFLAG;
			}
			// Encryption Setup
			_ZipEngine.EncryptFlag = false;

			// The name of the zip file which will be created or added to.
			_ZipEngine.ZIPFile = "\"" + fullPathZipFileName + "\"";

			// Actually, we assume that there will be only one file compressed.
			// A list of SPACE delimited file specifications that determine
			// which files will be zipped.  Each filespec must be described
			// as a fully qualified drive with pathname.  The filename and
			// any extension can contain the ? or * wildcard chareacters.
			// UNC paths are also supported.
			// IMPORTANT
			// If a filespec in the ItemList describes a long file name which contains
			// one or more SPACE characters in its folder or file name parts, then that
			// filespec must be enclosed within double quotes.
			//
			// for example:
			// c:\my folder of files\my documents.*
			// should be programatically entered into the ItemList as
			// "\"" + "c:\my folder of files\my documents.*" + "\""
			string itemList = "";
			foreach (string file in fullPathFileNames)
			{
				itemList = string.Concat(itemList, "\"" + file + "\"", " ");
			}
			_ZipEngine.ItemList = itemList.Trim();

			_ZipEngine.ActionDZ = CDZipSNET.DZACTION.ZIP_ADD;
			_ErrorString = _ZipEngine.ErrorCode.ToString();
			_ErrorCode = _ZipEngine.ErrorCode;
			bRet = (_ErrorCode == CDZipSNET.ZIPRESPONSE.ZE_OK) ? true : false;

			return bRet;
		}
		/// <summary>
		/// Sets flags common to all compression.
		/// </summary>
		private void SetCommonCompressionFlags()
		{
			// Always set the properties to true
			_ZipEngine.BackgroundProcessFlag = true;

			// Always set the properties to false
			_ZipEngine.UseEncodedBinary = false;

			// Always set the properties to 0
			_ZipEngine.WaitSeconds = 0;

			// Various flag settings
			// Set to true to add a comment to the zip file
			_ZipEngine.AddCommentFlag = false;
			// Set to a string with a comment, needs AddCommentFlag = true
			_ZipEngine.Comment = "";

			// Set to true to zip only files having dates after a specified date 
			_ZipEngine.AfterDateFlag = false;
			// Set to a string with the following format
			// mm/dd/yy
			// where:
			//   mm = 2-digit month 01 thru 12
			//   dd = 2-digit day of month 01 thru 31
			//   yy = 2-digit year 80 thru 79 (1980 thru 2079)
			// this property only has meaning if AfterDateFlag = true
			_ZipEngine.Date = "";

			// Set to true to inhibit all DynaZIP messages from appearing
			_ZipEngine.AllQuiet = true;
			// Set to tru to translate all Line Feed characters into 
			// Carriage Return-Line Feed character pairs.  Only use this
			// option when zipping pure text files
			_ZipEngine.ConvertLFToCRLFFlag = false;
			// Set to true to cause DynaZIP to delete the original
			// files that are being zipped.  
			// USE EXTREME CAUTION!
			_ZipEngine.DeleteOriginalFlag = false;
			// Set to true to cause DynaZIP to create a Diagnostic LOG
			// file which contains useful debugging information
			// this option should be set to false when you deploy the
			// final version of your application
			_ZipEngine.DiagnosticFlag = false;
			// Set to true to force DynaZIP to NOT compress files having
			// any extension matching the ones listed in the StoreSuffixes
			// string property
			_ZipEngine.DontCompressTheseSuffixesFlag = false;
			// A list of file extensions which should not be compressed
			// only has meaning if DontCompressTheseSuffixesFlag = true
			_ZipEngine.StoreSuffixes = "";

			// Set to true to cause DynaZIP to convert all filenames to
			// Upper Case characters as is the case for old DOS filenames
			_ZipEngine.DosifyFlag = false;

			// Set to true to cause DynaZIP to exclude any files matching the ExcludeList
			// from the zip process.  The matching algorithm assumes a naming convention 
			// relative to the way the file names and their paths would have appeared
			// assuming they were actually zipped.  So there is a dependency on the
			// following properties:
			//
			//   ZSO_RELATIVEPATHFLAG
			//   RecurseFlag
			//   ItemList
			//   noDirectoryNamesFlag
			//   noDirectoryEntriesFlag
			//   IncludeFollowing
			//   IncludeOnlyFollowingFlag
			//
			_ZipEngine.ExcludeFollowingFlag = false;
			// The filespec list describing what files to exclude from
			// the zipping process.  Only has effect if the ExcludeFollowingFlag
			// is set to true.
			_ZipEngine.ExcludeFollowing = "";

			//Set to a custom title that will appear on the title
			//bar of the external progress dialog.  Only has meaning if the 
			// ZSO_EXTERNALPROG option is set in the Zip SubOptions
			_ZipEngine.ExtProgTitle = "";

			// IMPORTANT!
			// The Fix and FixHarder options may drastically modify a 
			// zip file by deleting items that can not be recovered.  
			// It is always best to make a safe copy of the original zip 
			// file before using these options
			
			// Set to true to allow DynaZIP to attempt to fix a zip file
			// that may some some mild corruption.
			_ZipEngine.FixFlag = false;
			// Set to true to allow DynaZIP to attempt to fix a zip file
			// that may be more extensively corrupted.  
			_ZipEngine.FixHarderFlag = false;

			// A string to be zipped.  
			// Only has meaning for the ZIP_MEMTOFILE action
			// and can not be used at the same time as the MemoryBlock 
			// and MemoryBlockSize properties.
			// The item name will be set to the string contained in the 
			// ItemList property
			_ZipEngine.ZIPString = "";

			// Set to true to allow DynaZIP to append new file items
			// directly to an existing zip file.  Selecting this option
			// dramatically increases the zip performance but sacrifices
			// zip file recovery in the event of a power failure or other
			// system failure that could cause the zip file to become
			// incomplete.  Under normal zip operations when this option
			// is set to false, DynaZIP will first make a temporary copy
			// of the original zip file and then append new items to the 
			// temporary copy until the zip operation completes.  At the
			// end of the zip operation DynaZIP will then Delete the original
			// zip file and rename (or copy if necessary) the temporary zip file 
			// so it becomes the new original zip file.
			// Grow Existing will only be in effect if the list of items to be
			// zipped does not match any of the items already in the existing
			// original zip file.
			_ZipEngine.GrowExistingFlag = false;

			// Set to true to cause DynaZIP to include only those files matching the 
			// IncludeFollowing list during the zip process.  The matching algorithm 
			// assumes a naming convention relative to the way the file names and their
			// paths will appear when they are zipped.  So there is a dependency on the
			// following properties:
			//
			//   ZSO_RELATIVEPATHFLAG
			//   RecurseFlag
			//   ItemList
			//   noDirectoryNamesFlag
			//   noDirectoryEntriesFlag
			//   ExcludeFollowingFlag
			//   ExcludeFollowing
			_ZipEngine.IncludeOnlyFollowingFlag = false;

			// The filespec list describing what files to include during
			// the zipping process.  Only has effect if the IncludeOnlyFollowingFlag
			// is set to true.
			_ZipEngine.IncludeFollowing = "";

			// Set to true to cause DynaZIP to include SYSTEM and HIDDEN file types
			// in a zip file
			_ZipEngine.IncludeSysandHiddenFlag = false;

			// Set to true to cause DynaZIP to include an item in the zip file which
			// describes the volume name for the originating disk drive
			_ZipEngine.IncludeVolumeFlag = false;

			// A zip file can contain the following types of entries.
			// FileName only
			// FileName with Path prefix
			// Path only
			// Volume Label Entry

			// Setting this flag to true cause DynaZIP to exclude
			// the Path only entry type from a zip file during
			// the current zip process.  If a zip file is being updated
			// or added to, and it already contains Path type entries,
			// those Path type entries remain intact.
			_ZipEngine.NoDirectoryEntriesFlag = true;

			// Setting this flag to true cause DynaZIP to remove
			// the Path prefix from all FileName entries in a zip file
			// during the current zip process.  If a zip file is being 
			// updated or added to, and it already contains FileNames with
			// Path prefix type entries, those entries remain intact.
			// This option has the effect of placing all FileName entries at
			// the root level of the zip file.  If two or more items have the
			// same FileName then the last item found during the drive search
			// having that name will be zipped.
			_ZipEngine.NoDirectoryNamesFlag = true;

			// Set to true to cause DynaZIP to set the zip file's date and
			// time to be the same as the item in the zip file which has the 
			// latest date and time.
			_ZipEngine.OldAsLatestFlag = false;

			// Set to true to cause DynaZIP to use a specified temporary
			// directory when creating any temp files needed during the
			// zip process.  Usually temp files are created in the same folder
			// as the one where the zip file will be created.
			_ZipEngine.PathForTempFlag = false;
			// Set to the fully qualified Path name with Drive Letter or
			// UNC path to be used by DynaZIP to create temporary files.
			// Only has meaning if PathForTempFlag = true.
			_ZipEngine.TempPath = "";

			// Set to true to cause DynaZIP to inhibit the display of any
			// WARNING messages.  Warning messages are any non-fatal messages
			// that might be displayed by DynaZIP.  Fatal error messages and 
			// Multi-Volume related disk prompt messages will continue to 
			// display even if this flag is set to true.
			_ZipEngine.QuietFlag = true;

			// Set to true to cause DynaZIP to start at a specified folder
			// and recurse through all matching subfolders of that base
			// folder, while scanning a drive for matching files to zip.
			_ZipEngine.RecurseFlag = false;

			// Set to True to create Large Zip Files Greater than 4 GB
			_ZipEngine.LargeZIPFilesFlag = false;


			// Set to a Byte Array to be zipped. 
			// This options only has meaning for ZIP_MEMTOFILE action
			// and can not be used at the same time as the the ZIPString property.
			// The item name will be set to the string contained in the 
			// ItemList property
			_ZipEngine.MemoryBlock = null;
			// Contains the size of the MemoryBlock in bytes to be zipped
			// Only has meaning for the ZIP_MEMTOFILE action
			_ZipEngine.MemoryBlockSize = 0;

			// Setup the Zip SubOptions according to selections on the 
			// main form
			_ZipEngine.ZipSubOptions = CDZipSNET.ZIPSUBOPTION.ZSO_NONE;

			// When set, causes DynaZIP to interpret the ItemList
			// as a relative starting point for scanning the drive for matching
			// files.  All items will be zipped so that their path and filenames
			// are relative to the starting point.
			// _ZipEngine.ZipSubOptions |= CDZipSNET.ZIPSUBOPTION.ZSO_RELATIVEPATHFLAG;
		
			// allow the MinorCancel event to respond to cancel requests
			// _ZipEngine.ZipSubOptions |= CDZipSNET.ZIPSUBOPTION.ZSO_MINORCANCEL;

			// display an external progress window via DynaZIP .NET
			//	_ZipEngine.ZipSubOptions |= CDZipSNET.ZIPSUBOPTION.ZSO_EXTERNALPROG;
			// Allow the user to cancel an extraction via the external progress display
			//	_ZipEngine.ZipSubOptions |= CDZipSNET.ZIPSUBOPTION.ZSO_EXTERNALPROGCANCEL;

			// Don't zip any items in the zip file that have long file names
			// _ZipEngine.ZipSubOptions |= CDZipSNET.ZIPSUBOPTION.ZSO_IGNORELONGNAMES;

			// perform a simple mangling of file names to become Old DOS 8.3 file names
			//	_ZipEngine.ZipSubOptions |= CDZipSNET.ZIPSUBOPTION.ZSO_MANGLELONGNAMES;
			
			// Don't zip any files that are locked by the operating system 
			// or an application including those locked by your application.
			//	_ZipEngine.ZipSubOptions |= CDZipSNET.ZIPSUBOPTION.ZSO_SKIPLOCKEDFILES;
			
			// CAUTION!!!  Only use this option if absolutely necessary
			// It forces all drives to behave as if they are removable types
			// _ZipEngine.ZipSubOptions |= CDZipSNET.ZIPSUBOPTION.ZSO_FORCEREMOVABLE;

			// Keep a log of the files as they are zipped
			//	_ZipEngine.ZipSubOptions |= CDZipSNET.ZIPSUBOPTION.ZSO_LOGZIPRESULTS; 
			
			// Only zip files that have their archived bit set
			//	_ZipEngine.ZipSubOptions |= CDZipSNET.ZIPSUBOPTION.ZSO_JUSTARCHIVED;
			
			// As files are zipped, reset their archived bit.  Only has
			// meaning if ZSO_JUSTARCHIVED is also set
			//	_ZipEngine.ZipSubOptions |= CDZipSNET.ZIPSUBOPTION.ZSO_RESETARCHIVED;
			
			// Force pure synchronous activity without releasing control to OS
			//	_ZipEngine.ZipSubOptions |= CDZipSNET.ZIPSUBOPTION.ZSO_ANSI_NAMES;
			
			// When Renaming is active, do it early
			//	_ZipEngine.ZipSubOptions |= CDZipSNET.ZIPSUBOPTION.ZSO_RENAME_EARLY;
			
			// Force pure synchronous activity without releasing control to OS
			//	_ZipEngine.ZipSubOptions |= CDZipSNET.ZIPSUBOPTION.ZSO_INHIBITPUMP;
			
			// Force pure synchronous activity without releasing control to OS
			//	_ZipEngine.ZipSubOptions |= CDZipSNET.ZIPSUBOPTION.ZSO_DONTCHECKNAMES;
			
			// ZipSubOptions2 settings
			//_ZipEngine.ZipSubOptions2 = CDZipSNET.ZIPSUBOPTION2.ZSO2_NONE;
			//_ZipEngine.ZipSubOptions2 |= CDZipSNET.ZIPSUBOPTION2.ZSO2_DELNORECURSE;
			
			//	_ZipEngine.ZipSubOptions2 |= CDZipSNET.ZIPSUBOPTION2.ZSO2_EXTRENAME;
			
			//_ZipEngine.MultiVolumeControl = CDZipSNET.MULTIVOLCONTROL.MV_NONE;
			//We are performing a multi-volume zip
			//	_ZipEngine.MultiVolumeControl |= CDZipSNET.MULTIVOLCONTROL.MV_USEMULTI;
			// Media will be formatted
			//_ZipEngine.MultiVolumeControl |= CDZipSNET.MULTIVOLCONTROL.MV_FORMAT;
			//_ZipEngine.MultiVolumeControl |= CDZipSNET.MULTIVOLCONTROL.MV_LOWDENSE;
			
			// Media will be wiped at root level
			//_ZipEngine.MultiVolumeControl |= CDZipSNET.MULTIVOLCONTROL.MV_WIPE;
			
			// Also delete all files in all subdirectories
			//	_ZipEngine.MultiVolumeControl |= CDZipSNET.MULTIVOLCONTROL.MV_SUBDIR;
			
			// Also delete any SYSTEM or HIDDEN files
			//	_ZipEngine.MultiVolumeControl |= CDZipSNET.MULTIVOLCONTROL.MV_SYSHIDE;
			
			// Central Directory structure will be placed at beginning of zip file
			//		_ZipEngine.MultiVolumeControl |= CDZipSNET.MULTIVOLCONTROL.MV_CDFIRST;
			
            // Central Directory structure will be placed at beginning of zip file
			//	_ZipEngine.MultiVolumeControl |= CDZipSNET.MULTIVOLCONTROL.MV_TOHD;
			//	_ZipEngine.MultiVolumeControl |= CDZipSNET.MULTIVOLCONTROL.MV_FSA64;
			
			// Set the File Size Array for the Multi-Volume to Hard Drive creation
			// if selected.
			//_ZipEngine.FileSizeArray = iFSA;
		
			// Setup the event procedures
			// You only need to handle the events your app requires
			// In this case, the test program handles all DynaZIP events

			// Set to true to cause DynaZIP to fire the Major Status Event
			//_ZipEngine.MajorStatusFlag = MajorStatus.Checked;
			// Set to true to cause DynaZIP to fire the Minor Status Event
			//_ZipEngine.MinorStatusFlag = MinorStatus.Checked;
			// Set to true to cause DynaZIP to fire the Message Callback Event
			// which allows an application to change any message DynaZIP might 
			// display.
			//_ZipEngine.MessageCallbackFlag = true;
			// Set to true to cause DynaZIP to fire the rename callback event
			// for each item to be zipped.  During the event, the application may
			// change the name of the item as it will appear in the zip file.
			// IMPORTANT
			// You should not rename items in a way that causes more than one item
			// at the same folder level to have a name that matches another item at
			// the same level.
			// You may also change some of the items attributes such as 
			// File Attributes, Date, Time
			// This event is also useful for logging zip activity.
			//_ZipEngine.RenameCallbackFlag = AllowRename.Checked;
			// Associate event handlers with this instance of the DynaZIP assembly
			/*
			_ZipEngine.ZipMajorStatus += new CDZipSNET.OnZipMajorStatus(this.ZipMajorStatus_event);
			_ZipEngine.ZipMinorStatus += new CDZipSNET.OnZipMinorStatus(this.ZipMinorStatus_event);
			_ZipEngine.ZipRenameCallback += new CDZipSNET.OnZipRenameCallback(this.ZipRenameCallback_event);
			_ZipEngine.ZipRenameCallbackEX += new CDZipSNET.OnZipRenameCallbackEX(this.ZipRenameCallbackEX_event);
			_ZipEngine.ZipMessageCallback += new CDZipSNET.OnZipMessageCallback(this.ZipMessageCallback_event);
			*/
			// The level of compression to apply
			_ZipEngine.CompressionFactor = 9;
		}
		#endregion

		#region Event Handlers
		#endregion
	}
}
