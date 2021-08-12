using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    [Serializable]
    public class Message{
        public Message() {
        }
        public string key { get; set; }
        public object data { get; set; }
    }
}
