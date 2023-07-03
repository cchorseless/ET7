using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class TActivityMentorshipPrizeItemAwakeSystem : AwakeSystem<TActivityMentorshipPrizeItem,int>
    {
        protected override void Awake(TActivityMentorshipPrizeItem self,int configId)
        {
            self.ConfigId = configId;
        }
    }

    public static class TActivityMentorshipPrizeItemFunc
    {
        public static Conf.Activity.TActivityMentorshipTreeRecord MentorshipPrizeConfig(this TActivityMentorshipPrizeItem self)
        {
            return LuBanConfigComponent.Instance.Config().TActivityMentorshipTree.GetOrDefault(self.ConfigId);
        }

    }
}
