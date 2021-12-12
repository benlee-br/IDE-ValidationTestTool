using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombineCSVFilesFromTwoFolders
{
    public class CombineFileInfo
    {
        public string FileFullName1 { set; get; }
        public string FileFullName2 { set; get; }
        public string FileName {set;get;}   //pure name
        public string SampleName { set; get; }
        public int NumberOfSample { set; get; }
        public int NumberOfDifferent { set; get; }

        public int FileNameRow { set; get; }
       
      
        public string SheetName { set; get; }
      
        public bool ErrorFlag { set; get; }
        public CombineFileInfo(string s, string f1, string f2)
        {
            FileName = s;
            FileFullName1 = f1;
            FileFullName2 = f2;
            FileNameRow = 0;

            NumberOfSample = 0;
            NumberOfDifferent = 0;
            SampleName = GetSampleName();

            ErrorFlag = false;
        }
        private string GetSampleName()
        {
             
            string sSample=FileName;
            int LastDash = FileName.LastIndexOf('-');
            int PreUnderLine = -1;
            if (LastDash > -1)
            {
                PreUnderLine = FileName.LastIndexOf('_', LastDash);
            }
            if (PreUnderLine > -1)
            {
                sSample = FileName.Substring(PreUnderLine + 1, LastDash - PreUnderLine - 1);
            }
            if (sSample.Length < 4)
            {
                PreUnderLine = FileName.LastIndexOf('_', PreUnderLine-1);
                if (PreUnderLine > -1)
                {
                    sSample = FileName.Substring(PreUnderLine + 1, LastDash - PreUnderLine - 1);
                }
            }
            return sSample.Trim();
        }
        public void IncreaseNumberOfDifferent()
        {
            NumberOfDifferent++;
        }

    }
}
