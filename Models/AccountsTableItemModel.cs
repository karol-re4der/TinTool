using System;
using System.Collections.Generic;
using System.Text;
using Tinder.DataStructures;

namespace Tintool.Models
{
    class AccountsTableItemModel
    {
        public string ID { get; set; }
        public string Platform { get; set; }
        public int Matches { get; set; }
        public string FileName { get; set; }

        public Stats LinkedStats { get; set; }
    }
}
