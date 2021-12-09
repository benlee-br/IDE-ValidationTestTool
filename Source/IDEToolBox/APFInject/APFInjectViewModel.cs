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
        private string _doubleSigmoidCtCutOff = "4003";
        private string _colorString = "#E7E44D";
        private API m_API;
        /// <summary>
        /// 
        /// </summary>
        public APFInjectViewModel()
        {
            LoadSettings();
        }

        /// <summary>
        /// Apply Double Sigmoid Rule
        /// </summary>
        public bool ApplyDoubleSigmoidRule
        {
            get { return _applyDoubleSigmoidRule; }
            set { _applyDoubleSigmoidRule = value; RaisePropertyChangedEvent("ApplyDoubleSigmoidRule"); }
        }

        /// <summary>
        /// Apply Double Sigmoid Rule
        /// </summary>
        public bool ApplySlopeRule {
            get { return _applySlopeRule; }
            set { _applySlopeRule = value; RaisePropertyChangedEvent("ApplySlopeRule"); }
        }


        /// <summary>
        /// Apply Double Sigmoid Ct cut off
        /// </summary>
        public string DoubleSigmoidCtCutOff {

            get { return _doubleSigmoidCtCutOff; }
            set { _doubleSigmoidCtCutOff = value; RaisePropertyChangedEvent("DoubleSigmoidCtCutOff"); }
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
                    RaisePropertyChangedEvent("ResultPath");
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

        private bool CheckFileCompressed(string fileName)
        {
            bool isCompressed = false;
            using (var file = new StreamReader(fileName))
            {
                string content = file.ReadLine();

                if (!string.IsNullOrEmpty(content) && content.Length > 10 && content[0] == 'P' && content[1] == 'K')
                    isCompressed = true;
            }
            return isCompressed;
        }
        private void InjectParameters()
        {

            string currentWorkingFile = string.Empty;
            try
            {
                if (!Directory.Exists(ResultOutputPath))
                {
                    Directory.CreateDirectory(ResultOutputPath);
                }
                string targetDir = ResultOutputPath;
                //bool overwrite = true;
                bool rc = true;
                int compressedCount = 0;
                int injectedCount = 0;

                foreach (var sourceFile in Directory.GetFiles(SourcePath))
                {
                    currentWorkingFile = sourceFile;
                    string decompressedFileName = Path.Combine(ResultOutputPath, Path.GetFileName(sourceFile));
                    if (CheckFileCompressed(sourceFile))
                    {
                        if (FileUtilities.ExtractAllFromZipFile(sourceFile, ResultOutputPath))
                        {
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
                Logs.Add($"Total: {compressedCount} files decompressed. {injectedCount} files APF hacked");
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


        private string InjectElement(string content, bool applyDoubleSigmoid, string doubleSigmoidCtRFU)
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

            if (content.Contains(version3Element)) // upgrade to v4
                content = content.Replace(version3Element, version4Element);

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
                            string newfilename = Path.Combine(ResultOutputPath, $"{Path.GetFileNameWithoutExtension(fileName)}_modified.pcrd");
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
        }
        private void PopBrowseDialog()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>

}
