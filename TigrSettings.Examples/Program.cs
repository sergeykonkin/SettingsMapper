using System;
using TigrSettings;
using TigrSettings.Dynamic;
using TigrSettings.Providers.ConfigurationManager;

namespace Examples
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var appSettingsProvider = new AppSettingsProvider();

			var pocoBuilder = new PocoSettingsBuilder<AppSettings>(appSettingsProvider);
			AppSettings appSettings = pocoBuilder.Create();

			var staticBuilder = new StaticSettingsBuilder(appSettingsProvider);
			staticBuilder.MapTo(typeof(StaticAppSettings));

			var dynamicBuilder = new DynamicSettingsBuilder<IAppSettings>(appSettingsProvider);
			IAppSettings appSettings2 = dynamicBuilder.Create();
		}
	}

	public class AppSettings
	{
		public int Int { get; set; }
		public string String { get; set; }
		public EnumSetting Enum { get; set; }
		public DateTime DateTime { get; set; }
		public TimeSpan[] ArrayOfTimeSpans { get; set; }
		public bool? NullableBoolean { get; set; }
	}

	public static class StaticAppSettings
	{
		public static int Int { get; set; }
		public static string String { get; set; }
		public static EnumSetting Enum { get; set; }
		public static DateTime DateTime { get; set; }
		public static TimeSpan[] ArrayOfTimeSpans { get; set; }
		public static bool? NullableBoolean { get; set; }
	}

	public interface IAppSettings
	{
		int Int { get; }
		string String { get; }
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
