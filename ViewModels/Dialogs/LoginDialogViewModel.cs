using Caliburn.Micro;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tintool.Models.DataStructures;

namespace Tintool.ViewModels.Dialogs
{
    class LoginDialogViewModel:Screen
    {
        private TinderAPI _api;
        private AppSettings _settings;

        public string Number { get; set; }
        public string Code { get; set; }

        public LoginDialogViewModel(TinderAPI api, AppSettings set)
        {
            _api = api;
            _settings = set;

            Number = _settings.LoginNumber;
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
            SessionData session = _api.RequestNewSessionWithPhoneCode(Code, Number);
            if (!string.IsNullOrWhiteSpace(session?.AuthToken))
            {
                _api.SetSession(session);
                return true;
            }
            else if (string.IsNullOrWhiteSpace(_api.GetSession()?.AuthToken))
            {
                _api.SetSession(session);
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
            if (_api.RequestLoginCode(Number))
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
            if (_api.IsTokenWorking())
            {
                FileManager.SaveSession(_api.GetSession());
                return true;
            }
            else
            {
                return false;
            }
        }

        public void FinalizeLogin()
        {
            _settings.LoginNumber = Number;
            FileManager.SaveSettings(_settings);
            FileManager.SaveSession(_api.GetSession());
            TryClose();
        }

        public void LoginButtonClicked()
        {
            Authenticate();
        }
    }
}
