using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfTestApp.ViewModelBase
{
    public class RecentFilesManagerViewModelBase : ObservableObject
    {
        public readonly object UserRecentFilesCollectionLock = new object();


        public RecentFilesManagerViewModelBase(string recentFileRepositoryName)
        {
            UserRecentFilesCollectionCache = new List<IRecentFileViewModel>();
            FileRepositoryPath = recentFileRepositoryName;
        }
        public RecentFilesManagerViewModelBase(List<IRecentFileViewModel>  recentFilesCollectionCache)
        {
            UserRecentFilesCollectionCache = recentFilesCollectionCache;
        }
        string FileRepositoryPath { get; set; }

        string SearchString { get; set; }
        
        bool InitialFileListHasBeenBuilt { get; set; }
        private List<IRecentFileViewModel> UserRecentFilesCollectionCache { get; set; }

        //File base in directory sorting
        public virtual async Task GetUserRecentFilesAsync(bool startNew = false)
        {
            await Task.Factory.StartNew(async () =>
            {
                try
                {
                    //if (InfoContainer.SecurityObject == null)
                    //{
                    //    return;
                    //}
                    if (startNew)
                    {
                        UserRecentFilesCollectionCache.Clear();
                        InitialFileListHasBeenBuilt = false;
                        var quarantineDirectory = Path.Combine(FileRepositoryPath, @"Quarantine\");
                        DirectorySearch(FileRepositoryPath, quarantineDirectory);
                        await ResetRecentFilesCollection().ConfigureAwait(false);
                    }
                    else
                        UserRecentFilesCollection.Clear();
                    await OrderUserFileCollection(SearchString).ConfigureAwait(false);
                }
                catch (Exception ex)
                {

                }
            }).ConfigureAwait(false);
        }

        public virtual async Task RefreshExpandUiElements()
        {
            await Task.Yield();
        }

        private ObservableCollection<IRecentFileViewModel> _userRecentFilesDisplayNames;
        public ObservableCollection<IRecentFileViewModel> UserRecentFilesCollection
        {
            get => _userRecentFilesDisplayNames;
            private set
            {
                if (_userRecentFilesDisplayNames == value) return;
                _userRecentFilesDisplayNames = value;
                OnPropertyChanged();
            }
        }

        public async Task UpdateUserRecentFilesCollection(IList<IRecentFileViewModel> newList)
        {
            await DispatchAsync(() =>
            {
            // Must be executed in the UI thread to support "EnableCollectionSynchronization".
            // This allows updates of the observable collection for a worker thread.
            var newCollection = new ObservableCollection<IRecentFileViewModel>(newList);
                BindingOperations.EnableCollectionSynchronization(newCollection, UserRecentFilesCollectionLock);
                UserRecentFilesCollection = newCollection;

            }).ConfigureAwait(false);
            await RefreshExpandUiElements().ConfigureAwait(false);
        }

        public async Task ResetRecentFilesCollection()
        {
            await UpdateUserRecentFilesCollection(new List<IRecentFileViewModel>()).ConfigureAwait(false);
        }

        public bool WorkOffline { get; set; }
        protected virtual void DirectorySearch(string searchDirectory, string quarantineDirectory, bool isRecursive = false) { }

        public Task<bool> AddNewFileItem(string templateName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RefreshFileItemList()
        {
            throw new NotImplementedException();
        }

        private async Task OrderUserFileCollection(string filterString = "")
        {
            try
            {

                int numberOfRecentFilesToLoad = 50;
                var sortedCollection = UserRecentFilesCollectionCache.FindAll(x => x.SearchFileInfo(filterString))
                    .OrderByDescending(x => (x.TimeLastAccessed)).Take(numberOfRecentFilesToLoad);

                foreach (var fileModel in sortedCollection)
                {
                    UserRecentFilesCollection.Add(fileModel);
                }
                InitialFileListHasBeenBuilt = true;
                await Task.Yield();
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
