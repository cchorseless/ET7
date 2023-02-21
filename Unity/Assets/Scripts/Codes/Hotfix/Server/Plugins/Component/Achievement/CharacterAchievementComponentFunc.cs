using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class CharacterAchievementComponentFunc
    {
        public static void LoadAllChild(this CharacterAchievementComponent self)
        {
            var sheet = LuBanConfigComponent.Instance.Config().AchievementConfig;
            foreach (var configId in self.Achievements.Keys)
            {
                if (sheet.GetOrDefault(configId) == null)
                {
                    self.GetChild<TCharacterAchievementItem>(self.Achievements[configId])?.Dispose();
                    self.Achievements.Remove(configId);
                }
            }
            foreach (var item in sheet.DataList)
            {
                if (self.Achievements.TryGetValue(item.Id, out var entityId))
                {
                    self.GetChild<TCharacterAchievementItem>(entityId)?.LoadAllChild();
                }
                else
                {
                    var achieve = self.AddChild<TCharacterAchievementItem, int>(item.Id);
                    self.Achievements.Add(item.Id, achieve.Id);
                    achieve.LoadAllChild();
                }
            }
        }


        public static (int, string) GetPrize(this CharacterAchievementComponent self, int achievementConfigId)
        {
            if (!self.Achievements.TryGetValue(achievementConfigId, out var entityId))
            {
                return (ErrorCode.ERR_Error, "achievementConfigId not valid");
            }
            var entity = self.GetChild<TCharacterAchievementItem>(entityId);
            if (entity == null)
            {
                return (ErrorCode.ERR_Error, "achievement Entity not valid");
            }
            entity.UpdateAchieveState();
            if (!entity.IsAchieve)
            {
                return (ErrorCode.ERR_Error, "achievement  not achieve");
            }
            if (!entity.IsPrizeGet)
            {
                return (ErrorCode.ERR_Error, "achievement  had PrizeGet");
            }
            List<ValueTupleStruct<int, int>> itemsPrize = new List<ValueTupleStruct<int, int>>();
            entity.AchieveConfig().AchievePrize.ForEach(item =>
            {
                itemsPrize.Add(new ValueTupleStruct<int, int>(item.ItemConfigId, item.ItemCount));
            });
            var r = self.Character.BagComp.AddTItemOrMoney(itemsPrize);
            if (r.Item1 == ErrorCode.ERR_Success)
            {
                entity.IsPrizeGet = true;
            }
            return r;
        }

    }
}
