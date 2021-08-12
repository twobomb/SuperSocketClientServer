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
        private byte[] reserveBuff;
        public DataRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest) {
            byte[] localBuffer = new byte[length];
            if (reserveBuff != null && reserveBuff.Length > 0)
            {
                localBuffer = new byte[reserveBuff.Length + length];
                Buffer.BlockCopy(reserveBuff, 0, localBuffer, 0, reserveBuff.Length);
                Buffer.BlockCopy(readBuffer, offset, localBuffer, reserveBuff.Length, length);
                reserveBuff = null;
            }
            else
                Buffer.BlockCopy(readBuffer,offset,localBuffer,0,length);
            if (localBuffer.Length > 4) {
                byte[] head = new byte[4];
                Buffer.BlockCopy(localBuffer, 0, head,0,4);
                int sizeContent = BitConverter.ToInt32(head, 0);
                if (localBuffer.Length >= sizeContent + 4) {
                    BinaryFormatter formatter = new BinaryFormatter();
                    byte[] dataMessage = new byte[sizeContent];
                    Buffer.BlockCopy(localBuffer, 4, dataMessage, 0, sizeContent);
                    using (MemoryStream msTemp = new MemoryStream(dataMessage))
                    {
                        Message msg = (Message) formatter.Deserialize(msTemp);
                        rest = localBuffer.Length -  sizeContent - 4;
                        return new DataRequestInfo()
                        {
                         Key   = msg.key,
                         message = msg
                        };
                    }
                }
                else
                {
                    rest = 0;
                    reserveBuff = new byte[localBuffer.Length];
                    Buffer.BlockCopy(localBuffer, 0, reserveBuff, 0, localBuffer.Length);
                    return null;
                }
            }
            rest = 0;
            return null;
        }

        public void Reset()
        {
            reserveBuff = null;
        }

        public int LeftBufferSize { get; }
        public IReceiveFilter<DataRequestInfo> NextReceiveFilter { get; }
        public FilterState State { get; }
    }
}
