using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class TActivityHeroRecordLevelFunc
    {
        public static void LoadAllChild(this TActivityHeroRecordLevel self)
        {
            self.Items.Clear();
            var curTime = TimeHelper.ServerNow() / 1000;
            var monthData = LuBanConfigComponent.Instance.Config().TActivityHeroRecordLevel.DataList;
            if (monthData.Count > 0)
            {
                self.SetNeverOutOfDate();
                monthData.ForEach(item =>
                {
                    if (!self.Items.ContainsKey(item.Id))
                    {
                        var itemList = new List<FItemInfo>();
                        item.ItemGroup.ForEach(_info =>
                        {
                            itemList.Add(new FItemInfo (_info.ItemConfigId, _info.ItemCount));
                        });
                        self.Items.Add(item.Id, itemList);
                    }
                });
            }
        }


        public static (int, string) GetPrize(this TActivityHeroRecordLevel self, TCharacter character, int level)
        {
            if (!self.IsValid())
            {
                return (ErrorCode.ERR_Error, "active not valid");
            }
            var activityData = character.ActivityComp.GetActivityData<TActivityHeroRecordLevelData>(EActivityType.TActivityHeroRecordLevel);
            if (activityData == null || !activityData.IsValid())
            {
                return (ErrorCode.ERR_Error, "activityData not valid");
            }
            if (activityData.HeroSumLevel < level)
            {
                return (ErrorCode.ERR_Error, "Level not valid");
            }

            if (!self.Items.TryGetValue(level, out var prizeItem))
            {
                return (ErrorCode.ERR_Error, "level not valid");
            }
            if (activityData.ItemGetRecord.Contains(level))
            {
                return (ErrorCode.ERR_Error, "activityData had Get");
            }

            var addResult = character.BagComp.AddTItemOrMoney(prizeItem);
            if (addResult.Item1 == ErrorCode.ERR_Success)
            {
                activityData.ItemGetRecord.Add(level);
            }
            return addResult;
        }
        public static (int, string) OneKeyGetPrize(this TActivityHeroRecordLevel self, TCharacter character)
        {
            if (!self.IsValid())
            {
                return (ErrorCode.ERR_Error, "active not valid");
            }
            var activityData = character.ActivityComp.GetActivityData<TActivityHeroRecordLevelData>(EActivityType.TActivityHeroRecordLevel);
            if (activityData == null || !activityData.IsValid())
            {
                return (ErrorCode.ERR_Error, "activityData not valid");
            }
            var prizeList = new List<FItemInfo>();
            var ItemsIndex = new List<int>();
            for (int i = 0; i <= activityData.HeroSumLevel; i++)
            {
                if (self.Items.TryGetValue(i, out var prizeItem) && !activityData.ItemGetRecord.Contains(i))
                {
                    prizeList.AddRange(prizeItem);
                    ItemsIndex.Add(i);
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
            }
            return addResult;
        }

    }
}
