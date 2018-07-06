using System;

namespace SettingsMapper.Attributes
{
    /// <summary>
    /// Specifies the default setting value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DefaultAttribute : Attribute
    {
        internal object Value { get; }

        /// <summary>
        /// Initializes new instance of <see cref="DefaultAttribute"/>.
        /// </summary>
        /// <param name="value">The default setting value.</param>
        public DefaultAttribute(object value)
        {
            Value = value;
        }
    }
}
