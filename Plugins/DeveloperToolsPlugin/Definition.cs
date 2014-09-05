using System.Linq;
using PluginCore;

namespace DeveloperToolsPlugin
{
	public class Definition : PluginDefinition
	{
		public Definition()
		{
			Name = "Developer Tools";
			Description = "Developer utilities to inspect the running application";

			Requires = Enumerable.Empty<string>(); //new[] { "MenuBuilder", "PermissionAuthority" };
			Run = () => new DeveloperPlugin();
		}
	}
}
