using Caliburn.Micro;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using Tinder.DataStructures;
using Tintool.Models.DataStructures;

namespace Tintool.ViewModels.Tabs
{
    class MessagesUserControlViewModel: Screen
    {
        private IWindowManager _wm;

        private API _api;
        private Stats _stats;


        public PlotData MessagesThroughTimePlot { get; set; } = new PlotData();
        public OxyPlot.Wpf.Plot PlotItem { get; set; } = new OxyPlot.Wpf.Plot();

        public MessagesUserControlViewModel(IWindowManager wm, ref API api, ref Stats stats)
        {
            this._wm = wm;
            this._api = api;
            this._stats = stats;
        }

        public void RefreshContent()
        {
            Replot();
        }

        public void Replot()
        {
            //float avg = _stats.AverageMatchesPerDay();
            //MatchesThroughTimePlot.Title = "Matches per day (" + avg + " average)";
            //_stats.PlotMatchesThroughTime(MatchesThroughTimePlot);

            NotifyOfPropertyChange(() => MessagesThroughTimePlot);

            PlotItem.InvalidatePlot(true);
        }
    }
}
