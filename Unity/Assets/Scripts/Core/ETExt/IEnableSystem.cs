using System;

namespace ET
{
	public interface IEnable
	{
	}
	
	public interface IEnableSystem: ISystemType
	{
		void Run(object o);
	}

	[ObjectSystem]
	public abstract class EnableSystem<T> : IEnableSystem where T: IEnable
	{
		public void Run(object o)
		{
			this.Enable((T)o);
		}
		
		public Type Type()
		{
			return typeof(T);
		}
		
		public Type SystemType()
		{
			return typeof(IEnableSystem);
		}
		InstanceQueueIndex ISystemType.GetInstanceQueueIndex()
		{
			return InstanceQueueIndex.None;
		}

		protected abstract void Enable(T self);
	}
}
