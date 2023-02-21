﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [StatefulTimer]
    public class TServerZoneTimer : AStatefulTimer<TServerZone>
    {
        public override async ETTask Run(TServerZone self)
        {
            if (self.State.Contains((int)EServerZoneState.WaitForWorking))
            {
                self.State.Remove((int)EServerZoneState.WaitForWorking);
                if (self.State.Contains((int)EServerZoneState.Closing))
                {
                    self.State.Remove((int)EServerZoneState.Closing);
                }
                self.State.Add((int)EServerZoneState.Working);
            }
            else if (self.State.Contains((int)EServerZoneState.WaitForClosing))
            {
                self.State.Remove((int)EServerZoneState.WaitForClosing);
                if (self.State.Contains((int)EServerZoneState.Working))
                {
                    self.State.Remove((int)EServerZoneState.Working);
                }
                self.State.Add((int)EServerZoneState.Closing);
            }
            await self.Save();
        }
    }

    public static class TServerZoneFunc
    {
        public static async ETTask<T> LoadOrAddComponent<T>(this TServerZone self) where T : Entity, IAwake, new()
        {
            DBComponent db = DBManagerComponent.Instance.GetAccountDB();
            T comp = await db.Query<T>(self.Id);
            if (comp == null)
            {
                comp = self.AddComponent<T>();
                await db.Save(comp);
            }
            else
            {
                self.AddComponent(comp);
            }
            return comp;
        }
        public static async ETTask LoadAllComponent(this TServerZone self)
        {
            self.AddComponent<ServerZoneCharacterComponent>();
            self.AddComponent<TimerEntityComponent>();
            self.AddComponent<ServerZoneTimePlanComponent>();
            await self.LoadOrAddComponent<ServerZoneSeasonComponent>();
            self.SeasonComp.LoadAllChild();
            await self.LoadOrAddComponent<ServerZoneActivityComponent>();
            self.ActivityComp.LoadAllChild();
            await self.LoadOrAddComponent<ServerZoneShopComponent>();
            self.ShopComp.LoadAllChild();
            await self.LoadOrAddComponent<ServerZoneMailComponent>();
            self.MailComp.LoadAllChild();
            await self.LoadOrAddComponent<ServerZoneTaskComponent>();
            self.TaskComp.LoadAllChild();
            await self.LoadOrAddComponent<ServerZoneRankComponent>();
            self.RankComp.LoadAllChild();
            await self.LoadOrAddComponent<ServerZoneDrawTreasureComponent>();
            self.DrawTreasureComp.LoadAllChild();
            await self.LoadOrAddComponent<ServerZoneRechargeComponent>();
            self.RechargeComp.LoadAllChild();
            await self.LoadOrAddComponent<ServerZoneTItemManageComponent>();
            self.TItemManageComp.LoadAllChild();
            await self.LoadOrAddComponent<ServerZoneBuffComponent>();
            self.BuffComp.LoadAllChild();
            await self.LoadOrAddComponent<ServerZoneDataReportComponent>();
            self.DataReportComp.LoadAllChild();
            await self.LoadOrAddComponent<ServerZoneGameRecordComponent>();
            self.GameRecordComp.LoadAllChild();
            await self.Save();
            
        }


        public static async ETTask Save(this TServerZone self)
        {
            var accountDB = DBManagerComponent.Instance.GetAccountDB();
            await accountDB.Save(self.SeasonComp);
            await accountDB.Save(self.ActivityComp);
            await accountDB.Save(self.ShopComp);
            await accountDB.Save(self.MailComp);
            await accountDB.Save(self.TaskComp);
            await accountDB.Save(self.RankComp);
            await accountDB.Save(self.DrawTreasureComp);
            await accountDB.Save(self.RechargeComp);
            await accountDB.Save(self.TItemManageComp);
            await accountDB.Save(self.BuffComp);
            await accountDB.Save(self.DataReportComp);
            await accountDB.Save(self.GameRecordComp);
            await accountDB.Save(self);
        }


    }


    [ObjectSystem]
    public class TServerZoneAwakeSystem : AwakeSystem<TServerZone, int, int, string>
    {
        protected override void Awake(TServerZone self, int zoneid, int severid, string serverName)
        {
            self.ZoneID = zoneid;
            self.ServerID = severid;
            self.ServerName = serverName;
            self.ServerLabel = new HashSet<int>();
            self.State = new HashSet<int>();
            self.AddComponent<GhostEntityComponent, int>(severid);
        }
    }
}
