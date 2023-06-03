using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TActivityBattlePassFunc
    {
        public static void LoadAllChild(this TActivityBattlePass self)
        {
            self.Items.Clear();
            self.VipItems.Clear();
            var curTime = TimeHelper.ServerNow() / 1000;
            var battlePassData = LuBanConfigComponent.Instance.Config().TActivityBattlePass.DataList.Find(
                record =>
                {
                    return record.ActivityStartTime <= curTime && curTime <= (record.ActivityStartTime + record.ActivityEndTime);
                });
            if (battlePassData != null)
            {
                self.ConfigId = battlePassData.Id;
                self.StartTime = battlePassData.ActivityStartTime;
                self.EndTime = battlePassData.ActivityEndTime;
                battlePassData.ItemPrize.ForEach(item =>
                {
                    if (!self.Items.ContainsKey(item.Level))
                    {
                        self.Items.Add(item.Level, new FItemInfo(item.ItemConfigId, item.ItemCount));
                    }
                    if (!self.VipItems.ContainsKey(item.Level))
                    {
                        self.VipItems.Add(item.Level, new FItemInfo(item.VIPItemConfigId, item.VIPItemCount));
                    }
                });
            }
        }

        public static (int, string) GetPrize(this TActivityBattlePass self, TCharacter character, int level, bool isVip)
        {
            if (!self.IsValid())
            {
                return (ErrorCode.ERR_Error, "active not valid");
            }

            var activityData = character.ActivityComp.GetActivityData<TActivityBattlePassData>(EActivityType.TActivityBattlePass);
            if (activityData == null || !activityData.IsValid())
            {
                return (ErrorCode.ERR_Error, "activityData not valid");
            }
            if (activityData.Level < level)
            {
                return (ErrorCode.ERR_Error, "Level not valid");
            }
            if (isVip && (!activityData.IsVip))
            {
                return (ErrorCode.ERR_Error, "Vip not valid");
            }
            FItemInfo prizeItem;
            if (isVip)
            {
                if (!self.VipItems.TryGetValue(level, out prizeItem))
                {
                    return (ErrorCode.ERR_Error, "level not valid");
                }
                if (activityData.VipItemGetRecord.Contains(level))
                {
                    return (ErrorCode.ERR_Error, "activityData had Get");
                }
            }
            else
            {
                if (!self.Items.TryGetValue(level, out prizeItem))
                {
                    return (ErrorCode.ERR_Error, "level not valid");
                }
                if (activityData.ItemGetRecord.Contains(level))
                {
                    return (ErrorCode.ERR_Error, "activityData had Get");
                }
            }
            var addResult = character.BagComp.AddTItemOrMoney(prizeItem.ItemConfigId, prizeItem.ItemCount);
            if (addResult.Item1)
            {
                if (isVip)
                {
                    activityData.VipItemGetRecord.Add(level);
                }
                else
                {
                    activityData.ItemGetRecord.Add(level);
                }
                return (ErrorCode.ERR_Success, "");
            }
            return (ErrorCode.ERR_Error, "item  Get fail");
        }

        public static (int, string) OneKeyGetPrize(this TActivityBattlePass self, TCharacter character)
        {
            if (!self.IsValid())
            {
                return (ErrorCode.ERR_Error, "active not valid");
            }
            var activityData = character.ActivityComp.GetActivityData<TActivityBattlePassData>(EActivityType.TActivityBattlePass);
            if (activityData == null || !activityData.IsValid())
            {
                return (ErrorCode.ERR_Error, "activityData not valid");
            }
            var prizeList = new List<FItemInfo>();
            var ItemsIndex = new List<int>();
            var VipItemsIndex = new List<int>();
            for (int i = 0; i <= activityData.Level; i++)
            {
                if (self.Items.TryGetValue(i, out var prizeItem) && !activityData.ItemGetRecord.Contains(i))
                {
                    prizeList.Add(prizeItem);
                    ItemsIndex.Add(i);
                }
                if (activityData.IsVip)
                {
                    if (self.VipItems.TryGetValue(i, out var prizeItemVip) &&
                        !activityData.VipItemGetRecord.Contains(i))
                    {
                        prizeList.Add(prizeItemVip);
                        VipItemsIndex.Add(i);
                    }
                }
            }
            if (prizeList.Count == 0)
            {
                return (ErrorCode.ERR_Error, "activityData no prize");
            }
            var addResult = character.BagComp.AddTItemOrMoney(prizeList);
            if (addResult.Item1 == ErrorCode.ERR_Success)
            {
                activityData.ItemGetRecord.AddRange(ItemsIndex);
                activityData.VipItemGetRecord.AddRange(VipItemsIndex);
            }
            return addResult;
        }

        public static async ETTask<(int, string)> BuyBattlePass(this TActivityBattlePass self, TCharacter character, int payType)
        {
            await ETTask.CompletedTask;
            if (!self.IsValid())
            {
                return (ErrorCode.ERR_Error, "active not valid");
            }
            var activityData = character.ActivityComp.GetActivityData<TActivityBattlePassData>(EActivityType.TActivityBattlePass);
            if (activityData == null || !activityData.IsValid())
            {
                return (ErrorCode.ERR_Error, "activityData not valid");
            }
            if (activityData.IsVip)
            {
                return (ErrorCode.ERR_Error, "activityData had IsVip");
            }
            switch (payType)
            {
                case (int)EPayOrderSourceType.AliPay_QrCode:
                    var aliQrcode = await AliPayComponent.Instance.GetQrCodePay(character, "BattllePass", 30 * 100, EPayOrderLabel.PayForBattlePass);
                    return (ErrorCode.ERR_Success, aliQrcode);

                case (int)EPayOrderSourceType.WeChat_QrCodeV3:
                    var weChatQrcode = await WeChatPayComponent.Instance.GetQrCodePayV3(character, "BattllePass", 30 * 100, EPayOrderLabel.PayForBattlePass);
                    return (ErrorCode.ERR_Success, weChatQrcode);
                default:
                    return (ErrorCode.ERR_Error, "payType not valid");
            }
        }

        public static void OnBuyBattlePass(this TActivityBattlePass self, TCharacter character, TPayOrderItem payOrder)
        {
            var activityData = character.ActivityComp.GetActivityData<TActivityBattlePassData>(EActivityType.TActivityBattlePass);
            if (activityData == null || !activityData.IsValid())
            {
                return;
            }
            if (payOrder.IsPaySuccess())
            {
                activityData.IsVip = true;
            }
            character.SyncHttpEntity(payOrder);
        }

    }
}
