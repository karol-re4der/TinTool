using Caliburn.Micro;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Tinder.DataStructures;
using Tintool.Models;

namespace Tintool.ViewModels.Tabs
{
    class ToolsUserControlViewModel
    {
        private IWindowManager _wm;

        private API _api;
        private Stats _stats;

        public ToolsUserControlViewModel(IWindowManager wm, ref API api, ref Stats stats)
        {
            this._wm = wm;
            this._api = api;
            this._stats = stats;
        }

        public void RefreshContent()
        {
        }

        public void SwipeAllAction()
        {
            string result = "Swiped all, gained "+_api.SwipeAll()+" matches!";
            MessageBox.Show(result);
            Unitool.LogNewMatches(_api.GetMatches(100).data.matches, _stats);
        }
    }
}
