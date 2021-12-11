using BioRad.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using WpfTestApp.ViewModelBase;

namespace IDEToolBox.APFInject
{
    public class APFInjectViewModel : ObservableObject
    {
        private enum Result
        {
            Cancel = 0,
            ReadOnlyNo = 0x1,
            ReadOnlyYes = 0x2,
            ReadOnlyNoAll = 0x4,
            ReadOnlyYesAll = 0x8,
            OverwriteNo = 0x10,
            OverwriteYes = 0x20,
            OverwriteNoAll = 0x40,
            OverwriteYesAll = 0x80,
            ReadOnlyMask = 0xf,
            OverwriteMask = 0xf0
        }

        private const string c_ApplyDoubleSigmoidRule = "ApplyDoubleSigmoidRule";
        private const string c_DoubleSigmoidCtCutOff = "DoubleSigmoidCtCutOff";


        private ObservableCollection<String> _logList = new ObservableCollection<string>();

        private string m_ResultPath;
        private string m_SourcePath;
        private bool _applyDoubleSigmoidRule = true;
        private bool _applySlopeRule = true;
        private string _doubleSigmoidCtCutOff = "4000";
        private string _colorString = "-7354887";
        private API m_API;
        private int _version = 2;
        /// <summary>
        /// 
        /// </summary>
        public APFInjectViewModel()
        {
            LoadSettings();
        }
        private bool _removeUnzipFolder = false;
        public bool RemoveUnzipFolder
        {
            get { return _removeUnzipFolder; }
            set
            {
                _removeUnzipFolder = value;
                Properties.Settings.Default.RemoveUnzipFolder = _removeUnzipFolder;
                RaisePropertyChangedEvent("RemoveUnzipFolder");
            }
        }
        public string Version => $"version: {_version}";        
        /// <summary>
        /// Apply Double Sigmoid Rule
         /// </summary>
        public bool ApplyDoubleSigmoidRule
        {
            get { return _applyDoubleSigmoidRule; }
            set { _applyDoubleSigmoidRule = value;
                Properties.Settings.Default.ApplyDoubleSigmoid = _applyDoubleSigmoidRule;
                RaisePropertyChangedEvent("ApplyDoubleSigmoidRule"); }
        }
        private bool _enableAssayColor = false;
        /// <summary>
        /// _enableAssayColor
        /// </summary>
        public bool EnableAssayColor
        {
            get { return _applyDoubleSigmoidRule; }
            set
            {
                _enableAssayColor = value;
                Properties.Settings.Default.EnableAssayColor = _enableAssayColor;
                RaisePropertyChangedEvent("EnableAssayColor");
            }
        }
        /// <summary>
        /// Apply Double Sigmoid Rule
        /// </summary>
        public bool ApplySlopeRule {
            get { return _applySlopeRule; }
            set { _applySlopeRule = value; RaisePropertyChangedEvent("ApplySlopeRule"); }
        }
        bool _automaticClearLog = true;
        /// <summary>
        /// Automatic Clear Logs
        /// </summary>
        public bool AutomaticClearLog
        {
            get { return _automaticClearLog; }
            set { _automaticClearLog = value; RaisePropertyChangedEvent("AutomaticClearLog"); }
        }
        bool _overwriteOutputFolder = false;
        /// <summary>
        /// Overwrite Output Folder
        /// </summary>
        public bool OverwriteOutputFolder
        {
            get { return _overwriteOutputFolder; }
            set { _overwriteOutputFolder = value; RaisePropertyChangedEvent("OverwriteOutputFolder"); }
        }
        
        /// <summary>
        /// Apply Double Sigmoid Ct cut off
        /// </summary>
        public string DoubleSigmoidCtCutOff {

            get { return _doubleSigmoidCtCutOff; }
            set { _doubleSigmoidCtCutOff = value;
                Properties.Settings.Default.DoubleSigmoidCt = _doubleSigmoidCtCutOff;
                RaisePropertyChangedEvent("DoubleSigmoidCtCutOff"); }
        }
        public string ColorString
        {
            get { return _colorString; }
            set { _colorString = value; RaisePropertyChangedEvent("ColorString"); }
        }
        public ObservableCollection<string> Logs
        {
            get { return _logList; }
            set { 
                _logList = value; 
                RaisePropertyChangedEvent("Logs"); 
            }
        }
        public string ResultOutputPath
        {
            get { return m_ResultPath; }
            set
            {
                if (m_ResultPath != value)
                {
                    m_ResultPath = value;
                    Properties.Settings.Default.DefaultResultPath = value;
                    RaisePropertyChangedEvent("ResultOutputPath");
                }
            }
        }
        public string SourcePath
        {
            get { return m_SourcePath; }
            set
            {
                if (m_SourcePath != value)
                {
                    m_SourcePath = value;
                    Properties.Settings.Default.DefaultSourceFolder = value;
                    RaisePropertyChangedEvent("SourcePath");
                }
            }
        }

        #region Commands
        public ICommand BrowseCommand
        {
            get { return new DelegateCommand(PopBrowseDialog); }
        }
        public ICommand InjectParametersCommand
        {
            get { return new DelegateCommand(InjectParameters); }
        }
        public ICommand ClearLogsCommand
        {
            get { return new DelegateCommand(ClearLogs); }
        }

        //private RelayCommand _addPhoneCommand;
        //public RelayCommand AddPhoneCommand
        //{
        //    get
        //    {
        //        if (_addPhoneCommand == null)
        //        {
        //            _addPhoneCommand = new RelayCommand(
        //                (parameter) => AddPhone(parameter),
        //                (parameter) => IsValidPhone(parameter)
        //            );
        //        }
        //        return _addPhoneCommand;
        //    }
        //}


        #endregion

        #region Methods

        private void LoadSettings()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.DefaultResultPath))
                ResultOutputPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            else
                ResultOutputPath = Properties.Settings.Default.DefaultResultPath;

            if (string.IsNullOrEmpty(Properties.Settings.Default.DefaultSourceFolder))
                SourcePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            else
                SourcePath = Properties.Settings.Default.DefaultSourceFolder;

            DoubleSigmoidCtCutOff = Properties.Settings.Default.DoubleSigmoidCt;
            ApplyDoubleSigmoidRule = Properties.Settings.Default.ApplyDoubleSigmoid;
            RemoveUnzipFolder = Properties.Settings.Default.RemoveUnzipFolder ;
            AutomaticClearLog = Properties.Settings.Default.AutomaticClearLog;
            OverwriteOutputFolder = Properties.Settings.Default.OverwriteOutputFolder;
        }
        private void PopBrowseDialog()
        {
            throw new NotImplementedException();
        }
        private void ClearLogs()
        {
            if (Logs != null)
                Logs.Clear();
        }
        private void PrepareDataFolder(string folderName, bool overwrite)
        {
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            else if (overwrite)
            {
                // delete all files 
                Array.ForEach(Directory.GetFiles(folderName), File.Delete);
            }
        }

        string currentWorkingFile = string.Empty;
        private void InjectParameters()
        {
            if (AutomaticClearLog)
                Logs.Clear();

            currentWorkingFile = string.Empty;
            try
            {
                PrepareDataFolder(ResultOutputPath, OverwriteOutputFolder);


                string targetDir = ResultOutputPath;
                //bool overwrite = true;
                int compressedCount = 0;
                int injectedCount = 0;

                // temp folder for unzip
                string unZipFolder = Path.Combine(SourcePath, "Unzip");
                PrepareDataFolder(unZipFolder, true); 

                foreach (var sourceFile in Directory.GetFiles(SourcePath))
                {
                    currentWorkingFile = sourceFile;
                    string decompressedFileName = Path.Combine(unZipFolder, Path.GetFileName(sourceFile));
                    if (File.Exists(decompressedFileName))
                    {
                        Logs.Add($"({Path.GetFileName(decompressedFileName)}) exists in Unzip folder.");
                        continue;
                    }
                    if (FileUtilities.IsCompressedFile(sourceFile))
                    {
                        (bool, string[]) returnZip = FileUtilities.ExtractFileListFromZipFile(sourceFile, unZipFolder);
                        if (returnZip.Item1)
                        {
                            if (!string.IsNullOrEmpty(returnZip.Item2[0])) // 
                            {
                                char[] charsToTrim = { '\\' };
                                string dataFileName = returnZip.Item2[0].Trim(charsToTrim);


                                if (Path.GetFileName(sourceFile) != dataFileName && File.Exists(Path.Combine(unZipFolder, dataFileName)))
                                //    if (dataFileName == "datafile.pcrd" && File.Exists(Path.Combine(unZipFolder, dataFileName)))
                                {
                                    File.Move(Path.Combine(unZipFolder, dataFileName), decompressedFileName);

                                    Logs.Add($"({Path.GetFileName(sourceFile)}) renamed from output ({dataFileName}).");
                                }
                            }
                            Logs.Add($"({Path.GetFileName(sourceFile)}) decompressed.");
                            compressedCount++;
                        }
                    }
                    else
                    {
                        Logs.Add($"({Path.GetFileName(sourceFile)}) is not decompressed.");
                        System.IO.File.Copy(sourceFile, decompressedFileName);
                    }
                    if (File.Exists(decompressedFileName))
                    {
                        //string newfilename = Path.Combine(ResultOutputPath, $"{Path.GetFileNameWithoutExtension(sourceFile)}_modified.pcrd");
                        //System.IO.File.Move(decompressedFileName, newfilename);
                        InjectElements(decompressedFileName);
                        injectedCount++;
                    }
                }
                Logs.Add($"Total: {compressedCount} files decompressed. {injectedCount} files APF modified");


                if (RemoveUnzipFolder)
                {
                    Directory.Delete(unZipFolder, true);
                }
            }
            //catch (UnauthorizedAccessException ex)
            //{
            //    if ((result & Result.ReadOnlyYesAll) == Result.ReadOnlyYesAll)
            //    {
            //        ResetReadonlyAttribute(sourceFile, ex);
            //        return DecryptFile(sourceFile, SetROResult(result, dr), overwrite);
            //    }
            //    else if ((result & Result.ReadOnlyYes) == Result.ReadOnlyYes || (result & Result.ReadOnlyNo) == Result.ReadOnlyNo)
            //    {
            //        using (ReadonlyDialog rd = new ReadonlyDialog(sourceFile, false))
            //        {
            //            switch (dr = rd.ShowDialog(this))
            //            {
            //                case DialogResult.Yes:
            //                case DialogResult.Retry:
            //                    ResetReadonlyAttribute(sourceFile, ex);
            //                    return DecryptFile(sourceFile, SetROResult(result, dr), overwrite);

            //                //case DialogResult.Ignore:
            //                case DialogResult.Cancel:
            //                    return Result.Cancel;
            //            }
            //        }

            //        return SetROResult(result, dr);
            //    }
            //}
            catch (Exception ex)
            {
                Logs.Add($"InjectParameters->Exception:{currentWorkingFile}-{ex.Message}");
            }

            //return result;
        }
        private string BreakElements(string content, string searchElement, bool applyDoubleSigmoid, string doubleSigmoidCtRFU)
        {
            if (content.IndexOf(searchElement) == -1)
                return content;

            int first = content.IndexOf(searchElement);
            int last = content.LastIndexOf(searchElement);
            int sencond = content.IndexOf(searchElement, first + searchElement.Length);
            if (last == first && sencond == -1) // single APF
            {
                return InjectElement(content, applyDoubleSigmoid, doubleSigmoidCtRFU);
            }
            string content1 = content.Substring(0, sencond);

            string content2 = content.Substring(sencond, content.Length - sencond);

            content = InjectElement(content1, applyDoubleSigmoid, doubleSigmoidCtRFU)
                + BreakElements(content2, searchElement, applyDoubleSigmoid, doubleSigmoidCtRFU);


            return content;
        }
        private string InjectElement(string content, bool applyDoubleSigmoid, string doubleSigmoidCtRFU, string colorString = "-5004587")
        {
            string GetBooleanString(bool flag) { return flag ? "True" : "False"; }

            string version3Element = "<SerVersion>3</SerVersion><AssayName>";
            string version4Element = "<SerVersion>4</SerVersion><AssayName>";

            string applyNewBaseLiningKey = "</ApplyNewBaselining>";
            string doubleSigmoidKey = "<ApplyDoubleSigmoidRule>";
            string applySigmoidAppend = $"<ApplyDoubleSigmoidRule>{GetBooleanString(applyDoubleSigmoid)}</ApplyDoubleSigmoidRule>";
            string reverseApplySigmoidAppend = $"<ApplyDoubleSigmoidRule>{GetBooleanString(!applyDoubleSigmoid)}</ApplyDoubleSigmoidRule>";

            string sigmoidCutOffdKey = "<DoubleSigmoidCtCutOff>";

            string applySigmoidCutOffAppend = $"<DoubleSigmoidCtCutOff>{doubleSigmoidCtRFU}</DoubleSigmoidCtCutOff>";
            string applyNewSigmoidAppend = $"<ApplyDoubleSigmoidRule>{GetBooleanString(applyDoubleSigmoid)}</ApplyDoubleSigmoidRule><DoubleSigmoidCtCutOff>{doubleSigmoidCtRFU}</DoubleSigmoidCtCutOff>";
            string applyAPFColor = $"<Color>{colorString}</Color>";
            if (content.Contains(version3Element)) // upgrade to v4
            {
                content = content.Replace(version3Element, version4Element);
                Logs.Add($"({Path.GetFileName(currentWorkingFile)}) version 3 updated to 4.");
            }

            int first = content.IndexOf(applyNewBaseLiningKey) + applyNewBaseLiningKey.Length;
            if (!content.Contains(doubleSigmoidKey))
            {
                content = content.Insert(first, applyNewSigmoidAppend); // haven't been touched
            }
            else
            {
                content = content.Replace(reverseApplySigmoidAppend, applySigmoidAppend);
                if (!content.Contains(sigmoidCutOffdKey))
                {
                    content = content.Insert(first, applySigmoidCutOffAppend);
                }
            }

            return content;
        }

        private string UpdateColorSection(string section, string colorNumberString)
        {
            string head = "<Color>";
            string tail = "</Color>";
            if (section.IndexOf(head) == -1)
                return section;

            int openPos = section.IndexOf(head);
            int closePos = section.IndexOf(tail); 

            section = $"{section.Substring(0, openPos+head.Length)}{colorNumberString}{section.Substring(closePos, section.Length - closePos)}";
            return section;
        }


        private string ReplaceAPFSection(string content, string colorNumberString)
        {
            string APF_Head = "<APFData";
            string APF_Tail = "</APFData>";
            //string colorElement = "<Color>-5004587</Color>";
            if (content.IndexOf(APF_Head) == -1)
                return content;

            int openPos = content.IndexOf(APF_Head);
            int closePos = content.IndexOf(APF_Tail) + APF_Tail.Length;

            string apfSection = content.Substring(openPos, closePos- openPos);

            string updatedApfSection = UpdateColorSection(apfSection, colorNumberString);


            string content1 = content.Substring(0, openPos);
            string content2 = content.Substring(closePos, content.Length - closePos);
            content = content1 + updatedApfSection + ReplaceAPFSection(content2, colorNumberString);
            return content;

        }
        //private void RexPatternMatching(string content, string pattern)
        //{
        //    string[] sentences =
        //            {
        //                "Put the water over there.",
        //                "They're quite thirsty.",
        //                "Their water bottles broke."
        //            };

        //    string sPattern = "the(ir)?\\s";

        //    foreach (string s in sentences)
        //    {
        //        Console.Write($"{s,24}");

        //        if (System.Text.RegularExpressions.Regex.IsMatch(s, sPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        //        {
        //            Console.WriteLine($"  (match for '{sPattern}' found)");
        //        }
        //        else
        //        {
        //            Console.WriteLine();
        //        }
        //    }






        //    string[] numbers =
        //            {
        //                "123-555-0190",
        //                "444-234-22450",
        //                "690-555-0178",
        //                "146-893-232",
        //                "146-555-0122",
        //                "4007-555-0111",
        //                "407-555-0111",
        //                "407-2-5555",
        //                "407-555-8974",
        //                "407-2ab-5555",
        //                "690-555-8148",
        //                "146-893-232-"
        //            };

        //    sPattern = "^\\d{3}-\\d{3}-\\d{4}$";

        //    foreach (string s in numbers)
        //    {
        //        Console.Write($"{s,14}");

        //        if (System.Text.RegularExpressions.Regex.IsMatch(s, sPattern))
        //        {
        //            Console.WriteLine(" - valid");
        //        }
        //        else
        //        {
        //            Console.WriteLine(" - invalid");
        //        }
        //    }
        //}

        private void InjectElements(string fileName)
        {
            try
            {
                using (var file = new StreamReader(fileName))
                {
                    string content = file.ReadToEnd();
                    {
                        string searchElement = "<SerVersion>3</SerVersion><AssayName>";
                        if (content.IndexOf(searchElement) != -1)
                        {
                            content = BreakElements(content, searchElement, ApplyDoubleSigmoidRule, DoubleSigmoidCtCutOff);

                            #region color replacement
                            content = ReplaceAPFSection(content, ColorString);
                            #endregion

                            string newfilename = Path.Combine(ResultOutputPath, $"{Path.GetFileName(fileName)}");
                            using (System.IO.TextWriter writeFile = new StreamWriter(newfilename))
                            {
                                writeFile.Write(content);
                                writeFile.Close();
                            }
                            Logs.Add($"({Path.GetFileName(fileName)}) updated.");
                        }
                        else
                        {
                            Logs.Add($"({Path.GetFileName(fileName)}) Ignored, becuased it is already updated to Version 4.");
                        }

                    }
                    file.Close();
                }
            }
            catch (Exception ex)
            {
                Logs.Add($"Injectelements-> exception:{fileName}-{ex.Message}");
            }
        }

        #endregion
    }
}
