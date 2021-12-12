using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using WpfTestApp.ViewModelBase;

namespace IDEToolBox.CombineCSVs
{
    class CombineResultViewModel : ObservableObject
    {
        #region Members
        const string Title = "Combine Files From Two Folders";

        string[] Header = new string[] { "", "Well", "", "", "FAM Cq", "Cy5 Cq", "Cq", "I.C. Cq", "", "Result" };
        enum ColumnName { None, Well, Content, Sample, cq1, cq2, cq3, ICCq, SQ, Result }

        object lockme = new object();
        public List<ClsSheetInfo> lstSheet = null;
        public Microsoft.Office.Interop.Excel.Application xlApp = null;
        public Microsoft.Office.Interop.Excel.Workbook xlWorkbook = null;  //.Open(FileName);
        public Microsoft.Office.Interop.Excel._Worksheet xlWorksheet;
        private int _version = 9;

        private string m_ResultPath;
        private string m_DataPath;
        private string _folder1Path;
        private string _folder2Path;

        private string _progressFileCount = "0/0";
        private string _progressFileName = "None";
        private int _progressValue = 0;
        #endregion

        #region Properties
        public string Form_Title => Title;
        public string ProgressFileCount
        {
            get { return _progressFileCount; }
            set
            {
                if (_progressFileCount != value)
                {
                    _progressFileCount = value;
                    RaisePropertyChangedEvent("ProgressFileCount");
                }
            }
        }
        public int ProgressValue
        {
            get { return _progressValue; }
            set
            {
                if (_progressValue != value)
                {
                    _progressValue = value;
                    RaisePropertyChangedEvent("ProgressValue");
                }
            }
        }

        public string ProgressFileName
        {
            get { return _progressFileName; }
            set
            {
                if (_progressFileName != value)
                {
                    _progressFileName = value;
                    RaisePropertyChangedEvent("ProgressFileName");
                }
            }
        }
        public string Folder1Path {
            get { return _folder1Path; }
            set
            {
                if (_folder1Path != value)
                {
                    _folder1Path = value;
                    RaisePropertyChangedEvent("Folder1Path");
                }
            }
        }
        public string Folder2Path
        {
            get { return _folder2Path; }
            set
            {
                if (_folder2Path != value)
                {
                    _folder2Path = value;
                    RaisePropertyChangedEvent("Folder2Path");
                }
            }
        }

        public string Version => $"Version: {_version}";
        public string ResultPath
        {
            get { return m_ResultPath; }
            set
            {
                if (m_ResultPath != value)
                {
                    m_ResultPath = value;
                    Properties.Settings.Default.DefaultResultPath = value;
                    RaisePropertyChangedEvent("RemoveUnzipFolder");
                }
            }
        }
        public string DataPath
        {
            get { return m_DataPath; }
            set
            {
                if (m_DataPath != value)
                {
                    m_DataPath = value;
                    Properties.Settings.Default.DefaultCompareFolder = value;
                }
            }
        }
        #endregion
        public CombineResultViewModel()
        {
            LoadSettings();
        }

        #region Methods
        private void LoadSettings()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.DefaultCombineResultPath))
                ResultPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            else
                ResultPath = Properties.Settings.Default.DefaultCombineResultPath;

            if (string.IsNullOrEmpty(Properties.Settings.Default.DefaultCompareFolder))
                DataPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            else
                DataPath = Properties.Settings.Default.DefaultCompareFolder;
        }
        public void AddToSheet(string SheetSampleName, CombineFileInfo oFile, List<ClsSheetInfo> lstSheet, Microsoft.Office.Interop.Excel.Workbook xlWorkbook, bool NoDiff = false)
        {
            lock (lockme)
            {
                Microsoft.Office.Interop.Excel._Worksheet xlWorksheet;
                ClsSheetInfo oSheet = SearchSampleSheets(lstSheet, SheetSampleName);
                if (oSheet == null)
                {
                    xlWorkbook.Worksheets.Add();
                    //xlWorksheet = xlWorkbook.Sheets[xlWorkbook.Sheets.Count];
                    xlWorksheet = xlWorkbook.Sheets[1];
                    oSheet = new ClsSheetInfo(SheetSampleName, xlWorksheet.Name, 2);
                    lstSheet.Add(oSheet);
                    PopulateDataForSheetHeader(xlWorksheet);
                }
                else
                {
                    xlWorksheet = xlWorkbook.Sheets[oSheet.SheetName];
                }
                oFile.FileNameRow = oSheet.Row;
                oSheet.lstFile.Add(oFile);
                PopulateDataForSheet(xlWorksheet, oSheet, oFile, NoDiff);
            }

        }
        private ClsSheetInfo SearchSampleSheets(List<ClsSheetInfo> lstSheets, string SampleName)
        {

            foreach (ClsSheetInfo oSheetInfo in lstSheets)
            {
                if (oSheetInfo.SampleName.ToLower() == SampleName.ToLower())
                {
                    return oSheetInfo;
                }
            }
            return null;
        }
        private void PopulateDataForSheetHeader(Microsoft.Office.Interop.Excel._Worksheet xlWorksheet)
        {

            xlWorksheet.Cells[1, 3].Value = Folder1Path;
            xlWorksheet.Cells[1, 3].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[1, 13].Value = Folder2Path;
            xlWorksheet.Cells[1, 3].EntireRow.Font.Bold = true;

        }
        private void PopulateDataForSheet(Microsoft.Office.Interop.Excel._Worksheet xlWorksheet, ClsSheetInfo oSheetInfo, CombineFileInfo oFileInfo, bool NoDiff = false)
        {
            if (NoDiff) return;
            int row = oSheetInfo.Row;
            int colGroup1 = oSheetInfo.colGroup1;
            int colGroup2 = oSheetInfo.colGroup2;

            xlWorksheet.Cells[row, colGroup1].Value = oFileInfo.FileName;
            xlWorksheet.Cells[row, colGroup2].Value = oFileInfo.FileName;

            xlWorksheet.Cells[row, colGroup1].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, colGroup2].EntireRow.Font.Bold = true;


            //read header from file
            System.IO.StreamReader sr = new System.IO.StreamReader(oFileInfo.FileFullName1);
            string sline = sr.ReadLine();
            string[] Title = sline.Split(',');
            sr.Close();

            row += 1;
            //first group header
            int colheader = 0;
            for (int j = 0; j < Header.Length; j++)
            {
                if (Header[j] != "")
                {
                    xlWorksheet.Cells[row, colGroup1 + colheader].Value = Title[j];
                    colheader++;
                }

            }

            //second group header
            colheader = 0;
            for (int j = 0; j < Header.Length; j++)
            {
                if (Header[j] != "")
                {
                    xlWorksheet.Cells[row, colGroup2 + colheader].Value = Title[j];
                    colheader++;
                }

            }
            //if (NoDiff)
            //{
            //    row += 1;
            //    xlWorksheet.Cells[row, colGroup1].Value = "No Difference";
            //    xlWorksheet.Cells[row, colGroup2].Value = "No Difference";
            //    xlWorksheet.Cells[row, colGroup2].EntireRow.Font.Bold = true;
            //    row += 2;
            //    oSheetInfo.Row = row;
            //    return;
            //}


            int colDelta = colGroup2 + 7;
            xlWorksheet.Cells[row, colDelta + 1].Value = "Delta " + Title[(int)ColumnName.cq1];
            xlWorksheet.Cells[row, colDelta + 2].Value = "Delta " + Title[(int)ColumnName.cq2];
            xlWorksheet.Cells[row, colDelta + 3].Value = "Delta " + Title[(int)ColumnName.cq3];
            xlWorksheet.Cells[row, colDelta + 4].Value = "Delta " + Title[(int)ColumnName.ICCq];

            row += 1;

            int rowsinFile;
            rowsinFile = PopulateData(xlWorksheet, row, colGroup1, oFileInfo.FileFullName1, oFileInfo);
            rowsinFile = PopulateData(xlWorksheet, row, colGroup2, oFileInfo.FileFullName2, oFileInfo);

            oFileInfo.NumberOfSample = rowsinFile;

            row += 2 + rowsinFile;

            oSheetInfo.Row = row;
        }
        public void PopulateDataForSheetSummary(Microsoft.Office.Interop.Excel._Worksheet xlWorksheet, ClsSheetInfo oSheet, List<CombineFileInfo> lstFile, bool isDiffSheet = false)
        {

            //Write Total number
            int row = oSheet.Row;
            int colGroup1 = oSheet.colGroup1;
            int colGroup2 = oSheet.colGroup2;

            int col = colGroup2 + 12;
            xlWorksheet.Cells[1, col].Value = "Number of Samples";
            xlWorksheet.Cells[1, col + 1].Value = "Number of Difference";
            foreach (CombineFileInfo oFileInfo in lstFile)
            {
                xlWorksheet.Cells[oFileInfo.FileNameRow, col].Value = oFileInfo.NumberOfSample;
                xlWorksheet.Cells[oFileInfo.FileNameRow, col + 1].Value = oFileInfo.NumberOfDifferent;
            }

            //Write Summary
            xlWorksheet.Cells[row, 1].Value = "Summary";
            row++;
            col = 8;
            xlWorksheet.Cells[row, 1].Value = "File Name";
            xlWorksheet.Cells[row, col].Value = "Number of Samples";
            xlWorksheet.Cells[row, col + 1].Value = "Number of Difference";
            int countDiff = 0;
            foreach (CombineFileInfo oFileInfo in lstFile)
            {
                if (oFileInfo.NumberOfDifferent == 0 && isDiffSheet) continue;
                countDiff++;
                row++;
                xlWorksheet.Cells[row, 1].Value = oFileInfo.FileName;
                xlWorksheet.Cells[row, col].Value = oFileInfo.NumberOfSample;
                xlWorksheet.Cells[row, col + 1].Value = oFileInfo.NumberOfDifferent;
                if (oFileInfo.NumberOfDifferent > 0)
                {
                    Microsoft.Office.Interop.Excel.Range oRange = xlWorksheet.Range[xlWorksheet.Cells[row, 1], xlWorksheet.Cells[row, col + 3]].Cells;
                    //oRange.EntireRow.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbLightPink;
                    oRange.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbLightPink;
                    //xlWorksheet.Cells[row, 1].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbRed;
                    //xlWorksheet.Cells[row, col].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbRed;
                    //xlWorksheet.Cells[row, col + 1].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbRed;
                }

            }
            row++;
            xlWorksheet.Cells[row, 1].Value = countDiff;// lstFile.Count.ToString();
            xlWorksheet.Cells[row, col].Value = GetTotalNumberOfSamples(lstFile);
            xlWorksheet.Cells[row, col + 1].Value = GetTotalNumberOfDifference(lstFile);

        }
        private int PopulateData(Microsoft.Office.Interop.Excel._Worksheet xlWorksheet, int row, int col, string sFile, CombineFileInfo oFileInfo)
        {
            string sline;
            string[] sLineData;
            int j = 0;

            System.IO.StreamReader sr = new System.IO.StreamReader(sFile);
            sline = sr.ReadLine();


            sLineData = sline.Split(',');  //header row
            if (sLineData.Length >= 10)
            {
                j = 0;
                int CurrentRow;
                while (sr.EndOfStream == false)
                {
                    sline = sr.ReadLine();
                    if (sline.Trim().Length < 2) continue;
                    sLineData = sline.Split(',');
                    if (sLineData.Length >= 10)
                    {
                        CurrentRow = row + j;
                        xlWorksheet.Cells[CurrentRow, col].Value = sLineData[(int)ColumnName.Well];
                        xlWorksheet.Cells[CurrentRow, col + 1].Value = sLineData[(int)ColumnName.cq1];
                        xlWorksheet.Cells[CurrentRow, col + 2].Value = sLineData[(int)ColumnName.cq2];
                        xlWorksheet.Cells[CurrentRow, col + 3].Value = sLineData[(int)ColumnName.cq3];
                        xlWorksheet.Cells[CurrentRow, col + 4].Value = sLineData[(int)ColumnName.ICCq];
                        xlWorksheet.Cells[CurrentRow, col + 5].Value = sLineData[(int)ColumnName.Result];

                        //for second group, add delta column
                        if (col > 5)
                        {
                            if (xlWorksheet.Cells[CurrentRow, col + 5].Value != xlWorksheet.Cells[CurrentRow, 6].Value)
                            {
                                xlWorksheet.Cells[CurrentRow, col + 5].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbLightPink;

                                Microsoft.Office.Interop.Excel.Range oRange = xlWorksheet.Range[xlWorksheet.Cells[CurrentRow, 1], xlWorksheet.Cells[CurrentRow, col + 11]].Cells;
                                //oRange.EntireRow.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbLightPink;
                                oFileInfo.IncreaseNumberOfDifferent();

                            }
                            int colDelta = col + 7;


                            var cell = xlWorksheet.Cells[CurrentRow, colDelta + 1];
                            {
                                cell.Formula = "=M" + CurrentRow.ToString() + "-B" + CurrentRow.ToString();

                                if (!IsCellValueEqual(xlWorksheet.Cells[CurrentRow, 2].Value, xlWorksheet.Cells[CurrentRow, 13].Value))
                                    cell.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;
                            }

                            cell = xlWorksheet.Cells[CurrentRow, colDelta + 2];
                            {
                                cell.Formula = "=N" + CurrentRow.ToString() + "-C" + CurrentRow.ToString();
                                if (!IsCellValueEqual(xlWorksheet.Cells[CurrentRow, 3].Value, xlWorksheet.Cells[CurrentRow, 14].Value))
                                    cell.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;
                            }
                            cell = xlWorksheet.Cells[CurrentRow, colDelta + 3];
                            {
                                cell.Formula = "=O" + CurrentRow.ToString() + "-D" + CurrentRow.ToString();
                                if (!IsCellValueEqual(xlWorksheet.Cells[CurrentRow, 4].Value, xlWorksheet.Cells[CurrentRow, 15].Value))
                                    cell.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;
                            }

                            cell = xlWorksheet.Cells[CurrentRow, colDelta + 4];
                            {
                                cell.Formula = "=P" + CurrentRow.ToString() + "-E" + CurrentRow.ToString();
                                if (!IsCellValueEqual(xlWorksheet.Cells[CurrentRow, 5].Value, xlWorksheet.Cells[CurrentRow, 16].Value))
                                    cell.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;
                            }

                            //xlWorksheet.Cells[CurrentRow, colDelta + 1].Formula = "=M" + CurrentRow.ToString() + "-B" + CurrentRow.ToString();
                            //xlWorksheet.Cells[CurrentRow, colDelta + 2].Formula = "=N" + CurrentRow.ToString() + "-C" + CurrentRow.ToString();
                            //xlWorksheet.Cells[CurrentRow, colDelta + 3].Formula = "=O" + CurrentRow.ToString() + "-D" + CurrentRow.ToString();
                            //xlWorksheet.Cells[CurrentRow, colDelta + 4].Formula = "=P" + CurrentRow.ToString() + "-E" + CurrentRow.ToString();
                        }

                        j += 1;
                    }
                    else
                    {
                        //line has  error ignored
                        oFileInfo.ErrorFlag = true;
                    }
                }
            }
            else
            {
                //first line has error, ignore whole file
                oFileInfo.ErrorFlag = true;
            }
            sr.Close();
            return j;
        }

        private bool IsCellValueEqual(object cellValueA, object cellValueB)
        {
            if (Math.Abs(ResetNaNValue(cellValueA) - ResetNaNValue(cellValueB)) > double.Epsilon)
                return false;
            return true;
        }

        private double ResetNaNValue(object cellValue)
        {
            double result = 0;
            if (cellValue is Double)
                result = (double)cellValue;

            return result;
        }
        private bool IsDataFileValid(string sFile)
        {
            string sline;
            string[] sLineData;
            bool bValid = false;

            System.IO.StreamReader sr = new System.IO.StreamReader(sFile);
            sline = sr.ReadLine();

            sLineData = sline.Split(',');
            if (sLineData.Length >= 10)
            {
                bValid = true;
            }
            sr.Close();
            return bValid;
        }
        public int GetTotalNumberOfSamples(List<CombineFileInfo> lstFile)
        {
            int iTotal = 0;
            foreach (CombineFileInfo oFileInfo in lstFile)
            {
                iTotal = iTotal + oFileInfo.NumberOfSample;
            }
            return iTotal;
        }

        public int GetTotalNumberOfDifference(List<CombineFileInfo> lstFile)
        {
            int iTotal = 0;
            foreach (CombineFileInfo oFileInfo in lstFile)
            {
                iTotal = iTotal + oFileInfo.NumberOfDifferent;
            }
            return iTotal;
        }

        //private async void CombineCsv()
        //{
        //    Debug.WriteLine("start:" + DateTime.Now.ToString());
        //    Cursor.Current = Cursors.WaitCursor;
        //    xlApp = new Microsoft.Office.Interop.Excel.Application();
        //    xlWorkbook = xlApp.Workbooks.Add();  //.Open(FileName);

        //    //xlWorkbook.Worksheets.Add();
        //    //write excel header
        //    lstSheet = new List<ClsSheetInfo>();

        //    string sFile1;
        //    string sFile2;
        //    int count = 0;
        //    int FileCounts = System.IO.Directory.GetFiles(Folder1Path).Length;
        //    //tsProgress.Minimum = 0;
        //    //tsProgress.Maximum = FileCounts;
        //    //tsProgress.Value = 0;
        //    m_NoDiffAllFiles = true;
        //    await Task.Run(() =>
        //    {
        //        //Parallel.ForEach(System.IO.Directory.GetFiles(FileFolder1.Text), sFile =>
        //        foreach (string sFile in System.IO.Directory.GetFiles(DataPath))
        //        {
        //            System.IO.FileInfo fi = new System.IO.FileInfo(sFile);

        //            sFile1 = sFile;
        //            sFile2 = Path.Combine(Folder2Path, fi.Name);

        //            if (System.IO.File.Exists(sFile2) == true)
        //            {
        //                count++;

        //                Invoke((Action)(() =>
        //                {
        //                    ProgressFileCount = count.ToString() + " / " + FileCounts.ToString();
        //                    ProgressFiles = fi.Name;
        //                    tsProgress.Value = count;
        //                }));

        //                try
        //                {
        //                    CombineFileInfo oFileInfo = new CombineFileInfo(fi.Name, sFile1, sFile2);
        //                    AddToSheet("Total", oFileInfo, lstSheet, xlWorkbook);
        //                    CombineFileInfo oFileInfoSample = new CombineFileInfo(fi.Name, sFile1, sFile2);
        //                    AddToSheet(oFileInfoSample.SampleName, oFileInfoSample, lstSheet, xlWorkbook);
        //                    //Add the one that has diffenece to new sheet
        //                    {
        //                        CombineFileInfo oFileDiff = new CombineFileInfo(fi.Name, sFile1, sFile2);
        //                        m_NoDiffAllFiles &= (oFileInfoSample.NumberOfDifferent == 0);
        //                        AddToSheet("Difference", oFileDiff, lstSheet, xlWorkbook, oFileInfoSample.NumberOfDifferent == 0);
        //                    }

        //                    if (oFileInfo.ErrorFlag || oFileInfoSample.ErrorFlag)
        //                    {
        //                        Invoke((Action)(() =>
        //                        {
        //                            txtErrorFileFound.AppendText(fi.Name);
        //                            txtErrorFileFound.AppendText(Environment.NewLine); ;
        //                        }));
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    Invoke((Action)(() =>
        //                    {
        //                        txtErrorFileFound.AppendText(fi.Name);
        //                        txtErrorFileFound.AppendText(Environment.NewLine);
        //                    }));
        //                }
        //            }
        //            else
        //            {
        //                Invoke((Action)(() =>
        //                {
        //                    txtErrorFileFound.Text = fi.Name;
        //                }));
        //            }

        //        }
        //        //);
        //    });

        //    ProgressFileCount = "Combine:Done";
        //    ProgressFiles = "";

        //    tsProgress.Minimum = 0;
        //    tsProgress.Maximum = lstSheet.Count;
        //    tsProgress.Value = 0;
        //    int count2 = 0;
        //    await Task.Run(() =>
        //    {
        //        foreach (ClsSheetInfo oSheet in lstSheet)
        //        {
        //            count2++;
        //            Invoke((Action)(() =>
        //            {
        //                ProgressFileCount = count2.ToString() + " / " + lstSheet.Count.ToString();
        //                ProgressFiles = oSheet.SheetName;
        //                tsProgress.Value = count2;
        //            }));
        //            xlWorksheet = xlWorkbook.Sheets[oSheet.SheetName];
        //            if (oSheet.SampleName.Length < 30)
        //            {
        //                xlWorksheet.Name = oSheet.SampleName;
        //            }

        //            if (m_NoDiffAllFiles && xlWorksheet.Name == "Difference")
        //            {
        //                xlWorksheet.Cells[2, 1].Value = "No difference";
        //                continue;
        //            }
        //            //PopulateDataForSheetSummary(xlWorksheet, oSheet, oSheet.lstFile , xlWorksheet.Name == "Difference");
        //            PopulateDataForSheetSummary(xlWorksheet, oSheet, oSheet.lstFile, xlWorksheet.Name == "Difference");
        //        }
        //        xlWorkbook.Sheets["Total"].Move(xlWorkbook.Sheets[1]);
        //        xlWorkbook.Sheets["Difference"].Move(xlWorkbook.Sheets[1]);
        //    });
        //    ProgressFileCount = "Sheet:Done";
        //    ProgressFiles = "";


        //    xlApp.Visible = false;
        //    xlApp.UserControl = false;

        //    string errorMessage = "";
        //    string tempPath = System.IO.Path.GetTempPath();
        //    string resultFileName = $"{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss", CultureInfo.InvariantCulture)}.xlsx";
        //    string tempFullFileName = Path.Combine(tempPath, resultFileName);
        //    bool isSuccess = false;
        //    try
        //    {
        //        xlWorkbook.SaveAs(tempFullFileName);

        //        bool fileExist = File.Exists(tempFullFileName);
        //        int tryCount = 20;
        //        int i = 0;
        //        do
        //        {
        //            if (!fileExist)
        //            {
        //                Thread.Sleep(100);
        //                i++;
        //            }
        //            else
        //                File.Copy(tempFullFileName, Path.Combine(ResultPath, resultFileName));
        //        } while (!fileExist && i < tryCount);

        //        if (File.Exists(Path.Combine(ResultPath, resultFileName)))
        //        {
        //            isSuccess = true;
        //            ProgressFiles = $"{Path.Combine(ResultPath, resultFileName)}) saved.";
        //        }
        //        else
        //            errorMessage = "Result file not complete for unknown reason.";
        //    }
        //    catch (Exception exception)
        //    {
        //        errorMessage = exception.Message;
        //    }

        //    if (!String.IsNullOrEmpty(errorMessage))
        //    {
        //        ProgressFileCount = "Error";
        //        ProgressFiles = errorMessage;
        //        ProgressBarColor.SetState(tsProgress.ProgressBar, 2);
        //    }

        //    //cleanup
        //    GC.Collect();
        //    GC.WaitForPendingFinalizers();

        //    //release com objects to fully kill excel process from running in the background

        //    //close and release
        //    xlWorkbook.Close();
        //    //quit and release
        //    xlApp.Quit();

        //    try
        //    {
        //        File.Delete(tempFullFileName);
        //    }
        //    catch (Exception exception)
        //    {
        //        errorMessage = exception.Message;
        //    }

        //    // Set cursor as default arrow
        //    Cursor.Current = Cursors.Default;
        //    Debug.WriteLine("End:" + DateTime.Now.ToString());
        //    if (isSuccess)
        //    {
        //        string resultFile = Path.Combine(ResultPath, resultFileName);
        //        if (MessageBox.Show($"Combine complete, Open result Excel file({resultFile})? ", "Success", MessageBoxButtons.OKCancel)
        //            == DialogResult.OK)
        //        {
        //            Process.Start(resultFile);
        //        }
        //    }
        //}
        #endregion
    }

}
