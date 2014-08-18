using System;
using System.Collections.Generic;
using System.Linq;
using PluginCore.Internal.DirectedGraph;

namespace PluginCore.Internal
{
	internal class DependencyGraph<T> where T : class
	{
		readonly DirectedGraph.DirectedGraph _cycleDetector;
		readonly IDictionary<string, T> _items;
		readonly Func<T, string> _getName;
		readonly Func<T, IEnumerable<string>> _getDependencies;


		public DependencyGraph(Func<T, string> getName, Func<T, IEnumerable<string>> getDependencies)
		{
			_cycleDetector = new DirectedGraph.DirectedGraph();
			_items = new Dictionary<string, T>();
			_getName = getName;
			_getDependencies = getDependencies;
		}

		public void RegisterItem(T item)
		{
			var name = _getName(item);

			_items.Add(name, item);

			_cycleDetector.AddNode(new Node(name));
			foreach (var dep in _getDependencies(item))
			{
				//bottle X needs bottle Y
				_cycleDetector.Connect(_getName(item), dep);
			}
		}

		public bool HasCycles()
		{
			var cycles = _cycleDetector.FindCycles().ToList();
			return cycles.Any();
		}

		public IEnumerable<string> MissingDependencies()
		{
			var registeredNames = _items.Keys.ToList();
			var neededNames = _cycleDetector.Nodes.Select(n => n.Name).ToList();
			var missing = neededNames.Except(registeredNames);
			return missing;
		}

		public bool HasMissingDependencies()
		{
			var missing = MissingDependencies();
			return missing.Any();
		}

		public IEnumerable<T> Ordered()
		{
			return GetLoadOrder().Select(Convert).Where(x => x != null).ToList();
		}

		private T Convert(string name)
		{
			if (!_items.ContainsKey(name)) return null;

			try
			{
				return _items[name];
			}
			catch (KeyNotFoundException ex)
			{
				throw new KeyNotFoundException(string.Format("Couldn't find key '{0}' for type '{1}'", name, typeof(T)), ex);
			}
		}

		public IEnumerable<string> GetLoadOrder()
		{
			if (HasCycles())
				throw new InvalidOperationException("This graph has dependency cycles and cannot be ordered!");

			foreach (var node in _cycleDetector.Order())
			{
				yield return node.Name;
			}

			yield break;
		}
	}
}
