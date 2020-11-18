using System;
using System.Collections.Generic;
using System.Text;

namespace Tintool.Models.DataStructures
{
    public class MatchData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ResponseStatus { get; set; }
        public DateTime CreationDate { get; set; }

        public MatchData()
        {

        }
    }
}
