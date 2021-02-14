using Caliburn.Micro;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Tinder.DataStructures;
using Tintool.APIs.Badoo;
using Tintool.Models;
using Tintool.Models.DataStructures;

namespace Tintool.ViewModels
{
    class StartupViewModel : Screen
    {
        private int _progressBarValue = 50;
        public int ProgressBarValue {
            get
            {
                return _progressBarValue;
            }
            set
            {
                _progressBarValue = value;
                NotifyOfPropertyChange(() => ProgressBarValue);
            }
        }

        private string _progressBarText = "";
        public string ProgressBarText
        {
            get
            {
                return _progressBarText;
            }
            set
            {
                _progressBarText = value;
                NotifyOfPropertyChange(() => ProgressBarValue);
            }
        }

        private Task _startUpTask = null;

        public IWindowManager WM { get; set; }
        public MainViewModel BaseViewModel { get; set; }

        public StartupViewModel(IWindowManager wm)
        {
            this.WM = wm;
            StartUp();
        }

        public void StartUp()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            BaseViewModel = new MainViewModel(WM);

            _startUpTask = Unitool.StartUp(
                (x) => {
                    Progress(25, "Settings loaded...!");
                    BaseViewModel.Settings = x;
                },
                (x) => {
                    Progress(50, "Tinder session loaded...!");
                    BaseViewModel.TinderAPI = x;
                }, (x) => {
                    Progress(75, "Badoo session loaded...!");
                    BaseViewModel.BadooAPI = new BadooAPI();
                }, (x) =>
                {
                    Progress(99, "Stats loaded...!");
                    BaseViewModel.Stats = x;
                }, (x) => {
                    if (x)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            FinalizeLogin();
                        });
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            OnFail();
                        });
                    }
                }, token);
            _startUpTask.Start();
        }

        private void Progress(int value, string text)
        {
            ProgressBarValue = value;
            ProgressBarText = text;
        }

        private void FinalizeLogin()
        {
            BaseViewModel.InitializeTabs();
            WM.ShowWindow(BaseViewModel);
            this.TryClose();
        }

        private void OnFail()
        {
            Progress(0, "Failed");
            switch(MessageBox.Show("Cannot load! Try again?", "Tintool", MessageBoxButton.YesNo))
            {
                case MessageBoxResult.Yes:
                    StartUp();
                    break;
                case MessageBoxResult.No:
                    this.TryClose();
                    break;
            }
        }
    }
}
