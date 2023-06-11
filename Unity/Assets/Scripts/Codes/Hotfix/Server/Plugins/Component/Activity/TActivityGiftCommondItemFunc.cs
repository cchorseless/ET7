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
        }
    }
    public static class TActivityGiftCommondItemFunc
    {
        public static void LoadAllChild(this TActivityGiftCommondItem self)
        {
            self.GiftMax = self.ActivityGiftConfig().GiftCount;
        }

        public static cfg.Activity.TActivityGiftCommondRecord ActivityGiftConfig(this TActivityGiftCommondItem self)
        {
            return LuBanConfigComponent.Instance.Config().TActivityGiftCommond.GetOrDefault(self.ConfigId);
        }

    }
}
