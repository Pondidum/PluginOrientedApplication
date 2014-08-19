using System;
using PluginCore;

namespace ReceiverPlugin
{
	public class ReceivingPlugin : Plugin
	{
		public ReceivingPlugin()
		{
			SubscribeTo<TestMessage>(OnTestMessage);
		}

		private void OnTestMessage(TestMessage message)
		{
			Console.WriteLine("Message Received {0}: {1}", message.Count, message.Message);
		}
	}
}
