using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Server
{
    public static class TActivityMentorshipTreeDataFunc
    {
        public static void LoadAllChild(this TActivityMentorshipTreeData self)
        {
            self.UpdateApplyForMaster();
            self.UpdateMasterPrize();
        }

        public static void UpdateApplyForMaster(this TActivityMentorshipTreeData self)
        {
            var curTime = TimeHelper.ServerNow();
            var outDateList = new List<long>();
            foreach (var applyId in self.ApplyMasterRecord)
            {
                var entity = self.GetChild<TActivityMentorshipApplyForItem>(applyId);
                if (entity.OutOfDateTime <= curTime)
                {
                    entity.Dispose();
                    outDateList.Add(applyId);
                }
            }
            outDateList.ForEach(entityId =>
            {
                self.ApplyMasterRecord.Remove(entityId);
            });
        }

        public static void UpdateMasterPrize(this TActivityMentorshipTreeData self)
        {
            var config = LuBanConfigComponent.Instance.Config().TActivityMentorshipTree;
            foreach (var configId in self.MentorshipPrize.Keys)
            {
                if (config.GetOrDefault(configId) == null)
                {
                    self.GetChild<TActivityMentorshipPrizeItem>(self.MentorshipPrize[configId])?.Dispose();
                    self.MentorshipPrize.Remove(configId);
                }
            }
            foreach (var configInfo in config.DataList)
            {
                if (!self.MentorshipPrize.ContainsKey(configInfo.Id))
                {
                    var entity = self.AddChild<TActivityMentorshipPrizeItem, int>(configInfo.Id);
                    self.MentorshipPrize.Add(configInfo.Id, entity.Id);
                }
            }
        }

        // todo
        public static void OnBuildRelation(this TActivityMentorshipTreeData self)
        {
            foreach (var applyId in self.ApplyMasterRecord)
            {
                var entity = self.GetChild<TActivityMentorshipPrizeItem>(applyId);
                var config = entity.MentorshipPrizeConfig();
                if (config.PrizeConditionType == EMentorshipPrizeType.BuildRelation)
                {
                    List<ValueTupleStruct<int, int>> Items = new List<ValueTupleStruct<int, int>>();
                    config.ItemGroup.ForEach(item =>
                    {
                        Items.Add(new ValueTupleStruct<int, int>(item.ItemConfigId, item.ItemCount));
                    });
                    var character = self.CharacterActivity.Character;
                    var mailComp = character.GetMyServerZone().MailComp;
                    switch (config.TreeLevel)
                    {
                        case 0:
                            mailComp.AddCharacterPrizeMail(character.Id, "", "", 0, Items);
                            break;
                        case 1:
                            mailComp.AddCharacterPrizeMail(self.MasterId, "", "", 0, Items);
                            break;
                        case 2:
                            var activity = character.GetMyServerZone().ActivityComp.GetActivity<TActivityMentorshipTree>(EActivityType.TActivityMentorshipTree);
                            if (activity.MentorshipTree.TryGetValue(self.MasterId, out var master_masterId))
                            {
                                mailComp.AddCharacterPrizeMail(master_masterId, "", "", 0, Items);
                            }
                            break;
                    }
                }
            }
        }

    }
}
