using System;
using System.Diagnostics;
using System.IO;

using Xceed.Compression;
using Xceed.FileSystem;
using Xceed.Zip;


namespace BioRad.Common
{
	/// <summary>
	/// 
	/// </summary>
	public class XceedZip : IZipEngine
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
		#endregion

		#region Constants
		private const string c_EncryptPWD =
			 "SecureCompressDecompressKeyiQ5V4Files!!##$$";
		#endregion

		#region Members
		private string m_ErrorMessage = string.Empty;
		private static int m_RetryCounter = 0;
		private string m_FileMask = "*.*";
        private string m_EncryptPWD = c_EncryptPWD;
		#endregion
		#region Constructor
		/// <summary>
		/// 
		/// </summary>
        public XceedZip(string encryptPWD)
		{
			Xceed.Zip.Licenser.LicenseKey = "ZIN37-GEW4L-YAKWY-N8JA";
			Xceed.FileSystem.Licenser.LicenseKey = "ZIN37-GEW4L-YAKWY-N8JA";
			Xceed.Compression.Licenser.LicenseKey = "ZRT11-LEW6L-KBRTP-AN6A";
			//Xceed.Compression.Licenser.LicenseKey = "SCO11-GMWZT-TT7KY-K82A";

            m_EncryptPWD = c_EncryptPWD;
            //if (ApplicationStateData.GetInstance.IsRegulatory && !string.IsNullOrEmpty(encryptPWD))// (US458 SE Epic) US490 - TA876
            //{
            //    m_EncryptPWD = encryptPWD;
            //}
		}
		#endregion
		#region IZipEngine Members
		/// <summary>
		/// 
		/// </summary>
		public string ErrorString
		{
			get { return m_ErrorMessage; }
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="zipFilePath"></param>
		/// <param name="outputFolderPath"></param>
		/// <returns></returns>
        public (bool, string[]) ExtractFileListFromZipFile(string zipFilePath, string outputFolderPath)
		{
			bool rc = false;
            ZipArchive archive = null;
            bool beginUpdate = false;
			string[] unarchivedList = new string[0];
			try
			{
				DiskFile zipFile = new DiskFile(zipFilePath);

				if (!zipFile.Exists)
				{
					m_ErrorMessage = "The specified zip file does not exist.";
					return (rc, unarchivedList);
				}
                archive = new ZipArchive(zipFile);
                archive.DefaultDecryptionPassword = m_EncryptPWD;

                archive.BeginUpdate();
                beginUpdate = true;

                AbstractFolder destinationFolder = new DiskFolder(outputFolderPath);

				unarchivedList = new string[archive.GetItems(true).Length];
				int count = 0;

				foreach (FileSystemItem item in archive.GetItems(true))
                {
                    item.CopyTo(destinationFolder, true);
					unarchivedList[count] = item.FullName;
					count++;
				}
			}
			catch (Exception ex)
			{
				rc = false;
				m_ErrorMessage = ex.Message;
				return (rc, unarchivedList);
			}
            finally
            {
                if (archive != null && beginUpdate)
                    archive.EndUpdate();
            }
			rc = true;
			return (rc, unarchivedList);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="zipFilePath"></param>
		/// <param name="outputFolderPath"></param>
		/// <returns></returns>
		public bool ExtractAllFromZipFile(string zipFilePath, string outputFolderPath)
		{
			bool rc = false;
			ZipArchive archive = null;
			bool beginUpdate = false;

			try
			{
				DiskFile zipFile = new DiskFile(zipFilePath);

				if (!zipFile.Exists)
				{
					m_ErrorMessage = "The specified zip file does not exist.";
					return rc;
				}
				archive = new ZipArchive(zipFile);
				archive.DefaultDecryptionPassword = m_EncryptPWD;

				archive.BeginUpdate();
				beginUpdate = true;

				AbstractFolder destinationFolder = new DiskFolder(outputFolderPath);
				foreach (FileSystemItem item in archive.GetItems(true))
				{
					item.CopyTo(destinationFolder, true);
				}
			}
			catch (Exception ex)
			{
				rc = false;
				m_ErrorMessage = ex.Message;
				return rc;
			}
			finally
			{
				if (archive != null && beginUpdate)
					archive.EndUpdate();
			}
			rc = true;
			return rc;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="outputMemoryStream"></param>
		/// <param name="inputMemoryStream"></param>
		/// <returns></returns>
		public bool DecompressMemoryStream(System.IO.MemoryStream outputMemoryStream, System.IO.MemoryStream inputMemoryStream)
		{
			//throw new Exception("The method or operation is not implemented.");

			bool rc = false;
			try
			{
				// Creates a CompressedStream around the source FileStream so that all
				// data read from that stream will be decompressed.
				CompressedStream compStream = new CompressedStream(inputMemoryStream);

				// Copy the stream
				StreamCopy(compStream, outputMemoryStream);
			}
			catch (Exception ex)
			{
				rc = false;
				m_ErrorMessage = ex.Message;
				return rc;
			}

			rc = true;
			return rc;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="files"></param>
		/// <param name="fullPathZipFileName"></param>
		/// <param name="allowRecursionMode"></param>
		/// <returns></returns>
		public bool CreateZipFile(string[] files, string fullPathZipFileName, bool allowRecursionMode)
		{
			bool result = false;
            ZipArchive archive = null;
            bool beginUpdate = false;
            try
            {
                DiskFile zipFile = new DiskFile(fullPathZipFileName);

                // Check if the file exists
                if (!zipFile.Exists)
                {
                    zipFile.Create();
                }

                // Create a ZipArchive object to access the zipfile.
                archive = new ZipArchive(zipFile);
                archive.BeginUpdate();
                beginUpdate = true;
                archive.DefaultEncryptionPassword = ""; //Test Track Defect 11

                foreach (string f in files)
                {
                    if (!File.Exists(f))
                    {
                        continue;
                    }

                    AbstractFile abstractFile = new DiskFile(f);
                    abstractFile.CopyTo(archive, true);
                }
            }
            catch (Exception ex)
            {
                result = false;
                m_ErrorMessage = ex.Message;
                return result;
            }
            finally
            {
                if (archive != null && beginUpdate)
                    archive.EndUpdate();
            }

			result = true;
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fullPathZipFileName"></param>
		/// <param name="outputFolderPath"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public Results DecompressFile(string fullPathZipFileName, string outputFolderPath, ref string fileName)
		{
			Results result = new Results();
			try
			{
				// Create a DiskFile object for the specified zip filename
				DiskFile zipFile = new DiskFile(fullPathZipFileName);

				if (!zipFile.Exists)
				{
					result.Message = "The specified zip file does not exist.";
					return result;
				}
				// Create a ZipArchive object to access the zipfile.
				ZipArchive zip = new ZipArchive(zipFile);
                zip.DefaultDecryptionPassword = m_EncryptPWD;

				// Create a DiskFolder object for the destination folder
				DiskFolder destinationFolder = new DiskFolder(outputFolderPath);

				// Create a FileSystemEvents object for handling the ItemProgression event
				FileSystemEvents events = new FileSystemEvents();
				events.ItemProgression += new ItemProgressionEventHandler(OnItemProgression);
				events.ItemException += new ItemExceptionEventHandler(OnItemException);

				// Copy the contents of the zip to the destination folder.
				AbstractFile[] files = zip.GetFiles(false, "*.*");
				if (files.Length > 1)
				{
					result.Message = "More than one file.";
					return result;
				}
				else
				{
					zip.CopyFilesTo(events, "Extracting", destinationFolder, true, true, "*.*");
					if (files.Length == 1)
						fileName = string.Copy(files[0].Name);
					else
						fileName = zip.ZipFile.FullName;

					result.SetSuccess();
				}
			}
			catch (Exception ex)
			{
				result.Message = ex.Message;
			}
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="outputMemoryStream"></param>
		/// <param name="inputMemoryStream"></param>
		/// <returns></returns>
		public bool CompressMemoryStream(System.IO.MemoryStream outputMemoryStream, System.IO.MemoryStream inputMemoryStream)
		{
			throw new Exception("The method or operation is not implemented.");
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
            bool test = false;
            if (test)
                return CompressFileEx(fullPathZipFileName, fullPathZipFileName, compressionType);

			bool result = false;
			try
			{
				string sourceFolder = Path.GetDirectoryName(fullPathSourceFile);
				m_FileMask = Path.GetFileName(fullPathSourceFile);

				if (sourceFolder.Length == 0)
				{
					//result.Message = "You must specify a source folder from which files will be added to the zip file.";
					return result;
				}
				// Create a DiskFile object for the specified zip filename

				string destFolder = Path.GetDirectoryName(fullPathZipFileName);

				if (!Directory.Exists(destFolder))
					Directory.CreateDirectory(destFolder);
				DiskFile zipFile = new DiskFile(fullPathZipFileName);

				// Check if the file exists

				if (!zipFile.Exists)
				{
					Console.WriteLine("Creating a new zip file \"{0}\"...", fullPathZipFileName);
					zipFile.Create();
				}
				else
				{
					Console.WriteLine("Updating existing zip file \"{0}\"...", fullPathZipFileName);
				}

				Console.WriteLine();

				// Create a ZipArchive object to access the zipfile.

				ZipArchive zip = new ZipArchive(zipFile);

				zip.DefaultCompressionMethod = CompressionMethod.Deflated;
                zip.DefaultEncryptionPassword = m_EncryptPWD;
				zip.DefaultEncryptionMethod = EncryptionMethod.Compatible;
				switch (compressionType)
				{
					case CompressionType.HighestSlowest:
						zip.DefaultCompressionLevel = CompressionLevel.Highest;
						break;
					case CompressionType.LowestFastest:
						zip.DefaultCompressionLevel = CompressionLevel.Lowest;
						break;
					default:
						zip.DefaultCompressionLevel = CompressionLevel.Highest;
						Debug.Assert(false, "add a case.");
						break;
				}
				zip.AllowSpanning = true;

				// Create a DiskFolder object for the source folder

				DiskFolder source = new DiskFolder(sourceFolder);

				// Create a ZipEvents object for handling the ItemProgression event

				ZipEvents events = new ZipEvents();

				// Subscribe to the ItemProgression event and DiskRequired event

				events.ItemProgression += new ItemProgressionEventHandler(OnItemProgression);
				events.DiskRequired += new DiskRequiredEventHandler(OnDiskRequired);

				// Copy the contents of the zip to the destination folder.
				// fixed defect #10346 c:\ recursive throw unauthorized access exception
				source.CopyFilesTo(events, "Zipping", zip, false, true, m_FileMask);
			}
			catch (Exception ex)
			{
				m_ErrorMessage = ex.Message;
				result = false;
				return result;
			}

			result = true;
			return result;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullPathZipFileName"></param>
        /// <param name="fullPathSourceFile"></param>
        /// <param name="compressionType"></param>
        /// <returns></returns>
        public bool CompressFileEx(string fullPathZipFileName, string fullPathSourceFile, CompressionType compressionType)
        {
            bool result = false;
            try
            {
                AbstractFile zipFile = new DiskFolder(Path.GetDirectoryName(fullPathZipFileName)).GetFile(@"temp\new.zip");
                if (zipFile.Exists)
                {
                    zipFile.Delete();
                }

                ZipArchive archive = new ZipArchive(zipFile);
                AbstractFile abstractFile = new DiskFile(fullPathSourceFile);
                abstractFile.CopyTo(archive, true);

                if (File.Exists(fullPathZipFileName))
                    File.Delete(fullPathZipFileName);
                File.Move(zipFile.FullName, fullPathZipFileName);

                AbstractFolder folder = zipFile.ParentFolder;
                if (folder.Exists)
                    folder.Delete();
            }
            catch (Exception ex)
            {
                m_ErrorMessage = ex.Message;
                result = false;
                return result;
            }

            result = true;
            return result;
        }
		#endregion
		#region Zip events handlers
		/// <summary>
		/// Copies the contents of <paramref name="sourceStream"/> into <paramref name="destStream"/>.
		/// </summary>
		/// <param name="sourceStream">Input stream.</param>
		/// <param name="destStream">Output stream.</param>
		/// <remarks>
		/// When done, this function closes both streams.
		/// </remarks>
		private static void StreamCopy(Stream sourceStream, Stream destStream)
		{
			try
			{
				int bytesRead;
				byte[] buffer = new byte[32768];

				while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
					destStream.Write(buffer, 0, bytesRead);
			}
			finally
			{
				sourceStream.Close();
				destStream.Close();
			}
		}

		/// <summary>
		/// Handles the ItemProgression event.
		/// </summary>
		/// <param name="sender">The object that raised this event.</param>
		/// <param name="e">Data related to this event.</param>
		private static void OnItemProgression(object sender, ItemProgressionEventArgs e)
		{
			if ((e.CurrentItem != null) && (e.AllItems.Percent < 100))
			{
				m_RetryCounter = 0;
				Console.WriteLine("{0} {1}...", (string)e.UserData, e.CurrentItem.FullName);
			}
		}

		private static void OnDiskRequired(object sender, DiskRequiredEventArgs e)
		{
			if (e.Action == DiskRequiredAction.Fail)
			{
				Console.WriteLine("Please insert a disk and press <Enter>.");
				Console.WriteLine("Press <Ctrl-C> to cancel the operation.");
				Console.ReadLine();

				e.Action = DiskRequiredAction.Continue;
			}
		}
		private static void OnItemException(object sender, ItemExceptionEventArgs e)
		{
			if (e.CurrentItem is ZippedFile)
			{
				if (e.Exception is InvalidDecryptionPasswordException)
				{

					if (m_RetryCounter < 3)
					{
						Console.Write("Enter the password for the file {0}: ", e.CurrentItem.Name);

						((ZipArchive)e.CurrentItem.RootFolder).DefaultDecryptionPassword = Console.ReadLine();
						e.Action = ItemExceptionAction.Retry;
						m_RetryCounter++;
					}
					else
					{
						Console.WriteLine("{0} has been skipped due to an invalid password", e.CurrentItem.Name);
						e.Action = ItemExceptionAction.Ignore;
					}
				}
			}
		}
		#endregion
	}
}
