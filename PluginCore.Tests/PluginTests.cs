using PluginCore.Bus;
using PluginCore.Tests.TestUtils;
using Shouldly;
using Xunit;

namespace PluginCore.Tests
{
	public class PluginTests
	{
		[Fact]
		public void When_the_bus_is_injected_subscribers_are_registered()
		{
			var plugin = new TestPlugin();
			var bus = new LoggingBus(new MappingBus());

			plugin.SetupBus(bus);

			bus.Subscribers.ShouldContainKey(typeof (TestMessage));
		}

		[Fact]
		public void When_the_bus_is_injected_publishes_happen_after_subscribes()
		{
			var plugin = new TestPlugin();
			var bus = new LoggingBus(new MappingBus());

			plugin.SetupBus(bus);

			plugin.Received.ShouldBe(1);
		}


		private class TestPlugin : Plugin
		{
			public int Received { get; set; }

			public TestPlugin()
			{
				SubscribeTo<TestMessage>(OnTestMessage);
				Publish(new TestMessage());
			}

			private void OnTestMessage(TestMessage message)
			{
				Received++;
			}
		}

		private class TestMessage { }
	}
}
