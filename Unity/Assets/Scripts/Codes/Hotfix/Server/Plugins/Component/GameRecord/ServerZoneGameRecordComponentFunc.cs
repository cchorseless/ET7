using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class ServerZoneGameRecordComponentFunc
    {
        public static void LoadAllChild(this ServerZoneGameRecordComponent self)
        {

        }


        public static (int, string) CreateGameRecord(this ServerZoneGameRecordComponent self, TCharacter character, List<string> _allplayers)
        {
            var gameRecord = character.GameRecordComp;
            if (gameRecord == null) { return (ErrorCode.ERR_Error, "miss GameRecordComp"); }
            if (gameRecord.CurRecordID != 0 && self.GetChild<TGameRecordItem>(gameRecord.CurRecordID) != null)
            {
                return (ErrorCode.ERR_Error, "repeat create game record ");
            }
            List<long> allPlayers = new List<long>();
            Scene scene = character.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            foreach (var _playerId in _allplayers)
            {
                if (long.TryParse(_playerId, out long playerId))
                {
                    Player player = playerComponent.Get(playerId);
                    if (player != null && player.GetMyCharacter() != null)
                    {
                        allPlayers.Add(playerId);
                    }
                    else
                    {
                        return (ErrorCode.ERR_Error, "playerId not loginGate");
                    }
                }
                else
                {
                    return (ErrorCode.ERR_Error, "playerId error");
                }
            }
            if (!allPlayers.Contains(character.Int64PlayerId))
            {
                allPlayers.Add(character.Int64PlayerId);
            }
            var entity = self.AddChild<TGameRecordItem>();
            entity.Players.AddRange(allPlayers);
            character.SyncHttpEntity(entity);
            // 对局数统计
            self.ServerZone.DataStatisticComp.GetCurDataItem().UpdateHoursBattleCount();
            allPlayers.ForEach(playerId =>
            {
                playerComponent.Get(playerId).GetMyCharacter().GameRecordComp.AddGameRecord(entity.Id);
            });
            return (ErrorCode.ERR_Success, "");
        }


        public static async ETTask SaveAndDestroyGameRecord(this ServerZoneGameRecordComponent self, long entityId)
        {
            self.Records.Remove(entityId);
            var entity = self.GetChild<TGameRecordItem>(entityId);
            if (entity != null)
            {
                var db = DBManagerComponent.Instance.GetZoneDB(self.ServerZone.ZoneID);
                await db.Save(entity);
                entity.Dispose();
            }
            await ETTask.CompletedTask;
        }
    }
}
