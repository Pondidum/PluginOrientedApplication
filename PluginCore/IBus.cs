using System;

namespace PluginCore
{
	public interface IBus
	{
		void Publish<TMessage>(TMessage message);

		void Subscribe<TMessage>(Action<TMessage> handler);
	}
}
