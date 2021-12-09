using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using BioRad.Common.DiagnosticsLogger;
using BioRad.Common.Xml;
using BioRad.Common.Utilities;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>This class contains utility methods commonly needed in various file I/O
	/// tasks.  They exist here to prevent code duplication in many classes which must
	/// persist their state to a file.</summary>
	/// <remarks>This class is stateless therefore methods are static.</remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Lisa von Schlegell, Pramod Walse</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">924</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: FileUtilities.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Common/FileUtilities.cs $</item>
	///			<item name="vssrevision">$Revision: 87 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Thouser $</item>
	///			<item name="vssdate">$Date: 7/20/10 10:40a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class FileUtilities
	{
        private static object sync = new object();
		#region Contained Classes
		/// <summary>
		/// Configuration type represents data in a converted INI file.
		/// </summary>
		public class Configuration
		{
			/// <summary>
			/// Each section has a name and a list of settings
			/// </summary>
			public class Section
			{
				private string m_Name;
				private Setting[] m_Settings;
				/// <summary>
				/// Section name
				/// </summary>
				public string Name { get { return m_Name; } set { m_Name = value; } }
				/// <summary>
				/// Section name/value pairs
				/// </summary>
				public Setting[] Settings { get { return m_Settings; } set { m_Settings = value; } }
			}

			/// <summary>
			/// A name/value pair in a configuration section
			/// </summary>
			public class Setting
			{
				private string m_Name;
				private string m_Value;

				/// <summary>
				/// Setting name
				/// </summary>
				public string Name { get { return m_Name; } set { m_Name = value; } }
				/// <summary>
				/// Setting value
				/// </summary>
				public string Value { get { return m_Value; } set { m_Value = value; } }
			}
			private Section[] m_Sections;
			/// <summary>
			/// All sections in the configuration file
			/// </summary>
			public Section[] Sections { get { return m_Sections; } set { m_Sections = value; } }
		}

		/// <summary>
		/// Supplies Xml mapping information for the Configuration type.
		/// </summary>
		public class XmlConfigurationMapping
		{
			#region Methods
			/// <summary>
			/// Provides mapping for CustomControlConfig type.
			/// </summary>
			/// <returns>The type mapping.</returns>
			public virtual XmlToTypeMapping GetConfigurationMapping()
			{
				XmlToTypeMapping mapping = new XmlToTypeMapping(typeof(Configuration),
					c_MainElement);
				mapping.AddCollectionMapping(c_SectionsElement, "Sections", c_SectionElement,
					typeof(Configuration.Section));
				mapping.AddNestedTypeMapping(GetSectionMapping());
				return mapping;
			}

			/// <summary>
			/// Provides mapping for configuration sections
			/// </summary>
			/// <returns>The type mapping.</returns>
			public virtual XmlToTypeMapping GetSectionMapping()
			{
				XmlToTypeMapping mapping = new XmlToTypeMapping(typeof(Configuration.Section),
					c_SectionElement);
				mapping.AddCollectionMapping(c_SectionElementSettings, "Settings",
					c_SectionElementSetting, typeof(Configuration.Setting));
				mapping.AddAttributeMapping(c_SectionElementName, "Name", typeof(string));
				mapping.AddNestedTypeMapping(GetSettingMapping());
				return mapping;
			}

			/// <summary>
			/// Provides mapping for configuration section settings.
			/// </summary>
			/// <returns>The type mapping.</returns>
			public virtual XmlToTypeMapping GetSettingMapping()
			{
				XmlToTypeMapping mapping = new XmlToTypeMapping(typeof(Configuration.Setting),
					c_SectionElementSetting);
				mapping.AddAttributeMapping(c_SectionElementSettingName, "Name", typeof(string));
				mapping.AddAttributeMapping(c_SectionElementSettingValue, "Value", typeof(string));
				return mapping;
			}
			#endregion
		}

		#endregion

		#region Constants
		/// <summary>
		/// Initial size of the buffer used when calling the Win32 API functions
		/// </summary>
		private const int c_InitialBufferSize = 4096;
		private const string c_MainElement = "configuration";
		private const string c_SectionsElement = "sections";
		private const string c_SectionElement = "section";
		private const string c_SectionElementName = "name";
		private const string c_SectionElementSettings = "settings";
		private const string c_SectionElementSetting = "setting";
		private const string c_SectionElementSettingName = "name";
		private const string c_SectionElementSettingValue = "value";
		#endregion

		#region Methods
		static int c_MaxFileNameLength = 260;
		//static int c_MaxFilePathLength = 248;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="zipFilePath"></param>
		/// <param name="outputFolderPath"></param>
		/// <returns></returns>
		public static (bool,string[]) ExtractFileListFromZipFile(string zipFilePath, string outputFolderPath)
		{
			if (string.IsNullOrEmpty(zipFilePath))
				throw new ArgumentNullException("zipFilePath");
			if (string.IsNullOrEmpty(outputFolderPath))
				throw new ArgumentNullException("outputFolderPath");

			if (!File.Exists(zipFilePath))
				throw new ArgumentException("File not found.");
			if (!Directory.Exists(outputFolderPath))
				throw new ArgumentException("Folder not found.");

			IZipEngine compress = null;
			(bool,string[]) ok ;
			try
			{
				string encryptPWD = null;
				//if (ApplicationStateData.GetInstance.IsRegulatory)// (US458 SE Epic) US490 - TA876
				//{
				//    //todo
				//    //encryptPWD = CurrentUser.Instance.User.Password;
				//}
				compress = new XceedZip(encryptPWD);
				ok = compress.ExtractFileListFromZipFile(zipFilePath, outputFolderPath);
			}
			finally
			{
				if (compress != null)
					compress = null;
			}
			return ok;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="zipFilePath"></param>
		/// <param name="outputFolderPath"></param>
		/// <returns></returns>
		public static bool ExtractAllFromZipFile(string zipFilePath, string outputFolderPath)
        {
            if (string.IsNullOrEmpty(zipFilePath))
                throw new ArgumentNullException("zipFilePath");
            if (string.IsNullOrEmpty(outputFolderPath))
                throw new ArgumentNullException("outputFolderPath");
            
            if (!File.Exists(zipFilePath))
                throw new ArgumentException("File not found.");
            if (!Directory.Exists(outputFolderPath))
                throw new ArgumentException("Folder not found.");

            IZipEngine compress = null;
            bool ok = false;
            try
            {
                string encryptPWD = null;
                //if (ApplicationStateData.GetInstance.IsRegulatory)// (US458 SE Epic) US490 - TA876
                //{
                //    //todo
                //    //encryptPWD = CurrentUser.Instance.User.Password;
                //}
                compress = new XceedZip(encryptPWD);
                ok = compress.ExtractAllFromZipFile(zipFilePath, outputFolderPath);
            }
            finally
            {
                if (compress != null)
                    compress = null;
            }
            return ok;
        }
        /// <summary>
        /// Zip folder and all of it's subfolders
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="destinationZipFileName"></param>
        /// <returns></returns>		
        public static bool ZipFolder(string sourceFolder, string destinationZipFileName)
        {
            if (string.IsNullOrEmpty(sourceFolder))
                throw new ArgumentNullException("sourceFolder");
            if (string.IsNullOrEmpty(destinationZipFileName))
                throw new ArgumentNullException("destinationZipFileName");

            if (!Directory.Exists(sourceFolder))
                throw new ArgumentException("Folder not found.");

            IZipEngine compress = null;
            bool ok = false;
            try
            {
                DirectoryInfo di = new DirectoryInfo(sourceFolder);
                if (di.Exists)
                {
                    FileInfo[] files = di.GetFiles();
                    string[] filelist = new string[files.Length];
                    for (int i = 0; i < files.Length; i++)
                    {
                        filelist[i] = files[i].FullName;
                    }

                    string encryptPWD = null;
                    //if (ApplicationStateData.GetInstance.IsRegulatory)// (US458 SE Epic) US490 - TA876
                    //{
                    //    //todo
                    //    //encryptPWD = CurrentUser.Instance.User.Password;

                    //}
                    compress = new XceedZip(encryptPWD);
                    ok = compress.CreateZipFile(filelist, destinationZipFileName, true);
                }
            }
            finally
            {
                if (compress != null)
                    compress = null;
            }
            return ok;
        }
		/// <summary>
		/// Compresses/Encrpyts given file, saves with same name.  Uses highest/slowest compression type.
		/// </summary>
		/// <param name="fullPathSourcefileName"></param>
		/// <returns></returns>
		public static bool CompressFile(string fullPathSourcefileName)
		{
            return CompressFile(fullPathSourcefileName, Path.GetDirectoryName(fullPathSourcefileName), CompressionType.HighestSlowest);
		}
      
        // TFS 4229 deleted public static bool CompressFilePathCheck(string fullPathSourcefileName)

		/// <summary>
		/// Compresses/Encrpyts given file, saves with same name.
		/// </summary>
		/// <param name="fullPathSourcefileName"></param>
		/// <param name="compressionType">type of compression.</param>
		/// <returns></returns>
		public static bool CompressFile(string fullPathSourcefileName, CompressionType compressionType)
		{
            return CompressFile(fullPathSourcefileName, Path.GetDirectoryName(fullPathSourcefileName), compressionType);
		}
        /// <summary>
        /// Compresses/Encrpyts given file, saves with same name in target folder.
        /// </summary>
        /// <param name="fullPathSourcefileName">Full path name of source</param>
        /// <param name="targetFolder">Folder name only for target location.</param>
        /// <param name="compressionType"></param>
        /// <returns></returns>
        private static bool CompressFile(string fullPathSourcefileName, string targetFolder, CompressionType compressionType)
        {
            lock (sync)// thread safe
            {
                if (!File.Exists(fullPathSourcefileName))
                    throw new ArgumentException("fullPathSourcefileName not found");
                if (!Directory.Exists(targetFolder))
                    throw new ArgumentException("targetFolder not found");

                string tempDirectory = string.Empty;
                IZipEngine compress = null;
                try
                {
                    if (IsCompressedFile(fullPathSourcefileName))
                    {
                        return true;
                    }

                    // Compress file to new directory always then copy if success.
                    tempDirectory = Path.Combine(Path.GetDirectoryName(fullPathSourcefileName), Guid.NewGuid().ToString());
                    if (!Directory.Exists(tempDirectory))
                        Directory.CreateDirectory(tempDirectory);
                    
                    string defaultCompressCoreFileName = Path.Combine(tempDirectory, Path.GetFileName(fullPathSourcefileName));

                    File.Move(fullPathSourcefileName, defaultCompressCoreFileName);

                    string compressedFileName = Path.Combine(targetFolder, Path.GetFileName(fullPathSourcefileName));
                    if (File.Exists(compressedFileName))
                    {
                        DeleteFile(compressedFileName);
                    }

                    bool isSuccess = false;
                    string encryptPWD = null;
                    //if (ApplicationStateData.GetInstance.IsRegulatory)// (US458 SE Epic) US490 - TA876
                    //{
                    //    //todo
                    //    //encryptPWD = CurrentUser.Instance.User.Password;

                    //}
                    compress = new XceedZip(encryptPWD);

                    // Compress it
                    isSuccess = compress.CompressFile(compressedFileName, defaultCompressCoreFileName, compressionType);
                    if (isSuccess)
                    {
                        DeleteFile(defaultCompressCoreFileName);
                    }
                    else
                        File.Move(defaultCompressCoreFileName, fullPathSourcefileName);
                    return isSuccess;
                }
                catch (Exception ex) //Logged
                {
                    // log the error
                    DiagnosticsLogService.GetService.GetDiagnosticsLog
                         (WellKnownLogName.FileEncryptCompress).SeriousError
                         (StringUtility.FormatString(Properties.Resources.EncryptCompressError_2,
                         fullPathSourcefileName, ex.Message), ex);

                    return false;
                }
                finally
                {
                    if (compress != null)
                        compress = null;
                    if (Directory.Exists(tempDirectory))
                        Directory.Delete(tempDirectory);
                }
            }
        }
        /// <summary>
        /// Is compressed file?
        /// </summary>
        /// <param name="file">Input file</param>
        /// <returns>true if compressed file.</returns>
        public static bool IsCompressedFile(string file)// TFS 4231
        {
            lock (sync)// thread safe
            {
                bool compressed = false;
                if (File.Exists(file))
                {
                    //Read in first few chars to determine status.
                    using (StreamReader streamReader = new StreamReader(file))
                    {
                        char[] chars = new char[2];
                        int count = streamReader.Read(chars, 0, 2);
                        if (count == 2)
                        {
                            compressed = (chars[0].Equals('P') && chars[1].Equals('K'));
                        }
                    }
                }
                return compressed;
            }
        }
        /// <summary>
        /// DeCompresses/Decrpyts given file, saves with same name in different folder.
        /// NOTE this method only works for CFX Manager because of ApplicationPath.TempDirectory. 
        /// </summary>
        /// <param name="fullPathSourcefileName">Input file.</param>
        /// <param name="deCompressedFileName">decompressed file location.</param>
        /// <returns>True if decompressed</returns>
        public static Results DeCompressFile(string fullPathSourcefileName, ref string deCompressedFileName)
        {
            try
            {
                return DeCompressFile(fullPathSourcefileName, ApplicationPath.TempDirectory, ref deCompressedFileName);
            }
            catch
            {// in case applicationPath.TempDirectory not defined. Ariel does not have applicationPath.TempDirectory
                return DeCompressFile(fullPathSourcefileName, Path.GetDirectoryName(fullPathSourcefileName), ref deCompressedFileName);
            }
        }
		/// <summary>
		/// DeCompresses/Decrpyts given file, saves with same name.
		/// </summary>
		/// <param name="fullPathSourcefileName">Source file.</param>
		/// <param name="targetFolder">Target folder</param>
		/// <param name="deCompressedFileName">Decompressed filename.</param>
		/// <returns></returns>
        public static Results DeCompressFile(string fullPathSourcefileName, string targetFolder, ref string deCompressedFileName)
        {
            lock (sync)
            {
                Results result = new Results();
                string tempSourcefileName = string.Empty;
                deCompressedFileName = string.Empty;
                IZipEngine deCompress = null;
                try
                {
                    if (!File.Exists(fullPathSourcefileName))
                        throw new ArgumentException("fullPathSourcefileName file not found");

                    if (string.IsNullOrEmpty(targetFolder) || !Directory.Exists(targetFolder))
                        Directory.CreateDirectory(targetFolder);

                    if (string.IsNullOrEmpty(targetFolder) || !Directory.Exists(targetFolder))
                        throw new ArgumentException("targetFolder folder not found");

                    if (fullPathSourcefileName.Length > c_MaxFileNameLength)
                        throw new ArgumentException("File name length exceed the maximum length 256 characters");

                    if (!IsCompressedFile(fullPathSourcefileName))
                    {
                        deCompressedFileName = fullPathSourcefileName;
                        result.Message = "Non-compression file";
                        result.SetSuccess();
                        return result;
                    }

                    tempSourcefileName = Path.Combine(targetFolder, Guid.NewGuid().ToString());
                    if (tempSourcefileName.Length > c_MaxFileNameLength)
                    {
                        result.Message = "File name length exceed the maximum length 256 characters";
                        return result;
                    }

                    string encryptPWD = null;
                    //if (ApplicationStateData.GetInstance.IsRegulatory)// (US458 SE Epic) US490 - TA876
                    //{
                    //    //todo
                    //    //encryptPWD = CurrentUser.Instance.User.Password;

                    //}
                    deCompress = new XceedZip(encryptPWD);

                    File.Copy(fullPathSourcefileName, tempSourcefileName);
                    if ((File.GetAttributes(tempSourcefileName) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        File.SetAttributes(tempSourcefileName, FileAttributes.Normal);
                    }
                    if (File.Exists(tempSourcefileName))
                    {
                        string extractedFileName = string.Empty;

                        result = deCompress.DecompressFile(tempSourcefileName, targetFolder, ref extractedFileName);

                        if (!string.IsNullOrEmpty(extractedFileName))
                        {
                            deCompressedFileName = Path.Combine(targetFolder, extractedFileName);
                        }
                    }

                    if (result.IsSuccess())
                    {
                        result.Message = "File decompress sucess";
                    }
                }
                catch (Exception ex)
                {
                    result.Message = ex.ToString();
                    result.SetFailed();
                }
                finally
                {
                    if (deCompress != null)
                        deCompress = null;
                    if (File.Exists(tempSourcefileName))
                        File.Delete(tempSourcefileName);
                }
                return result;
            }
        }
		
		/// <summary>
		/// Converts an INI file into an XML file.
		/// Output XML file has the following structure...
		///   <configuration>
		///       <section name="Main">
		///           <setting name="Timeout" value="90"/>
		///           <setting name="Mode" value="Live"/>
		///      </section>
		///   </configuration>
		/// </summary>
		/// <param name="iniFile">
		/// Full path file name of the INI file to convert</param>
		public static void Convert(string iniFile)
		{
			Convert(iniFile, "");
		}

		/// <summary>
		/// Converts an INI file into an XML file.
		/// Output XML file has the following structure...
		/// <configuration>
		///		<sections>
		///			<section name="Main">
		///				<settings>
		///					<setting name="Timeout" value="90"/>
		///					<setting name="Mode" value="Live"/>
		///				</settings>
		///			</section>
		///		</sections>
		/// </configuration>
		/// 
		/// </summary>
		/// <param name="iniFile">
		/// Full path file name of the INI file to convert</param>
		/// <param name="xmlFile">
		/// Full path file name of the XML file that is created</param>
		public static void Convert(string iniFile, string xmlFile)
		{
			char[] charEquals = { '=' };
			string lpSections;
			int nSize;
			int nMaxSize;
			string strSection;
			int intSection;
			int intNameValue;
			string strName;
			string strValue;
			string strNameValue;
			string lpNameValues;
			byte[] str = new byte[1];
			XmlTextWriter xw = null;

			try
			{
				if (!Path.HasExtension(iniFile))
				{
					Path.ChangeExtension(iniFile, ".ini");
				}
				// Ini file path must be rooted to read successfully
				iniFile = GetPathForFile(iniFile);
				if (xmlFile.Length == 0)
				{
					xmlFile = Path.Combine(Path.GetDirectoryName(iniFile),
						Path.ChangeExtension(iniFile, ".xml"));
				}
				// Get all sections names
				// Making sure allocate enough space for data returned
				for (nMaxSize = c_InitialBufferSize / 2,
					nSize = nMaxSize;
					nSize != 0 && nSize >= (nMaxSize - 2);
					nMaxSize *= 2)
				{
					str = new byte[nMaxSize];
					nSize = IniFile.GetPrivateProfileSectionNames(
						str, nMaxSize, iniFile);
				}

				// convert the byte array into a .NET string
				lpSections = Encoding.ASCII.GetString(str);

				// Use this for Unicode
				// lpSections = Encoding.Unicode.GetString( str );

				// Create XML File
				xw = new XmlTextWriter(xmlFile, Encoding.UTF8);
				xw.Formatting = Formatting.Indented;

				// Write the opening xml
				xw.WriteStartDocument();
				// Text is deliberately not localized here
				xw.WriteComment(String.Format("Converted from INI file: {0}.", Path.GetFileName(iniFile)));
				xw.WriteStartElement(c_MainElement);
				xw.WriteStartElement(c_SectionsElement);

				// Loop through each section
				char[] charNull = { '\0' };
				for (intSection = 0,
					strSection = GetToken(lpSections, charNull, intSection);
					strSection.Length > 0;
					strSection = GetToken(lpSections, charNull, ++intSection))
				{
					// Write a Node for the Section
					xw.WriteStartElement(c_SectionElement);
					xw.WriteAttributeString(c_SectionElementName, strSection);

					// Get all values in this section, making sure to allocate 
					// enough space
					for (nMaxSize = c_InitialBufferSize,
						nSize = nMaxSize;
						nSize != 0 && nSize >= (nMaxSize - 2);
						nMaxSize *= 2)
					{
						str = new Byte[nMaxSize];
						nSize = IniFile.GetPrivateProfileSection(
							strSection, str, nMaxSize, iniFile);
					}

					// convert the byte array into a .NET string
					lpNameValues = Encoding.ASCII.GetString(str);

					// Use this for Unicode
					// lpNameValues = Encoding.Unicode.GetString( str );

					// Loop through each Name/Value pair
					xw.WriteStartElement(c_SectionElementSettings);
					for (intNameValue = 0,
						strNameValue = GetToken(lpNameValues, charNull, intNameValue);
						strNameValue.Length > 0;
						strNameValue = GetToken(lpNameValues, charNull, ++intNameValue))
					{
						// Get the name and value from the entire null separated string 
						// of name/value pairs. Also escape out the special characters, 
						// (ie. &"<> )
						strName = GetToken(strNameValue, charEquals, 0);
						strValue = strNameValue.Substring(strName.Length + 1);

						// Write the XML Name/Value Node to the xml file
						xw.WriteStartElement(c_SectionElementSetting);
						xw.WriteAttributeString(c_SectionElementSettingName, strName);
						xw.WriteAttributeString(c_SectionElementSettingValue, strValue);
						xw.WriteEndElement();
					}
					xw.WriteEndElement(); // settings
					// Close the section node
					xw.WriteEndElement(); // section
				}

				// Thats it
				xw.WriteEndElement(); // sections
				xw.WriteEndElement(); // configuration
				xw.WriteEndDocument();
			}
			finally
			{
				if (xw != null)
				{
					xw.Close();
				}
			}
		} // Convert

		/// <summary>
		/// Get full path for the given file name. If fileName includes a
		/// rooted path, it is unchanged. If not, checks application path
		/// for given filename, if it exists that path is returned. If not,
		/// the file path is set to relative path.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static string GetPathForFile(string fileName)
		{
			string filePath = fileName;
			if (!Path.IsPathRooted(fileName))
			{
				filePath = Path.Combine(ApplicationPath.DirectoryPath, fileName);
				if (!File.Exists(filePath))
				{
					//Try common data folder
					filePath = Path.Combine(ApplicationPath.CommonApplicationDataPath(), fileName);
					if (!File.Exists(filePath))
					{
						filePath = Path.GetFullPath(fileName);
					}
				}
			}
			return filePath;
		}

		/// <summary>
		/// Get a token from a delimited string, eg.
		///   intSection = 0
		///   strSection = GetToken(lpSections, charNull, intSection)
		/// </summary>
		/// <param name="strText">Text that is delimited</param>
		/// <param name="delimiter">The delimiter, eg. ","</param>
		/// <param name="intIndex">The index of the token to return, NB. first token is index 0.</param>
		/// <returns>Returns the nth token from a string.</returns>
		private static string GetToken(string strText, char[] delimiter, int intIndex)
		{
			string strTokenRet = "";

			string[] strTokens = strText.Split(delimiter);

			if (strTokens.GetUpperBound(0) >= intIndex)
				strTokenRet = strTokens[intIndex];

			return strTokenRet;
		} // GetToken

		/// <summary>
		/// Reads XML information from a file into a Configuration object.
		/// May throw an exception if file not found or Xml mapping fails.
		/// Input XML file should match the following structure...
		///   <configuration>
		///       <section name="Main">
		///           <setting name="Timeout" value="90"/>
		///           <setting name="Mode" value="Live"/>
		///      </section>
		///   </configuration>
		/// </summary>
		/// <returns>A configuration object containing array of sections, each with
		/// array of name/value pairs</returns>
		public static Configuration ReadConfigurationFromXmlFile(string fileName)
		{
			Configuration configuration = new Configuration();

			// Create deserializer for configuration object
			FileUtilities.XmlConfigurationMapping mappingInfo =
				new BioRad.Common.FileUtilities.XmlConfigurationMapping();
			XmlToTypeSerializer serializer =
				new XmlToTypeSerializer(mappingInfo.GetConfigurationMapping());

			// A FileStream is needed to read the XML document.
			using (FileStream fs = new FileStream(fileName, FileMode.Open,
						FileAccess.Read, FileShare.ReadWrite))
			{
				// Load the configuration object with data from the Xml document
				// with data from the XML document. 
				configuration = (Configuration)serializer.Deserialize(fs);
			}
			return configuration;
		}

		/// <summary>Reads a character array of the specified length from the file system
		/// using the caller's binary reader.  The binary reader must be associated with 
		/// an open file whose file pointer is correctly positioned for the read.  The 
		/// character encoding is set by the caller.  The array is stripped of trailing 
		/// nulls and returned as a string.</summary>
		/// <param name="br">Caller's binary reader with preset encoding.</param>
		/// <param name="length">Number of characters to read.</param>
		/// <returns>A string stripped of trailing nulls.</returns>
		public static string ReadCharArrayAndTrimNulls(BinaryReader br, int length)
		{
			char[] aNull = { '\0' };               // Trim trailing nulls.
			string temp = new string(br.ReadChars(length));
			return (temp.TrimEnd(aNull));
		}

		/// <summary>
		/// Gets the file names and versions from specified folder.
		/// </summary>
		/// <param name="folderPath">Folder path to search into.</param>
		/// <param name="includeFilesWithExtensions">Hashtable with 
		/// extensions(all lower case)on files to be included, 
		/// pass empty or null when all files needed.</param>
		/// <returns>Array of file names and their versions.</returns>
		public static ApplicationFilesInfo[] GetFileVersionInfo(string folderPath,
			Hashtable includeFilesWithExtensions)
		{
			bool getAllfiles = false;
			ArrayList filesArray = new ArrayList();

			if (includeFilesWithExtensions == null)
			{
				getAllfiles = true;
			}
			else if (includeFilesWithExtensions.Count.Equals(0))
			{
				getAllfiles = true;
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
			FileInfo[] files = directoryInfo.GetFiles();
			foreach (FileInfo fileInfo in files)
			{
				FileVersionInfo fileVersionInfo =
					FileVersionInfo.GetVersionInfo(fileInfo.FullName);
				ApplicationFilesInfo applicationFilesInfo =
					new ApplicationFilesInfo();
				applicationFilesInfo.CreationDate = fileInfo.CreationTime;
				applicationFilesInfo.Name = fileInfo.Name;
				applicationFilesInfo.Version = fileVersionInfo.FileVersion;

				if (getAllfiles)
				{
					filesArray.Add(applicationFilesInfo);
				}
				else if (includeFilesWithExtensions != null)
				{
					if (includeFilesWithExtensions.ContainsKey
						(fileInfo.Extension.ToLower()))
					{
						filesArray.Add(applicationFilesInfo);
					}
				}
			}
			return (ApplicationFilesInfo[])
				filesArray.ToArray(typeof(ApplicationFilesInfo));
		}
		/// <summary>
		/// Gets the file names and versions from specified folder.
		/// </summary>
		/// <param name="folderPath">Folder path to search into.</param>
		/// <param name="fileNames">Returns information about the specified files.</param>
		/// <returns>Array of file names and their versions.</returns>
		public static ApplicationFilesInfo[] GetFileVersionInfo(string folderPath,
			string[] fileNames)
		{
			ArrayList appFileInfoArray = new ArrayList();

			DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
			FileInfo[] files = directoryInfo.GetFiles();
			foreach (string name in fileNames)
			{
				if (name != null)
				{
					foreach (FileInfo fileInfo in files)
					{
						if (fileInfo.Name.Trim() == name.Trim())
						{
							FileVersionInfo fileVersionInfo =
								FileVersionInfo.GetVersionInfo(fileInfo.FullName);

							ApplicationFilesInfo appInfo = new ApplicationFilesInfo();
							appInfo.CreationDate = fileInfo.CreationTime;
							appInfo.Name = fileInfo.Name;
							appInfo.Version = fileVersionInfo.FileVersion;
							appFileInfoArray.Add((ApplicationFilesInfo)appInfo.Clone());
							break;
						}
					}
				}
			}

			return (ApplicationFilesInfo[])appFileInfoArray.ToArray
				(typeof(ApplicationFilesInfo));
		}

		/// <summary>Checks the file attributes for writing to a file.</summary>
		/// <remarks>Throws an exception if the file is ReadOnly, Hidden or is a System
		/// File.</remarks>
		/// <param name="fullPath">The full path for the name of the file the user
		/// wants to save to.</param>
		public static void CheckFileAttributesForWrite(string fullPath)
		{
			if (File.Exists(fullPath))
			{
				FileAttributes attributes = File.GetAttributes(fullPath);
				// ReadOnly
				if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
				{
					throw new InvalidOperationException
								(StringUtility.FormatString(Properties.Resources.ReadOnly));
				}
				// Hidden
				else if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
				{
					throw new InvalidOperationException
								(StringUtility.FormatString(Properties.Resources.Hidden));
				}
				// System
				else if ((attributes & FileAttributes.System) == FileAttributes.System)
				{
					throw new InvalidOperationException
								(StringUtility.FormatString(Properties.Resources.System));
				}
			}
		}
		/// <summary>Removes the file attributes from a file so that it can be writtent to
		/// .</summary>
		/// <param name="fullPath">The full path for the name of the file the user
		/// wants to save to.</param>
		public static void RemoveFileAttributesForWrite(string fullPath)
		{
			File.SetAttributes(fullPath, FileAttributes.Normal);
		}
		/// <summary>
		/// Sets a file as read-only.
		/// </summary>
		/// <param name="filePath"></param>
		public static void SetFileReadOnly(string filePath)
		{
			FileAttributes currentAttributes = File.GetAttributes(filePath);
			currentAttributes |= FileAttributes.ReadOnly;
			File.SetAttributes(filePath, currentAttributes);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fullPath"></param>
		/// <param name="fileExtension"></param>
		/// <returns></returns>
		public static string ValidateFilePathName(string fullPath, string fileExtension)
		{
			// remove any invalid path characters
			StringBuilder sb = new StringBuilder();
			bool valid = true;

			foreach (char ch in fullPath)
			{
				valid = true;
				// remove all white spaces except for the space 
				if ((Char.IsWhiteSpace(ch)) && (ch != ' '))
					valid = false;
				else
				{
					foreach (char c in Path.GetInvalidPathChars())
					{
						if (ch == c)
						{
							valid = false;
							break;
						}
					}
				}
				if (valid)
					sb.Append(ch);
			}
			fullPath = sb.ToString();
			sb.Remove(0, sb.Length);

			// Fix for Bug 3591 - just check the extension
			string extension = Path.GetExtension(fullPath);
			// extension has the '.' add this to fileExtension if needed
			if (fileExtension.IndexOf(".", 0, 1) != 0)
			{
				sb.Append(".");
				sb.Append(fileExtension);
				fileExtension = sb.ToString();
				sb.Remove(0, sb.Length);
			}

			if (!string.Compare(extension, fileExtension, true).Equals(0))
			{
				sb.Append(fullPath);
				sb.Append(fileExtension);
				fullPath = sb.ToString();

			}
			return fullPath;

		}


		/// <summary>Open the file, set its attribute to normal before opening</summary>
		/// <remarks>Throws an exception if the file is a System File or the attribute
		/// cannot be set to Normal.</remarks>
		/// <param name="fullPath">The full path for the name of the file to open</param>
		/// <param name="fileMode">System.IO.FileMode Specifies how the OS should open the file</param>
		/// <returns>System.IO.FileStream object</returns>
		public static FileStream OpenFile(string fullPath, FileMode fileMode)
		{
			FileStream fs = null;
			if (File.Exists(fullPath))
			{
				File.SetAttributes(fullPath, FileAttributes.Normal);	// force to normal
			}
			fs = File.Open(fullPath, fileMode);

			return fs;
		}

		/// <summary>Open the file, set its attribute to normal before opening</summary>
		/// <remarks>Throws an exception if the file is a System File or the attribute
		/// cannot be set to Normal.</remarks>
		/// <param name="fullPath">The full path for the name of the file to open</param>
		/// <param name="fileMode">System.IO.FileMode Specifies how the OS should open the file</param>
		/// <param name="fileAccess">System.IO.FileAccess Constants for read, 
		/// write or read/write access to a file</param>
		/// <returns>System.IO.FileStream object</returns>
		public static FileStream OpenFile(string fullPath, FileMode fileMode, FileAccess fileAccess)
		{
			FileStream fs = null;
			if (File.Exists(fullPath))
			{
				File.SetAttributes(fullPath, FileAttributes.Normal);	// force to normal
			}
			fs = File.Open(fullPath, fileMode, fileAccess);

			return fs;
		}

		/// <summary>Open the file, set its attribute to normal before opening</summary>
		/// <remarks>Throws an exception if the file is a System File or the attribute
		/// cannot be set to Normal.</remarks>
		/// <param name="fullPath">The full path for the name of the file to open</param>
		/// <param name="fileMode">System.IO.FileMode Specifies how the OS should open the file</param>
		/// <param name="fileAccess">System.IO.FileAccess Constants for read, 
		/// write or read/write access to a file</param>
		/// <param name="fileShare">System.IO.FileShare 
		/// Constants for controlling the kind of access to a file</param>
		/// <returns>System.IO.FileStream object</returns>
		public static FileStream OpenFile(string fullPath, FileMode fileMode,
			FileAccess fileAccess, FileShare fileShare)
		{
			FileStream fs = null;
			try
			{
				if (File.Exists(fullPath))
					File.SetAttributes(fullPath, FileAttributes.Normal);	// force to normal

				fs = File.Open(fullPath, fileMode, fileAccess, fileShare);
			}
            catch (Exception ex) //Logged
			{
				DiagnosticsLogService.GetService.GetDiagnosticsLog
					 (WellKnownLogName.ApplicationStartUp).Exception(ex);
				throw;
			}
			return fs;
		}
		/// <summary>Delete the file, set its attribute to normal before deleting,
		/// also delete the temp GUID folder. we use the temp GUID folder to hold 
		/// temp unsercure version of data file
		/// </summary>
		/// <remarks>Throws an exception if the file is a System File or the attribute
		/// cannot be set to Normal.</remarks>
		/// <param name="fullPath">The full path for the name of the file to delete</param>
		public static void DeleteDecompressTempFile(string fullPath)
		{
			try
			{
				if (!File.Exists(fullPath))
					return;

				// delete the file
				File.SetAttributes(fullPath, FileAttributes.Normal);	// force to normal
				File.Delete(fullPath);
			}
            catch (Exception ex) //Logged
			{
				DiagnosticsLogService.GetService.GetDiagnosticsLog
					  (WellKnownLogName.ApplicationStartUp).Exception(ex);
			}
		}

		/// <summary>Delete the file, set its attribute to normal before deleting</summary>
		/// <remarks>Throws an exception if the file is a System File or the attribute
		/// cannot be set to Normal.</remarks>
		/// <param name="fullPath">The full path for the name of the file to delete</param>
		public static void DeleteFile(string fullPath)
		{
			try
			{
				if (!File.Exists(fullPath))
					return;

				// delete the file
				File.SetAttributes(fullPath, FileAttributes.Normal);	// force to normal
				File.Delete(fullPath);
			}
            catch (Exception ex) //Logged
			{
				DiagnosticsLogService.GetService.GetDiagnosticsLog
                    (WellKnownLogName.ApplicationStartUp).Info("Could not delete file", ex);
			}
		}
		/// <summary>
		/// Copies an existing file to a new file. Overwriting a file of the same name is allowed.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="dest"></param>
		/// <returns>true if copy was successful.</returns>
		public static bool CopyFile(string source, string dest)
		{
			if (string.IsNullOrEmpty(source))
				throw new ArgumentNullException("source");
			if (string.IsNullOrEmpty(dest))
				throw new ArgumentNullException("dest");

			if (source.Length > c_MaxFileNameLength)
				throw new ArgumentException("File name length exceed the maximum length 256 characters");
			if (dest.Length > c_MaxFileNameLength)
				throw new ArgumentException("File name length exceed the maximum length 256 characters");

			bool status = true;

			try
			{
				if (File.Exists(dest))
					FileUtilities.DeleteFile(dest, 100, 100);

				if (!File.Exists(dest))
				{
					File.SetAttributes(source, FileAttributes.Normal);

					File.Copy(source, dest, true);

					File.SetAttributes(dest, FileAttributes.Normal);
					FileUtilities.CheckFileAttributesForWrite(dest);
				}
				else
				{
					DiagnosticsLogService.GetService.GetDiagnosticsLog
						  (WellKnownLogName.ApplicationStartUp).
						  SeriousError(string.Format("File delete failed. {0}", dest));
					status = false;
				}
			}
            catch (Exception ex) //Logged
			{
				DiagnosticsLogService.GetService.GetDiagnosticsLog
						  (WellKnownLogName.ApplicationStartUp).Exception(ex);
				status = false;
			}
			finally
			{
				if (!File.Exists(dest))
				{
					DiagnosticsLogService.GetService.GetDiagnosticsLog
						  (WellKnownLogName.ApplicationStartUp).
						  SeriousError(string.Format("File copy failed. {0}", dest));
					status = false;
				}
			}
			Debug.Assert(status, "file copy failed");
			return status;
		}
		/// <summary>Delete the file, set its attribute to normal before deleting.</summary>
		/// <remarks>
		/// This method will try to delete the file, up to the retry parameter, with
		/// a delay in millseconds you specified between retrys.  Exceptions are caught and
		/// iqnored.
		///</remarks>
		/// <param name="path">The full path for the name of the file to delete.</param>
		/// <param name="retry">The number of times to retry deleting then file.</param>
		/// <param name="delayMilliSeconds">The number of milliseconds to delay between retries.</param>
		public static void DeleteFile(string path, uint retry, int delayMilliSeconds)
		{
			if (string.IsNullOrEmpty(path))
				return;

			if (delayMilliSeconds < 0)
				delayMilliSeconds = 0;

			for (int i = 0; i < retry && File.Exists(path); i++)
			{
				try
				{
					//FileUtilities.DeleteFile(path);
					if (!File.Exists(path))
						return;

					// delete the file
					File.SetAttributes(path, FileAttributes.Normal);	// force to normal
					File.Delete(path);
				}
                catch (Exception ex) //Logged
				{
					DiagnosticsLogService.GetService.GetDiagnosticsLog
						("Utilities").Debug("File delete exception caught - attempt: "
						+ i.ToString() + " out of: " + retry.ToString(), ex);

					// iqnore all exceptions.
				}
				finally
				{
					if (File.Exists(path))
						Thread.Sleep(delayMilliSeconds);
				}
			}
			if (File.Exists(path))
			{
				DiagnosticsLogService.GetService.GetDiagnosticsLog
								("Utilities").SeriousError("File Deletion failed: " + path);
			}
		}

		/// <summary>
		/// Gets version stored in XML file.
		/// Note: works for master fluor file only for now, extend this to others depending on schema.
		/// </summary>
		/// <param name="fullPathFileName"></param>
		static public string GetVersionStoredInFile(string fullPathFileName)
		{
			string version = "0";
			try
			{
				using (Stream fs = FileCryptor.GetInstance.DecryptFileContentsToStream(fullPathFileName))
				{
					XmlTextReader reader = new XmlTextReader(fs);
					XmlDocument document = new XmlDocument();
					document.Load(reader);
					XmlNode docElement = document.DocumentElement;
					version = docElement.Attributes["version"].Value.ToString();
				}
				return version;
			}
			catch
			{
				return version;
			}
		}

		/// <summary>
		/// Read IST XML file. Decrypt it it needed. 
		/// Extract global version number as well as content to pass to the IST
		/// </summary>
		/// <param name="fullPathFileName"></param>
		/// <param name="textContent"></param>
		/// <returns></returns>
		static public float GetISTVersionAndContent(string fullPathFileName, out string textContent)
		{
			//Initialize out parameter
			textContent = String.Empty;

			//Define empty header string
			string textHeader = String.Empty;

			//Default to version zero
			float version = 0.0f;

			try
			{
				//Decrypt if necessary, this gives us a Stream
				using (Stream fs = FileCryptor.GetInstance.DecryptFileContentsToStream(fullPathFileName))
				{
					//Convert that stream to a StreamReader object, so 
					//we can read lines individually
					using (StreamReader sr = new StreamReader(fs))
					{
						//Read header (first line)
						textHeader = sr.ReadLine();
						//Read content that will be passed to the IST
						textContent = sr.ReadToEnd();
					}

					//Get global IST version number from header 
					textHeader = textHeader.Replace(
						"<?xml version=\"", //Do not localize
						String.Empty);
					textHeader = textHeader.Replace(
						"\" encoding=\"utf-8\"?>", //Do not localize
						String.Empty);

					//Now try to extract the version number (culture invariant)
					if (!float.TryParse(textHeader,
						System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture,
							out version)
						)
					{
						//zero returned if parse fails
					}

					//Fix for defect 8677- CfxistConfig.xml uses an unknown version of XML.
					//
					//On 2008/06/25, the top XML header "xml version=..." has been reset to 1.0
					//and we are no longer controlling IST version numbers using this field.
					//If we detect a 1.0, we look for the IST version number in "CodeVersion",
					//which is where we now find the code version 
					if (version == 1.0f)
					{
						//Code version to find
						string codeVersion = String.Empty;

						//Define parsing context
						XmlParserContext parser = new XmlParserContext(null, null, string.Empty,
							XmlSpace.None);
						//Reload XML file into a XmlTextReader object
						XmlTextReader reader = new XmlTextReader
							(textHeader + textContent, XmlNodeType.Element, parser);

						//Code Version flag 
						bool successfullyRetrievedCodeVersion = false;

						//Parse file
						while (reader.Read())
						{
							//Have we found the code version?
							if (StringUtility.StringMatch(reader.Name, "CodeVersion")) //Do not localize
							{
								//if so, read the corresponding value, and exit from loop
								reader.Read();
								codeVersion = reader.Value;
								successfullyRetrievedCodeVersion = true;
								break;
							}
						}

						//If Code version found, return it to caller
						if (successfullyRetrievedCodeVersion)
						{
							//Convert version from int to float
							int intVersion = 0;
							if (int.TryParse(codeVersion, out intVersion))
							{
								version = intVersion / 1000.0f;
							}
						}
					}
					//Return it
					return version;
				}

			}
			catch
			{
				//Return zero in case of failure
				return version;
			}

		}
		/// <summary>
		/// Copy fluor config file to APP Data location. Assumes version is stored in the file.
		/// </summary>
		static public bool CopyFluorConfigFile()
		{
			try
			{
				string sourceFile = Path.Combine(ApplicationPath.DirectoryPath,
					"config\\referencedata-masterfluors.xml");
				string destinationFolder = Path.Combine(ApplicationPath.CommonApplicationDataPath(), "config");
				if (!Directory.Exists(destinationFolder))
				{
					Directory.CreateDirectory(destinationFolder);
				}
				string destinationFile = Path.Combine(destinationFolder, Path.GetFileName(sourceFile));
				//Check if the file exist
				if (File.Exists(destinationFile))
				{
					//PW: code to check the version. if older, replace it.
					string sourceFileVersion = GetVersionStoredInFile(sourceFile);
					string destinationFileVersion = GetVersionStoredInFile(destinationFile);
					if (string.Compare(sourceFileVersion,
						destinationFileVersion, true,
						System.Globalization.CultureInfo.InvariantCulture) != 0)
					{
						//Delete
						FileUtilities.DeleteFile(destinationFile);
						//Copy
						File.Copy(sourceFile, destinationFile, true);
					}
				}
				else
				{
					//Copy
					File.Copy(sourceFile, destinationFile, true);
				}
				return true;
			}
            catch (Exception ex) //Logged
			{
				// log the error
				DiagnosticsLogService.GetService.GetDiagnosticsLog
						  (WellKnownLogName.ApplicationStartUp).Exception(ex);

				return false;
			}
		}

        /// <summary>
        /// Copy config file to APP Data location. Assumes version is stored in the file.
        /// </summary>
        static public bool CopyIstConfigFile(string sourceFile)
        {
            try
            {
                string destinationFolder = Path.Combine(ApplicationPath.CommonApplicationDataPath(), "config");
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }
                string destinationFile = Path.Combine(destinationFolder, Path.GetFileName(sourceFile));

                if (File.Exists(destinationFile))
                {
                    FileUtilities.DeleteFile(destinationFile);
                }

                File.Copy(sourceFile, destinationFile, true);
                return true;
            }
            catch (Exception ex) //Logged
            {
                DiagnosticsLogService.GetService.GetDiagnosticsLog
                     (WellKnownLogName.ApplicationStartUp).Exception(ex);

                return false;
            }
        }
		/// <summary>
		/// Retrieves the contents of a folder (ignoring subfolders).
		/// </summary>
		/// <param name="folderPath">Path to the folder.</param>
		/// <returns>A collection of FileInfo objects, one per file found in the given folder.</returns>
		public static FileInfo[] GetFolderContents(string folderPath)
		{
			return GetFolderContents(folderPath, false);
		}
		/// <summary>
		/// Retrieves the contents of a folder (ignoring subfolders).
		/// </summary>
		/// <param name="folderPath">Path to the folder.</param>
		/// <param name="recursive">Whether to search contained folders recursively</param>
		/// <returns>A collection of FileInfo objects, one per file found in the given folder.</returns>
		public static FileInfo[] GetFolderContents(string folderPath, bool recursive)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
			directoryInfo.Refresh();
			FileInfo[] filesInFolder = filesInFolder = directoryInfo.GetFiles("*", 
				(recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
			foreach (FileInfo fileInRip in filesInFolder)
				fileInRip.Refresh();
			return filesInFolder;
		}
		/// <summary>
		/// Verify if the file name and path contains valid characters
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static bool IsValidFileName(string filePath)
		{
			bool valid = false;
			if (!string.IsNullOrEmpty(filePath))
			{
				string thePath = Environment.ExpandEnvironmentVariables(filePath);
				bool validPathChar = false;

				// validate the path
				if (!string.IsNullOrEmpty(thePath))
				{
					validPathChar = true;
					char[] invalidPathChars = Path.GetInvalidPathChars();
					foreach (char invalidChar in invalidPathChars)
					{
						if (filePath.Contains(invalidChar.ToString()))
						{
							validPathChar = false;
							break;
						}
					}
					if (validPathChar)
						validPathChar = !string.IsNullOrEmpty(Path.GetPathRoot(thePath));
				}

				// validate the file name
				bool validFileChar = false;
				if (validPathChar)
				{
					string fileName = Path.GetFileName(thePath);
					if (!string.IsNullOrEmpty(fileName))
					{
						validFileChar = true;
						char[] invalidFileChars = Path.GetInvalidFileNameChars();
						foreach (char invalidChar in invalidFileChars)
						{
							if (fileName.Contains(invalidChar.ToString()))
							{
								validFileChar = false;
								break;
							}
						}
					}
				}
				// final validation                                          
				valid = validFileChar && validPathChar;
			}
			return valid;
		}
		/// <summary>
		/// Returns file name with added default extension.
		/// This method is created insrepsonse to bug 11288.
		/// </summary>
		/// <param name="fileName">Full path file name.</param>
		/// <param name="defaultExtension">Extension to be added.</param>
		/// <returns>Full path file name with default extension added.</returns>
		private static string GetFileNameWithDefaultExtension(string fileName, string defaultExtension)
		{
			if (!defaultExtension.StartsWith("."))
			{
				defaultExtension = string.Concat(".", defaultExtension);
			}
			if (Path.GetExtension(fileName) != null)
			{
				if (!Path.GetExtension(fileName).ToLower(CultureInfo.InvariantCulture).Equals(defaultExtension.ToLower(CultureInfo.InvariantCulture)))
				{
					fileName = string.Concat(fileName, defaultExtension);
				}
			}
			return fileName;
		}
		/// <summary>Displays the 'Open File' Dialog.</summary>
		/// <param name="dialogTitle">The Title for the Dialog, required.</param>
		/// <param name="fileFilter">The Filters for the Dialog, required.</param>
		/// <param name="filterIndex">The Filter index for the Dialog, required.</param>
		/// <param name="initialDirectoryPath">The Initial Directory for the Dialog, optional, if empty, uses current working directory</param>
		/// <param name="initialFileName">initial File name, optional.</param>
		/// <param name="isMultiSelect">True to allow multi selecttion of files, required.</param>
		/// <returns>A string collection object containing the user selected file name(s). If
		/// the user Cancels the dialog, null is returned.</returns>
		public static string[] ShowOpenFileDialog(string dialogTitle, string fileFilter, int filterIndex,
		 string initialDirectoryPath, string initialFileName, bool isMultiSelect)
		{
			using (OpenFileDialog openDialog = new OpenFileDialog())
			{
				openDialog.Filter = fileFilter;
				openDialog.FilterIndex = filterIndex;
				openDialog.Multiselect = isMultiSelect;
				openDialog.CheckFileExists = true;
				if (!string.IsNullOrEmpty(dialogTitle))
				{
					openDialog.Title = dialogTitle;
				}
				openDialog.CheckPathExists = true;

				if (!string.IsNullOrEmpty(initialFileName))
				{
					openDialog.FileName = initialFileName;
				}
				// if the passed in directory does not exist default to CWD
				if (!System.IO.Directory.Exists(initialDirectoryPath))
					initialDirectoryPath = Directory.GetCurrentDirectory();

				openDialog.InitialDirectory = initialDirectoryPath;

				if (openDialog.ShowDialog().Equals(DialogResult.OK))
				{
					// save the selected folder - the selected folder does not automatically
					// get saved in windows 7
					Directory.SetCurrentDirectory(Path.GetDirectoryName(openDialog.FileName));
					return openDialog.FileNames;
				}
				else
					return null;
			}
		}
		/// <summary>Displays the 'Save File' Dialog.</summary>
		/// <param name="dialogTitle">The Title for the Dialog, required.</param>
		/// <param name="fileFilter">The Filters for the Dialog, required.</param>
		/// <param name="filterIndex">The Filter index for the Dialog, required.</param>
		/// <param name="initialDirectoryPath">The Initial Directory for the Dialog, optional.</param>
		/// <param name="initialFileName">initial File name, optional.</param>
		/// <param name="defaultExt">Default extension for file dialog, required.</param>
		/// <param name="overwritePrompt">whether to prompt for overwrite of existing file, required.</param>
		/// <returns>A string object containing the user selected file name. If
		/// the user Cancels the dialog an empty string(string.Empty) is returned.</returns>
		public static string ShowFileSaveDialog(string dialogTitle, string fileFilter, int filterIndex,
		 string initialDirectoryPath, string initialFileName, string defaultExt, bool overwritePrompt)
		{
			using (SaveFileDialog saveDialog = new SaveFileDialog())
			{
				saveDialog.OverwritePrompt = overwritePrompt;
				saveDialog.Filter = fileFilter;
				saveDialog.FilterIndex = filterIndex;
				if (!string.IsNullOrEmpty(dialogTitle))
				{
					saveDialog.Title = dialogTitle;
				}
				saveDialog.DefaultExt = defaultExt;
				saveDialog.CheckPathExists = true;
				saveDialog.AddExtension = true;

				if (!string.IsNullOrEmpty(initialFileName))
				{
					saveDialog.FileName = initialFileName;
				}
				// if the passed in directory does not exist default to CWD
				if (!System.IO.Directory.Exists(initialDirectoryPath))
					initialDirectoryPath = Directory.GetCurrentDirectory();


				//PW: this InitialDirectory  call is needed.(For all saves)
				saveDialog.InitialDirectory = initialDirectoryPath;


                //-----
                DialogResult result = DialogResult.OK;
                bool filePathCheck = false;
                while (!filePathCheck)
				{
					if (saveDialog.ShowDialog().Equals(DialogResult.OK))
					{
						if (saveDialog.FileName != string.Empty)
						{
							result = DialogResult.OK;
                            filePathCheck = true;
                            // TFS 4229 deleted code no longer valid.
						}
						else
							result = DialogResult.Cancel;

						if (result == DialogResult.Cancel)
							return string.Empty; ;

						//Bug 11288
						if (filePathCheck)
						{
							string ext = Path.GetExtension(saveDialog.FileName).ToLower(CultureInfo.InvariantCulture);
							if (!saveDialog.Filter.ToLower(CultureInfo.InvariantCulture).Contains(ext))
							{
								saveDialog.FileName = FileUtilities.GetFileNameWithDefaultExtension(
								saveDialog.FileName, saveDialog.DefaultExt);
							}
						}

					}
					// Fix for WI 353, 358 and 371 - the dialog's Cancel button press was not getting handled.
					// Bug caused by fix for WI 226
					else
						return string.Empty;

                }
				return saveDialog.FileName;
			}
		}

		/// <summary>
		/// Checks if the given pathname points to a folder in which this application can create new files.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static bool CheckIsFolderWritable(string path)
		{
			if (Directory.Exists(path) == false)
				return false;
			try
			{
				string tempFileName = Guid.NewGuid().ToString();
				string tempFilePath = Path.Combine(path, tempFileName);
				File.Create(tempFilePath).Close();
				File.Delete(tempFilePath);
			}
			catch
			{
				return false;
			}
			return true;
		}
		/// <summary>Returns a legal version of the given filename</summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static string GetLegalFilenameFromPotentiallyIllegalFilename(string fileName)
		{
			string[] illegalNames = new string[]{
				"CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", 
				"LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"};

			string baseName = Path.GetFileNameWithoutExtension(fileName);
			foreach (string illegalName in illegalNames)
			{
				if (baseName == illegalName)
				{
					string directoryName = Path.GetDirectoryName(fileName);
					string extension = Path.GetExtension(fileName);
					string basePath = Path.Combine(directoryName, baseName + "_");
					string fullName = Path.ChangeExtension(basePath, extension);
					return fullName;
				}
			}

			return fileName;
		}
		#endregion Methods
	}

	/// <summary>
	/// WIN32 IniFile API Wrapper class
	/// </summary>
	public class IniFile
	{
		/// <summary>
		/// Get all the section names from an INI file
		/// </summary>
		[DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSectionNamesA")]
		public extern static int GetPrivateProfileSectionNames(
			[MarshalAs(UnmanagedType.LPArray)] byte[] lpReturnedString,
			int nSize,
			string lpFileName);

		/// <summary>
		/// Get all the settings from a section in a INI file
		/// </summary>
		[DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSectionA")]
		public extern static int GetPrivateProfileSection(
			string lpAppName,
			[MarshalAs(UnmanagedType.LPArray)] byte[] lpReturnedString,
			int nSize,
			string lpFileName);

		/// <summary>
		/// Write a string to specified file name in the section with the key name
		/// </summary>
		/// <param name="section">Section Name</param>
		/// <param name="key">Key Name</param>
		/// <param name="val">defualt value or string</param>
		/// <param name="filePath">the file name with the directory path</param>
		/// <returns>error code</returns>
		[DllImport("kernel32")]
		public static extern long WritePrivateProfileString(string section,
			string key, string val, string filePath);

		/// <summary>
		/// Read a string from specified file in the section under the key name
		/// </summary>
		/// <param name="section">Section name string</param>
		/// <param name="key">key name string</param>
		/// <param name="def">default value if not found</param>
		/// <param name="retVal">return value string</param>
		/// <param name="size">maximum length of return string</param>
		/// <param name="filePath">the file name with the directory path</param>
		/// <returns>error code</returns>
		[DllImport("kernel32")]
		public static extern int GetPrivateProfileString(string section,
			string key, string def, StringBuilder retVal,
			int size, string filePath);

		/// <summary>
		/// Read a integer from specified file in the section under the key name
		/// </summary>
		/// <param name="section">Section name string</param>
		/// <param name="key">key name string</param>
		/// <param name="def">default value if not found</param>
		/// <param name="filePath">the file name with the directory path</param>
		/// <returns>error code</returns>
		[DllImport("kernel32")]
		public static extern int GetPrivateProfileInt(string section,
			string key, int def, string filePath);

	}

	/// <summary>Sort array of files according to last modified time.</summary>
	public class FileSortLastModified : IComparable
	{
		private FileInfo m_FileInfo;
		/// <summary>Gets the full path of the file.</summary>
		public string FullName
		{
			get { return m_FileInfo.FullName; }
		}
		/// <summary></summary>
		/// <param name="filepath"></param>
		public FileSortLastModified(string filepath)
		{
			m_FileInfo = new FileInfo(filepath);
		}
		/// <summary></summary>
		/// <param name="fileInfo"></param>
		public FileSortLastModified(FileInfo fileInfo)
		{
			m_FileInfo = fileInfo;
		}
		/// <summary></summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int CompareTo(object obj)
		{
			//string myTime = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff");

			if (!(obj is FileSortLastModified))
				throw new ArgumentException("o must be of type 'FileSortLastModified'");
			FileSortLastModified v = (FileSortLastModified)obj;
			return DateTime.Compare(this.m_FileInfo.LastWriteTime, v.m_FileInfo.LastWriteTime);
		}
	}
}
