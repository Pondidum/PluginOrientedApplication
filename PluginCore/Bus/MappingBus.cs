using System;
using System.Collections.Generic;
using AutoMapper;
using PluginCore.Internal;

namespace PluginCore.Bus
{
	public class MappingBus : IBus
	{
		private readonly Dictionary<string, List<Mapping>> _mappings;

		public MappingBus()
		{
			_mappings = new Dictionary<string, List<Mapping>>(StringComparer.OrdinalIgnoreCase);
		}

		public void Subscribe<TMessage>(Action<TMessage> handler)
		{
			var messageType = typeof(TMessage);
			var name = messageType.Name;

			var mappingSet = _mappings.GetOrDefault(name);

			if (mappingSet == null)
			{
				_mappings[name] = mappingSet = new List<Mapping>();
			}

			mappingSet.Add(new Mapping(messageType, x => handler((TMessage)x)));
		}

		public void Publish<TMessage>(TMessage message)
		{
			var messageType = typeof(TMessage);
			var name = messageType.Name;

			var mappingSet = _mappings.GetOrDefault(name);

			if (mappingSet == null)
			{
				return;
			}

			mappingSet.ForEach(mapping =>
			{
				var target = messageType == mapping.Type 
					? message
					: Mapper.DynamicMap(message, messageType, mapping.Type);

				mapping.Trigger(target);
			});
		}

		private struct Mapping
		{
			public readonly Type Type;
			public readonly Action<object> Trigger;

			public Mapping(Type type, Action<object> trigger)
			{
				Type = type;
				Trigger = trigger;
			}
		}
	}
}
