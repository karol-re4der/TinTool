using Caliburn.Micro;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tintool.Views;

namespace Tintool.ViewModels
{
    class LoginViewModel:Screen
    {
        private IWindowManager wm;
        private API _api;

        private string _token;
        public string Token
        {
            get
            {
                return _token;
            }
            set {
                _token = value;
                NotifyOfPropertyChange(()=>Token);
            }
        }

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

        private string _login;
        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value;
                NotifyOfPropertyChange(() => Login);
            }
        }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                NotifyOfPropertyChange(()=>Password);
            }
        }

        private bool _codeSent = false;
        public string CodeSent
        {
            get
            {
                return _codeSent ? "Visible" : "Hidden";
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

        Task<bool> loginTask;


        public LoginViewModel(IWindowManager wm)
        {
            this.wm = wm;

            string loadedToken = FileManager.LoadToken();
            if (loadedToken?.Length > 0)
            {
                Token = loadedToken;
            }
        }

        private async void Authenticate()
        {
            if (loginTask == null || loginTask.IsCompleted)
            {
                if(!string.IsNullOrWhiteSpace(Code))
                {
                    if(await LogByCode())
                    {
                        FinalizeLogin();
                    }
                    else
                    {
                        MessageBox.Show("Cannot authenticate: wrong code!");
                    }
                }
                else if (string.IsNullOrWhiteSpace(Token) && (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Login)))
                {
                    MessageBox.Show("Cannot authenticate: need token, or login and password filled in");
                }
                else if (string.IsNullOrWhiteSpace(Token))
                {
                    if (await LogByCredentials())
                    {
                        _codeSent = true;
                        NotifyOfPropertyChange(() => CodeSent);
                    }
                }
                else
                {
                    if (await LogByToken())
                    {
                        FinalizeLogin();
                    }
                }
            }
        }

        public async Task<bool> LogByCode()
        {
            await Task.Delay(1);
            return false;
        }

        public async Task<bool> LogByCredentials()
        {
            await Task.Delay(1);
            _api = new API();
            if (_api.SendLoginCode(Login))
            {
                return true;
            }
            else
            {
                MessageBox.Show("Cannot authenticate: invalid login and/or password");
                return false;
            }
        }

        public async Task<bool> LogByToken()
        {
            await Task.Delay(1);
            _api = new API(_token);
            if (_api.CheckToken())
            {
                if (KeepLogged)
                {
                    FileManager.SaveToken(_token);
                }
                return true;
            }
            else
            {
                MessageBox.Show("Cannot authenticate: invalid token");
                return false;
            }
        }

        public void FinalizeLogin()
        {
            wm.ShowWindow(new LoggedViewModel(wm, _api));
            TryClose();
        }

        public void Button_Authenticate()
        {
            Authenticate();
        }
    }
}
