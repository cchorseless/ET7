using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Gate, "/Handle_CharacterMail")]
    public class Http_Post_Handle_CharacterMailHandler : HttpPostHandler<C2H_Handle_CharacterMail, H2C_CommonResponse>
    {
        protected override async ETTask Run(Entity domain, C2H_Handle_CharacterMail request, H2C_CommonResponse response, long playerid)
        {
            Scene scene = domain.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerid);
            var character = player.GetMyCharacter();
            if ( character.MailComp != null)
            {
                if (request.IsOneKey)
                {
                    switch (request.HandleType)
                    {
                        case 1:
                            (response.Error, response.Message) = character.MailComp.ReadAllMail();
                            break;
                        case 2:
                            (response.Error, response.Message) = character.MailComp.GetItemAllMail();
                            break;
                        case 3:
                            (response.Error, response.Message) = character.MailComp.DeleteAllMail();
                            break;
                    }
                }
                else
                {
                    if(long.TryParse(request.MailId, out var mailId))
                    {
                        switch (request.HandleType)
                        {
                            case 1:
                                (response.Error, response.Message) = character.MailComp.ReadOneMail(mailId);
                                break;
                            case 2:
                                (response.Error, response.Message) = character.MailComp.GetItemOneMail(mailId);
                                break;
                            case 3:
                                (response.Error, response.Message) = character.MailComp.DeleteOneMail(mailId);
                                break;
                        }
                    }
                }
            }
            await ETTask.CompletedTask;
        }
    }
}
