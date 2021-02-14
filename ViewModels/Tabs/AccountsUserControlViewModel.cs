using Caliburn.Micro;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Tinder.DataStructures;
using Tintool.APIs.Badoo;
using Tintool.Models;
using Tintool.Models.DataStructures;

namespace Tintool.ViewModels.Tabs
{
    class AccountsUserControlViewModel:Screen
    {
        public List<FileInfo> AvailableSaveFiles { get; set; }

        private FileInfo _selectedSaveFile;
        public FileInfo SelectedSaveFile
        {
            get
            {
                return _selectedSaveFile;
            }
            set
            {
                _selectedSaveFile = value;
                NotifyOfPropertyChange(() => SelectedSaveFile);
            }
        }

        public List<AccountsTableItemModel> ProfileIDsList { get; set; } = new List<AccountsTableItemModel>();
        public List<AccountsTableItemModel> AvailableIDsList { get; set; } = new List<AccountsTableItemModel>();

        public AccountsTableItemModel ProfileIDsSelection { get; set; }
        public AccountsTableItemModel AvailableIDsSelection { get; set; }

        private bool _useIDsMerge = false;
        public bool UseIDsMerge
        {
            get
            {
                return _useIDsMerge;
            }
            set
            {
                _useIDsMerge = value;
                NotifyOfPropertyChange(() => UseIDsMerge);
            }
        }

        string _idTextBox;
        public string IDTextBox
        {
            get
            {
                return _idTextBox;
            }
            set
            {
                _idTextBox = value;
                NotifyOfPropertyChange(() => IDTextBox);
            }
        }

        public MainViewModel BaseViewModel { get; set; }

        private bool _comboBoxHandled = false;

        public AccountsUserControlViewModel(MainViewModel baseViewModel)
        {
            BaseViewModel = baseViewModel;

            RefreshContent();
        }

        public void RefreshContent()
        {
            RefreshAllTables();
            AvailableSaveFiles = FileManager.FindAvailableSaveFiles();
            SelectedSaveFile = AvailableSaveFiles.Find((x) => x.Name.Replace(x.Extension, "").Equals(BaseViewModel.Stats.FileName));
        }

        private void RefreshAllTables()
        {
            RefreshCurrentIDsTable();
            RefreshAvailableIDsTable();
        }

        private void RefreshCurrentIDsTable()
        {
            ProfileIDsList = ListModelsFromStats(BaseViewModel.Stats);
            NotifyOfPropertyChange(() => ProfileIDsList);
        }

        private void RefreshAvailableIDsTable()
        {
            AvailableIDsList = new List<AccountsTableItemModel>();

            foreach (Stats s in FileManager.LoadAllSavefiles())
            {
                if (!s.FileName.Equals(BaseViewModel.Stats.FileName))
                {
                    AvailableIDsList.AddRange(ListModelsFromStats(s));
                }
            }
            NotifyOfPropertyChange(() => AvailableIDsList);
        }

        private void SwitchSaveFile()
        {
            FileManager.SaveStats(BaseViewModel.Stats);
            BaseViewModel.Stats = FileManager.LoadStatsWithFileName(SelectedSaveFile.Name);
            BaseViewModel.Settings.DefaultSaveFile = SelectedSaveFile.Name;
            RefreshContent();
        }

        private List<AccountsTableItemModel> ListModelsFromStats(Stats stats)
        {
            List<AccountsTableItemModel> result = new List<AccountsTableItemModel>();

            foreach(string id in stats.ProfileIDs)
            {
                result.Add(new AccountsTableItemModel
                {
                    FileName = stats.FileName,
                    ID = id,
                    Matches = stats.Matches.Where((x) => id.Equals(x.MatcherID)).Count(),
                    LinkedStats = stats
                });
            }

            return result;
        }

        #region Buttons
        public void Combo_SwitchSaveFile(SelectionChangedEventArgs e)
        {
            if (!_comboBoxHandled)
            {
                var result = MessageBox.Show($"Switch savefile to {((FileInfo)(e.AddedItems[0])).Name}", "", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    _comboBoxHandled = true;
                    SelectedSaveFile = (FileInfo)e.RemovedItems[0];
                }
                else
                {
                    SwitchSaveFile();
                }
            }
            else
            {
                _comboBoxHandled = false;
            }
        }

        public void Button_AddFromSelection()
        {
            if (AvailableIDsSelection != null)
            {
                if (!BaseViewModel.Stats.ProfileIDs.Contains(AvailableIDsSelection.ID))
                {
                    BaseViewModel.Stats.ProfileIDs.Add(AvailableIDsSelection.ID);
                }
                if (UseIDsMerge)
                {
                    BaseViewModel.Stats.MergeUniqueMatchesFrom(AvailableIDsSelection.LinkedStats, AvailableIDsSelection.ID);
                }
                RefreshCurrentIDsTable();
            }
        }

        public void Button_RemoveFromSelection()
        {
            if (BaseViewModel.Stats.ProfileIDs.Contains(ProfileIDsSelection.ID))
            {
                BaseViewModel.Stats.ProfileIDs.Remove(ProfileIDsSelection.ID);
                RefreshCurrentIDsTable();
            }
        }

        public void Button_AddFromTextBox()
        {
            if (!BaseViewModel.Stats.ProfileIDs.Contains(IDTextBox))
            {
                BaseViewModel.Stats.ProfileIDs.Add(IDTextBox);
                IDTextBox = "";
                RefreshCurrentIDsTable();
            }
        }
        #endregion
    }
}
