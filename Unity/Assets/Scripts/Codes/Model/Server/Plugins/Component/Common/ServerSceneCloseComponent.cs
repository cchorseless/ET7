using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class ServerSceneCloseComponent : Entity, IAwake<int>, IDestroy
    {
        public int SceneType;
        public bool IsClosing = false;
    }
}
