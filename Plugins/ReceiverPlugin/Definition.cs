using System.Linq;
using PluginCore;

namespace ReceiverPlugin
{
	public class Definition : PluginDefinition
	{
		public Definition()
		{
			Name = "Message Broadcasting Plugin";
			Description = "Broadcasts a message every 10s.";

			Requires = Enumerable.Empty<string>();
			Run = () => new ReceivingPlugin();
		}
	}
}