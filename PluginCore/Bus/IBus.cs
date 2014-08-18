using System;

namespace PluginCore.Bus
{
	public interface IBus
	{
		void Publish<TMessage>(TMessage message);

		void Subscribe<TMessage>(Action<TMessage> handler);
	}
}
