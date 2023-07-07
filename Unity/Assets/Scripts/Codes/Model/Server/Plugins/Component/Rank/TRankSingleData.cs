using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class TRankSingleData: Entity, IAwake, ISerializeToEntity
    {
        public long CharacterId;
        public string SteamAccountId;
        public int RankType;
        public int Score;
        public int RankIndex;

        public bool IsRobot = true;

        // 不需要同步的数据，真实数据
        [BsonIgnore]
        public bool IsMasterData = false;
    }
}

