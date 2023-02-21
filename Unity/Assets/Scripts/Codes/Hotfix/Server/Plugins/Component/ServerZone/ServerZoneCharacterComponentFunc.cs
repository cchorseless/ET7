using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class ServerZoneCharacterComponentFunc
    {
        public static async ETTask<(int, string)> SearchCharacterSteam(this ServerZoneCharacterComponent self, long masterId)
        {
            DBComponent db = DBManagerComponent.Instance.GetAccountDB();
            var character = self.Get(masterId);
            if (character != null)
            {
                return (ErrorCode.ERR_Success, MongoHelper.ToClientJson(character.SteamComp));
            }
            else
            {
                character = await db.Query<TCharacter>(masterId);
                if (character == null)
                {
                    return (ErrorCode.ERR_Error, "character not valid");
                }
                else
                {
                    var steam = await db.Query<CharacterSteamComponent>(masterId);
                    if (steam == null)
                    {
                        return (ErrorCode.ERR_Error, "CharacterSteamComponent not valid");
                    }
                    else
                    {
                        return (ErrorCode.ERR_Success, MongoHelper.ToClientJson(steam));
                    }
                }
            }
        }

        public static async ETTask<(int, string)> SearchCharacterSteam(this ServerZoneCharacterComponent self, string masterX64str)
        {
            await ETTask.CompletedTask;
            long masterId = MathHelper.X64ToInt(masterX64str);
            if (masterId == 0)
            {
                return (ErrorCode.ERR_Error, "masterX64str not valid");
            }
            return await self.SearchCharacterSteam(masterId);
        }

    }
}
