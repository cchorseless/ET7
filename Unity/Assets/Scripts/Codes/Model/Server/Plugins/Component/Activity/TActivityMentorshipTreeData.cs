using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public class TActivityMentorshipTreeData : TActivityData
    {
        public long MasterId;

        public List<long> Apprentice = new List<long>();

        public List<long> ApplyMasterRecord = new List<long>();

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> MentorshipPrize = new Dictionary<int, long>();


    }
}
