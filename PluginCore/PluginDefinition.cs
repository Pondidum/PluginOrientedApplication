using System;
using System.Reflection;

namespace PluginCore
{
	public class PluginDefinition
	{
		public string Name { get; protected set; }
		public string Description { get; protected set; }

		public Func<Plugin> Run { get; protected set; }
		public Assembly Provider { get; internal set; }

		protected void Requires(params string[] pluginNames)
		{
			
		}
	}
}
