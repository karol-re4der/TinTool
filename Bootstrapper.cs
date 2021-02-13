using Caliburn.Micro;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Tintool.Models.DataStructures;
using Tintool.ViewModels;
using Tintool.Views;

namespace Tintool
{
    class Bootstrapper: BootstrapperBase
    {
        private IWindowManager _wm = new WindowManager();
        private TinderAPI _api = new TinderAPI();

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            _wm.ShowWindow(new StartupViewModel(_wm));
        }

    }
}
