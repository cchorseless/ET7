//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace ET.Conf.Dota
{
public sealed partial class BattlePassTaskConfigRecord :  Bright.Config.BeanBase 
{
    public BattlePassTaskConfigRecord(ByteBuf _buf) 
    {
        Id = _buf.ReadInt();
        TaskName = _buf.ReadString();
        TaskDes = _buf.ReadString();
        TaskType = _buf.ReadString();
        BindHero = _buf.ReadString();
        TaskFinishCondition = Task.TaskFinishConditionBean.DeserializeTaskFinishConditionBean(_buf);
        TaskPrize = Item.ItemInfoBean.DeserializeItemInfoBean(_buf);
        PostInit();
    }

    public static BattlePassTaskConfigRecord DeserializeBattlePassTaskConfigRecord(ByteBuf _buf)
    {
        return new Dota.BattlePassTaskConfigRecord(_buf);
    }

    /// <summary>
    /// 任务id
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// 任务名称
    /// </summary>
    public string TaskName { get; private set; }
    /// <summary>
    /// 任务描述
    /// </summary>
    public string TaskDes { get; private set; }
    /// <summary>
    /// 任务类型
    /// </summary>
    public string TaskType { get; private set; }
    /// <summary>
    /// 绑定的英雄
    /// </summary>
    public string BindHero { get; private set; }
    public Task.TaskFinishConditionBean TaskFinishCondition { get; private set; }
    public Item.ItemInfoBean TaskPrize { get; private set; }

    public const int __ID__ = 1500789463;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        TaskFinishCondition?.Resolve(_tables);
        TaskPrize?.Resolve(_tables);
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
        TaskFinishCondition?.TranslateText(translator);
        TaskPrize?.TranslateText(translator);
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "TaskName:" + TaskName + ","
        + "TaskDes:" + TaskDes + ","
        + "TaskType:" + TaskType + ","
        + "BindHero:" + BindHero + ","
        + "TaskFinishCondition:" + TaskFinishCondition + ","
        + "TaskPrize:" + TaskPrize + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}