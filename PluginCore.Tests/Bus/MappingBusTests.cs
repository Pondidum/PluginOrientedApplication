﻿using PluginCore.Bus;
using PluginCore.Tests.TestUtils;
using Shouldly;
using Xunit;

namespace PluginCore.Tests.Bus
{
	public class MappingBusTests
	{
		private IBus _bus;

		public MappingBusTests()
		{
			_bus = new MappingBus();
		}

		[Fact]
		public void Publishing_a_message_with_no_subscribers_doesnt_error()
		{
			var ex = Catch.Exception(() => _bus.Publish(new Original.TestMessage { Message = "Incoming" }));

			ex.ShouldBe(null);
		}

		[Fact]
		public void A_message_is_published_to_all_handlers_with_matching_type_names()
		{
			_bus.Subscribe<Original.TestMessage>(m => m.Message.ShouldBe("Incoming"));
			_bus.Subscribe<Matching.TestMessage>(m => m.Message.ShouldBe("Incoming"));

			_bus.Publish(new Original.TestMessage { Message = "Incoming" });
		}

		[Fact]
		public void A_message_is_publishe_to_all_partial_matching_handlers()
		{
			_bus.Subscribe<Original.TestMessage>(m => m.Message.ShouldBe("Incoming"));
			_bus.Subscribe<Partial.TestMessage>(m => m.Message.ShouldBe("Incoming"));

			_bus.Publish(new Original.TestMessage { Message = "Incoming" });
		}

		[Fact]
		public void A_message_is_only_mapped_to_a_different_type_if_needed()
		{
			var testMessage = new Original.TestMessage { Message = "Incoming" };

			_bus.Subscribe<Original.TestMessage>(m => m.ShouldBe(testMessage));

			_bus.Publish(testMessage);
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
