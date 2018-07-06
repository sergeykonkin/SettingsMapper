using System;

namespace SettingsMapper.Attributes
{
    /// <summary>
    /// Specifies custom setting name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class NameAttribute : Attribute
    {
        internal string Value { get; }

        /// <summary>
        /// Initializes new instance of <see cref="NameAttribute"/>.
        /// </summary>
        /// <param name="value">The name of setting.</param>
        public NameAttribute(string value)
        {
            Value = value;
        }
    }
}
