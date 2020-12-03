using Caliburn.Micro;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using Tinder.DataStructures;
using Tintool.Models;
using Tintool.Models.DataStructures;

namespace Tintool.ViewModels.Tabs
{
    class AccountsUserControlViewModel:Screen
    {

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

        private IWindowManager _wm;

        private API _api;
        private Stats _stats;
        private AppSettings _settings;

        public AccountsUserControlViewModel(IWindowManager wm, ref API api, ref Stats stats, ref AppSettings settings)
        {
            _wm = wm;
            _api = api;
            _stats = stats;
            _settings = settings;

            RefreshAllTables();
        }

        private void RefreshAllTables()
        {
            RefreshCurrentIDsTable();
            RefreshAvailableIDsTable();
        }

        private void RefreshCurrentIDsTable()
        {
            ProfileIDsList = ListModelsFromStats(_stats);
            NotifyOfPropertyChange(() => ProfileIDsList);
        }

        private void RefreshAvailableIDsTable()
        {
            foreach (Stats s in FileManager.LoadAllSavefiles())
            {
                AvailableIDsList.AddRange(ListModelsFromStats(s));
            }
            NotifyOfPropertyChange(() => AvailableIDsList);
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
        public void Button_AddFromSelection()
        {
            if (AvailableIDsSelection != null)
            {
                if (!_stats.ProfileIDs.Contains(AvailableIDsSelection.ID))
                {
                    _stats.ProfileIDs.Add(AvailableIDsSelection.ID);
                }
                if (UseIDsMerge)
                {
                    _stats.MergeUniqueMatchesFrom(AvailableIDsSelection.LinkedStats, AvailableIDsSelection.ID);
                }
                RefreshCurrentIDsTable();
            }
        }

        public void Button_RemoveFromSelection()
        {
            if (_stats.ProfileIDs.Contains(ProfileIDsSelection.ID))
            {
                _stats.ProfileIDs.Remove(ProfileIDsSelection.ID);
                RefreshCurrentIDsTable();
            }
        }

        public void Button_AddFromTextBox()
        {
            if (!_stats.ProfileIDs.Contains(IDTextBox))
            {
                _stats.ProfileIDs.Add(IDTextBox);
                IDTextBox = "";
                RefreshCurrentIDsTable();
            }
        }
        #endregion
    }
}
