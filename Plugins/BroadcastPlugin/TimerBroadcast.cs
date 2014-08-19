using System.Threading;
using PluginCore;

namespace BroadcastPlugin
{
	public class TimerBroadcast : Plugin
	{
		private Timer _timer;

		public TimerBroadcast()
		{
			var count = 0;

			_timer = new Timer(
				t => Publish(new TestMessage { Count = count++, Message = "Test Message Sent."}),
				null,
				0,
				10 * 1000);
		}
	}
}
