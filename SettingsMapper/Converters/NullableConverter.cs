using System;
using System.Collections.Generic;
using System.Linq;

namespace SettingsMapper.Converters
{
    /// <summary>
    /// Converts raw string setting values to <see cref="Nullable{T}"/> type.
    /// </summary>
    public class NullableConverter : ISettingConverter
    {
        private readonly IEnumerable<ISettingConverter> _underlyingTypeConverters;

        /// <summary>
        /// Initializes new instance of <see cref="NullableConverter"/>.
        /// </summary>
        /// <param name="underlyingTypeConverters">Set of converters for underlying type.</param>
        public NullableConverter(IEnumerable<ISettingConverter> underlyingTypeConverters)
        {
            _underlyingTypeConverters = underlyingTypeConverters;
        }

        /// <inheritdoc />
        public virtual bool CanConvert(Type type)
        {
            var isNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
            if (!isNullable)
                return false;

            var underlyingType = type.GetGenericArguments()[0];
            return _underlyingTypeConverters.Any(c => c.CanConvert(underlyingType));
        }

        /// <inheritdoc />
        public virtual object Convert(string value, Type type)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Equals("null", StringComparison.InvariantCultureIgnoreCase))
                return null;

            Type underlyingType = type.GetGenericArguments()[0];
            var converter = _underlyingTypeConverters.First(c => c.CanConvert(underlyingType));
            return converter.Convert(value, underlyingType);
        }
    }
}
