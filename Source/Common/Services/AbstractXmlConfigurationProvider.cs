using System;
using System.Collections;
using System.IO;
using BioRad.Common.DiagnosticsLogger;
namespace BioRad.Common.Services
{
	#region Documentation Tags
	/// <summary>
	/// An abstract configuration provider specifically for working with xml data
	/// files.
	/// </summary>
	/// <remarks>
	/// This class takes care of a lot of the plumbing typically associated with dealing
	/// with xml data files used to configure services. One thing specifically that it
	/// does is properly deal with path information, looking first in the application
	/// path and then in the relative path for the given "FileName".
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Drew McAuliffe</item>
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
	///			<item name="vssfile">$Workfile: AbstractXmlConfigurationProvider.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Services/AbstractXmlConfigurationProvider.cs $</item>
	///			<item name="vssrevision">$Revision: 14 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Blie $</item>
	///			<item name="vssdate">$Date: 2/02/10 2:22p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public abstract partial class AbstractXmlConfigurationProvider : IConfigurationProvider
	{
        #region Member Data
		/// <summary>
		/// File name for configuration file.
		/// </summary>
		private string m_FileName;
        #endregion

        #region Accessors
		/// <summary>
		/// Accessors for file name.
		/// </summary>
		public string FileName
		{
		    get{return this.m_FileName;}
			set{this.m_FileName = value;}
		}

        #endregion

        #region Methods
		/// <summary>
		/// Properly checks for a valid file using the file name (from the
		/// accessor), first looking in the application path, then in a relative path.
		/// </summary>
		/// <remarks>Will throw <see cref="ServiceLoadException"/> if file doesn't
		/// exist.</remarks>
		/// <returns></returns>
		public string GetValidPathForFile()
		{
			return this.GetValidPathForFile(true);
		}

		/// <summary>
		/// Properly checks for a valid file using the file name (from the
		/// accessor), first looking in the application path, then in a relative path.
		/// </summary>
		/// <param name="fileMustExist">If true, throws <see cref="ServiceLoadException"/> if
		/// file not found.</param>
		/// <returns>File path in application path, if it exists, else file in relative path.
		/// if <see cref="FileName"/>is rooted, it is left unchanged.</returns>	
		public string GetValidPathForFile(bool fileMustExist)
		{
			string fileName = ApplicationPath.GetFullPath(this.FileName);
            try
            {
                // first check in the application path.
                FileInfo fileInfo = new FileInfo(fileName);
                if (!fileInfo.Exists)
                {
                    fileName = this.FileName;
                    // if this doesn't exist, there's a problem.
                    fileInfo = new FileInfo(fileName);
                    if (fileMustExist && !fileInfo.Exists)
                    {
                        throw new ServiceLoadException(this, DiagnosticSeverity.Serious,
                            StringUtility.FormatString(Properties.Resources.FileNotFoundInAppOrRelativePath_1,
                            this.FileName));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ServiceLoadException(this, DiagnosticSeverity.Serious,
                    StringUtility.FormatString("Fail to load Configuration file {0}){1}:{2}",
                    this.FileName, "System Exception",  ex.Message));
            }
			return fileName;
		}

		#region IConfigurationProvider Members

		/// <summary>
		/// Subclasses must implement the manner in which config elements are provided.
		/// </summary>
		/// <returns>A collection of config elements.</returns>
		public abstract ICollection GetConfigurationElements();

		#endregion

        #endregion

	}
}
