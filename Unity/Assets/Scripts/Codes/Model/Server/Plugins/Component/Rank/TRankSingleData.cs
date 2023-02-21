using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class TRankSingleData : Entity, IAwake, ISerializeToEntity
    {
        public long CharacterId;
        public int Score;
        public int RankIndex;

    }
}
