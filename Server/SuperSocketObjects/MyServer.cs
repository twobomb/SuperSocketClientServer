using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace Server.SuperSocketObjects
{
    public class MyServer : AppServer<SessionX, DataRequestInfo>
    {

        public MyServer(): base(new DefaultReceiveFilterFactory<DataRecieveFilter, DataRequestInfo>())
        {

        }


        public override bool Start()
        {
            return base.Start();
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}
