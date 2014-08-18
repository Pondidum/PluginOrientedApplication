using System;
using System.Collections.Generic;
using PluginCore;
using PluginCore.Bus;

namespace PluginCore.Internal.Bus
{
	public class InMemoryBus : IBus
	{
		private readonly IDictionary<Type, List<Handler>> _handlers;

		public InMemoryBus()
		{
			_handlers = new Dictionary<Type, List<Handler>>();
		}

		public void Publish<T>(T message)
		{
			var type = typeof(T);
			var handlers = _handlers.GetOrDefault(type);

			if (handlers == null)
			{
				return;
			}

			foreach (var entry in handlers)
			{
				entry.Action.Invoke(message);
			}
		}

		public void Subscribe<T>(Action<T> handler)
		{
			if (handler == null) throw new ArgumentNullException("handler");

			Wire(typeof(T), new Handler(handler.GetHashCode(), message => handler.Invoke((T)message)));
		}

		private void Wire(Type type, Handler handler)
		{
			var handlers = _handlers.GetOrDefault(type);

			if (handlers == null)
			{
				handlers = new List<Handler>();
				_handlers[type] = handlers;
			}

			handlers.Add(handler);
		}

		public void UnWire<T>(Action<T> handler)
		{
			if (handler == null) throw new ArgumentNullException("handler");

			UnWire(typeof(T), handler.GetHashCode());
		}

		private void UnWire(Type type, int hash)
		{
			var handlers = _handlers.GetOrDefault(type);

			if (handlers == null)
			{
				return;
			}

			handlers.RemoveAll(h => h.Hash == hash);
		}
	}
}
