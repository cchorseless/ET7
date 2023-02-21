

namespace ET
{
    public class TShopSellItem : Entity, IAwake, ILoad, ISerializeToEntity
    {
        /// <summary>
        /// 配置表id
        /// </summary>
        public int ConfigId;

        public int ShopId;

        public int BuyCount;

        public long CharacterId;

        /// <summary>
        /// 配置json,同步给客户端确保最新
        /// </summary>
        //public string ConfigJson;

    }
}
