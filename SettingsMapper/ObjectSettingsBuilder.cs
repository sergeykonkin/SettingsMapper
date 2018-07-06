using System;
using System.Globalization;

namespace SettingsMapper
{
    /// <summary>
    /// Provides functionality to map raw settings to objects.
    /// </summary>
    public class ObjectSettingsBuilder : SettingsBuilderBase
    {
        private readonly Type _settingsType;

        /// <summary>
        /// Initializes a new instance of <see cref="ObjectSettingsBuilder"/>.
        /// </summary>
        /// <param name="settingsType"></param>
        /// <param name="settingsProvider">Raw string settings provider.</param>
        /// <param name="converters">Set of additional converters.</param>
        public ObjectSettingsBuilder(
            Type settingsType,
            ISettingsProvider settingsProvider,
            params ISettingConverter[] converters)
            : this(settingsType, settingsProvider, CultureInfo.InvariantCulture, converters)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ObjectSettingsBuilder"/>.
        /// </summary>
        /// <param name="settingsType"></param>
        /// <param name="settingsProvider">Raw string settings provider.</param>
        /// <param name="formatProvider">Format provider for e.g. numbers and dates.</param>
        /// <param name="converters">Set of additional converters.</param>
        public ObjectSettingsBuilder(
            Type settingsType,
            ISettingsProvider settingsProvider,
            IFormatProvider formatProvider,
            params ISettingConverter[] converters)
            : base(settingsProvider, formatProvider, converters)
        {
            _settingsType = settingsType;
        }

        /// <summary>
        /// Creates new instance of settings with properties filled with converted settings.
        /// </summary>
        /// <returns>Poco object instance.</returns>
        public object Create()
        {
            return base.Build(_settingsType) ?? Activator.CreateInstance(_settingsType);
        }

        internal override IMapper Mapper { get; } = new ObjectMapper();
    }
}
