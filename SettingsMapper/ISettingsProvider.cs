namespace SettingsMapper
{
    /// <summary>
    /// Provides raw settings' values.
    /// </summary>
    public interface ISettingsProvider
    {
        /// <summary>
        /// Gets setting value by specified setting name.
        /// </summary>
        /// <param name="name">Name of the setting.</param>
        /// <returns>Raw string value of setting.</returns>
        string Get(string name);
    }
}
