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
using Tintool.Models;
using Tintool.APIs.Badoo;
using Tintool.Models.UI;

namespace Tintool.ViewModels.Tabs
{
    class MessagesUserControlViewModel: Screen
    {
        public MainViewModel BaseViewModel;

        #region Plots
        public PlotModel TotalThroughTimePlot { get; set; } = new PlotModel();
        public PlotModel SentThroughTimePlot { get; set; } = new PlotModel();
        public PlotModel ReceivedThroughTimePlot { get; set; } = new PlotModel();

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

        public MessagesUserControlViewModel(MainViewModel baseViewModel)
        { 
            BaseViewModel = baseViewModel;

            SetTime(BaseViewModel.Stats.Date.Date, DateTime.Today);
        }

        private void SetTime(DateTime start, DateTime end)
        {
            StartingDate = start;
            EndingDate = end;
            StartingDate = start;
        }

        public void RefreshContent()
        {
            Replot();
        }

        public void PreparePlot()
        {
            if (BaseViewModel.CurrentTask == null || BaseViewModel.CurrentTask.IsCompleted)
            {
                //Prepare validation
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;

                BaseViewModel.Progress = 0;
                BaseViewModel.ProgressText = "Loading new messages";
                BaseViewModel.CurrentTask = Unitool.ValidateMatches(BaseViewModel.TinderAPI, BaseViewModel.Stats, (x) => BaseViewModel.Progress = x, token);

                //Prepare ploting
                Action<object> continuation = (x) =>
                {
                    Replot();
                    BaseViewModel.Progress = 100;
                    BaseViewModel.ProgressText = "Complete!";
                };
                BaseViewModel.CurrentTask.ContinueWith(continuation);

                BaseViewModel.CurrentTask.Start();
            }
        }

        public void Replot()
        {
            BaseViewModel.Stats.PlotMessagesThroughTime(StartingDate, EndingDate, TotalThroughTimePlot, SentThroughTimePlot, ReceivedThroughTimePlot);
            PlotTitle = $"Messages per day. Response rate: {BaseViewModel.Stats.ResponseRate(StartingDate, EndingDate):F2}, Average convo length: {BaseViewModel.Stats.AverageConversationLength(StartingDate, EndingDate):F2}";
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
            DateTime start = BaseViewModel.Stats.Date.Date;
            DateTime end = DateTime.Now.Date;

            SetTime(start, end);
        }
        #endregion
    }
}
