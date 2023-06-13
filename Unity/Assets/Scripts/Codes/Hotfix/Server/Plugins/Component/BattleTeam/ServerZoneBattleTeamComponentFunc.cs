using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;

namespace ET.Server
{
    public static class ServerZoneBattleTeamComponentFunc
    {
        public static void LoadAllChild(this ServerZoneBattleTeamComponent self)
        {
            self.LoadDataFromDBAsync().Coroutine();
        }

        public static async ETTask LoadDataFromDBAsync(this ServerZoneBattleTeamComponent self)
        {
            DBComponent db = DBManagerComponent.Instance.GetAccountDB();
            // 最多加载10000条记录，todo 分页加载 随机加载
            var allTeam = await db.Query<TBattleTeamRecord>(x => true, 10000);
            allTeam.ForEach(record => { self.RegBattleTeamRecord(record); });
        }

        public static int GetBattleTeamCount(this ServerZoneBattleTeamComponent self)
        {
            int count = 0;
            foreach (var kv in self.RoundTBattleTeam.Values)
            {
                count += kv.Count;
            }
            return count;
        }
        
        public static void RegBattleTeamRecord(this ServerZoneBattleTeamComponent self, TBattleTeamRecord record)
        {
            int roundKey = record.GetRoundKey();
            if (!self.RoundTBattleTeam.ContainsKey(roundKey))
            {
                self.RoundTBattleTeam.Add(roundKey, new Dictionary<long, int>());
            }
            self.RoundTBattleTeam[roundKey][record.Id]= record.Score;
            if (record.Parent != self)
            {
                self.AddChild(record);
            }
        }

        public static List<TBattleTeamRecord> SearchBattleTeamRecord(this ServerZoneBattleTeamComponent self,
        int roundIndex, int score, int count)
        {
            List<TBattleTeamRecord> r = new List<TBattleTeamRecord>();
            if (roundIndex <= 0 || score <= 0 || count <= 0)
            {
                return r;
            }
            if (self.RoundTBattleTeam.TryGetValue(roundIndex, out var allinfo))
            {
                var listId = new List<long>();
                foreach (var kv in allinfo)
                {
                    if (Math.Abs(kv.Value - score) < 1000)
                    {
                        listId.Add(kv.Key);
                    }

                    if (listId.Count >= count * 10)
                    {
                        break;
                    }
                }

                if (listId.Count == 0)
                {
                    return r;
                }
                for (var i = 0; i < count * 10; i++)
                {
                    var entityid = RandomGenerator.RandomArray(listId);
                    var _child = self.GetChild<TBattleTeamRecord>(entityid);
                    if (_child != null)
                    {
                        r.Add(_child);
                    }

                    if (r.Count == count)
                    {
                        break;
                    }
                }
            }
            return r;
        }
    }
}