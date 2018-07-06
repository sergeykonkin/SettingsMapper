using System;

namespace SettingsMapper
{
    /// <summary>
    /// Static helper for AppSettings mapping.
    /// </summary>
    public static class AppSettings
    {
        private static readonly AppSettingsProvider _provider = new AppSettingsProvider();

        /// <summary>
        /// Creates new instance of <typeparamref name="TSettings"/> with properties filled with converted settings.
        /// </summary>
        /// <returns>Poco object instance.</returns>
        public static TSettings MapTo<TSettings>() where TSettings : class, new()
        {
            return new PocoSettingsBuilder<TSettings>(_provider).Create();
        }

        /// <summary>
        /// Creates new instance of settings with properties filled with converted settings.
        /// </summary>
        /// <returns>Poco object instance.</returns>
        public static object MapTo(Type type)
        {
            return new PocoSettingsBuilder(type, _provider).Create();
        }

        /// <summary>
        /// Maps converted settings to Static class properties.
        /// </summary>
        /// <param name="staticType">Static class type.</param>
        public static void MapToStatic(Type staticType)
        {
            new StaticSettingsBuilder(_provider).MapTo(staticType);
        }
    }
}
