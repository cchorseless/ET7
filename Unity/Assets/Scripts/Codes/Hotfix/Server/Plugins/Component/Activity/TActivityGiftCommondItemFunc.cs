using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [ObjectSystem]
    public class TActivityGiftCommondItemAwakeSystem : AwakeSystem<TActivityGiftCommondItem, string>
    {
        protected override void Awake(TActivityGiftCommondItem self, string configId)
        {
            self.ConfigId = configId;
            self.Des = self.ActivityGiftConfig().Des;
        }
    }
    public static class TActivityGiftCommondItemFunc
    {
        public static void LoadAllChild(this TActivityGiftCommondItem self)
        {
            self.GiftMax = self.ActivityGiftConfig().GiftCount;
            self.IsShowUI = self.ActivityGiftConfig().BindMonthIndex == TimeHelper.DateTimeNow().Month;
        }

        public static Conf.Activity.TActivityGiftCommondRecord ActivityGiftConfig(this TActivityGiftCommondItem self)
        {
            return LuBanConfigComponent.Instance.Config().TActivityGiftCommond.GetOrDefault(self.ConfigId);
        }

    }
}
