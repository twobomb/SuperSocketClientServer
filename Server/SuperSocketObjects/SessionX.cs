using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;

namespace Server.SuperSocketObjects
{
    public class SessionX : AppSession< SessionX, DataRequestInfo> {
        protected override void OnSessionStarted()
        {
            Console.WriteLine("{0}: Session created {1} from {2}", AppServer.Name, SessionID, RemoteEndPoint.Address.ToString());
            base.OnSessionStarted();
        }


        protected override void OnSessionClosed(CloseReason reason)
        {
            Console.WriteLine("{0}: Session closed {1}", AppServer.Name, SessionID);
            base.OnSessionClosed(reason);
        }

    }
}
