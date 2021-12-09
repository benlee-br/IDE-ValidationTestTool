using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// IZipEngine Class Summary
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:BL</item>
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
	///			<item name="vssfile">$Workfile: IZipEngine.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Common/IZipEngine.cs $</item>
	///			<item name="vssrevision">$Revision: 2 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Thouser $</item>
	///			<item name="vssdate">$Date: 7/15/09 11:47a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public interface IZipEngine
	{
		#region Accessors
		/// <summary>
		/// Gets error string.
		/// </summary>
		string ErrorString { get; }
		#endregion

		#region Methods
		#region decompress
		/// <summary>
		/// Exrtract all files from a zip file
		/// </summary>
		/// <param name="zipFilePath">zip file</param>
		/// <param name="outputFolderPath">target folder, which need not exist.</param>
		/// <returns>true on success, false on failure.</returns>
		bool ExtractAllFromZipFile(string zipFilePath, string outputFolderPath);
		/// <summary>
		/// Exrtract all files from a zip file
		/// </summary>
		/// <param name="zipFilePath">zip file</param>
		/// <param name="outputFolderPath">target folder, which need not exist.</param>
		/// <returns>true on success, false on failure.</returns>
		(bool, string[]) ExtractFileListFromZipFile(string zipFilePath, string outputFolderPath);
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
		bool DecompressMemoryStream(MemoryStream outputMemoryStream, MemoryStream inputMemoryStream);
		/// <summary>
		/// Do decompression. We require that there be exactly 1 data file in the zip file.
		/// We determine the name of that file, so you don't have to know this in advance.
		/// </summary>
		/// <param name="fullPathZipFileName">Full path name of zip file to decompress.
		/// Example: ""E:\Test Data\2ColorFamTexRed.zip"</param>
		/// <param name="fullPathDest">Full path to the destination directory for the contained file.
		/// Example: "E:\Test Data"</param>
		/// <param name="deCompressedFileName">Decompressed filename.</param>
		/// <returns>Results.IsSuccess() true if successfully decompresses the file, else false.</returns>
		Results DecompressFile(string fullPathZipFileName, string fullPathDest, ref string deCompressedFileName);

		///// <summary>
		///// We will assume that there is only a single file in the zip file, 
		///// so we take this first file name out.
		///// </summary>
		///// <param name="fullPathZipFileName">Full path to zip file containing compressed file.</param>
		///// <returns></returns>
		//string ExtractSourceFileName(string fullPathZipFileName);
		#endregion

		#region Compress

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
		bool CompressMemoryStream(MemoryStream outputMemoryStream, MemoryStream inputMemoryStream);
		/// <summary>
		/// Compresses a file. We allow for exactly one file to be compressed into a zip file.
		/// File format is Winzip.
		/// </summary>
		/// <param name="fullPathZipFileName">Full path for target zip file to be created.
		/// Example: "E:\Test Data\2ColorFamTexRed.zip"</param>
		/// <param name="fullPathSourceFile">Full path for single source file to be compressed.
		/// Example: "E:\Test Data\2ColorFamTexRed.opd"</param>
		/// <param name="compressionType">type of compression.  Zip engine is not obligated to use this.</param>
		/// <returns>true if compression occurs, else false.</returns>
		bool CompressFile(string fullPathZipFileName, string fullPathSourceFile, CompressionType compressionType);

		/// <summary>
		/// Creates zip file from provided files.
		/// </summary>
		/// <param name="fullPathFileNames">File names with full path to be zipped</param>
		/// <param name="fullPathZipFileName">Zip file name to be created</param>
		/// <param name="allowRecursionMode">Set to false in normal mode. 
		/// If set to true, all files in the specified folder will be zipped recursively and 
		/// their relative path stored as well in the zip file</param>
		/// <returns>true if compression occurs, else false.</returns>		 
		bool CreateZipFile(string[] fullPathFileNames, string fullPathZipFileName, bool allowRecursionMode);
		#endregion
		#endregion

	}
	/// <summary></summary>
	public enum CompressionType
	{
		/// <summary></summary>
		HighestSlowest,
		/// <summary></summary>
		LowestFastest
	}
	/// <summary>
	/// 
	/// </summary>
	public class DynaZip : IZipEngine
	{
		private DeCompress deCompressor = new DeCompress();
		private Compress compressor = new Compress();
		#region IZipEngine Members

		/// <summary>
		/// 
		/// </summary>
		public string ErrorString
		{
			get
			{
				if (string.IsNullOrEmpty(deCompressor.ErrorString) &&
					 !string.IsNullOrEmpty(compressor.ErrorString))
					return compressor.ErrorString;
				else if (!string.IsNullOrEmpty(deCompressor.ErrorString) &&
					 string.IsNullOrEmpty(compressor.ErrorString))
					return deCompressor.ErrorString;
				else return string.Empty;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fullPathFileNames"></param>
		/// <param name="fullPathZipFileName"></param>
		/// <param name="allowRecursionMode"></param>
		/// <returns></returns>
		public bool CreateZipFile(string[] fullPathFileNames, string fullPathZipFileName, bool allowRecursionMode)
		{
			return compressor.CreateZipFile(fullPathFileNames, fullPathZipFileName, allowRecursionMode);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="zipFilePath"></param>
		/// <param name="outputFolderPath"></param>
		/// <returns></returns>
		public bool ExtractAllFromZipFile(string zipFilePath, string outputFolderPath)
		{
			return deCompressor.ExtractAllFromZipFile(zipFilePath, outputFolderPath);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="zipFilePath"></param>
		/// <param name="outputFolderPath"></param>
		/// <returns></returns>
		public (bool, string[]) ExtractFileListFromZipFile(string zipFilePath, string outputFolderPath)
        {
			return deCompressor.ExtractFileListFromZipFile(zipFilePath, outputFolderPath);
		}
		/// <summary>
		/// We will assume that there is only a single file in the zip file, 
		/// so we take this first file name out.
		/// </summary>
		/// <param name="fullPathZipFileName">Full path to zip file containing compressed file.</param>
		/// <returns></returns>
		public string ExtractSourceFileName(string fullPathZipFileName)
		{
			return deCompressor.ExtractSourceFileName(fullPathZipFileName);
		}

		#region IZipEngine interface implementation
		/// <summary>
		/// 
		/// </summary>
		/// <param name="outputMemoryStream"></param>
		/// <param name="inputMemoryStream"></param>
		/// <returns></returns>
		public bool DecompressMemoryStream(MemoryStream outputMemoryStream, MemoryStream inputMemoryStream)
		{
			return deCompressor.DecompressMemoryStream(outputMemoryStream, inputMemoryStream);
		}
		/// <summary>
		/// DeCompresses/Decrpyts given file, saves with same name.
		/// </summary>
		/// <param name="fullPathSourcefileName">Source file.</param>
		/// <param name="targetFolder">Target folder</param>
		/// <param name="deCompressedFileName">Decompressed filename.</param>
		/// <returns></returns>
		public Results DecompressFile(string fullPathSourcefileName, string targetFolder, ref string deCompressedFileName)
		{
			Results result = new Results();

			deCompressedFileName = deCompressor.ExtractSourceFileName(fullPathSourcefileName);
			bool bRet = deCompressor.DecompressFile(fullPathSourcefileName, targetFolder);
			if (!bRet)
				result.Message = deCompressor.ErrorString;
			else result.SetSuccess();

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="outputMemoryStream"></param>
		/// <param name="inputMemoryStream"></param>
		/// <returns></returns>
		public bool CompressMemoryStream(MemoryStream outputMemoryStream, MemoryStream inputMemoryStream)
		{
			return compressor.CompressMemoryStream(outputMemoryStream, inputMemoryStream);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fullPathZipFileName"></param>
		/// <param name="fullPathSourceFile"></param>
		/// <param name="compressionType"></param>
		/// <returns></returns>
		public bool CompressFile(string fullPathZipFileName, string fullPathSourceFile, CompressionType compressionType)
		{
			return compressor.CompressFile(fullPathZipFileName, fullPathSourceFile);
		}
		#endregion
		#endregion
	}
}
