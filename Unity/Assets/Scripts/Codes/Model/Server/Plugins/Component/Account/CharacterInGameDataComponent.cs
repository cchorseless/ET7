using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET
{
    public class CharacterInGameDataComponent : Entity, IAwake, ISerializeToEntity
    {
        public int NumericType;
        public int NumericValue;
    }
}
