using System;
using System.Globalization;

namespace TigrSettings.Converters
{
	/// <summary>
	/// Converts raw string setting values to <see cref="TimeSpan"/> type.
	/// </summary>
	public class TimeSpanConverter : SettingValueConverterBase<TimeSpan>
	{
		private readonly IFormatProvider _formatProvider;

		/// <summary>
		/// Initializes a new instance of <see cref="TimeSpanConverter"/>.
		/// </summary>
		public TimeSpanConverter()
			: this(CultureInfo.InvariantCulture)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="TimeSpanConverter"/>.
		/// </summary>
		/// <param name="formatProvider">Format provider.</param>
		public TimeSpanConverter(IFormatProvider formatProvider)
		{
			_formatProvider = formatProvider;
		}

		/// <inheritdoc />
		public override TimeSpan Convert(string value)
		{
			return TimeSpan.Parse(value, _formatProvider);
		}
	}
}
