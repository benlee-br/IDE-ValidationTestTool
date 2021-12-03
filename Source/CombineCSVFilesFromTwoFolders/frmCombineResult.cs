using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Runtime.InteropServices;

namespace CombineCSVFilesFromTwoFolders
{
    public partial class frmCombineResult : Form, IDisposable
    {
        const int Version = 8;
        const string Form_Title = "Combine Files From Two Folders";
        string[] Header = new string[] { "", "Well","","", "FAM Cq", "Cy5 Cq", "Cq", "I.C. Cq","", "Result" };
        enum ColumnName {None , Well, Content, Sample, cq1, cq2, cq3, ICCq, SQ, Result }
        public frmCombineResult()
        {
            InitializeComponent();
        }
        object lockme = new object();
        List<clsSheetInfo> lstSheet = null;
        Microsoft.Office.Interop.Excel.Application xlApp = null;
        Microsoft.Office.Interop.Excel.Workbook xlWorkbook = null;  //.Open(FileName);
        Microsoft.Office.Interop.Excel._Worksheet xlWorksheet;

        bool m_NoDiffAllFiles = true;
        private void frmCombineResult_Load(object sender, EventArgs e)
        { 
            txtFolder1.Text = "C:\\Users\\yong_qin\\Downloads\\IDE Results_ON";
            txtFolder2.Text = "C:\\Users\\yong_qin\\Downloads\\IDE Results_OFF";
            txtFolder1.Text = @"C:\Users\yong_qin\Downloads\Debugging\Macros Excel\Copy data below each other\IDE Results_ON";
            txtFolder2.Text = @"C:\Users\yong_qin\Downloads\Debugging\Macros Excel\Copy data below each other\IDE Results_OFF";
            txtFolder1.Text = @"C:\Users\yong_qin\Downloads\Before";
            txtFolder2.Text = @"C:\Users\yong_qin\Downloads\After";
            txtFolder1.Text = @"C:\Temp\CrossTwice\IDE Result";
            txtFolder2.Text = @"C:\Users\yong_qin\Downloads\Debugging\Examples pcrd\6_Curves Crossign threshold twice\IDE Result";
            txtFolder1.Text = @"C:\Work\Data\2021_09_20\IDE Result_After Subset";
            txtFolder2.Text = @"C:\Work\Data\2021_09_20\IDE Result_Before Subset";


            //Form title
            Text = $"{Form_Title} - Version {Version}";
        }

        private void btnBrowse1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dgl = new FolderBrowserDialog();
            if (dgl.ShowDialog() == DialogResult.OK)
            {
                if(Directory.Exists(Path.Combine(dgl.SelectedPath, "Before")) &&
                    Directory.Exists(Path.Combine(dgl.SelectedPath, "After")))
                {
                    txtFolder1.Text = Path.Combine(dgl.SelectedPath, "Before");

                    txtFolder2.Text = Path.Combine(dgl.SelectedPath, "After");
                }
                else
                    txtFolder1.Text = dgl.SelectedPath;

            }
        }

        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dgl = new FolderBrowserDialog();
            if (dgl.ShowDialog() == DialogResult.OK)
            {
                txtFolder2.Text = dgl.SelectedPath;
            }
        }

        private async void btnCombine_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("start:" + DateTime.Now.ToString());
            Cursor.Current = Cursors.WaitCursor;
            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlWorkbook = xlApp.Workbooks.Add();  //.Open(FileName);
            
            //xlWorkbook.Worksheets.Add();
            //write excel header
            lstSheet = new List<clsSheetInfo>();

            string sFile1;
            string sFile2;
            int count = 0;
            int FileCounts = System.IO.Directory.GetFiles(txtFolder1.Text).Length;
            tsProgress.Minimum = 0;
            tsProgress.Maximum = FileCounts;
            tsProgress.Value = 0;
            m_NoDiffAllFiles = true;
            await Task.Run(() =>
            {
                //Parallel.ForEach(System.IO.Directory.GetFiles(txtFolder1.Text), sFile =>
                foreach(string sFile in System.IO.Directory.GetFiles(txtFolder1.Text))
                {
                   System.IO.FileInfo fi = new System.IO.FileInfo(sFile);

                   sFile1 = sFile;
                   sFile2 = txtFolder2.Text + "\\" + fi.Name;

                   if (System.IO.File.Exists(sFile2) == true)
                   {
                       count++;

                       Invoke((Action)(() =>
                       {
                           tsCount.Text = count.ToString() + " / " + FileCounts.ToString();
                           tsFileName.Text = fi.Name;
                           tsProgress.Value = count;
                       }));

                       try
                       {
                            FileInfo oFileInfo = new FileInfo(fi.Name, sFile1, sFile2);
                            AddToSheet("Total", oFileInfo, lstSheet, xlWorkbook);
                            FileInfo oFileInfoSample = new FileInfo(fi.Name, sFile1, sFile2);
                            AddToSheet(oFileInfoSample.SampleName, oFileInfoSample, lstSheet, xlWorkbook);
                            //Add the one that has diffenece to new sheet
                            {
                                FileInfo oFileDiff = new FileInfo(fi.Name, sFile1, sFile2);
                                m_NoDiffAllFiles &= (oFileInfoSample.NumberOfDifferent == 0);
                                AddToSheet("Difference", oFileDiff, lstSheet, xlWorkbook, oFileInfoSample.NumberOfDifferent == 0);
                            }

                            if(oFileInfo.ErrorFlag || oFileInfoSample.ErrorFlag)
                            {
                                Invoke((Action)(() =>
                                {
                                    txtErrorFileFound.AppendText(fi.Name);
                                    txtErrorFileFound.AppendText(Environment.NewLine); ;
                                }));
                            }
                       }
                       catch (Exception ex)
                       {
                           Invoke((Action)(() =>
                           {
                               txtErrorFileFound.AppendText(fi.Name);
                               txtErrorFileFound.AppendText(Environment.NewLine);
                           }));
                       }
                   }
                   else
                   {
                       Invoke((Action)(() =>
                       {
                           txtErrorFileFound.Text = fi.Name;
                       }));
                   }
                  
               }
               //);
            });

            tsCount.Text = "Combine:Done";
            tsFileName.Text = "";

            tsProgress.Minimum = 0;
            tsProgress.Maximum = lstSheet.Count;
            tsProgress.Value = 0;
            int count2 = 0;
            await Task.Run(() =>
            {
                foreach (clsSheetInfo oSheet in lstSheet)
                {
                    count2++;
                    Invoke((Action)(() =>
                    {
                        tsCount.Text = count2.ToString() + " / " + lstSheet.Count.ToString();
                        tsFileName.Text = oSheet.SheetName;
                        tsProgress.Value = count2;
                    }));
                    xlWorksheet = xlWorkbook.Sheets[oSheet.SheetName];
                    if (oSheet.SampleName.Length < 30)
                    {
                        xlWorksheet.Name = oSheet.SampleName;
                    }

                    if (m_NoDiffAllFiles && xlWorksheet.Name == "Difference")
                    {
                        xlWorksheet.Cells[2, 1].Value = "No difference"; 
                        continue;
                    }
                    PopulateDataForSheetSummary(xlWorksheet, oSheet, oSheet.lstFile);
                }
                xlWorkbook.Sheets["Total"].Move(xlWorkbook.Sheets[1]);
                xlWorkbook.Sheets["Difference"].Move(xlWorkbook.Sheets[1]);
            });
            tsCount.Text = "Sheet:Done";
            tsFileName.Text = "";
           

            xlApp.Visible = false;
            xlApp.UserControl = false;

            string errorMessage = "";
            string tempPath = System.IO.Path.GetTempPath();
            string resultFileName = $"{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss", CultureInfo.InvariantCulture)}.xlsx";
            string tempFullFileName = Path.Combine(tempPath, resultFileName);
            bool isSuccess = false;
            try
            {
                xlWorkbook.SaveAs(tempFullFileName);

                bool fileExist = File.Exists(tempFullFileName);
                int tryCount = 20;
                int i = 0;
                do
                {
                    if (!fileExist)
                    {
                        Thread.Sleep(100);
                        i++;
                    }
                    else
                        File.Copy(tempFullFileName, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), resultFileName));
                } while (!fileExist && i < tryCount);

                if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), resultFileName)))
                {
                    isSuccess = true;
                    tsFileName.Text = $"{Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), resultFileName)}) saved.";
                }
                else
                    errorMessage = "Result file not complete for unknown reason.";
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
            }

            if (!String.IsNullOrEmpty(errorMessage))
            {
                tsCount.Text = "Error";
                tsFileName.Text = errorMessage;
                ProgressBarColor.SetState(tsProgress.ProgressBar, 2);
            }

            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //release com objects to fully kill excel process from running in the background

            //close and release
            xlWorkbook.Close();
            //quit and release
            xlApp.Quit();

            try
            {
                File.Delete(tempFullFileName);
            }
            catch(Exception exception)
            {
                errorMessage = exception.Message;
            }

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
            Debug.WriteLine("End:" + DateTime.Now.ToString());
            if (isSuccess)
            {
                string resultFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), resultFileName);
                if (MessageBox.Show($"Combine complete, Open result Excel file({resultFile})? ", "Success", MessageBoxButtons.OKCancel) 
                    == DialogResult.OK)
                {
                    Process.Start(resultFile);
                }
            }
        }

        private void AddToSheet(string SheetSampleName, FileInfo oFile, List<clsSheetInfo> lstSheet,Microsoft.Office.Interop.Excel.Workbook xlWorkbook, bool NoDiff = false)
        {
            lock (lockme)
            {
                Microsoft.Office.Interop.Excel._Worksheet xlWorksheet;
                clsSheetInfo oSheet = SearchSampleSheets(lstSheet, SheetSampleName);
                if (oSheet == null)
                {
                    xlWorkbook.Worksheets.Add();
                    //xlWorksheet = xlWorkbook.Sheets[xlWorkbook.Sheets.Count];
                    xlWorksheet = xlWorkbook.Sheets[1];
                    oSheet = new clsSheetInfo(SheetSampleName, xlWorksheet.Name, 2);
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
        private clsSheetInfo SearchSampleSheets(List<clsSheetInfo>lstSheets, string SampleName)
        {
            
            foreach(clsSheetInfo oSheetInfo in lstSheets)
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

            xlWorksheet.Cells[1, 3].Value = txtFolder1.Text; 
            xlWorksheet.Cells[1, 3].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[1, 13].Value = txtFolder2.Text;
            xlWorksheet.Cells[1, 3].EntireRow.Font.Bold = true;

        }
        private void PopulateDataForSheet(Microsoft.Office.Interop.Excel._Worksheet xlWorksheet, clsSheetInfo oSheetInfo, FileInfo oFileInfo, bool NoDiff = false)
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
            string[] Title  = sline.Split(',');
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
        private void PopulateDataForSheetSummary(Microsoft.Office.Interop.Excel._Worksheet xlWorksheet, clsSheetInfo oSheet, List<FileInfo> lstFile)
        {

            //Write Total number
            int row = oSheet.Row;
            int colGroup1 = oSheet.colGroup1;
            int colGroup2 = oSheet.colGroup2;

            int col = colGroup2 + 12;
            xlWorksheet.Cells[1, col].Value = "Number of Samples";
            xlWorksheet.Cells[1, col + 1].Value = "Number of Difference";
            foreach (FileInfo oFileInfo in lstFile)
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

            foreach (FileInfo oFileInfo in lstFile)
            {
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
            xlWorksheet.Cells[row, 1].Value = lstFile.Count.ToString();
            xlWorksheet.Cells[row, col].Value = GetTotalNumberOfSamples(lstFile);
            xlWorksheet.Cells[row, col + 1].Value = GetTotalNumberOfDifference(lstFile);

        }
        private int PopulateData(Microsoft.Office.Interop.Excel._Worksheet xlWorksheet, int row, int col, string sFile, FileInfo oFileInfo)
        {
            string sline;
            string[] sLineData;
            int j=0;

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
                            xlWorksheet.Cells[CurrentRow, colDelta + 1].Formula = "=M" + CurrentRow.ToString() + "-B" + CurrentRow.ToString();
                            xlWorksheet.Cells[CurrentRow, colDelta + 2].Formula = "=N" + CurrentRow.ToString() + "-C" + CurrentRow.ToString();
                            xlWorksheet.Cells[CurrentRow, colDelta + 3].Formula = "=O" + CurrentRow.ToString() + "-D" + CurrentRow.ToString();
                            xlWorksheet.Cells[CurrentRow, colDelta + 4].Formula = "=P" + CurrentRow.ToString() + "-E" + CurrentRow.ToString();

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
         public int GetTotalNumberOfSamples(List<FileInfo> lstFile)
        {
            int iTotal = 0;
            foreach(FileInfo oFileInfo in lstFile)
            {
                iTotal = iTotal + oFileInfo.NumberOfSample;
            }
            return iTotal;
        }

        public int GetTotalNumberOfDifference(List<FileInfo> lstFile)
        {
            int iTotal = 0;
            foreach (FileInfo oFileInfo in lstFile)
            {
                iTotal = iTotal + oFileInfo.NumberOfDifferent;
            }
            return iTotal;
        }
    }
    public static class ProgressBarColor
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public static void SetState(this ProgressBar p, int state)
        {
            SendMessage(p.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }
    }
    
}
