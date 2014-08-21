using System;
using System.Net.Mail;

namespace PluginCore.Tests.TestUtils
{
	public class Catch
	{
		public static Exception Exception(Action action)
		{
			try
			{
				action();
			}
			catch (Exception ex)
			{
				return ex;
			}

			return null;
		}

		public static T Exception<T>(Action action) where T : Exception
		{
			try
			{
				action();
			}
			catch (T ex)
			{
				return ex;
			}

			return null;
		}
	}
}