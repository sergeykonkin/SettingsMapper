using System;
using System.Dynamic;
using ImpromptuInterface;

namespace TigrSettings.Dynamic
{
	/// <summary>
	/// Provides functionality to bind raw settings to Dynamic objects that implements specified interface.
	/// </summary>
	public class DynamicSettingsBuilder<TSettings> : SettingsBuilderBase
		where TSettings : class
	{
		/// <summary>
		/// Initializes a new instance of <see cref="DynamicSettingsBuilder{TSettings}"/>.
		/// </summary>
		/// <param name="settingsProvider">Raw string settings provider.</param>
		/// <param name="converters">Set of additional converters.</param>
		public DynamicSettingsBuilder(
			ISettingsProvider settingsProvider,
			params ISettingValueConverter[] converters)
			: base(settingsProvider, converters)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="DynamicSettingsBuilder{TSettings}"/>.
		/// </summary>
		/// <param name="settingsProvider">Raw string settings provider.</param>
		/// <param name="formatProvider">Format provider for e.g. numbers and dates.</param>
		/// <param name="converters">Set of additional converters.</param>
		public DynamicSettingsBuilder(
			ISettingsProvider settingsProvider,
			IFormatProvider formatProvider,
			params ISettingValueConverter[] converters)
			: base(settingsProvider, formatProvider, converters)
		{
		}

		/// <summary>
		/// Creates new instance of dynamic object that implements <see cref="TSettings"/> with properties filled with converted settings.
		/// </summary>
		/// <returns>Dynamic object instance.</returns>
		public TSettings Create()
		{
			var settings = new ExpandoObject();
			base.FillProps(new DynamicBinder(settings, typeof(TSettings)));
			return Impromptu.ActLike<TSettings>(settings);
		}
	}
}
