using Caliburn.Micro;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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

        private IWindowManager _wm;
        private TinderAPI _tinderAPI;
        private BadooAPI _badooAPI = new BadooAPI();
        private AppSettings _settings;
        public StartupViewModel(IWindowManager wm)
        {
            _wm = wm;
            StartUp();
        }

        public void StartUp()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            _startUpTask = Unitool.StartUp(
                (x) => {
                    Progress(33, "Settings loaded...!");
                    _settings = x;
                },
                (x) => {
                    Progress(66, "Tinder session loaded...!");
                    _tinderAPI = x;
                }, (x) => {
                    Progress(99, "Badoo session loaded...!");
                },
                (x) => {
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
            _wm.ShowWindow(new MainViewModel(_wm, _tinderAPI, _badooAPI, _settings));
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
