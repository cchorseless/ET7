using System;
using System.Collections.Generic;


namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    public class C2G_EnterGameHandler : AMRpcHandler<C2G_EnterGame, G2C_EnterGame>
    {
        protected override async ETTask Run(Session session, C2G_EnterGame request, G2C_EnterGame response)
        {
            await ETTask.CompletedTask;
            Scene scene = session.DomainScene();
            DBComponent db = DBManagerComponent.Instance.GetZoneDB(scene.Zone);
            var sessionPlayer = session.GetComponent<SessionPlayerComponent>();
            Player player = sessionPlayer.GetMyPlayer();
            TCharacter character = player.GetMyCharacter();
            // 断线重连
            if (character != null)
            {
                character.SyncClientEntity(character);
            }
            else
            {
                List<TCharacter> characters = await db.Query<TCharacter>(x =>
                x.Int64PlayerId == player.Id &&
                x.ServerID == sessionPlayer.ServerId);
                if (characters.Count == 0 && GameConfig.AutoCreateDefaultCharacter)
                {
                    TCharacter newCharacter =  player.AddChild<TCharacter, long>(player.Id);
                    newCharacter.ZoneID = session.DomainZone();
                    newCharacter.ServerID = sessionPlayer.ServerId;
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
                // player 可以接受消息
                player.AddComponent<MailBoxComponent>();
            }
            player.GetComponent<PlayerLoginOutComponent>().LogOutType = (int)ELogOutHandlerType.LogOutCharacter;
            response.MyId = character.Id;
        }
    }
}