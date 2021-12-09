using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;

namespace BioRad.Common.UIUtils
{
    #region Documentation Tags
    /// <summary>
    /// This class defines all topic IDs from robohelp help file(chm).
    /// It provides method to set the topic ID for given control.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <classinformation>
    ///		<list type="bullet">
    ///			<item name="authors">Authors:Pramod Walse</item>
    ///			<item name="review">Last design/code review:10/03/06</item>
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
    ///			<item name="vssfile">$Workfile: HelpProviderUtility.cs $</item>
    ///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/UIUtils/HelpProviderUtility.cs $</item>
    ///			<item name="vssrevision">$Revision: 14 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Blie $</item>
    ///			<item name="vssdate">$Date: 8/19/10 2:22p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    public partial class HelpProviderUtility
    {
        #region Constants
        #endregion

        #region Member Data
        private static HelpProvider m_HelpProvider = null;
        /// <summary>Help Topic: Startup Wizard Form</summary>
        public readonly static string StartUpForm_HelpTopicID = "101";
        /// <summary>Help Topic: Experiment Setup Form</summary>
        public readonly static string ExpSetupForm_HelpTopicID = "201";
        /// <summary>Help Topic: Event Log Form</summary>
        public readonly static string LogForm_HelpTopicID = "1505";
        /// <summary>Help Topic: Event Log Form</summary>
        public readonly static string AuditTrailForm_HelpTopicID = "1505";
        /// <summary>Help Topic: Change Password Log Form</summary>
        public readonly static string ChangePasswordForm_HelpTopicID = "1605";
        /// <summary>Help Topic: Data Analysis Form</summary>
        public readonly static string DataAnalysis_HelpTopicID = "600";
        /// <summary>Help Topic: Dye Calibration Form</summary>
        public readonly static string DyeCalibration_HelpTopicID = "403";

        /// <summary>Help Topic: File Info Form</summary>
        public readonly static string FileInfo_HelpTopicID = "601";
        /// <summary>Help Topic: Analysis Tool Form</summary>
        public readonly static string AnalysisTool_HelpTopicID = "602";
        /// <summary>Help Topic: Gene Study Form</summary>
        public readonly static string GeneStudy_HelpTopicID = "800";
        /// <summary>Help Topic: Instrument Summary Form</summary>
        public readonly static string InstrumentSummary_HelpTopicID = "1204";
        /// <summary>Help Topic: Login Form</summary>
        public readonly static string Login_HelpTopicID = "1603";
        /// <summary>Help Topic: Plate Editor Form</summary>
        public readonly static string PlateEditor_HelpTopicID = "500";
        /// <summary>Help Topic: Print Form</summary>
        public readonly static string Print_HelpTopicID = "1503";
        /// <summary>Help Topic: Print Preview Form</summary>
        public readonly static string PrintPreview_HelpTopicID = "1504";
        /// <summary>Help Topic: Protocol Editor Form</summary>
        public readonly static string ProtocolEditor_HelpTopicID = "301";
        /// <summary>Help Topic: Run Log Form</summary>
        public readonly static string RunLog_HelpTopicID = "1501";
        /// <summary>Help Topic: Runtime Optimizer Form</summary>
        public readonly static string RuntimeOptimizer_HelpTopicID = "304";
        /// <summary>Help Topic: Select Fluorophore Form</summary>
        public readonly static string SelectFluor_HelpTopicID = "404";
        /// <summary>Help Topic: Select User Form</summary>
        public readonly static string SelectUser_HelpTopicID = "1604";
        /// <summary>Help Topic: User Preference Form</summary>
        public readonly static string UserPref_HelpTopicID = "1601";
        /// <summary>Help Topic: User Admin Form</summary>
        public readonly static string UserAdmin_HelpTopicID = "1602";
        /// <summary>Help Topic: Well Info Form</summary>
        public readonly static string WellInfo_HelpTopicID = "402";
        /// <summary>Help Topic: Zip Data Files Form</summary>
        public readonly static string ZipDataFiles_HelpTopicID = "1502";
        /// <summary>Help Topic: Calibration Wizard Form</summary>
        public readonly static string CalibrationWizard_HelpTopicID = "408";
        /// <summary>Help Topic: Well Group Manager Form</summary>
        public readonly static string WellGroupManager_HelpTopicID = "405";
        /// <summary>Help Topic: Experiment Settings Form</summary>
        public readonly static string PlateExperimentSettings_HelpTopicID = "407";
        /// <summary>Help Topic: Plate Spreadsheet View Form</summary>
        public readonly static string PlateSpreadsheetView_HelpTopicID = "406";
        /// <summary>Help Topic: Plate Spreadsheet View Form</summary>
        public readonly static string ReportView_HelpTopicID = "900";
        /// <summary>Help Topic: Master Mix Form</summary>
        public readonly static string MasterMix_HelpTopicID = "700";
        /// <summary>Help Topic : Gene Expression Formula Definitions.</summary>
        public readonly static string GeneExpressionFormulaDefinitions_HelpTopicID = "650";
        /// <summary>Help Topic : Anova Calculations.</summary>
		public readonly static string AnovaCalculations_HelpTopicID = "1603";
        #endregion

        #region Accessors
        #endregion

        #region Delegates and Events
        #endregion

        #region Constructors and Destructor

        /// <summary>Initializes a new instance of the HelpProviderUtility class.</summary>
        public HelpProviderUtility() { }

        #endregion

        #region Methods
        /// <summary>
        /// Sets the help topic ID for given control.
        /// </summary>
        /// <param name="control">Windows Control</param>
        /// <param name="topicID">Tpoic ID as defined in chm file.</param>
        public static void AssignHelpID(Control control, string topicID)
        {
            string helpFileName = GetHelpFileName();

            //PW: Must initialize every time as there are memory leak issues for static object.
            HelpProvider helpProvider = new HelpProvider();
            helpProvider.HelpNamespace = helpFileName;
            helpProvider.SetHelpNavigator(control, HelpNavigator.TopicId);
            helpProvider.SetHelpKeyword(control, topicID);
        }

        /// <summary>
		/// Sets the help topic for given control.
		/// </summary>
		/// <param name="control">Windows Control</param>
		/// <param name="topic">Tpoic ID as defined in chm file.</param>
		public static void AssignHelpTopic(Control control, string topic)
        {
            string helpFileName = GetHelpFileName();

            //PW: Must initialize every time as there are memory leak issues for static object.
            HelpProvider helpProvider = new HelpProvider();
            helpProvider.HelpNamespace = helpFileName;
            helpProvider.SetHelpNavigator(control, HelpNavigator.Topic);
            helpProvider.SetHelpKeyword(control, topic);
        }
        /// <summary>
        /// Gets help provider instance for the application.
        /// </summary>
        /// <returns>Static instance of HelpProvider object</returns>
        public static HelpProvider GetApplicationHelpProvider()
        {
            if (m_HelpProvider == null)
            {
                m_HelpProvider = new HelpProvider();
            }
            if (m_HelpProvider.HelpNamespace == null)
            {
                //Initialize help provider
                string helpFileName = GetHelpFileName();
                m_HelpProvider.HelpNamespace = helpFileName;
            }
            return m_HelpProvider;
        }
        /// <summary>
        /// Gets help provider instance for the application.
        /// </summary>
        /// <returns></returns>
        public static string GetHelpNamespace()
        {
            return GetHelpFileName();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string getCultureSpecificFilename(string fileName)
        {
            CultureInfo uiCulture = Thread.CurrentThread.CurrentUICulture;
            string newName = fileName;
            string filenameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            string fileNameWithLocalizedString
                = string.Concat(
                filenameWithoutExt,
                "_", //DNL
                uiCulture.Name,
                extension
                );

            return fileNameWithLocalizedString;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        static public bool foundLocalMachineRegistryKey(string location, out string content)
        {
            content = String.Empty;
            try
            {
                //Following code seem to be working.
                //Example: HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Bio-Rad\CFXDoc\CFXSTD_1.0
                //32-Bit app running in 64-Bit OS pointing to Wow64 (not using RegistryView.Registry64);
                var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);

                using (RegistryKey key = hklm.OpenSubKey(location))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue("Path");
                        if (o != null)
                        {
                            content = (string)o;
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Load specified documentation text file. Set FileExist to false in case of failure
        /// If a localized version of the file (such as chinese or chinese version exist), load that file instead
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="fileExist"></param>
        /// <returns>lines contained in the file or null in case of failure</returns>
        static public string [] loadDocumentationFile(string filename,out bool fileExist)
        {
            fileExist = false;
            string [] content=null;

            string errorMessage = String.Empty;
            string versionFileFullPath = string.Empty;
            if (BioRad.Common.UIUtils.HelpProviderUtility.findLocalizedDocument(filename, 
                out versionFileFullPath, out errorMessage))
            {

                if (File.Exists(versionFileFullPath))
                {
                    fileExist = true;
                    try
                    {
                        content = File.ReadAllLines(versionFileFullPath);
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return content;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathFound"></param>
        /// <returns></returns>
        static public bool pathToAdditionalDocumentation(out string pathFound)
        {
            pathFound = String.Empty;

            try
            {
                string rootLocation = String.Empty;

                string link = String.Empty;
                if (ApplicationStateData.GetInstance.IsFSDApplication)
                {
                    link = ApplicationStateData.GetInstance[ApplicationStateData.Setting.UserDocsLinkIDE];
                }
                else if (ApplicationStateData.GetInstance.IsMacApplication)
                {
                    link = ApplicationStateData.GetInstance[ApplicationStateData.Setting.UserDocsLinkMAC];
                }
                else
                {
                    link = ApplicationStateData.GetInstance[ApplicationStateData.Setting.UserDocsLinkSTD];
                }

                if (foundLocalMachineRegistryKey(link, out pathFound))
                {
                    return true;
                }
                
            }
            catch (Exception)
            {                
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subfolder"></param>
        /// <param name="docLocation"></param>
        /// <param name="expectedLocation"></param>
        /// <returns>false if path cannot be found</returns>
        static private bool pathToSeparateInstallerFolder(string subfolder, bool docLocation, out string expectedLocation)
        {
            expectedLocation = String.Empty;
            string fullPath = String.Empty;
            string root = String.Empty;
            if (docLocation)
            {
                //Path to CFXDoc folder
                if (!pathToAdditionalDocumentation(out root))
                {
                    return false;
                }
            }
            else
            {
                //Path to CFX folder
                root = AppDomain.CurrentDomain.BaseDirectory;                
            }
            
            expectedLocation= Path.Combine(root, subfolder);
            return true;
        }


        /// <summary>
        /// 
        /// </summary>        
        /// <param name="docLocation"></param>        
        /// <returns></returns>
        static public string pathTodocumentFolder(bool docLocation)
        {
            string expectedLocation = String.Empty;
            string subFolder = "Documentation"; //DNL Do not localize

            pathToSeparateInstallerFolder(subFolder, docLocation, out expectedLocation);
            return expectedLocation;
        }

        /// <summary>
        /// Look if documentName exists under C:\Program Files (x86)\Bio-Rad\CFXDoc\CFX\Documentation   when docLocation==true
        /// then                              C:\Program Files (x86)\Bio-Rad\CFX\Documentation          when docLocation==false
        /// </summary>
        /// <param name="documentName"></param>
        /// <param name="docLocation"></param>
        /// <param name="expectedLocation"></param>
        /// <returns></returns>
        static private bool documentExistsAtLocation(string documentName, bool docLocation, out string expectedLocation)
        {
            expectedLocation = String.Empty;
            try
            {
                //Try to find document in separate Doc installer folder (when docLocation==true)
                //By default: C:\Program Files (x86)\Bio-Rad\CFXDoc\CFX\Documentation
                //            C:\Program Files (x86)\Bio-Rad\CFXDoc\CFXIDE\Documentation
                //            C:\Program Files (x86)\Bio-Rad\CFXDoc\CFXMAC\Documentation

                //Try to find document under EXE folder: (when docLocation==false)
                //By default: C:\Program Files (x86)\Bio-Rad\CFX\Documentation
                //            C:\Program Files (x86)\Bio-Rad\CFXIDE\Documentation
                //            C:\Program Files (x86)\Bio-Rad\CFXMAC\Documentation
                string path = pathTodocumentFolder(docLocation);
                expectedLocation = Path.Combine(path, documentName);

                if (File.Exists(expectedLocation)) return true;                
            }
            catch (Exception)
            {                
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public string documentationErrorMessage()
        {
            string message = String.Format(Properties.Resources.UserGuideNotFound2);
            return message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        static public string documentationErrorMessageWithURL( out string url)
        {
            string message = String.Empty;
            //TODO Seem like the Bio-Rad software updates page is the best place to go for now...
            url = "http://www.bio-rad.com/en-us/product/firmware-software-updates";//DO not localize

            //@"The user documentation is missing or out of date. "
            //        + "Please install the latest documentation (CFX_Documentation.EXE) "
            //        + "from the Bio-Rad website: {0}."
            //        + " Would you like to visit the download page now?", url);
            message = String.Format(Properties.Resources.UserGuideNotFoundWithURL, url);

            return message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        static public string documentationErrorMessage(string message)
        {
            //Error message is now more generic, as it may not be a user guide. 
            //Also removed any reference to Maestro, as it also displayed for IDE version
            //The message was blessed by Theresa 1/26/2017
            string returnedMessage = String.Empty;
            if (String.IsNullOrEmpty(message))
            {
                //Fix for defect 1012-[Regression] CFX Help -> Additional Documentation dialog has no text
                returnedMessage =
                String.Format(Properties.Resources.UserGuideNotFoundParam, message); 
            }
            else
            {
                //Fix for defect 1011-Help: Message has 2 spaces between the end of the sentence and the period
                returnedMessage = Properties.Resources.UserGuideNotFound; 
            }
            return returnedMessage;
        }
        /// <summary>
        /// Look for localized version of the provided "documentName". If none found, falls back to "documentName"
        /// Example: if documentName="Help.chm", look for Help.Ru-ru.chm or Help.Cz-cz.chm (assuming Russian or Chinese)
        /// Default back to "Help.chm" is no file above found
        /// </summary>
        /// <param name="documentName"></param>
        /// <param name="locationToUse"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        static public bool findLocalizedDocument(string documentName, out string locationToUse, out string errorMessage)
        {
            errorMessage = String.Empty;
            locationToUse = String.Empty;
            
            //Check if a localized version of the specified documentName exists
            //(ending for example with Ru-ru.* or CH-ch.*)
            //If one is found that match current locale, use it, otherwise defaults to english version
            string localizedFilename = BioRad.Common.UIUtils.HelpProviderUtility.getCultureSpecificFilename(documentName);

            //Check if there is a localized version of the document such as "UserGuide-ru-ru.pdf" of "UserGuide.pdf in additional user doc folder
            if (BioRad.Common.UIUtils.HelpProviderUtility.documentExistsAtLocation(localizedFilename, true, out locationToUse)) return true;

            //Check if there is a non localized version of the document in additional user doc folder
            if (BioRad.Common.UIUtils.HelpProviderUtility.documentExistsAtLocation(documentName, true, out locationToUse)) return true;

            //Check if there is a localized version of the document such as "UserGuide-ru-ru.pdf" of "UserGuide.pdf in CFX EXE documentation folder
            if (BioRad.Common.UIUtils.HelpProviderUtility.documentExistsAtLocation(localizedFilename, false, out locationToUse)) return true;

            //Check if there is a non localized version of the document in CFX EXE documentation folder
            if (BioRad.Common.UIUtils.HelpProviderUtility.documentExistsAtLocation(documentName,false, out locationToUse)) return true;

            //If not found anywhere, display a simple error message that does not prompt user to browse to the Bio-Rad update URL
            errorMessage = documentationErrorMessage(documentName);

            return false;
        }

        
        /// <summary>
        /// Attempt to open document in Windows (for example a PDF)
        /// </summary>
        /// <param name="fullFilename"></param>
        /// <returns></returns>
        static public bool showDocument(string fullFilename)
        {
            try
            {
                Process.Start(fullFilename);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        static public bool openFolder(string folderName)
        {
            try
            {
                if (!Directory.Exists(folderName))
                {
                    //If folderName specifies a filename (not a directory)
                    //get the corresponding path				
                    folderName = Path.GetDirectoryName(folderName);
                }
                string windir = Environment.GetEnvironmentVariable("WINDIR"); //DNL
                System.Diagnostics.Process prc = new System.Diagnostics.Process();
                prc.StartInfo.FileName = windir + @"\explorer.exe"; //DNL
                prc.StartInfo.Arguments = "\"" + folderName + "\"";               
                prc.Start();
            }
            catch (Exception ex)
            {
                string ignoredMessage = ex.ToString();                
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestSecurityEditionDocument"></param>
        /// <returns></returns>
        static public string GetUserGuideNameFromConfig(bool requestSecurityEditionDocument)
        {
            string documentName = String.Empty;

            if (ApplicationStateData.GetInstance.IsRegulatory)
            {
                //TT 1151-Users cannot access standard user guide in Security mode
                if (requestSecurityEditionDocument)
                {
                    documentName = ApplicationStateData.GetInstance[
                         ApplicationStateData.Setting.UserGuideFileSE];
                }
                else
                {
                    documentName = documentName = ApplicationStateData.GetInstance[ApplicationStateData.Setting.UserGuideFileSTD];
                }
            }
            else
            {
                if (ApplicationStateData.GetInstance.IsFSDApplication)
                {
                    documentName = documentName = ApplicationStateData.GetInstance[ApplicationStateData.Setting.UserGuideFileIDE];
                }
                else if (ApplicationStateData.GetInstance.IsMacApplication)
                {
                    documentName = documentName = ApplicationStateData.GetInstance[ApplicationStateData.Setting.UserGuideFileMAC];
                }
                else                
                {
                    documentName = documentName = ApplicationStateData.GetInstance[ApplicationStateData.Setting.UserGuideFileSTD];
                }

                string expectedLocation = String.Empty;

                
            }
            return documentName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public string GetEULAFilename()
        {
            string documentName = String.Empty;
            
            if (ApplicationStateData.GetInstance.CanDoRealTime)
            {
                //Fix for defect 2134- 3.1 Security Edition can't find EULA via Help -> About -> View Eula
                //Now use the same PDF for both the standard and security Edition
                documentName = "CFX_EULA.pdf";      //DO NOT LOCALIZE
            }
            else
            {
                documentName = "CFX_EULA_C1000.pdf"; //DO NOT LOCALIZE
            }

            //string eulaFileName = Path.Combine(ApplicationPath.DirectoryPath, "Documentation");
            //eulaFileName = Path.Combine(eulaFileName, documentName);
            return documentName;// eulaFileName;
        }

        private static string GetHelpFileName()
		{
            string shortFileName = String.Empty;
            string applicationPath = ApplicationPath.DirectoryPath;

            //Task 2091 - Please add and apply security edition help file in daily CFX Build.
            //(ST) 2013/07/01
            //We now open the Security Edition CHM file if in Security Mode,
            //and open standard edition if no dongle is present

            if (ApplicationStateData.GetInstance.IsRegulatory)
            {
                shortFileName = ApplicationStateData.GetInstance[
                 ApplicationStateData.Setting.OnlineHelpFileSE];
            }
            else
            {
                shortFileName = ApplicationStateData.GetInstance[
                 ApplicationStateData.Setting.OnlineHelpFile];
            }
            
            string newLocation = String.Empty;
            string ignoredErrorMessage = String.Empty;
            if (findLocalizedDocument(shortFileName, out newLocation,out ignoredErrorMessage)) //TODO: have option to not look for localized version?
            {
                //It is ok if the file is not found                
            }

			return newLocation;

		}
		#endregion

		#region Event Handlers
		#endregion
	}
}
