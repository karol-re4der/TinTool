using Caliburn.Micro;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tintool.Models.DataStructures;
using Tintool.Views;

namespace Tintool.ViewModels
{
    class LoginViewModel:Screen
    {
        private IWindowManager _wm;
        private API _api;
        private AppSettings _settings;

        private string _code;
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
                NotifyOfPropertyChange(() => Code);
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                _phoneNumber = value;
                NotifyOfPropertyChange(() => PhoneNumber);
            }
        }

        private bool _codeSent = false;
        public string CodeSent
        {
            get
            {
                return _codeSent ? "True" : "False";
            }
        }
        public string CodeSentNegation
        {
            get
            {
                return _codeSent ? "False" : "True";
            }
        }

        private bool _keepLogged = true;
        public bool KeepLogged
        {
            get
            {
                return _keepLogged;
            }
            set
            {
                _keepLogged = value;
                NotifyOfPropertyChange(() => KeepLogged);
            }
        }



        public LoginViewModel(IWindowManager wm)
        {
            FileManager.Prepare();

            this._wm = wm;
            _api = new API();
            _settings = FileManager.LoadSettings();
            KeepLogged = _settings.KeepLogged;
            PhoneNumber = _settings.LoginNumber;
        }

        protected override void OnActivate()
        {
            if (KeepLogged)
            {
                string loadedToken = FileManager.LoadToken();
                if (loadedToken?.Length > 0)
                {
                    _api.SetToken(loadedToken);
                    if (_api.IsTokenWorking())
                    {
                        FinalizeLogin();
                    }
                }
            }
        }

        private async void Authenticate()
        {
            if (!string.IsNullOrWhiteSpace(Code))
            {
                if (await RequestToken())
                {
                    FinalizeLogin();
                }
                else
                {
                    MessageBox.Show("Cannot authenticate!");
                }
            }
            else if (!string.IsNullOrWhiteSpace(PhoneNumber))
            {
                if (await RequestCode())
                {
                    _codeSent = true;
                    NotifyOfPropertyChange(() => CodeSent);
                    NotifyOfPropertyChange(() => CodeSentNegation);
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

        public async Task<bool> RequestToken()
        {
            await Task.Delay(1);
            string token = _api.RequestAuthToken(Code, PhoneNumber);
            if (!string.IsNullOrWhiteSpace(token))
            {
                _api.SetToken(token);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RequestCode()
        {
            await Task.Delay(1);
            if (_api.RequestLoginCode(PhoneNumber))
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
                if (KeepLogged)
                {
                    FileManager.SaveToken(_api.GetToken());
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void FinalizeLogin()
        {
            _settings.KeepLogged = KeepLogged;
            _settings.LoginNumber = PhoneNumber;
            FileManager.SaveSettings(_settings);
            FileManager.SaveToken(_api.GetToken());
            _wm.ShowWindow(new LoggedViewModel(_wm, _api, _settings));
            TryClose();
        }

        public void Button_Authenticate()
        {
            Authenticate();
        }
    }
}
