using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Tintool.ViewModels;

namespace Tintool
{
    class Bootstrapper: BootstrapperBase
    {
        private IWindowManager wm = new WindowManager();

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            LoginViewModel loginViewModel = new LoginViewModel(wm);
            try
            {
                wm.ShowWindow(loginViewModel);
            }
            catch(Exception ex)
            {

            }
        }

    }
}
