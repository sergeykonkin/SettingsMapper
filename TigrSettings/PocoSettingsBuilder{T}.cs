using System;
using System.Globalization;

namespace TigrSettings
{
    /// <summary>
    /// Provides functionality to bind raw settings to Poco objects.
    /// </summary>
    public class PocoSettingsBuilder<TSettings> : SettingsBuilderBase
        where TSettings : class, new()
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PocoSettingsBuilder{TSettings}"/>.
        /// </summary>
        /// <param name="settingsProvider">Raw string settings provider.</param>
        /// <param name="converters">Set of additional converters.</param>
        public PocoSettingsBuilder(
            ISettingsProvider settingsProvider,
            params ISettingConverter[] converters)
            : this(settingsProvider, CultureInfo.InvariantCulture, converters)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PocoSettingsBuilder{TSettings}"/>.
        /// </summary>
        /// <param name="settingsProvider">Raw string settings provider.</param>
        /// <param name="formatProvider">Format provider for e.g. numbers and dates.</param>
        /// <param name="converters">Set of additional converters.</param>
        public PocoSettingsBuilder(
            ISettingsProvider settingsProvider,
            IFormatProvider formatProvider,
            params ISettingConverter[] converters)
            : base(settingsProvider, formatProvider, converters)
        {
        }

        /// <summary>
        /// Creates new instance of <typeparamref name="TSettings"/> with properties filled with converted settings.
        /// </summary>
        /// <returns>Poco object instance.</returns>
        public TSettings Create()
        {
            return (TSettings) base.Build(typeof(TSettings)) ?? new TSettings();
        }

        internal override IBinder Binder { get; } = new PocoBinder();
    }
}
