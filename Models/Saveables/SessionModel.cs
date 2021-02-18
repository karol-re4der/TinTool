using System;
using System.Collections.Generic;
using System.Text;

namespace Tintool.Models.Saveables
{
    public class SessionModel
    {
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
