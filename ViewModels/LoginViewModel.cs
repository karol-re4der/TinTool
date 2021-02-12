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
        private TinderAPI _api;
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

        private string _mailCode;
        public string MailCode
        {
            get
            {
                return _mailCode;
            }
            set
            {
                _mailCode = value;
                NotifyOfPropertyChange(() => MailCode);
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

        private bool _mailCodeSent = false;
        public string MailCodeSent
        {
            get
            {
                return _mailCodeSent ? "True" : "False";
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
            _api = new TinderAPI();
            _settings = FileManager.LoadSettings();
            KeepLogged = _settings.KeepLogged;
            PhoneNumber = _settings.LoginNumber;
        }

        protected override void OnActivate()
        {
            if (KeepLogged)
            {
                SessionData loadedSession = FileManager.LoadSession();
                if (loadedSession?.AuthToken.Length > 0)
                {
                    _api.SetSession(loadedSession);
                    if (_api.IsTokenWorking())
                    {
                        FinalizeLogin();
                    }
                    else if (loadedSession?.RefreshToken.Length > 0)
                    {
                        loadedSession = _api.TryRefresh(loadedSession);

                        if (loadedSession != null)
                        {
                            _api.SetSession(loadedSession);
                            FinalizeLogin();
                        }
                    }
                }
            }
        }

        private async void Authenticate()
        {
            if (!string.IsNullOrWhiteSpace(MailCode))
            {
                if (await RequestTokenWithMail())
                {
                    FinalizeLogin();
                }
                else
                {
                    MessageBox.Show("Cannot authenticate!");
                }
            }
            else if (!string.IsNullOrWhiteSpace(Code))
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

        public async Task<bool> RequestTokenWithCode()
        {
            await Task.Delay(1);
            SessionData session = _api.RequestNewSessionWithPhoneCode(Code, PhoneNumber);
            if (!string.IsNullOrWhiteSpace(session?.AuthToken))
            {
                _api.SetSession(session);
                return true;
            }
            else if (string.IsNullOrWhiteSpace(_api.GetSession()?.AuthToken))
            {
                _api.SetSession(session);
                _codeSent = false;
                _mailCodeSent = true;
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
            if (_api.RequestLoginCode(PhoneNumber))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RequestTokenWithMail()
        {
            await Task.Delay(1);
            SessionData session = _api.RequestNewSessionWithEmailCode(MailCode, _api.GetSession());
            if (!string.IsNullOrWhiteSpace(session?.AuthToken))
            {
                _api.SetSession(session);
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
                    FileManager.SaveSession(_api.GetSession());
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
            FileManager.SaveSession(_api.GetSession());
            _wm.ShowWindow(new LoggedViewModel(_wm, _api, _settings));
            TryClose();
        }

        public void Button_Authenticate()
        {
            Authenticate();
        }
    }
}
