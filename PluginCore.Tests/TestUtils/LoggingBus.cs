using System;
using System.Collections.Generic;
using NSubstitute;
using PluginCore.Bus;

namespace PluginCore.Tests.TestUtils
{
	public class LoggingBus : IBus
	{
		public Dictionary<Type, List<Action<object>>> Subscribers { get; private set; }
		public List<object> MessagesPublished { get; private set; }

		private readonly IBus _other;

		public LoggingBus() : this(Substitute.For<IBus>())
		{
		}

		public LoggingBus(IBus other)
		{
			_other = other;
			Subscribers = new Dictionary<Type, List<Action<object>>>();
			MessagesPublished = new List<object>();
		}
		
		public void Subscribe<TMessage>(Action<TMessage> handler)
		{
			var messageType = typeof(TMessage);

			if (Subscribers.ContainsKey(messageType) == false)
			{
				Subscribers[messageType] = new List<Action<object>>();
			}

			Subscribers[messageType].Add(message => handler((TMessage)message));
			_other.Subscribe<TMessage>(handler);
		}

		public void Publish<TMessage>(TMessage message)
		{
			MessagesPublished.Add(message);
			_other.Publish<TMessage>(message);
		}
	}
}
