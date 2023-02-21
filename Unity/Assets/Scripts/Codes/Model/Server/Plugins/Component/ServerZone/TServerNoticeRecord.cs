using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class TServerNoticeRecord : Entity, IAwake
    {
        public string Notice;
        public long CreateTime;
        public HashSet<int> State = new HashSet<int>();
    }
}
