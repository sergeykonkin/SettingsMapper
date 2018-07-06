using System;
using System.Collections.Generic;

namespace SettingsMapper.Converters
{
    public static class DefaultConverters
    {
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
