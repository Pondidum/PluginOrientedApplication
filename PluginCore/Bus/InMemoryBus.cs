using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using PluginCore.Internal;

namespace PluginCore.Bus
{
	public class InMemoryBus : IBus
	{
		private static readonly Logger Logger = LogManager.GetLogger("InMemoryBus");

		private readonly IDictionary<Type, List<Handler>> _handlers;

		public InMemoryBus()
		{
			_handlers = new Dictionary<Type, List<Handler>>();
		}

		public void Publish<T>(T message)
		{
			var type = typeof(T);
			var handlers = _handlers.GetOrDefault(type, Enumerable.Empty<Handler>().ToList());

			Logger.Info("Bus: Publishing {0} to {1} handlers.",
				type.FullName,
				handlers.Count);

			foreach (var entry in handlers)
			{
				entry.Action.Invoke(message);
			}
		}

		public void Subscribe<T>(Action<T> handler)
		{
			if (handler == null) throw new ArgumentNullException("handler");

			Logger.Info("Bus: Subscribing to {0}.",
				typeof(T).FullName);

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
