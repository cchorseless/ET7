using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    public enum ELogOutHandlerType
    {
        LogOutPlayer = 0,
        LogOutCharacter,
    }

    public static class PlayerLoginOutComponentFunc
    {
        public static void CancelLogOutGate(this PlayerLoginOutComponent self)
        {
            Player player = self.GetParent<Player>();
            player.IsOnline = true;
            if (self.CancelTimer != null)
            {
                self.CancelTimer.Cancel();
                self.CancelTimer = null;
            }
        }

        public static async ETTask DelayLogOutGate(this PlayerLoginOutComponent self)
        {
            Player player = self.GetParent<Player>();
            player.IsOnline = false;
            self.CancelTimer = new ETCancellationToken();
            await TimerComponent.Instance.WaitAsync(GameConfig.WaitReConnectTime, self.CancelTimer);
            self.CancelTimer = null;
            if (!self.IsHadKnockOut)
            {
                switch (self.LogOutType)
                {
                    case (int)ELogOutHandlerType.LogOutPlayer:
                        self.LogOutHandler = self.LogOutPlayer();
                        break;
                    case (int)ELogOutHandlerType.LogOutCharacter:
                        self.LogOutHandler = self.LogOutCharacter();
                        break;

                }
                if (self.LogOutHandler != null)
                {
                    await self.LogOutHandler;
                }
            }
            self.LogOutHandler = null;
        }

        public static async ETTask KnockOutGate(this PlayerLoginOutComponent self)
        {
            if (self.IsHadKnockOut) { return; }
            Player player = self.GetParent<Player>();
            player.IsOnline = false;
            self.IsHadKnockOut = true;
            if (self.CancelTimer != null)
            {
                self.CancelTimer.Cancel();
                self.CancelTimer = null;
            }
            if (self.LogOutHandler != null)
            {
                await self.LogOutHandler;
                self.LogOutHandler = null;
            }
            else
            {
                switch (self.LogOutType)
                {
                    case (int)ELogOutHandlerType.LogOutPlayer:
                        await self.LogOutPlayer();
                        break;
                    case (int)ELogOutHandlerType.LogOutCharacter:
                        await self.LogOutCharacter();
                        break;
                }
            }
        }

        private static async ETTask LogOutPlayer(this PlayerLoginOutComponent self)
        {
            Player player = self.GetParent<Player>();
            if (player.IsOnline == false)
            {
                player.Dispose();
                Log.Debug("player log out => " + player.Account);
            }
            await ETTask.CompletedTask;
        }

        public static async ETTask LogOutCharacter(this PlayerLoginOutComponent self)
        {
            Player player = self.GetParent<Player>();
            TCharacter character = player.GetMyCharacter();
            if (player.IsOnline == false)
            {
                await character.Save();
                player.Dispose();
                Log.Debug("player log out => " + player.Account);
                Log.Console("player log out => " + player.Account);
            }
            await ETTask.CompletedTask;
        }

    }
}
