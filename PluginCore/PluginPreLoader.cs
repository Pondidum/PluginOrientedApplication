using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PluginCore
{
	internal class PluginPreLoader
	{
		public IEnumerable<PluginDefinition> BuildDefinitions(IEnumerable<string> assemblies)
		{
			var definitionType = typeof (PluginDefinition);

			return assemblies
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
