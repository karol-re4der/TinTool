using Caliburn.Micro;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
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


        public void LoginAction()
        {
            API api = new API(_token);
            if (api.Authenticate())
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
    }
}
