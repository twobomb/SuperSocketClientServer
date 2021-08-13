using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Core;
using SuperSocket.ProtoBase;

namespace Client.SuperSocketObjects
{
    class FixedRecieveFilterX : FixedHeaderReceiveFilter<DataPackageInfo>
    {
        public FixedRecieveFilterX()
            : base(4)
        {
        }
        public override DataPackageInfo ResolvePackage(IBufferStream bufferStream){
            bufferStream.Skip(4);
            return new DataPackageInfo(){
                Data = (Message)new BinaryFormatter().Deserialize(bufferStream.GetCurrentStream())
            };
        }

        protected override int GetBodyLengthFromHeader(IBufferStream bufferStream, int length)
        {
            byte[] head = new byte[4];
            bufferStream.Read(head,0,4);
            return BitConverter.ToInt32(head, 0);
        }
    }
}
