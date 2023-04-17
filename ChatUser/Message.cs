using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatUser
{
    internal class Message
    {
        public string SenderName { get; set; }
        public string? ReceiverName { get; set; }
        public string Content { get; set; }
    }
}
