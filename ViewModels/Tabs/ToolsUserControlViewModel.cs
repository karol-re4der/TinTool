using Caliburn.Micro;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Tinder.DataStructures;
using Tinder.DataStructures.Responses.Matches;
using Tintool.Models;

namespace Tintool.ViewModels.Tabs
{

    class ToolsUserControlViewModel:Screen
    {
        private int _proximityDistance;
        private int _proximityInactivityCutout;
        private int _swipeAllSize;
        public string ProximityDistance
        {
            get
            {
                return "" + _proximityDistance;
            }
            set
            {
                _proximityDistance = int.Parse(value);
                NotifyOfPropertyChange(() => ProximityDistance);
            }
        }
        public string ProximityInactivityCutout
        {
            get
            {
                return "" + _proximityInactivityCutout;
            }
            set
            {
                _proximityInactivityCutout = int.Parse(value);
                NotifyOfPropertyChange(() => ProximityInactivityCutout);
            }
        }
        public string SwipeAllSize
        {
            get
            {
                return "" + _swipeAllSize;
            }
            set
            {
                _swipeAllSize = int.Parse(value);
                NotifyOfPropertyChange(() => SwipeAllSize);
            }
        }



        private IWindowManager _wm;

        private API _api;
        private Stats _stats;

        public ToolsUserControlViewModel(IWindowManager wm, ref API api, ref Stats stats)
        {
            this._wm = wm;
            this._api = api;
            this._stats = stats;

            ProximityDistance = "20";
            ProximityInactivityCutout = "1";
            SwipeAllSize = "100";
        }

        public void RefreshContent()
        {
        }

        public void ProximityCheckAction()
        {
            //make it check only non messaged matches, then make it check for inactivity

            List<Models.DataStructures.UserResponse.Results> potentials = new List<Models.DataStructures.UserResponse.Results>();
            foreach(Match match in _api.GetMatches(100).data.matches)
            {
                potentials.Add(_api.GetUser(match.person._id)?.results);
            }
            var foo = Unitool.ProximityCheck(potentials, _proximityInactivityCutout, _proximityDistance);
            string resultsText = "Proximity check results: " + foo.Count + " potential matches";
            foo.ForEach((x) => resultsText += "\n" + x);
            MessageBox.Show(resultsText);
        }

        public void SwipeAllAction()
        {
            string result = "Swiped all, gained "+_api.SwipeAll()+" matches!";
            MessageBox.Show(result);
            Unitool.LogNewMatches(_api.GetMatches(100).data.matches, _stats);
        }
    }
}
