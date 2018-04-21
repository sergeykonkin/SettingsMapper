using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TigrSettings.Converters;

namespace TigrSettings
{
	/// <summary>
	/// Provides base functionality to bind raw settings to target types.
	/// </summary>
	public abstract class SettingsBuilderBase
	{
		private readonly ISettingValueConverter[] DefaultConverters =
		{

		};

		private readonly IFormatProvider _formatProvider;
		private readonly ISettingsProvider _settingsProvider;
		private readonly List<ISettingValueConverter> _converters;

		/// <summary>
		/// Initializes a new instance of <see cref="SettingsBuilderBase"/>.
		/// </summary>
		/// <param name="settingsProvider">Raw string settings provider.</param>
		/// <param name="converters">Set of additional converters.</param>
		protected SettingsBuilderBase(
			ISettingsProvider settingsProvider,
			params ISettingValueConverter[] converters)
			: this(settingsProvider, CultureInfo.InvariantCulture, converters)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="SettingsBuilderBase"/>.
		/// </summary>
		/// <param name="settingsProvider">Raw string settings provider.</param>
		/// <param name="formatProvider">Format provider for e.g. numbers and dates.</param>
		/// <param name="converters">Set of additional converters.</param>
		protected SettingsBuilderBase(
			ISettingsProvider settingsProvider,
			IFormatProvider formatProvider,
			params ISettingValueConverter[] converters)
		{
			_settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
			_formatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));
			if (converters == null) throw new ArgumentNullException(nameof(converters));

			_converters = new List<ISettingValueConverter>();
			_converters.AddRange(converters);

			_converters.Add(new ScalarConverter(_formatProvider));
			_converters.Add(new DateTimeConverter(_formatProvider));
			_converters.Add(new TimeSpanConverter());
			_converters.Add(new EnumConverter());
			_converters.Add(new NullableConverter(_converters.ToArray()));
			_converters.Add(new ArrayConverter(_converters.ToArray()));
		}

		/// <summary>
		/// Identifies whether this settings builder can convert string to specified type.
		/// </summary>
		/// <param name="type">Type convert to.</param>
		/// <returns>true - if this settings builder can convert string to type; false - otherwise.</returns>
		public bool CanConvert(Type type)
		{
			return _converters.Any(c => c.CanConvert(type));
		}

		/// <summary>
		/// Fills target's properties with converted settings' values.
		/// </summary>
		/// <param name="binder">Property binder to use.</param>
		protected void FillProps(IBinder binder)
		{
			var props = binder.GetProps();
			foreach (var prop in props)
			{
				var stringValue = _settingsProvider.Get(prop.Name);
				if (string.IsNullOrWhiteSpace(stringValue) && prop.PropertyType.IsNonNullableValueType())
				{
					throw new InvalidOperationException("Null value encountered for value type setting.");
				}

				if (prop.PropertyType == typeof(string))
				{
					binder.Bind(prop.Name, stringValue);
					continue;
				}

				if (!TryConvert(stringValue, prop.PropertyType, out var convertedValue))
				{
					throw new NotSupportedException($"No setting value converter found for '{prop.PropertyType.Name}' type.");
				}

				binder.Bind(prop.Name, convertedValue);
			}
		}

		private bool TryConvert(string stringValue, Type type, out object convertedValue)
		{
			var converter = _converters.FirstOrDefault(c => c.CanConvert(type));

			if (converter == null)
			{
				convertedValue = null;
				return false;
			}

			convertedValue = converter.Convert(stringValue, type);
			return true;
		}
	}
}
