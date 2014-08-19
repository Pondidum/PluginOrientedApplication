using System.Linq;
using PluginCore;

namespace ReceiverPlugin
{
	public class Definition : PluginDefinition
	{
		public Definition()
		{
			Name = "Message Receiving Plugin";
			Description = "Receives and prints TestMessages.";

			Requires = Enumerable.Empty<string>();
			Run = () => new ReceivingPlugin();
		}
	}
}