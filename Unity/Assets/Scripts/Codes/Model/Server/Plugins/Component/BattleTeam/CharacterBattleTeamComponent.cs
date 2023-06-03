using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    public class CharacterBattleTeamComponent: Entity, IAwake
    {
        // 每回合存1场战斗的战斗记录
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> BattleTeams = new Dictionary<int, long>();

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, int> SeasonBattleScore = new Dictionary<int, int>();
        
        // 当前赛季天梯分数
        public int BattleScore = 1000;
        
        [BsonIgnore]
        public Dictionary<int, List<long>> SearchBattleTeams = new Dictionary<int, List<long>>();

        [BsonIgnore]
        public TCharacter Character
        {
            get => this.GetParent<TCharacter>();
        }
    }
}