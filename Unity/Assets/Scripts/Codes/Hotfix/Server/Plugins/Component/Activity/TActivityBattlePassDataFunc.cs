using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class TActivityBattlePassDataAwakeSystem : AwakeSystem<TActivityBattlePassData>
    {
        protected override void Awake(TActivityBattlePassData self)
        {
            self.IsVip = false;
            self.Level = 1;
        }
    }

    /// <summary>
    /// 监视BattlePassExp变化
    /// </summary>
    [NumericWatcher(SceneType.Gate, EMoneyType.BattlePassExp)]
    public class NumericWatcher_BattlePassExp : INumericWatcher
    {
        public void Run(Entity unit, ET.EventType.NumbericChange args)
        {
            CharacterDataComponent characterDataComp = args.Unit as CharacterDataComponent;
            var character = characterDataComp.Character;
            var serverzone = character.GetMyServerZone();
            var activity = serverzone.ActivityComp.GetActivity<TActivityBattlePass>(EActivityType.TActivityBattlePass);
            var activityData = character.ActivityComp.GetActivityData<TActivityBattlePassData>(EActivityType.TActivityBattlePass);
            var config = LuBanConfigComponent.Instance.Config().TActivityBattlePass.GetOrDefault(activity.ConfigId);
            if (config != null)
            {
                foreach (var info in config.ItemPrize)
                {
                    if (info.Exp > args.New)
                    {
                        activityData.Level = info.Level;
                        break;
                    }
                }
            }
        }
    }


    public static class TActivityBattlePassDataFunc
    {
        public static void LoadAllChild(this TActivityBattlePassData self)
        {

        }
    }
}
