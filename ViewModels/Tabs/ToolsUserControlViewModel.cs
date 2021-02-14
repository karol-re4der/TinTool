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
using Tintool.APIs.Badoo;
using Tintool.Models;
using Tintool.Models.DataStructures;
using Tintool.ViewModels.Dialogs;
using Tintool.Views;

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

        private TinderAPI _tinderAPI;
        private BadooAPI _badooAPI;
        private Stats _stats;
        private AppSettings _settings;
        private MainViewModel _baseViewModel;

        public ToolsUserControlViewModel(IWindowManager wm, ref TinderAPI tinderAPI, ref BadooAPI badooAPI, ref Stats stats, ref AppSettings settings, MainViewModel baseViewModel)
        {
            this._wm = wm;
            this._tinderAPI = tinderAPI;
            this._badooAPI = badooAPI;
            this._stats = stats;
            this._settings = settings;
            this._baseViewModel = baseViewModel;

            ProximityDistance = "20";
            ProximityInactivityCutout = "1";
            SwipeAllSize = "100";
        }

        public void RefreshContent()
        {
        }

        public void ProximityCheckAction()
        {
            if (_baseViewModel.CurrentTask == null || _baseViewModel.CurrentTask.IsCompleted)
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;

                _baseViewModel.Progress = 0;
                _baseViewModel.ProgressText = "Checking";
                _baseViewModel.CurrentTask = Unitool.ProximityCheck(_tinderAPI, _stats, _proximityDistance, (x)=> _baseViewModel.Progress = x, (x) => _baseViewModel.ProgressText = x+" is eligible!", (x) => MessageBox.Show(x), token);
                Action<object> continuation = (x) =>
                {
                    _baseViewModel.ProgressText = "Checked all!";
                    _baseViewModel.Progress = 100;
                };
                _baseViewModel.CurrentTask.ContinueWith(continuation);
                _baseViewModel.CurrentTask.Start();
            }
        }

        public void SwipeAllAction()
        {
            if (_baseViewModel.CurrentTask == null || _baseViewModel.CurrentTask.IsCompleted)
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;

                _baseViewModel.Progress = 0;
                _baseViewModel.ProgressText = "Swiping";
                _baseViewModel.CurrentTask = Unitool.SwipeAll(_tinderAPI, _swipeAllSize, (x) => _baseViewModel.Progress = x, (x) => _baseViewModel.ProgressText = x, (x) => MessageBox.Show(x), token);
                _baseViewModel.CurrentTask.Start();
            }
        }
    }
}
