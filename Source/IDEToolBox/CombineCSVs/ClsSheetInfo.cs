using System.Collections.Generic;

namespace IDEToolBox.CombineCSVs
{
    internal class ClsSheetInfo
    {
        public string SampleName { set; get; }
        public string SheetName { set; get; }
        public int Row { set; get; }  //row number for each sheet

        public List<CombineFileInfo> lstFile { set; get; }

        public int colGroup1;
        public int colGroup2;
        public ClsSheetInfo(string sample, string sheet, int r)
        {
            lstFile = new List<CombineFileInfo>();
            SampleName = sample;
            SheetName = sheet;
            Row = r;

            colGroup1 = 1;
            colGroup2 = 12;
        }

    }
}