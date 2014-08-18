using System;
using System.Collections.Generic;
using System.Reflection;

namespace PluginCore
{
	public class PluginDefinition
	{
		public string Name { get; protected set; }
		public string Description { get; protected set; }

		public Func<Plugin> Run { get; protected set; }
		public Assembly Provider { get; internal set; }

		public IEnumerable<string> Requires { get; protected set; }
	}
}
