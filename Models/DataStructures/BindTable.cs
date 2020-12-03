using System;
using System.Collections.Generic;
using System.Text;

namespace Tintool.Models.DataStructures
{
    public class BindTableItemModel
    {
        public string SaveFileName { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class BindTable
    {
        public List<BindTableItemModel> bindings { get; set; } = new List<BindTableItemModel>();
    }
}
