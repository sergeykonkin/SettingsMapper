using System;
using System.Collections.Generic;
using System.Linq;
using TigrSettings.Converters;

namespace TigrSettings
{
	/// <summary>
	/// Provides base functionality to bind raw settings to target types.
	/// </summary>
	public abstract class SettingsBuilderBase
	{
		private readonly ISettingsProvider _settingsProvider;
		private readonly List<ISettingConverter> _converters;

		/// <summary>
		/// Gets binder for settings-to-type conversion.
		/// </summary>
		internal abstract IBinder Binder { get; }

		/// <summary>
		/// Initializes a new instance of <see cref="SettingsBuilderBase"/>.
		/// </summary>
		/// <param name="settingsProvider">Raw string settings provider.</param>
		/// <param name="formatProvider">Format provider for numbers and/or dates.</param>
		/// <param name="converters">Set of additional converters.</param>
		internal SettingsBuilderBase(
			ISettingsProvider settingsProvider,
			IFormatProvider formatProvider,
			params ISettingConverter[] converters)
		{
			_settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
			if (formatProvider == null) throw new ArgumentNullException(nameof(formatProvider));
			if (converters == null) throw new ArgumentNullException(nameof(converters));

			_converters = new List<ISettingConverter>();
			_converters.AddRange(converters);

			_converters.Add(new ScalarConverter(formatProvider));
			_converters.Add(new GuidConverter());
			_converters.Add(new DateTimeConverter(formatProvider));
			_converters.Add(new TimeSpanConverter());
			_converters.Add(new EnumConverter());
			_converters.Add(new ByteArrayConverter());
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
		/// Builds target object with properties filled with converted settings' values.
		/// </summary>
		/// <param name="targetType">Target's type.</param>
		/// <param name="prefix">Setting name prefix.</param>
		internal virtual object Build(Type targetType, string prefix = null)
		{
			var target = Binder.CreateTarget(targetType);
			var props = Binder.GetProps(targetType).ToList();

			foreach (var prop in props)
			{
				string name = prop.Name;
				Type type = prop.Type;

				if (type != typeof(string) && !CanConvert(type))
				{
					try
					{
						string newPrefix = (prefix ?? "") + (prop.CustomPrefix ?? name)+ ".";
						object inner = Build(type, newPrefix);
						Binder.Bind(target, targetType, name, inner);
						continue;
					}
					catch (Exception ex)
					{
						throw new InvalidOperationException(
							$"Cannot map settings to {type.Name} type. Ensure that this type is {TypeDesc}. Alternatively, you can provide a custom converter. See inner exception for details.",
							ex);
					}
				}

				string settingName = !string.IsNullOrWhiteSpace(prefix)
					? prefix + name
					: name;

				var stringValue = _settingsProvider.Get(settingName);
				if (string.IsNullOrWhiteSpace(stringValue) && type.IsNonNullableValueType())
				{
					throw new InvalidOperationException($"Null value encountered for value type setting {name} (of type {type}).");
				}

				if (type == typeof(string))
				{
					Binder.Bind(target, targetType, name, stringValue);
					continue;
				}

				ISettingConverter converter = _converters.First(c => c.CanConvert(type));
				object convertedValue = converter.Convert(stringValue, type);
				Binder.Bind(target, targetType, name, convertedValue);
			}

			return target;
		}

		/// <summary>
		/// Gets required inner type description for friendly exception message.
		/// </summary>
		protected virtual string TypeDesc => "simple { get; set; } POCO with parameterless constructor";
	}
}
