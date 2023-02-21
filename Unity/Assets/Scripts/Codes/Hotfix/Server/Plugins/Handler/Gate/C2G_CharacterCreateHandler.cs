using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    public class C2G_CharacterCreateHandler : AMRpcHandler<C2G_CharacterCreate, G2C_CharacterCreate>
    {
        protected override async ETTask Run(Session session, C2G_CharacterCreate request, G2C_CharacterCreate response)
        {
            await ETTask.CompletedTask;
            try
            {
                Scene scene = session.DomainScene();
                DBComponent db = DBManagerComponent.Instance.GetZoneDB(scene.Zone);
                Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
                //判断角色是否达到上限
                List<TCharacter> characterList = await db.Query<TCharacter>(x => x.Int64PlayerId == player.Id);
                if (characterList.Count >= GameConfig.MaxCharacterCount)
                {
                    response.Error = ErrorCode.ERR_Error;
                    response.Message = "已达上限";
                    return;
                }
                //player.LastPlay = request.Index;
                TCharacter newCharacter = Entity.CreateOne<TCharacter, long>(scene, player.Id);
                newCharacter.Gender = request.Gender;

                ////TODO为新建角色增加各种物品
                ////创建背包
                //TBagComponent newBag = DBCollection.CreateWithId<TBagComponent>(scene, newCharacter.Id);

                //newBag.MaxSize = 80;
                //newBag.CurrSize = 48;
                //newBag.Items = new List<Item>();
                ////为角色添加10个金创药
                //newBag.AddBagItem(1001, 10);
                //newBag.AddBagItem(2030, 2);
                //newBag.AddBagItem(2032, 1);
                //newBag.AddBagItem(1028, 3);
                //newBag.AddBagItem(1506, 1);
                //newBag.AddBagItem(1016, 1);
                //newBag.AddBagItem(1026, 3);
                //newBag.AddBagItem(1027, 2);
                //newBag.AddBagItem(3020, 1);
                //newBag.AddBagItem(5028, 1);
                //newBag.AddBagItem(7029, 2);
                //newBag.AddBagItem(10018, 1);
                //newBag.AddBagItem(4007, 1);
                //newBag.AddBagItem(2031, 1);
                //newBag.AddBagItem(2033, 1);
                //newBag.AddBagItem(2025, 1);
                //newBag.AddBagItem(1009, 50);
                //newBag.AddBagItem(1008, 18);

                //TEquipComponent newEquip = DBCollection.CreateWithId<TEquipComponent>(scene, newCharacter.Id);


                //response.Characters = DataHelper.GetCharacterInfo(newCharacter);
                //response.Index = request.Index;


                //await db.Save(user);
                await db.Save(newCharacter);
                //await db.Save(newBag);
                //await db.Save(newEquip);
                response.Error = ErrorCode.ERR_Success;
            }
            catch (Exception e)
            {
                ReplyError(response, e);
            }
        }
    }
}
