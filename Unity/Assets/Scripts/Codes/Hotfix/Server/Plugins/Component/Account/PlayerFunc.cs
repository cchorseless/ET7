using System;
using System.Linq;

namespace ET.Server
{
    [FriendOf(typeof (Player))]
    public static class PlayerSystem
    {
        [ObjectSystem]
        public class PlayerAwakeSystem: AwakeSystem<Player, string>
        {
            protected override void Awake(Player self, string a)
            {
                self.Account = a;
            }
        }

        [ObjectSystem]
        public class PlayerDestroySystem: DestroySystem<Player>
        {
            protected override void Destroy(Player self)
            {
                self.Domain.GetComponent<PlayerComponent>()?.Remove(self.Id);
            }
        }

        public static void SelectCharacter(this Player player, TCharacter character)
        {
            if (character == null)
            {
                Log.Error("character == null");
                return;
            }

            if (player.GetChild<TCharacter>(player.CharacterId) == null)
            {
                if (character.Parent != player)
                {
                    player.AddChild(character);
                }

                player.CharacterId = character.Id;
                character.GmLevel = player.GmLevel;
                character.Name = player.Account;
                character.IsFirstLoginToday = character.LastLoginTime < TimeInfo.Instance.Transition(DateTime.Now.Date);
                character.IsFirstLoginWeek = false;
                character.IsFirstLoginSeason = false;
                if (character.IsFirstLoginToday)
                {
                    character.PartialOnlineTime = 0;
                    var lastWeek = TimeHelper.WeekCount(TimeInfo.Instance.ToDateTime(character.LastLoginTime));
                    character.IsFirstLoginWeek = lastWeek < TimeHelper.WeekCount(TimeHelper.DateTimeNow());
                    // Log.Console($"  {character.ServerID} --- {character.GetMyServerZone() == null}");
                    // Log.Console($"  {JsonHelper.ToJson(ServerZoneManageComponent.Instance.ServerZoneDict.Keys.ToList())} ");
                    // Log.Console($"  {JsonHelper.ToJson(ServerZoneManageComponent.Instance.ServerZoneDict.Values.ToList())} ");
                    character.IsFirstLoginSeason = character.GetMyServerZone().SeasonComp.CurSeason.IsInSeasonDuration(character.LastLoginTime);
                }

                character.LastLoginTime = TimeHelper.ServerNow();
            }
            else
            {
                Log.Error("Repeat SelectCharacter");
            }
        }

        public static void SelectMapUnit(this Player player, Unit unit)
        {
            if (player.UnitId == 0)
            {
                player.UnitId = unit.Id;
            }
            else
            {
                Log.Error("Repeat Select Unit");
            }
        }

        public static TCharacter GetMyCharacter(this Player player)
        {
            TCharacter character = null;
            if (player.CharacterId != 0)
            {
                character = player.GetChild<TCharacter>(player.CharacterId);
            }

            return character;
        }

        public static Session GetMySession(this Player player)
        {
            if (player.GateSessionActorId != 0)
            {
                return Root.Instance.Get(player.GateSessionActorId) as Session;
            }

            return null;
        }

        public static bool HasGmRolePermission(this Player self, EGmPlayerRole role)
        {
            if (self.GmLevel == (int)EGmPlayerRole.GmRole_Admin)
            {
                return true;
            }
            else
            {
                return (self.GmLevel & (int)role) != 0;
            }
        }
    }
}