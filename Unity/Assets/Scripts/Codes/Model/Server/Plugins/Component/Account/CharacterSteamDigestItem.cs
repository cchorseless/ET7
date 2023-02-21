using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class CharacterSteamDigestItem : Entity, IAwake, ISerializeToEntity
    {
        public long CharacterId;
        public string SteamName;
        public string SteamIconUrl;
    }
}
