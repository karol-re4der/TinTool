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
                try
                {
                    _proximityDistance = int.Parse(value);
                    NotifyOfPropertyChange(() => ProximityDistance);
                }
                catch (Exception e)
                {

                }
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
                try
                {
                    _proximityInactivityCutout = int.Parse(value);
                    NotifyOfPropertyChange(() => ProximityInactivityCutout);
                }
                catch (Exception e)
                {

                }
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
                }
                catch(Exception e)
                {

                }
            }
        }


        private IWindowManager _wm;

        private API _api;
        private Stats _stats;

        #region Progressbar
        private Task _currentTask;
        private int _progress = 0;
        public int Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = value;
                NotifyOfPropertyChange(() => Progress);
            }
        }
        private string _progressText = "";
        public string ProgressText
        {
            get
            {
                return _progressText;
            }
            set
            {
                _progressText = value;
                NotifyOfPropertyChange(() => ProgressText);
            }
        }
        #endregion

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
            if (_currentTask == null || _currentTask.IsCompleted)
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;

                Progress = 0;
                ProgressText = "Checking";
                _currentTask = Unitool.ProximityCheck(_api, _stats, _proximityDistance, (x)=>Progress = x, (x) => ProgressText = x+" is eligible!", (x) => MessageBox.Show(x), token);
                Action<object> continuation = (x) =>
                {
                    ProgressText = "Checked all!";
                    Progress = 100;
                };
                _currentTask.ContinueWith(continuation);
                _currentTask.Start();
            }
        }

        public void SwipeAllAction()
        {
            if (_currentTask==null || _currentTask.IsCompleted)
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;

                Progress = 0;
                ProgressText = "Swiping";
                _currentTask = Unitool.SwipeAll(_api, _swipeAllSize, (x) => Progress = x, (x) => ProgressText = "Matched with "+x+"!", (x)=>MessageBox.Show(x), token);
                Action<object> continuation = (x) => 
                {
                    Unitool.LogNewMatches(_api.GetMatches(100), _stats);
                    ProgressText = "Swiped all!";
                    Progress = 100;
                };
                _currentTask.ContinueWith(continuation);
                _currentTask.Start();
            }
        }
    }
}
