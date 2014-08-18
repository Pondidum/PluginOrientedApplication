using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PluginCore
{
	public class PluginLocator
	{
		public IEnumerable<PluginDefinition> Plugins { get { return _plugins; }}

		private readonly List<PluginDefinition> _plugins;
		private readonly string _directoryPath;

		public PluginLocator(string directoryPath)
		{
			_directoryPath = directoryPath;
			_plugins = new List<PluginDefinition>();
		}

		public void Scan()
		{
			var files = Directory.EnumerateFiles(_directoryPath, "*Plugin.dll");

			var definitionType = typeof (PluginDefinition);

			var definitions = files
				.Select(Assembly.LoadFile)
				.SelectMany(assembly =>
				{
					var instances = assembly
						.GetTypes()
						.Where(definitionType.IsAssignableFrom)
						.Select(t => t.GetConstructor(Type.EmptyTypes))
						.Where(c => c != null)
						.Select(c => c.Invoke(null))
						.Cast<PluginDefinition>()
						.ToList();

					instances.ForEach(definition => definition.Provider = assembly);

					return instances;
				});

			_plugins.Clear();
			_plugins.AddRange(definitions);
		}
	}
}
