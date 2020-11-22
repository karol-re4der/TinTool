using Caliburn.Micro;
using Models;
using OxyPlot.Axes;
using System;
using System.Linq;
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

        #region Progressbar
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
        #endregion

        #region Plots
        public PlotData TotalThroughTimePlot { get; set; } = new PlotData();
        public PlotData SentThroughTimePlot { get; set; } = new PlotData();
        public PlotData ReceivedThroughTimePlot { get; set; } = new PlotData();

        private bool _totalVisible = true;
        public string TotalVisibleAsString
        {
            get
            {
                return _totalVisible ? "Visible" : "Hidden";
            }
        }
        public bool TotalVisible
        {
            get
            {
                return _totalVisible;
            }
            set
            {
                _totalVisible = value;
                NotifyOfPropertyChange(() => TotalVisible);
                NotifyOfPropertyChange(() => TotalVisibleAsString);
            }
        }
        private bool _sentVisible = true;
        public string SentVisibleAsString
        {
            get
            {
                return _sentVisible ? "Visible" : "Hidden";
            }
        }
        public bool SentVisible
        {
            get
            {
                return _sentVisible;
            }
            set
            {
                _sentVisible = value;
                NotifyOfPropertyChange(() => SentVisible);
                NotifyOfPropertyChange(() => SentVisibleAsString);
            }
        }
        private bool _receivedVisible = true;
        public string ReceivedVisibleAsString
        {
            get
            {
                return _receivedVisible ? "Visible" : "Hidden";
            }
        }
        public bool ReceivedVisible
        {
            get
            {
                return _receivedVisible;
            }
            set
            {
                _receivedVisible = value;
                NotifyOfPropertyChange(() => ReceivedVisible);
                NotifyOfPropertyChange(() => ReceivedVisibleAsString);
            }
        }

        private DateTime _startingDate;
        public DateTime StartingDate
        {
            get
            {
                return _startingDate;
            }
            set
            {
                if (value < EndingDate)
                {
                    _startingDate = value;
                    StartingDateAsDouble = DateTimeAxis.ToDouble(value);
                }
                NotifyOfPropertyChange(() => StartingDate);
                Replot();
            }
        }
        private double _startingDateAsDouble;
        public double StartingDateAsDouble
        {
            get
            {
                return _startingDateAsDouble;
            }
            set
            {
                _startingDateAsDouble = value;
                NotifyOfPropertyChange(() => StartingDateAsDouble);
            }
        }

        private DateTime _endingDate;
        public DateTime EndingDate
        {
            get
            {
                return _endingDate;
            }
            set
            {
                if (value > StartingDate)
                {
                    _endingDate = value;
                    EndingDateAsDouble = DateTimeAxis.ToDouble(value);
                }
                NotifyOfPropertyChange(() => EndingDate);
                Replot();
            }
        }
        private double _endingDateAsDouble;
        public double EndingDateAsDouble
        {
            get
            {
                return _endingDateAsDouble;
            }
            set
            {
                _endingDateAsDouble = value;
                NotifyOfPropertyChange(() => EndingDateAsDouble);
            }
        }
        
        private double _plotUpperConstraint;
        public double PlotUpperConstraint
        {
            get
            {
                return _plotUpperConstraint;
            }
            set
            {
                _plotUpperConstraint = value;
                NotifyOfPropertyChange(() => PlotUpperConstraint);
            }
        }

        private string _dateTimeAxisFormat;
        public string DateTimeAxisFormat
        {
            get
            {
                return _dateTimeAxisFormat;
            }
            set
            {
                _dateTimeAxisFormat = value;
                NotifyOfPropertyChange(() => DateTimeAxisFormat);
            }
        }

        public string PlotTitle { get; set; } = "Messages per day";

        public OxyPlot.Wpf.Plot PlotItem { get; set; } = new OxyPlot.Wpf.Plot();
        #endregion

        public MessagesUserControlViewModel(IWindowManager wm, ref API api, ref Stats stats)
        {
            this._wm = wm;
            this._api = api;
            this._stats = stats;

            SetTime();
        }

        private void SetTime()
        {
            EndingDate = DateTime.Today;
            StartingDate = _stats.Date.Date;
        }

        public void RefreshContent()
        {
            //Replot();
        }

        public void PreparePlot()
        {
            if (_currentTask == null || _currentTask.IsCompleted)
            {
                //Prepare validation
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;

                Progress = 0;
                ProgressText = "Loading new messages";
                _currentTask = Unitool.ValidateMatches(_api, _stats, (x) => Progress = x, token);

                //Prepare ploting
                Action<object> continuation = (x) =>
                {
                    Replot();
                    Progress = 100;
                    ProgressText = "Complete!";
                };
                _currentTask.ContinueWith(continuation);
                
                _currentTask.Start();
            }
        }

        public void Replot()
        {
            _stats.PlotMessagesThroughTime(StartingDate, EndingDate, TotalThroughTimePlot, SentThroughTimePlot, ReceivedThroughTimePlot);
            PlotTitle = $"Messages per day. Response rate: {_stats.ResponseRate(StartingDate, EndingDate):F2}, Average convo length: {_stats.AverageConversationLength(StartingDate, EndingDate):F2}";
            PlotUpperConstraint = TotalThroughTimePlot.Points.Max((x) => x.Y) + 5.0;

            TimeSpan timespan = EndingDate.Subtract(StartingDate);
            if (timespan.Days <= 360)
            {
                DateTimeAxisFormat = "dd.MM"; 
            }
            else
            {
                DateTimeAxisFormat = "dd.MM.yyyy";
            }

            NotifyOfPropertyChange(() => TotalThroughTimePlot);
            NotifyOfPropertyChange(() => SentThroughTimePlot);
            NotifyOfPropertyChange(() => ReceivedThroughTimePlot);
            NotifyOfPropertyChange(() => PlotTitle);
        }

        public void Button_Refresh()
        {
            PreparePlot();
        }

        
    }
}
