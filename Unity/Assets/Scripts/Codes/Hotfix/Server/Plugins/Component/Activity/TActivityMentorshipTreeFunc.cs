using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class EMentorshipPrizeType
    {
        public const string BuildRelation = "BuildRelation";
        public const string Recharge = "Recharge";
        public const string Online = "Online";
    }
    public static class TActivityMentorshipTreeFunc
    {
        public static void LoadAllChild(this TActivityMentorshipTree self)
        {
            self.SetNeverOutOfDate();
        }

        public static async ETTask<(int, string)> ApplyForMaster(this TActivityMentorshipTree self,
            TCharacter character, string masterX64str, string applyMessage)
        {
            await ETTask.CompletedTask;
            if (!self.IsValid())
            {
                return (ErrorCode.ERR_Error, "active not valid");
            }
            long masterId = MathHelper.X64ToInt(masterX64str);
            if (masterId == 0)
            {
                return (ErrorCode.ERR_Error, "masterX64str not valid");
            }
            var selfData = character.ActivityComp.GetActivityData<TActivityMentorshipTreeData>(EActivityType.TActivityMentorshipTree);
            if (selfData.MasterId != 0)
            {
                return (ErrorCode.ERR_Error, "character had master");
            }
            TCharacter master = self.ServerZoneActivity.ServerZone.CharacterComp.Get(masterId);
            if (master != null)
            {
                var activityData = master.ActivityComp.GetActivityData<TActivityMentorshipTreeData>(EActivityType.TActivityMentorshipTree);
                var record = activityData.AddChild<TActivityMentorshipApplyForItem>();
                activityData.ApplyMasterRecord.Add(record.Id);
                record.ApplyForMessage = applyMessage;
                var digest = character.SteamComp.GetSteamDigest();
                record.SteamDigestId = digest.Id;
                record.OutOfDateTime = TimeHelper.ServerNow() + 7 * 24 * 3600 * 1000;
                record.AddChild(digest);
                return (ErrorCode.ERR_Success, "");
            }
            else
            {
                DBComponent db = DBManagerComponent.Instance.GetAccountDB();
                var activity = await db.Query<CharacterActivityComponent>(masterId);
                if (activity == null)
                {
                    return (ErrorCode.ERR_Error, "masterId not valid");
                }
                DBTempSceneComponent.Instance.AddComponent(activity);
                var masterData = activity.GetActivityData<TActivityMentorshipTreeData>(EActivityType.TActivityMentorshipTree);
                var record = masterData.AddChild<TActivityMentorshipApplyForItem>();
                masterData.ApplyMasterRecord.Add(record.Id);
                record.ApplyForMessage = applyMessage;
                var digest = character.SteamComp.GetSteamDigest();
                record.SteamDigestId = digest.Id;
                record.OutOfDateTime = TimeHelper.ServerNow() + 7 * 24 * 3600 * 1000;
                record.AddChild(digest);
                await db.Save(activity);
                activity.Dispose();
                return (ErrorCode.ERR_Success, "");
            }

        }

        public static async ETTask<(int, string)> DropMentorshipTree(this TActivityMentorshipTree self,
    TCharacter character, string entity_str, bool isMaster)
        {
            await ETTask.CompletedTask;
            if (!long.TryParse(entity_str, out var entityId))
            {
                return (ErrorCode.ERR_Error, "entity_str not valid");
            }
            var activityData = character.ActivityComp.GetActivityData<TActivityMentorshipTreeData>(EActivityType.TActivityMentorshipTree);
            if (isMaster)
            {
                if (activityData.MasterId != entityId)
                {
                    return (ErrorCode.ERR_Error, "master error");
                }
                TCharacter master = self.ServerZoneActivity.ServerZone.CharacterComp.Get(entityId);
                if (master == null)
                {
                    DBComponent db = DBManagerComponent.Instance.GetAccountDB();
                    var activity = await db.Query<CharacterActivityComponent>(entityId);
                    if (activity == null)
                    {
                        return (ErrorCode.ERR_Error, "masterId not valid");
                    }
                    DBTempSceneComponent.Instance.AddComponent(activity);
                    var masterData = activity.GetActivityData<TActivityMentorshipTreeData>(EActivityType.TActivityMentorshipTree);
                    masterData.Apprentice.Remove(character.Id);
                    activityData.MasterId = 0;
                    self.MentorshipTree.Remove(character.Id);
                    await db.Save(activity);
                    activity.Dispose();
                    return (ErrorCode.ERR_Success, "");
                }
                else
                {
                    var masterData = master.ActivityComp.GetActivityData<TActivityMentorshipTreeData>(EActivityType.TActivityMentorshipTree);
                    if (masterData != null)
                    {
                        masterData.Apprentice.Remove(character.Id);
                        activityData.MasterId = 0;
                        self.MentorshipTree.Remove(character.Id);
                        return (ErrorCode.ERR_Success, "");
                    }
                }
                return (ErrorCode.ERR_Error, "cant master find acitvityData");
            }
            else
            {
                if (!activityData.Apprentice.Contains(entityId))
                {
                    return (ErrorCode.ERR_Error, "apprentice error");
                }
                TCharacter apprentice = self.ServerZoneActivity.ServerZone.CharacterComp.Get(entityId);
                if (apprentice == null)
                {
                    DBComponent db = DBManagerComponent.Instance.GetAccountDB();
                    var activity = await db.Query<CharacterActivityComponent>(entityId);
                    if (activity == null)
                    {
                        return (ErrorCode.ERR_Error, "apprenticeId not valid");
                    }
                    DBTempSceneComponent.Instance.AddComponent(activity);
                    var apprenticeData = activity.GetActivityData<TActivityMentorshipTreeData>(EActivityType.TActivityMentorshipTree);
                    apprenticeData.MasterId = 0;
                    self.MentorshipTree.Remove(entityId);
                    activityData.Apprentice.Remove(entityId);
                    await db.Save(activity);
                    activity.Dispose();
                    return (ErrorCode.ERR_Success, "");
                }
                else
                {
                    var apprenticeData = apprentice.ActivityComp.GetActivityData<TActivityMentorshipTreeData>(EActivityType.TActivityMentorshipTree);
                    if (apprenticeData != null)
                    {
                        apprenticeData.MasterId = 0;
                        self.MentorshipTree.Remove(entityId);
                        activityData.Apprentice.Remove(entityId);
                        return (ErrorCode.ERR_Success, "");
                    }
                }
                return (ErrorCode.ERR_Error, "cant find apprentice acitvityData");
            }
        }

        public static async ETTask<(int, string)> ChangeApplyForMasterState(this TActivityMentorshipTree self,
            TCharacter character, string entity_str, bool isAgree)
        {
            await ETTask.CompletedTask;
            if (!long.TryParse(entity_str, out var entityId))
            {
                return (ErrorCode.ERR_Error, "entity_str not valid");
            }
            var activityData = character.ActivityComp.GetActivityData<TActivityMentorshipTreeData>(EActivityType.TActivityMentorshipTree);
            if (activityData.Apprentice.Contains(entityId))
            {
                return (ErrorCode.ERR_Error, "entity had been apprentice");
            }

            foreach (var _applyId in activityData.ApplyMasterRecord)
            {
                var _entity = self.GetChild<TActivityMentorshipApplyForItem>(_applyId);
                if (_entity != null && _entity.SteamDigestId == entityId)
                {
                    activityData.ApplyMasterRecord.Remove(_applyId);
                    _entity.Dispose();
                    if (!isAgree)
                    {
                        return (ErrorCode.ERR_Success, "");
                    }
                    TCharacter Apprentice = self.ServerZoneActivity.ServerZone.CharacterComp.Get(entityId);
                    if (Apprentice == null)
                    {
                        DBComponent db = DBManagerComponent.Instance.GetAccountDB();
                        var activity = await db.Query<CharacterActivityComponent>(entityId);
                        if (activity == null)
                        {
                            return (ErrorCode.ERR_Error, "apprenticeId not valid");
                        }
                        DBTempSceneComponent.Instance.AddComponent(activity);
                        var apprenticeData = activity.GetActivityData<TActivityMentorshipTreeData>(EActivityType.TActivityMentorshipTree);
                        if (apprenticeData.MasterId != 0)
                        {
                            return (ErrorCode.ERR_Error, "other had master");
                        }
                        apprenticeData.MasterId = character.Id;
                        self.MentorshipTree.Add(entityId, character.Id);
                        activityData.Apprentice.Add(entityId);
                        await db.Save(activity);
                        activity.Dispose();
                        return (ErrorCode.ERR_Success, "");
                    }
                    else
                    {
                        TActivityMentorshipTreeData apprenticeData = Apprentice.ActivityComp.GetActivityData<TActivityMentorshipTreeData>(EActivityType.TActivityMentorshipTree);
                        if (apprenticeData.MasterId != 0)
                        {
                            return (ErrorCode.ERR_Error, "other had master");
                        }
                        else
                        {
                            apprenticeData.MasterId = character.Id;
                            self.MentorshipTree.Add(entityId, character.Id);
                            activityData.Apprentice.Add(entityId);
                            return (ErrorCode.ERR_Success, "");
                        }
                    }
                }
            }
            return (ErrorCode.ERR_Error, "can find apply");
        }





    }
}
