using System;
using System.Collections.Generic;
using System.Linq;

namespace PluginCore
{
	public class PluginLoader
	{
		private readonly IBus _bus;
		private readonly HashSet<string> _pluginPaths;

		public PluginLoader(IBus bus)
		{
			_bus = bus;
			_pluginPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		}

		public void Add(string pluginPath)
		{
			if (_pluginPaths.Contains(pluginPath))
			{
				return;
			}

			_pluginPaths.Add(pluginPath);
		}

		public void Load()
		{
			var definitions = new PluginPreLoader().BuildDefinitions(_pluginPaths);
			var sorted = new PluginGraph(_bus).Build(definitions);
			
			sorted.ToList().ForEach(d => d.Run());
		}
	}
}
