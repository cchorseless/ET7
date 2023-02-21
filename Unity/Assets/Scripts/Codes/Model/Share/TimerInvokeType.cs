namespace ET
{
    [UniqueId(100, 10000)]
    public static class TimerInvokeType
    {
        // 框架层100-200，逻辑层的timer type从200起
        public const int WaitTimer = 100;
        public const int SessionIdleChecker = 101;
        public const int ActorLocationSenderChecker = 102;
        public const int ActorMessageSenderChecker = 103;
        
        // 框架层100-200，逻辑层的timer type 200-300
        public const int MoveTimer = 201;
        public const int AITimer = 202;
        public const int SessionAcceptTimeout = 203;
        public const int RoomCheckEmptyTimeout = 204;

        public const int TActivityDailyOnlinePrize = 2000;
        public const int CharacterTitleItem = 2001;
        public const int ServerZoneHourPlan = 3000;
        public const int ServerZoneDailyPlan = 3001;
        public const int ServerZoneWeekPlan = 3002;
        public const int ServerZoneMonthPlan = 3003;
    }
}