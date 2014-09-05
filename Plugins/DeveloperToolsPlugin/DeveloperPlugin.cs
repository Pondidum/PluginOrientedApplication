using System;
using PluginCore;
using PluginCore.Messages;

namespace DeveloperToolsPlugin
{
	public class DeveloperPlugin : Plugin
	{
		public DeveloperPlugin()
		{
			Publish(new DisplayDialogMessage(
				() => new OptionsView(),
				result => Console.WriteLine(result)
			));
		}
	}
}
