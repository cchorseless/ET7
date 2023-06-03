using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public enum EMailState
    {
        Read = 1,
        UnRead = 2,
        ItemGet = 4,
        UnItemGet = 8,
    }
    public class TMail : Entity, IAwake, ISerializeToEntity
    {
        public string Title;
        public string Content;
        public HashSet<int> State;

        public long From;
        public string FromDes;
        public List<long> To;
        public string ToDes;
        public long Time;
        public int ValidTime;

        public List<FItemInfo> Items;


    }
}
