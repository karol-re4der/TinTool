using Caliburn.Micro;
using Models;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Tintool.Models.Saveables;
using System.Linq;

namespace Tintool.ViewModels.Dialogs
{
    class BadooLoginDialogViewModel:Screen
    {
        private MainViewModel _baseViewModel;

        public string Number { get; set; }
        public PasswordBox PassBox { get
            {
                return (PasswordBox)(this.Views.First().Value as FrameworkElement).FindName("PassBox");
            }
        }

        public BadooLoginDialogViewModel(MainViewModel baseViewModel)
        {
            _baseViewModel = baseViewModel;

            Number = _baseViewModel.Settings.LoginNumber;
        }

        private async void Authenticate()
        {
            if(string.IsNullOrWhiteSpace(Number) || string.IsNullOrWhiteSpace(PassBox.Password))
            {
                MessageBox.Show("Enter credentials!");
            }
            else
            {
                if (await RequestSessionWithCredentials())
                {
                    if (await LogByToken())
                    {
                        FinalizeLogin();
                    }
                    else
                    {
                        MessageBox.Show("Cannot authenticate!");
                    }
                }
                else
                {
                    MessageBox.Show("Cannot authenticate!");
                }
            }
        }

        public async Task<bool> RequestSessionWithCredentials()
        {
            await Task.Delay(1);
            SessionModel session = _baseViewModel.BadooAPI.RequestNewSessionWithCredentials(Number, PassBox.Password);
            if (!string.IsNullOrWhiteSpace(session?.AuthToken))
            {
                _baseViewModel.BadooAPI.SetSession(session);
                return true;
            }
            else if (string.IsNullOrWhiteSpace(_baseViewModel.BadooAPI.GetSession()?.AuthToken))
            {
                _baseViewModel.BadooAPI.SetSession(session);
                return false;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> LogByToken()
        {
            await Task.Delay(1);
            if (_baseViewModel.BadooAPI.IsTokenWorking())
            {
                FileManager.SaveSession(_baseViewModel.BadooAPI.GetSession(), "badoo");
                return true;
            }
            else
            {
                return false;
            }
        }

        public void FinalizeLogin()
        {
            _baseViewModel.Settings.LoginNumber = Number;
            FileManager.SaveSettings(_baseViewModel.Settings);
            FileManager.SaveSession(_baseViewModel.BadooAPI.GetSession(), "badoo");
            _baseViewModel.RefreshStatusIcons();
            TryClose();
        }

        public void LoginButtonClicked()
        {
            Authenticate();
        }
    }
}
