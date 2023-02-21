using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class CharacterTaskComponent : Entity, IAwake
    {
        public List<long> DailyTasks = new List<long>();
        public List<long> WeekTasks = new List<long>();
        public List<long> SeasonTasks = new List<long>();

        public bool IsReplaceDailyTask;


        [BsonIgnore]
        public TCharacter Character { get => this.GetParent<TCharacter>(); }
    }
}
