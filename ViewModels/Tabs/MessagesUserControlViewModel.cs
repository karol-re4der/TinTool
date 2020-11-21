using Caliburn.Micro;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using Tinder.DataStructures;
using Tintool.Models;
using Tintool.Models.DataStructures;

namespace Tintool.ViewModels.Tabs
{
    class MessagesUserControlViewModel: Screen
    {
        private IWindowManager _wm;

        private API _api;
        private Stats _stats;


        public PlotData TotalThroughTimePlot { get; set; } = new PlotData();
        public PlotData SentThroughTimePlot { get; set; } = new PlotData();
        public PlotData ReceivedThroughTimePlot { get; set; } = new PlotData();

        public string PlotTitle { get; set; } = "";

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
            Unitool.ValidateMatches(_api, _stats);

            PlotTitle = $"Messages per day. Response rate: {_stats.ResponseRate():F2}, Average convo length: {_stats.AverageConversationLength():F2}";
            _stats.PlotMessagesThroughTime(TotalThroughTimePlot, SentThroughTimePlot, ReceivedThroughTimePlot);


            NotifyOfPropertyChange(() => TotalThroughTimePlot);
            NotifyOfPropertyChange(() => SentThroughTimePlot);
            NotifyOfPropertyChange(() => ReceivedThroughTimePlot);
            NotifyOfPropertyChange(() => PlotTitle);


            PlotItem.InvalidatePlot(true);
        }
    }
}
