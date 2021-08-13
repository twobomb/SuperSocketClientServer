using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Core;
using SuperSocket.Facility.Protocol;

namespace Server.SuperSocketObjects
{
    public class DataFixedRecieveFilter: FixedHeaderReceiveFilter<DataRequestInfo>
    {
        public DataFixedRecieveFilter() : base(4){
        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length){
            return BitConverter.ToInt32(header,offset);
        }

        protected override DataRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            using (MemoryStream ms = new MemoryStream(bodyBuffer, offset, length))
            {
                BinaryFormatter bf = new BinaryFormatter();
                Message msg = (Message)bf.Deserialize(ms);
                return new DataRequestInfo()
                {
                    Key = msg.key,
                    message = msg
                };
            }

        }
    }
}
