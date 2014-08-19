using System;
using System.Linq;
using PluginCore;

namespace BroadcastPlugin
{
	public class Definition : PluginDefinition
	{
		public Definition()
		{
			Name = "Message Broadcasting Plugin";
			Description = "Broadcasts a message every 10s.";

			Requires = Enumerable.Empty<string>();
			Run = () => new TimerBroadcast();
		}
	}
}
