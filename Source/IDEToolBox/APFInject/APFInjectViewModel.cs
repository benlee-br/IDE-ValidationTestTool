using BioRad.Common;
using System;
using System.Collections.Generic;
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

        private string m_ResultPath;
        private string m_SourcePath;
        private bool _applyDoubleSigmoidRule = true;
        private bool _applySlopeRule = true;
        private string _doubleSigmoidCtCutOff = "4000";
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
                int count = 0;

                foreach (var sourceFile in Directory.GetFiles(SourcePath))
                {
                    currentWorkingFile = sourceFile;
                    bool isSuccess =FileUtilities.ExtractAllFromZipFile(sourceFile, ResultOutputPath);
                    string decompressedFileName = Path.Combine(ResultOutputPath, Path.GetFileName(sourceFile));
                    if (isSuccess && File.Exists(decompressedFileName))
                    {
                        //string newfilename = Path.Combine(ResultOutputPath, $"{Path.GetFileNameWithoutExtension(sourceFile)}_modified.pcrd");
                        //System.IO.File.Move(decompressedFileName, newfilename);
                        InjectElements(decompressedFileName);
                        count++;
                    }
                    rc &= isSuccess;
                }

                if (rc)
                    MessageBox.Show($"{count} files decompress complete.","IDE tools");
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
                MessageBox.Show(currentWorkingFile + ": " + ex.Message, "IDE tools");
            }

            //return result;

        }

        private void InjectElements(string fileName)
        {
            string version3Element = "<SerVersion>3</SerVersion><AssayName>";
            string version4Element = "<SerVersion>4</SerVersion><AssayName>";


            string applyNewBaseLining = "<ApplyNewBaselining>True</ApplyNewBaselining>";
            string applySigmoidAppend = "<ApplyDoubleSigmoidRule>True</ApplyDoubleSigmoidRule>";
            string applySigmoidCutOffAppend = "<DoubleSigmoidCtCutOff>4000</DoubleSigmoidCtCutOff>";

            string applyNewSigmoidAppend = "<ApplyDoubleSigmoidRule>True</ApplyDoubleSigmoidRule><DoubleSigmoidCtCutOff>4000</DoubleSigmoidCtCutOff>";



            //private const string c_ApplyDoubleSigmoidRule = "ApplyDoubleSigmoidRule";
            //private const string c_DoubleSigmoidCtCutOff = "DoubleSigmoidCtCutOff";
            List<string> entries = new List<string>();

            try
            {
                using (var file = new StreamReader(fileName))
                {
                    string content = file.ReadToEnd();
                    {
                        if (content.Contains(version3Element))
                        {
                            content = content.Replace(version3Element, version4Element);
                            int first = content.IndexOf(applyNewBaseLining) + applyNewBaseLining.Length;

                            if (!content.Contains(applySigmoidAppend))
                            {
                                content = content.Insert(first, applyNewSigmoidAppend);
                            }
                            else if (!content.Contains(applySigmoidCutOffAppend))
                            {

                                content = content.Insert(first, applySigmoidCutOffAppend);
                            }
                            string newfilename = Path.Combine(ResultOutputPath, $"{Path.GetFileNameWithoutExtension(fileName)}_modified.pcrd");
                            using (System.IO.TextWriter writeFile = new StreamWriter(newfilename))
                            {
                                writeFile.Write(content);
                                writeFile.Close();
                            }
                        }
                    }
                    file.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"InjectElements exception:{ex.Message}");
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
