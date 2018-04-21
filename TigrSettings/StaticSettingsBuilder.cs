using System;

namespace TigrSettings
{
	/// <summary>
	/// Provides functionality to bind raw settings to Static classes.
	/// </summary>
	public class StaticSettingsBuilder : SettingsBuilderBase
	{
		/// <summary>
		/// Initializes a new instance of <see cref="StaticSettingsBuilder"/>.
		/// </summary>
		/// <param name="settingsProvider">Raw string settings provider.</param>
		/// <param name="converters">Set of additional converters.</param>
		public StaticSettingsBuilder(
			ISettingsProvider settingsProvider,
			params ISettingValueConverter[] converters)
			: base(settingsProvider, converters)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="StaticSettingsBuilder"/>.
		/// </summary>
		/// <param name="settingsProvider">Raw string settings provider.</param>
		/// <param name="formatProvider">Format provider for e.g. numbers and dates.</param>
		/// <param name="converters">Set of additional converters.</param>
		public StaticSettingsBuilder(
			ISettingsProvider settingsProvider,
			IFormatProvider formatProvider,
			params ISettingValueConverter[] converters)
			: base(settingsProvider, formatProvider, converters)
		{
		}

		/// <summary>
		/// Maps converted settings to Static class properties.
		/// </summary>
		/// <param name="staticType">Static class type.</param>
		public void MapTo(Type staticType)
		{
			base.FillProps(new StaticBinder(staticType));
		}
	}
}
