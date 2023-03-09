namespace ET
{
    [ConsoleHandler(ConsoleMode.ReloadDll)]
    public class ReloadDllConsoleHandler: IConsoleHandler
    {
        public async ETTask Run(ModeContex contex, string content)
        {
            switch (content)
            {
                case ConsoleMode.ReloadDll:
                    contex.Parent.RemoveComponent<ModeContex>();
                    await Handle();
                    break;
            }

            await ETTask.CompletedTask;
        }

        public static async ETTask Handle()
        {
            CodeLoader.Instance.LoadHotfix();
            // 先加載配置
            await LuBanConfigComponent.Instance.Reload();
            EventSystem.Instance.Load();
            Log.Console("ReloadDll Finish");
        }
    }
}