using System;
using System.Globalization;

namespace TigrSettings.Converters
{
	/// <summary>
	/// Converts raw string setting values to <see cref="DateTime"/> type.
	/// </summary>
	public class DateTimeConverter : SettingValueConverterBase<DateTime>
	{
		private readonly IFormatProvider _formatProvider;

		/// <summary>
		/// Initializes a new instance of <see cref="DateTimeConverter"/>.
		/// </summary>
		public DateTimeConverter()
			: this(CultureInfo.InvariantCulture)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="DateTimeConverter"/>.
		/// </summary>
		/// <param name="formatProvider">Format provider for dates.</param>
		public DateTimeConverter(IFormatProvider formatProvider)
		{
			_formatProvider = formatProvider;
		}

		/// <inheritdoc />
		public override DateTime Convert(string value)
		{
			return DateTime.Parse(value, _formatProvider);
		}
	}
}
