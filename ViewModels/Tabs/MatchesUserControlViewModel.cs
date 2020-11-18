using Caliburn.Micro;
using Models;
using Tinder.DataStructures;
using Tintool.Models.DataStructures;

namespace Tintool.ViewModels
{
    class MatchesUserControlViewModel : Screen
    {
        private IWindowManager _wm;

        private API _api;
        private Stats _stats;


        public PlotData MatchesThroughTimePlot { get; set; } = new PlotData();
        public OxyPlot.Wpf.Plot PlotItem { get; set; } = new OxyPlot.Wpf.Plot();

        public MatchesUserControlViewModel(IWindowManager wm, ref API api, ref Stats stats)
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
            float avg = _stats.AverageMatchesPerDay();
            MatchesThroughTimePlot.Title = "Matches per day (" + avg + " average)";
            _stats.PlotMatchesThroughTime(MatchesThroughTimePlot);

            NotifyOfPropertyChange(() => MatchesThroughTimePlot);

            PlotItem.InvalidatePlot(true);
        }
    }
}
