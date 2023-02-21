using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class TAccountInfo : Entity
    {
        //用户名
        public string Account { get; set; }

        //密码
        public string Password { get; set; }

        public int GmLevel { get; set; }

        public long CreateTime;

        public long LastLoginTime;
        
        public long LastGateId;

    }
}
