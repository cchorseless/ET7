using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public enum EGmPlayerRole
    {
        /// <summary>
        /// 玩家
        /// </summary>
        GmRole_Player = 0,
        /// <summary>
        /// 客户端发送GM指令
        /// </summary>
        GmRole_PlayerGm = 1,
        /// <summary>
        /// 后台数据查看
        /// </summary>
        GmRole_WebDataView = 2,
        /// <summary>
        /// 活动编辑
        /// </summary>
        GmRole_WebActivityEditer = 4,
        /// <summary>
        /// 修改玩家数据
        /// </summary>
        GmRole_WebPlayerDataEditer = 8,
        /// <summary>
        /// 服务器管理
        /// </summary>
        GmRole_WebServerManager = 16,
        /// <summary>
        /// 超级管理
        /// </summary>
        GmRole_Admin = 2048,
    }


    public static class GmWebConfig
    {


        //public static async ETTask InitGateScene(Scene scene)
        //{
        //    var matchBattle = scene.AddComponent<MatchBattleComponent>();
        //    await matchBattle.LoadDataFromDBAsync();

        //}
    }


}
