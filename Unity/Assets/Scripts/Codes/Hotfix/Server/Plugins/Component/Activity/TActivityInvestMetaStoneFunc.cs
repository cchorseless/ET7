using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TActivityInvestMetaStoneFunc
    {
        public static void LoadAllChild(this TActivityInvestMetaStone self)
        {
            self.Items.Clear();
            var curTime = TimeHelper.ServerNow() / 1000;
            var data = LuBanConfigComponent.Instance.Config().TActivityInvestMetaStone.DataList.Find(
                record =>
                {
                    return record.ActivityStartTime <= curTime && curTime <= (record.ActivityEndTime);
                });

            if (data != null)
            {
                self.ConfigId = data.Id;
                self.StartTime = data.ActivityStartTime;
                self.EndTime = data.ActivityEndTime;
                data.InvestItemGroup.ForEach(item =>
                {
                    if (!self.Items.ContainsKey(item.Need))
                    {
                        self.Items.Add(item.Need, new FItemInfo(item.GainMin, item.GainMax));
                    }
                });
            }
        }


        public static (int, string) GetPrize(this TActivityInvestMetaStone self, TCharacter character, int metaStoneCount)
        {
            if (!self.IsValid())
            {
                return (ErrorCode.ERR_Error, "active not valid");
            }
            if (!self.Items.TryGetValue(metaStoneCount, out var info))
            {
                return (ErrorCode.ERR_Error, "dayIndex not valid");
            }
            var activityData = character.ActivityComp.GetActivityData<TActivityInvestMetaStoneData>(EActivityType.TActivityInvestMetaStone);
            if (activityData == null || !activityData.IsValid())
            {
                return (ErrorCode.ERR_Error, "activityData not valid");
            }
            if (activityData.ItemState.ContainsKey(metaStoneCount))
            {
                return (ErrorCode.ERR_Error, "activityData had Get");
            }
            var keys = self.Items.Keys.ToList();
            keys.Sort();
            var index = keys.IndexOf(metaStoneCount);
            if (index > 0)
            {
                var lastMeta = keys[index - 1];
                if (!activityData.ItemState.ContainsKey(lastMeta))
                {
                    return (ErrorCode.ERR_Error, "last activityData not Get");
                }
            }
            int metaGet = RandomGenerator.RandomNumber(info.ItemConfigId, info.ItemCount);
            activityData.ItemState.Add(metaStoneCount, metaGet);
            character.BagComp.AddTItemOrMoney((int)EMoneyType.MetaStone, metaGet);
            return (ErrorCode.ERR_Success, "" + metaGet);
        }
    }
}
