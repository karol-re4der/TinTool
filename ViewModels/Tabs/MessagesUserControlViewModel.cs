using Caliburn.Micro;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
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
        private Task _currentTask;

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


        public PlotData TotalThroughTimePlot { get; set; } = new PlotData();
        public PlotData SentThroughTimePlot { get; set; } = new PlotData();
        public PlotData ReceivedThroughTimePlot { get; set; } = new PlotData();

        public string PlotTitle { get; set; } = "MessagesPerDay";

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

        public void PreparePlot()
        {
            if (_currentTask == null || _currentTask.IsCompleted)
            {
                //Prepare validation
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;

                ProgressText = "Loading new messages";
                _currentTask = Unitool.ValidateMatches(_api, _stats, (x) => Progress = x, token);

                //Prepare ploting
                Action<object> continuation = (x) =>
                {
                    ProgressText = "Preparing the plot";
                    _stats.PlotMessagesThroughTime(TotalThroughTimePlot, SentThroughTimePlot, ReceivedThroughTimePlot);
                    PlotTitle = $"Messages per day. Response rate: {_stats.ResponseRate():F2}, Average convo length: {_stats.AverageConversationLength():F2}";
                    Replot();
                    ProgressText = "Complete!";
                };
                _currentTask.ContinueWith(continuation);
                
                _currentTask.Start();
            }
        }

        public void Replot()
        {
            NotifyOfPropertyChange(() => TotalThroughTimePlot);
            NotifyOfPropertyChange(() => SentThroughTimePlot);
            NotifyOfPropertyChange(() => ReceivedThroughTimePlot);
            NotifyOfPropertyChange(() => PlotTitle);


            //PlotItem.InvalidatePlot(true);
        }

        public void Button_Refresh()
        {
            PreparePlot();
        }

        
    }
}
