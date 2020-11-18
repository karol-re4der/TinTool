using Caliburn.Micro;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Tinder.DataStructures;
using Tinder.DataStructures.Responses.Matches;
using Tintool.Models;
using Tintool.Models.DataStructures;
using Tintool.ViewModels.Dialogs;

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
                try
                {
                    _swipeAllSize = int.Parse(value);
                    NotifyOfPropertyChange(() => SwipeAllSize);
                }catch(Exception e)
                {
                    _swipeAllSize = 0;
                }
            }
        }


        private IWindowManager _wm;

        private API _api;
        private Stats _stats;
        private ProgressDialogViewModel _dialog;

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
            foreach(MatchData match in _api.GetMatches(100))
            {
                //potentials.Add(_api.GetUser(match.Id)?.results);
            }
            var foo = Unitool.ProximityCheck(potentials, _proximityInactivityCutout, _proximityDistance);
            string resultsText = "Proximity check results: " + foo.Count + " potential matches";
            foo.ForEach((x) => resultsText += "\n" + x);
            MessageBox.Show(resultsText);
        }

        public void SwipeAllAction()
        {
            if (_dialog==null)
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;

                _dialog = new ProgressDialogViewModel(_swipeAllSize, 0, 0, "No matches gained", "Swipe all!");
                Task task = Unitool.SwipeAll(_api, _swipeAllSize, (x) => _dialog.Progress = x, (x) => _dialog.ProgressText = x + " matched!", token);
                Action<object> finalTask = (x) => Unitool.LogNewMatches(_api.GetMatches(100), _stats);
                task.ContinueWith(finalTask);
                _dialog.OnCloseAction = (x) => { tokenSource.Cancel(); _dialog = null; finalTask.Invoke(null); };
                _wm.ShowWindow(_dialog);
                task.Start();
            }
        }
    }
}
