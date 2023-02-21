namespace ET.Server
{
    public class HttpHandlerAttribute: BaseAttribute
    {
        public SceneType SceneType { get; }

        public string Path { get; }

        public bool NeedAuth { get; }
        public HttpHandlerAttribute(SceneType sceneType, string path, bool needAuth = true)
        {
            this.SceneType = sceneType;
            this.Path = path;
            this.NeedAuth = needAuth;
        }
    }
}