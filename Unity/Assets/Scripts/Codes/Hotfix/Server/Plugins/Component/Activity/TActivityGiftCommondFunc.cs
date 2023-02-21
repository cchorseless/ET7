using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TActivityGiftCommondFunc
    {
        public static void LoadAllChild(this TActivityGiftCommond self)
        {
            var curTime = TimeHelper.ServerNow() / 1000;
            self.SetNeverOutOfDate();
            var config = LuBanConfigComponent.Instance.Config().TActivityGiftCommond;
            foreach (var giftId in self.Gifts.Keys)
            {
                var gift = self.GetChild<TActivityGiftCommondItem>(self.Gifts[giftId]);
                var info = config.GetOrDefault(giftId);
                if (info == null || !info.IsValid || info.ActivityStartTime > curTime || info.ActivityEndTime < curTime)
                {
                    gift.Dispose();
                    self.Gifts.Remove(giftId);
                }
                else
                {
                    gift.LoadAllChild();
                }
            }
            config.DataList.ForEach(info =>
            {
                if (info.IsValid  && info.ActivityStartTime <= curTime && info.ActivityEndTime >= curTime)
                {
                    if (!self.Gifts.ContainsKey(info.Id))
                    {
                        var gift = self.AddChild<TActivityGiftCommondItem, int>(info.Id);
                        self.Gifts.Add(info.Id, gift.Id);
                        gift.LoadAllChild();
                    }
                }
            });
        }

        public static (int, string) GetPrize(this TActivityGiftCommond self, TCharacter character, int giftConfigId)
        {
            if (!self.IsValid())
            {
                return (ErrorCode.ERR_Error, "active not valid");
            }
            if (!self.Gifts.TryGetValue(giftConfigId, out var giftItemId))
            {
                return (ErrorCode.ERR_Error, "giftConfigId not valid");
            }
            var gift = self.GetChild<TActivityGiftCommondItem>(giftItemId);
            if (gift == null)
            {
                return (ErrorCode.ERR_Error, "gift entity not valid");
            }
            if (gift.GiftCost >= gift.GiftMax)
            {
                return (ErrorCode.ERR_Error, "gift bag had useless");
            }
            var config = gift.ActivityGiftConfig();
            var curTime = TimeHelper.ServerNow() / 1000;
            if (config == null || !config.IsValid || config.ActivityStartTime > curTime || config.ActivityEndTime < curTime)
            {
                return (ErrorCode.ERR_Error, "gift entity out of date");
            }
            var activityData = character.ActivityComp.GetActivityData<TActivityGiftCommondData>(EActivityType.TActivityGiftCommond);
            if (activityData == null)
            {
                return (ErrorCode.ERR_Error, "activityData not valid");
            }
            if (activityData.GiftCommonds.ContainsKey(giftConfigId))
            {
                return (ErrorCode.ERR_Error, "giftConfigId had used");
            }
            List<ValueTupleStruct<int, int>> itemsPrize = new List<ValueTupleStruct<int, int>>();
            config.ItemGroup.ForEach(item =>
            {
                itemsPrize.Add(new ValueTupleStruct<int, int>(item.ItemConfigId, item.ItemCount));
            });
            var addResult = character.BagComp.AddTItemOrMoney(itemsPrize);
            if (addResult.Item1 == ErrorCode.ERR_Success)
            {
                activityData.GiftCommonds.Add(giftConfigId, curTime);
                gift.GiftCost++;
            }
            return addResult;
        }

    }
}
