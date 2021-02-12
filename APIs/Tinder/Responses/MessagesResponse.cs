using System;
using System.Collections.Generic;
using System.Text;

namespace Tintool.Models.DataStructures.Responses.Messages
{
    public class Meta
    {
        public int status { get; set; }
    }

    public class Message
    {
        public string _id { get; set; }
        public string match_id { get; set; }
        public DateTime sent_date { get; set; }
        public string message { get; set; }
        public string to { get; set; }
        public string from { get; set; }
        public DateTime created_date { get; set; }
        public object timestamp { get; set; }
    }

    public class Data
    {
        public List<Message> messages { get; set; }
    }

    public class MessagesResponse
    {
        public Meta meta { get; set; }
        public Data data { get; set; }
    }
}

