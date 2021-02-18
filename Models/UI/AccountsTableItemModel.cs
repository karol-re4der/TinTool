using System;
using System.Collections.Generic;
using System.Text;
using Tintool.Models.Saveables;

namespace Tintool.Models.UI
{
    class AccountsTableItemModel
    {
        public string ID { get; set; }
        public string Platform { get; set; }
        public int Matches { get; set; }
        public string FileName { get; set; }

        public StatsModel LinkedStats { get; set; }
    }
}
