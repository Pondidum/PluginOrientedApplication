using System.Collections.Generic;

namespace PluginCore.Messages
{
	public class PluginErrorMessage
	{
		public string Message { get; set; }
		public List<string> Missing { get; set; }
	}
}
