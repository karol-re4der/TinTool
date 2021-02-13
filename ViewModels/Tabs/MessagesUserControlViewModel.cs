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
using Tintool.APIs.Badoo;

namespace Tintool.ViewModels.Tabs
{
    class MessagesUserControlViewModel: Screen
    {
        private IWindowManager _wm;

        private BadooAPI _badooAPI;
        private TinderAPI _tinderAPI;
        private Stats _stats;
        private AppSettings _settings;
        private MainViewModel _baseViewModel;

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

        public MessagesUserControlViewModel(IWindowManager wm, ref TinderAPI tinderAPI, ref BadooAPI badooAPI, ref Stats stats, ref AppSettings settings, MainViewModel baseViewModel)
        {
            this._wm = wm;
            this._tinderAPI = tinderAPI;
            this._badooAPI = badooAPI;
            this._stats = stats;
            this._settings = settings;
            this._baseViewModel = baseViewModel;

            SetTime(_stats.Date.Date, DateTime.Today);
        }

        private void SetTime(DateTime start, DateTime end)
        {
            StartingDate = start;
            EndingDate = end;
            StartingDate = start;
        }

        public void RefreshContent()
        {
            //Replot();
        }

        public void PreparePlot()
        {
            if (_baseViewModel.CurrentTask == null || _baseViewModel.CurrentTask.IsCompleted)
            {
                //Prepare validation
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;

                _baseViewModel.Progress = 0;
                _baseViewModel.ProgressText = "Loading new messages";
                _baseViewModel.CurrentTask = Unitool.ValidateMatches(_tinderAPI, _stats, (x) => _baseViewModel.Progress = x, token);

                //Prepare ploting
                Action<object> continuation = (x) =>
                {
                    Replot();
                    _baseViewModel.Progress = 100;
                    _baseViewModel.ProgressText = "Complete!";
                };
                _baseViewModel.CurrentTask.ContinueWith(continuation);

                _baseViewModel.CurrentTask.Start();
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

        #region Buttons
        public void Button_Refresh()
        {
            PreparePlot();
        }

        public void Button_DefaultTimeframe()
        {
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime end = start.AddMonths(1).AddDays(-1);

            SetTime(start, end);
        }

        public void Button_FullTimeframe()
        {
            DateTime start = _stats.Date.Date;
            DateTime end = DateTime.Now.Date;

            SetTime(start, end);
        }
        #endregion
    }
}
