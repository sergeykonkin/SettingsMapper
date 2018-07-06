using System;
using System.Linq;

namespace SettingsMapper.Converters
{
    /// <summary>
    /// Converts raw string setting values to specified <see cref="Enum"/> type.
    /// </summary>
    public class EnumConverter : ISettingConverter
    {
        /// <inheritdoc />
        public virtual bool CanConvert(Type type)
        {
            return type.IsEnum;
        }

        /// <inheritdoc />
        public virtual object Convert(string value, Type type)
        {
            if (!value.Contains("|"))
                return Enum.Parse(type, value);

            var flags = value
                .Split('|')
                .Select(str => str.Trim())
                .Select(str => Enum.Parse(type, str));

            var resultValue = flags
                .Cast<int>()
                .Aggregate(0, (acc, cur) => acc | cur);

            return Enum.ToObject(type, resultValue);
        }
    }
}
