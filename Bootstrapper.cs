using Caliburn.Micro;
using Models;
using System.Windows;
using Tintool.ViewModels;

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
