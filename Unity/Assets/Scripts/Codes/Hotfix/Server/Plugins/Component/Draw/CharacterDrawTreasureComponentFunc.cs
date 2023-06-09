using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public enum EDrawTreasureType
    {
        HeroExpTreasure = 10013,
        MingWenTreasure= 10014,
        CourierRareTreasure= 10015,
        HeroDressRareTreasure= 10016,
    }
    public static class CharacterDrawTreasureComponentFunc
    {
        public static void LoadAllChild(this CharacterDrawTreasureComponent self)
        {

        }
        public static (int, string) DrawTreasure(this CharacterDrawTreasureComponent self,  int treasureId, int times)
        {
            if (!Enum.IsDefined(typeof(EDrawTreasureType), treasureId))
            {
                return (ErrorCode.ERR_Error, "treasureId not valid");
            }
            if (times <= 0)
            {
                return (ErrorCode.ERR_Error, "times not valid");
            }
            var config = LuBanConfigComponent.Instance.Config().DrawTreasureConfig.GetOrDefault(treasureId);
            if (config == null)
            {
                return (ErrorCode.ERR_Error, "treasureId not valid");
            }
            if (!self.IsDrawItemEnough(treasureId, times))
            {
                return (ErrorCode.ERR_Error, "item not enough");
            }
            var r = new List<FItemInfo>();
            for (var i = 0; i < times; i++)
            {
                var items = self.DrawTreasureOnce(treasureId);
                if (items != null)
                {
                    r.AddRange(items);
                }
            }
            return (ErrorCode.ERR_Success, JsonHelper.ToLitJson(r));
        }
        public static bool IsFreeDraw(this CharacterDrawTreasureComponent self, int treasureId)
        {
            var config = LuBanConfigComponent.Instance.Config().DrawTreasureConfig.GetOrDefault(treasureId);
            if (config == null)
            {
                return false;
            }
            if (config.FreeInterval > 0)
            {
                if (self.FreeTimeStamp.TryGetValue(treasureId, out var lastFree))
                {
                    return (lastFree + config.FreeInterval < TimeHelper.ServerNow());
                }
                else
                {
                    self.FreeTimeStamp.Add(treasureId, 0);
                    return true;
                }

            }
            return false;
        }

        public static bool IsDrawItemEnough(this CharacterDrawTreasureComponent self, int treasureId, int times)
        {
            bool isFree = self.IsFreeDraw(treasureId);
            var count = self.Character.BagComp.GetTItemCount(treasureId);
            if (isFree)
            {
                count += 1;
            }
            return count >= times;
        }

        public static List<FItemInfo> DrawTreasureOnce(this CharacterDrawTreasureComponent self, int treasureId)
        {
            if (!Enum.IsDefined(typeof(EDrawTreasureType), treasureId))
            {
                return null;
            }
            var config = LuBanConfigComponent.Instance.Config().DrawTreasureConfig.GetOrDefault(treasureId);
            if (config == null)
            {
                return null;
            }
            bool isFree = self.IsFreeDraw(treasureId);
            if (!isFree)
            {
                var count = self.Character.BagComp.GetTItemCount(treasureId);
                if (count < 1)
                {
                    return null;
                }
            }
            int poolGroupConfigId = config.ComItemPoolGroup;
            if (config.SpePrizeTimes > 0)
            {
                if (self.TreasureTimes.TryGetValue(treasureId, out int hadTimes))
                {
                    if (hadTimes % config.SpePrizeTimes == 0)
                    {
                        poolGroupConfigId = config.SpeItemPoolGroup;
                    }
                }
                else
                {
                    self.TreasureTimes.Add(treasureId, 0);
                }
            }
            var poolGroupConfig = LuBanConfigComponent.Instance.Config().ItemPrizePoolGroupConfig.GetOrDefault(poolGroupConfigId);
            var r = new List<FItemInfo>();
            poolGroupConfig.GetRandomItemId().ForEach(item =>
            {
                r.Add(new FItemInfo(item.ItemConfigId,item.ItemCount) );
            });
            self.TreasureTimes[treasureId] += 1;
            if (isFree)
            {
                self.FreeTimeStamp[treasureId] = TimeHelper.ServerNow();
            }
            else
            {
                self.Character.BagComp.RemoveTItem<TItem>(treasureId, 1);
            }
            return r;
        }
    }
}
