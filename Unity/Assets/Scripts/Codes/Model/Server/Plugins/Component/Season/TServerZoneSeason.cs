using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class TServerZoneSeason : Entity, IAwake<int>, ISerializeToEntity
    {
        public int ConfigId;

        public long StartTime;
        public long EndTime;
    }
}
