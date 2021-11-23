using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfTestApp.ViewModelBase;

namespace WpfTestApp.ViewModelBase
{
    public class RecentFileViewModelBase : ObservableObject, IRecentFileViewModel
    {
        public RecentFileViewModelBase(string filePath, DateTime timeLastAccessed, string rootDirectory = "")
        {
            _filePath = filePath;
            _rootDirectory = rootDirectory;

            TimeLastAccessed = timeLastAccessed;
            FileInfo file = new FileInfo(filePath);
            Displayname = file.Name;
            ExperimentName = string.Empty;
        }
        private string _experimentName;
        public string ExperimentName
        {
            get => _experimentName;
            set
            {
                if (_experimentName == value) return;
                _experimentName = value;
                OnPropertyChanged();
            }
        }
        private string _displayName;
        public string Displayname
        {
            get => _displayName;
            set
            {
                if (_displayName == value) return;
                _displayName = value;
                OnPropertyChanged();
            }
        }
        private string _filePath;
        public string FilePath { get => _filePath; }

        private string _rootDirectory;
        public string RootDirectory { get => _rootDirectory; }

        protected DateTime _timeLastAccessed;
        public virtual DateTime TimeLastAccessed
        {
            get => _timeLastAccessed;
            set
            {
                if (value == _timeLastAccessed) { return; }
                _timeLastAccessed = value;
                OnPropertyChanged();
                DisplayDate = $"{TimeLastAccessed:f}";
            }
        }

        private string _displayDate;
        public string DisplayDate
        {
            get => _displayDate;
            set
            {
                if (value == _displayDate) { return; }
                _displayDate = value;
                OnPropertyChanged();
            }
        }

        virtual public bool IsSessionsExpanded { get; set; }

        public string DisplayName => throw new NotImplementedException();

        public bool SearchFileInfo(string filterString = "")
        {
            throw new NotImplementedException();
        }
    }

    public interface IRecentFileViewModel 
    {
        string DisplayName { get;}
        //string FilePath { get; }
        //string RootDirectory { get; }
        DateTime TimeLastAccessed { get; }
        //string DisplayDate { get; set; }
        bool SearchFileInfo(string filterString = "");

    }
}
