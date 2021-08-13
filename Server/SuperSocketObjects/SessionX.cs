using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Core;
using SuperSocket.SocketBase;

namespace Server.SuperSocketObjects
{
    public class SessionX : AppSession< SessionX, DataRequestInfo> {
        protected override void OnSessionStarted()
        {
            Console.WriteLine("{0}: Session created {1} from {2}", AppServer.Name, SessionID, RemoteEndPoint.Address.ToString());
            base.OnSessionStarted();
        }


        public void SendMessage(Message msg){
            if (!Connected){
                Console.WriteLine("Клиент отключен, не могу послать сообщение!");
                return;
            }
            using (MemoryStream ms = new MemoryStream()){
                ms.WriteByte(0);
                ms.WriteByte(0);
                ms.WriteByte(0);
                ms.WriteByte(0);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, msg);
                ms.Seek(0, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(ms.Length - 4), 0, 4);
                ms.Seek(0, SeekOrigin.End);
                Send(ms.ToArray());
            }
        }

        private void Send(byte[] arr){
            Console.WriteLine("Send packet {0} byte",arr.Length);
            Send(arr,0,arr.Length);
        }

        public void SendMessages(List<Message> msgs)
        {
            if (!Connected){
                Console.WriteLine("Клиент отключен, не могу послать сообщение!");
                return;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                foreach (var message in msgs)
                {
                    int pos = (int)ms.Length;
                    ms.Seek(pos, SeekOrigin.Begin);
                    ms.WriteByte(0);
                    ms.WriteByte(0);
                    ms.WriteByte(0);
                    ms.WriteByte(0);
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, message);
                    ms.Seek(pos, SeekOrigin.Begin);
                    ms.Write(BitConverter.GetBytes(ms.Length - 4 - pos), 0, 4);
                    ms.Seek(0, SeekOrigin.End);
                }
                Send(ms.ToArray());
            }
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            Console.WriteLine("{0}: Session closed {1}", AppServer.Name, SessionID);
            base.OnSessionClosed(reason);
        }

    }
}
