using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Core;
using SuperSocket.SocketBase.Protocol;

namespace Server.SuperSocketObjects
{
    class DataRecieveFilter : IReceiveFilter<DataRequestInfo> {
        public DataRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest) {
            if (length> 4) {
                byte[] head = new byte[4];
                Buffer.BlockCopy(readBuffer, offset, head,0,4);
                int sizeContent = BitConverter.ToInt32(head, 0);
                if (length >= sizeContent + 4) {
                    BinaryFormatter formatter = new BinaryFormatter();
                    byte[] dataMessage = new byte[sizeContent];
                    Buffer.BlockCopy(readBuffer, offset+4, dataMessage, 0, sizeContent);
                    using (MemoryStream msTemp = new MemoryStream(dataMessage))
                    {
                        Message msg = (Message) formatter.Deserialize(msTemp);
                        rest = length - sizeContent - 4;
                        return new DataRequestInfo()
                        {
                         Key   = msg.key,
                         message = msg
                        };
                    }
                }
                else
                {
                    rest = length;
                    return null;
                }
            }
            rest = 0;
            return null;
        }

        public void Reset()
        {
        }

        public int LeftBufferSize { get; }
        public IReceiveFilter<DataRequestInfo> NextReceiveFilter { get; }
        public FilterState State { get; }
    }
}
