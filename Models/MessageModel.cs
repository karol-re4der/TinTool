using System;
using System.Collections.Generic;
using System.Text;

namespace Tintool.Models
{
    public class MessageModel
    {
        public string Text { get; set; }
        public string ReceiverId { get; set; }
        public DateTime Date { get; set; }

        public MessageModel()
        {

        }
    }
}
