using Caliburn.Micro;
using Models;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using Tinder.DataStructures;
using Tintool.APIs.Badoo;
using Tintool.Models;
using Tintool.Models.DataStructures;
using Tintool.ViewModels.Dialogs;

namespace Tintool.ViewModels.Tabs
{
    class ConnectUserControlViewModel : Screen
    {
        #region fields
        private IWindowManager _wm;

        private TinderAPI _tinderAPI;
        private BadooAPI _badooAPI;
        private Stats _stats;
        private AppSettings _settings;
        private MainViewModel _baseViewModel;

        private Brush _tinderButtonColor = Brushes.Gray;
        public Brush TinderButtonColor
        {
            set
            {
                _tinderButtonColor = value;
                NotifyOfPropertyChange(() => TinderButtonColor);
            }
            get
            {
                return _tinderButtonColor;
            }
        }
        private Brush _badooButtonColor = Brushes.Gray;
        public Brush BadooButtonColor { 
            get 
            {
                return _badooButtonColor;
            }
            set
            {
                _badooButtonColor = value;
                NotifyOfPropertyChange(() => BadooButtonColor);
            }
        }

        private Brush ButtonActiveColor = Brushes.LimeGreen;
        private Brush ButtonInactiveColor = Brushes.White;
        #endregion

        public ConnectUserControlViewModel(IWindowManager wm, ref TinderAPI tinderAPI, ref BadooAPI badooAPI, ref Stats stats, ref AppSettings settings, MainViewModel baseViewModel)
        {
            this._wm = wm;
            this._tinderAPI = tinderAPI;
            this._badooAPI = badooAPI;
            this._stats = stats;
            this._settings = settings;
            this._baseViewModel = baseViewModel;

            RefreshContent();
        }

        #region methods
        public void RefreshContent()
        {
            if (_tinderAPI.IsTokenWorking())
            {
                TinderButtonColor = ButtonActiveColor;
            }
            else
            {
                TinderButtonColor = ButtonInactiveColor;
            }
        }
        public void TinderButtonClicked()
        {
            _wm.ShowDialog(new LoginDialogViewModel(_tinderAPI, _settings));

            TinderButtonColor = ButtonActiveColor;
        }
        public void BadooButtonClicked()
        {
            BadooButtonColor = ButtonActiveColor;

        }
        #endregion
    }
}
