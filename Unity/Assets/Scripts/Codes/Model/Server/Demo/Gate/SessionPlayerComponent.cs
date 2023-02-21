namespace ET.Server
{
	[ComponentOf(typeof(Session))]
	public class SessionPlayerComponent : Entity, IAwake, IDestroy
	{
		public long PlayerId { get; set; }

		public int ServerId { get; set; }

		public bool IsKnockOut { get; set; } = false;
	}
}