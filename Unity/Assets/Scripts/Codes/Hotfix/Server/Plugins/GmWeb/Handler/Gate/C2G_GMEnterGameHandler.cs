using System;
using System.Collections.Generic;


namespace ET.Server

{
    [MessageHandler(SceneType.Gate)]
    public class C2G_GMEnterGameHandler : AMRpcHandler<C2G_GMEnterGame, G2C_GMEnterGame>
    {
        protected override async ETTask Run(Session session, C2G_GMEnterGame request, G2C_GMEnterGame response)
        {
            Scene scene = session.DomainScene();
            DBComponent db = DBManagerComponent.Instance.GetZoneDB(scene.Zone);
            Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();

            List<TCharacter> characters = await db.Query<TCharacter>(x => x.Int64PlayerId == player.Id);
            if (characters.Count == 0)
            {
                TCharacter newCharacter = Entity.CreateOne<TCharacter, long>(scene, player.Id);
                newCharacter.ZoneID = 1;
                newCharacter.ServerID = 1;
                var gmDataComp = newCharacter.AddComponent<GmCharacterDataComponent>();
                await db.Save(newCharacter);
                characters.Add(newCharacter);
            }
            TCharacter character = characters[0];
            player.SelectCharacter(character);
            character.SyncClientEntity(character);
            // player 可以接受消息
            player.AddComponent<MailBoxComponent>();
            player.GetComponent<PlayerLoginOutComponent>().LogOutType = (int)ELogOutHandlerType.LogOutCharacter;
            response.MyId = character.Id;
        }
      
    }
}