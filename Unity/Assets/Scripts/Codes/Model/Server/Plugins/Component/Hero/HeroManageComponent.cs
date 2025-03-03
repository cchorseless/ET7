﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class HeroManageComponent : Entity, IAwake
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<string, long> HeroUnits = new Dictionary<string, long>();

        public List<long> HeroBanDesign = new List<long>();
        public List<int> HeroLevelPrizeGet = new List<int>();
        public int SumHeroLevel;
            
        [BsonIgnore]
        public TCharacter Character { get => this.GetParent<TCharacter>(); }
    }
}
