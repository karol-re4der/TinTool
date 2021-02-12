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
using Tintool.Models;
using Tintool.Models.DataStructures;
using Tintool.ViewModels;
using Tintool.ViewModels.Tabs;

namespace Tintool.ViewModels
{
    class LoggedViewModel:Screen
    {
        private IWindowManager wm;

        private AppSettings _settings;
        public Stats stats;
        public TinderAPI api;

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

        public LoggedViewModel(IWindowManager wm, TinderAPI api, AppSettings settings)
        {
            this.wm = wm;
            this.api = api;
            this._settings = settings;

            stats = FileManager.LoadStatsWithNumber(settings.LoginNumber);
            if (stats == null)
            {
                stats = new Stats(FileManager.CreateUniqueStatsName());
                FileManager.AddBinding(_settings.LoginNumber, stats.FileName);
                FileManager.SaveStats(stats);
                stats.ResetDate();
            }
            else
            {

            }
            stats.ProfileIDs.Add(api.GetProfileID());
            stats.ProfileIDs = stats.ProfileIDs.Distinct().ToList();
            //Unitool.LogNewMatches(api.GetMatches(100), stats);

            MatchesUserControl = new MatchesUserControlViewModel(wm, ref api, ref stats, ref _settings, this);
            MessagesUserControl = new MessagesUserControlViewModel(wm, ref api, ref stats, ref _settings, this);
            ToolsUserControl = new ToolsUserControlViewModel(wm, ref api, ref stats, ref _settings, this);
            AccountsUserControl = new AccountsUserControlViewModel(wm, ref api, ref stats, ref _settings, this);
            ConnectUserControl = new ConnectUserControlViewModel(wm, ref api, ref stats, ref _settings, this);
        }

        public void OnStartup(object sender)
        {

        }

        public void RefreshContent(SelectionChangedEventArgs args)
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
            catch(Exception e)
            {

            }
        }

        public void WindowExit()
        {
            FileManager.SaveStats(stats);
        }

        public void Button_LogOut()
        {
            _settings.KeepLogged = false;
            FileManager.SaveSettings(_settings);
            FileManager.SaveStats(stats);
            wm.ShowWindow(new LoginViewModel(wm));
            TryClose();
        }
    }
}
