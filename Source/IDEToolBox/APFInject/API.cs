using System;
using System.Collections;
using System.IO;

namespace IDEToolBox.APFInject
{
	public class API
	{
		#region Constructors and Destructor

		/// <summary>Initializes a new instance of the API class.</summary>
		public API() { }

		#endregion

		#region Methods

		/// <summary>
		/// Encrypts all files in a directory based on a list of wildcard filters and creates encyrpted versions in the same or a different directory.
		/// </summary>
		/// <param name="sourceDir">Directory containing files to be encrypted.</param>
		/// <param name="targetDir">Direcotry where encrypted files will be placed.</param>
		/// <param name="overwrite">Overwrite any existing file of the same name.</param>
		/// <param name="passFilters">List of wildcard filters to determining files to encrypt.</param>
		/// <param name="blockFilters">List of wildcard filters to determining files to ignore.</param>
		public void EncryptDirectory(string sourceDir, string targetDir, bool overwrite, IList passFilters, IList blockFilters)
		{
			if (!Directory.Exists(sourceDir))
				throw new ApplicationException(sourceDir + " does not exist.");

			if (!Directory.Exists(targetDir))
				throw new ApplicationException(targetDir + " does not exist.");

			foreach (string filename in GetFiles(sourceDir, passFilters, blockFilters))
				EncryptFile(filename, targetDir, overwrite);
		}

		/// <summary>
		/// Decrypts all files in a directory based on a list of wildcard filters and creates encyrpted versions in the same or a different directory.
		/// </summary>
		/// <param name="sourceDir">Directory containing files to be decrypted.</param>
		/// <param name="targetDir">Direcotry where decrypted files will be placed.</param>
		/// <param name="overwrite">Overwrite any existing file of the same name.</param>
		/// <param name="passFilters">List of wildcard filters to determining files to decrypt.</param>
		/// <param name="blockFilters">List of wildcard filters to determining files to ignore.</param>
		public void DecryptDirectory(string sourceDir, string targetDir, bool overwrite, IList passFilters, IList blockFilters)
		{
			if (!Directory.Exists(sourceDir))
				throw new ApplicationException(sourceDir + " does not exist.");

			if (!Directory.Exists(targetDir))
				throw new ApplicationException(sourceDir + " does not exist.");

			foreach (string filename in GetFiles(sourceDir, passFilters, blockFilters))
				DecryptFile(filename, targetDir, overwrite);
		}

		/// <summary>
		/// Decrypts a single file.
		/// </summary>
		/// <param name="sourceFile">Full path source file name.</param>
		/// <param name="targetDir">Full path target directory name.</param>
		/// <param name="overwrite">Force the overwrite of an existing file of the same name in the target directory.</param>
		/// <returns>false if file already exists, otherwise true.</returns>
		/// <remarks>New filename is always the same as the source filename, however the directory location may be different.</remarks>
		public bool DecryptFile(string sourceFile, string targetDir, bool overwrite)
		{
			BioRad.Common.Utilities.FileCryptor fc = BioRad.Common.Utilities.FileCryptor.GetInstance;

			string targetFile = Path.Combine(targetDir, Path.GetFileName(sourceFile));

			if (overwrite || !File.Exists(targetFile))
				fc.DecryptFile(sourceFile, targetFile);
			else
				return false;

			return true;
		}

		/// <summary>
		/// Encrypts a single file.
		/// </summary>
		/// <param name="sourceFile">Full path source file name.</param>
		/// <param name="targetDir">Full path target directory name.</param>
		/// <param name="overwrite">Force the overwrite of an existing file of the same name in the target directory.</param>
		/// <returns>false if file already exists, otherwise true.</returns>
		/// <remarks>New filename is always the same as the source filename, however the directory location may be different.</remarks>
		public bool EncryptFile(string sourceFile, string targetDir, bool overwrite)
		{
			BioRad.Common.Utilities.FileCryptor fc = BioRad.Common.Utilities.FileCryptor.GetInstance;

			string targetFile = Path.Combine(targetDir, Path.GetFileName(sourceFile));

			if (overwrite || !File.Exists(targetFile))
				fc.EncryptFile(sourceFile, targetFile);
			else
				return false;

			return true;
		}

		/// <summary>
		/// Returns ArrayList of files from a single directory.
		/// </summary>
		/// <param name="sourceDir">Full directory path sourceDir.</param>
		/// <returns>Collection of sourceDir names.</returns>
		public IList GetFiles(string sourceDir)
		{
			IList filelist = new ArrayList();

			foreach (string filename in Directory.GetFiles(sourceDir))
				filelist.Add(filename);

			return filelist;
		}

		/// <summary>
		/// Returns ArrayList of files from a single directory according to a collection of wildcard filters.
		/// </summary>
		/// <param name="sourceDir">Full directory path sourceDir.</param>
		/// <param name="passFilters">Collection of wildcard filters whose resulting files to include.</param>
		/// <param name="blockFilters">Collection of wildcard filters whose result files to ingnore.</param>
		/// <returns>Collection of sourceDir names.</returns>
		public IList GetFiles(string sourceDir, IList passFilters, IList blockFilters)
		{
			IList filelist = new ArrayList();
			IList filelist2 = new ArrayList();

			if (!Directory.Exists(sourceDir))
				throw new ApplicationException(sourceDir + " is not a valid directory.");
			else
			{
				if (passFilters.Count > 0)
				{
					foreach (string passFilter in passFilters)
					{
						foreach (string filename in Directory.GetFiles(sourceDir, passFilter))
							filelist.Add(filename);
					}
				}
				else
					return GetFiles(sourceDir);

				if (blockFilters.Count > 0)
				{
					foreach (string blockFilter in blockFilters)
					{
						foreach (string filename in filelist)
						{
							if (!Path.Equals(Path.GetFileName(filename), blockFilter))
								filelist2.Add(filename);
						}
					}
				}
			}

			return filelist2;
		}

		/// <summary>
		/// Check if a directory exists, and offer option to create it if not.
		/// </summary>
		/// <param name="newPath">Directory in question.</param>
		/// <param name="create">Force creation of directory in question.</param>
		/// <returns>boolean based on whether the directory in question exists.</returns>
		public bool CheckDestinationPath(string newPath, bool create)
		{
			if (create && !Directory.Exists(newPath))
				Directory.CreateDirectory(newPath);

			return Directory.Exists(newPath);
		}

		#endregion
	}
}
