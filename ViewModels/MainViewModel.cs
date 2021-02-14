using Caliburn.Micro;
using Models;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Tinder.DataStructures;
using Tintool.APIs.Badoo;
using Tintool.Models;
using Tintool.Models.DataStructures;
using Tintool.ViewModels;
using Tintool.ViewModels.Tabs;

namespace Tintool.ViewModels
{
    class MainViewModel : Screen
    {
        private IWindowManager _wm;

        private AppSettings _settings;
        private Stats _stats;
        private TinderAPI _tinderAPI;
        private BadooAPI _badooAPI;

        public MatchesUserControlViewModel MatchesUserControl { get; }
        public MessagesUserControlViewModel MessagesUserControl { get; }
        public ToolsUserControlViewModel ToolsUserControl { get; }
        public AccountsUserControlViewModel AccountsUserControl { get; }
        public ConnectUserControlViewModel ConnectUserControl { get; }


        #region Progressbar
        public Task CurrentTask;
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

        public MainViewModel(IWindowManager wm, TinderAPI tinderAPI, BadooAPI badooAPI, AppSettings settings, Stats stats)
        {
            this._wm = wm;
            this._tinderAPI = tinderAPI;
            this._badooAPI = badooAPI;
            this._settings = settings;
            this._stats = stats;

            MatchesUserControl = new MatchesUserControlViewModel(_wm, ref _tinderAPI, ref _badooAPI, ref stats, ref _settings, this);
            MessagesUserControl = new MessagesUserControlViewModel(_wm, ref _tinderAPI, ref _badooAPI, ref stats, ref _settings, this);
            ToolsUserControl = new ToolsUserControlViewModel(_wm, ref _tinderAPI, ref _badooAPI, ref stats, ref _settings, this);
            AccountsUserControl = new AccountsUserControlViewModel(_wm, ref _tinderAPI, ref _badooAPI, ref stats, ref _settings, this);
            ConnectUserControl = new ConnectUserControlViewModel(_wm, ref _tinderAPI, ref _badooAPI, ref stats, ref _settings, this);
        }

        public void OnStartup(object sender)
        {

        }

        public void RefreshContent(SelectionChangedEventArgs args)
        {
            if (_tinderAPI.IsTokenWorking())
            {
                try
                {
                    if (args.AddedItems.Count > 0)
                    {
                        string tab = ((TabItem)args.AddedItems[0]).Name;
                        if (tab.Equals("MatchesTab"))
                        {
                            MatchesUserControl.RefreshContent();
                        }
                        else if (tab.Equals("ToolsTab"))
                        {
                            ToolsUserControl.RefreshContent();
                        }
                        else if (tab.Equals("MessagesTab"))
                        {
                            MessagesUserControl.RefreshContent();
                        }
                        else if (tab.Equals("ConnectTab"))
                        {
                            ConnectUserControl.RefreshContent();
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception e)
                {

                }
            }
        }

        public void WindowExit()
        {
            FileManager.SaveStats(_stats);
            FileManager.SaveSettings(_settings);
        }

        public void Button_LogOut()
        {
            FileManager.SaveSettings(_settings);
            FileManager.SaveStats(_stats);
            TryClose();
        }
    }
}
