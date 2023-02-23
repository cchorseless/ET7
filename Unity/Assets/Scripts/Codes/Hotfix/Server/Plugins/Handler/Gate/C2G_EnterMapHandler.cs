using System;
using System.Collections.Generic;


namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    public class C2G_EnterMapHandler : AMRpcHandler<C2G_EnterMap, G2C_EnterMap>
    {
        protected override async ETTask Run(Session session, C2G_EnterMap request, G2C_EnterMap response)
        {
            await ETTask.CompletedTask;
            Scene scene = session.DomainScene();
            DBComponent db = DBManagerComponent.Instance.GetZoneDB(scene.Zone);
            Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
            TCharacter character = player.GetMyCharacter();
            // 断线重连
            if (character != null)
            {
                character.SyncClientEntity(character);
                response.MyId = character.Id;
                return;
            }
            List<TCharacter> characters = await db.Query<TCharacter>(x => x.Int64PlayerId == player.Id);
            if (characters.Count == 0 && GameConfig.AutoCreateDefaultCharacter)
            {
                TCharacter newCharacter =  player.AddChild<TCharacter, long>(player.Id);
                newCharacter.ZoneID = session.DomainZone();
                await db.Save(newCharacter);
                characters.Add(newCharacter);
            }
            int characterIndex = (request.Index > 0) ? request.Index : 1;
            if (characters.Count < characterIndex)
            {
                response.Error = ErrorCode.ERR_Error;
                response.Message = "选择角色错误";
                return;
            }
            character = characters[characterIndex - 1];
            player.SelectCharacter(character);
            character.SyncClientEntity(character);
            player.GetComponent<PlayerLoginOutComponent>().LogOutType = (int)ELogOutHandlerType.LogOutCharacter;
            // player 可以接受消息
            player.AddComponent<MailBoxComponent>();
            // 在Gate上动态创建一个Map Scene，把Unit从DB中加载放进来，然后传送到真正的Map中，这样登陆跟传送的逻辑就完全一样了
            GateMapComponent gateMapComponent = player.GetComponent<GateMapComponent>();
            gateMapComponent.Scene = await SceneFactory.CreateServerScene(gateMapComponent, player.Id, IdGenerater.Instance.GenerateInstanceId(), gateMapComponent.DomainZone(), "GateMap", SceneType.Map);
            Scene gateScene = gateMapComponent.Scene;
            // 这里可以从DB中加载Unit
            Unit unit = null;
            List<Unit> units = await db.Query<Unit>(unit => unit.Id == character.Id);
            if (units.Count == 0)
            {
                unit = UnitFactory.Create(gateScene, character.Id, UnitType.Player);
                await db.Save(unit);
            }
            else
            {
                unit = units[0];
                gateScene.AddChild(unit);
            }
            player.SelectMapUnit(unit);
            UnitGateComponent unitGate = unit.AddComponent<UnitGateComponent, long>(session.InstanceId);
            unitGate.GatePlayerActorId = player.Id;
            StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), "Map1");
            response.MyId = character.Id;
            // 开始传送
            await TransferHelper.Transfer(unit, startSceneConfig.InstanceId, startSceneConfig.Name);
        }
    }
}