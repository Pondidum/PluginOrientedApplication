using System;
using System.Collections.Generic;
using System.Linq;
using PluginCore.Bus;

namespace PluginCore
{
	public class Plugin
	{
		private IBus _bus;
		private readonly Queue<Action> _waitingSubscribes;
		private readonly Queue<Action> _waitingPublishes;

		public Plugin()
		{
			_waitingSubscribes = new Queue<Action>();
			_waitingPublishes = new Queue<Action>();
		}

		protected void SubscribeTo<TMessage>(Action<TMessage> handler)
		{
			if (_bus != null)
			{
				_bus.Subscribe(handler);
			}
			else
			{
				_waitingSubscribes.Enqueue(() => _bus.Subscribe(handler));
			}
		}

		protected void Publish<TMessage>(TMessage message)
		{
			if (_bus != null)
			{
				_bus.Publish(message);
			}
			else
			{
				_waitingPublishes.Enqueue(() => _bus.Publish(message));
			}
		}

		internal void SetupBus(IBus bus)
		{
			_bus = bus;

			while (_waitingSubscribes.Any())
			{
				_waitingSubscribes.Dequeue().Invoke();
			}

			while (_waitingPublishes.Any())
			{
				_waitingSubscribes.Dequeue().Invoke();
			}
		}
	}
}
