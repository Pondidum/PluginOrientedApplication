using PluginCore.Bus;
using Should;
using Xunit;

namespace PluginCore.Tests.Bus
{
	public class MappingBusTests
	{
		private IBus _bus;

		private string _firstMessage;
		private string _secondMessage;

		public MappingBusTests()
		{
			_bus = new MappingBus();
		}

		[Fact]
		public void A_message_is_published_to_all_handlers_with_matching_type_names()
		{
			_bus.Subscribe<Original.TestMessage>(m => _firstMessage = m.Message);
			_bus.Subscribe<Matching.TestMessage>(m => _secondMessage = m.Message);

			_bus.Publish(new Original.TestMessage { Message = "Incoming" });

			_firstMessage.ShouldEqual("Incoming");
			_secondMessage.ShouldEqual("Incoming");
		}

		[Fact]
		public void A_message_is_publishe_to_all_partial_matching_handlers()
		{
			_bus.Subscribe<Original.TestMessage>(m => _firstMessage = m.Message);
			_bus.Subscribe<Partial.TestMessage>(m => _secondMessage = m.Message);

			_bus.Publish(new Original.TestMessage { Message = "Incoming" });

			_firstMessage.ShouldEqual("Incoming");
			_secondMessage.ShouldEqual("Incoming");
		}
	}


	namespace Original
	{
		public class TestMessage
		{
			public int Count { get; set; }
			public string Message { get; set; }
		}
	}

	namespace Matching
	{
		public class TestMessage
		{
			public int Count { get; set; }
			public string Message { get; set; }
		}
	}

	namespace Partial
	{
		public class TestMessage
		{
			public string Message { get; set; }
		}
	}
}
