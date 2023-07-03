//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace ET.Conf.Rank
{
   
public partial class RankPrizeConfig
{
    private readonly Dictionary<int, Rank.RankPrizeConfigRecord> _dataMap;
    private readonly List<Rank.RankPrizeConfigRecord> _dataList;
    
    public RankPrizeConfig(ByteBuf _buf)
    {
        _dataMap = new Dictionary<int, Rank.RankPrizeConfigRecord>();
        _dataList = new List<Rank.RankPrizeConfigRecord>();
        
        for(int n = _buf.ReadSize() ; n > 0 ; --n)
        {
            Rank.RankPrizeConfigRecord _v;
            _v = Rank.RankPrizeConfigRecord.DeserializeRankPrizeConfigRecord(_buf);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
        PostInit();
    }

    public Dictionary<int, Rank.RankPrizeConfigRecord> DataMap => _dataMap;
    public List<Rank.RankPrizeConfigRecord> DataList => _dataList;

    public Rank.RankPrizeConfigRecord GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public Rank.RankPrizeConfigRecord Get(int key) => _dataMap[key];
    public Rank.RankPrizeConfigRecord this[int key] => _dataMap[key];

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