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
        public IWindowManager WM { get; set; }
        public AppSettings Settings { get; set; }
        public Stats Stats { get; set; }
        public TinderAPI TinderAPI { get; set; }
        public BadooAPI BadooAPI { get; set; }

        public MatchesUserControlViewModel MatchesUserControl { get; set; }
        public MessagesUserControlViewModel MessagesUserControl { get; set; }
        public ToolsUserControlViewModel ToolsUserControl { get; set; }
        public AccountsUserControlViewModel AccountsUserControl { get; set; }
        public ConnectUserControlViewModel ConnectUserControl { get; set; }

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

        public MainViewModel(IWindowManager wm)
        {
            this.WM = wm;
        }

        public void InitializeTabs()
        {
            MatchesUserControl = new MatchesUserControlViewModel(this);
            MessagesUserControl = new MessagesUserControlViewModel(this);
            ToolsUserControl = new ToolsUserControlViewModel(this);
            AccountsUserControl = new AccountsUserControlViewModel(this);
            ConnectUserControl = new ConnectUserControlViewModel(this);
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
                    else if (tab.Equals("AccountsTab"))
                    {
                        AccountsUserControl.RefreshContent();
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

        public void WindowExit()
        {
            FileManager.SaveStats(Stats);
            FileManager.SaveSettings(Settings);
        }

        public void Button_LogOut()
        {
            FileManager.SaveSettings(Settings);
            FileManager.SaveStats(Stats);
            TryClose();
        }
    }
}
