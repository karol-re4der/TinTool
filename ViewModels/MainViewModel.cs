using Caliburn.Micro;
using Models;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using Tintool.APIs.Badoo;
using Tintool.Models.Saveables;
using Tintool.ViewModels.Dialogs;
using Tintool.ViewModels.Tabs;

namespace Tintool.ViewModels
{
    class MainViewModel : Screen
    {
        public IWindowManager WM { get; set; }
        public SettingsModel Settings { get; set; }
        public StatsModel Stats { get; set; }
        public TinderAPI TinderAPI { get; set; }
        public BadooAPI BadooAPI { get; set; }

        public MatchesUserControlViewModel MatchesUserControl { get; set; }
        public MessagesUserControlViewModel MessagesUserControl { get; set; }
        public ToolsUserControlViewModel ToolsUserControl { get; set; }
        public AccountsUserControlViewModel AccountsUserControl { get; set; }

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
        private double _badooIconOpacity;
        public double BadooIconOpacity
        {
            get
            {
                return _badooIconOpacity;
            }
            set
            {
                _badooIconOpacity = value;
                NotifyOfPropertyChange(() => BadooIconOpacity);
            }
        }
        private double _tinderIconOpacity;
        public double TinderIconOpacity
        {
            get
            {
                return _tinderIconOpacity;
            }
            set
            {
                _tinderIconOpacity = value;
                NotifyOfPropertyChange(() => TinderIconOpacity);
            }
        }
        private const double ActiveIconOpacity = 1;
        private const double DisabledIconOpacity = 0.5;
        #endregion

        public MainViewModel(IWindowManager wm)
        {
            this.WM = wm;
        }

        public void Initialize(bool tinderUp, bool badooUp)
        {
            MatchesUserControl = new MatchesUserControlViewModel(this);
            MessagesUserControl = new MessagesUserControlViewModel(this);
            ToolsUserControl = new ToolsUserControlViewModel(this);
            AccountsUserControl = new AccountsUserControlViewModel(this);

            TinderIconOpacity = tinderUp ? ActiveIconOpacity : DisabledIconOpacity;
            BadooIconOpacity = badooUp ? ActiveIconOpacity : DisabledIconOpacity;
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

        public void TinderButtonClicked()
        {
            WM.ShowDialog(new TinderLoginDialogViewModel(this));
        }
        public void BadooButtonClicked()
        {
            WM.ShowDialog(new BadooLoginDialogViewModel(this));
        }

        public void RefreshStatusIcons()
        {
            TinderIconOpacity = TinderAPI.IsTokenWorking() ? ActiveIconOpacity : DisabledIconOpacity;
            BadooIconOpacity = BadooAPI.IsTokenWorking() ? ActiveIconOpacity : DisabledIconOpacity;
        }
    }
}
