﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class TServerZoneDailyDataStatisticItemAwakeSystem: AwakeSystem<TServerZoneDailyDataStatisticItem>
    {
        protected override void Awake(TServerZoneDailyDataStatisticItem self)
        {
            var now = TimeHelper.DateTimeNow();
            self.Time = TimeHelper.ServerNow();
            self.Year = now.Year;
            self.Month = now.Month;
            self.Day = now.Day;
        }
    }

    public static class TServerZoneDailyDataStatisticItemFunc
    {
        public static bool IsTodayData(this TServerZoneDailyDataStatisticItem self)
        {
            var now = TimeHelper.DateTimeNow();
            return self.Day == now.Day && self.Month == now.Month && self.Year == now.Year;
        }

        public static async ETTask SaveAndExit(this TServerZoneDailyDataStatisticItem self, bool bexit = true)
        {
            DBComponent db = DBManagerComponent.Instance.GetZoneDB(GameConfig.AccountDBZone);
            await db.Save(self);
            if (bexit)
            {
                self.Dispose();
            }
        }

        public static int GetTodayPlayerNew(this TServerZoneDailyDataStatisticItem self)
        {
            int r = 0;
            foreach (int count in self.HoursPlayerNew)
            {
                r += count;
            }

            return r;
        }

        public static int GetTodayPlayerOnline(this TServerZoneDailyDataStatisticItem self)
        {
            int r = 0;
            foreach (int count in self.HoursPlayerOnline)
            {
                if (r < count)
                {
                    r = count;
                }
            }

            return r;
        }

        public static void MergeData(this TServerZoneDailyDataStatisticItem self, TServerZoneDailyDataStatisticItem other)
        {
            var count = self.HoursPlayerOnline.Length;
            for (var i = 0; i < count; i++)
            {
                self.HoursPlayerNew[i] += other.HoursPlayerNew[i];
                self.HoursPlayerOnline[i] += other.HoursPlayerOnline[i];
                self.HoursBattleCount[i] += other.HoursBattleCount[i];
            }

            foreach (var kv in other.OrderIncome)
            {
                self.UpdateOrderIncome(kv.Key, kv.Value);
            }

            foreach (var kv in other.ShopSellItem)
            {
                self.UpdateShopSellItem(kv.Key, kv.Value);
            }
        }

        public static void UpdateHoursPlayerNew(this TServerZoneDailyDataStatisticItem self)
        {
            int hour = TimeHelper.DateTimeNow().Hour;
            var d = self.HoursPlayerNew[hour];
            self.HoursPlayerNew[hour] = d + 1;
        }

        public static void UpdateHoursPlayerOnline(this TServerZoneDailyDataStatisticItem self)
        {
            int hour = TimeHelper.DateTimeNow().Hour;
            var playerComp = self.DomainScene().GetComponent<PlayerComponent>();
            if (playerComp != null)
            {
                var count = playerComp.idPlayers.Count;
                if (self.HoursPlayerOnline[hour] < count)
                {
                    self.HoursPlayerOnline[hour] = count;
                }
            }
        }

        public static async ETTask UpdateTotalPlayerCount(this TServerZoneDailyDataStatisticItem self)
        {
            var db = DBManagerComponent.Instance.GetAccountDB();
            self.TotalPlayerCount = (int)await db.QueryCount<TCharacter>(v => true);
        }

        public static void UpdateHoursBattleCount(this TServerZoneDailyDataStatisticItem self)
        {
            int hour = TimeHelper.DateTimeNow().Hour;
            var d = self.HoursBattleCount[hour];
            self.HoursBattleCount[hour] = d + 1;
        }

        public static void UpdateOrderIncome(this TServerZoneDailyDataStatisticItem self, int paytype, int count)
        {
            if (self.OrderIncome.ContainsKey(paytype))
            {
                self.OrderIncome[paytype] += count;
            }
            else
            {
                self.OrderIncome[paytype] = count;
            }
        }

        public static void UpdateShopSellItem(this TServerZoneDailyDataStatisticItem self, int itemconfigId, int itemcount)
        {
            if (self.ShopSellItem.ContainsKey(itemconfigId))
            {
                self.ShopSellItem[itemconfigId] += itemcount;
            }
            else
            {
                self.ShopSellItem[itemconfigId] = itemcount;
            }
        }
    }
}