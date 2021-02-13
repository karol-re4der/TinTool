using Caliburn.Micro;
using Models;
using OxyPlot.Axes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tinder.DataStructures;
using Tintool.APIs.Badoo;
using Tintool.Models;
using Tintool.Models.DataStructures;

namespace Tintool.ViewModels
{
    class MatchesUserControlViewModel : Screen
    {
        private IWindowManager _wm;

        private TinderAPI _tinderAPI;
        private BadooAPI _badooAPI;
        private Stats _stats;
        private AppSettings _settings;
        private MainViewModel _baseViewModel;

        #region Plots
        public PlotData TotalThroughTimePlot { get; set; } = new PlotData();
        public PlotData RegularThroughTimePlot { get; set; } = new PlotData();
        public PlotData SuperThroughTimePlot { get; set; } = new PlotData();
        public PlotData ExpThroughTimePlot { get; set; } = new PlotData();
        public PlotData BoostsThroughTimePlot { get; set; } = new PlotData();
        public PlotData FastThroughTimePlot { get; set; } = new PlotData();


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
        private bool _regularVisible = false;
        public string RegularVisibleAsString
        {
            get
            {
                return _regularVisible ? "Visible" : "Hidden";
            }
        }
        public bool RegularVisible
        {
            get
            {
                return _regularVisible;
            }
            set
            {
                _regularVisible = value;
                NotifyOfPropertyChange(() => RegularVisible);
                NotifyOfPropertyChange(() => RegularVisibleAsString);
            }
        }
        private bool _superVisible = false;
        public string SuperVisibleAsString
        {
            get
            {
                return _superVisible ? "Visible" : "Hidden";
            }
        }
        public bool SuperVisible
        {
            get
            {
                return _superVisible;
            }
            set
            {
                _superVisible = value;
                NotifyOfPropertyChange(() => SuperVisible);
                NotifyOfPropertyChange(() => SuperVisibleAsString);
            }
        }
        private bool _expVisible = false;
        public string ExpVisibleAsString
        {
            get
            {
                return _expVisible ? "Visible" : "Hidden";
            }
        }
        public bool ExpVisible
        {
            get
            {
                return _expVisible;
            }
            set
            {
                _expVisible = value;
                NotifyOfPropertyChange(() => ExpVisible);
                NotifyOfPropertyChange(() => ExpVisibleAsString);
            }
        }
        private bool _boostsVisible = false;
        public string BoostsVisibleAsString
        {
            get
            {
                return _boostsVisible ? "Visible" : "Hidden";
            }
        }
        public bool BoostsVisible
        {
            get
            {
                return _boostsVisible;
            }
            set
            {
                _boostsVisible = value;
                NotifyOfPropertyChange(() => BoostsVisible);
                NotifyOfPropertyChange(() => BoostsVisibleAsString);
            }
        }
        private bool _fastVisible = false;
        public string FastVisibleAsString
        {
            get
            {
                return _fastVisible ? "Visible" : "Hidden";
            }
        }
        public bool FastVisible
        {
            get
            {
                return _fastVisible;
            }
            set
            {
                _fastVisible = value;
                NotifyOfPropertyChange(() => FastVisible);
                NotifyOfPropertyChange(() => FastVisibleAsString);
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

        public MatchesUserControlViewModel(IWindowManager wm, ref TinderAPI tinderAPI, ref BadooAPI badooAPI, ref Stats stats, ref AppSettings settings, MainViewModel baseViewModel)
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
            Replot();
        }

        public void PreparePlot()
        {
            _baseViewModel.ProgressText = "Searching for new matches!";
            _baseViewModel.Progress = 0;
            Unitool.LogNewMatches(_tinderAPI.GetMatches(100), _stats);
            Replot();
            _baseViewModel.ProgressText = "Complete!";
            _baseViewModel.Progress = 100;
        }

        public void Replot()
        {
            float avg = _stats.AverageMatchesPerDay(StartingDate, EndingDate);
            TotalThroughTimePlot.Title = $"Matches per day. Average: ({avg:F2})";
            _stats.PlotMatchesThroughTime(StartingDate, EndingDate, TotalThroughTimePlot, RegularThroughTimePlot, SuperThroughTimePlot, BoostsThroughTimePlot, FastThroughTimePlot, ExpThroughTimePlot);
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
            NotifyOfPropertyChange(() => RegularThroughTimePlot);
            NotifyOfPropertyChange(() => SuperThroughTimePlot);
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
