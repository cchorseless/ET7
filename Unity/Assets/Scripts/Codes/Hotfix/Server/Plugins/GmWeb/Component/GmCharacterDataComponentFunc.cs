using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ObjectSystem]
    public class GmCharacterDataComponentAwakeSystem : AwakeSystem<GmCharacterDataComponent>
    {
        protected override void Awake(GmCharacterDataComponent self)
        {
            self.Routes = new List<string>();
            self.Avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif";
            //https://wpimg.wallstcn.com/4c69009c-0fd4-4153-b112-6cb53d1cf943
        }
    }


}
