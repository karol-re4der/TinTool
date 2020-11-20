using Caliburn.Micro;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Tintool.Views;

namespace Tintool.ViewModels
{
    class LoginViewModel:Screen
    {
        private IWindowManager wm;

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
            try
            {
                API api = new API(_token);

                if (await api.Authenticate())
                {
                    FileManager.SaveToken(_token);
                    LoggedViewModel loggedViewModel = new LoggedViewModel(wm, api);
                    wm.ShowWindow(loggedViewModel);
                    TryClose();
                }
                else
                {
                    MessageBox.Show("Cannot authenticate");
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Error authenticating: " + e.Message);
            }
        }
        public void LoginAction()
        {
            Authenticate();
        }
    }
}
