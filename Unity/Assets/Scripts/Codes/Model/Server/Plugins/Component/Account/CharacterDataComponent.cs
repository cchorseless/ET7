using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class CharacterDataComponent : Entity, IAwake
    {
        
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<string, string> GameRecord = new Dictionary<string, string>();

        
        public List<string> AllCouriers = new List<string>();
        
        [BsonIgnore]
        public NumericComponent NumericComp { get => this.GetComponent<NumericComponent>(); }


        [BsonIgnore]
        public TCharacter Character { get => this.GetParent<TCharacter>(); }
    }
}
