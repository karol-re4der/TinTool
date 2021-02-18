using Caliburn.Micro;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tintool.Models.Saveables;

namespace Tintool.ViewModels.Dialogs
{
    class LoginDialogViewModel:Screen
    {
        private MainViewModel _baseViewModel;

        public string Number { get; set; }
        public string Code { get; set; }

        public LoginDialogViewModel(MainViewModel baseViewModel)
        {
            _baseViewModel = baseViewModel;

            Number = _baseViewModel.Settings.LoginNumber;
        }

        private async void Authenticate()
        {
            if (!string.IsNullOrWhiteSpace(Number) && !string.IsNullOrWhiteSpace(Code))
            {
                if (await RequestTokenWithCode())
                {
                    FinalizeLogin();
                }
                else
                {
                    MessageBox.Show("Cannot authenticate!");
                }
            }
            else if (!string.IsNullOrWhiteSpace(Number))
            {
                if (!await RequestCode())
                {
                    MessageBox.Show("Cannot request code!");
                }
            }
            else
            {
                MessageBox.Show("Enter phone number first!");
            }
        }

        public async Task<bool> RequestTokenWithCode()
        {
            await Task.Delay(1);
            SessionModel session = _baseViewModel.TinderAPI.RequestNewSessionWithPhoneCode(Code, Number);
            if (!string.IsNullOrWhiteSpace(session?.AuthToken))
            {
                _baseViewModel.TinderAPI.SetSession(session);
                return true;
            }
            else if (string.IsNullOrWhiteSpace(_baseViewModel.TinderAPI.GetSession()?.AuthToken))
            {
                _baseViewModel.TinderAPI.SetSession(session);
                return false;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RequestCode()
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
                FileManager.SaveSession(_baseViewModel.TinderAPI.GetSession());
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
            FileManager.SaveSession(_baseViewModel.TinderAPI.GetSession());
            _baseViewModel.RefreshStatusIcons();
            TryClose();
        }

        public void LoginButtonClicked()
        {
            Authenticate();
        }
    }
}
