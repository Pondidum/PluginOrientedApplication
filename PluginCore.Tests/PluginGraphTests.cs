using System.Linq;
using NSubstitute;
using PluginCore.Bus;
using PluginCore.Messages;
using PluginCore.Tests.TestUtils;
using Shouldly;
using Xunit;

namespace PluginCore.Tests
{
	public class PluginGraphTests
	{
		[Fact]
		public void A_plugin_with_a_dependency_loads_in_order()
		{
			var plugin = new TestPlugin("Main", "Dep1");
			var dependency = new TestPlugin("Dep1");

			var graph = new PluginGraph(Substitute.For<IBus>());
			var order = graph.Build(new[] { plugin, dependency });

			order.ShouldBe(new[] { dependency, plugin });
		}

		[Fact]
		public void A_plugin_with_a_missing_dependency_reports_to_bus_and_doesnt_load()
		{
			var plugin = new TestPlugin("Main", "Dep1");
			var bus = new LoggingBus();

			var graph = new PluginGraph(bus);
			var order = graph.Build(new[] { plugin });

			bus.MessagesPublished.First().ShouldBeOfType<PluginErrorMessage>();
			order.ShouldBeEmpty();
		}

		private class TestPlugin : PluginDefinition
		{
			public TestPlugin(string name, params string[] dependencies)
			{
				Name = name;
				Requires = dependencies;
			}
		}

	}
}
