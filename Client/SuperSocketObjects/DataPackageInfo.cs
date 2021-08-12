using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.ProtoBase;

namespace Client.SuperSocketObjects
{
    public class DataPackageInfo: IPackageInfo
    {
        public object Data { get; set; }
    }
}
