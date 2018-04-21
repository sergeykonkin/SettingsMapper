using System;

namespace TigrSettings
{
	/// <summary>
	/// Provides functionality to bind raw settings to Poco objects.
	/// </summary>
	public class PocoSettingsBuilder<TSettings> : SettingsBuilderBase
		where TSettings : class, new()
	{
		/// <summary>
		/// Initializes a new instance of <see cref="PocoSettingsBuilder{TSettings}"/>.
		/// </summary>
		/// <param name="settingsProvider">Raw string settings provider.</param>
		/// <param name="converters">Set of additional converters.</param>
		public PocoSettingsBuilder(
			ISettingsProvider settingsProvider,
			params ISettingValueConverter[] converters)
			: base(settingsProvider, converters)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="PocoSettingsBuilder{TSettings}"/>.
		/// </summary>
		/// <param name="settingsProvider">Raw string settings provider.</param>
		/// <param name="formatProvider">Format provider for e.g. numbers and dates.</param>
		/// <param name="converters">Set of additional converters.</param>
		public PocoSettingsBuilder(
			ISettingsProvider settingsProvider,
			IFormatProvider formatProvider,
			params ISettingValueConverter[] converters)
			: base(settingsProvider, formatProvider, converters)
		{
		}

		/// <summary>
		/// Creates new instance of <see cref="TSettings"/> with properties filled with converted settings.
		/// </summary>
		/// <returns>Poco object instance.</returns>
		public TSettings Create()
		{
			var instance = new TSettings();
			base.FillProps(new PocoBinder(instance));
			return instance;
		}
	}
}
