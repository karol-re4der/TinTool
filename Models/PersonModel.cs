using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Tintool.Models
{
    public class PersonModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }

        [JsonIgnore]
        public int Distance { get; set; }

        [JsonIgnore]
        public string SCode { get; set; }
    }
}
