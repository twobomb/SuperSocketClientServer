using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.ProtoBase;

namespace Client.SuperSocketObjects
{
    class RecieveFilterX : IReceiveFilter<DataPackageInfo>
    {
        public DataPackageInfo Filter(BufferList data, out int rest)
        {
            Console.WriteLine("filter");
            rest = 0;
            return null;
            //throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public IReceiveFilter<DataPackageInfo> NextReceiveFilter { get; }
        public FilterState State { get; }
    }
}
