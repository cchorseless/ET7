using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{

    public enum ERoomType
    {
        Common,
    }


    [MessageHandler(SceneType.Gate)]
    public class C2G_CreateRoomHandler : AMRpcHandler<C2G_CreateRoom, G2C_CreateRoom>
    {
        protected override async ETTask Run(Session session, C2G_CreateRoom request, G2C_CreateRoom response)
        {
            await ETTask.CompletedTask;

            try
            {
                Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
                Room room = RoomManagerComponent.Instance.CreateRoom();
                /// 应该加载Unit
                player.AddComponent<RoomEntity, long>(room.Id);
                int roomType = request.RoomType;
                switch (roomType)
                {
                    case (int)ERoomType.Common:
                        break;
                    default:
                        response.Error = ErrorCode.ERR_Error;
                        response.Message = "创建房间失败";
                        break;
                }
            }
            catch (Exception e)
            {
                ReplyError(response, e);
            }
        }

    }
}
