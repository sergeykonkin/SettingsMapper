using System;

namespace SettingsMapper
{
    /// <summary>
    /// Converts raw string setting values to specified type.
    /// </summary>
    public interface ISettingConverter
    {
        /// <summary>
        /// Identifies whether this converter can convert string to specified type.
        /// </summary>
        /// <param name="type">Type convert to.</param>
        /// <returns>true - if this converter can convert string to type; false - otherwise.</returns>
        bool CanConvert(Type type);

        /// <summary>
        /// Converts string setting value to specified type.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="type">Type convert to.</param>
        /// <returns>Converted value.</returns>
        object Convert(string value, Type type);
    }
}
