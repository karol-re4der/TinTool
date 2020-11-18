using Caliburn.Micro;
using Models;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        public Stats stats;
        public API api;

        public MatchesUserControlViewModel MatchesUserControl { get; }
        public MessagesUserControlViewModel MessagesUserControl { get; }
        public ToolsUserControlViewModel ToolsUserControl { get; }


        public LoggedViewModel(IWindowManager wm, API api)
        {
            this.wm = wm;
            this.api = api;

            stats = FileManager.LoadStats() ?? (new Stats());
            stats.ResetDate();
            Unitool.LogNewMatches(api.GetMatches(100), stats);

            MatchesUserControl = new MatchesUserControlViewModel(wm, ref api, ref stats);
            MessagesUserControl = new MessagesUserControlViewModel(wm, ref api, ref stats);
            ToolsUserControl = new ToolsUserControlViewModel(wm, ref api, ref stats);
        }



        public void OnStartup(object sender)
        {

        }

        public void RefreshContent(SelectionChangedEventArgs args)
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
                    var foo = Unitool.GetMessages(api);
                }
            }
            else
            {
                return;
            }
        }

        public void WindowExit()
        {
            FileManager.SaveStats(stats);
        }
    }
}
