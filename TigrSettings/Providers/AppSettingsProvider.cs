namespace TigrSettings
{
    /// <summary>
    /// Provides raw settings' values using <see cref="System.Configuration.ConfigurationManager"/>.
    /// </summary>
    public class AppSettingsProvider : ISettingsProvider
    {
        /// <inheritdoc />
        public string Get(string name)
        {
            return System.Configuration.ConfigurationManager.AppSettings[name];
        }
    }
}
