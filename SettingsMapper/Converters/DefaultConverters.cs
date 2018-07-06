using System;
using System.Collections.Generic;

namespace SettingsMapper.Converters
{
    /// <summary>
    /// Helper for getting all default setting converters.
    /// </summary>
    public static class DefaultConverters
    {
        /// <summary>
        /// Gets all default setting converters.
        /// </summary>
        /// <param name="formatProvider">Format provider.</param>
        /// <returns>All default setting converters.</returns>
        public static ISettingConverter[] GetAll(IFormatProvider formatProvider = null)
        {
            var list = new List<ISettingConverter>
            {
                new ScalarConverter(formatProvider),
                new GuidConverter(),
                new DateTimeConverter(formatProvider),
                new TimeSpanConverter(),
                new EnumConverter(),
                new ByteArrayConverter()
            };

            list.Add(new NullableConverter(list.ToArray()));
            list.Add(new ArrayConverter(list.ToArray()));

            return list.ToArray();
        }
    }
}
