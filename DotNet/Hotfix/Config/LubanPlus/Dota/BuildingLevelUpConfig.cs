using Bright.Serialization;
using System.Collections.Generic;

namespace cfg.Dota
{
    public partial class BuildingLevelUpConfig
    {
        private readonly Dictionary<int, string> HeroIdNameMap = new Dictionary<int, string>();

        public string GetHeroName(int heroConfigId)
        {
            if (this.HeroIdNameMap.Count == 0)
            {
                this.DataList.ForEach(record => { this.HeroIdNameMap.Add(record.BindHeroId, record.Id); });
            }

            this.HeroIdNameMap.TryGetValue(heroConfigId, out var r);
            return r;
        }
    }
}