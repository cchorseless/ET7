
namespace ET
{
    public partial class EventSystem
    {
        public void EnableSystem(Entity component)
        {
            List<object> iEnableSystems = this.typeSystems.GetSystems(component.GetType(), typeof(IEnableSystem));
            if (iEnableSystems == null)
            {
                return;
            }

            foreach (IEnableSystem enableSystem in iEnableSystems)
            {
                if (enableSystem == null)
                {
                    continue;
                }

                try
                {
                    enableSystem.Run(component);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}