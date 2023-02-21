using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public  class TActivityMentorshipApplyForItem : Entity, IAwake, ISerializeToEntity
    {
        public string ApplyForMessage;

        public long SteamDigestId;

        public long OutOfDateTime;


    }
}
