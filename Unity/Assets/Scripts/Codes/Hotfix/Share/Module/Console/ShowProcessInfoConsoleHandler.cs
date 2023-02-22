namespace ET
{
    [ConsoleHandler(ConsoleMode.ShowMemory)]
    public class ShowProcessInfoConsoleHandler: IConsoleHandler
    {
        public async ETTask Run(ModeContex contex, string content)
        {
            switch (content)
            {
                case ConsoleMode.ShowMemory:
                    contex.Parent.RemoveComponent<ModeContex>();
                    Log.Console($"GetCurrentMemory : {ProcessHelper.GetCurrentMemory()}");
                    Log.Console($"EntityInfo : {Root.Instance.ToString()}");
                    await ETTask.CompletedTask;
                    break;
            }

            await ETTask.CompletedTask;
        }
    }
}