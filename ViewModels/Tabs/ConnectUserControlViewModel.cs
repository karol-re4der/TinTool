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
        private MainViewModel BaseViewModel { get; set; }

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

        public ConnectUserControlViewModel(MainViewModel baseViewModel)
        {
            BaseViewModel = baseViewModel;

            RefreshContent();
        }

        #region methods
        public void RefreshContent()
        {
            if (BaseViewModel.TinderAPI.IsTokenWorking())
            {
                TinderButtonColor = ButtonActiveColor;
            }
            else
            {
                TinderButtonColor = ButtonInactiveColor;
            }

            if (BaseViewModel.BadooAPI.IsTokenWorking())
            {
                BadooButtonColor = ButtonActiveColor;
            }
            else
            {
                BadooButtonColor = ButtonInactiveColor;
            }
        }
        public void TinderButtonClicked()
        {
            BaseViewModel.WM.ShowDialog(new LoginDialogViewModel(BaseViewModel.TinderAPI, BaseViewModel.Settings));

            TinderButtonColor = ButtonActiveColor;
        }
        public void BadooButtonClicked()
        {
            BadooButtonColor = ButtonActiveColor;

        }
        #endregion
    }
}
