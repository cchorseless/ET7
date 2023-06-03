using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    public class ServerZoneBattleTeamComponent: Entity, IAwake
    {
        [BsonIgnore]
        public TServerZone ServerZone
        {
            get => GetParent<TServerZone>();
        }

        [BsonIgnore]
        public readonly Dictionary<int, Dictionary<long, int>> RoundTBattleTeam = new Dictionary<int, Dictionary<long, int>>();
    }
}