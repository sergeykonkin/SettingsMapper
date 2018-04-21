using System;

namespace TigrSettings.Converters
{
	/// <summary>
	/// Converts raw string setting values to <see cref="TimeSpan"/> type.
	/// </summary>
	public class TimeSpanConverter : SettingValueConverterBase<TimeSpan>
	{
		/// <inheritdoc />
		public override TimeSpan Convert(string value)
		{
			return TimeSpan.Parse(value);
		}
	}
}
