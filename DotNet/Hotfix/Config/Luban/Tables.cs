//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;



namespace cfg
{ 
public partial class Tables
{
    public Item.ItemConfig ItemConfig {get; }
    public Item.ItemEquipConfig ItemEquipConfig {get; }
    public Item.ItemBuffConfig ItemBuffConfig {get; }
    public Item.ItemPrizePoolConfig ItemPrizePoolConfig {get; }
    public Item.ItemPrizePoolGroupConfig ItemPrizePoolGroupConfig {get; }
    public Shop.ShopConfig ShopConfig {get; }
    public Activity.ActivityConfig ActivityConfig {get; }
    public Activity.TActivityMonthLogin TActivityMonthLogin {get; }
    public Activity.TActivitySevenDayLogin TActivitySevenDayLogin {get; }
    public Activity.TActivityDailyOnlinePrize TActivityDailyOnlinePrize {get; }
    public Activity.TActivityInvestMetaStone TActivityInvestMetaStone {get; }
    public Activity.TActivityTotalGainMetaStone TActivityTotalGainMetaStone {get; }
    public Activity.TActivityTotalOnlineTime TActivityTotalOnlineTime {get; }
    public Activity.TActivityTotalSpendMetaStone TActivityTotalSpendMetaStone {get; }
    public Activity.TActivityGiftCommond TActivityGiftCommond {get; }
    public Activity.TActivityMentorshipTree TActivityMentorshipTree {get; }
    public Season.SeasonConfig SeasonConfig {get; }
    public Rank.RankPrizeConfig RankPrizeConfig {get; }
    public Draw.DrawTreasureConfig DrawTreasureConfig {get; }
    public Achievement.AchievementConfig AchievementConfig {get; }
    public Title.TitleConfig TitleConfig {get; }
    public Glob.GlobalSetting GlobalSetting {get; }
    public Dota.BuildingLevelUpConfig BuildingLevelUpConfig {get; }
    public Dota.BuildingLevelUpExpConfig BuildingLevelUpExpConfig {get; }
    public Dota.PropConfig PropConfig {get; }
    public Dota.PropRandomConfig PropRandomConfig {get; }
    public Dota.BattlePassChargeConfig BattlePassChargeConfig {get; }
    public Dota.BattlePassTaskConfig BattlePassTaskConfig {get; }
    public Dota.BattlePassLevelUpConfig BattlePassLevelUpConfig {get; }
    public Dota.InfoPassLevelUpConfig InfoPassLevelUpConfig {get; }
    public Dota.RoundEnemyPoolConfig RoundEnemyPoolConfig {get; }

    public Tables(System.Func<string, ByteBuf> loader)
    {
        var tables = new System.Collections.Generic.Dictionary<string, object>();
        ItemConfig = new Item.ItemConfig(loader("item_itemconfig")); 
        tables.Add("Item.ItemConfig", ItemConfig);
        ItemEquipConfig = new Item.ItemEquipConfig(loader("item_itemequipconfig")); 
        tables.Add("Item.ItemEquipConfig", ItemEquipConfig);
        ItemBuffConfig = new Item.ItemBuffConfig(loader("item_itembuffconfig")); 
        tables.Add("Item.ItemBuffConfig", ItemBuffConfig);
        ItemPrizePoolConfig = new Item.ItemPrizePoolConfig(loader("item_itemprizepoolconfig")); 
        tables.Add("Item.ItemPrizePoolConfig", ItemPrizePoolConfig);
        ItemPrizePoolGroupConfig = new Item.ItemPrizePoolGroupConfig(loader("item_itemprizepoolgroupconfig")); 
        tables.Add("Item.ItemPrizePoolGroupConfig", ItemPrizePoolGroupConfig);
        ShopConfig = new Shop.ShopConfig(loader("shop_shopconfig")); 
        tables.Add("Shop.ShopConfig", ShopConfig);
        ActivityConfig = new Activity.ActivityConfig(loader("activity_activityconfig")); 
        tables.Add("Activity.ActivityConfig", ActivityConfig);
        TActivityMonthLogin = new Activity.TActivityMonthLogin(loader("activity_tactivitymonthlogin")); 
        tables.Add("Activity.TActivityMonthLogin", TActivityMonthLogin);
        TActivitySevenDayLogin = new Activity.TActivitySevenDayLogin(loader("activity_tactivitysevendaylogin")); 
        tables.Add("Activity.TActivitySevenDayLogin", TActivitySevenDayLogin);
        TActivityDailyOnlinePrize = new Activity.TActivityDailyOnlinePrize(loader("activity_tactivitydailyonlineprize")); 
        tables.Add("Activity.TActivityDailyOnlinePrize", TActivityDailyOnlinePrize);
        TActivityInvestMetaStone = new Activity.TActivityInvestMetaStone(loader("activity_tactivityinvestmetastone")); 
        tables.Add("Activity.TActivityInvestMetaStone", TActivityInvestMetaStone);
        TActivityTotalGainMetaStone = new Activity.TActivityTotalGainMetaStone(loader("activity_tactivitytotalgainmetastone")); 
        tables.Add("Activity.TActivityTotalGainMetaStone", TActivityTotalGainMetaStone);
        TActivityTotalOnlineTime = new Activity.TActivityTotalOnlineTime(loader("activity_tactivitytotalonlinetime")); 
        tables.Add("Activity.TActivityTotalOnlineTime", TActivityTotalOnlineTime);
        TActivityTotalSpendMetaStone = new Activity.TActivityTotalSpendMetaStone(loader("activity_tactivitytotalspendmetastone")); 
        tables.Add("Activity.TActivityTotalSpendMetaStone", TActivityTotalSpendMetaStone);
        TActivityGiftCommond = new Activity.TActivityGiftCommond(loader("activity_tactivitygiftcommond")); 
        tables.Add("Activity.TActivityGiftCommond", TActivityGiftCommond);
        TActivityMentorshipTree = new Activity.TActivityMentorshipTree(loader("activity_tactivitymentorshiptree")); 
        tables.Add("Activity.TActivityMentorshipTree", TActivityMentorshipTree);
        SeasonConfig = new Season.SeasonConfig(loader("season_seasonconfig")); 
        tables.Add("Season.SeasonConfig", SeasonConfig);
        RankPrizeConfig = new Rank.RankPrizeConfig(loader("rank_rankprizeconfig")); 
        tables.Add("Rank.RankPrizeConfig", RankPrizeConfig);
        DrawTreasureConfig = new Draw.DrawTreasureConfig(loader("draw_drawtreasureconfig")); 
        tables.Add("Draw.DrawTreasureConfig", DrawTreasureConfig);
        AchievementConfig = new Achievement.AchievementConfig(loader("achievement_achievementconfig")); 
        tables.Add("Achievement.AchievementConfig", AchievementConfig);
        TitleConfig = new Title.TitleConfig(loader("title_titleconfig")); 
        tables.Add("Title.TitleConfig", TitleConfig);
        GlobalSetting = new Glob.GlobalSetting(loader("glob_globalsetting")); 
        tables.Add("Glob.GlobalSetting", GlobalSetting);
        BuildingLevelUpConfig = new Dota.BuildingLevelUpConfig(loader("dota_buildinglevelupconfig")); 
        tables.Add("Dota.BuildingLevelUpConfig", BuildingLevelUpConfig);
        BuildingLevelUpExpConfig = new Dota.BuildingLevelUpExpConfig(loader("dota_buildinglevelupexpconfig")); 
        tables.Add("Dota.BuildingLevelUpExpConfig", BuildingLevelUpExpConfig);
        PropConfig = new Dota.PropConfig(loader("dota_propconfig")); 
        tables.Add("Dota.PropConfig", PropConfig);
        PropRandomConfig = new Dota.PropRandomConfig(loader("dota_proprandomconfig")); 
        tables.Add("Dota.PropRandomConfig", PropRandomConfig);
        BattlePassChargeConfig = new Dota.BattlePassChargeConfig(loader("dota_battlepasschargeconfig")); 
        tables.Add("Dota.BattlePassChargeConfig", BattlePassChargeConfig);
        BattlePassTaskConfig = new Dota.BattlePassTaskConfig(loader("dota_battlepasstaskconfig")); 
        tables.Add("Dota.BattlePassTaskConfig", BattlePassTaskConfig);
        BattlePassLevelUpConfig = new Dota.BattlePassLevelUpConfig(loader("dota_battlepasslevelupconfig")); 
        tables.Add("Dota.BattlePassLevelUpConfig", BattlePassLevelUpConfig);
        InfoPassLevelUpConfig = new Dota.InfoPassLevelUpConfig(loader("dota_infopasslevelupconfig")); 
        tables.Add("Dota.InfoPassLevelUpConfig", InfoPassLevelUpConfig);
        RoundEnemyPoolConfig = new Dota.RoundEnemyPoolConfig(loader("dota_roundenemypoolconfig")); 
        tables.Add("Dota.RoundEnemyPoolConfig", RoundEnemyPoolConfig);

        PostInit();
        ItemConfig.Resolve(tables); 
        ItemEquipConfig.Resolve(tables); 
        ItemBuffConfig.Resolve(tables); 
        ItemPrizePoolConfig.Resolve(tables); 
        ItemPrizePoolGroupConfig.Resolve(tables); 
        ShopConfig.Resolve(tables); 
        ActivityConfig.Resolve(tables); 
        TActivityMonthLogin.Resolve(tables); 
        TActivitySevenDayLogin.Resolve(tables); 
        TActivityDailyOnlinePrize.Resolve(tables); 
        TActivityInvestMetaStone.Resolve(tables); 
        TActivityTotalGainMetaStone.Resolve(tables); 
        TActivityTotalOnlineTime.Resolve(tables); 
        TActivityTotalSpendMetaStone.Resolve(tables); 
        TActivityGiftCommond.Resolve(tables); 
        TActivityMentorshipTree.Resolve(tables); 
        SeasonConfig.Resolve(tables); 
        RankPrizeConfig.Resolve(tables); 
        DrawTreasureConfig.Resolve(tables); 
        AchievementConfig.Resolve(tables); 
        TitleConfig.Resolve(tables); 
        GlobalSetting.Resolve(tables); 
        BuildingLevelUpConfig.Resolve(tables); 
        BuildingLevelUpExpConfig.Resolve(tables); 
        PropConfig.Resolve(tables); 
        PropRandomConfig.Resolve(tables); 
        BattlePassChargeConfig.Resolve(tables); 
        BattlePassTaskConfig.Resolve(tables); 
        BattlePassLevelUpConfig.Resolve(tables); 
        InfoPassLevelUpConfig.Resolve(tables); 
        RoundEnemyPoolConfig.Resolve(tables); 
        PostResolve();
    }

    public void TranslateText(System.Func<string, string, string> translator)
    {
        ItemConfig.TranslateText(translator); 
        ItemEquipConfig.TranslateText(translator); 
        ItemBuffConfig.TranslateText(translator); 
        ItemPrizePoolConfig.TranslateText(translator); 
        ItemPrizePoolGroupConfig.TranslateText(translator); 
        ShopConfig.TranslateText(translator); 
        ActivityConfig.TranslateText(translator); 
        TActivityMonthLogin.TranslateText(translator); 
        TActivitySevenDayLogin.TranslateText(translator); 
        TActivityDailyOnlinePrize.TranslateText(translator); 
        TActivityInvestMetaStone.TranslateText(translator); 
        TActivityTotalGainMetaStone.TranslateText(translator); 
        TActivityTotalOnlineTime.TranslateText(translator); 
        TActivityTotalSpendMetaStone.TranslateText(translator); 
        TActivityGiftCommond.TranslateText(translator); 
        TActivityMentorshipTree.TranslateText(translator); 
        SeasonConfig.TranslateText(translator); 
        RankPrizeConfig.TranslateText(translator); 
        DrawTreasureConfig.TranslateText(translator); 
        AchievementConfig.TranslateText(translator); 
        TitleConfig.TranslateText(translator); 
        GlobalSetting.TranslateText(translator); 
        BuildingLevelUpConfig.TranslateText(translator); 
        BuildingLevelUpExpConfig.TranslateText(translator); 
        PropConfig.TranslateText(translator); 
        PropRandomConfig.TranslateText(translator); 
        BattlePassChargeConfig.TranslateText(translator); 
        BattlePassTaskConfig.TranslateText(translator); 
        BattlePassLevelUpConfig.TranslateText(translator); 
        InfoPassLevelUpConfig.TranslateText(translator); 
        RoundEnemyPoolConfig.TranslateText(translator); 
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}