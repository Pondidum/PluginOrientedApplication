using System.Collections.Generic;
using System.Linq;
using PluginCore.Internal;
using PluginCore.Messages;

namespace PluginCore
{
	public class PluginGraph
	{
		private readonly IBus _bus;
		private readonly DependencyGraph<PluginDefinition> _graph;

		public PluginGraph(IBus bus)
		{
			_bus = bus;
			_graph = new DependencyGraph<PluginDefinition>(p => p.Name, p => p.Requires);
		}

		public IEnumerable<PluginDefinition> Build(IEnumerable<PluginDefinition> plugins)
		{
			plugins.ToList().ForEach(plugin => _graph.RegisterItem(plugin));

			if (_graph.HasMissingDependencies())
			{
				_bus.Publish(new PluginErrorMessage
				{
					Message = "There are missing dependencies.",
					Missing = _graph.MissingDependencies().ToList()
				});
			}

			return _graph.Ordered();
		}
	}
}
