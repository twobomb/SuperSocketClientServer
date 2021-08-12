using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using SuperSocket.SocketBase.Protocol;

namespace Server.SuperSocketObjects
{
    public class DataRequestInfo : IRequestInfo
    {
        public string Key { get; set; }
        public Message message { get; set; }
    }
}
