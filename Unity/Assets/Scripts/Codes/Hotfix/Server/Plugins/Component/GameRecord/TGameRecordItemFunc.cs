using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class EGameRecordConfig
    {
        public const int CharacterMaxGameRecordCount = 50;
    }

    public static class EGameRecordKey
    {
        public static class GameTime
        {
            public const string GAMERULES_STATE_INIT = "GAMERULES_STATE_INIT";
            public const string GAMERULES_STATE_WAIT_FOR_PLAYERS_TO_LOAD = "GAMERULES_STATE_WAIT_FOR_PLAYERS_TO_LOAD";
            public const string GAMERULES_STATE_CUSTOM_GAME_SETUP = "GAMERULES_STATE_CUSTOM_GAME_SETUP";
            public const string GAMERULES_STATE_HERO_SELECTION = "GAMERULES_STATE_HERO_SELECTION";
            public const string GAMERULES_STATE_STRATEGY_TIME = "GAMERULES_STATE_STRATEGY_TIME";
            public const string GAMERULES_STATE_TEAM_SHOWCASE = "GAMERULES_STATE_TEAM_SHOWCASE";
            public const string GAMERULES_STATE_WAIT_FOR_MAP_TO_LOAD = "GAMERULES_STATE_WAIT_FOR_MAP_TO_LOAD";
            public const string GAMERULES_STATE_PRE_GAME = "GAMERULES_STATE_PRE_GAME";
            public const string GAMERULES_STATE_SCENARIO_SETUP = "GAMERULES_STATE_SCENARIO_SETUP";
            public const string GAMERULES_STATE_GAME_IN_PROGRESS = "GAMERULES_STATE_GAME_IN_PROGRESS";
            public const string GAMERULES_STATE_POST_GAME = "GAMERULES_STATE_POST_GAME";
            public const string GAMERULES_STATE_DISCONNECT = "GAMERULES_STATE_DISCONNECT";
        }

    }






    public static class TGameRecordItemFunc
    {
        public static void PlayerLogOut(this TGameRecordItem self, long playerId)
        {
            if (self.Players.Contains(playerId))
            {
                self.LogOutPlayers.Add(playerId);
            }
            if (self.Players.Count == self.LogOutPlayers.Count)
            {
                self.GameRecordComp.SaveAndDestroyGameRecord(self.Id).Coroutine();
            }
        }

        public static (int, string) UploadGameRecord(this TGameRecordItem self, Dictionary<string, string> record)
        {
            foreach (var kv in record)
            {
                if (self.RecordInfo.ContainsKey(kv.Key))
                {
                    self.RecordInfo[kv.Key] = kv.Value;
                }
                else
                {
                    self.RecordInfo.Add(kv.Key, kv.Value);
                }
            }
            return (ErrorCode.ERR_Success, "");
        }

    }
}
