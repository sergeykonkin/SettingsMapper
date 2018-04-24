using System;
using System.Configuration;
using TigrSettings;

namespace Examples
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CopyAppSettingsToEnv();
			var settingsProvider
				//	= new AppSettingsProvider();
				= new EnvironmentVariablesProvider();

			var pocoBuilder = new PocoSettingsBuilder<AppSettings>(settingsProvider);
			var poco = pocoBuilder.Create();
			var pp3 = poco.Inner.Deeper.InnerProp3;

			var staticBuilder = new StaticSettingsBuilder(settingsProvider);
			staticBuilder.MapTo(typeof(StaticAppSettings));
			var sp3 = StaticAppSettings.Inner.Deeper.InnerProp3;
			var snp = StaticAppSettings.NestedStaticClass.NestedStaticProp;

			var dynamicBuilder = new DynamicSettingsBuilder<IAppSettings>(settingsProvider);
			var dynamic = dynamicBuilder.Create();
			var dp3 = dynamic.Inner.Deeper.InnerProp3;
		}

		private static void CopyAppSettingsToEnv()
		{
			foreach (string key in ConfigurationManager.AppSettings.AllKeys)
			{
				Environment.SetEnvironmentVariable(key, ConfigurationManager.AppSettings[key]);
			}
		}
	}
}
