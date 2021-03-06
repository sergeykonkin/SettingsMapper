﻿using System;
using System.Collections.Generic;
using System.Linq;
using SettingsMapper.Converters;

namespace SettingsMapper
{
    /// <summary>
    /// Provides base functionality to map raw settings to target types.
    /// </summary>
    public abstract class SettingsBuilderBase
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly List<ISettingConverter> _converters;

        public string PrefixDelimiter { get; set; } = ":";

        /// <summary>
        /// Gets mapper for settings-to-type conversion.
        /// </summary>
        internal abstract IMapper Mapper { get; }

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
            _converters.AddRange(DefaultConverters.GetAll(formatProvider));
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
            var target = Mapper.CreateTarget(targetType);
            var props = Mapper.GetProps(targetType).Where(p => !p.IsIgnored).ToList();

            foreach (var prop in props)
            {
                Type type = prop.Type;

                if (type != typeof(string) && !CanConvert(type))
                {
                    try
                    {
                        string newPrefix = (prefix ?? "") + (prop.CustomPrefix ?? prop.SettingName) + PrefixDelimiter;
                        object inner = Build(type, newPrefix);
                        Mapper.Map(target, targetType, prop.PropName, inner);
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
                    ? prefix + prop.SettingName
                    : prop.SettingName;

                var stringValue = _settingsProvider.Get(settingName);
                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    if (type.IsNonNullableValueType() && !prop.HasDefaultValue)
                    {
                        throw new InvalidOperationException(
                            $"Null value encountered for value type setting {prop.PropName} (of type {type}).");
                    }

                    if (prop.HasDefaultValue)
                    {
                        Mapper.Map(target, targetType, prop.PropName, prop.DefaultValue);
                        continue;
                    }
                }

                if (type == typeof(string))
                {
                    Mapper.Map(target, targetType, prop.PropName, stringValue);
                    continue;
                }

                ISettingConverter converter = _converters.First(c => c.CanConvert(type));
                object convertedValue = converter.Convert(stringValue, type);
                Mapper.Map(target, targetType, prop.PropName, convertedValue);
            }

            return target;
        }

        /// <summary>
        /// Gets required inner type description for exception message.
        /// </summary>
        protected virtual string TypeDesc => "simple { get; set; } POCO with parameterless constructor";
    }
}
