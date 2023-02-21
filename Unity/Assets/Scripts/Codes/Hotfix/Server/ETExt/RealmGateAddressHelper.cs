using System.Collections.Generic;

namespace ET.Server
{
    public static partial class RealmGateAddressHelper
    {

        public static StartSceneConfig GetGate(int zone, int serverid)
        {
            List<StartSceneConfig> zoneGates = StartSceneConfigCategory.Instance.Gates[zone];
            zoneGates = zoneGates.FindAll(info => info.ServerMin <= serverid && info.ServerMax >= serverid &&
            info.Process != GameConfig.GmWebProcessID);
            int n = RandomGenerator.RandomNumber(0, zoneGates.Count);
            return zoneGates[n];
        }

        public static StartSceneConfig GetGMWebGate()
        {
            List<StartSceneConfig> Gates = StartSceneConfigCategory.Instance.GetByProcess(GameConfig.GmWebProcessID);
            for (var i = 0; i < Gates.Count; i++)
            {
                if (Gates[i].Type == SceneType.Gate)
                {
                    return Gates[i];
                }
            }
            return null;
        }

    }
}