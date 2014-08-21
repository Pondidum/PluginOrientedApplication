using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PluginCore
{
	internal class AssemblyParser
	{
		public IEnumerable<PluginDefinition> BuildDefinitions(IEnumerable<string> paths)
		{
			var definitionType = typeof (PluginDefinition);

			return paths
				.Select(path => Assembly.LoadFrom(path))
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
				})
				.ToList();
		}
	}
}
