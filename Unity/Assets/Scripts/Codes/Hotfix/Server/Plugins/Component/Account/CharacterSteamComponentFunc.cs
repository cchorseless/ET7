using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class CharacterSteamComponentFunc
    {
        public static void LoadAllChild(this CharacterSteamComponent self)
        {

        }

        public static CharacterSteamDigestItem GetSteamDigest(this CharacterSteamComponent self)
        {
            var entity = Entity.CreateOne<CharacterSteamDigestItem>();
            entity.CharacterId = self.Id;
            return entity;
        }
    }
}
