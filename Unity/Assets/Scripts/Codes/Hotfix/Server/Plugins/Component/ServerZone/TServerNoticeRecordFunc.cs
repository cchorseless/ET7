using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{

    public static class TServerNoticeRecordFunc
    {

    }

    [StatefulTimer]
    public class TServerNoticeRecordTimer : AStatefulTimer<TServerNoticeRecord>
    {
        public override async ETTask Run(TServerNoticeRecord self)
        {
            if (self.State.Contains((int)EStatefulTimer.WaitEnable))
            {
                await ServerZoneManageComponent.Instance.EnableNotice(self);
            }
            await ETTask.CompletedTask;
        }
    }
}
