using System;
using System.Reflection;
using TigrSettings;
using TigrSettings.Dynamic;
using TigrSettings.Providers.ConfigurationManager;
using TigrSettings.Providers.EnvironmentVariables;

namespace Examples
{
	public class Program
	{
		public static void Main(string[] args)
		{
			AppSettingsToPoco();

			EnvironmentVariablesToStaticClass();

			AppSettingsToDynamic();

			Console.ReadLine();
		}

		public static void AppSettingsToPoco()
		{
			ISettingsProvider settingsProvider = new AppSettingsProvider();

			var pocoBuilder = new PocoSettingsBuilder<AppSettings>(settingsProvider);
			AppSettings appSettings = pocoBuilder.Create();

			Console.WriteLine("POCO object:");
			PrintProps(appSettings);
			Console.WriteLine();
		}

		public static void EnvironmentVariablesToStaticClass()
		{
			Environment.SetEnvironmentVariable("INT", "5");
			Environment.SetEnvironmentVariable("String", "foobar");
			Environment.SetEnvironmentVariable("guid", "9d50f0fb-4127-4433-b062-7ecf211a2adb");
			Environment.SetEnvironmentVariable("enum", "Foo|Baz");
			Environment.SetEnvironmentVariable("DATE_TIME", "2018-04-21T12:34:56.789Z");
			Environment.SetEnvironmentVariable("array_of_time_spans", "00:00:05, 00:00:10, 00:00:15");
			Environment.SetEnvironmentVariable("NuLlaBle_BoOlean", "true");

			ISettingsProvider settingsProvider = new EnvironmentVariablesProvider();

			var staticBuilder = new StaticSettingsBuilder(settingsProvider);
			staticBuilder.MapTo(typeof(StaticAppSettings));

			Console.WriteLine("Static class:");
			PrintStaticProps(typeof(StaticAppSettings));
			Console.WriteLine();
		}

		public static void AppSettingsToDynamic()
		{
			ISettingsProvider settingsProvider = new AppSettingsProvider();

			var dynamicBuilder = new DynamicSettingsBuilder<IAppSettings>(settingsProvider);
			IAppSettings appSettings = dynamicBuilder.Create();

			Console.WriteLine("Dynamic object:");
			PrintProps(appSettings);
			Console.WriteLine();
		}

		private static void PrintProps(object obj)
		{
			foreach (var prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				Console.WriteLine(prop.Name + ": " + prop.GetValue(obj));
			}
		}

		private static void PrintStaticProps(Type staticType)
		{
			foreach (var prop in staticType.GetProperties(BindingFlags.Public | BindingFlags.Static))
			{
				Console.WriteLine(prop.Name + ": " + prop.GetValue(null));
			}
		}
	}

	public class AppSettings
	{
		public int Int { get; set; }
		public string String { get; set; }
		public Guid Guid { get; set; }
		public EnumSetting Enum { get; set; }
		public DateTime DateTime { get; set; }
		public TimeSpan[] ArrayOfTimeSpans { get; set; }
		public bool? NullableBoolean { get; set; }
	}

	public static class StaticAppSettings
	{
		public static int Int { get; set; }
		public static string String { get; set; }
		public static Guid Guid { get; set; }
		public static EnumSetting Enum { get; set; }
		public static DateTime DateTime { get; set; }
		public static TimeSpan[] ArrayOfTimeSpans { get; set; }
		public static bool? NullableBoolean { get; set; }
	}

	public interface IAppSettings
	{
		int Int { get; }
		string String { get; }
		Guid Guid { get; }
		EnumSetting Enum { get; }
		DateTime DateTime { get; }
		TimeSpan[] ArrayOfTimeSpans { get; }
		bool? NullableBoolean { get; }
	}

	[Flags]
	public enum EnumSetting
	{
		Foo = 1,
		Bar = 2,
		Baz = 4
	}
}
