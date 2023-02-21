using System;
using System.Collections.Generic;


namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    public class C2G_GetCharacterHandler : AMRpcHandler<C2G_GetCharacter, G2C_GetCharacter>
    {
        protected override async ETTask Run(Session session, C2G_GetCharacter request, G2C_GetCharacter response)
        {
            await ETTask.CompletedTask;
            try
            {
                Scene scene = session.DomainScene();
                DBComponent db = DBManagerComponent.Instance.GetZoneDB(scene.Zone);
                Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
                List<TCharacter> characters = await db.Query<TCharacter>(x => x.Int64PlayerId == player.Id);
                //foreach (var character in characters)
                //{
                //    response.Characters.Add(DataHelper.GetCharacterInfo(character));
                //}
                //response.Index = user.LastPlay;
                response.Error = ErrorCode.ERR_Success;

            }
            catch (Exception e)
            {
                ReplyError(response, e);
            }

        }
    }
}
