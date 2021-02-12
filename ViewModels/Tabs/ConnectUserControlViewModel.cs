using Caliburn.Micro;
using Models;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using Tinder.DataStructures;
using Tintool.Models;
using Tintool.Models.DataStructures;
using Tintool.ViewModels.Dialogs;

namespace Tintool.ViewModels.Tabs
{
    class ConnectUserControlViewModel : Screen
    {
        #region fields
        private IWindowManager _wm;

        private TinderAPI _api;
        private Stats _stats;
        private AppSettings _settings;
        private LoggedViewModel _baseViewModel;

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

        public ConnectUserControlViewModel(IWindowManager wm, ref TinderAPI api, ref Stats stats, ref AppSettings settings, LoggedViewModel baseViewModel)
        {
            this._wm = wm;
            this._api = api;
            this._stats = stats;
            this._settings = settings;
            this._baseViewModel = baseViewModel;

            RefreshContent();
        }

        #region methods
        public void RefreshContent()
        {
            if (_api.IsTokenWorking())
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
            _wm.ShowDialog(new LoginDialogViewModel(_api, _settings));

            TinderButtonColor = ButtonActiveColor;
        }
        public void BadooButtonClicked()
        {
            BadooButtonColor = ButtonActiveColor;

        }
        #endregion
    }
}
