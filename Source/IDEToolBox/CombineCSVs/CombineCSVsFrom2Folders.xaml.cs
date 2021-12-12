using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using MessageBox = System.Windows.Forms.MessageBox;

namespace IDEToolBox.CombineCSVs
{
    /// <summary>
    /// Interaction logic for CombineCSVsFrom2Folders.xaml
    /// </summary>
    public partial class CombineCSVsFrom2Folders : System.Windows.Controls.UserControl
    {
        public CombineCSVsFrom2Folders()
        {
            InitializeComponent();
            DataContext = _combineResultViewModel;
            
        }
        CombineResultViewModel _combineResultViewModel = new CombineResultViewModel();
        #region Event

        private void Folder1BrowseClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dgl = new FolderBrowserDialog();

            dgl.SelectedPath = _combineResultViewModel.DataPath;
            if (dgl.ShowDialog() == DialogResult.OK)
            {
                if (Directory.Exists(System.IO.Path.Combine(dgl.SelectedPath, "Before")) &&
                    Directory.Exists(System.IO.Path.Combine(dgl.SelectedPath, "After")))
                {
                    _combineResultViewModel.DataPath = dgl.SelectedPath;

                    _combineResultViewModel.Folder1Path = System.IO.Path.Combine(dgl.SelectedPath, "Before");
                    _combineResultViewModel.Folder2Path = System.IO.Path.Combine(dgl.SelectedPath, "After");
                }
                else
                    _combineResultViewModel.Folder1Path = dgl.SelectedPath;

            }
        }
        private void Folder2BrowseClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dgl = new FolderBrowserDialog();
            if (dgl.ShowDialog() == DialogResult.OK)
            {
                _combineResultViewModel.Folder2Path = dgl.SelectedPath;
            }
        }

        bool m_NoDiffAllFiles = true;

        private async void CombineCsv()
        {
            Debug.WriteLine("start:" + DateTime.Now.ToString());
            //Cursor.Current = Cursors.WaitCursor;
            _combineResultViewModel.xlApp = new Microsoft.Office.Interop.Excel.Application();
            _combineResultViewModel.xlWorkbook = _combineResultViewModel.xlApp.Workbooks.Add();  //.Open(FileName);

            //xlWorkbook.Worksheets.Add();
            //write excel header
            var lstSheet = new List<ClsSheetInfo>();

            string sFile1;
            string sFile2;
            int count = 0;
            int FileCounts = System.IO.Directory.GetFiles(_combineResultViewModel.Folder1Path).Length;
            tsProgress.Minimum = 0;
            tsProgress.Maximum = FileCounts;
            tsProgress.Value = 0;
            m_NoDiffAllFiles = true;
            await Task.Run(() =>
            {
                //Parallel.ForEach(System.IO.Directory.GetFiles(FileFolder1.Text), sFile =>
                foreach (string sFile in System.IO.Directory.GetFiles(_combineResultViewModel.Folder1Path))
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(sFile);

                    sFile1 = sFile;
                    sFile2 = Path.Combine(_combineResultViewModel.Folder2Path, fi.Name);

                    if (System.IO.File.Exists(sFile2) == true)
                    {
                        count++;

                        System.Windows.Application.Current.Dispatcher.Invoke(delegate
                        {
                            _combineResultViewModel.ProgressFileCount = count.ToString() + " / " + FileCounts.ToString();
                            _combineResultViewModel.ProgressFileName = fi.Name;
                            tsProgress.Value = count;
                        });



                        try
                        {
                            CombineFileInfo oFileInfo = new CombineFileInfo(fi.Name, sFile1, sFile2);
                            _combineResultViewModel.AddToSheet("Total", oFileInfo, lstSheet, _combineResultViewModel.xlWorkbook);
                            CombineFileInfo oFileInfoSample = new CombineFileInfo(fi.Name, sFile1, sFile2);
                            _combineResultViewModel.AddToSheet(oFileInfoSample.SampleName, oFileInfoSample, lstSheet, _combineResultViewModel.xlWorkbook);
                            //Add the one that has diffenece to new sheet
                            {
                                CombineFileInfo oFileDiff = new CombineFileInfo(fi.Name, sFile1, sFile2);
                                m_NoDiffAllFiles &= (oFileInfoSample.NumberOfDifferent == 0);
                                _combineResultViewModel.AddToSheet("Difference", oFileDiff, lstSheet, _combineResultViewModel.xlWorkbook, oFileInfoSample.NumberOfDifferent == 0);
                            }

                            if (oFileInfo.ErrorFlag || oFileInfoSample.ErrorFlag)
                            {
                                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                                {
                                    txtErrorFileFound.AppendText(fi.Name);
                                    txtErrorFileFound.AppendText(Environment.NewLine); ;
                                });


                            }
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(delegate
                            {
                                txtErrorFileFound.AppendText(fi.Name);
                                txtErrorFileFound.AppendText(Environment.NewLine); ;
                            });

                        }
                    }
                    else
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(delegate
                        {
                            txtErrorFileFound.Text = fi.Name;
                        });
                    }
                    //);
                }
            });

            _combineResultViewModel.ProgressFileCount = "Combine:Done";
            _combineResultViewModel.ProgressFileName = "";

            tsProgress.Minimum = 0;
            tsProgress.Maximum = lstSheet.Count;
            tsProgress.Value = 0;
            int count2 = 0;
            await Task.Run(() =>
            {
                foreach (ClsSheetInfo oSheet in lstSheet)
                {
                    count2++;
                    System.Windows.Application.Current.Dispatcher.Invoke(delegate
                    {
                        _combineResultViewModel.ProgressFileCount = count2.ToString() + " / " + lstSheet.Count.ToString();
                        _combineResultViewModel.ProgressFileName = oSheet.SheetName;
                        tsProgress.Value = count2;
                    });

                    _combineResultViewModel.xlWorksheet = _combineResultViewModel.xlWorkbook.Sheets[oSheet.SheetName];
                    if (oSheet.SampleName.Length < 30)
                    {
                        _combineResultViewModel.xlWorksheet.Name = oSheet.SampleName;
                    }

                    if (m_NoDiffAllFiles && _combineResultViewModel.xlWorksheet.Name == "Difference")
                    {
                        _combineResultViewModel.xlWorksheet.Cells[2, 1].Value = "No difference";
                        continue;
                    }
                    //PopulateDataForSheetSummary(xlWorksheet, oSheet, oSheet.lstFile , xlWorksheet.Name == "Difference");
                    _combineResultViewModel.PopulateDataForSheetSummary(_combineResultViewModel.xlWorksheet, oSheet, oSheet.lstFile, _combineResultViewModel.xlWorksheet.Name == "Difference");
                }
                _combineResultViewModel.xlWorkbook.Sheets["Total"].Move(_combineResultViewModel.xlWorkbook.Sheets[1]);
                _combineResultViewModel.xlWorkbook.Sheets["Difference"].Move(_combineResultViewModel.xlWorkbook.Sheets[1]);
            });
            _combineResultViewModel.ProgressFileCount = "Sheet:Done";
            _combineResultViewModel.ProgressFileName = "";


            _combineResultViewModel.xlApp.Visible = false;
            _combineResultViewModel.xlApp.UserControl = false;

            string errorMessage = "";
            string tempPath = System.IO.Path.GetTempPath();
            string resultFileName = $"{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss", CultureInfo.InvariantCulture)}.xlsx";
            string tempFullFileName = Path.Combine(tempPath, resultFileName);
            bool isSuccess = false;
            try
            {
                _combineResultViewModel.xlWorkbook.SaveAs(tempFullFileName);

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
                        File.Copy(tempFullFileName, Path.Combine(_combineResultViewModel.ResultPath, resultFileName));
                } while (!fileExist && i < tryCount);

                if (File.Exists(Path.Combine(_combineResultViewModel.ResultPath, resultFileName)))
                {
                    isSuccess = true;
                    _combineResultViewModel.ProgressFileName = $"{Path.Combine(_combineResultViewModel.ResultPath, resultFileName)}) saved.";
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
                _combineResultViewModel.ProgressFileCount = "Error";
                _combineResultViewModel.ProgressFileName = errorMessage;
                //ProgressBarColor.SetState(tsProgress.ProgressBar, 2);
            }

            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //release com objects to fully kill excel process from running in the background

            //close and release
            _combineResultViewModel.xlWorkbook.Close();
            //quit and release
            _combineResultViewModel.xlApp.Quit();

            try
            {
                File.Delete(tempFullFileName);
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
            }

            // Set cursor as default arrow
            //Cursor.Current = Cursors.Default;
            Debug.WriteLine("End:" + DateTime.Now.ToString());
            if (isSuccess)
            {
                string resultFile = Path.Combine(_combineResultViewModel.ResultPath, resultFileName);
                if (MessageBox.Show($"Combine complete, Open result Excel file({resultFile})? ", "Success", MessageBoxButtons.OKCancel)
                    == DialogResult.OK)
                {
                    Process.Start(resultFile);
                }
            }
        }
























        private void frmCombineResult_FormClosing(object sender, FormClosingEventArgs e)
        {
            var path = Properties.Settings.Default.DefaultResultPath;
            Properties.Settings.Default.Save();
        }
        #endregion

        private void CombineButtonClick(object sender, RoutedEventArgs e)
        {
            CombineCsv();
        }
        private void ShowMenuDrop(object sender, RoutedEventArgs e)
        {
            (sender as System.Windows.Controls.Button).ContextMenu.IsEnabled = true;
            (sender as System.Windows.Controls.Button).ContextMenu.PlacementTarget = (sender as System.Windows.Controls.Button);
            (sender as System.Windows.Controls.Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as System.Windows.Controls.Button).ContextMenu.IsOpen = true;
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.MenuItem contextMenuItem = (System.Windows.Controls.MenuItem)sender;
            FolderBrowserDialog dgl = new FolderBrowserDialog();
            switch (contextMenuItem.Name)
            {
                case "ResultFolderItem":
                    dgl.SelectedPath = _combineResultViewModel.ResultPath;
                    if (dgl.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(dgl.SelectedPath) && Directory.Exists(dgl.SelectedPath))
                    {
                        _combineResultViewModel.ResultPath = dgl.SelectedPath;
                    }
                    break;
                case "CompareDataFolderItem":

                    dgl.SelectedPath = _combineResultViewModel.DataPath;
                    if (dgl.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(dgl.SelectedPath) && Directory.Exists(dgl.SelectedPath))
                    {
                        _combineResultViewModel.DataPath = dgl.SelectedPath;
                    }
                    break;
                case "GotoOutputFolder":
                    if (Directory.Exists(_combineResultViewModel.ResultPath))
                        Process.Start(_combineResultViewModel.ResultPath);
                    else
                        MessageBox.Show($"Folder ({_combineResultViewModel.ResultPath}) not found.", "IDE Test Tool");
                    break;

            }

        }
    }
    //public static class ProgressBarColor
    //{
    //    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
    //    static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
    //    public static void SetState(this ProgressBar p, int state)
    //    {
    //        //SendMessage((IntPtr)p.Handle, 1040, (IntPtr)state, IntPtr.Zero);
    //    }
    //}
}
