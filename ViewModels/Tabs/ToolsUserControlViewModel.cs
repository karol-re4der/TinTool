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

        private MainViewModel BaseViewModel { get; set; }

        public ToolsUserControlViewModel(MainViewModel baseViewModel)
        {
            BaseViewModel = baseViewModel;

            ProximityDistance = "20";
            ProximityInactivityCutout = "1";
            SwipeAllSize = "100";
        }

        public void RefreshContent()
        {
        }

        public void ProximityCheckAction()
        {
            if (BaseViewModel.CurrentTask == null || BaseViewModel.CurrentTask.IsCompleted)
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;

                BaseViewModel.Progress = 0;
                BaseViewModel.ProgressText = "Checking";
                BaseViewModel.CurrentTask = Unitool.ProximityCheck(BaseViewModel.TinderAPI, BaseViewModel.Stats, _proximityDistance, (x)=> BaseViewModel.Progress = x, (x) => BaseViewModel.ProgressText = x+" is eligible!", (x) => MessageBox.Show(x), token);
                Action<object> continuation = (x) =>
                {
                    BaseViewModel.ProgressText = "Checked all!";
                    BaseViewModel.Progress = 100;
                };
                BaseViewModel.CurrentTask.ContinueWith(continuation);
                BaseViewModel.CurrentTask.Start();
            }
        }

        public void SwipeAllAction()
        {
            if (BaseViewModel.CurrentTask == null || BaseViewModel.CurrentTask.IsCompleted)
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;

                BaseViewModel.Progress = 0;
                BaseViewModel.ProgressText = "Swiping";
                BaseViewModel.CurrentTask = Unitool.SwipeAll(BaseViewModel.TinderAPI, _swipeAllSize, (x) => BaseViewModel.Progress = x, (x) => BaseViewModel.ProgressText = x, (x) => MessageBox.Show(x), token);
                BaseViewModel.CurrentTask.Start();
            }
        }
    }
}
