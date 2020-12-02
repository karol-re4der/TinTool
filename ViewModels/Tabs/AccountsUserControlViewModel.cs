using Caliburn.Micro;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tinder.DataStructures;
using Tintool.Models;
using Tintool.Models.DataStructures;

namespace Tintool.ViewModels.Tabs
{
    class AccountsUserControlViewModel:Screen
    {

        public List<AccountsTableItemModel> ProfileIDsList { get; set; } = new List<AccountsTableItemModel>();


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

            ProfileIDsList = ListModelsFromStats(_stats);
            NotifyOfPropertyChange(() => ProfileIDsList);
        }

        private void RefreshTables()
        {

        }

        private List<AccountsTableItemModel> ListModelsFromStats(Stats stats)
        {
            List<AccountsTableItemModel> result = new List<AccountsTableItemModel>();

            foreach(string id in stats.ProfileIDs)
            {
                result.Add(new AccountsTableItemModel
                {
                    FileName = "Foo",
                    ID = id,
                    Matches = stats.Matches.Where((x) => id.Equals(x.MatcherID)).Count()
                });
            }

            return result;
        }

        #region Buttons
        public void Button_AddFromSelection()
        {

        }

        public void Button_RemoveFromSelection()
        {

        }

        public void Button_AddFromTextBox()
        {

        }
        #endregion
    }
}
