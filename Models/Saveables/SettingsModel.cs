using System;
using System.Collections.Generic;
using System.Text;

namespace Tintool.Models.Saveables
{
    class SettingsModel
    {
        public bool KeepLogged { get; set; } = false;
        public string LoginNumber { get; set; } = "";
        public string DefaultSaveFile { get; set; } = "";
    }
}
