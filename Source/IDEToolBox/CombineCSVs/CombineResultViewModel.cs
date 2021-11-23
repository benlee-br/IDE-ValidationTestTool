using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTestApp.ViewModelBase;

namespace IDEToolBox.CombineCSVs
{
    class CombineResultViewModel : ObservableObject
    {
        public string Folder1Path { get; set; }
        public string Folder2Path { get; set; }
    }

}
