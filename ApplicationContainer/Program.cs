using System;
using System.Windows.Forms;
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
			//Application.Run(new Form1());

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
