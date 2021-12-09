using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BioRad.Common.Utilities
{
	#region Documentation Tags
	/// <summary>
	/// Crypto utility class for encrypting/decrypting files.
	/// </summary>
	/// <remarks>
	/// Uses Rijndael algorithm.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Pramod Walse</item>
	///			<item name="review">Last design/code review:Pramod Walse, 8/14/04</item>
	///			<item name="conformancereview">Conformance review:Pramod Walse, 8/14/04</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">Replace this text with ID</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: FileCryptor.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Utilities/FileCryptor.cs $</item>
	///			<item name="vssrevision">$Revision: 20 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Blie $</item>
	///			<item name="vssdate">$Date: 11/03/09 4:29p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class FileCryptor
	{
		#region Constants
		/// <summary>
		/// Init Vector.
		/// </summary>
		private const string c_IV = "27z+MDG7kRgQ0d7deOBSMQ==";
		/// <summary>
		/// TODO: need to store hidden
		/// </summary>
		private const string c_EncryptPWD = "SecureCompressDecompressKeyiQ5V4Files!!##$$";
		#endregion

		#region Member Data
		/// <summary>Object used to provide singleton access to this object.</summary>
		private static FileCryptor m_FileCryptor = null;
		/// <summary>
		/// Crypto service.
		/// </summary>
		private static SymmetricAlgorithm m_CryptoService = null;
		#endregion

		#region Accessors
		/// <summary>
		/// Get reference to this object.
		/// </summary>
		public static FileCryptor GetInstance
		{
			get
			{
				if (m_FileCryptor == null)
					m_FileCryptor = new FileCryptor();
				return m_FileCryptor;
			}
		}
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Default Constructor
		/// </summary>
		private FileCryptor()
		{
			m_CryptoService = new RijndaelManaged();
			m_CryptoService.IV = Convert.FromBase64String(c_IV);
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(c_EncryptPWD,
				new byte[] {0x33, 0x55, 0x22, 0x8e, 0x20, 0x6d, 0x46, 0x65, 0x13, 
							   0x88, 0x56, 0x87, 0x89});
			m_CryptoService.Key = pdb.GetBytes(32);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Encryption, as Encrypt(), which transforms the encrypted byte array to a unicode string.
		/// </summary>
		/// <param name="textToEncrypt">text to encrypt.</param>
		/// <returns>encrypted text</returns>
		public string EncryptToString(string textToEncrypt)
		{
			byte[] encryptedBytes = Encrypt(textToEncrypt);
			return System.Convert.ToBase64String(encryptedBytes, Base64FormattingOptions.None);
		}
		/// <summary>
		/// Decryption, from an encrypted string returned from EncryptToString().
		/// </summary>
		/// <param name="encryptedText">text to decrypt</param>
		/// <returns>decrypted text</returns>
		public string DecryptFromString(string encryptedText)
		{
			byte[] encryptedBytes = System.Convert.FromBase64String(encryptedText);
			return Decrypt(encryptedBytes);
		}
		/// <summary>
		/// Encrypt string.  Unicode encoding is used in transforming the string to a Byte array,
		/// and that byte array is then encrypted.
		/// </summary>
		/// <param name="inputDecrypted">String to encrypt.</param>
		/// <returns>Encrypted byte array or zero length byte array</returns>
		public byte[] Encrypt(string inputDecrypted)
		{
			byte[] result = new byte[0];

			byte[] data =
				System.Text.ASCIIEncoding.Unicode.GetBytes(inputDecrypted);

			ICryptoTransform encrypter = m_CryptoService.CreateEncryptor();

			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream cryptostream = new CryptoStream(
						memoryStream,
						encrypter,
						CryptoStreamMode.Write))
				{
					cryptostream.Write(data, 0, data.Length);
					cryptostream.FlushFinalBlock();

					result = new byte[memoryStream.Length];
					memoryStream.Position = 0;
					memoryStream.Read(result, 0, result.Length);
				}
			}

			return result;
		}
		/// <summary>
		/// Decrypt array of bytes to a string.  Unicode encoding for the unencrypted Bytes is assumed.
		/// </summary>
		/// <param name="inputEncrypted">Array of bytes</param>
		/// <returns>Decrypted string or empty string</returns>
		public string Decrypt(byte[] inputEncrypted)
		{
			return Decrypt(inputEncrypted, Encoding.Unicode);
		}
		/// <summary>
		/// Decrypt array of bytes to a string, using a provided encoding for the transformation from
		/// the unencrypted bytes to the final string.
		/// </summary>
		/// <param name="inputEncrypted">Array of bytes</param>
		/// <param name="encoding">encoding of the byte array which was encrypted.</param>
		/// <returns>Decrypted string or empty string</returns>
		public string Decrypt(byte[] inputEncrypted, Encoding encoding)
		{
			string decryptedString = string.Empty;

			using (ICryptoTransform decrypter = m_CryptoService.CreateDecryptor())
			{
				using (MemoryStream memoryStream = new MemoryStream(inputEncrypted))
				{
					using (CryptoStream cryptostream = new CryptoStream(
							memoryStream,
							decrypter,
							CryptoStreamMode.Read))
					{
						StreamReader sr = new StreamReader(cryptostream, encoding);
						decryptedString = sr.ReadToEnd();
					}
				}
			}

			return decryptedString;
		}
		/// <summary>
		/// Encrypts file and saves it to output file.
		/// Overwrites if same, does not delete input file if different from output.
		/// </summary>
		/// <param name="sInputFilename">Input full path file name</param>
		/// <param name="sOutputFilename">Output full path file name</param>
		public void EncryptFile(string sInputFilename, string sOutputFilename)
		{
			CryptoStream cryptostream = null;
			FileStream fsInput = null;
			FileStream fsEncrypted = null;
			try
			{
				// get the encrypter
				ICryptoTransform encrypter = m_CryptoService.CreateEncryptor();

				//Read the contents
				fsInput = new FileStream(sInputFilename,
					FileMode.Open,
					FileAccess.ReadWrite, FileShare.ReadWrite);
				byte[] bytearrayinput = new byte[fsInput.Length];
				fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
				//Close input stream
				fsInput.Close();

				//Write encrypted data
				fsEncrypted = new FileStream(sOutputFilename,
					FileMode.OpenOrCreate,
					FileAccess.ReadWrite, FileShare.ReadWrite);
				cryptostream = new CryptoStream(fsEncrypted,
					encrypter,
					CryptoStreamMode.Write);
				cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				//Close streams
				if (fsInput != null)
				{
					fsInput.Close();
				}
				if (cryptostream != null)
				{
					cryptostream.Close();
				}
				if (fsEncrypted != null)
				{
					fsEncrypted.Close();
				}
			}
		}
		/// <summary>
		/// Encrypts stream.
		/// Pramod: Does not work, some issues.
		/// </summary>
		/// <param name="stream">Input full path file name</param>
		/// <returns>encrypted Stream</returns>
		private Stream EncryptStream(Stream stream)
		{
			CryptoStream cryptostream = null;
			//try
			{
				// get the encrypter
				ICryptoTransform encrypter = m_CryptoService.CreateEncryptor();
				cryptostream =
					new CryptoStream(stream, encrypter, CryptoStreamMode.Write);
				//Get encrypted array of bytes.
				return (Stream)cryptostream;
			}
		}
        /// <summary>
        /// Decrypts input file and compares first 6 bytes from input and decrypted file. 
        /// Slower than method IsXmlFileEncryted but more accurate. IsEncryted can be used on any file type 
        /// where as method IsXmlFileEncryted only supports xml files. 
        /// Method IsXmlFileEncryted says any non xml file is encrypted.
        /// </summary>
        /// <param name="sInputFilename">Absoulute path of file.</param>
        /// <returns>True if any of 1st 6 bytes are different.</returns>
        public bool IsEncryted(string sInputFilename)// ralph
        {
            const string unencryptedIndicator = "<?xml";

            bool isEncryted = false;

            try
            {
                string directoryName = Path.GetDirectoryName(sInputFilename);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sInputFilename);
                string extension = Path.GetExtension(sInputFilename);
                string sOutputFilename = Path.Combine(directoryName, Guid.NewGuid().ToString() );

                char[] firstChars = new char[6];
                char[] secondChars = new char[6];

                DecryptFile(sInputFilename, sOutputFilename);
                if (File.Exists(sOutputFilename))
                {
                    using (FileStream fs = new FileStream(sOutputFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        firstChars = br.ReadChars(unencryptedIndicator.Length + 1);
                    }
                    File.Delete(sOutputFilename);
                }

                using (FileStream fs = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (BinaryReader br = new BinaryReader(fs))
                {
                    secondChars = br.ReadChars(unencryptedIndicator.Length + 1);
                    for ( int i=0; i<firstChars.Length; i++)
                    {
                        if (firstChars[i] != secondChars[i])
                        {
                            isEncryted = true;
                            break;
                        }
                    }
                }
            }
            catch
            {
                //...
            }

            return isEncryted;
        }
        /// <summary>
        /// This tesing only apply to well formated XML with start documentation
        /// </summary>
        /// <param name="sInputFilename"></param>
        /// <returns></returns>
        public bool IsXmlFileEncryted(string sInputFilename)
        {
            FileStream fsInput = null;

			fsInput = new FileStream(sInputFilename,
				FileMode.Open,
				FileAccess.Read, FileShare.Read);
            
            bool unencryptedIndicatorFound = false;

            try
            {
                // check to see if it not encrypted
                const string unencryptedIndicator = "<?xml";
                BinaryReader r = new BinaryReader(fsInput);
                char[] firstChars = new char[6];
                firstChars = r.ReadChars(unencryptedIndicator.Length + 1);
                string firstString = new string(firstChars);
                // I think the string may terminate early if certain characters exists in the char array,
                //  so this check is here to avoid taking the substring (below) of a too-small string.
                if (firstString.Length == firstChars.Length)
                {
                    // Check for the magic string starting at file position zero or one.  The XmlWriter
                    //  in DotNet prepends some chars before "?<xml" which may have to be skipped.  All
                    //  those chars (there are three, actually) apparently get consolidated into one char
                    //  by the ReadChars method, so looking at position zero and one here is sufficient.
                    if (firstString.Substring(0, unencryptedIndicator.Length).Equals(unencryptedIndicator) ||
                            firstString.Substring(1, unencryptedIndicator.Length).Equals(unencryptedIndicator))
                    {
                        unencryptedIndicatorFound = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Clean up
                if (fsInput != null)
                {
                    fsInput.Close();
                }
            }
            return !unencryptedIndicatorFound;
        }
		/// <summary>
		/// Decrypts input file and saves as output file.
		/// Overwrites if same, does not delete input file if different from output.
		/// </summary>
		/// <param name="sInputFilename">Input full path file name</param>
		/// <param name="sOutputFilename">Output full path file name</param>
		public void DecryptFile(string sInputFilename, string sOutputFilename)
		{
			CryptoStream cryptostreamDecr = null;
			FileStream fsInput = null;
			StreamWriter fsDecrypted = null;
            try
            {
                // get the encrypter
                ICryptoTransform decrypter = m_CryptoService.CreateDecryptor();

                //Read the contents
                fsInput = new FileStream(sInputFilename,
                    FileMode.Open,
                    FileAccess.ReadWrite, FileShare.ReadWrite);
                cryptostreamDecr = new CryptoStream(fsInput,
                    decrypter, CryptoStreamMode.Read);
                string sr = new StreamReader(cryptostreamDecr).ReadToEnd();
                cryptostreamDecr.Close();
                fsInput.Close();

                //Print the contents of the decrypted file.
                fsDecrypted = new StreamWriter(sOutputFilename);
                fsDecrypted.Write(sr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
			finally
			{
				//Clean up
				if (fsInput != null)
				{
					fsInput.Close();
				}
				if (cryptostreamDecr != null)
				{
					cryptostreamDecr.Close();
				}
				if (fsDecrypted != null)
				{
					fsDecrypted.Flush();
					fsDecrypted.Close();
				}
			}
		}
		/// <summary>
		/// Decrypts file contents and returns text.
		/// </summary>
		/// <param name="sInputFilename">Input full path file name</param>
		/// <returns>decrypted string</returns>
		public string DecryptFileContents(string sInputFilename)
		{
			Stream stream = DecryptFileContentsToStream(sInputFilename);
			//return the contents of the decrypted file.
			string contents = new StreamReader(stream).ReadToEnd();
			return contents;
		}

		/// <summary>
		/// Decrypts file contents and returns stream.
		/// </summary>
		/// <param name="sInputFilename">Input full path file name</param>
		/// <returns>decrypted Stream</returns>
		public Stream DecryptFileContentsToStream(string sInputFilename)
		{
			FileStream fsInput = null;

			fsInput = new FileStream(sInputFilename,
				FileMode.Open,
				FileAccess.Read, FileShare.Read);

			try
			{
				// check to see if it not encrypted
				const string unencryptedIndicator = "<?xml";
				BinaryReader r = new BinaryReader(fsInput);
                char[] firstChars = r.ReadChars(unencryptedIndicator.Length + 1);
				string firstString = new string(firstChars);
				bool unencryptedIndicatorFound = false;
				// I think the string may terminate early if certain characters exists in the char array,
				//  so this check is here to avoid taking the substring (below) of a too-small string.
				if (firstString.Length == firstChars.Length)
				{
					// Check for the magic string starting at file position zero or one.  The XmlWriter
					//  in DotNet prepends some chars before "?<xml" which may have to be skipped.  All
					//  those chars (there are three, actually) apparently get consolidated into one char
					//  by the ReadChars method, so looking at position zero and one here is sufficient.
					if (firstString.Substring(0, unencryptedIndicator.Length).Equals(unencryptedIndicator) ||
							firstString.Substring(1, unencryptedIndicator.Length).Equals(unencryptedIndicator))
					{
						unencryptedIndicatorFound = true;
					}
					if (unencryptedIndicatorFound)
					{
						fsInput.Seek(0, SeekOrigin.Begin);
						return fsInput;
					}
				}
			}
			catch
			{
				//Ignore it
			}

			fsInput.Seek(0, SeekOrigin.Begin);

			try
			{
				CryptoStream cryptostreamDecr = null;
				Stream decryptedStream = null;
				string contents = "";
				// get the decrypter
				ICryptoTransform decrypter = m_CryptoService.CreateDecryptor();
				//Get current contents.
				cryptostreamDecr = new CryptoStream(fsInput,
					decrypter,
					CryptoStreamMode.Read);
				//Check for contents
				contents = new StreamReader(cryptostreamDecr).ReadToEnd();
				if (contents.Equals(string.Empty))
				{
					throw new ApplicationException("failed");
				}
				//return the decrypted stream.
				fsInput.Position = 0;
				cryptostreamDecr = new CryptoStream(fsInput,
					decrypter,
					CryptoStreamMode.Read);
				decryptedStream = (Stream)cryptostreamDecr;
				return decryptedStream;
			}
			catch
			{
				// do nothing, assume that the file is not xml and not encrypted
			}

			fsInput.Close();

			fsInput = new FileStream(sInputFilename,
				FileMode.Open,
				FileAccess.Read, FileShare.Read);

			return fsInput;

		}

		#endregion
	}
}
