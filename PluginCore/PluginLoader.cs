using System;
using System.Collections.Generic;
using System.Linq;
using PluginCore.Bus;

namespace PluginCore
{
	public class PluginLoader
	{
		private readonly IBus _bus;
		private readonly HashSet<string> _pluginPaths;

		private readonly List<Plugin> _runningPlugins;

		public PluginLoader(IBus bus)
		{
			_bus = bus;
			_pluginPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			_runningPlugins = new List<Plugin>();
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
			
			sorted.ToList().ForEach(d =>
			{
				var plugin = d.Run();
				plugin.SetupBus(_bus);

				_runningPlugins.Add(plugin);
			});
		}
	}
}
