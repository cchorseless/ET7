//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace cfg.Glob
{
   
public partial class GlobalSetting
{

     private readonly Glob.GlobalSettingRecord _data;

    public GlobalSetting(ByteBuf _buf)
    {
        int n = _buf.ReadSize();
        if (n != 1) throw new SerializationException("table mode=one, but size != 1");
        _data = Glob.GlobalSettingRecord.DeserializeGlobalSettingRecord(_buf);
        PostInit();
    }


    /// <summary>
    /// desc1
    /// </summary>
     public int GuildOpenLevel => _data.GuildOpenLevel;
    /// <summary>
    /// desc2
    /// </summary>
     public int BagInitCapacity => _data.BagInitCapacity;
    /// <summary>
    /// desc3
    /// </summary>
     public int BagMaxCapacity => _data.BagMaxCapacity;
    /// <summary>
    /// desc4
    /// </summary>
     public System.Collections.Generic.List<int> NewbieTasks => _data.NewbieTasks;

    public void Resolve(Dictionary<string, object> _tables)
    {
        _data.Resolve(_tables);
        PostResolve();
    }

    public void TranslateText(System.Func<string, string, string> translator)
    {
        _data.TranslateText(translator);
    }

    
    partial void PostInit();
    partial void PostResolve();
}

}