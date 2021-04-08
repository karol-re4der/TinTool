using Caliburn.Micro;
using Models;
using System.Threading.Tasks;
using System.Windows;
using Tintool.Models.Saveables;

namespace Tintool.ViewModels.Dialogs
{
    class TinderLoginDialogViewModel:Screen
    {
        private MainViewModel _baseViewModel;

        public string Number { get; set; }
        public string PhoneCode { get; set; }
        public string MailCode { get; set; }

        public SessionModel Session { get; set; }

        public TinderLoginDialogViewModel(MainViewModel baseViewModel)
        {
            _baseViewModel = baseViewModel;

            Number = _baseViewModel.Settings.LoginNumber;
        }

        private async void Authenticate()
        {
            if (!string.IsNullOrWhiteSpace(MailCode) && !string.IsNullOrWhiteSpace(Number) && !string.IsNullOrWhiteSpace(PhoneCode))
            {
                if (await RequestTokenWithMailCode())
                {
                    FinalizeLogin();
                }
                else
                {
                    MessageBox.Show("Something went wrong!");
                }
            }
            else if (!string.IsNullOrWhiteSpace(PhoneCode) && !string.IsNullOrWhiteSpace(Number))
            {
                if (!await RequestTokenWithPhoneCode())
                {
                    if (!string.IsNullOrWhiteSpace(_baseViewModel.TinderAPI.GetSession()?.AuthToken))
                    {
                        FinalizeLogin();
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong! Is mail code required?");
                    }
                }
            }
            else if (!string.IsNullOrWhiteSpace(Number))
            {
                if (!await RequestPhoneCode())
                {
                    MessageBox.Show("Something went wrong!");
                }
            }
            else
            {
                MessageBox.Show("Enter credentials first!");
            }
        }

        public async Task<bool> RequestTokenWithPhoneCode()
        {
            await Task.Delay(1);
            Session = _baseViewModel.TinderAPI.RequestNewSessionWithPhoneCode(PhoneCode, Number);
            if (!string.IsNullOrWhiteSpace(Session?.AuthToken))
            {
                _baseViewModel.TinderAPI.SetSession(Session);
                return true;
            }
            else if (string.IsNullOrWhiteSpace(_baseViewModel.TinderAPI.GetSession()?.AuthToken))
            {
                _baseViewModel.TinderAPI.SetSession(Session);
                return false;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RequestTokenWithMailCode()
        {
            await Task.Delay(1);
            Session = _baseViewModel.TinderAPI.RequestNewSessionWithEmailCode(MailCode, Session);
            if (!string.IsNullOrWhiteSpace(Session?.AuthToken))
            {
                _baseViewModel.TinderAPI.SetSession(Session);
                return true;
            }
            else if (string.IsNullOrWhiteSpace(_baseViewModel.TinderAPI.GetSession()?.AuthToken))
            {
                _baseViewModel.TinderAPI.SetSession(Session);
                return false;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RequestPhoneCode()
        {
            await Task.Delay(1);
            if (_baseViewModel.TinderAPI.RequestLoginCode(Number))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> LogByToken()
        {
            await Task.Delay(1);
            if (_baseViewModel.TinderAPI.IsTokenWorking())
            {
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
            FileManager.SaveSession(_baseViewModel.TinderAPI.GetSession(), "tinder");
            _baseViewModel.RefreshStatusIcons();
            TryClose();
        }

        public void LoginButtonClicked()
        {
            Authenticate();
        }
    }
}
