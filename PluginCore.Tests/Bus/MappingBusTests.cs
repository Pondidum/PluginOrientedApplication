using Xunit;

namespace PluginCore.Tests.Bus
{
	public class MappingBusTests
	{
		[Fact]
		public void A_message_is_mapped_to_all_messages_of_the_same_name()
		{
			var bus = new mappingbus
		}
	}


	namespace First
	{
		public class TestMessage
		{
			public string Message { get; set; }
		}
	}

	namespace Second
	{
		public class TestMessage
		{
			public string Message { get; set; }
		}
	}
}
