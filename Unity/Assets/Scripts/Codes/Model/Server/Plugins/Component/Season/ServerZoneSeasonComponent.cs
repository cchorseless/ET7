using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    public class ServerZoneSeasonComponent : Entity, IAwake
    {
        public int CurSeasonConfigId;
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> Seasons = new Dictionary<int, long>();

        [BsonIgnore]
        public TServerZoneSeason CurSeason
        {
            get
            {
                if (Seasons.TryGetValue(CurSeasonConfigId, out var CurSeasonId))
                {
                    return GetChild<TServerZoneSeason>(CurSeasonId);
                }
                return null;
            }
        }

        [BsonIgnore]
        public TServerZone ServerZone { get => GetParent<TServerZone>(); }
    }
}
