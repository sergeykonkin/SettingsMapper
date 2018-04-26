using System;
using System.Globalization;
using System.Linq;

namespace TigrSettings.Converters
{
	/// <summary>
	/// Converts raw string setting values to scalar primitive types.
	/// </summary>
	public class ScalarConverter : ISettingConverter
	{
		private static readonly Type[] Exclude =
		{
			typeof(IntPtr),
			typeof(UIntPtr),
			typeof(string),
			typeof(object)
		};

		private readonly IFormatProvider _formatProvider;

		/// <summary>
		/// Initializes a new instance of <see cref="ScalarConverter"/>.
		/// </summary>
		public ScalarConverter()
			: this(CultureInfo.InvariantCulture)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="ScalarConverter"/>.
		/// </summary>
		/// <param name="formatProvider">Format provider for numbers.</param>
		public ScalarConverter(IFormatProvider formatProvider)
		{
			_formatProvider = formatProvider;
		}

		/// <inheritdoc />
		public virtual bool CanConvert(Type type)
		{
			return Exclude.All(t => t != type) && type.IsPrimitive;
		}

		/// <inheritdoc />
		public virtual object Convert(string value, Type type)
		{
			return System.Convert.ChangeType(value, type, _formatProvider);
		}
	}
}
