using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class DBTempSceneComponent : Entity, IAwake, IDestroy
    {
        public static DBTempSceneComponent Instance;

    }

    [ObjectSystem]
    public class DBTempSceneComponentAwakeSystem : AwakeSystem<DBTempSceneComponent>
    {
        protected override void Awake(DBTempSceneComponent self)
        {
            DBTempSceneComponent.Instance = self;
        }
    }


    [ObjectSystem]
    public class DBTempSceneComponentDestroySystem : DestroySystem<DBTempSceneComponent>
    {
        protected override void Destroy(DBTempSceneComponent self)
        {
            DBTempSceneComponent.Instance = null;
        }
    }
}
