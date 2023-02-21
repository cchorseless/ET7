using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class ServerZoneCharacterComponent : Entity, IAwake
    {
        public readonly Dictionary<long, TCharacter> idCharacters = new Dictionary<long, TCharacter>();

        public int Count
        {
            get
            {
                return this.idCharacters.Count;
            }
        }
    }
    [FriendOf(typeof(ServerZoneCharacterComponent))]
    public static class ServerZoneCharacterComponentFunc
    {
        public static void Add(this ServerZoneCharacterComponent self, TCharacter character)
        {
            self.idCharacters.Add(character.Id, character);
        }

        public static TCharacter Get(this ServerZoneCharacterComponent self, long id)
        {
            self.idCharacters.TryGetValue(id, out TCharacter character);
            return character;
        }

        public static void Remove(this ServerZoneCharacterComponent self, long id)
        {
            self.idCharacters.Remove(id);
        }
        public static TCharacter[] GetAll(this ServerZoneCharacterComponent self)
        {
            return self.idCharacters.Values.ToArray();
        }
    }
}
