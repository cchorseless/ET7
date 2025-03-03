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
   
public partial class InfoPassLevelUpConfig
{
    private readonly Dictionary<int, Dota.InfoPassLevelUpConfigRecord> _dataMap;
    private readonly List<Dota.InfoPassLevelUpConfigRecord> _dataList;
    
    public InfoPassLevelUpConfig(ByteBuf _buf)
    {
        _dataMap = new Dictionary<int, Dota.InfoPassLevelUpConfigRecord>();
        _dataList = new List<Dota.InfoPassLevelUpConfigRecord>();
        
        for(int n = _buf.ReadSize() ; n > 0 ; --n)
        {
            Dota.InfoPassLevelUpConfigRecord _v;
            _v = Dota.InfoPassLevelUpConfigRecord.DeserializeInfoPassLevelUpConfigRecord(_buf);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
        PostInit();
    }

    public Dictionary<int, Dota.InfoPassLevelUpConfigRecord> DataMap => _dataMap;
    public List<Dota.InfoPassLevelUpConfigRecord> DataList => _dataList;

    public Dota.InfoPassLevelUpConfigRecord GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public Dota.InfoPassLevelUpConfigRecord Get(int key) => _dataMap[key];
    public Dota.InfoPassLevelUpConfigRecord this[int key] => _dataMap[key];

    public void Resolve(Dictionary<string, object> _tables)
    {
        foreach(var v in _dataList)
        {
            v.Resolve(_tables);
        }
        PostResolve();
    }

    public void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var v in _dataList)
        {
            v.TranslateText(translator);
        }
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}