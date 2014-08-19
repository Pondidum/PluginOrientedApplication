using System;
using System.Windows.Forms;
using NLog;
using NLog.Config;
using NLog.Targets;
using PluginCore;
using PluginCore.Bus;
using PluginCore.Messages;

namespace ApplicationContainer
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var config = new LoggingConfiguration();
			var console = new ColoredConsoleTarget();

			config.AddTarget("console", console );
			config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, console));

			LogManager.Configuration = config;

			var bus = new InMemoryBus();
			bus.Subscribe<PluginErrorMessage>(m => Console.WriteLine(m.Message));

			var plugins = new PluginLoader(bus);
			plugins.Add(@"plugins\DeveloperToolsPlugin.dll");
			plugins.Add(@"plugins\BroadcastPlugin.dll");
			plugins.Add(@"plugins\ReceiverPlugin.dll");

			plugins.Load();

			Console.WriteLine("Done.");
			Console.ReadKey();
		}
	}


}
