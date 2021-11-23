using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombineCSVFilesFromTwoFolders
{
    class clsSheetInfo
    {
        public string SampleName { set; get; }
        public string SheetName { set; get; }
        public int Row { set; get; }  //row number for each sheet

        public List<FileInfo> lstFile { set; get; }

        public int colGroup1;
        public int colGroup2;
        public clsSheetInfo(string sample, string sheet, int r)
        {
            lstFile = new List<FileInfo>();
            SampleName = sample;
            SheetName = sheet;
            Row = r;

            colGroup1 = 1;
            colGroup2 = 12;
        }
        
    }
}
